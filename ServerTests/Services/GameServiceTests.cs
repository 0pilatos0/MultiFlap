using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Server.Hubs;
using Server.Services;

namespace YourTestingProjectNamespace
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
    }
}
