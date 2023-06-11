using Microsoft.AspNetCore.SignalR.Client;
using App.Auth0;
using System.Net.Http.Headers;
using App.Views;
using App.Models;
using App.Services;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace App
{
    public partial class MainPage : ContentPage
    {
        private readonly Auth0Client _auth0Client;
        private readonly IApiService _apiService;
        private string accessToken;
        private EditUserSettings editUserSettings;
        private Leaderboard leaderboard;
        private Achievements achievements;
        private PowerUps powerUps;
        private Game game;

        public MainPage(
            Auth0Client client,
            IApiService apiService,
            EditUserSettings editUserSettingsPage,
            Leaderboard leaderboardPage,
            Achievements achievement,
            PowerUps powerUps,
            Game game
            
        )
        {
            InitializeComponent();
            _auth0Client = client;
            _apiService = apiService;
            this.editUserSettings = editUserSettingsPage;
            this.leaderboard = leaderboardPage;
            this.achievements = achievement;
            this.powerUps = powerUps;
            this.game = game;


            SetHighScore();
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
            Navigation.PushAsync(game);
        }

        private async void OnLeaderboardClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(leaderboard);
        }

        private async void OnAchievementsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(achievements);
        }
        private async void OnLoadoutClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(powerUps);
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

        private async void SetHighScore()
        {
            var _highScore = "0";
            try
            {
                string apiUrl = "api/leaderboard/me";
                string response = await _apiService.GetAsync(apiUrl, _auth0Client.AccessToken);
                if (!string.IsNullOrEmpty(response))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true // this is the point
                    };

                    _highScore = JsonSerializer.Deserialize<int>(response, options).ToString();
                }
                else
                {
                    Console.WriteLine("Error loading leaderboard");
                    _highScore = "0";
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the API request
                Console.WriteLine("An error occurred: " + ex.Message);
                _highScore = "0";
            }

            _ = Task.Run(() =>
            {
                Dispatcher.Dispatch(async () =>
                {
                    highScore.Text = _highScore;
                });
            });
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(editUserSettings);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            SetHighScore();
        }
    }
}
