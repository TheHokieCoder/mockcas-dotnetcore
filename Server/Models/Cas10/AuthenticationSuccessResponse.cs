namespace Server.Models.Cas10
{
	public class AuthenticationSuccessResponse
	{
		private const string _successMessage = "yes";

		public AuthenticationSuccessResponse(string username)
		{
			Username = username;
		}

		public string Username
		{
			get;
		}

		public override string ToString()
		{
			return _successMessage + "\n" + Username + "\n";
		}
	}
}
