namespace Server.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Server.Models.Login;
	using Server.Services;
	using System;
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
	}
}