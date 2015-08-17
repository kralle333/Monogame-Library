using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary.Library
{
	class MenuPopUp : GameScreen
	{

		#region Private fields
		private Vector2 _position;
		private int _gapSize = 5;
		private string _text;
		private List<MenuEntry> _entries = new List<MenuEntry>();
		private int index = 0;
		private SpriteFont _font;
		private bool _isVisible = true;
		private bool _contentLoaded = false;
		private int _width;
		private int _height;
		private string ChosenIndex { get{return _entries[index].text;}}
		#endregion

		public MenuPopUp(int xCoordinate, int yCoordinate, int gapSize, int width, int height,string text,string[] entries)
			: base(true, false)
		{
			_position = new Vector2(xCoordinate, yCoordinate);
			_gapSize = gapSize;
			_text = text;
			foreach (string s in entries)
			{
				_entries.Add(new MenuEntry(s));
			}
			_width = width;
			_height = height;
		}
		public override void LoadContent()
		{
			ContentManager c = new ContentManager(screenManager.Game.Services, "Content//");
			_font = c.Load<SpriteFont>("MainGame//DialogFont");
			_contentLoaded = true;
			_gapSize += (int)_font.MeasureString("Height").Y;
		}
		public void MoveUp()
		{
			if (index > 0)
			{
				index--;
			}
		}
		public void MoveDown()
		{
			if (index < _entries.Count - 1)
			{
				index++;

			}
		}
		public override void HandleInput(InputState inputState)
		{
			if (inputState.IsKeyNewPressed(Keys.Up))
			{
				MoveUp();
			}
			if (inputState.IsKeyNewPressed(Keys.Down))
			{
				MoveDown();
			}
		}
		public override void CustomDraw(GameTime gameTime)
		{
			if (_contentLoaded)
			{
				if (_isVisible)
				{
					Color fontColor;
					screenManager.spriteBatch.DrawString(_font, _text, _position - new Vector2(0, _gapSize), Color.White);
					for (int i = 0; i < _entries.Count; i++)
					{
						MenuEntry itemToDraw = _entries.ElementAt(i);
						if (index == i)
						{
							fontColor = Color.Red;

						}
						else
						{
							fontColor = Color.White;
						}
						screenManager.spriteBatch.DrawString(_font, itemToDraw.text, _position + new Vector2(0, _gapSize * i), fontColor);
					}
				}
			}
			else
			{
				LoadContent();
			}
		}

	}
}
public class MenuEntry
{
	public string text;
	public MenuEntry(string text)
	{
		this.text = text;
	}
}
