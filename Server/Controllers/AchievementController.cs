using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/users/{userId}/achievements")]
	public class AchievementController : BaseController
	{
		private readonly MultiFlapDbContext _context;

		public AchievementController(MultiFlapDbContext context)
		{
			_context = context;
		}

		// GET api/users/{userId}/achievements
		[HttpGet]
		public ActionResult<IEnumerable<Achievement>> GetAchievements(int userId)
		{
			// Retrieve achievements for the given user ID
			var achievements = _context.Achievements.Where(a => a.UserId == userId).ToList();

			return achievements;
		}

		// POST api/users/{userId}/achievements
		[HttpPost]
		public async Task<ActionResult<Achievement>> AddAchievement(int userId, Achievement achievement)
		{
			achievement.UserId = userId;

			// Add the new achievement to the context and save changes
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

			// Update the achievement in the context and save changes
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
			// Find the achievement to delete
			var achievement = await _context.Achievements.FindAsync(id);

			if (achievement == null || achievement.UserId != userId)
			{
				return NotFound();
			}

			// Remove the achievement from the context and save changes
			_context.Achievements.Remove(achievement);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET api/users/{userId}/achievements/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<Achievement>> GetAchievement(int userId, int id)
		{
			// Find the requested achievement
			var achievement = await _context.Achievements.FindAsync(id);

			if (achievement == null || achievement.UserId != userId)
			{
				return NotFound();
			}

			return achievement;
		}

		private bool AchievementExists(int userId, int id)
		{
			// Check if an achievement with the given user ID and ID exists in the context
			return _context.Achievements.Any(a => a.UserId == userId && a.Id == id);
		}
	}
}
