using MatAppWebAPI2.Base;
using MatAppWebAPI2.Data;
using MatAppWebAPI2.DTOs;
using MatAppWebAPI2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MatAppWebAPI2.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/customer")]
	public class CustomerController : CustomControllerBase
	{
		private readonly DatabaseContext _databaseContext;

		public CustomerController(DatabaseContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		[HttpGet("restaurants")]
		public async Task<ActionResult<List<User>>> Restaurants()
		{
			if (Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Users.Where(x => x.Type == UserType.Resaurant).ToListAsync();
		}

		[HttpGet("restaurants/{id}")]
		public async Task<ActionResult<List<User>>> Restaurants(Guid id)
		{
			if (Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}

			var user = await _databaseContext.Users.Include(x => x.Products).FirstOrDefaultAsync(x => x.Id == id);

			if (user is null)
			{
				return BadRequest(new
				{
					Message = "Restaurant with id " + id + " not found"
				});
			}
			return Ok(user);

			//return await _databaseContext.Users.Where(x => x.Type == UserType.Resaurant).ToListAsync();
		}

		[HttpGet("history")]
		public async Task<ActionResult<List<Order>>> HistoryOrders()
		{
			if (Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Orders.Where(x => x.UserCustomerId == UserId && !x.IsActive).ToListAsync();
		}

		[HttpGet("active")]
		public async Task<ActionResult<List<Order>>> ActiveOrders()
		{
			if (Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Orders.Where(x => x.UserCustomerId == UserId && x.IsActive).ToListAsync();
		}


		[HttpPost("order")]
		public async Task<IActionResult> Order([FromBody] OrderModel ordermodel)
		{
			if (Type != UserType.Customer)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			if (ordermodel.Products is null)
			{
				return BadRequest(new
				{
					Message = "No Products ordered"
				});
			}

			Guid x = Guid.NewGuid();
			Order order = new Order
			{
				Id = x,
				UserCustomerId = UserId,
				UserRestaurantId = ordermodel.UserRestaurantId,
				Products = new(),
				IsActive = true,

			};

			foreach (Guid id in ordermodel.Products)
			{
				Product? prod = await _databaseContext.Products.FirstOrDefaultAsync(x => x.Id == id);
				if (prod is null)
				{
					return BadRequest(new
					{
						Message = "Product does not exist"
					});
				}
				order.Products.Add(new OrderProduct
				{
					Id = Guid.NewGuid(),
					OrderId = x,
					ProductId = id
				});
				order.Price += prod.Price;
			}

			_databaseContext.Orders.Add(order);
			await _databaseContext.SaveChangesAsync();

			return Ok(order.Id);
		}
	}
}
