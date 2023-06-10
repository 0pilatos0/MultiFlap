using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using Server.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Server.Controllers
{
    // Base controller class for common functionalities
    public class BaseController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;

        public BaseController(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache)
        {
            _httpClient = httpClientFactory.CreateClient();
            _memoryCache = memoryCache;
        }

        // Retrieves the Auth0 ID from the authorized request using the provided memory cache
        public async Task<string> GetAuth0IdFromAuthorizedRequestAsync()
        {
            // Retrieve the authorization header from the request
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (
                !string.IsNullOrEmpty(authorizationHeader)
                && authorizationHeader.StartsWith("Bearer ")
            )
            {
                var token = authorizationHeader.Substring("Bearer ".Length);

                // Check if the user ID is already cached
                if (_memoryCache.TryGetValue(token, out string userId))
                {
                    return userId;
                }

                // Make a request to Auth0's userinfo endpoint to get the user ID
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    token
                );
                var response = await _httpClient.GetAsync(
                    "https://dev-84ref6m25ippcu2o.us.auth0.com/userinfo"
                );

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

            // No valid user ID found in the request
            throw new Exception("Failed to retrieve the user ID from the authorized request.");
        }

        // Retrieves a user from the provided Auth0 ID, creates a new user if not found
        public async Task<User> GetUserFromIdAsync(MultiFlapDbContext context, string userAuth0Id)
        {
            var user = await context.Users.FirstOrDefaultAsync(
                u => u.Auth0Identifier == userAuth0Id
            );

            if (user == null)
            {
                // Generate a random username
                string username = "user" + new Random().Next(1000000, 9999999).ToString();

                // Create a new user with the provided ID
                user = new User { Auth0Identifier = userAuth0Id, Email = "" };
                context.Users.Add(user);

                // Create UserSettings for the new user and set the DisplayName
                UserSettings userSettings = new UserSettings
                {
                    User = user,
                    DisplayName = username,
                    Language = "english",
                    ReceiveNotifications = true,
                    SoundEnabled = true
                };
                context.UserSettings.Add(userSettings);
            }

            return user;
        }
    }
}
