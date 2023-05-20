using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/leaderboard")]
	public class LeaderboardEntryController : ControllerBase
	{
		private readonly MultiFlapDbContext _context; // Replace YourAppContext with your actual database context

		public LeaderboardEntryController(MultiFlapDbContext context)
		{
			_context = context;
		}

		// GET api/leaderboard
		[HttpGet]
		public ActionResult<IEnumerable<LeaderboardEntry>> GetLeaderboard()
		{
			var leaderboard = _context.LeaderboardEntries.OrderByDescending(le => le.Score).ToList();

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
		public async Task<ActionResult<LeaderboardEntry>> AddLeaderboardEntry(LeaderboardEntry leaderboardEntry)
		{
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
}
