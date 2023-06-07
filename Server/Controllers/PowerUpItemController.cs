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
	[Route("api/users/{userId}/powerups")]
	public class PowerUpItemController : BaseController
	{
		private readonly MultiFlapDbContext _context;

		public PowerUpItemController(MultiFlapDbContext context, IMemoryCache memoryCache, IHttpClientFactory httpClientFactory) : base(httpClientFactory, memoryCache)
		{
			_context = context;
		}

		// GET api/users/{userId}/powerups
		[HttpGet]
		public ActionResult<IEnumerable<PowerUpItem>> GetPowerUpItems(int userId)
		{
			var powerUpItems = _context.PowerUpItems
				.Where(pu => pu.UserId == userId)
				.ToList();

			return powerUpItems;
		}

		// POST api/users/{userId}/powerups
		[HttpPost]
		public async Task<ActionResult<PowerUpItem>> AddPowerUpItem(int userId, PowerUpItem powerUpItem)
		{
			powerUpItem.UserId = userId;

			_context.PowerUpItems.Add(powerUpItem);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetPowerUpItem), new { userId = powerUpItem.UserId, id = powerUpItem.Id }, powerUpItem);
		}

		// PUT api/users/{userId}/powerups/{id}
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdatePowerUpItem(int userId, int id, PowerUpItem updatedPowerUpItem)
		{
			if (userId != updatedPowerUpItem.UserId || id != updatedPowerUpItem.Id)
			{
				return BadRequest();
			}

			_context.Entry(updatedPowerUpItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!PowerUpItemExists(userId, id))
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

		// DELETE api/users/{userId}/powerups/{id}
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeletePowerUpItem(int userId, int id)
		{
			var powerUpItem = await _context.PowerUpItems.FindAsync(id);

			if (powerUpItem == null || powerUpItem.UserId != userId)
			{
				return NotFound();
			}

			_context.PowerUpItems.Remove(powerUpItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET api/users/{userId}/powerups/{id}
		[HttpGet("{id}")]
		private ActionResult<PowerUpItem> GetPowerUpItem(int userId, int id)
		{
			var powerUpItem = _context.PowerUpItems.FirstOrDefault(pu => pu.UserId == userId && pu.Id == id);

			if (powerUpItem == null)
			{
				return NotFound();
			}

			return powerUpItem;
		}

		private bool PowerUpItemExists(int userId, int id)
		{
			return _context.PowerUpItems.Any(pu => pu.UserId == userId && pu.Id == id);
		}
	}
}
