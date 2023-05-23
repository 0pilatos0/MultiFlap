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

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

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
                            IsCompleted = true,
                            Name = "High Scorer",
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            IsCompleted = false,
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
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(887),
                            Score = 100,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(932),
                            Score = 200,
                            UserId = 2
                        },
                        new
                        {
                            Id = 3,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(934),
                            Score = 150,
                            UserId = 1
                        },
                        new
                        {
                            Id = 4,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(936),
                            Score = 180,
                            UserId = 2
                        },
                        new
                        {
                            Id = 5,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(938),
                            Score = 220,
                            UserId = 1
                        },
                        new
                        {
                            Id = 6,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(940),
                            Score = 300,
                            UserId = 2
                        },
                        new
                        {
                            Id = 7,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(942),
                            Score = 250,
                            UserId = 1
                        },
                        new
                        {
                            Id = 8,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(944),
                            Score = 190,
                            UserId = 2
                        },
                        new
                        {
                            Id = 9,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(946),
                            Score = 280,
                            UserId = 1
                        },
                        new
                        {
                            Id = 10,
                            DateAchieved = new DateTime(2023, 5, 23, 11, 27, 32, 496, DateTimeKind.Local).AddTicks(948),
                            Score = 230,
                            UserId = 2
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

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("SoundEnabled")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DisplayName = "Admin1",
                            Email = "admin1@example.com",
                            SoundEnabled = true
                        },
                        new
                        {
                            Id = 2,
                            DisplayName = "Admin2",
                            Email = "admin2@example.com",
                            SoundEnabled = false
                        });
                });

            modelBuilder.Entity("Server.Models.UserSettings", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ReceiveNotifications")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserSettings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Language = "English",
                            ReceiveNotifications = true,
                            UserId = 1
                        },
                        new
                        {
                            Id = 2,
                            Language = "French",
                            ReceiveNotifications = false,
                            UserId = 2
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
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Navigation("Achievements");

                    b.Navigation("LeaderboardEntries");

                    b.Navigation("PowerUpItems");
                });
#pragma warning restore 612, 618
        }
    }
}
