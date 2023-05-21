using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using App.Services;
using MauiAuth0App.Auth0;

namespace App.ViewModels
{
	public class LeaderboardViewModel : BaseViewModel
	{
		private ObservableCollection<LeaderboardEntryDTO> _leaderboardEntries;
		private readonly IApiService _apiService;
		private readonly Auth0Client _auth0Client;

		public ObservableCollection<LeaderboardEntryDTO> LeaderboardEntries
		{
			get => _leaderboardEntries;
			set
			{
				if (_leaderboardEntries != value)
				{
					_leaderboardEntries = value;
					OnPropertyChanged();
				}
			}
		}

		public LeaderboardViewModel(IApiService apiService, Auth0Client auth0Client)
		{
			_apiService = apiService;
			_auth0Client = auth0Client;
			LeaderboardEntries = new ObservableCollection<LeaderboardEntryDTO>();

			LoadLeaderboard();
		}

		private async Task LoadLeaderboard()
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
					// Deserialize the response JSON to a list of LeaderboardEntry objects
					LeaderboardEntries = JsonSerializer.Deserialize<ObservableCollection<LeaderboardEntryDTO>>(response);
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
		}
	}
}


