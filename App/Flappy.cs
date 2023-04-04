using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
	public class Flappy
	{
		private const float Gravity = 0.2f;
		private const float JumpSpeed = -5.0f;

		public float X { get; private set; }
		public float Y { get; private set; }
		private float velocityY;

		public Flappy(float x, float y)
		{
			X = x;
			Y = y;
			velocityY = 0;
		}

		public void UpdatePosition()
		{
			velocityY += Gravity;
			Y += velocityY;
		}

		public void Jump()
		{
			velocityY = JumpSpeed;
		}
	}
}
