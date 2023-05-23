using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/users/{userId}/settings")]
	public class UserSettingsController : BaseController
	{
		private readonly MultiFlapDbContext _context; // Replace YourAppContext with your actual database context

		public UserSettingsController(MultiFlapDbContext context)
		{
			_context = context;
		}

		// GET api/users/{userId}/settings
		[HttpGet]
		public async Task<ActionResult<UserSettings>> GetUserSettings(int userId)
		{
			var userSettings = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == userId);

			if (userSettings == null)
			{
				return NotFound();
			}

			return userSettings;
		}

		// PUT api/users/{userId}/settings
		[HttpPut]
		public async Task<IActionResult> UpdateUserSettings(int userId, UserSettings updatedUserSettings)
		{
			if (userId != updatedUserSettings.UserId)
			{
				return BadRequest();
			}

			_context.Entry(updatedUserSettings).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserSettingsExists(userId))
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

		private bool UserSettingsExists(int userId)
		{
			return _context.UserSettings.Any(us => us.UserId == userId);
		}
	}
}
