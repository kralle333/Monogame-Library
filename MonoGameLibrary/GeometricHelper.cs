using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary
{
	public class GeometricHelper
	{
		private static Random _random = new Random();
		public static float GetDistance(float x1, float y1, float x2, float y2)
		{
			return (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
		}
		public static float GetDistance(Vector2 p1, Vector2 p2)
		{
			return GetDistance(p1.X, p1.Y, p2.X, p2.Y);
		}
		public static float GetAngleBetweenPositions(Vector2 p1, Vector2 p2)
		{
			double angle = Math.Atan2(p2.Y-p1.Y, p2.X-p1.X);
			if (angle < 0)
			{
				angle += Math.PI*2;
			}
			return (float)angle;
		}

		public static Vector2 GetRandomPosition(Vector2 origin, float radius)
		{
			double randAngle = _random.NextDouble() * Math.PI * 2;
			double randRadius = _random.NextDouble() * radius;
			double x = origin.X + randRadius * Math.Cos(randAngle);
			double y = origin.Y + randRadius * Math.Sin(randAngle);
			return new Vector2((float)x, (float)y);
		}
		public static Vector2 GetRandomPosition(Vector2 origin,float minRadius, float maxRadius)
		{
			double randAngle = _random.NextDouble() * Math.PI * 2;
			double randRadius = (_random.NextDouble() * (maxRadius - minRadius))+minRadius;
			double x = origin.X + randRadius * Math.Cos(randAngle);
			double y = origin.Y + randRadius * Math.Sin(randAngle);
			return new Vector2((float)x, (float)y);
		}
		public static float GetAngleFromVectorDirection(Vector2 direction)
		{
			float angle = (float)Math.Atan2(direction.Y, direction.X);
			if (angle < 0)
			{
				angle += (float)(2*Math.PI);
			}
			return angle;
		}
		public static Vector2 GetVectorDirectionFromAngle(float angleInRadians)
		{
			float x = (float)Math.Cos((double)(angleInRadians));
			float y = (float)Math.Sin((double)(angleInRadians));

			return new Vector2(x,y);
		}
	}
}
