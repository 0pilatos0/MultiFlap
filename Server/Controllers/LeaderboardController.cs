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
	public class LeaderboardEntryController : ControllerBase
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
			var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync(); // Replace this with your implementation to get the user ID

			// Check if the user exists, otherwise create it
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Auth0Identifier == userAuth0Id);
			if (user == null)
			{

				//generate random username 
				string username = "user" + new Random().Next(1000000, 9999999).ToString();

				// Create a new user with the provided ID
				user = new User { Auth0Identifier = userAuth0Id, DisplayName = username, Email = "",  SoundEnabled = true};
				_context.Users.Add(user);
			}


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

		private async Task<string> GetAuth0IdFromAuthorizedRequestAsync()
		{
			var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
			{
				var token = authorizationHeader.Substring("Bearer ".Length);

				// Check if the user ID is already cached
				if (_memoryCache.TryGetValue(token, out string userId))
				{
					return userId;
				}

				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var response = await httpClient.GetAsync("https://dev-84ref6m25ippcu2o.us.auth0.com/userinfo");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var json = JObject.Parse(content);
					userId = json["sub"]?.Value<string>();

					if (userId != null)
					{
						// Cache the user ID for future use
						_memoryCache.Set(token, userId, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
						return userId;
					}
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
		public string? DisplayName { get; set; }
	}

}
