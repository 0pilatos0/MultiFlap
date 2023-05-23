using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/users")]
	public class UserController : BaseController
	{
		private readonly MultiFlapDbContext _context; // Replace YourAppContext with your actual database context

		public UserController(MultiFlapDbContext context)
		{
			_context = context;
		}

		// GET api/users/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			return user;
		}

		// POST api/users
		[HttpPost]
		public async Task<ActionResult<User>> CreateUser(User user)
		{
			// You may perform additional validation or checks here

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
		}

		// PUT api/users/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateUser(int id, User updatedUser)
		{
			if (id != updatedUser.Id)
			{
				return BadRequest();
			}

			_context.Entry(updatedUser).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!UserExists(id))
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

		// DELETE api/users/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteUser(int id)
		{
			var user = await _context.Users.FindAsync(id);

			if (user == null)
			{
				return NotFound();
			}

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		private bool UserExists(int id)
		{
			return _context.Users.Any(u => u.Id == id);
		}
	}
}
