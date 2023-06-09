using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Models;
using System.Threading.Tasks;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/users")]
    public class UserController : BaseController
    {
        private readonly MultiFlapDbContext _context;

        public UserController(
            MultiFlapDbContext context,
            IMemoryCache memoryCache,
            IHttpClientFactory httpClientFactory
        )
            : base(httpClientFactory, memoryCache)
        {
            _context = context;
        }

        // DELETE api/users/
        [HttpDelete()]
        public async Task<IActionResult> DeleteUser()
        {
            var userAuth0Id = await GetAuth0IdFromAuthorizedRequestAsync();
            var user = await GetUserFromIdAsync(_context, userAuth0Id);

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
