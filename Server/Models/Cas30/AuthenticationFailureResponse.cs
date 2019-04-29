namespace Server.Models.Cas30
{
	/// <summary>
	///		An enumeration of possible authentication failure codes that may be returned by the CAS v3.0 service ticket validation endpoint.
	/// </summary>
	/// <remarks>
	///		See section 2.5.3 of the CAS protocol specification for more details:
	///		https://github.com/apereo/cas/blob/master/docs/cas-server-documentation/protocol/CAS-Protocol-Specification.md#253-error-codes
	/// </remarks>
	public enum AuthenticationFailureCode
	{
		/// <summary>
		///		An internal error occurred during ticket validation
		/// </summary>
		INTERNAL_ERROR,
		/// <summary>
		///		The proxy callback specified is invalid. The credentials specified for proxy authentication do not meet the security requirements
		/// </summary>
		INVALID_PROXY_CALLBACK,
		/// <summary>
		///		Not all of the required request parameters were present
		/// </summary>
		INVALID_REQUEST,
		/// <summary>
		///		The ticket provided was valid, but the service specified did not match the service associated with the ticket
		/// </summary>
		INVALID_SERVICE,
		/// <summary>
		///		The ticket provided was not valid
		/// </summary>
		INVALID_TICKET,
		/// <summary>
		///		Failure to meet the requirements of validation specification
		/// </summary>
		INVALID_TICKET_SPEC,
		/// <summary>
		///		The proxy callback specified is invalid. The credentials specified for proxy authentication do not meet the security requirements
		/// </summary>
		UNAUTHORIZED_SERVICE_PROXY
	}

	/// <summary>
	///		An object representing the CAS v3.0 response sent when authentication fails.
	/// </summary>
	public class AuthenticationFailureResponse
	{
		private AuthenticationFailureCode _failureCode;
		private string _failureDescription;

		/// <summary>
		///		Default constructor that sets the failure code and description.
		/// </summary>
		/// <param name="code">
		///		The value from the <see cref="AuthenticationFailureCode"/> enumeration that represents the error encountered during authentication
		/// </param>
		/// <param name="description">
		///		A string providing more detail about the error encountered
		/// </param>
		public AuthenticationFailureResponse(AuthenticationFailureCode code, string description)
		{
			_failureCode = code;
			_failureDescription = description;
		}

		/// <summary>
		///		Generates a JSON object string representing the CAS v3.0 authentication failure response.
		/// </summary>
		/// <returns>
		///		A JSON object in string form
		/// </returns>
		public string ToJSON()
		{
			return
				"{" +
					"\"serviceResponse\":{" +
						"\"authenticationFailure\":{" +
							"\"code\":" + Newtonsoft.Json.JsonConvert.ToString(_failureCode.ToString()) + "," +
							"\"description\":" + Newtonsoft.Json.JsonConvert.ToString(_failureDescription) +
						"}" +
					"}" +
				"}";
		}

		/// <summary>
		///		Generates a XML object string representing the CAS v3.0 authentication failure response.
		/// </summary>
		/// <returns>
		///		A XML object in string form
		/// </returns>
		public string ToXML()
		{
			return
				"<cas:serviceResponse xmlns:cas=\"http://www.yale.edu/tp/cas\">" +
					"<cas:authenticationFailure code=\"" + _failureCode.ToString() + "\">" +
						_failureDescription +
					"</cas:authenticationFailure>" +
				"</cas:serviceResponse>";
		}
	}
}
