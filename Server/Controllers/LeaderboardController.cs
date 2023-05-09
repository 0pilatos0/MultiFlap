using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class LeaderboardController : ControllerBase
	{
		private readonly MultiFlapDbContext _dbContext;

		public LeaderboardController(MultiFlapDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// POST api/leaderboard
		[HttpPost]
		public IActionResult Post([FromBody] User user)
		{
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
			return Ok(user);
		}

		// GET api/leaderboard
		[HttpGet]
		public IActionResult Get()
		{
			List<User> leaderboard = _dbContext.Users.OrderByDescending(u => u.HighScore)
													.Take(10)
													.ToList();
			return Ok(leaderboard);
		}

		// PUT api/leaderboard/{id}
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] int highscore)
		{
			User user = _dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			user.HighScore = highscore;
			_dbContext.SaveChanges();

			return Ok(user);
		}
	}
}
