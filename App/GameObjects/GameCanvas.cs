using App.GameObjects;

namespace App;

public class GameCanvas : IDrawable
{
	private int _height = 500;
	private int _width = 350;
	public Flappy flappy;
	public List<GreenPipe> _greenPipes = new List<GreenPipe>();
	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		if (canvas == null)
			return;

		// Draw background
		canvas.FillColor = Colors.LightBlue;
		canvas.FillRectangle(0, 0, _width, _height);

		//Draw Pipes
		foreach (GreenPipe pipe in _greenPipes)
		{
			pipe.Draw(canvas);
		}

		if (flappy != null)
			flappy.Draw(canvas);
	}
}
