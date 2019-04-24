namespace Server.Services
{
	using Microsoft.Extensions.Configuration;
	using System;
	using System.Collections.Generic;


	public interface IUsersService
	{
		bool Authenticate(string username, string password);
	}

	public class UsersService : IUsersService
	{
		private readonly IConfiguration _configuration;
		private readonly string _universalPassword;
		private const string UNIVERSAL_PASSWORD_KEY_NAME = "UniversalPassword";
		private readonly IList<Models.User> _users;
		private readonly string _usersDataFile;
		private const string USERS_DATA_FILE_KEY_NAME = "UsersDataFile";

		public bool Authenticate(string username, string password)
		{
			if (_users != null)
			{
				foreach (Models.User user in _users)
				{
					if ((String.Compare(user.Username, username, true) == 0) && (String.Compare(_universalPassword, password, false) == 0))
					{
						return true;
					}
				}
			}

			return false;
		}

		public UsersService(IConfiguration configuration)
		{
			_configuration = configuration;

			if (_configuration != null)
			{
				_universalPassword = _configuration[UNIVERSAL_PASSWORD_KEY_NAME];
				if (String.IsNullOrEmpty(_universalPassword))
				{
					throw new Exception("Required application setting \"" + UNIVERSAL_PASSWORD_KEY_NAME + "\" was not found.");
				}
				_usersDataFile = _configuration[USERS_DATA_FILE_KEY_NAME];
				if (String.IsNullOrEmpty(_usersDataFile))
				{
					throw new Exception("Required application setting \"" + USERS_DATA_FILE_KEY_NAME + "\" was not found.");
				}
				_users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.User>>(System.IO.File.ReadAllText(_usersDataFile));
			}
		}
	}
}
