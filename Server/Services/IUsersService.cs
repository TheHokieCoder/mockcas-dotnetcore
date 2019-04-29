namespace Server.Services
{
	/// <summary>
	///		An interface for implementing a service that manages the users that are able to authenticate with the CAS server.
	/// </summary>
	public interface IUsersService
	{
		/// <summary>
		///		Determines the authenticity of the user based on the specified credentials.
		/// </summary>
		/// <param name="username">
		///		The username of the user
		/// </param>
		/// <param name="password">
		///		The password of the user
		/// </param>
		/// <returns>
		///		A Boolean indicating if authentication was successful
		/// </returns>
		bool Authenticate(string username, string password);

		/// <summary>
		///		Retrieves the user with the specified username from the collection.
		/// </summary>
		/// <param name="username">
		///		The username of the user
		/// </param>
		/// <returns>
		///		A <see cref="Models.User"/> object containing all properties of the user
		/// </returns>
		Models.User GetUser(string username);
	}
}
