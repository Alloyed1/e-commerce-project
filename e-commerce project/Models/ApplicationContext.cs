using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace e_commerce_project.Models
{
	public class ApplicationContext : IdentityDbContext<User>
	{
		public ApplicationContext(DbContextOptions<ApplicationContext> options)
			: base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<Favorite>().HasKey(k => new { k.Id ,k.UserId });
			builder.Entity<Basket>().HasKey(k => new { k.Id, k.UserId });
		}
		public DbSet<Item> Items { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Brand> Brand { get; set; }
		public DbSet<Favorite> Favorites { get; set; }
		public DbSet<Basket> Baskets { get; set; }
	}
}
