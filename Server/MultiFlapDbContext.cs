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
	}
}
