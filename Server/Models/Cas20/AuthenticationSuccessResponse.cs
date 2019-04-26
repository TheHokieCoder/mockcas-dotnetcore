namespace Server.Models.Cas20
{
	/// <summary>
	///		An object representing the CAS v2.0 response sent when authentication succeeds.
	/// </summary>
	public class AuthenticationSuccessResponse
	{
		private string _username;

		/// <summary>
		///		Default constructor that sets the identity of the authenticated user.
		/// </summary>
		/// <param name="username">
		///		The identity of the authenticated user
		/// </param>
		public AuthenticationSuccessResponse(string username)
		{
			_username = username;
		}

		/// <summary>
		///		Generates a JSON object string representing the CAS v2.0 authentication success response.
		/// </summary>
		/// <returns>
		///		A JSON object in string form
		/// </returns>
		public string ToJSON()
		{
			return
				"{" +
					"\"serviceResponse\":{" +
						"\"authenticationSuccess\":{" +
							"\"user\":" + Newtonsoft.Json.JsonConvert.ToString(_username) +
						"}" +
					"}" +
				"}";
		}

		/// <summary>
		///		Generates a XML object string representing the CAS v2.0 authentication success response.
		/// </summary>
		/// <returns>
		///		A XML object in string form
		/// </returns>
		public string ToXML()
		{
			return
				"<cas:serviceResponse xmlns:cas=\"http://www.yale.edu/tp/cas\">" +
					"<cas:authenticationSuccess>" +
						"<cas:user>" + _username + "</cas:user>" +
					"</cas:authenticationSuccess>" +
				"</cas:serviceResponse>";
		}
	}
}
