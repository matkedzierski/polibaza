using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace PoliBaza.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<UserPreferences> UserPreferences { get; set; }
		public DbSet<LibraryItem?> LibraryItems { get; set; }
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
			
			builder.Entity<LibraryItem>()
				.Property(e => e.Tags)
				.HasConversion(
					v => string.Join(',', v ?? new string[]{}),
					v => v.Split(',', StringSplitOptions.RemoveEmptyEntries),
					new ValueComparer<string[]>(
						(c1, c2) => (c1 == null && c2 == null) || (c1 != null && c2 != null && c1.Length == c2.Length && c1.Except(c2).ToArray().Length == 0),
						c => string.Join(',', c).GetHashCode(),
						c => c));
		}
	}
}