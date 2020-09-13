#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using HollowAether.Lib.GAssets;

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GameWindow {
	static class Settings {
		static Settings() {
			GV.MonoGameImplement.backgrounds["GW_SETTINGS"] = new Background() {
				new StretchedBackgroundLayer(@"backgrounds\windows\settingsbg", new Frame(0, 0, 1080, 720, 1, 1, 1), 0.5f),
			};
		}

		public static void Draw() {
			GV.MonoGameImplement.SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

			buttons.Draw();

			foreach (Button button in multiChoiceAssistiveButtons) button.Draw();

			GV.MonoGameImplement.SpriteBatch.End();
		}

		public static void Reset() {
			BuildButtons(); // Rebuild displayed full screen menu buttons
			GV.MonoGameImplement.background = GV.MonoGameImplement.backgrounds["GW_SETTINGS"];
		}

		private static void BuildButtons() {
			int width = GV.Variables.windowWidth, height = GV.Variables.windowHeight; // Store window dimensions for positioning
			float buttonWidth = (Button.SPRITE_WIDTH * BUTTON_SCALE) + 225, buttonHeight = Button.SPRITE_HEIGHT * BUTTON_SCALE;

			#region Build Buttons
			int yOffset = (int)(buttonHeight * 1.25f);
			
			TextButton toggleFullscreenButton = new TextButton(Vector2.Zero, "Toggle Fullscreen", (int)buttonWidth, (int)buttonHeight) {
				Position = new Vector2((width - buttonWidth) / 2, 0.25f * buttonHeight) // Center screen with primary offset vertically
			};

			#region BuildMultiChoiceButtons
			Vector2 fpsButtonPosition = toggleFullscreenButton.Position + new Vector2(buttonWidth / 2, yOffset);
			var fpsOptions = (from X in Enumerable.Range(10, 120) where X % 5 == 0 select X.ToString()).ToArray(); // Frames Per Second Options For Button To Select
			MultichoiceButton fpsButton = new MultichoiceButton(fpsButtonPosition, width: (int)buttonWidth, height: (int)buttonHeight, options: fpsOptions);
			int fpsIndex = Array.IndexOf(fpsOptions, GV.BasicMath.RoundToNearestMultiple(Convert.ToInt32(GV.MonoGameImplement.framesPerSecond), 5, true).ToString());
			fpsButton.SetSelectedOption(fpsIndex != -1 ? fpsIndex : 0); // If current frame value not found in options array then just set to 10 for now

			Vector2 screenSizeButtonPosition = toggleFullscreenButton.Position + new Vector2(buttonWidth / 2, 2 * yOffset);
			string[] sizeOptions = (from X in Enumerable.Range(10, 21) select $"{X * 100}x{X * 100 * 960 / 1600}").ToArray(); // Multiples, while maintaining aspect ratio
			MultichoiceButton screenSizeButton = new MultichoiceButton(screenSizeButtonPosition, width: (int)buttonWidth, height: (int)buttonHeight, options: sizeOptions);
			//int index = Array.IndexOf(sizeOptions, GV.BasicMath.RoundToNearestMultiple(Convert.ToInt32(GV.MonoGameImplement.framesPerSecond), 5, true).ToString());
			int sizeIndex = Array.IndexOf(sizeOptions, $"{GV.Variables.windowWidth}x{GV.Variables.windowHeight}");
			screenSizeButton.SetSelectedOption(sizeIndex != -1 ? sizeIndex : 0); // Select option corresponding to current window size

			#region AssistiveSideButtons
			multiChoiceAssistiveButtons = new Button[2];

			multiChoiceAssistiveButtons[0] = new TextButton(Vector2.Zero, "FPS", (int)buttonWidth, (int)buttonHeight) {
				Position = fpsButtonPosition - new Vector2(buttonWidth, 0)
			};

			multiChoiceAssistiveButtons[1] = new TextButton(Vector2.Zero, "Dimensions", (int)buttonWidth, (int)buttonHeight) {
				Position = screenSizeButtonPosition - new Vector2(buttonWidth, 0)
			};
			#endregion

			#endregion

			TextButton goBackButton = new TextButton(Vector2.Zero, "Back", (int)buttonWidth, (int)buttonHeight) {
				Position = new Vector2(toggleFullscreenButton.Position.X, screenSizeButtonPosition.Y + yOffset)
			};
			#endregion

			#region Build Event Handlers
			toggleFullscreenButton.Click += (self) => GV.hollowAether.ToggleFullScreen();
			goBackButton.Click           += (self) => ReturnToHomeState();

			fpsButton.Changed += (self) => {
				GV.MonoGameImplement.framesPerSecond = int.Parse(self.ActiveSelection);
			};

			screenSizeButton.Click += (self) => {
				string[] split = screenSizeButton.ActiveSelection.Split('x');
				int horizontal = int.Parse(split[0]), vertical = int.Parse(split[1]);
				GV.hollowAether.SetWindowDimensions(new Vector2(horizontal, vertical));
				BuildButtons();
			};
			#endregion

			buttons = new ButtonList(0, 3, new Button[] {toggleFullscreenButton, fpsButton, screenSizeButton, goBackButton});
		}

		public static void Update() {
			buttons.Update(false);

			GV.PeripheralIO.ControlState controlState = GV.PeripheralIO.currentControlState;

			bool altPressed = GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.LeftAlt, Keys.RightAlt); // Whether alt key is pressed
			bool enter = GetEnterInput(), goBack = GetReturnInput(); // Determines What To Do/The specific command the input corresponds to
			bool moveForward = controlState.Down || controlState.Right, moveBackward = controlState.Up || controlState.Left; // Buttons
			bool nothingPressed = !(altPressed || enter || goBack || moveForward || moveBackward); // If no input detected

			if (WaitForInputToBeRemoved) {
				if (nothingPressed) WaitForInputToBeRemoved = false;
				return; // Input has been removed || Is still in affect
			}

			if (inputTimeout > 0) {
				if (nothingPressed) inputTimeout = 0; // Remove input time out, user can input again
				else inputTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			} else if (!nothingPressed) {
				if (goBack) ReturnToHomeState(); else if (enter) buttons.ActiveButton.InvokeClick();
				else if (moveForward || moveBackward) {
					if ((controlState.Left || controlState.Right) && buttons.ActiveButton is MultichoiceButton) {
						if (moveForward) (buttons.ActiveButton as MultichoiceButton).SelectNextOption();
						else			 (buttons.ActiveButton as MultichoiceButton).SelectPreviousOption();
					} else {
						if (moveForward) buttons.MoveToNextButton();
						else		     buttons.MoveToPreviousButton();
					}
				}

				inputTimeout = 150; // Dont accept input for given value in milleseconds
			}
		}

		#region GetInputStates
		private static bool GetEnterInput() {
			return (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.Buttons.A == ButtonState.Pressed)
				|| GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Enter);
		}

		private static bool GetReturnInput() {
			return (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.Buttons.B == ButtonState.Pressed)
				|| GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Back, Keys.B);
		}
		#endregion

		private static void ReturnToHomeState() {
			GV.MonoGameImplement.gameState = GameState.Home;
			Home.Reset(); // Reset home screen window
		}

		private static Vector2 GetCenterAllignedButtonPosition(int width, int height, int windowWidth, int Y) {
			return new Vector2();
		}

		public static void FullScreenReset(bool isFullScreen) { if (GV.MonoGameImplement.gameState == GameState.Settings) Reset(); }

		private static ButtonList buttons;

		private static Button[] multiChoiceAssistiveButtons;

		public static bool WaitForInputToBeRemoved { get; set; }

		private static int inputTimeout = 0;

		private const float BUTTON_SCALE = 3.5f;
	}
}
