namespace Server.Controllers
{
	using Microsoft.AspNetCore.Mvc;
	using Server.Models.Cas10;
	using System;

	[ApiController]
	public class Cas10Controller : ControllerBase
	{
		// GET api/values
		[HttpGet]
		[Route("cas/validate")]
		public ActionResult<string> Validate([FromQuery]string renew, [FromQuery]string service, [FromQuery]string ticket)
		{
			// Assert that, if specified, the "renew" parameter evaluates to a Boolean value
			if (!Boolean.TryParse(renew, out bool requirePrimaryCredentials))
			{
				return new ActionResult<string>(new AuthenticationFailureResponse().ToString());
			}

			// Assert that the "service" parameter was specified
			if (String.IsNullOrEmpty(service))
			{
				return new ActionResult<string>(new AuthenticationFailureResponse().ToString());
			}

			// Assert that the "ticket" parameter was specified
			if (String.IsNullOrEmpty(service))
			{
				return new ActionResult<string>(new AuthenticationFailureResponse().ToString());
			}

			return new ActionResult<string>(new AuthenticationSuccessResponse("blallen1").ToString());
		}
	}
}
