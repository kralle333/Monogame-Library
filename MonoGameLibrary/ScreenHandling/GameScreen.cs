using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameLibrary
{
	public abstract class GameScreen
	{
		#region Fields
		public enum ScreenState { Hidden,Active, TransitionOn, TransitionOff }
		public enum ScreenType { Standard, Popup, Overlay }
		//The time it takes to transition the screen on
		private float _transitionOnTime = 0.0f;
		public float TransitionOnTime { get { return _transitionOnTime; } }

		//The time it takes to transition the screen off
		private float _transitionOffTime = 0.0f;
		public float TransitionOffTime { get { return _transitionOffTime; } }

		private float _transitionTimer = 0.0f;

		//For fading sprites
		private float _spritesFadeSmoothness = 1;
		private bool _spritesAreFading = false;

		//State of the screen
		public ScreenState state;

		//The type affects whether the screen will be focused, drawn when covered and so on
		private ScreenType _screenType;
		public ScreenType Type { get { return _screenType; } }

		//ScreenManager the screen belongs to
		protected ScreenManager screenManager;
		public ScreenManager ScreenManager
		{
			get { return screenManager; }
			set
			{
				if (screenManager == null)
				{
					screenManager = value;
				}
				else
				{
					throw new Exception("Screen manager already set, something is wrong");
				}
			}
		}

		//ContentManager used for loading sprites
		protected ContentManager _contentManager;

		//SpriteBatch used for drawing sprites
		protected SpriteBatch _spriteBatch;

		//Is the screen a pop up?
		private bool _isPopUp;
		public bool IsPopUp { get { return _isPopUp; } }

		private bool graphicsInstantiated = false;

		//used for moving the camera and zooming
		protected Camera2D camera = new Camera2D();

		//Is the screen only covering temporary? Used if you do not want to remove the covered screen
		private bool _isTempCovering;
		public bool IsTempCovering { get { return _isTempCovering; } }

		//Sprites attached to the gamescreen
		private List<Sprite> _attachedSprites = new List<Sprite>();

		#endregion

		#region Constructors
		public GameScreen(ScreenType screenType)
		{
			_screenType = screenType;
		}
		public GameScreen(bool isPopUp, bool isTempCovering)
		{
			_isPopUp = isPopUp;
			_isTempCovering = isTempCovering;
		}
		public GameScreen(bool isPopUp, bool isTempCovering, float transitionOnTime, float transitionOffTime):this(isPopUp,isTempCovering)
		{
			_transitionOnTime = transitionOnTime;
			_transitionOffTime = transitionOffTime;
		}
		public GameScreen(bool isPopUp, bool isTempCovering, float transitionOnTime, float transitionOffTime, int spritesFadeSmoothness):
			this(isPopUp,isTempCovering,transitionOnTime,transitionOffTime)
		{
			_spritesFadeSmoothness = spritesFadeSmoothness;
		}
		#endregion

		#region Methods
		public virtual void HandleInput(InputState inputState) { }
		public virtual void LoadContent() 
		{
			InstantiateGraphics();
		}
		
		public void Draw(GameTime gameTime)
		{
			if (graphicsInstantiated)
			{
				_spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, camera.GetTransformation(screenManager.GraphicsDevice));
				DrawSprites(gameTime);
				_spriteBatch.End();
				CustomDraw(gameTime);
				
			}
			else
			{
				InstantiateGraphics();
			}
		}
		private void DrawSprites(GameTime gameTime)
		{
			foreach (Sprite sprite in _attachedSprites)
			{
				#region Checks if the sprites are currently fading
				if (!_spritesAreFading)
				{
					if (state == ScreenState.TransitionOff)
					{
						sprite.FadeOut(_transitionOffTime, _spritesFadeSmoothness);
						_spritesAreFading = true;
					}
					else if (state == ScreenState.TransitionOn)
					{
						sprite.FadeIn(_transitionOnTime, _spritesFadeSmoothness);
						_spritesAreFading = true;
					}
				}
				#endregion
				sprite.Draw(gameTime);
			}
		}
		public virtual void CustomDraw(GameTime gameTime) { }
		public virtual void Update(GameTime gameTime)
		{
			foreach (Sprite sprite in _attachedSprites)
			{
				sprite.Update(gameTime);
			}
		}
		public void InstantiateGraphics()
		{
			if (!graphicsInstantiated)
			{
				_spriteBatch = screenManager.spriteBatch;
				_contentManager = new ContentManager(screenManager.Game.Services, "Content\\");
				graphicsInstantiated = true;
			}
		}

		public void Add(Sprite sprite)
		{
			if (graphicsInstantiated)
			{
				sprite.LoadContent(_contentManager, _spriteBatch);
				_attachedSprites.Add(sprite);
				foreach (Sprite overlay in sprite.SpriteOverlays)
				{
					overlay.LoadContent(_contentManager, _spriteBatch);
					_attachedSprites.Add(overlay);
				}
			}
			else
			{
				throw new Exception("base.LoadContent must be called before adding");
			}
		}

		public void AddCollection(SpriteCollection sprites)
		{
			foreach (Sprite sprite in sprites.sprites)
			{
				Add(sprite);
			}
		}
		public void Remove(Sprite sprite)
		{
			_attachedSprites.Remove(sprite);
		}
		public void FadeOutThenIn(float fadeTime, float fadeSmoothness)
		{
			foreach (Sprite sprite in _attachedSprites)
			{
				sprite.FadeOut(fadeTime, fadeSmoothness);
			}
			foreach (Sprite sprite in _attachedSprites)
			{
				sprite.FadeIn(fadeTime, fadeSmoothness);
			}
		}
		public virtual void UpdateState(GameTime gameTime, bool covered)
		{
			if (covered)
			{
				if (StillTransitioning(gameTime, _transitionOffTime))
				{
					state = ScreenState.TransitionOff;
				}
				else
				{
					state = ScreenState.Hidden;
				}
			}
			else if(state != ScreenState.Active)
			{
				if (StillTransitioning(gameTime, _transitionOnTime) == false)
				{
					state = ScreenState.Active;
					LoadContent();
				}
				else
				{
					state = ScreenState.TransitionOn;
				}
			}
		}
		private bool StillTransitioning(GameTime gameTime, float transitionTime)
		{
			_transitionTimer += gameTime.ElapsedGameTime.Milliseconds;
			if (_transitionTimer >= transitionTime)
			{
				_transitionTimer = 0.0f;
				return false;
			}
			else
			{
				return true;
			}
		}
		#endregion
	}
}
