namespace Server.Models.Cas10
{
	/// <summary>
	///		An object representing the CAS v1.0 response sent when authentication succeeds.
	/// </summary>
	public class AuthenticationSuccessResponse
	{
		private const string _successMessage = "yes";
		private string _username;

		/// <summary>
		///		Default constructor that sets the identity of the successfully authenticated user.
		/// </summary>
		/// <param name="username">
		///		The identity of the authenticated user
		/// </param>
		public AuthenticationSuccessResponse(string username)
		{
			_username = username;
		}

		/// <summary>
		///		Generates a string representing the CAS v1.0 response.
		/// </summary>
		/// <returns>
		///		A string
		/// </returns>
		public override string ToString()
		{
			return _successMessage + "\n" + _username + "\n";
		}
	}
}
