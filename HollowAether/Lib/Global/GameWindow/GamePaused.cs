using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GameWindow {
	public static class GamePaused {
		public static void Draw() {
			GameRunning.Draw(); // Set as backdrop

			GV.MonoGameImplement.InitializeSpriteBatch(false);

			var span = new Rectangle(0, 0, GV.Variables.windowWidth, GV.Variables.windowHeight);
			GV.MonoGameImplement.SpriteBatch.Draw(overlayTexture, span, Color.White * 0.75f);

			Vector2 size = Font.MeasureString("Paused"), position = new Vector2() {
				X = GV.Variables.windowWidth - size.X - 25, Y = 25 //GV.Variables.windowHeight
			};

			GV.MonoGameImplement.SpriteBatch.DrawString(Font, "Paused", position, Color.White);

			GV.MonoGameImplement.SpriteBatch.End();
		}

		public static void Update() {
			if (GV.PeripheralIO.ImplementWaitForInputToBeRemoved(ref WaitForInputToBeRemoved))
				return; // If Still Waiting For Input Retrieval Then Return B4 Update

			if (GV.PeripheralIO.currentControlState.Pause) {
				GV.MonoGameImplement.gameState = GameState.GameRunning;
				GameRunning.WaitForInputToBeRemoved = true;
			}
		}

		public static void Reset() {

		}

		public static bool WaitForInputToBeRemoved = false;

		private static Texture2D overlayTexture { get { return GV.MonoGameImplement.textures[overlayTextureKey]; } }
		private static SpriteFont Font { get { return GV.MonoGameImplement.fonts[fontKey]; } }

		private static readonly String overlayTextureKey = @"backgrounds\windows\playerdeadoverlay", fontKey = @"homescreen";
	}
}
