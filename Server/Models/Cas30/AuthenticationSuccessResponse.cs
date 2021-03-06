﻿namespace Server.Models.Cas30
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
		/// <param name="attributes">
		///		A collection of attributes of the authenticated user
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
			string attributesJSON = String.Empty;
			if (_attributes != null)
			{
				// Attributes are available to convert to a JSON object string
				attributesJSON = ",\"attributes\":" + Newtonsoft.Json.JsonConvert.SerializeObject(_attributes);
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
			StringBuilder casAttributes = new StringBuilder();
			
			if (_attributes != null && _attributes.Count > 0)
			{
				// Attributes are available to convert to elements in the XML response
				casAttributes.Append("<cas:attributes>");

				// Iterate over each attribute, creating a separate XML element for each value of that attribute
				foreach (string key in _attributes.Keys)
				{
					foreach (string value in _attributes[key])
					{
						casAttributes.Append("<cas:" + key + ">" + value + "</cas:" + key + ">");
					}
				}

				casAttributes.Append("</cas:attributes>");
			}

			return
				"<cas:serviceResponse xmlns:cas=\"http://www.yale.edu/tp/cas\">" +
					"<cas:authenticationSuccess>" +
						"<cas:user>" + _username + "</cas:user>" +
						casAttributes.ToString() +
					"</cas:authenticationSuccess>" +
				"</cas:serviceResponse>";
		}
	}
}
