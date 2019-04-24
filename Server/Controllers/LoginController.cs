namespace Server.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Server.Models.Login;
	using Server.Services;
	using System;
	using System.Web;


	public class LoginController : Controller
    {
		private const string SERVICE_TICKET_PREFIX = "ST-";
		private ITicketService _ticketService;
		private IUsersService _usersService;

		[HttpGet]
		[Route("cas/login")]
        public IActionResult Index([FromQuery]string service)
        {
			LoginModel model = new LoginModel();

			if (!String.IsNullOrEmpty(service))
			{
				model.Service = HttpUtility.UrlEncode(service);
			}

			return View(model);
        }

		[HttpPost]
		[Route("cas/login")]
		public IActionResult Index(LoginModel model)
		{
			if(ModelState.IsValid)
			{
				if (_usersService.Authenticate(model.Username, model.Password))
				{
					if (String.IsNullOrEmpty(model.Service))
					{
						model.SuccessMessage = "Authenticated!";
					}
					else
					{
						if (_ticketService != null)
						{
							string newServiceTicket = SERVICE_TICKET_PREFIX + Guid.NewGuid().ToString().ToUpper();
							_ticketService.InsertTicket(newServiceTicket, model.Username);
							return Redirect(model.Service + "?ticket=" + newServiceTicket);
						}
						else
						{
							throw new InvalidOperationException("FAIL");
						}
					}
				}
				else
				{
					model.ErrorMessage = "Authentication failed!";
				}
			}
			else
			{
				model.ErrorMessage = "Model is not valid!";
			}

			return View(model);
		}

		public LoginController(ITicketService ticketService, IUsersService usersService)
		{
			_ticketService = ticketService;
			_usersService = usersService;
		}
    }
}