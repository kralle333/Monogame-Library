using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonoGameLibrary
{
	public class SpriteCollection
	{
		public List<Sprite> sprites = new List<Sprite>();

		public void Add(Sprite sprite)
		{
			sprites.Add(sprite);
		}
		public void Remove(Sprite sprite)
		{
			sprites.Remove(sprite);
		}
	}
}
