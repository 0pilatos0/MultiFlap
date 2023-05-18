using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using MauiAuth0App.Auth0;
using CommunityToolkit.Maui;
using App.Views;
using App.ViewModels;
using App.Services;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {  
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

		


#if DEBUG
        builder.Logging.AddDebug();
#endif
        //Pages
		builder.Services.AddTransient<MainPage>();
		builder.Services.AddTransient<Game>();
		builder.Services.AddTransient<LoginPage>();
		builder.Services.AddTransient<EditUserSettings>();

		//ViewModels
		builder.Services.AddSingleton<UserSettingsViewModel>();

		//Services
		builder.Services.AddSingleton<IApiService, ApiService>();
		builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton<IPreferences>(Preferences.Default);
		builder.Services.AddSingleton(new Auth0Client(new()
		{
			Domain = "dev-84ref6m25ippcu2o.us.auth0.com",
			ClientId = "ChftVfAPQaAIncpXAxI9ir2opzb61Srw",
			Scope = "openid profile",
			RedirectUri = "multiflap://callback",
			Audience = "161.97.97.200",
		}, Preferences.Default));

		return builder.Build();
    }
}