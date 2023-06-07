using Server.Models.Game;

namespace Server.Services
{
	public interface IGameService
	{
		Task AddPlayerAsync(string connectionId);
		Task RemovePlayerAsync(string connectionId);
		Task StartMatchmakingAsync(string connectionId);
		Task GameOverAsync(string connectionId, int score);
		Task CancelMatchmakingAsync(string connectionId);
	}
}
