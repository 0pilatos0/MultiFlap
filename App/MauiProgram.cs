﻿using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using App.Auth0;
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
        builder.Services.AddTransient<Leaderboard>();
        builder.Services.AddTransient<Achievements>();
        builder.Services.AddTransient<PowerUps>();

        //ViewModels
        builder.Services.AddSingleton<UserSettingsViewModel>();
        builder.Services.AddSingleton<LeaderboardViewModel>();
        builder.Services.AddSingleton<AchievementsViewModel>();
        builder.Services.AddSingleton<PowerUpsViewModel>();

        //Services
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton(AudioManager.Current);
        builder.Services.AddSingleton<IPreferences>(Preferences.Default);
        builder.Services.AddSingleton(
            new Auth0Client(
                new()
                {
                    Domain = "dev-84ref6m25ippcu2o.us.auth0.com",
                    ClientId = "ChftVfAPQaAIncpXAxI9ir2opzb61Srw",
                    Scope = "openid profile",
                    RedirectUri = "multiflap://callback",
                    Audience = "161.97.97.200",
                },
                Preferences.Default
            )
        );

        return builder.Build();
    }
}
