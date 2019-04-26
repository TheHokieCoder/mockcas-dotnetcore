namespace Server.Models.Login
{
	using System.ComponentModel.DataAnnotations;

	/// <summary>
	///		The model used with the Login action of the CAS controller.
	/// </summary>
	public class LoginModel
	{
		/// <summary>
		///		A message describing the error that occurred, if any.
		/// </summary>
		public string ErrorMessage { get; set; }

		/// <summary>
		///		A utility property that determines if an error message has been set.
		/// </summary>
		public bool HasError
		{
			get
			{
				return !string.IsNullOrEmpty(ErrorMessage);
			}
		}

		/// <summary>
		///		A utilility property that determines if a success message has been set.
		/// </summary>
		public bool HasSuccess
		{
			get
			{
				return !string.IsNullOrEmpty(SuccessMessage);
			}
		}

		/// <summary>
		///		The password entered via the login form.
		/// </summary>
		[Required]
		public string Password
		{
			get;
			set;
		}

		/// <summary>
		///		The service identifier specified via the request query string.
		/// </summary>
		public string Service
		{
			get;
			set;
		}

		/// <summary>
		///		A message describing successful form submission, if applicable.
		/// </summary>
		public string SuccessMessage { get; set; }

		/// <summary>
		///		The username entered via the login form.
		/// </summary>
		[Required]
		public string Username
		{
			get;
			set;
		}
	}
}
