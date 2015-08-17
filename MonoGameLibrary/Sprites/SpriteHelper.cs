using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary
{
	public class SpriteHelper
	{
		public enum SpriteDepth { High = 9, Middle = 5, Low = 2 }

		public static float GetDefaultDepth(SpriteDepth depth)
		{
			return ((float)depth) / 10;
		}


		/// <summary>
		/// Used for getting a tile from a tilesheet
		/// </summary>
		public static Rectangle GetSpriteRectangle(int width, int height, int border, int xPosition, int yPosition)
		{
			return new Rectangle(xPosition * width + (border * (xPosition + 1)), yPosition * height + (border * (yPosition + 1)), width, height);
		}
		/// <summary>
		/// Used for getting a strip of tiles from a tilesheet
		/// </summary>
		public static Rectangle[] GetSpriteRectangleStrip(int width, int height, int border, int xStart, int xEnd, int yStart, int yEnd)
		{
			if (xStart != xEnd && yStart != yEnd)
			{
				throw new Exception("To get a rectangle strip either x or y must be constant");
			}
			else if (xStart > xEnd || yStart > yEnd)
			{
				throw new Exception("The start coordinate can not be bigger than the end coordinate");
			}

			bool verticalStrip = (xEnd - xStart) == 0;
			int stripLength = Math.Max(xEnd - xStart, yEnd - yStart);
			Rectangle[] rectangleStrip = new Rectangle[stripLength];
			int i = 0;
			if (verticalStrip)
			{
				for (int y = yStart; y < yEnd; y++, i++)
				{
					rectangleStrip[i] = GetSpriteRectangle(width, height, 0, xStart, y);
				}

			}
			else
			{
				for (int x = xStart; x < xEnd; x++, i++)
				{
					rectangleStrip[i] = GetSpriteRectangle(width, height, 0, x, yStart);
				}
			}

			return rectangleStrip;
		}

		public static List<T> GetSpritesWithinRange<T>(Vector2 origin, float range, List<T> spriteList) where T : Sprite
		{
			List<T> sprites = new List<T>();
			foreach (T sprite in spriteList)
			{
				if (GeometricHelper.GetDistance(origin, sprite.position) <= range)
				{
					sprites.Add(sprite);
				}
			}

			return sprites;
		}
		public static T GetClosestSpriteWithinRange<T>(Vector2 origin, float range, List<T> spriteList) where T : Sprite
		{
			T closestSprite = null;
			float closestDistance = float.MaxValue;
			float currentDistance = 0;
			foreach (T sprite in spriteList)
			{
				currentDistance = GeometricHelper.GetDistance(origin, sprite.position);
				if (currentDistance < closestDistance && currentDistance <= range)
				{
					closestSprite = sprite;
					closestDistance = currentDistance;
				}
			}

			return closestSprite;
		}

		public static void ChangeColorOfTexture(Color oldColor, Color newColor, ref Texture2D texture)
		{
			if (texture == null)
			{
				throw new ArgumentNullException();
			}
			Color[] data = new Color[texture.Width * texture.Height];
			texture.GetData(data);

			for (int i = 0; i < data.Length; i++)
				if (data[i] == oldColor)
					data[i] = newColor;

			texture.SetData(data);
		}
	}
}
