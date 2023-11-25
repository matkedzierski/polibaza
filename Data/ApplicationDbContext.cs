using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PoliBaza.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<UserPreferences> UserPreferences { get; set; }
		public DbSet<LibraryItem> LibraryItems { get; set; }
		public DbSet<LibraryItem.Book> Books { get; set; }
		public DbSet<LibraryItem.Magazine> Magazines { get; set; }
		public DbSet<LibraryItem.Multimedia> Multimedia { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			builder.Entity<LibraryItem>()
				.HasDiscriminator(b => b.ItemType)
				.HasValue<LibraryItem>(LibraryItem.Type.GENERAL)
				.HasValue<LibraryItem.Book>(LibraryItem.Type.BOOK)
				.HasValue<LibraryItem.Magazine>(LibraryItem.Type.MAGAZINE)
				.HasValue<LibraryItem.Multimedia>(LibraryItem.Type.MULTIMEDIA);
		}
	}
}