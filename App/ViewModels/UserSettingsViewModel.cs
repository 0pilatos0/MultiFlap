using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json;
using App.Models;
using CommunityToolkit.Mvvm.Input;
using MauiAuth0App.Auth0;

namespace App.ViewModels
{
	public class UserSettingsViewModel : BaseViewModel
	{
		private UserSettings _userSettings;
		private Auth0Client auth0Client;

		public UserSettings UserSettings
		{
			get => _userSettings;
			set
			{
				if (_userSettings != value)
				{
					_userSettings = value;
					OnPropertyChanged();
				}
			}
		}

		public UserSettingsViewModel(Auth0Client auth0Client)
		{
			// Initialize the UserSettings instance
			this.auth0Client = auth0Client;
			UserSettings = new UserSettings();
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
				// Create an instance of HttpClient
				using (HttpClient httpClient = new HttpClient())
				{
					// Set the access token in the Authorization header
					string accessToken = auth0Client.AccessToken; // Replace with your actual method to retrieve the access token
					httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

					// Define the API endpoint URL
					string apiUrl = "https://de68-81-206-192-243.ngrok-free.app/"; // Replace with your actual API endpoint

					// Create a JSON payload with the updated user settings

					string jsonPayload = JsonSerializer.Serialize(UserSettings);

					// Create a StringContent object with the JSON payload
					var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

					// Send a PUT request to the API endpoint to update the user settings
					HttpResponseMessage response = await httpClient.PutAsync(apiUrl, content);

					// Check if the request was successful
					if (response.IsSuccessStatusCode)
					{
						// User settings saved successfully
						Console.WriteLine("User settings saved successfully!");
					}
					else
					{
						// Handle the error response from the API
						string errorMessage = await response.Content.ReadAsStringAsync();
						Console.WriteLine("Error saving user settings: " + errorMessage);
					}
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
