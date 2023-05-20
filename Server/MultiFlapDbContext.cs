using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server
{
	public class MultiFlapDbContext : DbContext
	{
		public MultiFlapDbContext(DbContextOptions<MultiFlapDbContext> options) : base(options)
		{
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserSettings> UserSettings { get; set; }
		public DbSet<Achievement> Achievements { get; set; }
		public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
		public DbSet<PowerUpItem> PowerUpItems { get; set; }
		
		
	}
}
