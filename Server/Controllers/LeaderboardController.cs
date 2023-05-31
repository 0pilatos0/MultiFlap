using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/leaderboard")]
	public class LeaderboardEntryController : BaseController
	{
		private readonly MultiFlapDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public LeaderboardEntryController(MultiFlapDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		// GET api/leaderboard
		[HttpGet]
		public ActionResult<IEnumerable<LeaderboardEntryDTO>> GetLeaderboard()
		{
			// Retrieve leaderboard entries with associated user data and order by score
			var leaderboard = _context.LeaderboardEntries
				.Include(le => le.User)
				.OrderByDescending(le => le.Score)
				.Select(le => new LeaderboardEntryDTO
				{
					Id = le.Id,
					Score = le.Score,
					DateAchieved = le.DateAchieved,
					DisplayName = le.User.UserSettings.DisplayName
				})
				.ToList();

			return leaderboard;
		}

		[HttpGet("me")]
		public async Task<ActionResult<int>> GetOwnHighscore()
		{
			var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync(_memoryCache);

			User? user = await GetUserFromIdAsync(_context, userAuth0Id);

			var highscore = _context.LeaderboardEntries
				.Where(le => le.User == user)
				.OrderByDescending(le => le.Score)
				.Select(le => le.Score)
				.FirstOrDefault();

			return Ok(highscore);
		}

		public async Task<ActionResult<LeaderboardEntry>> GetLeaderboardEntry(int id)
		{
			var leaderboardEntry = await _context.LeaderboardEntries.FindAsync(id);

			if (leaderboardEntry == null)
			{
				return NotFound();
			}

			return leaderboardEntry;
		}

		// POST api/leaderboard
		[HttpPost]
		public async Task<ActionResult<LeaderboardEntry>> AddLeaderboardEntry(LeaderboardEntryDTO newLeaderboardEntry)
		{
			// Get the user's Auth0 ID from the authorized request
			var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync(_memoryCache);

			// Check if the user exists, otherwise create it
			User? user = await GetUserFromIdAsync(_context, userAuth0Id);

			LeaderboardEntry leaderboardEntry = new LeaderboardEntry
			{
				User = user,
				Score = newLeaderboardEntry.Score,
				DateAchieved = DateTime.Now
			};

			Console.WriteLine($"Adding leaderboard entry for user {user.Id} with score {newLeaderboardEntry.Score}");

			// Add the leaderboard entry to the database
			_context.LeaderboardEntries.Add(leaderboardEntry);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetLeaderboardEntry), new { id = leaderboardEntry.Id }, leaderboardEntry);
		}

		private bool LeaderboardEntryExists(int id)
		{
			return _context.LeaderboardEntries.Any(le => le.Id == id);
		}
	}

	public class LeaderboardEntryDTO
	{
		public int Id { get; set; }
		public int Score { get; set; }
		public DateTime DateAchieved { get; set; }
		public string? DisplayName { get; set; }
	}
}
