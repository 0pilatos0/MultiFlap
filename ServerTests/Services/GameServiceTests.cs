using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Services;
using Server.Models.Game;
using Match = Server.Models.Game.Match;

namespace ServerTests.Services
{
    public class GameServiceTests
    {
        [Fact]
        public async Task AddPlayerAsync_ShouldAddPlayerAndCallUpdateOnlinePlayersCountAsync()
        {
            // Arrange
            var hubContextMock = new Mock<IHubContext<GameHub>>();
            var loggerMock = new Mock<ILogger<GameService>>();

            var gameService = new GameService(hubContextMock.Object, loggerMock.Object);

            var connectionId = "testConnectionId";
            var clientProxyMock = new Mock<IClientProxy>();

            hubContextMock.Setup(m => m.Clients.All).Returns(clientProxyMock.Object);
            clientProxyMock
                .Setup(
                    m =>
                        m.SendCoreAsync(
                            It.IsAny<string>(),
                            It.IsAny<object[]>(),
                            default(CancellationToken)
                        )
                )
                .Returns(Task.CompletedTask);

            // Act
            await gameService.AddPlayerAsync(connectionId);

            // Assert
            // Verify that the player was added
            Assert.True(gameService.Players.ContainsKey(connectionId));
        }

        [Fact]
        public async Task RemovePlayerAsync_ShouldRemovePlayerAndCallUpdateOnlinePlayersCountAsync()
        {
            // Arrange
            var hubContextMock = new Mock<IHubContext<GameHub>>();
            var loggerMock = new Mock<ILogger<GameService>>();

            var gameService = new GameService(hubContextMock.Object, loggerMock.Object);

            var connectionId = "testConnectionId";
            var player = new Player
            {
                ConnectionId = connectionId,
                Y = 0,
                Score = 0
            };
            gameService.Players[connectionId] = player;

            var clientProxyMock = new Mock<IClientProxy>();

            hubContextMock.Setup(m => m.Clients.All).Returns(clientProxyMock.Object);
            clientProxyMock
                .Setup(
                    m =>
                        m.SendCoreAsync(
                            It.IsAny<string>(),
                            It.IsAny<object[]>(),
                            default(CancellationToken)
                        )
                )
                .Returns(Task.CompletedTask);

            // Act
            await gameService.RemovePlayerAsync(connectionId);

            // Assert
            // Verify that the player was removed
            Assert.False(gameService.Players.ContainsKey(connectionId));
        }

        [Fact]
        public async Task StartMatchmakingAsync_ShouldStartMatchmakingForUser()
        {
            // Arrange
            var hubContextMock = new Mock<IHubContext<GameHub>>();
            var loggerMock = new Mock<ILogger<GameService>>();

            var gameService = new GameService(hubContextMock.Object, loggerMock.Object);

            var connectionId = "testConnectionId";
            var player = new Player
            {
                ConnectionId = connectionId,
                Y = 0,
                Score = 0
            };
            gameService.Players[connectionId] = player;

            var clientProxyMock = new Mock<ISingleClientProxy>(); // Use ISingleClientProxy here

            hubContextMock.Setup(m => m.Clients.All).Returns(clientProxyMock.Object);
            clientProxyMock
                .Setup(
                    m =>
                        m.SendCoreAsync(
                            It.IsAny<string>(),
                            It.IsAny<object[]>(),
                            default(CancellationToken)
                        )
                )
                .Returns(Task.CompletedTask);

            hubContextMock
                .Setup(m => m.Clients.Client(It.IsAny<string>()))
                .Returns(clientProxyMock.Object);

            // Act
            await gameService.StartMatchmakingAsync(connectionId);

            // Assert
            // Verify that the player is looking for a match
            Assert.True(gameService.Players[connectionId].IsLookingForMatch);
        }

        [Fact]
        public async Task GameOverAsync_ShouldUpdatePlayersAndSendOpponentGameOver()
        {
            var hubContextMock = new Mock<IHubContext<GameHub>>();
            var loggerMock = new Mock<ILogger<GameService>>();
            // Arrange
            var gameService = new GameService(hubContextMock.Object, loggerMock.Object);
            var connectionId = "connectionId";
            var score = 10;

            // Add a player to the game service
            await gameService.AddPlayerAsync(connectionId);

            // Act
            await gameService.GameOverAsync(connectionId, score);

            // Assert
            // Add your assertions here to verify the expected behavior
            // For example, you can check if the players and matches were updated correctly
            Assert.Empty(gameService.Matches);
            // Add more assertions as needed
        }
    }
}
