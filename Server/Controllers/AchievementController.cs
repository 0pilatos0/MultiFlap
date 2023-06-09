using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/achievements")]
    public class AchievementController : BaseController
    {
        private readonly MultiFlapDbContext _context;

        public AchievementController(
            MultiFlapDbContext context,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
        )
            : base(httpClientFactory, memoryCache)
        {
            _context = context;
        }

        // GET api/achievements
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Achievement>>> GetAchievements()
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var achievements = await _context.Achievements
                .Where(a => a.UserId == user.Id)
                .ToListAsync();

            return achievements;
        }

        // POST api/achievements
        [HttpPost]
        public async Task<ActionResult<Achievement>> AddAchievement(Achievement achievement)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            achievement.UserId = user.Id;

            _context.Achievements.Add(achievement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAchievement),
                new { id = achievement.Id },
                achievement
            );
        }

        // PUT api/achievements/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAchievement(int id, Achievement updatedAchievement)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null || updatedAchievement.UserId != user.Id || updatedAchievement.Id != id)
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
                if (!AchievementExists(user.Id, id))
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

        // DELETE api/achievements/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievement(int id)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements.FirstOrDefaultAsync(
                a => a.UserId == user.Id && a.Id == id
            );

            if (achievement == null)
            {
                return NotFound();
            }

            _context.Achievements.Remove(achievement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET api/achievements/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Achievement>> GetAchievement(int id)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var achievement = await _context.Achievements.FirstOrDefaultAsync(
                a => a.UserId == user.Id && a.Id == id
            );

            if (achievement == null)
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
