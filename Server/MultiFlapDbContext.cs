using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server
{
	public class MultiFlapDbContext : DbContext
	{
		public MultiFlapDbContext(DbContextOptions<MultiFlapDbContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>().HasData(
				new User { Id = 1, Email = "admin1@example.com"},
				new User { Id = 2, Email = "admin2@example.com"}
			);

			modelBuilder.Entity<UserSettings>().HasData(
				new UserSettings { Id = 1, UserId = 1, Language = "English", ReceiveNotifications = true, DisplayName = "Admin1", SoundEnabled = false },
				new UserSettings { Id = 2, UserId = 2, Language = "French", ReceiveNotifications = false, DisplayName = "Admin2", SoundEnabled = true }
			);

			modelBuilder.Entity<Achievement>().HasData(
				new Achievement { Id = 1, Name = "High Scorer", IsCompleted = true, UserId = 1 },
				new Achievement { Id = 2, Name = "Speed Runner", IsCompleted = false, UserId = 2 }
			);

			modelBuilder.Entity<LeaderboardEntry>().HasData(
				new LeaderboardEntry { Id = 1, Score = 100, DateAchieved = DateTime.Now, UserId = 1 },
				new LeaderboardEntry { Id = 2, Score = 200, DateAchieved = DateTime.Now, UserId = 2 },
				// Additional leaderboard entries
				new LeaderboardEntry { Id = 3, Score = 150, DateAchieved = DateTime.Now, UserId = 1 },
				new LeaderboardEntry { Id = 4, Score = 180, DateAchieved = DateTime.Now, UserId = 2 },
				new LeaderboardEntry { Id = 5, Score = 220, DateAchieved = DateTime.Now, UserId = 1 },
				new LeaderboardEntry { Id = 6, Score = 300, DateAchieved = DateTime.Now, UserId = 2 },
				new LeaderboardEntry { Id = 7, Score = 250, DateAchieved = DateTime.Now, UserId = 1 },
				new LeaderboardEntry { Id = 8, Score = 190, DateAchieved = DateTime.Now, UserId = 2 },
				new LeaderboardEntry { Id = 9, Score = 280, DateAchieved = DateTime.Now, UserId = 1 },
				new LeaderboardEntry { Id = 10, Score = 230, DateAchieved = DateTime.Now, UserId = 2 }
			);

			modelBuilder.Entity<PowerUpItem>().HasData(
				new PowerUpItem { Id = 1, Name = "Extra Life", Quantity = 5, UserId = 1 },
				new PowerUpItem { Id = 2, Name = "Super Boost", Quantity = 3, UserId = 2 }
			);
		}

		public DbSet<User> Users { get; set; }
		public DbSet<UserSettings> UserSettings { get; set; }
		public DbSet<Achievement> Achievements { get; set; }
		public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
		public DbSet<PowerUpItem> PowerUpItems { get; set; }
		
		
	}
}
