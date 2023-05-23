using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;

namespace Server.Controllers
{
	public class BaseController : ControllerBase
	{
		protected async Task<string> GetAuth0IdFromAuthorizedRequestAsync(IMemoryCache _memoryCache)
		{
			var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
			if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
			{
				var token = authorizationHeader.Substring("Bearer ".Length);

				// Check if the user ID is already cached
				if (_memoryCache.TryGetValue(token, out string userId))
				{
					return userId;
				}

				var httpClient = new HttpClient();
				httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

				var response = await httpClient.GetAsync("https://dev-84ref6m25ippcu2o.us.auth0.com/userinfo");
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var json = JObject.Parse(content);
					userId = json["sub"]?.Value<string>();

					if (userId != null)
					{
						// Cache the user ID for future use
						_memoryCache.Set(token, userId, TimeSpan.FromMinutes(10)); // Cache for 10 minutes
						return userId;
					}
				}
			}

			throw new Exception("Failed to retrieve the user ID from the authorized request.");
		}
	}
}
