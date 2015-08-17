using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary
{
	public class InputState
	{
		#region Fields
		public enum InputType { KeyboardAndMouse, MouseOnly, KeyboardOnly }
		private InputType _type;
		public InputType Type { get { return _type; } }
		private PlayerIndex _activePlayerIndex = PlayerIndex.One;
		public PlayerIndex ActivePlayerIndex 
		{ 
			set 
			{
				_activePlayerIndex = value; 
			} 
			get 
			{
				return _activePlayerIndex; 
			} 
		}

		private KeyboardState _currentKeyboardState;
		private KeyboardState _previousKeyboardState;

		private MouseState _currentMouseState;
		private MouseState _previousMouseState;

		private GamePadState[] _currentGamePadState = new GamePadState[4];
		private GamePadState[] _previousGamePadState = new GamePadState[4];
		#endregion

		#region Constructor
		public InputState(InputType type)
		{
			_type = type;
		}
		#endregion

		#region Methods
		public void UpdateStates()
		{
			_currentKeyboardState = Keyboard.GetState();
			for (int i = 0; i < 4; i++)
			{
				_currentGamePadState[i] = GamePad.GetState((PlayerIndex)i);
			}
#if WINDOWS
			_currentMouseState = Mouse.GetState();
#endif
	
		}
		public void UpdatePrevStates()
		{

			_previousMouseState = _currentMouseState;
			for (int i = 0; i < 4; i++)
			{
				if (_currentGamePadState[i].IsConnected)
				{
					_previousGamePadState[i] = _currentGamePadState[i];
				}
			}
			_previousKeyboardState = _currentKeyboardState;
		}
		#region Keyboard methods
		public bool IsKeyPressed(Keys key)
		{
			return _currentKeyboardState.IsKeyDown(key);
		}
		public bool IsKeyUp(Keys key)
		{
			return _currentKeyboardState.IsKeyUp(key);
		}
		public bool IsKeyNewPressed(Keys key)
		{
			return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
		}
		public bool IsKeyNewReleased(Keys key)
		{
			return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
		}
		#endregion

#if WINDOWS
		#region Mouse methods
		#region Left button
		public bool IsMouseLeftButtonPressed()
		{
			if (_currentMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseLeftButtonReleased()
		{
			if (_currentMouseState.LeftButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseLeftButtonNewPressed()
		{
			if (_currentMouseState.LeftButton == ButtonState.Pressed &&
				_previousMouseState.LeftButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseLeftButtonNewReleased()
		{
			if (_currentMouseState.LeftButton == ButtonState.Released &&
				_previousMouseState.LeftButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseLeftNewClickingSprite(Sprite sprite)
		{
			if (IsMouseLeftButtonNewPressed() && IsMouseOver(sprite))
			{
				return true;
			}
			return false;
		}
		#endregion
		#region Right button
		public bool IsMouseRightButtonPressed()
		{
			if (_currentMouseState.RightButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseRightButtonReleased()
		{
			if (_currentMouseState.RightButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseRightButtonNewPressed()
		{
			if (_currentMouseState.RightButton == ButtonState.Pressed &&
				_previousMouseState.RightButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseRightButtonNewReleased()
		{
			if (_currentMouseState.RightButton == ButtonState.Released &&
				_previousMouseState.RightButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseRightNewClickingSprite(Sprite sprite)
		{
			if (IsMouseRightButtonNewPressed() && IsMouseOver(sprite))
			{
				return true;
			}
			return false;
		}
		#endregion
		#region Middle Button
		public bool IsMouseMiddleButtonPressed()
		{
			if (_currentMouseState.MiddleButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseMiddleButtonReleased()
		{
			if (_currentMouseState.MiddleButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseMiddleButtonNewPressed()
		{
			if (_currentMouseState.MiddleButton == ButtonState.Pressed &&
				_previousMouseState.MiddleButton == ButtonState.Released)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseMiddleButtonNewReleased()
		{
			if (_currentMouseState.MiddleButton == ButtonState.Released &&
				_previousMouseState.MiddleButton == ButtonState.Pressed)
			{
				return true;
			}
			return false;
		}
		public bool IsMouseMiddleNewClickingSprite(Sprite sprite)
		{
			if (IsMouseMiddleButtonNewPressed() && IsMouseOver(sprite))
			{
				return true;
			}
			return false;
		}
		#endregion

		#region Misc Methods
		public bool IsMouseOver(Sprite sprite)
		{
			if ((_currentMouseState.X >= sprite.position.X) && _currentMouseState.X <= (sprite.position.X + sprite.Width) &&
					_currentMouseState.Y >= sprite.position.Y && _currentMouseState.Y <= (sprite.position.Y + sprite.Height))
				return true;

			return false;
		}
		public float GetMouseScroll()
		{
			return _previousMouseState.ScrollWheelValue - _currentMouseState.ScrollWheelValue;
		}
		public Vector2 GetMousePosition()
		{
			return new Vector2(_currentMouseState.X, _currentMouseState.Y);
		}
		#endregion
		#endregion
		#endif
		
		#region Controller Methods
		public bool IsButtonPressed(Buttons button)
		{
			return _currentGamePadState[(int)_activePlayerIndex].IsButtonDown(button);
		}
		public bool IsButtonUp(Buttons button)
		{
			return _currentGamePadState[(int)_activePlayerIndex].IsButtonUp(button);
		}
		public bool IsButtonNewPressed(Buttons button)
		{

			return IsButtonPressed(button) && _previousGamePadState[(int)_activePlayerIndex].IsButtonUp(button);
		}
		public bool IsButtonNewReleased(Buttons button)
		{
			if ( _currentGamePadState[(int)_activePlayerIndex].IsButtonUp(button) &&
				_previousGamePadState[(int)_activePlayerIndex].IsButtonDown(button))
			{
				return true;
			}
			return false;
		}
		public enum StickPosition{Neutral,Left,Up,Down,Right,UpLeft,UpRight,DownLeft,DownRight}
		public bool IsLeftStickInPosition(StickPosition position)
		{
			return ConvertVectorDirectionToStickPosition(_currentGamePadState[(int)_activePlayerIndex].ThumbSticks.Left) == position;  
		}
		public bool IsRightStickInPosition(StickPosition position)
		{
			return ConvertVectorDirectionToStickPosition(_currentGamePadState[(int)_activePlayerIndex].ThumbSticks.Left) == position;  
		}
		public Vector2 GetLeftStickPosition()
		{
			return _currentGamePadState[(int)_activePlayerIndex].ThumbSticks.Left;
		}
		public Vector2 GetRightStickPosition()
		{
			return _currentGamePadState[(int)_activePlayerIndex].ThumbSticks.Right;
		}
		
		public static StickPosition ConvertVectorDirectionToStickPosition(Vector2 position)
		{
			const float slack = 0.5f;
			position.Normalize();
			if(position.X-slack>0)
			{
				if(position.Y+slack<0)
				{
					return StickPosition.DownRight;
				}
				else if(position.Y-slack>0)
				{
					return StickPosition.UpRight;
				}
				else
				{
					return StickPosition.Right;
				}
			}
			else if (position.X + slack < 0)
			{
				if(position.Y+slack<0)
				{
					return StickPosition.DownLeft;
				}
				else if (position.Y - slack > 0)
				{
					return StickPosition.UpLeft;
				}
				else
				{
					return StickPosition.Left;
				}
			}
			else
			{
				if(position.Y>0)
				{
					return StickPosition.Up;
				}
				else if(position.Y<0)
				{
					return StickPosition.Down;
				}
				
			}
			return StickPosition.Neutral;
		}
		#endregion
		//Saves the inputstate from reading input if it is not needed.
		public void ChangeInputType(InputType type)
		{
			_type = type;
		}
		#endregion
	}
}
