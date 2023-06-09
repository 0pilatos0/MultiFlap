﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(MultiFlapDbContext))]
    partial class MultiFlapDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Server.Models.Achievement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Achievements");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "yes",
                            Name = "High Scorer",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Description = "no",
                            Name = "Speed Runner",
                            UserId = 2
                        });
                });

            modelBuilder.Entity("Server.Models.LeaderboardEntry", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateAchieved")
                        .HasColumnType("datetime2");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("LeaderboardEntries");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9601),
                            Score = 700,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9684),
                            Score = 450,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9691),
                            Score = 900,
                            UserId = 3
                        },
                        new
                        {
                            Id = 4,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9697),
                            Score = 350,
                            UserId = 4
                        },
                        new
                        {
                            Id = 5,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9704),
                            Score = 600,
                            UserId = 5
                        },
                        new
                        {
                            Id = 6,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9709),
                            Score = 800,
                            UserId = 6
                        },
                        new
                        {
                            Id = 7,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9716),
                            Score = 250,
                            UserId = 7
                        },
                        new
                        {
                            Id = 8,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9722),
                            Score = 550,
                            UserId = 8
                        },
                        new
                        {
                            Id = 9,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9727),
                            Score = 950,
                            UserId = 9
                        },
                        new
                        {
                            Id = 10,
                            DateAchieved = new DateTime(2023, 6, 10, 20, 15, 52, 289, DateTimeKind.Local).AddTicks(9733),
                            Score = 400,
                            UserId = 10
                        });
                });

            modelBuilder.Entity("Server.Models.PowerUpItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PowerUpItems");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Extra Life",
                            Quantity = 5,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Name = "Super Boost",
                            Quantity = 3,
                            UserId = 2
                        });
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Auth0Identifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "john.doe@example.com"
                        },
                        new
                        {
                            Id = 2,
                            Email = "jane.smith@example.com"
                        },
                        new
                        {
                            Id = 3,
                            Email = "mark.johnson@example.com"
                        },
                        new
                        {
                            Id = 4,
                            Email = "amy.wilson@example.com"
                        },
                        new
                        {
                            Id = 5,
                            Email = "michael.brown@example.com"
                        },
                        new
                        {
                            Id = 6,
                            Email = "emily.davis@example.com"
                        },
                        new
                        {
                            Id = 7,
                            Email = "jacob.miller@example.com"
                        },
                        new
                        {
                            Id = 8,
                            Email = "olivia.jones@example.com"
                        },
                        new
                        {
                            Id = 9,
                            Email = "william.moore@example.com"
                        },
                        new
                        {
                            Id = 10,
                            Email = "sophia.taylor@example.com"
                        });
                });

            modelBuilder.Entity("Server.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReceiveNotifications")
                        .HasColumnType("bit");

                    b.Property<bool>("ShakeEnabled")
                        .HasColumnType("bit");

                    b.Property<bool>("SoundEnabled")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayName = "Johndogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            DisplayName = "Janedogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            DisplayName = "Markdogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 3
                        },
                        new
                        {
                            Id = 4,
                            DisplayName = "Amydogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 4
                        },
                        new
                        {
                            Id = 5,
                            DisplayName = "Michaeldogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 5
                        },
                        new
                        {
                            Id = 6,
                            DisplayName = "Emilydogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 6
                        },
                        new
                        {
                            Id = 7,
                            DisplayName = "Jacobdogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 7
                        },
                        new
                        {
                            Id = 8,
                            DisplayName = "Oliviadogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 8
                        },
                        new
                        {
                            Id = 9,
                            DisplayName = "Williamdogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 9
                        },
                        new
                        {
                            Id = 10,
                            DisplayName = "Sophiadogamer",
                            Language = "English",
                            ReceiveNotifications = true,
                            ShakeEnabled = false,
                            SoundEnabled = false,
                            UserId = 10
                        });
                });

            modelBuilder.Entity("Server.Models.Achievement", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithMany("Achievements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.LeaderboardEntry", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithMany("LeaderboardEntries")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.PowerUpItem", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithMany("PowerUpItems")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.UserSettings", b =>
                {
                    b.HasOne("Server.Models.User", "User")
                        .WithOne("UserSettings")
                        .HasForeignKey("Server.Models.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Navigation("Achievements");

                    b.Navigation("LeaderboardEntries");

                    b.Navigation("PowerUpItems");

                    b.Navigation("UserSettings")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
