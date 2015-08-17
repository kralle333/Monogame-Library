using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary
{
	public class Sprite
	{
		
		#region protected fields
		//Used to load content
		protected ContentManager _contentManager;

		//Used for drawing the sprite
		protected SpriteBatch _spriteBatch;

		//Used for loading the texture
		protected string _texturePath;
		
		//Used when TextureAtlases are used
		private string _textureAtlasPath;
		private string _textureAtlasName;
		private TextureAtlas _textureAtlas;
		protected TextureAtlas TextureAtlas { get { return _textureAtlas; } }
		private bool _usesTextureAtlas = false;

		//The texture of the sprite
		private Texture2D _texture;
		public Texture2D Texture { get { return _texture; } set { _texture = value; } }

		//Used to check if the sprite is hidden
		protected bool _isVisible = true;
		public bool isVisible { get { return _isVisible; } }

		//Used for drawing the sprite in the right depth
		protected float _depth = 0.0f;
		public float Depth { get { return _depth; } set { _depth = value; } }

		//Used for fading the sprite in or out
		private bool _isFadingIn = false;
		private bool _isFadingOut = false;
		private float _fadeDelay = 0.35f;
		private double _fadeTimer = 0.0f;
		private float _fadeSmoothness = 1.0f;
		private float _alphaLevel = 0.0f;

		private bool usesRotation = false;

		//Used for drawing the sprite
		private Rectangle _textureRectangle;
		public Rectangle TextureRectangle { get { return _textureRectangle; } }
		public virtual void SetTextureRectangle(Rectangle rectangle)
		{
			_textureRectangle = rectangle;
			_width = rectangle.Width;
			_height = rectangle.Height;
		}
		public virtual void SetTextureRectangle(string textureAtlasName)
		{
			if (_textureAtlas == null)
			{
				throw new Exception("Textureatlas was never loaded!");
			}
			TextureRegion region = _textureAtlas.GetRegion(textureAtlasName);
			if(region == null)
			{
				throw new Exception("Specified textureAtlasName: "+textureAtlasName+" was not found in atlas: "+_textureAtlasPath);
			}
			_textureRectangle = region.Bounds;
			_width = _textureRectangle.Width;
			_height = _textureRectangle.Height;
		}

		//Used to check if the sprite is animated
		private bool _isAnimated = false;

		//Used for animating the sprite
		private List<SpriteState> _animationStates = new List<SpriteState>();
		private SpriteState _currentState;
		private int previousCurrentFrame = 0;
        public bool IsAtLasAnimationFrame { get { return _currentState.NumberOfFrames - 1 == _currentState.currentFrame; } }
		private int animationCycles = 0;
		public int AnimationCycles { get { return animationCycles; } }
		private bool _isAnimationPaused = false;
		#endregion

		#region Public fields

		//White is default
		public Color color = Color.White;

		//Transform fields
		public Vector2 position;
		public float rotation = 0f;
		public float rotationSpeed = 0f;

		private int _width = 0;
		public int Width { get { return _width; } }

		private int _height = 0;
		public int Height { get { return _height; } }

		//Used for drawing rotating/scaled sprites - Default is x=width/2 and y=height/2
		private Vector2 _origin;
		public Vector2 Origin { get { return _origin; } set { _origin = value; } }
		public Vector2 Center { get { return position + new Vector2(_width / 2, _height / 2); } }

		//Used for scaling the sprite
		private float _scale = 1;
		public float Scale { get { return _scale; } set { _scale = value; } }


		//Sprite overlays
		protected List<Sprite> _spriteOverlays = new List<Sprite>();
		public List<Sprite> SpriteOverlays { get { return _spriteOverlays; } }


		/// <summary>
		/// SpriteEffect of the sprite. Can be used to flipping the sprite - Default is no effects
		/// </summary>
		public SpriteEffects spriteEffects = SpriteEffects.None;

		

		private Rectangle _collisionRectangle = new Rectangle();
		public Rectangle CollisionRectangle
		{
			get
			{
				_collisionRectangle.X = (int)position.X;
				_collisionRectangle.Y = (int)position.Y;
				_collisionRectangle.Width = Width;
				_collisionRectangle.Height = Height;
				return _collisionRectangle;
			}
		}
		#endregion

		#region Constructors
		public Sprite(string texturePath, Vector2 position, float depth)
		{
			_texturePath = texturePath;
			this.position = position;
			_depth = depth;
		}
		public Sprite(string texturePath, int xCoordinate, int yCoordinate, float depth) : 
			this(texturePath, new Vector2(xCoordinate, yCoordinate), depth) { }

		public Sprite(string texturePath, string textureAtlasPath, string textureAtlasName,int xCoordinate, int yCoordinate,float depth):
			this(texturePath,new Vector2(xCoordinate,yCoordinate),depth)
			{
				_textureAtlasPath = textureAtlasPath;
				_textureAtlasName = textureAtlasName;
				_usesTextureAtlas = true;
			}

		/// <summary>
		/// Use for animated sprites
		/// </summary>
		public Sprite(string texturePath, Vector2 position, int width, int height, bool animated, float depth)
		{
			_texturePath = texturePath;
			this.position = position;
			_width = width;
			_height = height;
			_isAnimated = animated;
			_depth = depth;
		}
		/// <summary>
		/// Use for animated sprites
		/// </summary>
		public Sprite(string texturePath, int xCoordinate, int yCoordinate, int width, int height, bool animated, float depth) :
			this(texturePath, new Vector2(xCoordinate, yCoordinate), width, height, animated, depth) { }
		
		/// <summary>
		/// Used for sprites that are not animated
		/// </summary>
		public Sprite(string texturePath, int tileX, int tileY, int width, int height, int xPos, int yPos, float depth):
			this(texturePath,xPos,yPos,depth)
		{
			_textureRectangle = SpriteHelper.GetSpriteRectangle(width, height, 0, tileX, tileY);
			_width = width;
			_height = height;
			_isAnimated = false;
		}
		
		/// <summary>
		/// Used for sprites that are not animated
		/// </summary>
		public Sprite(string texturePath,int xPos, int yPos, Rectangle tileSheetRectangle,float depth) :
			this(texturePath, xPos, yPos, depth)
		{
			_textureRectangle = tileSheetRectangle;
			_width = tileSheetRectangle.Width;
			_height = tileSheetRectangle.Height;
			_isAnimated = false;
		}
		#endregion

		#region Loading and drawing
		protected virtual void InitiateAnimationStates() { }
		/// <summary>
		/// GameScreen class uses this for both loading and setting the spritebatch. Called when using Add();
		/// </summary>
		public virtual void LoadContent(ContentManager contentManager, SpriteBatch spriteBatch)
		{
			try
			{
				_contentManager = contentManager;
				_spriteBatch = spriteBatch;
				_texture = _contentManager.Load<Texture2D>(_texturePath);

				if (_width == 0 && _height == 0) 
				{ 
					_width = _texture.Width; 
					_height = _texture.Height; 
					_textureRectangle = new Rectangle(0, 0, _width, _height); 
				}
				if (_usesTextureAtlas)
				{
					_textureAtlas = _contentManager.Load<TextureAtlas>(_textureAtlasPath);
					SetTextureRectangle(_textureAtlasName);
				}
				if (usesRotation)
				{
					_origin = new Vector2(_width / 2, _height / 2);
				}
				foreach (Sprite sprite in _spriteOverlays)
				{
					sprite.LoadContent(contentManager, spriteBatch);
				}
				InitiateAnimationStates();
			}
			catch(Exception e)
			{
				throw new Exception("Unable to load content at path: "+_texturePath+"\n"+e.InnerException);
			}
		}
		/// <summary>
		/// Used for manual loading of the textures of sprites - Use only when not using Add()
		/// </summary>
		public void LoadContent(ContentManager contentManager)
		{
			LoadContent(contentManager, null);
		}
		
		/// <summary>
		/// Used by GameScreen to draw sprites
		/// </summary>
		public virtual void Draw(GameTime gameTime)
		{
			if (_isVisible)
			{
				if(_isAnimated && _currentState != null)
				{
					if (!_isAnimationPaused)
					{
						_currentState.UpdateFrame((float)gameTime.ElapsedGameTime.TotalSeconds);
					}
					_textureRectangle = _currentState.GetCurrentFrameRectangle();
					
					if (previousCurrentFrame != _currentState.currentFrame)
					{
						animationCycles++;
						previousCurrentFrame = _currentState.currentFrame;
					}
				}
				rotation += rotationSpeed;
				//Rounding the position to integers ensures less blurriness
				position.X = (int)position.X;
				position.Y = (int)position.Y;
				_spriteBatch.Draw(_texture, position, _textureRectangle, Color.Lerp(color, Color.Transparent, _alphaLevel / 255), rotation, _origin, _scale, spriteEffects, _depth);

				foreach (Sprite sprite in _spriteOverlays)
				{
					sprite.Draw(gameTime);
				}

				//I dont think this works
				if (_isFadingIn)
					UpdateFadeIn(gameTime);
				else if (_isFadingOut)
					UpdateFadeOut(gameTime);
			}
		}
		/// <summary>
		/// Used for manual drawing
		/// </summary>
		public void Draw(SpriteBatch spriteBatch,GameTime gameTime)
		{
			_spriteBatch = spriteBatch;
			Draw(gameTime);
		}
		public virtual void Update(GameTime gameTime) { }

		
		
		private void UpdateFadeIn(GameTime gameTime)
		{
			_fadeTimer -= gameTime.ElapsedGameTime.TotalSeconds;
			if (_fadeTimer <= 0)
			{
				_fadeTimer = _fadeDelay;
				_alphaLevel += _fadeSmoothness;
				if (_alphaLevel >= 255)
				{
					_isFadingIn = false;
				}
			}
		}
		private void UpdateFadeOut(GameTime gameTime)
		{
			_fadeTimer -= gameTime.ElapsedGameTime.TotalSeconds;
			if (_fadeTimer <= 0)
			{
				_fadeTimer = _fadeDelay;
				_alphaLevel -= _fadeSmoothness;
				if (_alphaLevel <= 0)
				{
					_isFadingOut = false;
				}
			}
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Hides the sprite
		/// </summary>
		public void Hide()
		{
			_isVisible = false;
		}

		/// <summary>
		/// Makes the sprite visible
		/// </summary>
		public void Show()
		{
			_isVisible = true;
		}

		//Fix fade in and out
		/// <summary>
		/// Used for fading the sprite in
		/// </summary>
		public void FadeIn(float fadeTime, float fadeSmoothness)
		{
			if (_isFadingIn == false)
			{
				_isFadingIn = true;
				_fadeSmoothness = fadeSmoothness;
				_fadeDelay = fadeSmoothness / fadeTime;
				_alphaLevel = 0;
			}
		}

		/// <summary>
		/// Used for fading the sprite out
		/// </summary>
		public void FadeOut(float fadeTime, float fadeSmoothness)
		{
			if (_isFadingOut == false)
			{
				_isFadingOut = true;
				_fadeSmoothness = fadeSmoothness/fadeTime;
				_fadeDelay = _fadeSmoothness;
				_alphaLevel = 255;
			}
		}

		/// <summary>
		/// Used for setting the origin of the sprite. 
		/// This is used when scaling or rotating the sprite
		/// </summary>
		public void SetOrigin(int x, int y)
		{
			_origin = new Vector2(x, y);
		}

		/// <summary>
		/// Set the scale of the sprite. 1 is the original size.
		/// </summary>
		public void SetScale(float scale)
		{
			_scale = scale;
		}

		/// <summary>
		/// Checks if the sprite is colliding with another sprite, returns true if this is the case and return false if not.
		/// Warning: Very simple collision check
		/// </summary>
		public bool CheckRectangluarCollision(Sprite sprite)
		{
			if (sprite.CollisionRectangle.Intersects(CollisionRectangle))
			{
				return true;
			}
			return false;
		}
		/// <summary>
		/// Checks if the sprite is colliding with rectangle, returns true if this is the case and return false if not.
		/// Warning: Very simple collision check
		/// </summary>
		public bool CheckRectangluarCollision(Rectangle rectangle)
		{
			_collisionRectangle = new Rectangle((int)position.X, (int)position.Y, _width, _height);
			if (rectangle.Intersects((_collisionRectangle)))
			{
				return true;
			}
			return false;
		}
		public void ClearAnimationStates()
		{
			_animationStates.Clear();
		}
		public void AddAnimationState(SpriteState state)
		{
			_animationStates.Add(state);
		}

		public void NextFrame()
		{
			_currentState.NextFrame();
		}
		public void SetFrame(int number)
		{
			_currentState.SetFrame(number);
		}
		public void SetCurrentAnimationState(string name)
		{
			foreach(SpriteState state in _animationStates )
			{
				if (state.name == name)
				{
					_currentState = state;
					_currentState.currentFrame = 0;
					animationCycles = 0;
					previousCurrentFrame = 0;
					break;
				}
			}
		}
		public void PauseAnimation()
		{
			_isAnimationPaused = true;
		}
		public void ResumeAnimation()
		{
			_isAnimationPaused = false;
			_isAnimated = true;
		}
		#endregion
	}
}
