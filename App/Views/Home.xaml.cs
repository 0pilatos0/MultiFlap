using Microsoft.AspNetCore.SignalR.Client;
using MauiAuth0App.Auth0;
using System.Net.Http.Headers;
using App.Views;
using App.Models;

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

        protected async override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);
            if (!auth0Client.IsAuthenticated)
            {
                await Navigation.PushAsync(new LoginPage(auth0Client));
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
                await Navigation.PushAsync(new LoginPage(auth0Client));
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
            await Navigation.PushAsync(new EditUserSettings());
        }

	}
}
