using Microsoft.AspNetCore.SignalR.Client;
using MauiAuth0App.Auth0;

namespace App
{
    public partial class MainPage : ContentPage
	{
		private readonly Auth0Client auth0Client;

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
    }
}
