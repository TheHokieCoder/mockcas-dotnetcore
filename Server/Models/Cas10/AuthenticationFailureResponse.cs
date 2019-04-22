namespace Server.Models.Cas10
{
	public class AuthenticationFailureResponse
	{
		private const string _failureMessage = "no";

		public override string ToString()
		{
			return _failureMessage + "\n\n";
		}
	}
}
