namespace App
{
	public partial class MainPage : ContentPage
	{
		private bool isRunning;
		private Flappy flappy;
		private List<GreenPipe> pipes;

		int _width = 400;
		int _height = 600;
		private int score;

		public MainPage()
		{
			InitializeComponent();
			var tapGestureRecognizer = new TapGestureRecognizer();
			tapGestureRecognizer.Tapped += OnCanvasTapped;
			canvas.GestureRecognizers.Add(tapGestureRecognizer);
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			isRunning = true;
			flappy = new Flappy(_width / 2, _height / 2);
			pipes = new List<GreenPipe>();
			pipes.Add(new GreenPipe(_width, 200, _height));
			canvas.Drawable = new GraphicsDrawable() { flappy = flappy, _greenPipes = pipes };
			RunGameLoop();
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
							await DisplayAlert("Game Over", $"Score: {score}", "OK");
							return;
						}
					}
				}

				if (flappy.Y < 0 || flappy.Y > _height)
				{
					isRunning = false;
					await DisplayAlert("Game Over", $"Score: {score}", "OK");
					return;
				}

				score++;
				ScoreLabel.Text = $"Score: {score}";


				canvas.Invalidate();

				await Task.Delay(TimeSpan.FromSeconds(1.0 / 60));
			}
		}

		private void OnCanvasTapped(object sender, EventArgs e)
		{
			flappy.Jump();
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
			flappy = new Flappy(_width / 2, _height / 2);
			pipes = new List<GreenPipe>();
			pipes.Add(new GreenPipe(_width, 200, _height));
			canvas.Drawable = new GraphicsDrawable() { flappy = flappy, _greenPipes = pipes };
			RunGameLoop();
		}
	}

	public class GraphicsDrawable : IDrawable
	{
		private int _height = 600;
		private int _width = 400;
		public Flappy flappy;
		public List<GreenPipe> _greenPipes = new List<GreenPipe>();

		public GraphicsDrawable()
		{
		}

		public void Draw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.FillColor = Colors.LightBlue;
			canvas.FillRectangle(0, 0, _width, _height);

			// Draw the green pipes
			canvas.FillColor = Colors.Green;
			foreach (GreenPipe pipe in _greenPipes)
			{
				// Draw the top part of the pipe
				canvas.FillRectangle(pipe.X, 0, 100, pipe.TopHeight);

				// Draw the bottom part of the pipe
				canvas.FillRectangle(pipe.X, pipe.TopHeight + pipe.GapSize, 100, pipe.BottomHeight);
			}

			// Draw the flappy bird
			canvas.FillColor = Colors.Yellow;
			if (flappy != null)
			{
				canvas.FillCircle(flappy.X, flappy.Y, 20);
			}
		}

		public void Update(double deltaTime)
		{
			// Update the green pipes
			foreach (GreenPipe pipe in _greenPipes)
			{
				pipe.UpdatePosition();
			}
		}
	}


	public class GreenPipe
	{
		public int X { get; private set; }
		public int TopHeight { get; private set; }
		public int BottomHeight { get; private set; }
		public int GapSize { get; private set; }

		private int _speed = 5;
		private int _maxTopHeight = 400;
		private int _minTopHeight = 200;
		private int _minBottomHeight = 150;
		private int _maxBottomHeight;

		public GreenPipe(int x, int gapSize, int maxBottomHeight)
		{
			X = x;
			GapSize = gapSize;
			_maxBottomHeight = maxBottomHeight;
			GenerateHeights();
		}

		public void UpdatePosition()
		{
			X -= _speed;
			if (X < -100) // the pipe is off-screen
			{
				X = 500; // reset to the right side of the screen
				GenerateHeights();
			}
		}

		private void GenerateHeights()
		{
			Random random = new Random();
			TopHeight = random.Next(_minTopHeight, _maxTopHeight - GapSize);
			BottomHeight = _maxBottomHeight - (TopHeight + GapSize);
		}
	}
}
