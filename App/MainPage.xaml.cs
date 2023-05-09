using Microsoft.AspNetCore.SignalR.Client;
using MauiAuth0App.Auth0;
using System.Net.Http.Headers;

namespace App
{
    public partial class MainPage : ContentPage
	{
		private readonly Auth0Client auth0Client;
		private string accessToken;

		public MainPage(Auth0Client client)
		{
			InitializeComponent();
			auth0Client = client;

		}

		private async void OnLoginClicked(object sender, EventArgs e)
		{
			var loginResult = await auth0Client.LoginAsync();

			if (!loginResult.IsError)
			{
				LoginView.IsVisible = false;
				HomeView.IsVisible = true;

				accessToken = loginResult.AccessToken;
			}
			else
			{
				await DisplayAlert("Error", loginResult.ErrorDescription, "OK");
			}
		}

		private void StartGameClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Game());
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
		{
			var logoutResult = await auth0Client.LogoutAsync();

			if (!logoutResult.IsError)
			{
				HomeView.IsVisible = false;
				LoginView.IsVisible = true;
			}
			else
			{
				await DisplayAlert("Error", logoutResult.ErrorDescription, "OK");
			}
		}

		private async void OnApiCallClicked(object sender, EventArgs e)
		{
			using (var httpClient = new HttpClient())
			{
				string ApiUrl = "http://161.97.97.200:5076/WeatherForecast";
				httpClient.DefaultRequestHeaders.Authorization
							 = new AuthenticationHeaderValue("Bearer", accessToken);
				try
				{
					HttpResponseMessage response = await httpClient.GetAsync(ApiUrl);
					{
						string content = await response.Content.ReadAsStringAsync();
						await DisplayAlert("Info", content, "OK");
					}
				}
				catch (Exception ex)
				{
					await DisplayAlert("Error", ex.Message, "OK");
				}
			}
		}
	}
}
