using MatAppWebAPI2.Base;
using MatAppWebAPI2.Data;
using MatAppWebAPI2.DTOs;
using MatAppWebAPI2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace MatAppWebAPI2.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthController : CustomControllerBase
	{
		private readonly AuthService _authService;
		private readonly DatabaseContext _databaseContext;

		public AuthController(AuthService authService, DatabaseContext databaseContext)
		{
			_authService = authService;
			_databaseContext = databaseContext;
		}

		[HttpPost("login")]
		public async Task<IActionResult> Login([FromBody] LoginModel login)
		{
			var user = await _databaseContext.Users.FirstOrDefaultAsync(x => x.Name == login.Username);

			if(user is null)
			{
				return BadRequest(new
				{
					Message = "Name does not exist"
				});
			}
			if (user.Password != login.Password)
			{
				return BadRequest(new
				{
					Message = "Wrong password"
				});
			}

			var token = _authService.GetToken(user);
			return Ok(token);
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterModel register)
		{
			var exists = await _databaseContext.Users.AnyAsync(x => x.Name == register.Username);
			if (exists)
			{
				return BadRequest(new
				{
					Message = "Name already exists"
				});
			}
			User user = new User()
			{
				//Id = Guid.NewGuid(),
				Email = "",
				Name = register.Username,
				Password = register.Password,
				Type = register.Type,
				Description = register.Description,
			};
			_databaseContext.Users.Add(user);
			await _databaseContext.SaveChangesAsync();
			return Ok(user);
		}

		/*
		//[Authorize]
		[HttpGet("kalleanka")]
		public async Task<IActionResult> Get()
		{
			
			var prod = await _databaseContext.Products.Include(x => x.OrderProducts)!.ThenInclude(x => x.Order).FirstOrDefaultAsync(x => x.Id == Guid.Parse("5A6B645C-289A-4846-9061-13FB8F806DC9"));
			var y = 0;
			return Ok(prod);
		}
		*/
	}
}
