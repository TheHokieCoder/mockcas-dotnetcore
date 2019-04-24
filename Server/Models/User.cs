namespace Server.Models
{
	using System.Collections.Generic;


	/// <summary>
	///		A user of the mock CAS system that is able to authenticate.
	/// </summary>
	public class User
	{
		/// <summary>
		///		The username (net ID) of the user
		/// </summary>
		[Newtonsoft.Json.JsonProperty("user")]
		public string Username;
		/// <summary>
		///		A collection of additional attributes known about the user
		/// </summary>
		[Newtonsoft.Json.JsonProperty("attributes")]
		public IDictionary<string, IList<string>> Attributes;
	}
}
