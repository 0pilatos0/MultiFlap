using Microsoft.AspNetCore.SignalR.Client;
using MauiAuth0App.Auth0;
using System.Net.Http.Headers;
using App.Views;
using App.Models;
using App.Services;

namespace App
{
    public partial class MainPage : ContentPage
    {
        private readonly Auth0Client _auth0Client;
        private readonly IApiService _apiService;
        private string accessToken;
		private EditUserSettings editUserSettings;
		private Leaderboard leaderboard;

		public MainPage(Auth0Client client, IApiService apiService, EditUserSettings editUserSettingsPage, Leaderboard leaderboardPage)

		{
            InitializeComponent();
            _auth0Client = client;
			_apiService = apiService;
			this.editUserSettings = editUserSettingsPage;
			this.leaderboard = leaderboardPage;
		}

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            if (!_auth0Client.IsAuthenticated)
            {
                await Navigation.PushAsync(new LoginPage(_auth0Client));
            }
        }

        private void StartGameClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Game(_apiService, _auth0Client));
        }

		private async void OnLeaderboardClicked(object sender, EventArgs e)
		{
            await Navigation.PushAsync(leaderboard);
		}

		private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var logoutResult = await _auth0Client.LogoutAsync();

            if (!logoutResult.IsError)
            {
                await Navigation.PushAsync(new LoginPage(_auth0Client));
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
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );
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

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(editUserSettings);
        }

	}
}
