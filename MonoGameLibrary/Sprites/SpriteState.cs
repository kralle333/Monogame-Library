using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary
{
	public class SpriteState
	{
		public string name;

		private Rectangle[] _spriteRectangles;
		public int currentFrame = 0;
		private int _numberOfFrames = 0;
        public int NumberOfFrames { get { return _numberOfFrames; } }
		private float _totalElapsed = 0;
		private float _timePerFrame = 0;

		private int _width;
		private int _height;

		public int Width { get { return _width; } }
		public int Height { get { return _height; } }

		public SpriteState(string name, Rectangle[] spriteRectangles, int framesPerSec)
		{
			this.name = name;
			this._spriteRectangles = spriteRectangles;
			_timePerFrame = (float)1/framesPerSec;
			_numberOfFrames = spriteRectangles.Length;
			_width = _spriteRectangles[0].Width;
			_height = _spriteRectangles[0].Height;
		}

		public void SetFrame(int number)
		{
			currentFrame = number;
			currentFrame = currentFrame % _spriteRectangles.Length;
			_totalElapsed = 0;
		}
		public void NextFrame()
		{
			currentFrame++;
			currentFrame = currentFrame % _spriteRectangles.Length;
			_totalElapsed = 0;
		}
		public void UpdateFrame(float elapsed)
		{
			if (_numberOfFrames > 1)
			{
				_totalElapsed += elapsed;
				if (_totalElapsed > _timePerFrame)
				{
					currentFrame++;
					// Keep the Frame between 0 and the total frames, minus one.
					currentFrame = currentFrame % _spriteRectangles.Length;
					_totalElapsed -= _timePerFrame;
				}
			}
		}

		public Rectangle GetCurrentFrameRectangle()
		{
			return _spriteRectangles[currentFrame];
		}
	}
}
