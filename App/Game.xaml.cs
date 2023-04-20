using App.GameObjects;
using Microsoft.AspNetCore.SignalR.Client;
using Plugin.Maui.Audio;
using System.Net.Sockets;

namespace App;

public partial class Game : ContentPage
{
	private bool isRunning;
	private Flappy flappy;
	private List<GreenPipe> pipes;

	int _width = 350;
	int _height = 500;
	private int score;

	private readonly HubConnection _connection;
	
	public Game()
	{
		InitializeComponent();

			_connection = new HubConnectionBuilder()
            .WithUrl("http://145.49.40.171:5076/game")
            //.WithUrl("http://192.168.2.24:5076/game")
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
				await _connection.InvokeAsync("OnConnectedAsync");
			}
			catch
			{
				Dispatcher.Dispatch(() => 
				{
					MultiplayerButton.Text = "Offline";
					MultiplayerButton.IsEnabled = false;
				});
			}
		});

		var tapGestureRecognizer = new TapGestureRecognizer();
		tapGestureRecognizer.Tapped += OnCanvasTapped;
		canvas.GestureRecognizers.Add(tapGestureRecognizer);
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
	}

	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		isRunning = false;
	}

	private async void RunGameLoop()
	{
		while (isRunning)
		{
			flappy.UpdatePosition();

			if (score > 100)
			{
				foreach (var pipe in pipes)
				{
					pipe.UpdatePosition();
				}
			}

			foreach (var pipe in pipes)
			{
				// Check for collision
				if (flappy.X + 20 > pipe.X && flappy.X - 20 < pipe.X + 100)
				{
					if (flappy.Y - 20 < pipe.TopHeight || flappy.Y + 20 > pipe.TopHeight + pipe.GapSize)
					{
						isRunning = false;
						var player2 = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gameOver.mp3"));
						player2.Play();
						await DisplayAlert("Game Over", $"Score: {score}", "OK");
						return;
					}
				}
			}

			if (flappy.Y < 0 || flappy.Y > _height)
			{
				isRunning = false;
				var player2 = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("gameOver.mp3"));
				player2.Play();
				await DisplayAlert("Game Over", $"Score: {score}", "OK");
				return;
			}

			score++;
			ScoreLabel.Text = $"Score: {score}";


			canvas.Invalidate();

			await Task.Delay(TimeSpan.FromSeconds(1.0 / 45));
		}
	}

	private async void OnCanvasTapped(object sender, EventArgs e)
	{
		if (isRunning)
		{
			var player = AudioManager.Current.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("jump.mp3"));
			player.Play();

			//TODO CLEAN AUDIO PLAYERS ABOVE SOMEHOW


			flappy.Jump();
		}
	}

	private async void OnMenuClicked(object sender, EventArgs e)
	{
		string action = await DisplayActionSheet("Menu", "Cancel", null, "Option 1", "Option 2", "Option 3");

		switch (action)
		{
			case "Option 1":
				// Perform action for Option 1
				break;
			case "Option 2":
				// Perform action for Option 2
				break;
			case "Option 3":
				// Perform action for Option 3
				break;
		}
	}

	private async void OnStartClicked(object sender, EventArgs e)
	{
		isRunning = true;
		score = 0;
		flappy = new Flappy(_width / 2, _height / 2, Colors.Yellow);
		pipes = new List<GreenPipe>();
		pipes.Add(new GreenPipe(_width, 200, _height));
		canvas.Drawable = new Canvas() { flappy = flappy, _greenPipes = pipes };
		RunGameLoop();
	}

    private async void OnStartMatchmaking(object sender, EventArgs e)
    {
        //start matchmaking
        await _connection.InvokeAsync("StartMatchmaking");
		//set matchmaking text in the middle of the canvas
    }
}

public class Canvas : IDrawable
{
	private int _height = 500;
	private int _width = 350;
	public Flappy flappy;
	public List<GreenPipe> _greenPipes = new List<GreenPipe>();


	public Canvas()
	{
	}

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		if (canvas == null)
			return;

		// Draw background
		canvas.FillColor = Colors.LightBlue;
		canvas.FillRectangle(0, 0, _width, _height);

		//Draw Pipes
		foreach (GreenPipe pipe in _greenPipes)
		{
			pipe.Draw(canvas);
		}

		if (flappy != null)
			flappy.Draw(canvas);
	}
}