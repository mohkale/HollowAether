#region SystemImports
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
#endregion

using GV = HollowAether.Lib.GlobalVars;
using SUM = HollowAether.StartUpMethods;
using HollowAether.Lib.GAssets;

using GW = HollowAether.Lib.GameWindow;

namespace HollowAether {
	public class HollowAetherGame : Game {
		public HollowAetherGame() {
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";

			GV.Variables.windowWidth   = graphics.PreferredBackBufferWidth;
			GV.Variables.windowHeight  = graphics.PreferredBackBufferHeight;
			GV.Variables.displayWidth  = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
			GV.Variables.displayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

			FullScreenChanged = (newState) => { };

			FullScreenChanged += GW.Home.FullScreenReset;
			FullScreenChanged += GW.GameRunning.FullScreenReset;
			FullScreenChanged += GW.SaveLoad.FullScreenReset;
			FullScreenChanged += GW.Settings.FullScreenReset;
			FullScreenChanged += GW.PlayerDeceased.FullScreenReset;
			FullScreenChanged += GW.Settings.FullScreenReset;
		}
		
		protected override void Initialize() {
			base.Initialize();
			
			god = new God();
			god.Initialize();

			if (GV.FileIO.settingsManager.shouldBeFullScreen)
				IsFullScreen = true;
		}

		public void SetWindowDimensions(Vector2 XY) {
			SetWindowDimensions(X: (int)XY.X, Y: (int)XY.Y);
		}

		public void SetWindowDimensions(int X, int Y) {
			graphics.PreferredBackBufferWidth = X;
			graphics.PreferredBackBufferHeight = Y;

			GlobalVars.Variables.windowWidth = X;
			GlobalVars.Variables.windowHeight = Y;

			Lib.GameWindow.GameWindows.WindowSizeChanged();

			CentreAlignWindow(); // always align 
			graphics.ApplyChanges(); // Save Changes
		}

		public void CentreAlignWindow() {
			if (graphics.IsFullScreen) { return; } // Would Throw Error but BLARG :)

			Vector2 centerOfScreen = new Vector2(GlobalVars.Variables.displayWidth / 2, GlobalVars.Variables.displayHeight / 2);
			Vector2 windowOffsetXY = new Vector2(GlobalVars.Variables.windowWidth / 2, GlobalVars.Variables.windowHeight / 2);
			Window.Position = (centerOfScreen - windowOffsetXY).ToPoint(); // set centre position
		}

		public void ToggleFullScreen() {
			graphics.ToggleFullScreen(); // Switch type
			if (!graphics.IsFullScreen) CentreAlignWindow();
			FullScreenChanged(graphics.IsFullScreen);
		}

		protected override void LoadContent() {
			spriteBatch = new SpriteBatch(GraphicsDevice); // Built in, DO NOT REMOVE

			GV.Content.LoadContent();

			#region NonContentManagerContentDefinitions
			GV.MonoGameImplement.textures["debugFrame"] = GV.TextureCreation.GenerateBlankTexture(Color.White);
			GV.MonoGameImplement.textures["tri"] = GV.TextureCreation.GenerateBlankTexture(Color.Red);
			#endregion
		}

		protected override void UnloadContent() {
			GV.MonoGameImplement.textures["debugFrame"].Dispose();
			GV.MonoGameImplement.textures["tri"].Dispose();
		}
		
		protected override void Update(GameTime gameTime) {
			base.Update(gameTime);
			god.Update(gameTime);
		}
		
		protected override void Draw(GameTime gameTime) {
			GraphicsDevice.Clear(Color.Blue);
			
			god.Draw(); // Draw - pass to game
			
			base.Draw(gameTime);
		}

		public GraphicsDeviceManager graphics;
		public SpriteBatch spriteBatch;
		public God god;

		public static event Action<bool> FullScreenChanged;

		public bool IsFullScreen { get { return graphics.IsFullScreen; } set {
			if (graphics.IsFullScreen != value) ToggleFullScreen();
		} }
	}
}
