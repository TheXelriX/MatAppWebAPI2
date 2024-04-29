namespace MatAppWebAPI2.DTOs
{
	public class OrderModel
	{
		public Guid UserRestaurantId { get; set; }
		public List<Guid>? Products { get; set; }
	}
}
