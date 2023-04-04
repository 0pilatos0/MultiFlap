namespace App
{
	public partial class MainPage : ContentPage
	{
		private bool isRunning;
		public MainPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			isRunning = true;
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
				Console.WriteLine("Running game loop");

				await Task.Delay(TimeSpan.FromSeconds(1.0 / 60));
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

	}

	public class GraphicsDrawable : IDrawable
	{

		int _height = 600;
		int _width = 400;
		
		public void Draw(ICanvas canvas, RectF dirtyRect)
		{
			canvas.FillColor = Colors.LightBlue;
			canvas.FillRectangle(0,0, _width, _height);

		}
	}
}
