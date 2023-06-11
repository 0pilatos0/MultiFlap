using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/leaderboard")]
    public class LeaderboardEntryController : BaseController
    {
        private readonly MultiFlapDbContext _context;
        private readonly IMemoryCache _memoryCache;

        public LeaderboardEntryController(
            MultiFlapDbContext context,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
        )
            : base(httpClientFactory, memoryCache)
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
                .Select(
                    le =>
                        new LeaderboardEntryDTO
                        {
                            Id = le.Id,
                            Score = le.Score,
                            DateAchieved = le.DateAchieved,
                            DisplayName = le.User.UserSettings.DisplayName
                        }
                )
                .ToList();

            return leaderboard;
        }

        [HttpGet("me")]
        public async Task<ActionResult<int>> GetOwnHighscore()
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();

            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var highscore = _context.LeaderboardEntries
                .Where(le => le.User == user)
                .OrderByDescending(le => le.Score)
                .Select(le => le.Score)
                .FirstOrDefault();

            return Ok(highscore);
        }

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

        [HttpPost]
        public async Task<ActionResult<LeaderboardEntry>> AddLeaderboardEntry(
            LeaderboardEntryDTO newLeaderboardEntry
        )
        {
            if (newLeaderboardEntry == null)
            {
                return BadRequest();
            }

            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();

            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var leaderboardEntry = new LeaderboardEntry
            {
                User = user,
                Score = newLeaderboardEntry.Score,
                DateAchieved = DateTime.Now
            };

            _context.LeaderboardEntries.Add(leaderboardEntry);

            //if its the users first entry, give them the achievement
            var userAchievements = await _context.Achievements
                .Where(ua => ua.UserId == user.Id)
                .ToListAsync();

            //if doenst have achievement with name "First Score"
            if (!userAchievements.Any(ua => ua.Name == "First Score"))
            {
                //add achievement to user
                var userAchievement = new Achievement
                {
                    Name = "First Score",
                    Description = "You got your first score!",
                    User = user
                };

                _context.Achievements.Add(userAchievement);
            }

            //if score was bigger than 1000, give them the achievement "1000+ Score" if they dont have it
            if (leaderboardEntry.Score >= 1000)
            {
                if (!userAchievements.Any(ua => ua.Name == "1000+ Score"))
                {
                    var userAchievement = new Achievement
                    {
                        Name = "1000+ Score",
                        Description = "You got a score of 1000 or more!",
                        User = user
                    };

                    _context.Achievements.Add(userAchievement);
                }
            }

            if (leaderboardEntry.Score >= 200)
            {
                var powerUpItem = new PowerUpItem { Name = "1.05 Multiplier", User = user };
                _context.PowerUpItems.Add(powerUpItem);
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetLeaderboardEntry),
                new { id = leaderboardEntry.Id },
                leaderboardEntry
            );
        }

        private bool LeaderboardEntryExists(int id)
        {
            return _context.LeaderboardEntries.Any(le => le.Id == id);
        }
    }
}
