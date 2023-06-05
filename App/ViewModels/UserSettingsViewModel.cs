using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using App.Models;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;
using App.Services;

namespace App.ViewModels
{
	public class UserSettingsViewModel : BaseViewModel
	{
		private UserSettings _userSettings;
		private readonly IApiService _apiService;
		private readonly Auth0Client _auth0Client;

		public UserSettings UserSettings
		{
			get => _userSettings;
			set
			{
				if (_userSettings != value)
				{
					_userSettings = value;
					if (_userSettings.ShakeEnabled)
					{
						Preferences.Set("Shake", true);
					}
					else
					{
						Preferences.Set("Shake", false);
					}

					OnPropertyChanged();
				}
			}
		}

		public UserSettingsViewModel(IApiService apiService, Auth0Client auth0Client)
		{
			_apiService = apiService;
			_auth0Client = auth0Client;
			UserSettings = new UserSettings();

			LoadUserSettings();
		}

		public async Task LoadUserSettings()
		{
			try
			{
				// Get the access token from the Auth0Client
				string accessToken = _auth0Client.AccessToken; // Replace with your actual method to retrieve the access token

				// Define the API endpoint URL
				string apiUrl = "api/users/settings"; // Use the API base URL from the ApiService

				// Send a GET request to the API endpoint to retrieve the user settings
				string response = await _apiService.GetAsync(apiUrl, accessToken);

				// Check if the request was successful
				if (!string.IsNullOrEmpty(response))
				{
					var options = new JsonSerializerOptions
					{
						WriteIndented = true,
						PropertyNameCaseInsensitive = true // this is the point
					};
					// Deserialize the response JSON to UserSettings object
					UserSettings = JsonSerializer.Deserialize<UserSettings>(response, options);
					Console.WriteLine("User settings loaded successfully!");
				}
				else
				{
					Console.WriteLine("Error loading user settings");
				}
			}
			catch (Exception ex)
			{
				// Handle any exception that occurred during the API request
				Console.WriteLine("An error occurred: " + ex.Message);
			}
		}

		// Command for saving the user settings
		private RelayCommand _saveCommand;
		public RelayCommand SaveCommand
		{
			get
			{
				if (_saveCommand == null)
				{
					_saveCommand = new RelayCommand(async () => await SaveUserSettings());
				}
				return _saveCommand;
			}
		}

		private async Task SaveUserSettings()
		{
			try
			{
				if (UserSettings.Language == null)
					UserSettings.Language = "English";

				// Serialize the UserSettings object to JSON
				string jsonPayload = JsonSerializer.Serialize(UserSettings);

				// Get the access token from the Auth0Client
				string accessToken = _auth0Client.AccessToken; // Replace with your actual method to retrieve the access token

				// Define the API endpoint URL
				string apiUrl = "api/users/settings"; // Use the API base URL from the ApiService

				// Send a PUT request to the API endpoint to update the user settings
				string response = await _apiService.PutAsync(apiUrl, jsonPayload, accessToken);

				// Check if the request was successful
				if (!string.IsNullOrEmpty(response))
				{
					// User settings saved successfully
					Console.WriteLine("User settings saved successfully!");
				}
				else
				{
					// Handle the error response from the API
					Console.WriteLine("Error saving user settings: " + response);
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
