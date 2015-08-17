using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary
{
	public class Camera2D
	{
		/// <summary>
		/// Borrowed from http://www.david-amador.com/2009/10/xna-camera-2d-with-zoom-and-rotation/
		/// </summary>


		protected float _zoom; // Camera Zoom
		private Matrix _transform; // Matrix Transform
		private Vector2 _pos; // Camera Position
		protected float _rotation; // Camera Rotation

		public Camera2D()
		{
			_zoom = 1.0f;
			_rotation = 0.0f;
			_pos = Vector2.Zero;
		}


		// Sets and gets zoom
		public float Zoom
		{
			get { return _zoom; }
			set { _zoom = value; if (_zoom < 0.1f) _zoom = 0.1f; } // Negative zoom will flip image
		}

		public float Rotation
		{
			get { return _rotation; }
			set { _rotation = value; }
		}

	
		private bool _wasCameraMoved = true;
		private Rectangle _cameraWorldRectangle;
		public Rectangle CameraWorldRectangle
		{
			get
			{
				if (_wasCameraMoved)
				{
					_cameraWorldRectangle = new Rectangle(
						Convert.ToInt32(Pos.X -((GameSettings.GetResolution().X / 2) / Zoom)),
						Convert.ToInt32(Pos.Y - ((GameSettings.GetResolution().Y / 2) / Zoom)),
						Convert.ToInt32(GameSettings.GetResolution().X / Zoom),
						Convert.ToInt32(GameSettings.GetResolution().Y / Zoom));
				}
				return _cameraWorldRectangle;
			}
		}

		public Vector2 ToWorldSpace(Vector2 position)
		{
			return Vector2.Transform(position, Matrix.Invert(_transform));
		}

		// Auxiliary function to move the camera
		public void Move(Vector2 amount)
		{
			_pos += amount;
			_wasCameraMoved = true;
		}
		public void Follow(Vector2 position)
		{
			if (_pos != position-GameSettings.GetResolution()/2)
			{
				_pos = position - GameSettings.GetResolution() / 2;
				_wasCameraMoved = true;
			}
		}
		// Get set position
		public Vector2 Pos
		{
			get { return _pos; }
		}
		public void SetPosition(float x, float y)
		{
			_pos.X = x;
			_pos.Y = y;
			_wasCameraMoved = true;
		}
		public Matrix GetTransformation(GraphicsDevice graphicsDevice)
		{
			if (_wasCameraMoved)
			{
				_transform =       // Thanks to o KB o for this solution
				  Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *
											 Matrix.CreateRotationZ(Rotation) *
											 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
											 Matrix.CreateTranslation(new Vector3(0, 0, 0));


			}
			return _transform;
		}
	}
}
