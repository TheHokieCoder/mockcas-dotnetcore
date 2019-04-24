namespace Server.Models.Login
{
	using System.ComponentModel.DataAnnotations;

	public class LoginModel
	{
		public string ErrorMessage { get; set; }

		public bool HasError
		{
			get
			{
				return !string.IsNullOrEmpty(ErrorMessage);
			}
		}

		public bool HasSuccess
		{
			get
			{
				return !string.IsNullOrEmpty(SuccessMessage);
			}
		}

		[Required]
		public string Password
		{
			get;
			set;
		}

		public string Service
		{
			get;
			set;
		}

		public string SuccessMessage { get; set; }

		[Required]
		public string Username
		{
			get;
			set;
		}
	}
}
