#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GameWindow {
	public static class GameWindows {
		public static void WindowSizeChanged() {
			GV.MonoGameImplement.backgrounds["GW_HOME_SCREEN"] = new Background(GetMovingHSCESLayers()) {
				new StretchedBackgroundLayer(@"backgrounds\misc\bg07_alt", new Frame(0, 0, 400, 240, 1, 1, 1), 0.5f),
			};
		}

		private static IEnumerable<MovingBackgroundLayer> GetMovingHSCESLayers(float scale=1.5f) {
			int HSCES_Width = 640, HSCES_Height = 125; // Determine home screen cloud sprite dimensions

			int cloudHeight	 = (int)(scale * HSCES_Height), yOffset = (int)(0.35f * cloudHeight);
			Vector2 bottom	 = new Vector2(0, (GV.Variables.windowHeight - cloudHeight));
			String HSCES_Key = @"backgrounds\homescreencloudeffect"; // Path to texture

			var frames = (from int X in Enumerable.Range(0, 4) select new Frame(0, X, HSCES_Width, HSCES_Height, HSCES_Width, HSCES_Height, 1)).ToArray();
			// Frames order = { Light, Medium Light, Medium Dark, Dark } from indexes { 0, 1, 2, 3 }. Easier to use LINQ instead of manually.

			Func<int, Vector2> GetPos = (X) => bottom - new Vector2(GV.Variables.random.Next(0, 720), X * yOffset);

			Vector2[] positions = (from X in Enumerable.Range(0, 4) select GetPos(X)).ToArray(); // Get all position-vars

			foreach (int X in Enumerable.Range(0, positions.Length)) {
				yield return new MovingBackgroundLayer(HSCES_Key, frames[X], (int)(scale * HSCES_Width), cloudHeight, 1.0f - (X * 0.1f)) {
					Offset = positions[X], Velocity = new Vector2(250 - (50 * X), 0), RepeatHorizontally = true // General variables 
				};
			}
		}

		public static void DrawCurrentGameWindow() {
			switch (GlobalVars.MonoGameImplement.gameState) {
				case (GameState.Home):           Home.Draw();			break;
				case (GameState.Settings):       Settings.Draw();		break;
				case (GameState.SaveLoad):       SaveLoad.Draw();		break;
				case (GameState.GameRunning):    GameRunning.Draw();	break;
				case (GameState.GamePaused):     GamePaused.Draw();     break;
				case (GameState.PlayerDeceased): PlayerDeceased.Draw(); break;
				default:
					throw new FatalException("No Game Window For Current Game State");
			}
		}

		public static void UpdateCurrentGameWindow() {
			switch (GV.MonoGameImplement.gameState) {
				case (GameState.Home):			 Home.Update();           break;
				case (GameState.Settings):		 Settings.Update();       break;
				case (GameState.SaveLoad):		 SaveLoad.Update();       break;
				case (GameState.GameRunning):	 GameRunning.Update();	  break;
				case (GameState.GamePaused):     GamePaused.Update();     break;
				case (GameState.PlayerDeceased): PlayerDeceased.Update(); break;
				default:
					throw new FatalException("No Game Window For Current Game State");
			}
		}

		public static void ResetCurrentGameWindow() {
			switch (GV.MonoGameImplement.gameState) {
				case (GameState.Home):			 Home.Reset();			 break;
				case (GameState.Settings):		 Settings.Reset();	     break;
				case (GameState.SaveLoad):		 SaveLoad.Reset();		 break;
				case (GameState.GameRunning):	 GameRunning.Reset();    break;
				case (GameState.GamePaused):     GamePaused.Reset();     break;
				case (GameState.PlayerDeceased): PlayerDeceased.Reset(); break;
				default:
					throw new FatalException("No Game Window For Current Game State");
			}
		}
	}
}
