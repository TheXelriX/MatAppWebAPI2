using System.ComponentModel.DataAnnotations;

namespace MatAppWebAPI2.Models
{
	public class Product
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public string? Description { get; set; }
		public decimal Price { get; set; }
		public Guid UserRestaurantId { get; set; }
		public User? UserRestaurant { get; set; }
		public bool IsActive { get; set; }

		public List<OrderProduct>? OrderProducts { get; set; }
	}
}
