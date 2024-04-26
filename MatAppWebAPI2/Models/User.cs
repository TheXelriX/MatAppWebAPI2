using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatAppWebAPI2.Models
{
	public class User
	{
		public Guid Id { get; set; }
		public required string Name { get; set; }
		public required string Email { get; set; }
		public string? PhoneNumber {  get; set; }
		public string? Description { get; set; }
		public required string Password { get; set; }
		public List<Order>? UserCustomerOrders { get; set; }
		public List<Order>? UserRestaurantOrders { get; set; }
		public UserType Type { get; set; }
    }

	public enum UserType {
		Customer,
		Resaurant
	}

}
