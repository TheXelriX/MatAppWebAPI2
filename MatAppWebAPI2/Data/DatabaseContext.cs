using MatAppWebAPI2.Models;
using Microsoft.EntityFrameworkCore;

namespace MatAppWebAPI2.Data
{
	public class DatabaseContext : DbContext
	{
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var x = modelBuilder.Entity<User>();
			x.HasMany(x => x.UserCustomerOrders)
				.WithOne(x => x.UserCustomer)
				.OnDelete(DeleteBehavior.NoAction);

			x.HasMany(x => x.UserRestaurantOrders)
				.WithOne(x => x.UserRestaurant)
				.OnDelete(DeleteBehavior.NoAction);

			base.OnModelCreating(modelBuilder);
		}
	}
}
