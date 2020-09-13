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

using GV    = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GameWindow {
	static class Home {
		static Home() { Reset(); }

		public static void Draw() {
			GV.MonoGameImplement.InitializeSpriteBatch(false);

			buttons.Draw();

			GV.MonoGameImplement.SpriteBatch.End(); 
		}

		private static void BuildButtonPositions() {
			int   width       = GV.Variables.windowWidth,                  height       = GV.Variables.windowHeight; // Store window dimensions
			float buttonWidth = (Button.SPRITE_WIDTH * BUTTON_SCALE) + 55, buttonHeight = Button.SPRITE_HEIGHT * BUTTON_SCALE;

			Vector2 position = new Vector2((width - buttonWidth) / 2, height - (3 * (buttonHeight + 25))); // Default button posiition
			Vector2 position2 = position + new Vector2(0, buttonHeight + 25); // Secondary button position, below previous
			Vector2 position3 = position + new Vector2(0, 2 * (buttonHeight + 25)); // Tertiary button position, again below previous

			TextButton startButton    = new TextButton(position, "Start",     (int)buttonWidth, (int)buttonHeight) { Layer = 0.4f };
			TextButton settingsButton = new TextButton(position2, "Settings", (int)buttonWidth, (int)buttonHeight) { Layer = 0.4f };
			TextButton exitButton     = new TextButton(position3, "Exit",     (int)buttonWidth, (int)buttonHeight) { Layer = 0.4f };

			startButton.Click += (self) => {
				GV.MonoGameImplement.gameState = GameState.SaveLoad;
				SaveLoad.WaitForInputToBeRemoved = true;
				SaveLoad.Reset(); // Reset new game state
			};

			settingsButton.Click += (self) => {
				GV.MonoGameImplement.gameState = GameState.Settings;
				Settings.WaitForInputToBeRemoved = true;
				Settings.Reset(); // Reset new game state
			};

			exitButton.Click += (self) => {
				GV.hollowAether.god.EndGame();
			};

			buttons = new ButtonList(0, 3, startButton, settingsButton, exitButton);
		}
	
		public static void Update() {
			buttons.Update(false);

			GV.PeripheralIO.ControlState controlState = GV.PeripheralIO.currentControlState;

			if (inputTimeout > 0) {
				if (controlState.KeyboardNotPressed() && controlState.GamepadNotPressed())
					inputTimeout = 0; // Remove input time out, user can input again
				else inputTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			} else {
				bool altPressed = GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.LeftAlt, Keys.RightAlt);

				if (controlState.KeyboardNotPressed() && controlState.GamepadNotPressed() || altPressed)
					return; // No input detected, skip forward to next update call

				if (controlState.Down || controlState.Right)   buttons.MoveToNextButton();
				else if (controlState.Up || controlState.Left) buttons.MoveToPreviousButton();

				else if (controlState.Gamepad.Jump || GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Enter))
					buttons.ActiveButton.InvokeClick(); // If any of the above inputs have been entered then execute

				inputTimeout = 300; // Dont accept input for given value in milleseconds
			}
		}

		public static void Reset() {
			BuildButtonPositions(); // Set the position of any buttons used by this class

			try {
				GV.MonoGameImplement.background = GV.MonoGameImplement.backgrounds["GW_HOME_SCREEN"];
			} catch {
				Console.WriteLine("Failed");
			}
		}

		public static void FullScreenReset(bool isFullScreen) {
			if (GV.MonoGameImplement.gameState == GameState.Home) Reset();
		}

		private const float BUTTON_SCALE = 3.5f;

		private static int inputTimeout = 0;

		private static ButtonList buttons;
	}
}
