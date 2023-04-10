using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.GameObjects
{
    public class Flappy : IGameObject
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public double Velocity { get; private set; }

        private const double Gravity = 1.2;
        private const int JumpVelocity = -17;

        public Flappy(int x, int y)
        {
            X = x;
            Y = y;
            Velocity = 0;
        }

        public void UpdatePosition()
        {
            Velocity += Gravity;
            //convert to int
            Y += (int)Velocity;
        }

        public void Jump()
        {
            Velocity = JumpVelocity;
        }

		public void Draw(ICanvas canvas)
		{
            if (this == null) return;
         
			canvas.FillColor = Colors.Yellow;
			canvas.FillCircle(X, Y, 20);
		}
	}
}
