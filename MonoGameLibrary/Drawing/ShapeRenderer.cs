using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Drawing
{
	public class ShapeRenderer
	{
		private string _borderTexturePath;
		private Texture2D _borderTexture;
		private float _depth = 1;
		public float Depth
		{
			get
			{
				return _depth;
			}
			set
			{
				_depth = value;
				_depth = Math.Max(0, _depth);
				_depth = Math.Min(1, _depth);
			}
		}


		public ShapeRenderer(string borderTexturePath)
		{
			_borderTexturePath = borderTexturePath;
		}

		public void LoadContent(ContentManager contentManager)
		{
			_borderTexture = contentManager.Load<Texture2D>(_borderTexturePath);
		}


		private void DrawHorizontalLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition)
		{
			DrawHorizontalLine(spriteBatch, startPosition.X, startPosition.Y, endPosition.X, endPosition.Y);
		}
		private void DrawHorizontalLine(SpriteBatch spriteBatch, float startX, float startY, float endX, float endY)
		{
			if (startX > endX)
			{
				endX = startX;
				startX = endX;
			}
			for (float x = startX; x <= endX; x += _borderTexture.Width)
			{
				spriteBatch.Draw(_borderTexture, new Vector2(x, startY), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, _depth);
			}
		}
		private void DrawVerticalLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition)
		{
			float startY = startPosition.Y;
			float endY = endPosition.Y;
			if (startY > endY)
			{
				endY = startPosition.Y;
				startY = endPosition.Y;
			}
			for (float y = startY; y <= endY; y += _borderTexture.Height)
			{
				spriteBatch.Draw(_borderTexture, new Vector2(startPosition.X, y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, _depth);
			}
		}
		private void DrawVerticalLine(SpriteBatch spriteBatch, float startX, float startY, float endX, float endY)
		{
			if (startY > endY)
			{
				endY = startY;
				startY = endY;
			}
			for (float y = startY; y <= endY; y += _borderTexture.Height)
			{
				spriteBatch.Draw(_borderTexture, new Vector2(startX, y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, _depth);
			}
		}
		
		private void DrawDiagonalLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition)
		{
			throw new NotImplementedException();
		}
		public void DrawLine(SpriteBatch spriteBatch, Vector2 startPosition, Vector2 endPosition)
		{
			if (startPosition.X == endPosition.X)
			{
				DrawHorizontalLine(spriteBatch, startPosition, endPosition);
			}
			else if (startPosition.Y == endPosition.Y)
			{
				DrawDiagonalLine(spriteBatch, startPosition, endPosition);
			}
			else
			{
				DrawDiagonalLine(spriteBatch, startPosition, endPosition);
			}
		}

		public void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle)
		{
			Vector2 topLeft = new Vector2(rectangle.X, rectangle.Y);
			Vector2 topRight = new Vector2(rectangle.X + rectangle.Width, rectangle.Y);
			Vector2 bottomLeft = new Vector2(rectangle.X, rectangle.Y + rectangle.Height);
			Vector2 bottomRight = new Vector2(rectangle.X + rectangle.Width, rectangle.Y + rectangle.Height);

			DrawHorizontalLine(spriteBatch, topLeft, topRight);
			DrawHorizontalLine(spriteBatch, bottomLeft, bottomRight);
			DrawVerticalLine(spriteBatch, topLeft, bottomLeft);
			DrawVerticalLine(spriteBatch, topRight, bottomRight);
		}
		public void DrawRectangle(SpriteBatch spriteBatch, Vector2 topLeft, Vector2 bottomRight)
		{
			Vector2 topRight = new Vector2(bottomRight.X, topLeft.Y);
			Vector2 bottomLeft = new Vector2(topLeft.X, bottomRight.Y);

			DrawHorizontalLine(spriteBatch, topLeft, topRight);
			DrawHorizontalLine(spriteBatch, bottomLeft, bottomRight);
			DrawVerticalLine(spriteBatch, topLeft, bottomLeft);
			DrawVerticalLine(spriteBatch, topRight, bottomRight);
		}
		public void DrawRectangle(SpriteBatch spriteBatch, float x, float y, float width, float height)
		{
			DrawHorizontalLine(spriteBatch, x, y, x + width, y);
			DrawHorizontalLine(spriteBatch, x,y+height,x+width, y);
			DrawVerticalLine(spriteBatch, x,y, x,y+height);
			DrawVerticalLine(spriteBatch, x+width,y,x+width, y+height);
		}
	}
}
