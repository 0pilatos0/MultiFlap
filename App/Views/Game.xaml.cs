using App.Services;
using MauiAuth0App.Auth0;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Maui.Audio;
using System;
using System.Net.Sockets;
using System.Text.Json;
using App.Models;
using App.GameCore;
using App.GameCore.GameObjects;

namespace App;

public partial class Game : ContentPage
{
	private Flappy flappy;
	private List<GreenPipe> pipes;

	private readonly HubConnection connection;
	private readonly IApiService apiService;
	private readonly Auth0Client auth0Client;

	private readonly GameEngine gameEngine;

	public Game(IApiService apiService, Auth0Client auth0Client)
	{
		InitializeComponent();

		gameEngine = new GameEngine();

		isMatchMaking.IsVisible = false;

		this.apiService = apiService;
		this.auth0Client = auth0Client;

		connection = new HubConnectionBuilder()
			//.WithUrl("http://145.49.40.171:5076/game")
			//.WithUrl("https://192.168.2.24:5076/game") //Localhost
			.WithUrl("http://161.97.97.200:5076/game") //self hosted
			.Build();

		connection.On<int>("UpdateOnlinePlayers", (onlinePlayers) =>
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				OnlinePlayersLabel.Text = $"Online Players: {onlinePlayers}";
			});
		});

		connection.On<List<PlayerMatchInfo>>("MatchStarted", (players) =>
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				isMatchMaking.IsVisible = false;

				gameEngine.OnlineMatch = true;
				gameEngine.ResetScore();
				gameEngine.IsRunning = true;

				flappy = new Flappy(gameEngine.Width / 2, gameEngine.Height / 2, Colors.Yellow);
				pipes = new List<GreenPipe>();
				pipes.Add(new GreenPipe(gameEngine.Width, 200, gameEngine.Height));
				canvas.Drawable = new GameCanvas() { Flappy = flappy, GreenPipes = pipes };
				RunGameLoop();
			});
		});

		connection.On<int>("OpponentGameOver", (opponentScore) =>
		{
			Device.BeginInvokeOnMainThread(async () =>
			{
				gameEngine.IsRunning = false;
				await DisplayAlert("Game Over", $"You won! Your score: {gameEngine.Score} Opponent score: {opponentScore}", "OK");
			});
		});

		Task.Run(async () =>
		{
			try
			{
				await connection.StartAsync();
				if (connection.State != HubConnectionState.Connected)
					throw new SocketException();
			}
			catch (Exception ex)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					MultiplayerButton.Text = "Offline";
					MultiplayerButton.IsEnabled = false;
				});
			}
		});
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		var tapGestureRecognizer = new TapGestureRecognizer();
		tapGestureRecognizer.Tapped += OnCanvasTapped;
		canvas.GestureRecognizers.Add(tapGestureRecognizer);

		gameEngine.ShakeEnabled = Preferences.Get("Shake", false);

		if (gameEngine.ShakeEnabled)
		{
			Accelerometer.ShakeDetected += OnShakeDetected;
			Accelerometer.Start(SensorSpeed.Game);
		}
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		gameEngine.IsRunning = false;

		if (gameEngine.ShakeEnabled)
		{
			Accelerometer.ShakeDetected -= OnShakeDetected;
			Accelerometer.Stop();
		}
	}

	private async void RunGameLoop()
	{
		while (gameEngine.IsRunning)
		{
			flappy.UpdatePosition();

			if (gameEngine.Score > 100)
			{
				foreach (var pipe in pipes)
				{
					pipe.UpdatePosition();
				}
			}

			if (gameEngine.CheckCollision(flappy, pipes))
			{
				GameOver(gameEngine.Score);
				return;
			}

			gameEngine.Score++;
			ScoreLabel.Text = $"Score: {gameEngine.Score}";
			
			canvas.Invalidate();
;
			await Task.Delay(TimeSpan.FromSeconds(1.0 / 45));
		}
	}

	private async void GameOver(int score)
	{
		gameEngine.IsRunning = false;
		var player2 = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gameOver.mp3"));
		player2.Play();

		if (gameEngine.OnlineMatch)
		{
			await connection.InvokeAsync("GameOver", score);
			gameEngine.OnlineMatch = false;
		}
		else
		{
			await DisplayAlert("Game Over", $"Score: {score}", "OK");

			try
			{
				LeaderboardEntry leaderboardEntry = new LeaderboardEntry { Score = score };
				string payload = JsonSerializer.Serialize(leaderboardEntry);
				string response = await apiService.PostAsync("api/leaderboard", payload, auth0Client.AccessToken);
			}
			catch (Exception e)
			{
				await DisplayAlert("Connection Error", "Your highscore could not be submitted, are you connected to the internet?", "OK");
			}
		}

		return;
	}

	private async void OnCanvasTapped(object sender, EventArgs e)
	{
		if (gameEngine.IsRunning)
		{
			var player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("jump.mp3"));
			player.Play();

			flappy.Jump();
		}
	}

	private async void OnStartClicked(object sender, EventArgs e)
	{
		gameEngine.IsRunning = true;
		gameEngine.ResetScore();
		flappy = new Flappy(gameEngine.Width / 2, gameEngine.Height / 2, Colors.Yellow);
		pipes = new List<GreenPipe>();
		pipes.Add(new GreenPipe(gameEngine.Width, 200, gameEngine.Height));
		canvas.Drawable = new GameCanvas() { Flappy = flappy, GreenPipes = pipes };
		RunGameLoop();
	}

	private async void OnShakeDetected(object sender, EventArgs e)
	{
		if (gameEngine.IsRunning)
		{
			var player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("jump.mp3"));
			player.Play();

			flappy.Jump();
		}
	}

	private async void OnStartMatchmaking(object sender, EventArgs e)
	{
		isMatchMaking.IsVisible = true;
		await connection.InvokeAsync("StartMatchmaking");
	}

	private async void OnCancelMatchmakingClicked(object sender, EventArgs e)
	{
		await connection.InvokeAsync("CancelMatchmaking");
		isMatchMaking.IsVisible = false;
	}
}
