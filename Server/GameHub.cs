﻿using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Server
{
	public class GameHub : Hub
	{
		public override Task OnConnectedAsync()
		{
			var player = new Player()
			{
				ConnectionId = Context.ConnectionId,
				Y = 0,
				Score = 0
			};

			GameData.Instance.Players.TryAdd(player.ConnectionId, player);

			Clients.Caller.SendAsync("Initialize", player);

			Console.WriteLine($"Player {player.ConnectionId} connected");
			Console.WriteLine($"Player count: {GameData.Instance.Players.Count}");

			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception exception)
		{
			if (GameData.Instance.Players.TryGetValue(Context.ConnectionId, out Player player))
			{
				if (player.MatchId != null && GameData.Instance.Matches.TryGetValue(player.MatchId, out Match match))
				{
					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;
					GameData.Instance.Players[opponentId].MatchId = null;

					GameData.Instance.Matches.TryRemove(match.Id, out Match _);

					Clients.Client(opponentId).SendAsync("OpponentDisconnected");
				}

				GameData.Instance.Players.TryRemove(player.ConnectionId, out Player _);
			}

			Console.WriteLine($"Player {Context.ConnectionId} disconnected");

			return base.OnDisconnectedAsync(exception);
		}

		public async Task StartMatchmaking()
		{
			Console.WriteLine($"Player {Context.ConnectionId} started matchmaking");
			Console.WriteLine($"Player count: {GameData.Instance.Players.Count}");
			if (GameData.Instance.Players.TryGetValue(Context.ConnectionId, out Player player))
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

					GameData.Instance.Matches.TryAdd(match.Id, match);

					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;

					GameData.Instance.Players[opponentId].MatchId = match.Id;
					GameData.Instance.Players[opponentId].IsLookingForMatch = false;

					await Clients.Clients(new List<string>() { player.ConnectionId, opponentId }).SendAsync("MatchStarted", match.Players);
				}

				Console.WriteLine($"Player {player.ConnectionId} started matchmaking");

			}
			else
			{
				Console.WriteLine($"Player {Context.ConnectionId} not found");

			}
		}

		private Match FindMatch(Player player)
		{
			var match = GameData.Instance.Matches.Values.FirstOrDefault(m => m.Players.Count == 1 && m.Players[0].ConnectionId != player.ConnectionId);

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

			Console.WriteLine($"Match {match.Id} found");
		}

		public async Task UpdatePlayerState(int y, int score)
		{
			if (GameData.Instance.Players.TryGetValue(Context.ConnectionId, out Player player))
			{
				player.Y = y;
				player.Score = score;

				if (player.MatchId != null && GameData.Instance.Matches.TryGetValue(player.MatchId, out Match match))
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

			Console.WriteLine($"Player {Context.ConnectionId} updated state");
		}

		public async Task EndMatch()
		{
			if (GameData.Instance.Players.TryGetValue(Context.ConnectionId, out Player player))
			{
				if (player.MatchId != null && GameData.Instance.Matches.TryGetValue(player.MatchId, out Match match))
				{
					var opponentId = match.Players.FirstOrDefault(p => p.ConnectionId != player.ConnectionId)?.ConnectionId;

					GameData.Instance.Players[opponentId].MatchId = null;

					GameData.Instance.Matches.TryRemove(match.Id, out Match removedMatch);

					await Clients.Clients(new List<string>() { player.ConnectionId, opponentId }).SendAsync("MatchEnded");
				}
			}

			Console.WriteLine($"Player {Context.ConnectionId} ended match");
		}

		public async Task CancelMatchmaking()
		{
			if (GameData.Instance.Players.TryGetValue(Context.ConnectionId, out Player player))
			{
				player.IsLookingForMatch = false;

				await Clients.Caller.SendAsync("MatchmakingCanceled");
			}

			Console.WriteLine($"Player {Context.ConnectionId} canceled matchmaking");
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
