using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/users/{userId}/achievements")]
	public class AchievementController : ControllerBase
	{
		private readonly MultiFlapDbContext _context; // Replace YourAppContext with your actual database context

		public AchievementController(MultiFlapDbContext context)
		{
			_context = context;
		}

		// GET api/users/{userId}/achievements
		[HttpGet]
		public ActionResult<IEnumerable<Achievement>> GetAchievements(int userId)
		{
			var achievements = _context.Achievements.Where(a => a.UserId == userId).ToList();

			return achievements;
		}

		// POST api/users/{userId}/achievements
		[HttpPost]
		public async Task<ActionResult<Achievement>> AddAchievement(int userId, Achievement achievement)
		{
			achievement.UserId = userId;

			_context.Achievements.Add(achievement);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetAchievement), new { userId = achievement.UserId, id = achievement.Id }, achievement);
		}

		// PUT api/users/{userId}/achievements/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateAchievement(int userId, int id, Achievement updatedAchievement)
		{
			if (userId != updatedAchievement.UserId || id != updatedAchievement.Id)
			{
				return BadRequest();
			}

			_context.Entry(updatedAchievement).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!AchievementExists(userId, id))
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

		// DELETE api/users/{userId}/achievements/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteAchievement(int userId, int id)
		{
			var achievement = await _context.Achievements.FindAsync(id);

			if (achievement == null || achievement.UserId != userId)
			{
				return NotFound();
			}

			_context.Achievements.Remove(achievement);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET api/users/{userId}/achievements/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Achievement>> GetAchievement(int userId, int id)
		{
			var achievement = await _context.Achievements.FindAsync(id);

			if (achievement == null || achievement.UserId != userId)
			{
				return NotFound();
			}

			return achievement;
		}

		private bool AchievementExists(int userId, int id)
		{
			return _context.Achievements.Any(a => a.UserId == userId && a.Id == id);
		}
	}
}
