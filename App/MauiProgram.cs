using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;
using MauiAuth0App.Auth0;

namespace App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {  
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddSingleton(AudioManager.Current);
        
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<Game>();


#if DEBUG
        builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddSingleton(new Auth0Client(new()
		{
			Domain = "dev-84ref6m25ippcu2o.us.auth0.com",
			ClientId = "ChftVfAPQaAIncpXAxI9ir2opzb61Srw",
			Scope = "openid profile",
			RedirectUri = "multiflap://callback",
			Audience = "161.97.97.200",
		}));

		return builder.Build();
    }
}