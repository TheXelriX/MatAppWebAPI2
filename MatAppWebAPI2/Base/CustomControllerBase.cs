using MatAppWebAPI2.Models;
using Microsoft.AspNetCore.Mvc;

namespace MatAppWebAPI2.Base
{
	public class CustomControllerBase : ControllerBase
	{
		public Guid UserId
		{
			get
			{
				string? id = HttpContext.User.FindFirst("userid")?.Value;
				return id is null ? throw new InvalidOperationException() : Guid.Parse(id);
			}
		}
		public UserType Type
		{
			get
			{
				string? type = HttpContext.User.FindFirst("usertype")?.Value;
				return type is null ? throw new InvalidOperationException() : (UserType)Enum.Parse(typeof(UserType), type);
			}
		}
	}
}
