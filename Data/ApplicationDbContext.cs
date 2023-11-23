using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace PoliBaza.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		public DbSet<UserPreferences> UserPreferences { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		
		{
		}
	}
}