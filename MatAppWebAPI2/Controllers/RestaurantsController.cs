using MatAppWebAPI2.Base;
using MatAppWebAPI2.Data;
using MatAppWebAPI2.DTOs;
using MatAppWebAPI2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Text.Json.Serialization;

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


		[HttpGet("products")]
		public async Task<ActionResult<List<Product>>> Products()
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			List<ProductDTO> products = new();
			List<Product> prodlist = await _databaseContext.Products.Where(x => x.UserRestaurantId == UserId).ToListAsync();
			foreach (var prod in prodlist)
			{
				products.Add(new ProductDTO
				{
					Id = prod.Id,
					Name = prod.Name,
					Description = prod.Description == null ? string.Empty : prod.Description,
					Price = prod.Price,
					UserRestaurantId = UserId,
				});
			}
			return Ok(products);
		}

		[HttpPost("product")]
		public async Task<IActionResult> AddProduct([FromBody] ProductModel product)
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			var prod = new Product
			{
				Name = product.Name,
				Description = product.Description,
				Price = product.Price,
				UserRestaurantId = UserId
			};
			_databaseContext.Products.Add(prod);
			await _databaseContext.SaveChangesAsync();


			return Ok(prod);
		}

		[HttpDelete("product")]
		public async Task<IActionResult> DeleteProduct([FromBody] Guid id)
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			var product = await _databaseContext.Products.FindAsync(id);
            if (product is null)
            {
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			product.IsActive = false;
			await _databaseContext.SaveChangesAsync();
			return Ok();
        }

		
		[HttpPut("ordercomplete")]
		public async Task<IActionResult> CompleteOrder([FromBody] Guid id)
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			var order = await _databaseContext.Orders.FirstOrDefaultAsync(x => x.Id == id);
			if (order is null)
			{
				return BadRequest(new
				{
					Message = "Order does not exist"
				});
			}
			order.IsActive = false;
			_databaseContext.Entry(order).State = EntityState.Modified;
			await _databaseContext.SaveChangesAsync();
			return Ok();
		}

		[HttpGet("history")]
		public async Task<ActionResult<List<Order>>> HistoryOrders()
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Orders.Where(x => x.UserRestaurantId == UserId && !x.IsActive).ToListAsync();
		}

		[HttpGet("active")]
		public async Task<ActionResult<List<Order>>> ActiveOrders()
		{
			if (Type != UserType.Resaurant)
			{
				return BadRequest(new
				{
					Message = "Not authorized"
				});
			}
			return await _databaseContext.Orders.Where(x => x.UserRestaurantId == UserId && x.IsActive).ToListAsync();
		}

	}

}