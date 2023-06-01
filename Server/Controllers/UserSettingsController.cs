﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/users/settings")]
	public class UserSettingsController : BaseController
	{
		private readonly MultiFlapDbContext _context;
		private readonly IMemoryCache _memoryCache;

		public UserSettingsController(MultiFlapDbContext context, IMemoryCache memoryCache)
		{
			_context = context;
			_memoryCache = memoryCache;
		}

		// GET api/users/settings
		[HttpGet]
		public async Task<ActionResult<UserSettingsDTO>> GetUserSettings()
		{
			var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync(_memoryCache);
			var user = await GetUserFromIdAsync(_context, userAuth0Id);

			if (user == null)
			{
				return NotFound();
			}

			var userSettings = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == user.Id);

			if (userSettings == null)
			{
				return NotFound();
			}

			return new UserSettingsDTO
			{
				Language = userSettings.Language,
				ReceiveNotifications = userSettings.ReceiveNotifications,
				DisplayName = userSettings.DisplayName,
				SoundEnabled = userSettings.SoundEnabled,
				ShakeEnabled = userSettings.ShakeEnabled
			};
		}

		// PUT api/users/settings
		[HttpPut]
		public async Task<IActionResult> UpdateUserSettings(UserSettingsDTO updatedUserSettings)
		{
			var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync(_memoryCache);
			var user = await GetUserFromIdAsync(_context, userAuth0Id);

			if (user == null)
			{
				return NotFound();
			}

			var userSettings = await _context.UserSettings.FirstOrDefaultAsync(us => us.UserId == user.Id);

			if (userSettings == null)
			{
				userSettings = new UserSettings();
				user.UserSettings = userSettings;
			}

			// Update the user settings properties
			userSettings.Language = updatedUserSettings.Language;
			userSettings.ReceiveNotifications = updatedUserSettings.ReceiveNotifications;
			userSettings.DisplayName = updatedUserSettings.DisplayName;
			userSettings.SoundEnabled = updatedUserSettings.SoundEnabled;
			userSettings.ShakeEnabled = updatedUserSettings.ShakeEnabled;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserSettingsExists(user.Id))
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

	public class UserSettingsDTO
	{
		public string Language { get; set; }
		public bool ReceiveNotifications { get; set; }
		public string DisplayName { get; set; }
		public bool SoundEnabled { get; set; }
		public bool ShakeEnabled { get; set; }
	}
}
