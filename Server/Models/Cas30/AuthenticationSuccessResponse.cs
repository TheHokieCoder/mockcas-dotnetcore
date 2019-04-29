namespace Server.Models.Cas30
{
	using System;
	using System.Collections.Generic;
	using System.Text;


	/// <summary>
	///		An object representing the CAS v3.0 response sent when authentication succeeds.
	/// </summary>
	public class AuthenticationSuccessResponse
	{
		private IDictionary<string, IList<string>> _attributes;
		private string _username;

		/// <summary>
		///		Default constructor that sets the identity of the authenticated user.
		/// </summary>
		/// <param name="username">
		///		The identity of the authenticated user
		/// </param>
		public AuthenticationSuccessResponse(string username, IDictionary<string, IList<string>> attributes)
		{
			_attributes = attributes;
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
			StringBuilder attributesJSON = new StringBuilder();
			if (_attributes != null && _attributes.Count > 0)
			{
				attributesJSON.Append(",\"attributes\":{");
				bool firstAttribute = true;

				foreach(string key in _attributes.Keys)
				{
					if (!firstAttribute)
					{
						attributesJSON.Append(",");
					}
					else
					{
						firstAttribute = false;
					}

					attributesJSON.Append(Newtonsoft.Json.JsonConvert.ToString(key) + ":[");
					bool firstItem = true;

					foreach (string item in _attributes[key])
					{
						if (!firstItem)
						{
							attributesJSON.Append(",");
						}
						else
						{
							firstItem = false;
						}

						attributesJSON.Append(Newtonsoft.Json.JsonConvert.ToString(item));
					}

					attributesJSON.Append("]");
				}

				attributesJSON.Append("}");
			}

			return
				"{" +
					"\"serviceResponse\":{" +
						"\"authenticationSuccess\":{" +
							"\"user\":" + Newtonsoft.Json.JsonConvert.ToString(_username) +
							attributesJSON +
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
