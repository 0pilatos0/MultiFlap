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
    [Route("api/powerups")]
    public class PowerUpItemController : BaseController
    {
        private readonly MultiFlapDbContext _context;

        public PowerUpItemController(
            MultiFlapDbContext context,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
        )
            : base(httpClientFactory, memoryCache)
        {
            _context = context;
        }

        // GET api/powerups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PowerUpItem>>> GetPowerUpItems()
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var powerUpItems = await _context.PowerUpItems
                .Where(pu => pu.UserId == user.Id)
                .ToListAsync();

            return powerUpItems;
        }

        // POST api/powerups
        [HttpPost]
        public async Task<ActionResult<PowerUpItem>> AddPowerUpItem(PowerUpItem powerUpItem)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            powerUpItem.UserId = user.Id;

            _context.PowerUpItems.Add(powerUpItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetPowerUpItem),
                new { userId = powerUpItem.UserId, id = powerUpItem.Id },
                powerUpItem
            );
        }

        // PUT api/powerups/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePowerUpItem(int id, PowerUpItem updatedPowerUpItem)
        {
            if (id != updatedPowerUpItem.Id)
            {
                return BadRequest();
            }

            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            if (user.Id != updatedPowerUpItem.UserId)
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
                if (!PowerUpItemExists(updatedPowerUpItem.UserId, id))
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

        // DELETE api/powerups/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePowerUpItem(int id)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var powerUpItem = await _context.PowerUpItems.FindAsync(id);

            if (powerUpItem == null || powerUpItem.UserId != user.Id)
            {
                return NotFound();
            }

            _context.PowerUpItems.Remove(powerUpItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET api/powerups/{id}
        [HttpGet("{id}")]
        private async Task<ActionResult<PowerUpItem>> GetPowerUpItem(int id)
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

            if (user == null)
            {
                return NotFound();
            }

            var powerUpItem = await _context.PowerUpItems.FirstOrDefaultAsync(
                pu => pu.UserId == user.Id && pu.Id == id
            );

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
