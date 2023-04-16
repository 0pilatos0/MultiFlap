using Microsoft.AspNetCore.SignalR;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Server
{
	public class GameHub : Hub
	{
		private Dictionary<string, Player> players = new Dictionary<string, Player>();
		private Dictionary<string, Match> matches = new Dictionary<string, Match>();

		public override Task OnConnectedAsync()
		{
			var player = new Player()
			{
				ConnectionId = Context.ConnectionId,
				Y = 0,
				Score = 0
			};

			players.Add(player.ConnectionId, player);

			Clients.Caller.SendAsync("Initialize", player);

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			if (players.TryGetValue(Context.ConnectionId, out Player player))
			{
				if (player.MatchId != null && matches.TryGetValue(player.MatchId, out Match match))
				{
					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;
					players[opponentId].MatchId = null;

					matches.Remove(player.MatchId);

					Clients.Client(opponentId).SendAsync("OpponentDisconnected");
				}

				players.Remove(Context.ConnectionId);
			}

			return base.OnDisconnectedAsync(exception);
		}

		public async Task StartMatchmaking()
		{
			if (players.TryGetValue(Context.ConnectionId, out Player player))
			{
				player.MatchId = null;
				player.Y = 0;
				player.Score = 0;
				player.IsLookingForMatch = true;

				var match = FindMatch(player);

				if (match == null)
				{
					await Clients.Caller.SendAsync("MatchmakingStarted");
				}
				else
				{
					match.Players.Add(new PlayerMatchInfo()
					{
						ConnectionId = player.ConnectionId,
						Y = player.Y,
						Score = player.Score
					});

					matches.Add(match.Id, match);

					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;

					players[opponentId].MatchId = match.Id;
					players[opponentId].IsLookingForMatch = false;

					await Clients.Clients(new List<string>() { player.ConnectionId, opponentId }).SendAsync("MatchStarted", match.Players);
				}
			}
		}

		private Match FindMatch(Player player)
		{
			var match = matches.Values.FirstOrDefault(m => m.Players.Count == 1 && m.Players[0].ConnectionId != player.ConnectionId);

			if (match == null)
			{
				return null;
			}

			return new Match()
			{
				Id = match.Id,
				Players = new List<PlayerMatchInfo>()
				{
					new PlayerMatchInfo()
					{
						ConnectionId = match.Players[0].ConnectionId,
						Y = match.Players[0].Y,
						Score = match.Players[0].Score
					},
					new PlayerMatchInfo()
					{
						ConnectionId = player.ConnectionId,
						Y = player.Y,
						Score = player.Score
					}
				}
			};
		}

		public async Task UpdatePlayerState(int y, int score)
		{
			if (players.TryGetValue(Context.ConnectionId, out Player player))
			{
				player.Y = y;
				player.Score = score;

				if (player.MatchId != null && matches.TryGetValue(player.MatchId, out Match match))
				{
					match.Players = match.Players.Select(p =>
					{
					if (p.ConnectionId == player.ConnectionId)
					{
						return new PlayerMatchInfo()
						{
							ConnectionId = p.ConnectionId,
							Y = player.Y,
							Score = player.Score
						};
						}
						else
						{
							return p;
						}
					}).ToList();

					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;

					await Clients.Client(opponentId).SendAsync("OpponentStateUpdated", player.Y, player.Score);
				}
			}
		}

		public async Task EndMatch()
		{
			if (players.TryGetValue(Context.ConnectionId, out Player player))
			{
				if (player.MatchId != null && matches.TryGetValue(player.MatchId, out Match match))
				{
					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;

					players[opponentId].MatchId = null;

					matches.Remove(player.MatchId);

					await Clients.Clients(new List<string>() { player.ConnectionId, opponentId }).SendAsync("MatchEnded");
				}
			}
		}

		public async Task CancelMatchmaking()
		{
			if (players.TryGetValue(Context.ConnectionId, out Player player))
			{
				player.IsLookingForMatch = false;

				await Clients.Caller.SendAsync("MatchmakingCanceled");
			}
		}
	}

	public class Player
	{
		public string ConnectionId { get; set; }
		public int Y { get; set; }
		public int Score { get; set; }
		public string MatchId { get; set; }
		public bool IsLookingForMatch { get; set; }
	}

	public class Match
	{
		public string Id { get; set; } = Guid.NewGuid().ToString();
		public List<PlayerMatchInfo> Players { get; set; } = new List<PlayerMatchInfo>();
	}

	public class PlayerMatchInfo
	{
		public string ConnectionId { get; set; }
		public int Y { get; set; }
		public int Score { get; set; }
	}
}
