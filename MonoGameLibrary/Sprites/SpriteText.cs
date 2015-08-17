using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary
{
	public class SpriteText:Sprite
	{
		public string text = "";
		private SpriteFont font;
		public SpriteText(string spriteFontPath,string text,Vector2 position):base(spriteFontPath,position,1)
		{
			this.text = text;
			color = Color.White;
		}
		public override void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			_spriteBatch = spriteBatch;
			font = contentManager.Load<SpriteFont>(_texturePath);
		}
		public override void Draw(GameTime gameTime)
		{
			_spriteBatch.DrawString(font, text, position, color);
		}
	}
}
