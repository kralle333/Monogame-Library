using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Menus
{
	public class MenuSystem
	{
		private List<string> _menuItems = new List<string>();
		private Dictionary<string, MenuSystem> _subSystems = new Dictionary<string, MenuSystem>();

		private int _selectedItemIndex;
		public int SelectedItemIndex { get { return _selectedItemIndex; } set { _selectedItemIndex = value; } }

		private string _title;
		private Color _titleColor = Color.Blue;
		public Color TitleColor { get { return _titleColor; } set { _titleColor = value; } }
		private bool _isTitleSeen = false;
		public void ToggleTitle() { _isTitleSeen = !_isTitleSeen; }

		private SpriteFont _usedFont;
		private Vector2 _position;

		private Vector2 _itemOffsets;
		public Vector2 ItemOffsets { get { return _itemOffsets; } set { _itemOffsets = value; } }

		private Color _menuItemColor = Color.Black;
		public Color MenuItemColor { get { return _menuItemColor; } set { _menuItemColor = value; } }
		private Color _selectedMenuItemColor = Color.Red;
		public Color SelectedMenuItemColor { get { return _selectedMenuItemColor; } set { _selectedMenuItemColor = value; } }

		private Texture2D _menuFrame;
		public Texture2D MenuFrame { get { return _menuFrame; } set { _menuFrame = value; } }
		private Texture2D _selectedMenuFrame;
		public Texture2D SelectedMenuFrame { get { return _selectedMenuFrame; } set { _selectedMenuFrame = value; } }

		private MenuSystem _activeSubSystem;

		private string _pressedItem;
		public string PressedItem { get { return _pressedItem; } }

		/// <summary>
		/// Used for creating a new menu system. Default is a horizontal menu
		/// </summary>
		/// <param name="title">Displayed above the menu items, use ToggleTitle to actually see it</param>
		/// <param name="position">The position of the first menu item, the title is drawn above using standard offset</param>
		/// <param name="font">Font used to draw the menu items</param>
		public MenuSystem(string title, Vector2 position, SpriteFont font)
		{
			_title = title;
			_position = position;
			_usedFont = font;
			_itemOffsets = new Vector2(0, font.MeasureString("1234567890").Y * 1.5f);
		}

		public void AddItem(string itemText)
		{
			_menuItems.Add(itemText);
		}

		public void AddSubSystem(string itemText, MenuSystem subSystem)
		{
			_menuItems.Add(itemText);
			_subSystems[itemText] = subSystem;
			subSystem.AddBackButton(this);
		}
		public void AddBackButton(MenuSystem parent)
		{
			if (parent != null)
			{
				string backText = "Back: " + parent._title;
				_menuItems.Add(backText);
				_subSystems[backText] = parent;
			}
		}

		public void GoUp()
		{
			if (_activeSubSystem != null)
			{
				_activeSubSystem.GoUp();
			}
			else
			{
				if (_selectedItemIndex > 0)
				{
					_selectedItemIndex--;
				}
			}
		}
		public void GoDown()
		{
			if (_activeSubSystem != null)
			{
				_activeSubSystem.GoDown();
			}
			else
			{
				if (_selectedItemIndex < _menuItems.Count - 1)
				{
					_selectedItemIndex++;
				}
			}
		}
		public void PressItem()
		{
			if (_activeSubSystem != null)
			{
				_activeSubSystem.PressItem();
				//The button was a back button to the parent, meaning no active subsystem
				if (this == _activeSubSystem._activeSubSystem)
				{
					//The button was not trying to set a new subsystem, so remove the references
					_activeSubSystem._activeSubSystem = null;
					_activeSubSystem = null;
					
				}
			}
			else
			{
				string pressedItem = _menuItems[_selectedItemIndex];
				if (_subSystems.ContainsKey(pressedItem))
				{
					_activeSubSystem = _subSystems[pressedItem];
				}
				else
				{
					_pressedItem = pressedItem;
				}
			}
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (_activeSubSystem != null)
			{
				_activeSubSystem.Draw(spriteBatch);
			}
			else
			{
				if (_isTitleSeen)
				{
					spriteBatch.DrawString(_usedFont, _title, _position - (_itemOffsets * 1.2f), _titleColor, 0, Vector2.Zero, new Vector2(1.5f, 1.5f), SpriteEffects.None, 1f);
				}
				for (int i = 0; i < _menuItems.Count; i++)
				{
					spriteBatch.DrawString(_usedFont, _menuItems[i], _position + _itemOffsets * i, i == _selectedItemIndex ? _selectedMenuItemColor : _menuItemColor);
				}
			}

		}
	}
}
