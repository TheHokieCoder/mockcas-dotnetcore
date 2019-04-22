namespace Server.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Server.Models.Login;


	public class LoginController : Controller
    {
		[HttpGet]
		[Route("cas/login")]
        public IActionResult Index()
        {
			return View(new LoginModel());
        }

		[HttpPost]
		[Route("cas/login")]
		public IActionResult Index(LoginModel model)
		{
			if (model != null)
			{
				return View();
			}
			else
			{
				return View();
			}
		}
    }
}