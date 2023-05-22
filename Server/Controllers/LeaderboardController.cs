using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.IdentityModel.Tokens.Jwt;

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
					DisplayName = le.User.DisplayName
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
			var userId = GetUserIdFromAuthorizedRequest(); // Replace this with your implementation to get the user ID

			// Check if the user exists, otherwise create it
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
			if (user == null)
			{
				// Create a new user with the provided ID
				user = new User { Id = userId };
				_context.Users.Add(user);
			}

			LeaderboardEntry leaderboardEntry = new LeaderboardEntry
			{
				UserId = userId,
				Score = newLeaderboardEntry.Score,
				DateAchieved = DateTime.Now
			};

			// Add the user ID to the leaderboard entry
			leaderboardEntry.UserId = userId;

			Console.WriteLine($"Adding leaderboard entry for user {userId} with score {newLeaderboardEntry.Score}");

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

		private int GetUserIdFromAuthorizedRequest()
		{
			var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
			{
				var token = authorizationHeader.Substring("Bearer ".Length);
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
				var userIdClaim = jwtToken?.Claims.FirstOrDefault(claim => claim.Type == "sub");

				if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
				{
					return userId;
				}
			}
			throw new Exception("Failed to retrieve the user ID from the authorized request.");
		}
	}

	public class LeaderboardEntryDTO
{
    public int Id { get; set; }
    public int Score { get; set; }
    public DateTime DateAchieved { get; set; }
    public string DisplayName { get; set; }
}

}
