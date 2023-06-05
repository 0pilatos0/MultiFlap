using App.GameCore.GameObjects;
using System.Collections.Generic;
using System.IO.Pipelines;

namespace App.GameCore
{
	public class GameEngine
	{
		public bool IsRunning { get; set; }
		public int Score { get; set; }
		public int Width { get; private set; } = 350;
		public int Height { get; private set; } = 500;

		public bool OnlineMatch { get; set; } = false;
		public bool ShakeEnabled { get; set; } = true;

		private const int PipeWidth = 100;
		private const int BirdSize = 20;
		private const int GameWindowLowerBound = 0;

		public void ResetScore()
		{
			Score = 0;
		}

		public bool CheckCollision(Flappy flappy, List<GreenPipe> pipes)
		{

			foreach (var pipe in pipes)
			{
				// Check for collision
				if (flappy.X + BirdSize > pipe.X && flappy.X - BirdSize < pipe.X + PipeWidth)
				{
					if (flappy.Y - BirdSize < pipe.TopHeight || flappy.Y + BirdSize > pipe.TopHeight + pipe.GapSize)
					{
						return true;
					}
				}
			}
		
			if (flappy.Y < GameWindowLowerBound || flappy.Y > Height)
			{
				return true;

			}

			return false;
		}

	}
}
