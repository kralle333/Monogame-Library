using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MonoGameLibrary
{
	//Used for frequently used fonts and other art for menus etc
	public class MenuTextureRegistry
	{
		
		public static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
		public static Dictionary<string, SpriteFont> fonts = new Dictionary<string, SpriteFont>();
		private static ContentManager _contentManager;

		public static void Initialize(ContentManager contentManager)
		{
			_contentManager = contentManager;
		}
		public static void AddFont(string name, string path)
		{
			SpriteFont font;
			try
			{
				font = _contentManager.Load<SpriteFont>(path);
			}
			catch (Exception e)
			{
				throw new Exception("Registry was not initialized, use Initalize(ContentManager contentManager)" + e.Message);
			}
			fonts.Add(name, font);
		}
		public static void AddTexture(string name, string path)
		{
			Texture2D texture;
			try
			{
				texture = _contentManager.Load<Texture2D>(path);
			}
			catch (Exception e)
			{
				throw new Exception("Registry was not initialized, use Initalize(ContentManager contentManager)" + e.Message);
			}
			textures.Add(name,texture);
		}
	}
}
