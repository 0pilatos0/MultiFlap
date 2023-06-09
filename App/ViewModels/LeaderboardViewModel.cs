﻿using System.Collections.ObjectModel;
using System.Text.Json;
using App.Services;
using App.Auth0;
using App.Models;
using System.Windows.Input;

namespace App.ViewModels
{
    public class LeaderboardViewModel : BaseViewModel
    {
        private ObservableCollection<LeaderboardEntry> _leaderboardEntries;
        private readonly IApiService _apiService;
        private readonly Auth0Client _auth0Client;

        private ICommand _refreshCommand;
        private bool _isRefreshing;

        public ObservableCollection<LeaderboardEntry> LeaderboardEntries
        {
            get => _leaderboardEntries;
            set
            {
                if (_leaderboardEntries != value)
                {
                    _leaderboardEntries = value;
                    OnPropertyChanged(nameof(HasItems));
                    OnPropertyChanged(nameof(HasNoItems));
                    OnPropertyChanged(nameof(LeaderboardEntries));
                    OnPropertyChanged();
                }
            }
        }

        public bool HasItems => LeaderboardEntries.Count > 0;
        public bool HasNoItems => !HasItems;

        public LeaderboardViewModel(IApiService apiService, Auth0Client auth0Client)
        {
            _apiService = apiService;
            _auth0Client = auth0Client;
            LeaderboardEntries = new ObservableCollection<LeaderboardEntry>();

            LoadLeaderboard();
        }

        public async Task LoadLeaderboard()
        {
            try
            {
                // Define the API endpoint URL
                string apiUrl = "api/leaderboard"; // Use the appropriate API endpoint

                // Send a GET request to the API endpoint to retrieve the leaderboard data
                string response = await _apiService.GetAsync(apiUrl, _auth0Client.AccessToken);

                // Check if the request was successful
                if (!string.IsNullOrEmpty(response))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true // this is the point
                    };

                    // Deserialize the response JSON to a list of LeaderboardEntry objects
                    LeaderboardEntries = JsonSerializer.Deserialize<
                        ObservableCollection<LeaderboardEntry>
                    >(response, options);
                    Console.WriteLine("Leaderboard loaded successfully!");
                }
                else
                {
                    Console.WriteLine("Error loading leaderboard");
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the API request
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            OnPropertyChanged(nameof(HasItems));
            OnPropertyChanged(nameof(HasNoItems));
        }

        public ICommand RefreshCommand =>
            _refreshCommand ??= new Command(async () =>
            {
                IsRefreshing = true;
                await LoadLeaderboard();
                IsRefreshing = false;
            });

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing != value)
                {
                    _isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
