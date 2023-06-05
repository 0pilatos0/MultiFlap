#if !WINDOWS
using App;
using Microsoft.Maui.Graphics.Platform;
#endif
using System.Reflection;
using IImage = Microsoft.Maui.Graphics.IImage;

namespace App.GameCore.GameObjects
{
    public class Flappy : IGameObject
    {
        public int X { get; private set; }
        public int Y { get; private set; }
		public float Velocity { get; private set; }

		private const float Gravity = 1.2f;
		private const int JumpVelocity = -17;

		private static IImage flappyImage;

		public Flappy(int x, int y, Color color)
        {
            X = x;
            Y = y;
            Velocity = 0;

#if !WINDOWS
			if (flappyImage == null) // Load image resource only once
			{
				Assembly assembly = GetType().GetTypeInfo().Assembly;
				using (Stream stream = assembly.GetManifestResourceStream("App.Resources.Images.flappy.png"))
				{
					flappyImage = PlatformImage.FromStream(stream);
				}
			}
#endif
		}



        public void UpdatePosition()
        {
            Velocity += Gravity;
			Y += (int)Velocity; 

		}

        public void Jump()
        {
            Velocity = JumpVelocity;
        }

        public void Draw(ICanvas canvas)
        {
#if !WINDOWS
			if (flappyImage != null) // Ensure image is loaded
			{
				float width = 40; // Adjust the width of the image as needed
				float height = 40; // Adjust the height of the image as needed
				canvas.DrawImage(flappyImage, X, Y, width, height);
			}
#endif
		}
    }
}
