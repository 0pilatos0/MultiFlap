using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Server.Services;
using System.Threading.Tasks;

namespace Server.Hubs
{
	public class GameHub : Hub
	{
		private readonly IGameService _gameService;
		private readonly ILogger<GameHub> _logger;

		public GameHub(IGameService gameService, ILogger<GameHub> logger)
		{
			_gameService = gameService;
			_logger = logger;
		}

		public override async Task OnConnectedAsync()
		{
			await _gameService.AddPlayerAsync(Context.ConnectionId);

			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			await _gameService.RemovePlayerAsync(Context.ConnectionId);

			await base.OnDisconnectedAsync(exception);
		}

		public async Task StartMatchmaking()
		{
			await _gameService.StartMatchmakingAsync(Context.ConnectionId);
		}

		public async Task GameOver(int score)
		{
			await _gameService.GameOverAsync(Context.ConnectionId, score);
		}

		public async Task CancelMatchmaking()
		{
			await _gameService.CancelMatchmakingAsync(Context.ConnectionId);
		}
	}
}
