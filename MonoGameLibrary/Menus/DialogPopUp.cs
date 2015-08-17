using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary
{
	class DialogPopUp : GameScreen
	{
		private enum Symbol { Wait, Char, None, Click,Switch };
		private List<string> _messages = new List<string>();
		private bool _parsing = false;
		private Symbol _currentSymbol = Symbol.None;
		private string _currentString = "";
		private string _drawString = "";
		private string _clickDots = "";
		private double _clickDotTimer = 0;
		private SpriteFont _font;
		Sprite frame;
		double waitTime = 0;
		double timer = 0;
		bool _loadedFont = false;
		private string _fontPath;
		public bool doneShowing = false;

		public DialogPopUp(string[] messages,string texturePath,string fontPath)
			: base(true, false)
		{
			_messages = messages.ToList();
			_fontPath = fontPath;
			_currentString = _messages[0];
			frame = new Sprite(texturePath, new Vector2(400, 400), 0.4f);
			Add(frame);
		}
		public override void LoadContent()
		{
			ContentManager contentManager = new ContentManager(screenManager.Game.Services, "Content\\");
			_font = contentManager.Load<SpriteFont>(_fontPath);
			_loadedFont = true;
		}
		public override void HandleInput(InputState inputState)
		{
			if (_currentSymbol == Symbol.Wait && timer < waitTime)
			{
				timer += screenManager.currentGameTime.ElapsedGameTime.Milliseconds;

			}
			else if (_currentSymbol == Symbol.Wait && timer >= waitTime)
			{
				timer = 0;
				waitTime = 0;
				_currentSymbol = Symbol.None;
			}
			else if (_currentSymbol == Symbol.Switch)
			{
				if (inputState.IsKeyNewPressed(Keys.X))
				{
					_currentSymbol = Symbol.None;
				}
			}
			if (_currentSymbol == Symbol.Click && inputState.IsKeyNewPressed(Keys.X))
			{
				_currentSymbol = Symbol.None;
				_clickDots = "";
				if (_currentString.Length == 0)
				{
					if (_messages.Count() > 1)
					{
						Console.WriteLine(_messages.Count());
						_messages.Remove(_messages[0]);
						_currentString = _messages[0];
						_drawString = "";
					}
					else
					{
						//Event handling was made here before - Removed because it did not work as intented
					}
				}
			}
			else if (_currentSymbol == Symbol.Char)
			{
				_currentSymbol = Symbol.None;
			}
			if (!_parsing && timer>= waitTime && _currentSymbol == Symbol.None)
			{
				if (_currentString.Length > 0)
				{
					ParseNextSymbol();
				}
				else
				{
					_currentSymbol = Symbol.Click;
				}
			}
		}

		public override void CustomDraw(GameTime gameTime)
		{
			if (_loadedFont)
			{
				if (_currentSymbol == Symbol.Click)
				{
					DrawDots(gameTime);
				}
				screenManager.spriteBatch.DrawString(_font, _drawString + _clickDots, new Vector2(150, 350), Color.White,0,Vector2.Zero,1,SpriteEffects.None,0);
			}
			else
			{
				LoadContent();
			}
		}
		private void DrawDots(GameTime gameTime)
		{
			if ( _clickDotTimer > 300)
			{
				_clickDotTimer = 0;
				if (_clickDots.Count() < 3)
				{
					_clickDots += ".";
				}
				else
				{
					_clickDots = "";
				}
			}
			else
			{
				_clickDotTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
			}
		}
		private void ParseNextSymbol()
		{
			_parsing = true;
			int index = 0;
			string currentString = _currentString;
			Symbol symbol = Symbol.None;
			while (symbol == Symbol.None)
			{
				switch (currentString[index])
				{
					case '%': index++;
						if (currentString[index] == 'w')
						{
							string num = "";
							index++;
							while (symbol == Symbol.None)
							{
								switch (currentString[index])
								{
									case '0': num += '0'; index++; break;
									case '1': num += '1'; index++; break;
									case '2': num += '2'; index++; break;
									case '3': num += '3'; index++; break;
									case '4': num += '4'; index++; break;
									case '5': num += '5'; index++; break;
									case '6': num += '6'; index++; break;
									case '7': num += '7'; index++; break;
									case '8': num += '8'; index++; break;
									case '9': num += '9'; index++; break;
									default: symbol = Symbol.Wait; index++; waitTime = Int32.Parse(num); break;
								}

								if (index >= currentString.Count())
								{
									symbol = Symbol.Wait;
									waitTime = Int32.Parse(num);
									break;
								}
							}
						}
						else if (currentString[index] == 'c')
						{
							symbol = Symbol.Click;
							index++;
							
						}break;
					default: symbol = Symbol.Char; _drawString += _currentString[index]; index++; break;
				}
			}
			_currentString = currentString.Remove(0, index);
			_currentSymbol = symbol;
			_parsing = false;
		}

	}
}
