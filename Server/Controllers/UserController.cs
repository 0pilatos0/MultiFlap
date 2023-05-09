using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UserController : ControllerBase
	{
		private readonly MultiFlapDbContext _dbContext;

		public UserController(MultiFlapDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// POST api/user
		[HttpPost]
		public IActionResult Post([FromBody] UserSettings userSettings)
		{
			User user = new User { UserSettings = userSettings };
			_dbContext.Users.Add(user);
			_dbContext.SaveChanges();
			return Ok(user);
		}

		// GET api/user/{id}
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			User user = _dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		// PUT api/user/{id}
		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] UserSettings userSettings)
		{
			User user = _dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			user.UserSettings = userSettings;
			_dbContext.SaveChanges();

			return Ok(user);
		}

		// DELETE api/user/{id}
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			User user = _dbContext.Users.Find(id);
			if (user == null)
			{
				return NotFound();
			}

			_dbContext.Users.Remove(user);
			_dbContext.SaveChanges();

			return Ok();
		}
	}
}
