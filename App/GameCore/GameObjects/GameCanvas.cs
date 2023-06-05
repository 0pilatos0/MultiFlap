namespace App.GameCore.GameObjects;

public class GameCanvas : IDrawable
{
	private int _height = 500;
	private int _width = 350;
	private Flappy _flappy;
	private List<GreenPipe> _greenPipes;

	public Flappy Flappy
	{
		get { return _flappy; }
		set { _flappy = value; }
	}

	public List<GreenPipe> GreenPipes
	{
		get { return _greenPipes; }
		set { _greenPipes = value; }
	}

	public void Draw(ICanvas canvas, RectF dirtyRect)
	{
		if (canvas == null)
			return;

		// Draw background
		canvas.FillColor = Colors.LightBlue;
		canvas.FillRectangle(0, 0, _width, _height);

		// Draw Pipes
		if (_greenPipes != null)
		{
			foreach (GreenPipe pipe in _greenPipes)
			{
				pipe.Draw(canvas);
			}
		}

		if (_flappy != null)
			_flappy.Draw(canvas);
	}
}