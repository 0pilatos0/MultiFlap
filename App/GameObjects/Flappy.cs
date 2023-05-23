using Microsoft.Maui.Graphics.Platform;
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace App.GameObjects
{
    public class Flappy : IGameObject
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        public double Velocity { get; private set; }

        private const double Gravity = 1.2;
        private const int JumpVelocity = -17;

		private IImage flappyImage;

		public Flappy(int x, int y, Color color)
		{
            X = x;
            Y = y;
            Velocity = 0;

			Assembly assembly = GetType().GetTypeInfo().Assembly;
			using (Stream stream = assembly.GetManifestResourceStream("App.Resources.Images.flappy.png"))
			{
				flappyImage = PlatformImage.FromStream(stream);
			}
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

			float width = 40; // Adjust the width of the image as needed
			float height = 40; // Adjust the height of the image as needed

			canvas.DrawImage(flappyImage, X, Y, width, height);
		}
	}
}
