using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MonoGameLibrary
{
	public class ScreenManager : DrawableGameComponent
	{
		#region Fields
		private List<GameScreen> _screens;
		private List<GameScreen> _screensToUpdate;
		private InputState inputState = new InputState(InputState.InputType.KeyboardOnly);
		public Matrix SpriteScale;
		public bool isInitialized = false;
		public SpriteBatch spriteBatch;
		public GameTime currentGameTime;
		private Color _clearColor = Color.CornflowerBlue;
		public Color ClearColor { get { return _clearColor; } set { _clearColor = value; } }
		#endregion

		#region Constructors
		public ScreenManager(Game game)
			: base(game)
		{
			_screens = new List<GameScreen>();
			_screensToUpdate = new List<GameScreen>();
		}
		#endregion
		public override void Initialize()
		{

			base.Initialize();
			SamplerState customState = new SamplerState();
			customState.Filter = TextureFilter.Linear;
			GraphicsDevice.SamplerStates[0] = customState;
			isInitialized = true;
		}
		#region MonoGame methods
		protected override void LoadContent()
		{
			base.LoadContent();
			spriteBatch = new SpriteBatch(GraphicsDevice);
			float screenscale = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
			SpriteScale = Matrix.CreateScale(screenscale, screenscale, 1);
		}
		public override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(_clearColor);
			base.Draw(gameTime);
			DrawScreens(gameTime);
		}
		public override void Update(GameTime gameTime)
		{
			UpdateScreens(gameTime);
			currentGameTime = gameTime;
		}
		#endregion

		#region Private Methods
		private void UpdateScreens(GameTime gameTime)
		{
			_screensToUpdate.Clear();
			//Find out which to update
			foreach (GameScreen gameScreen in _screens)
			{
				_screensToUpdate.Add(gameScreen);
			}

			bool covered = false;
			inputState.UpdateStates();
			while (_screensToUpdate.Count > 0)
			{
				GameScreen gameScreen = _screensToUpdate[_screensToUpdate.Count - 1];
				_screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);
				gameScreen.UpdateState(gameTime, covered);
				if (gameScreen.state == GameScreen.ScreenState.Active)
				{
					gameScreen.HandleInput(inputState);
					gameScreen.Update(gameTime);
					if (gameScreen.Type == GameScreen.ScreenType.Standard)
					{
						covered = true;
					}
				}
			}
			inputState.UpdatePrevStates();
			//FOR DEBUG
			if (inputState.IsKeyPressed(Keys.Escape)) { Game.Exit(); }
		}
		private void DrawScreens(GameTime gameTime)
		{
			foreach (GameScreen screen in _screens)
			{
				if (screen.state != GameScreen.ScreenState.Hidden)
				{
					screen.Draw(gameTime);
				}
			}
		}
		#endregion

		#region Public Methods
		public void AddScreen(GameScreen gameScreen)
		{
			gameScreen.ScreenManager = this;
			_screens.Add(gameScreen);
		}
		public void RemoveScreen(GameScreen gameScreen)
		{
			_screens.Remove(gameScreen);
			_screensToUpdate.Remove(gameScreen);
		}
		#endregion
	}
}
