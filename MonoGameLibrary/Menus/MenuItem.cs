using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace MonoGameLibrary
{
	public class MenuItem
	{
		int level = 0;
		MenuItem parent;
		public MenuItem childSelected;
		public MenuItem childMarked;
		public List<MenuItem> children = new List<MenuItem>();
		public bool currentlyMarked = false;
		public bool currentlySelected = false;
		public string text;
		public int index = 0;
		private int scrollIndex = 0;
		private int lastDrawnIndex = 0;
		private bool firstIndexSeen = true;
		private bool lastIndexSeen = false;
		private int currentLevel = 1;
		public Vector2 position;
		public bool locked = false;
		public int width = 0;
		public bool showingDescription = false;
		public bool scrollable = false;
		public string description;

		public Texture2D menuTexture;
		public Texture2D markedMenuTexture;
		public SpriteFont menuFont;

		public MenuItem(string text)
		{
			this.text = text;
			this.description = "No description";
		}
		public MenuItem(string text, string description)
		{
			this.text = text;
			this.description = description;
		}

		public MenuItem(string text, Vector2 position)
		{
			this.text = text;
			this.position = position;
			currentlySelected = true;
			this.description = "No description";
		}
		public void AddChild(MenuItem menuItem)
		{
			menuItem.level = level + 1;
			menuItem.parent = this;
			menuItem.position = position;


			menuItem.menuFont = menuFont;
			menuItem.markedMenuTexture = markedMenuTexture;
			menuItem.menuTexture = menuTexture;

			menuItem.position.Y = position.Y + ((children.Count+1) * (menuTexture.Height*1.5f));
			menuItem.position.X = position.X;

			children.Add(menuItem);
			if (children.Count == 1)
			{
				menuItem.currentlyMarked = true;
			}
		}
		public void HandleInput(InputState inputState)
		{
			if (!showingDescription)
			{
				if (currentlySelected && childSelected == null && children.Count >= 0)
				{

					if (inputState.IsKeyNewPressed(Keys.Up) && index > 0)
					{
						children[index].currentlyMarked = false;
						index--;
						children[index].currentlyMarked = true;
						childMarked = children[index];
						if (scrollable && !firstIndexSeen && index == 1)
						{
							scrollIndex--;
						}
					}
					else if (inputState.IsKeyNewPressed(Keys.Down) && index + 1 < children.Count)
					{
						children[index].currentlyMarked = false;
						index++;
						children[index].currentlyMarked = true;
						childMarked = children[index];
						if (scrollable && !lastIndexSeen && lastDrawnIndex == index)
						{
							scrollIndex++;
						}
					}
					else if (inputState.IsKeyNewPressed(Keys.Z))
					{
						if (level > 0)
						{
							currentlySelected = false;
							currentlyMarked = true;
							if (children.Count() > 0)
							{
								childMarked.currentlyMarked = false;
								childSelected = null;
							}
							if (parent != null)
							{
								parent.childSelected = null;
								parent.childMarked = parent.children[parent.index];
							}

						}
					}
					else if (inputState.IsKeyNewPressed(Keys.X) && children.Count() > 0 && !children[index].locked)
					{
						currentlySelected = true;
						currentlyMarked = false;
						children[index].currentlySelected = true;
						childSelected = children[index];
						if (parent != null)
						{
							parent.childMarked = null;
						}
						childMarked.currentlyMarked = false;
						childMarked = null;
						currentLevel++;
					}
					else if (inputState.IsKeyNewPressed(Keys.H) && childMarked != null)
					{
						showingDescription = true;
					}
				}
				else if (childSelected != null)
				{
					childSelected.HandleInput(inputState);
				}
			}
			else
			{
				showingDescription = false;
			}
		}
		public void Draw(SpriteBatch spriteBatch)
		{
			
			if (level == 0)
			{
				DrawChildren(spriteBatch);
			}
			else
			{
				if (currentlyMarked)
				{
					spriteBatch.Draw(markedMenuTexture, position, Color.White);
				}
				else if (currentlySelected)
				{
					spriteBatch.Draw(menuTexture, position, Color.White);
				}
			}
			spriteBatch.DrawString(menuFont, text, position + new Vector2(0, 0), Color.White);
				
		}
		public void DrawChildren(SpriteBatch spriteBatch)
		{
			if (scrollIndex == 0)
			{
				firstIndexSeen = true;
			}
			else if (scrollIndex >0)
			{
				firstIndexSeen = false;
			}
			for (int i = scrollIndex; i < children.Count; i++)
			{
				if (!scrollable)
				{
					if (i == 0 && childMarked == null)
					{
						children[0].currentlyMarked = true;
						childMarked = children[0];
					}
					if (children[i].currentlyMarked)
					{
						spriteBatch.Draw(markedMenuTexture, children[i].position, Color.White);
					}
					else if (children[i].currentlySelected)
					{
						spriteBatch.Draw(menuTexture, children[i].position, Color.White);
					}
					spriteBatch.DrawString(menuFont, children[i].text, children[i].position, Color.White);
				}
				else
				{
					lastDrawnIndex = i - 1;
					lastIndexSeen = false;
					break;
				}
				lastIndexSeen = true;
				if (scrollIndex > 0)
				{
					firstIndexSeen = false;
				}
			}
			if (childSelected != null)
			{
				childSelected.DrawChildren(spriteBatch);
			}
		}
		public MenuItem GetItem(string item)
		{
			if (text == item)
			{
				return this;
			}
			foreach (MenuItem child in children)
			{
				MenuItem i = child.GetItem(item);
				if (i != null)
				{
					return i;
				}
			}
			return null;
		}
		public int IsChildrenPressed(string item)
		{
			MenuItem i = GetItem(item);
			if (children.Count > 0)
			{
				for (int count = 0; count < i.children.Count; count++)
				{
					if (i.children[count].currentlySelected)
					{
						return count;
					}
				}
			}
			return -1;
		}
		public bool IsSelected(string item)
		{
			MenuItem i = GetItem(item);
			if (i.text == item && i.currentlySelected)
			{
				return true;
			}
			return false;
		}
		public void RemoveChild(string item)
		{
			MenuItem childFound = null;
			foreach (MenuItem child in children)
			{
				if (childFound != null)
				{
					child.position.X -= 80;
				}
				if (child.text == item)
				{
					childFound = child;
				}
				else
				{
					child.RemoveChild(item);
				}
			}
			children.Remove(childFound);
		}
		public string GetSelectedItemText(int level)
		{
			if (this.level == level)
			{
				if (currentlySelected)
				{
					return text;
				}
			}
			foreach (MenuItem child in children)
			{
				string text = child.GetSelectedItemText(level);
				if (text != "")
				{
					return text;
				}
			}
			return "";
		}
		public string GetMarkedItemText()
		{
			if (currentlyMarked)
			{
				return text;
			}
			foreach (MenuItem child in children)
			{
				string text = child.GetMarkedItemText();
				if (text != "")
				{
					return text;
				}
			}
			return "";
		}
		public MenuItem GetMarkedItem()
		{
			if (currentlyMarked)
			{
				return this;
			}
			foreach (MenuItem child in children)
			{
				MenuItem item = child.GetMarkedItem();
				if (item != null)
				{
					return item;
				}
			}
			return null;
		}
		public void Reset()
		{
			index = 0;
			childSelected = null;
			childMarked = null;
			currentlySelected = false;
			currentlyMarked = false;
			foreach (MenuItem child in children)
			{
				child.Reset();
			}
			if (level == 0)
			{
				currentlySelected = true;
				if (children.Count() > 0)
				{
					children[0].currentlyMarked = true;
				}
			}
		}
	}
}
