﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.GameCore.GameObjects
{
    public class GreenPipe : IGameObject
    {
        public int X { get; private set; }
        public int TopHeight { get; private set; }
        public int BottomHeight { get; private set; }
        public int GapSize { get; private set; }

        private int _speed = 5;
        private int _maxTopHeight = 500;
        private int _minTopHeight = 100;
        private int _minBottomHeight = 100;
        private int _maxBottomHeight;

        public GreenPipe(int x, int gapSize, int maxBottomHeight)
        {
            X = x;
            GapSize = gapSize;
            _maxBottomHeight = maxBottomHeight;
            GenerateHeights();
        }

        public void UpdatePosition()
        {
            X -= _speed;
            if (X < -100) // the pipe is off-screen
            {
                X = 500; // reset to the right side of the screen
                GenerateHeights();
            }
        }

        private void GenerateHeights()
        {
            Random random = new Random();
            TopHeight = random.Next(_minTopHeight, _maxTopHeight - GapSize);
            BottomHeight = _maxBottomHeight - (TopHeight + GapSize);
        }

        public void Draw(ICanvas canvas)
        {
            canvas.FillColor = Colors.Green;
            canvas.FillRectangle(X, 0, 75, TopHeight);
            canvas.FillRectangle(X, TopHeight + GapSize, 75, BottomHeight);
        }
    }
}
