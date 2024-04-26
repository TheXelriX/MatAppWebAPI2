using MatAppWebAPI2.Models;

namespace MatAppWebAPI2.DTOs
{
	public class RegisterModel
	{
		public string Username { get; set; } = string.Empty;
		public string Password { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;
		public UserType Type { get; set; }
	}
}
