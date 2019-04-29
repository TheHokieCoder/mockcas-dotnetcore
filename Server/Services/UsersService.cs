namespace Server.Services
{
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Collections.Generic;


	/// <summary>
	///		A user service implemented using a JSON data file containing user definitions.
	/// </summary>
	public class UsersService : IUsersService
	{
		private readonly IConfiguration _configuration;
		private readonly string _universalPassword;
		private const string UNIVERSAL_PASSWORD_KEY_NAME = "UniversalPassword";
		private readonly IList<Models.User> _users;
		private readonly string _usersDataFile;
		private const string USERS_DATA_FILE_KEY_NAME = "UsersDataFile";

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
		public bool Authenticate(string username, string password)
		{
			if (_users != null)
			{
				// Iterate over each user in the collection
				foreach (Models.User user in _users)
				{
					// Compare the usernames (case-insensitive) and the passwords (case-sensitive)
					if ((String.Compare(user.Username, username, true) == 0) && (String.Compare(_universalPassword, password, false) == 0))
					{
						// The username and password match, so the specified credentials are authentic
						return true;
					}
				}
			}

			// No users in the collection have matching credentials, so authentication failed
			return false;
		}

		/// <summary>
		///		Retrieves the user with the specified username from the collection.
		/// </summary>
		/// <param name="username">
		///		The username of the user
		/// </param>
		/// <returns>
		///		A <see cref="Models.User"/> object containing all properties of the user
		/// </returns>
		public Models.User GetUser(string username)
		{
			if (_users != null)
			{
				// Iterate over each user in the collection
				foreach (Models.User user in _users)
				{
					if (String.Compare(user.Username, username, true) == 0)
					{
						// The user has been found, so return the object to the caller
						return user;
					}
				}
			}

			// No users in the collection, so the user is not found
			return null;
		}

		/// <summary>
		///		Default constructor that receives an instance of the configuration provider via dependency injection
		/// </summary>
		/// <param name="configuration">
		///		An instance of the configuration provider
		/// </param>
		public UsersService(IConfiguration configuration)
		{
			_configuration = configuration;

			if (_configuration != null)
			{
				// Retrieve the universal password from the application configuration
				_universalPassword = _configuration[UNIVERSAL_PASSWORD_KEY_NAME];
				if (String.IsNullOrEmpty(_universalPassword))
				{
					throw new Exception("Required application setting \"" + UNIVERSAL_PASSWORD_KEY_NAME + "\" was not found.");
				}
				// Retrieve the location of the data file containing the user definitions
				_usersDataFile = _configuration[USERS_DATA_FILE_KEY_NAME];
				if (String.IsNullOrEmpty(_usersDataFile))
				{
					throw new Exception("Required application setting \"" + USERS_DATA_FILE_KEY_NAME + "\" was not found.");
				}
				// Deserialize the user data file from JSON into a list of User instances
				_users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.User>>(System.IO.File.ReadAllText(_usersDataFile));
			}
		}
	}
}
