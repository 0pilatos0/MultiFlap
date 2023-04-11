using App.GameObjects;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Maui.Graphics.Skia;
using Plugin.Maui.Audio;
using System.Reflection;
using System.Resources;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace App
{
    public partial class MainPage : ContentPage
	{
		private readonly HubConnection _connection;

		public MainPage()
		{
			InitializeComponent();
			_connection = new HubConnectionBuilder()
				.WithUrl("http://192.168.2.24:5076/game")
				.Build();

			_connection.On<string>("MessageReceived", (message) =>
			{
				Task.Run(() =>
				{
					Dispatcher.Dispatch(async () =>
					{
						chatMessages.Text += $"{Environment.NewLine}{message}";
					});
				});
			});

			Task.Run(() =>
			{
				Dispatcher.Dispatch(async () => await _connection.StartAsync());
			});
		}

		private void StartGameClicked(object sender, EventArgs e)
		{
			Navigation.PushAsync(new Game());
		}

		private async void sendButton_Clicked(object sender, EventArgs e)
		{
			await _connection.InvokeCoreAsync("SendMessage", args: new[]
			{
				myChatMessage.Text
			});

			myChatMessage.Text = String.Empty;
        }
    }
}
