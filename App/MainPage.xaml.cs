using App.GameObjects;
using Microsoft.Maui.Graphics.Skia;
using System.Reflection;
using System.Resources;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace App
{
    public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void StartGameClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Game());
		}
	}
}
