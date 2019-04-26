namespace Server.Models.Cas10
{
	/// <summary>
	///		An object representing the CAS v1.0 response sent when authentication fails.
	/// </summary>
	public class AuthenticationFailureResponse
	{
		private const string _failureMessage = "no";

		/// <summary>
		///		Generates a string representing the CAS v1.0 response.
		/// </summary>
		/// <returns>
		///		A string
		/// </returns>
		public override string ToString()
		{
			return _failureMessage + "\n\n";
		}
	}
}
