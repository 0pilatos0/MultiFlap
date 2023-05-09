using IdentityModel.Client;
using IdentityModel.OidcClient;
using IdentityModel.OidcClient.Browser;

namespace MauiAuth0App.Auth0;

public class Auth0Client
{
	private readonly OidcClient oidcClient;
	private string audience;

	readonly IPreferences preferences;

	public bool IsAuthenticated => preferences.Get<bool>(nameof(IsAuthenticated), false);
	public string CurrentUser => preferences.Get<string>(nameof(CurrentUser), string.Empty);
	public string AccessToken => preferences.Get<string>(nameof(AccessToken), string.Empty);

	public Auth0Client(Auth0ClientOptions options, IPreferences preferences)
	{
		oidcClient = new OidcClient(new OidcClientOptions
		{
			Authority = $"https://{options.Domain}",
			ClientId = options.ClientId,
			Scope = options.Scope,
			RedirectUri = options.RedirectUri,
			Browser = options.Browser
		});

		audience = options.Audience;
		this.preferences = preferences;
	}

	public IdentityModel.OidcClient.Browser.IBrowser Browser
	{
		get
		{
			return oidcClient.Options.Browser;
		}
		set
		{
			oidcClient.Options.Browser = value;
		}
	}

	public async Task<LoginResult> LoginAsync()
	{
		LoginRequest loginRequest = null;

		if (!string.IsNullOrEmpty(audience))
		{
			loginRequest = new LoginRequest
			{
				FrontChannelExtraParameters = new Parameters(new Dictionary<string, string>()
			{
			  {"audience", audience}
			})
			};
		}
		var loginResult = await oidcClient.LoginAsync(loginRequest);

		if (!loginResult.IsError)
		{
			preferences.Set(nameof(IsAuthenticated), true);
			preferences.Set(nameof(CurrentUser), loginResult.User.Identity.Name);
			preferences.Set(nameof(AccessToken), loginResult.AccessToken);
		}

		return loginResult;
	}

	public async Task<BrowserResult> LogoutAsync()
	{
		var logoutParameters = new Dictionary<string, string>
	{
	  {"client_id", oidcClient.Options.ClientId },
	  {"returnTo", oidcClient.Options.RedirectUri }
	};

		var logoutRequest = new LogoutRequest();
		var endSessionUrl = new RequestUrl($"{oidcClient.Options.Authority}/v2/logout")
		  .Create(new Parameters(logoutParameters));
		var browserOptions = new BrowserOptions(endSessionUrl, oidcClient.Options.RedirectUri)
		{
			Timeout = TimeSpan.FromSeconds(logoutRequest.BrowserTimeout),
			DisplayMode = logoutRequest.BrowserDisplayMode
		};

		var browserResult = await oidcClient.Options.Browser.InvokeAsync(browserOptions);

		return browserResult;
	}
}