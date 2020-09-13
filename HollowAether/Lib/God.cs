#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GameWindow;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Debug;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.GAssets.Items;
using FC = HollowAether.Lib.GAssets.FX.ValueChangedEffect.FlickerColor;
using HollowAether.Lib.GAssets.FX;
#endregion

namespace HollowAether.Lib {
	public class God {
		public God() {
			
		}

		public void Initialize() {
			GV.FileIO.settingsManager = new SettingsManager();

			GV.MonoGameImplement.Player = new Player(); // Create Player First!!!

			GV.MonoGameImplement.camera = new Camera2D(zoom: GV.MonoGameImplement.gameZoom);

			GV.MonoGameImplement.map = new Map();
			GV.MonoGameImplement.map.Initialize();

			GV.MonoGameImplement.GameHUD = new HUD();

			#if DEBUG // Will have no impact on release build CPU usage
			DebugPrinter.Add("Position",       null);
			DebugPrinter.Add("CameraPosition", null);
			DebugPrinter.Add("BurstPoints",    null);
			DebugPrinter.Add("BurstPointWait", null);
			DebugPrinter.Add("Money",          null);
			DebugPrinter.Add("Intersected",    null);
			DebugPrinter.Add("ObjectsCount",   null);
			#endif
		}

		public void Update(GameTime gt) {
			GV.MonoGameImplement.gameTime = gt;

			#region ControlUpdates
			GV.PeripheralIO.previousKBState = GV.PeripheralIO.currentKBState;
			GV.PeripheralIO.currentKBState  = Keyboard.GetState();
			GV.PeripheralIO.previousGPState = GV.PeripheralIO.currentGPState;
			GV.PeripheralIO.currentGPState  = GamePad.GetState(PlayerIndex.One);

			GV.PeripheralIO.previousControlState = GV.PeripheralIO.currentControlState;
			GV.PeripheralIO.currentControlState.Update(); // Assign new ControlState vars
			#endregion
			 
			if (GV.PeripheralIO.currentKBState.IsKeyDown(Keys.Enter) && // Enter has to be pressed
				GV.PeripheralIO.CheckMultipleKeys(keys: new Keys[] { Keys.LeftAlt, Keys.RightAlt }))
				GV.hollowAether.ToggleFullScreen(); // fscreen->!fscreen || !fscreen->fscreen

			if (GV.PeripheralIO.currentKBState.IsKeyDown(Keys.Escape)) EndGame();

			GameWindows.UpdateCurrentGameWindow();

			if (GV.MonoGameImplement.background != null)
				GV.MonoGameImplement.background.Update();
		}

		public void EndGame() {
			if (GV.FileIO.settingsManager != null)
				GV.FileIO.settingsManager.Save();

			GV.hollowAether.Exit();
		}

		public void Draw() {
			if (GV.MonoGameImplement.background != null)
				GV.MonoGameImplement.background.Draw();

			GameWindows.DrawCurrentGameWindow();
		}
	}
}
