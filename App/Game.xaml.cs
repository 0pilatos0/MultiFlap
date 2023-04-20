using App.GameObjects;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Maui.Audio;
using System.Net.Sockets;

namespace App;

public partial class Game : ContentPage
{
	private bool _isRunning;
	private Flappy _flappy;
	private List<GreenPipe> _pipes;
	private int _score;
	private int _width = 350;
	private int _height = 500;

	private readonly HubConnection _connection;

	public Game()
	{
		InitializeComponent();

		_connection = new HubConnectionBuilder()
			//.WithUrl("http://145.49.40.171:5076/game")
			.WithUrl("http://192.168.2.24:5076/game")
			//.WithUrl("http://161.97.97.200:5076/game")
			.Build();

		_connection.On<int>("UpdateOnlinePlayers", (onlinePlayers) =>
		{
			Task.Run(() =>
			{
				Dispatcher.Dispatch(async () =>
				{
					OnlinePlayersLabel.Text = $"Online Players: {onlinePlayers}";
				});
			});
		});

		Task.Run(async () =>
		{
			try
			{
				await _connection.StartAsync();
				//if connection couldnt be made throw exception
				if (_connection.State != HubConnectionState.Connected)
					throw new SocketException();
			}
			catch (Exception ex)
			{
				Dispatcher.Dispatch(() =>
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
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		_isRunning = false;
	}

	private async void RunGameLoop()
	{
		while (_isRunning)
		{
			_flappy.UpdatePosition();

			if (_score > 100)
			{
				foreach (var pipe in _pipes)
				{
					pipe.UpdatePosition();
				}
			}

			foreach (var pipe in _pipes)
			{
				// Check for collision
				if (_flappy.X + 20 > pipe.X && _flappy.X - 20 < pipe.X + 100)
				{
					if (_flappy.Y - 20 < pipe.TopHeight || _flappy.Y + 20 > pipe.TopHeight + pipe.GapSize)
					{
						_isRunning = false;
						var player2 = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gameOver.mp3"));
						player2.Play();
						await DisplayAlert("Game Over", $"Score: {_score}", "OK");
						return;
					}
				}
			}

			if (_flappy.Y < 0 || _flappy.Y > _height)
			{
				_isRunning = false;
				var player2 = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gameOver.mp3"));
				player2.Play();
				await DisplayAlert("Game Over", $"Score: {_score}", "OK");
				return;
			}

			_score++;
			ScoreLabel.Text = $"Score: {_score}";

			canvas.Invalidate();

			await Task.Delay(TimeSpan.FromSeconds(1.0 / 45));
		}
	}

	private async void OnCanvasTapped(object sender, EventArgs e)
	{
		if (_isRunning)
		{
			var player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("jump.mp3"));
			player.Play();

			_flappy.Jump();
		}
	}

	private async void OnStartClicked(object sender, EventArgs e)
	{
		_isRunning = true;
		_score = 0;
		_flappy = new Flappy(_width / 2, _height / 2, Colors.Yellow);
		_pipes = new List<GreenPipe>();
		_pipes.Add(new GreenPipe(_width, 200, _height));
		canvas.Drawable = new GameCanvas() { flappy = _flappy, _greenPipes = _pipes };
		RunGameLoop();
	}


	private async void OnStartMatchmaking(object sender, EventArgs e)
	{
		await _connection.InvokeAsync("StartMatchmaking");
	}
}
