using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using App.Services;
using Moq;
using Xunit;

namespace AppTests.Services
{
	public class ApiServiceTests
	{
		private readonly Mock<IApiService> apiServiceMock;
		private readonly ApiServiceWrapper apiServiceWrapper;

		public ApiServiceTests()
		{
			apiServiceMock = new Mock<IApiService>();
			apiServiceWrapper = new ApiServiceWrapper(apiServiceMock.Object);
		}

		[Fact]
		public async Task GetAsync_ReturnsResponseContent()
		{
			// Arrange
			var endpoint = "example";
			var accessToken = "token";
			var expectedResponse = "Response content";
			apiServiceMock.Setup(s => s.GetAsync(endpoint, accessToken)).ReturnsAsync(expectedResponse);

			// Act
			var result = await apiServiceWrapper.GetAsync(endpoint, accessToken);

			// Assert
			Assert.Equal(expectedResponse, result);
			apiServiceMock.VerifyAll();
		}

		[Fact]
		public async Task PostAsync_ReturnsResponseContent()
		{
			// Arrange
			var endpoint = "example";
			var body = "Request body";
			var accessToken = "token";
			var expectedResponse = "Response content";
			apiServiceMock.Setup(s => s.PostAsync(endpoint, body, accessToken)).ReturnsAsync(expectedResponse);

			// Act
			var result = await apiServiceWrapper.PostAsync(endpoint, body, accessToken);

			// Assert
			Assert.Equal(expectedResponse, result);
			apiServiceMock.VerifyAll();
		}

		[Fact]
		public async Task PutAsync_ReturnsResponseContent()
		{
			// Arrange
			var endpoint = "example";
			var body = "Request body";
			var accessToken = "token";
			var expectedResponse = "Response content";
			apiServiceMock.Setup(s => s.PutAsync(endpoint, body, accessToken)).ReturnsAsync(expectedResponse);

			// Act
			var result = await apiServiceWrapper.PutAsync(endpoint, body, accessToken);

			// Assert
			Assert.Equal(expectedResponse, result);
			apiServiceMock.VerifyAll();
		}

		[Fact]
		public async Task DeleteAsync_ReturnsResponseContent()
		{
			// Arrange
			var endpoint = "example";
			var accessToken = "token";
			var expectedResponse = "Response content";
			apiServiceMock.Setup(s => s.DeleteAsync(endpoint, accessToken)).ReturnsAsync(expectedResponse);

			// Act
			var result = await apiServiceWrapper.DeleteAsync(endpoint, accessToken);

			// Assert
			Assert.Equal(expectedResponse, result);
			apiServiceMock.VerifyAll();
		}

		[Fact]
		public async Task PatchAsync_ReturnsResponseContent()
		{
			// Arrange
			var endpoint = "example";
			var body = "Request body";
			var accessToken = "token";
			var expectedResponse = "Response content";
			apiServiceMock.Setup(s => s.PatchAsync(endpoint, body, accessToken)).ReturnsAsync(expectedResponse);

			// Act
			var result = await apiServiceWrapper.PatchAsync(endpoint, body, accessToken);

			// Assert
			Assert.Equal(expectedResponse, result);
			apiServiceMock.VerifyAll();
		}
	}

	public class ApiServiceWrapper : IApiService
	{
		private readonly IApiService apiService;

		public ApiServiceWrapper(IApiService apiService)
		{
			this.apiService = apiService;
		}

		public string ApiUrl => apiService.ApiUrl;

		public Task<string> GetAsync(string endpoint, string accessToken)
		{
			return apiService.GetAsync(endpoint, accessToken);
		}

		public Task<string> PostAsync(string endpoint, string body, string accessToken)
		{
			return apiService.PostAsync(endpoint, body, accessToken);
		}

		public Task<string> PutAsync(string endpoint, string body, string accessToken)
		{
			return apiService.PutAsync(endpoint, body, accessToken);
		}

		public Task<string> DeleteAsync(string endpoint, string accessToken)
		{
			return apiService.DeleteAsync(endpoint, accessToken);
		}

		public Task<string> PatchAsync(string endpoint, string body, string accessToken)
		{
			return apiService.PatchAsync(endpoint, body, accessToken);
		}
	}
}
