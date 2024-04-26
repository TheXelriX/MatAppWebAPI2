using System.ComponentModel.DataAnnotations;

namespace MatAppWebAPI2.Models
{
	public class Order
	{
		public Guid Id { get; set; }

		public Guid UserCustomerId { get; set; }
		public User? UserCustomer { get; set; }

		public Guid UserRestaurantId { get; set; }
		public User? UserRestaurant { get; set; }

		public List<OrderProduct>? Products { get; set; }
		public decimal Price { get; set; }
		public bool IsActive { get; set; }
	}
}
