using Java.Util.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services
{
	internal interface IApiService
	{
		string ApiUrl { get; }

		Task<string> GetAsync(string endpoint, string accessToken);
		Task<string> PostAsync(string endpoint, string body, string accessToken);
		Task<string> PutAsync(string endpoint, string body, string accessToken);
		Task<string> DeleteAsync(string endpoint, string accessToken);
		Task<string> PatchAsync(string endpoint, string body, string accessToken);
	}
}
