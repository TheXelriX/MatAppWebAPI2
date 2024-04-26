using MatAppWebAPI2.Base;
using MatAppWebAPI2.Data;
using MatAppWebAPI2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatAppWebAPI2.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/restaurants")]
	public class RestaurantsController : CustomControllerBase
	{
		private readonly DatabaseContext _databaseContext;

		public RestaurantsController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		
		[HttpGet("restaurants")]
		public async Task<ActionResult<List<User>>> Restaurants()
		{
			if(Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Users.Where(x => x.Type == UserType.Resaurant).ToListAsync();
		}
	}
}
