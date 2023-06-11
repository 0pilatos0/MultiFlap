using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using App.Models;
using App.Services;
using App.Auth0;
using System.Windows.Input;

namespace App.ViewModels
{
    public class PowerUpsViewModel : BaseViewModel
    {
        private ObservableCollection<PowerUpItem> _powerUps;
        private PowerUpItem _selectedPowerUp;
        private readonly IApiService _apiService;
        private readonly Auth0Client _auth0Client;

        private ICommand _activateCommand;
        private bool _isRefreshing;

        public ObservableCollection<PowerUpItem> PowerUps
        {
            get => _powerUps;
            set
            {
                if (_powerUps != value)
                {
                    _powerUps = value;
                    OnPropertyChanged(nameof(HasItems));
                    OnPropertyChanged(nameof(HasNoItems));
                    OnPropertyChanged(nameof(PowerUps));
                    OnPropertyChanged();
                }
            }
        }

        public PowerUpItem SelectedPowerUp
        {
            get => _selectedPowerUp;
            set
            {
                if (_selectedPowerUp != value)
                {
                    _selectedPowerUp = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasItems => PowerUps.Count > 0;
        public bool HasNoItems => !HasItems;

        public PowerUpsViewModel(IApiService apiService, Auth0Client auth0Client)
        {
            _apiService = apiService;
            _auth0Client = auth0Client;
            PowerUps = new ObservableCollection<PowerUpItem>();

            LoadPowerUps();
        }

        public async Task LoadPowerUps()
        {
            try
            {
                // Define the API endpoint URL
                string apiUrl = "api/powerups"; // Use the appropriate API endpoint

                // Send a GET request to the API endpoint to retrieve the power-ups data
                string response = await _apiService.GetAsync(apiUrl, _auth0Client.AccessToken);

                // Check if the request was successful
                if (!string.IsNullOrEmpty(response))
                {
                    var options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize the response JSON to a list of PowerUpItem objects
                    var newPowerUps = JsonSerializer.Deserialize<List<PowerUpItem>>(
                        response,
                        options
                    );

                    // Clear the existing power-ups and add the new ones
                    PowerUps.Clear();
                    foreach (var powerUp in newPowerUps)
                    {
                        PowerUps.Add(powerUp);
                    }

                    Console.WriteLine("Power-ups loaded successfully!");

                    OnPropertyChanged(nameof(HasItems));
                    OnPropertyChanged(nameof(HasNoItems));
                }
                else
                {
                    Console.WriteLine("Error loading power-ups");
                }
            }
            catch (Exception ex)
            {
                // Handle any exception that occurred during the API request
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        public ICommand ActivateCommand =>
            _activateCommand ??= new Command(async () =>
            {
                if (SelectedPowerUp != null)
                {
                    try
                    {
                        // Define the API endpoint URL for deleting a power-up
                        string apiUrl = $"api/powerups/{SelectedPowerUp.Id}"; // Use the appropriate API endpoint

                        // Send a DELETE request to the API endpoint to consume the power-up
                        String response = await _apiService.DeleteAsync(
                            apiUrl,
                            _auth0Client.AccessToken
                        );

                        Preferences.Set("ActivePowerUp", SelectedPowerUp.Name);

                        // Remove the consumed power-up from the collection
                        PowerUps.Remove(SelectedPowerUp);

                        Console.WriteLine("Power-up activated and consumed successfully!");

                        Console.WriteLine("Error consuming power-up");
                    }
                    catch (Exception ex)
                    {
                        // Handle any exception that occurred during the API request
                        Console.WriteLine("An error occurred: " + ex.Message);
                    }
                }
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
