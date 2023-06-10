using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server
{
    public class MultiFlapDbContext : DbContext
    {
        public MultiFlapDbContext(DbContextOptions<MultiFlapDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User { Id = 1, Email = "john.doe@example.com" },
                    new User { Id = 2, Email = "jane.smith@example.com" },
                    new User { Id = 3, Email = "mark.johnson@example.com" },
                    new User { Id = 4, Email = "amy.wilson@example.com" },
                    new User { Id = 5, Email = "michael.brown@example.com" },
                    new User { Id = 6, Email = "emily.davis@example.com" },
                    new User { Id = 7, Email = "jacob.miller@example.com" },
                    new User { Id = 8, Email = "olivia.jones@example.com" },
                    new User { Id = 9, Email = "william.moore@example.com" },
                    new User { Id = 10, Email = "sophia.taylor@example.com" }
                );

            modelBuilder
                .Entity<UserSettings>()
                .HasData(
                    new UserSettings
                    {
                        Id = 1,
                        UserId = 1,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Johndogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 2,
                        UserId = 2,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Janedogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 3,
                        UserId = 3,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Markdogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 4,
                        UserId = 4,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Amydogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 5,
                        UserId = 5,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Michaeldogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 6,
                        UserId = 6,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Emilydogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 7,
                        UserId = 7,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Jacobdogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 8,
                        UserId = 8,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Oliviadogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 9,
                        UserId = 9,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Williamdogamer",
                        SoundEnabled = false
                    },
                    new UserSettings
                    {
                        Id = 10,
                        UserId = 10,
                        Language = "English",
                        ReceiveNotifications = true,
                        DisplayName = "Sophiadogamer",
                        SoundEnabled = false
                    }
                );

            modelBuilder
                .Entity<Achievement>()
                .HasData(
                    new Achievement
                    {
                        Id = 1,
                        Name = "High Scorer",
                        Description = "yes",
                        UserId = 1
                    },
                    new Achievement
                    {
                        Id = 2,
                        Name = "Speed Runner",
                        Description = "no",
                        UserId = 2
                    }
                );

            modelBuilder
                .Entity<LeaderboardEntry>()
                .HasData(
                    new LeaderboardEntry
                    {
                        Id = 1,
                        Score = 700,
                        DateAchieved = DateTime.Now,
                        UserId = 1
                    },
                    new LeaderboardEntry
                    {
                        Id = 2,
                        Score = 450,
                        DateAchieved = DateTime.Now,
                        UserId = 2
                    },
                    new LeaderboardEntry
                    {
                        Id = 3,
                        Score = 900,
                        DateAchieved = DateTime.Now,
                        UserId = 3
                    },
                    new LeaderboardEntry
                    {
                        Id = 4,
                        Score = 350,
                        DateAchieved = DateTime.Now,
                        UserId = 4
                    },
                    new LeaderboardEntry
                    {
                        Id = 5,
                        Score = 600,
                        DateAchieved = DateTime.Now,
                        UserId = 5
                    },
                    new LeaderboardEntry
                    {
                        Id = 6,
                        Score = 800,
                        DateAchieved = DateTime.Now,
                        UserId = 6
                    },
                    new LeaderboardEntry
                    {
                        Id = 7,
                        Score = 250,
                        DateAchieved = DateTime.Now,
                        UserId = 7
                    },
                    new LeaderboardEntry
                    {
                        Id = 8,
                        Score = 550,
                        DateAchieved = DateTime.Now,
                        UserId = 8
                    },
                    new LeaderboardEntry
                    {
                        Id = 9,
                        Score = 950,
                        DateAchieved = DateTime.Now,
                        UserId = 9
                    },
                    new LeaderboardEntry
                    {
                        Id = 10,
                        Score = 400,
                        DateAchieved = DateTime.Now,
                        UserId = 10
                    }
                );

            modelBuilder
                .Entity<PowerUpItem>()
                .HasData(
                    new PowerUpItem
                    {
                        Id = 1,
                        Name = "Extra Life",
                        Quantity = 5,
                        UserId = 1
                    },
                    new PowerUpItem
                    {
                        Id = 2,
                        Name = "Super Boost",
                        Quantity = 3,
                        UserId = 2
                    }
                );
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserSettings> UserSettings { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
        public DbSet<PowerUpItem> PowerUpItems { get; set; }
    }
}
