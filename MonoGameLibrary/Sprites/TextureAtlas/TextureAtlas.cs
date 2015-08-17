using System.Collections.Generic;

namespace MonoGameLibrary
{
	public class TextureAtlas
	{
		internal TextureAtlas(Dictionary<string, TextureRegion> regions)
		{
			Regions = regions;
		}

		public bool ContainsTexture(string textureName)
		{
			return Regions.ContainsKey(textureName);
		}
		
		public TextureRegion GetRegion(string textureName)
		{
			TextureRegion region;
			
			if(Regions.TryGetValue(textureName, out region))
				return region;
			return null;
		}

		Dictionary<string, TextureRegion> Regions { get; set; }
	}
}
