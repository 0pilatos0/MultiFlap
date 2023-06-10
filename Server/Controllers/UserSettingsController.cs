using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using Shared.DTOs;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users/settings")]
    public class UserSettingsController : BaseController
    {
        private readonly MultiFlapDbContext _context;

        public UserSettingsController(
            MultiFlapDbContext context,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
        )
            : base(httpClientFactory, memoryCache)
        {
            _context = context;
        }

        // GET api/users/settings
        [HttpGet]
        public async Task<ActionResult<UserSettingsDTO>> GetUserSettings()
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(
                us => us.UserId == user.Id
            );

            if (userSettings == null)
            {
                return NotFound();
            }

            var userSettingsDto = MapToUserSettingsDto(userSettings);
            return userSettingsDto;
        }

        // PUT api/users/settings
        [HttpPut]
        public async Task<IActionResult> UpdateUserSettings(UserSettingsDTO updatedUserSettings)
        {
            if (updatedUserSettings == null)
            {
                return BadRequest();
            }

            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var userSettings = await _context.UserSettings.FirstOrDefaultAsync(
                us => us.UserId == user.Id
            );

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

            //if its the users first entry, give them the achievement
            var userAchievements = await _context.Achievements
                .Where(ua => ua.UserId == user.Id)
                .ToListAsync();

            //if doenst have achievement with name "First Score"
            if (!userAchievements.Any(ua => ua.Name == "Making choices"))
            {
                //add achievement to user
                var userAchievement = new Achievement
                {
                    Name = "Making choices",
                    Description = "You made the choice to change your settings!",
                    User = user
                };

                _context.Achievements.Add(userAchievement);
            }

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

        private static UserSettingsDTO MapToUserSettingsDto(UserSettings userSettings)
        {
            return new UserSettingsDTO
            {
                Language = userSettings.Language,
                ReceiveNotifications = userSettings.ReceiveNotifications,
                DisplayName = userSettings.DisplayName,
                SoundEnabled = userSettings.SoundEnabled,
                ShakeEnabled = userSettings.ShakeEnabled
            };
        }
    }
}
