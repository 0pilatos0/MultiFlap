using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Server.Hubs;
using Server.Models.Game;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Services
{
    public class GameService : IGameService
    {
        private readonly ConcurrentDictionary<string, Player> _players =
            new ConcurrentDictionary<string, Player>();
        private readonly ConcurrentDictionary<string, Match> _matches =
            new ConcurrentDictionary<string, Match>();
        private readonly IHubContext<GameHub> _hubContext;
        private readonly ILogger<GameService> _logger;

        public GameService(IHubContext<GameHub> hubContext, ILogger<GameService> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public Task AddPlayerAsync(string connectionId)
        {
            var player = new Player
            {
                ConnectionId = connectionId,
                Y = 0,
                Score = 0
            };

            _players.TryAdd(player.ConnectionId, player);

            _logger.LogInformation($"Player {player.ConnectionId} connected");
            _logger.LogInformation($"Player count: {_players.Count}");

            return UpdateOnlinePlayersCountAsync();
        }

        public async Task RemovePlayerAsync(string connectionId)
        {
            if (_players.TryRemove(connectionId, out var player))
            {
                if (player.MatchId != null && _matches.TryGetValue(player.MatchId, out var match))
                {
                    var opponentId = match.Players
                        .FirstOrDefault(p => p.ConnectionId != connectionId)
                        ?.ConnectionId;

                    _players[opponentId].MatchId = null;
                    _players[opponentId].IsLookingForMatch = false;
                    _players[connectionId].MatchId = null;
                    _players[connectionId].IsLookingForMatch = false;

                    _matches.TryRemove(match.Id, out var removedMatch);

                    await SendToClientAsync(opponentId, "OpponentGameOver", 0);
                    _logger.LogInformation($"Sent OpponentGameOver to {opponentId}");
                }
                else
                {
                    _logger.LogInformation($"Match not found for player {connectionId}");
                }
            }
            else
            {
                _logger.LogInformation($"Could not find player in the current player list");
            }

            _logger.LogInformation($"Player {connectionId} disconnected");

            await UpdateOnlinePlayersCountAsync();
        }

        public async Task StartMatchmakingAsync(string connectionId)
        {
            _logger.LogInformation($"Player {connectionId} started matchmaking");
            _logger.LogInformation($"Player count: {_players.Count}");

            //log all players
            foreach (var plsayer in _players)
            {
                _logger.LogInformation(
                    $"Player {plsayer.Key} is looking for match: {plsayer.Value.IsLookingForMatch}"
                );
            }

            if (_players.TryGetValue(connectionId, out var player))
            {
                player.MatchId = null;
                player.Y = 0;
                player.Score = 0;
                player.IsLookingForMatch = true;

                var match = FindMatch(player);

                if (match == null)
                {
                    _logger.LogInformation($"Matchmaking started for player {player.ConnectionId}");
                    await SendToClientAsync(connectionId, "MatchmakingStarted");
                }
                else
                {
                    _logger.LogInformation($"Match found for player {player.ConnectionId}");
                    match.Players.Add(
                        new PlayerMatchInfo
                        {
                            ConnectionId = player.ConnectionId,
                            Y = player.Y,
                            Score = player.Score
                        }
                    );

                    _matches[match.Id] = match;

                    var opponentId = match.Players
                        .FirstOrDefault(p => p.ConnectionId != player.ConnectionId)
                        ?.ConnectionId;

                    _players[opponentId].MatchId = match.Id;
                    _players[opponentId].IsLookingForMatch = false;

                    _players[player.ConnectionId].MatchId = match.Id;
                    _players[player.ConnectionId].IsLookingForMatch = false;

                    await SendToClientsAsync(
                        new List<string> { player.ConnectionId, opponentId },
                        "MatchStarted",
                        match.Players
                    );
                    _logger.LogInformation($"Match {match.Id} started");
                }
            }
            else
            {
                _logger.LogInformation($"Player {connectionId} not found");
            }
        }

        public async Task GameOverAsync(string connectionId, int score)
        {
            _logger.LogInformation($"Received game over from {connectionId}");

            if (string.IsNullOrEmpty(connectionId))
            {
                _logger.LogInformation("Connection ID is null or empty");
                return;
            }

            if (_players.TryGetValue(connectionId, out var player))
            {
                if (player.MatchId != null && _matches.TryGetValue(player.MatchId, out var match))
                {
                    var opponentId = match.Players
                        .FirstOrDefault(p => p.ConnectionId != connectionId)
                        ?.ConnectionId;

                    _players[opponentId].MatchId = null;
                    _players[opponentId].IsLookingForMatch = false;
                    _players[connectionId].MatchId = null;
                    _players[connectionId].IsLookingForMatch = false;

                    _matches.TryRemove(match.Id, out var removedMatch);

                    await SendToClientAsync(opponentId, "OpponentGameOver", score);
                    _logger.LogInformation($"Sent OpponentGameOver to {opponentId}");
                }
                else
                {
                    _logger.LogInformation($"Match not found for player {connectionId}");
                }
            }
            else
            {
                _logger.LogInformation($"Could not find player in the current player list");
            }

            _logger.LogInformation($"Player {connectionId} game over");
        }

        public async Task CancelMatchmakingAsync(string connectionId)
        {
            if (_players.TryGetValue(connectionId, out var player))
            {
                player.IsLookingForMatch = false;
                if (player.MatchId != null && _matches.TryGetValue(player.MatchId, out var match))
                {
                    if (match.Players.Count == 1)
                    {
                        _matches.TryRemove(match.Id, out var removedMatch);
                    }
                    else
                    {
                        match.Players = match.Players
                            .Where(p => p.ConnectionId != connectionId)
                            .ToList();
                    }
                }

                await SendToClientAsync(connectionId, "MatchmakingCanceled");
            }

            _logger.LogInformation($"Player {connectionId} canceled matchmaking");
        }

        private async Task UpdateOnlinePlayersCountAsync()
        {
            await SendToAllClientsAsync("UpdateOnlinePlayers", _players.Count);
        }

        private async Task SendToClientAsync(
            string connectionId,
            string methodName,
            object? argument = null
        )
        {
            await _hubContext.Clients.Client(connectionId).SendAsync(methodName, argument);
        }

        private async Task SendToClientsAsync(
            IEnumerable<string> connectionIds,
            string methodName,
            object? argument = null
        )
        {
            await _hubContext.Clients
                .Clients(connectionIds.ToList())
                .SendAsync(methodName, argument);
        }

        private async Task SendToAllClientsAsync(string methodName, object? argument = null)
        {
            await _hubContext.Clients.All.SendAsync(methodName, argument);
        }

        private Match FindMatch(Player player)
        {
            foreach (var match in _matches.Values)
            {
                if (match.Players.Count == 1)
                {
                    var matchPlayer = match.Players.First();
                    if (Math.Abs(matchPlayer.Y - player.Y) <= 1)
                    {
                        return match;
                    }
                }
            }

            var newMatch = new Match
            {
                Id = Guid.NewGuid().ToString(),
                Players = new List<PlayerMatchInfo>
                {
                    new PlayerMatchInfo
                    {
                        ConnectionId = player.ConnectionId,
                        Y = player.Y,
                        Score = player.Score
                    }
                }
            };

            _matches.TryAdd(newMatch.Id, newMatch);
            return newMatch;
        }
    }
}
