using App.Services;
using App.Auth0;
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
    private readonly HubConnection connection;
    private readonly IApiService apiService;
    private readonly Auth0Client auth0Client;

    private readonly GameEngine gameEngine;

    private Flappy flappy;
    private List<GreenPipe> pipes;
    private PowerUpItem activatedPowerUp;

    [Obsolete]
    public Game(IApiService apiService, Auth0Client auth0Client)
    {
        InitializeComponent();

        gameEngine = new GameEngine();

        isMatchMaking.IsVisible = false;
        isCountDown.IsVisible = false;

        this.apiService = apiService;
        this.auth0Client = auth0Client;

        LoadActivatedPowerUpFromPreferences();
        LoadSoundEnabledFromPreferences();

        connection = new HubConnectionBuilder()
            //.WithUrl("http://145.49.40.171:5076/game")
            //.WithUrl("https://192.168.2.24:5076/game") //Localhost
            .WithUrl("http://161.97.97.200:5076/game") //self hosted
            .Build();

        connection.On<int>(
            "UpdateOnlinePlayers",
            (onlinePlayers) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    OnlinePlayersLabel.Text = $"Online Players: {onlinePlayers}";
                });
            }
        );

        connection.On<List<PlayerMatchInfo>>(
            "MatchStarted",
            (players) =>
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
            }
        );

        connection.On<int>(
            "OpponentGameOver",
            (opponentScore) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    gameEngine.IsRunning = false;
                    await DisplayAlert(
                        "Game Over",
                        $"You won! Your score: {gameEngine.Score} Opponent score: {opponentScore}",
                        "OK"
                    );
                });
            }
        );

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

        connection.InvokeAsync("OnAppearing");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        gameEngine.IsRunning = false;

        //dis

        if (gameEngine.ShakeEnabled)
        {
            Accelerometer.ShakeDetected -= OnShakeDetected;
            Accelerometer.Stop();
        }
    }

    private async void OnCanvasTapped(object sender, EventArgs e)
    {
        if (gameEngine.IsRunning)
        {
            if (gameEngine.SoundEnabled)
            {
                var player = AudioManager.Current.CreatePlayer(
                    await FileSystem.OpenAppPackageFileAsync("jump.mp3")
                );
                player.Play();
            }

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
            var player = AudioManager.Current.CreatePlayer(
                await FileSystem.OpenAppPackageFileAsync("jump.mp3")
            );
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

    private void RemoveActivatedPowerUp()
    {
        activatedPowerUp = null;
        gameEngine.ScoreMultiplier = 1.0f;
        Preferences.Remove("ActivatedPowerUp");
    }

    private void LoadActivatedPowerUpFromPreferences()
    {
        string powerUpJson = Preferences.Get("ActivatedPowerUp", null);
        if (!string.IsNullOrEmpty(powerUpJson))
        {
            activatedPowerUp = JsonSerializer.Deserialize<PowerUpItem>(powerUpJson);
            // Apply the score multiplier if the power-up is set
            if (activatedPowerUp != null)
            {
                //if name is "1.05 Multiplier" then multiplier is 1.05
                gameEngine.ScoreMultiplier = float.Parse(activatedPowerUp.Name.Split(" ")[0]);
            }
        }
    }

    private void LoadSoundEnabledFromPreferences()
    {
        bool soundEnabled = Preferences.Get("Sound", true);
        gameEngine.SoundEnabled = soundEnabled;
    }

    private async void RunGameLoop()
    {
        isCountDown.IsVisible = true;

        // Countdown interval
        for (int count = 5; count > 0; count--)
        {
            // Update countdown label
            CountdownLabel.Text = count.ToString();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }

        // Hide the matchmaking overlay
        isCountDown.IsVisible = false;

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

            //gameEngine.Score++ with the multiplier rounded to whole int
            gameEngine.Score += (int)Math.Round(gameEngine.ScoreMultiplier);
            ScoreLabel.Text = $"Score: {gameEngine.Score}";

            canvas.Invalidate();
            ;
            await Task.Delay(TimeSpan.FromSeconds(1.0 / 45));
        }
    }

    private async void GameOver(int score)
    {
        //clear canvas
        canvas.Drawable = null;

        RemoveActivatedPowerUp();
        gameEngine.IsRunning = false;

        if (gameEngine.SoundEnabled)
        {
            var player2 = AudioManager.Current.CreatePlayer(
                await FileSystem.OpenAppPackageFileAsync("gameOver.mp3")
            );
            player2.Play();
        }

        if (gameEngine.OnlineMatch)
        {
            await connection.InvokeAsync("GameOver", score);
            gameEngine.OnlineMatch = false;
            await DisplayAlert("Game Over", $"Score: {score}", "OK");
        }
        else
        {
            await DisplayAlert("Game Over", $"Score: {score}", "OK");

            try
            {
                LeaderboardEntry leaderboardEntry = new LeaderboardEntry { Score = score };
                string payload = JsonSerializer.Serialize(leaderboardEntry);
                string response = await apiService.PostAsync(
                    "api/leaderboard",
                    payload,
                    auth0Client.AccessToken
                );
            }
            catch (Exception e)
            {
                await DisplayAlert(
                    "Connection Error",
                    "Your highscore could not be submitted, are you connected to the internet?",
                    "OK"
                );
            }
        }

        return;
    }
}
