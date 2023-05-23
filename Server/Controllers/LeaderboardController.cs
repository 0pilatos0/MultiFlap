using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/leaderboard")]
	public class LeaderboardEntryController : BaseController
	{
		private readonly MultiFlapDbContext _context; // Replace YourAppContext with your actual database context
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

		// GET api/leaderboard/{id}
		[HttpGet("{id}")]
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
			// Get user based on the authorized request
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

		

		// PUT api/leaderboard/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateLeaderboardEntry(int id, LeaderboardEntry updatedLeaderboardEntry)
		{
			if (id != updatedLeaderboardEntry.Id)
			{
				return BadRequest();
			}

			_context.Entry(updatedLeaderboardEntry).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!LeaderboardEntryExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}

		// DELETE api/leaderboard/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteLeaderboardEntry(int id)
		{
			var leaderboardEntry = await _context.LeaderboardEntries.FindAsync(id);

			if (leaderboardEntry == null)
			{
				return NotFound();
			}

			_context.LeaderboardEntries.Remove(leaderboardEntry);
			await _context.SaveChangesAsync();

			return NoContent();
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
