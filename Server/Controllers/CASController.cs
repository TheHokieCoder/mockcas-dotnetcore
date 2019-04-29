namespace Server.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Server.Models.Login;
	using Server.Services;
	using System;
	using System.Text;
	using System.Web;


	/// <summary>
	///		The main controller for all CAS server actions.
	/// </summary>
	[Route("cas")]
	public class CasController : Controller
    {
		/// <summary>
		///		The prefix required for service ticket identifiers by the CAS protocol specification.
		/// </summary>
		private const string SERVICE_TICKET_PREFIX = "ST-";
		/// <summary>
		///		The CAS server's ticket service
		/// </summary>
		private ITicketService _ticketService;
		/// <summary>
		///		The CAS server's user service
		/// </summary>
		private IUsersService _usersService;

		/// <summary>
		///		The service ticket validation endpoint for v1.0 of the CAS protocol.
		/// </summary>
		/// <param name="service">
		///		The identifier of the service for which the ticket was issued
		/// </param>
		/// <param name="ticket">
		///		The service ticket issued by the "/login" endpoint
		/// </param>
		/// <returns>
		///		A plain-text response representing the success or failure of the service ticket validation
		/// </returns>
		/// <remarks>
		///		This endpoint of the mock CAS server does not support the "renew" parameter as single sign-on (SSO) is not supported. Therefore, all
		///		service tickets will have been generated from the presentation of credentials and never from a SSO session.
		///	</remarks>
		[HttpGet]
		[Route("validate")]
		public string Cas10Validate([FromQuery]string service, [FromQuery]string ticket)
		{
			if (String.IsNullOrEmpty(service))
			{
				// No "service" parameter was specified, which is required, so the service ticket cannot be validated
				return new Models.Cas10.AuthenticationFailureResponse().ToString();
			}

			if (String.IsNullOrEmpty(ticket))
			{
				// No "ticket" parameter was specified, which is required, so the service ticket cannot be validated
				return new Models.Cas10.AuthenticationFailureResponse().ToString();
			}

			return new Models.Cas10.AuthenticationSuccessResponse("blallen1").ToString();
		}

		/// <summary>
		///		Default constructor for the <see cref="CasController"/> class that receives services via dependency injection from the application's
		///		service provider.
		/// </summary>
		/// <param name="ticketService">
		///		The CAS server's ticket service that manages the storage of generated service tickets
		/// </param>
		/// <param name="usersService">
		///		The CAS server's user service that manages known identities
		/// </param>
		public CasController(ITicketService ticketService, IUsersService usersService)
		{
			_ticketService = ticketService;
			_usersService = usersService;
		}

		/// <summary>
		///		Razor view page for requesting credentials for authentication via an HTML form.
		/// </summary>
		/// <param name="service">
		///		The identifier of the service for which the ticket was issued (optional)
		/// </param>
		/// <returns>
		///		A Razor view page
		/// </returns>
		[HttpGet]
		[Route("login")]
		public IActionResult Login([FromQuery]string service)
		{
			LoginModel model = new LoginModel();

			if (!String.IsNullOrEmpty(service))
			{
				model.Service = HttpUtility.UrlEncode(service);
			}

			return View(model);
		}

		/// <summary>
		///		The controller action that performs form validation and user credential authentication.
		/// </summary>
		/// <param name="model">
		///		The login form data model
		/// </param>
		/// <returns>
		///		An action result based on the result of form validation, and if necessary, credential authentication
		/// </returns>
		[HttpPost]
		[Route("login")]
		public IActionResult Login(LoginModel model)
		{
			if (ModelState.IsValid)
			{
				if (_usersService.Authenticate(model.Username, model.Password))
				{
					if (String.IsNullOrEmpty(model.Service))
					{
						// The credential authentication was successful, but no service identifier was provided, so the only thing that can be done
						// is to confirm the authentication
						model.SuccessMessage = "Authenticated!";
					}
					else
					{
						if (_ticketService != null)
						{
							// The credential authentication was successful, so generate a new service ticket for the authenticated user
							string newServiceTicket = SERVICE_TICKET_PREFIX + Guid.NewGuid().ToString().ToUpper();
							// Store the service ticket with the ticket service
							_ticketService.InsertTicket(newServiceTicket, model.Username);
							// Redirect the user to the service with the new service ticket
							return Redirect(model.Service + "?ticket=" + newServiceTicket);
						}
						else
						{
							// The credential authentication was successful, but no ticket service exists to create and store a service ticket for the
							// user, which is an unsupported condition
							throw new InvalidOperationException("User is authenticated, but no ticket service exists to generate a CAS service " +
								"ticket.");
						}
					}
				}
				else
				{
					// The credentials are not valid, so present an error message
					model.ErrorMessage = "Authentication failed!";
				}
			}
			else
			{
				// The model (form data) is not valid, so present an error message
				model.ErrorMessage = "Model is not valid!";
			}

			return View(model);
		}

		/// <summary>
		///		Razor view page for logging a user out of the single sign-on (SSO) session with the CAS server.
		/// </summary>
		/// <returns>
		///		A Razor view page
		/// </returns>
		/// <remarks>
		///		Because this mock CAS server does not support SSO, this endpoint essentially has no action, hence the message in the Razor view page.
		/// </remarks>
		[HttpGet]
		[Route("logout")]
		public IActionResult Logout([FromQuery]string service, [FromQuery]string url)
		{	
			if (!String.IsNullOrEmpty(url))
			{
				ViewBag.RedirectURL = url;
			}
			if (!String.IsNullOrEmpty(service))
			{
				ViewBag.RedirectURL = service;
			}
			return View();
		}

		/// <summary>
		///		The service ticket validation endpoint for v2.0 of the CAS protocol.
		/// </summary>
		/// <param name="format">
		///		The format of the validation response (JSON or XML)
		/// </param>
		/// <param name="service">
		///		The identifier of the service for which the ticket was issued
		/// </param>
		/// <param name="ticket">
		///		The service ticket issued by the "/login" endpoint
		/// </param>
		/// <returns>
		///		A response representing the success or failure of the service ticket validation in either JSON or XML format
		/// </returns>
		/// <remarks>
		///		The following are some important differences between the CAS v2.0 protocol specification and this server implementation:
		///		
		///		- The "renew" parameter is not supported as single sign-on (SSO) is not supported. Therefore, all service tickets will have been
		///		  generated from the presentation of credentials and never from a SSO session.
		///		- The "pgtUrl" parameter is not supported as proxies are not supported.
		///		- The "service" parameter is not verified against the service parameter that was supplied when the service ticket was generated. This
		///		  is a mock CAS server and that level of security would be overkill.
		///	</remarks>
		[HttpGet]
		[Route("serviceValidate")]
		public IActionResult Cas20Validate([FromQuery]string format, [FromQuery]string service, [FromQuery]string ticket)
		{
			if (String.IsNullOrEmpty(format))
			{
				// Default to the XML-formatted response per CAS protocol specification
				format = "xml";
			}
			else
			{
				// Force the specified format to all lowercase characters
				format = format.ToLower();
			}

			if (format != "json" && format != "xml")
			{
				// The requested format is not valid/supported, so return an error in the default XML format
				return Content(new Models.Cas20.AuthenticationFailureResponse(Models.Cas20.AuthenticationFailureCode.INVALID_REQUEST,
					"Specified response format is invalid.").ToJSON(), "application/json", Encoding.UTF8);
			}

			if (String.IsNullOrEmpty(service))
			{
				// The required parameter "service" was not specified, thus the request is invalid.
				Models.Cas20.AuthenticationFailureResponse failureResponse = new Models.Cas20.AuthenticationFailureResponse(
					Models.Cas20.AuthenticationFailureCode.INVALID_REQUEST, "Required parameter \"service\" was not specified.");
				if (format == "json")
				{
					// Return the error in the JSON format
					return Content(failureResponse.ToJSON(), "application/json", Encoding.UTF8);
				}
				else
				{
					// Return the error in the only other format, XML
					return Content(failureResponse.ToXML(), "application/xml", Encoding.UTF8);
				}
			}

			if (String.IsNullOrEmpty(ticket))
			{
				// The required parameter "ticket" was not specified, thus the request is invalid.
				Models.Cas20.AuthenticationFailureResponse failureResponse = new Models.Cas20.AuthenticationFailureResponse(
					Models.Cas20.AuthenticationFailureCode.INVALID_REQUEST, "Required parameter \"ticket\" was not specified.");
				if (format == "json")
				{
					// Return the error in the JSON format
					return Content(failureResponse.ToJSON(), "application/json", Encoding.UTF8);
				}
				else
				{
					// Return the error in the only other format, XML
					return Content(failureResponse.ToXML(), "application/xml", Encoding.UTF8);
				}
			}

			// All request validation has passed, so now validate the specified service ticket
			string username = _ticketService.GetTicket(ticket);
			if (String.IsNullOrEmpty(username))
			{
				// The specified service ticket does not exist in the ticket service cache, so it is invalid
				Models.Cas20.AuthenticationFailureResponse failureResponse = new Models.Cas20.AuthenticationFailureResponse(
					Models.Cas20.AuthenticationFailureCode.INVALID_TICKET, "Ticket " + ticket + " not recognized.");
				if (format == "json")
				{
					// Return the error in the JSON format
					return Content(failureResponse.ToJSON(), "application/json", Encoding.UTF8);
				}
				else
				{
					// Return the error in the only other format, XML
					return Content(failureResponse.ToXML(), "application/xml", Encoding.UTF8);
				}
			}

			// The identity linked to the service ticket is now known, so send an authentication success response
			Models.Cas20.AuthenticationSuccessResponse successResponse = new Models.Cas20.AuthenticationSuccessResponse(username);
			if (format == "json")
			{
				// Return the response in the JSON format
				return Content(successResponse.ToJSON(), "application/json", Encoding.UTF8);
			}
			else
			{
				// Return the error in the only other format, XML
				return Content(successResponse.ToXML(), "application/xml", Encoding.UTF8);
			}
		}
	}
}