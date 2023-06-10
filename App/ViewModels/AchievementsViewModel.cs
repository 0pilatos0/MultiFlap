using System.Collections.ObjectModel;
using System.Text.Json;
using App.Models;
using App.Services;
using MauiAuth0App.Auth0;
using System.Windows.Input;

namespace App.ViewModels
{
    public class AchievementsViewModel : BaseViewModel
    {
        private ObservableCollection<Achievement> _achievements;
        private readonly IApiService _apiService;
        private readonly Auth0Client _auth0Client;

        private ICommand _refreshCommand;
        private bool _isRefreshing;

        public ObservableCollection<Achievement> Achievements
        {
            get => _achievements;
            set
            {
                if (_achievements != value)
                {
                    _achievements = value;
                    OnPropertyChanged(nameof(HasItems));
                    OnPropertyChanged(nameof(HasNoItems));
                    OnPropertyChanged(nameof(Achievements));
                    OnPropertyChanged();
                }
            }
        }

        public bool HasItems => Achievements.Count > 0;
        public bool HasNoItems => !HasItems;

        public AchievementsViewModel(IApiService apiService, Auth0Client auth0Client)
        {
            _apiService = apiService;
            _auth0Client = auth0Client;
            Achievements = new ObservableCollection<Achievement>();

            LoadAchievements();
        }

        public async Task LoadAchievements()
        {
            try
            {
                // Define the API endpoint URL
                string apiUrl = "api/achievements"; // Use the appropriate API endpoint

                // Send a GET request to the API endpoint to retrieve the achievements data
                string response = await _apiService.GetAsync(apiUrl, _auth0Client.AccessToken);

                // Check if the request was successful
                if (!string.IsNullOrEmpty(response))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize the response JSON to a list of Achievement objects
                    var newAchievements = JsonSerializer.Deserialize<List<Achievement>>(
                        response,
                        options
                    );

                    // Clear the existing achievements and add the new ones
                    Achievements.Clear();
                    foreach (var achievement in newAchievements)
                    {
                        Achievements.Add(achievement);
                    }

                    Console.WriteLine("Achievements loaded successfully!");

                    OnPropertyChanged(nameof(HasItems));
                    OnPropertyChanged(nameof(HasNoItems));
                }
                else
                {
                    Console.WriteLine("Error loading achievements");
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the API request
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public ICommand RefreshCommand =>
            _refreshCommand ??= new Command(async () =>
            {
                IsRefreshing = true;
                await LoadAchievements();
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
