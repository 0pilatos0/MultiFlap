using App.Services;
using MauiAuth0App.Auth0;

namespace App.Views;

public partial class LoginPage : ContentPage
{
    private readonly Auth0Client auth0Client;

    public LoginPage(Auth0Client client)
    {
        InitializeComponent();
        auth0Client = client;
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        if (Preferences.Get("IsLoggedIn", false))
        {
            await LogoutAsync();
        }
        else
        {
            await LoginAsync();
        }
    }

    private async Task LoginAsync()
    {
        LoginButton.IsEnabled = false;
        LoggingIn.IsVisible = true;

        var loggingInResult = await auth0Client.LoginAsync();

        LoggingIn.IsVisible = false;

        if (loggingInResult.IsError)
        {
            await App.Current.MainPage.DisplayAlert(
                "Error",
                "Something went wrong logging you in. Please try again.",
                "OK"
            );
            LoginButton.IsEnabled = true;
            LoggingIn.IsVisible = false;
        }
        else // user is logged in
        {
            await Navigation.PopToRootAsync();
        }
    }

    private async Task LogoutAsync()
    {
        LoginButton.IsEnabled = false;
        LoggingIn.IsVisible = true;

        var loggingOutResult = await auth0Client.LogoutAsync();

        LoggingIn.IsVisible = false;

        if (loggingOutResult.IsError)
        {
            await App.Current.MainPage.DisplayAlert(
                "Error",
                "Something went wrong logging you out. Please try again.",
                "OK"
            );
            LoginButton.IsEnabled = true;
            LoggingIn.IsVisible = false;
        }
        else // user is logged out
        {
            Preferences.Clear();
            Preferences.Set("IsLoggedIn", false);
            Preferences.Set("UserName", String.Empty);
            await Navigation.PopModalAsync();
        }
    }
}
