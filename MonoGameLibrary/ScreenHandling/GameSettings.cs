using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary
{
	public class GameSettings
	{
		private static GraphicsDeviceManager _graphics;
		public static float ScreenWidth
		{
			get { return _graphics.PreferredBackBufferWidth; }
		}
		public static float ScreenHeight
		{
			get { return _graphics.PreferredBackBufferHeight; }
		}
		public GameSettings(GraphicsDeviceManager graphics)
		{
			_graphics = graphics;
		}
		public static void SetResolution(int width, int height, bool fullScreen)
		{
			_graphics.PreferredBackBufferWidth = width;
			_graphics.PreferredBackBufferHeight = height;
			_graphics.IsFullScreen = fullScreen;
			_graphics.ApplyChanges();
		}
		public static Vector2 GetResolution()
		{
			return new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
		}

	}
}
