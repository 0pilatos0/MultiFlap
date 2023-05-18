using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
	internal class ApiService : IApiService
	{
		private readonly HttpClient httpClient;

		public ApiService()
		{
			httpClient = new HttpClient();
		}

		public string ApiUrl { get; } = "http://161.97.97.200:5076/";

		public async Task<string> GetAsync(string endpoint, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

			var response = await httpClient.GetAsync($"{ApiUrl}/{endpoint}");
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> PostAsync(string endpoint, string body, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

			var content = new StringContent(body);
			var response = await httpClient.PostAsync($"{ApiUrl}/{endpoint}", content);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> PutAsync(string endpoint, string body, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

			var content = new StringContent(body);
			var response = await httpClient.PutAsync($"{ApiUrl}/{endpoint}", content);
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> DeleteAsync(string endpoint, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

			var response = await httpClient.DeleteAsync($"{ApiUrl}/{endpoint}");
			return await response.Content.ReadAsStringAsync();
		}

		public async Task<string> PatchAsync(string endpoint, string body, string accessToken)
		{
			httpClient.DefaultRequestHeaders.Clear();
			httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

			var content = new StringContent(body);
			var request = new HttpRequestMessage(new HttpMethod("PATCH"), $"{ApiUrl}/{endpoint}")
			{
				Content = content
			};

			var response = await httpClient.SendAsync(request);
			return await response.Content.ReadAsStringAsync();
		}
	}
}
