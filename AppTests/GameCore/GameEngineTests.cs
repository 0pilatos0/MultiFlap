using System;
using System.Drawing;
using App.GameCore;
using App.GameCore.GameObjects;

namespace AppTests.GameCore
{
    public class GameEngineTests
    {
        [Fact]
        public void ResetScore_SetsScoreToZero()
        {
            // Arrange
            var gameEngine = new GameEngine();
            gameEngine.Score = 10;

            // Act
            gameEngine.ResetScore();

            // Assert
            Assert.Equal(0, gameEngine.Score);
        }
    }
}
