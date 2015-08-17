using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary
{
		//Copy this to your new project
		class EntryPoint : Game
		{
			private GraphicsDeviceManager graphics;
			private ScreenManager screenManager;
			private GameSettings gameSettings;


			public EntryPoint()
			{
				graphics = new GraphicsDeviceManager(this);


				screenManager = new ScreenManager(this);
				gameSettings = new GameSettings(graphics);

				Components.Add(screenManager);
				//Use this for your first screen
				//screenManager.AddScreen(new WhatEverScreen());
			}
		}
		static class Program
		{
			static void Main(string[] args)
			{
				using (EntryPoint game = new EntryPoint())
				{
					game.Run();
				}
			}
		}
}
