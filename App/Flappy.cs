using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
	public class Flappy
	{
		public int X { get; private set; }
		public int Y { get; private set; }
		public int Velocity { get; private set; }

		private const int Gravity = 1;
		private const int JumpVelocity = -15;

		public Flappy(int x, int y)
		{
			X = x;
			Y = y;
			Velocity = 0;
		}

		public void UpdatePosition()
		{
			Velocity += Gravity;
			Y += Velocity;
		}

		public void Jump()
		{
			Velocity = JumpVelocity;
		}
	}
}
