using Bulky.Models;
using Microsoft.EntityFrameworkCore;

namespace Bulky.DataAccess.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}
		public DbSet<Category> Categories { get; set; }

		//seeder de la base de donnees

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			//seeder de la table category
			modelBuilder.Entity<Category>().HasData(
				new Category { categoryId = 1, Name= "Electricity", DisplayOrder= 1},
				new Category { categoryId = 2, Name= "Man", DisplayOrder= 2},
				new Category { categoryId = 3, Name= "Test", DisplayOrder= 3},
				new Category { categoryId = 4, Name= "Jobs", DisplayOrder= 4},
				new Category { categoryId = 5, Name= "DevTech", DisplayOrder= 5}

				);
		}
	}
}
