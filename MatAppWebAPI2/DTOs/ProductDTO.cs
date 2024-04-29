namespace MatAppWebAPI2.DTOs
{
	public class ProductDTO
	{
		public Guid Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		public decimal Price { get; set; }
		public Guid UserRestaurantId { get; set; }
	}
}
