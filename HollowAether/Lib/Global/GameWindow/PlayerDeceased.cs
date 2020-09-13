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
	public static class PlayerDeceased {
		//static PlayerDeceased() { Reset(); }

		public static void Draw() {
			GameRunning.Draw(); // Set as backdrop

			GV.MonoGameImplement.InitializeSpriteBatch(false);

			GV.MonoGameImplement.SpriteBatch.Draw(
				GV.MonoGameImplement.textures[overlayTexture],
				new Rectangle(0, 0, GV.Variables.windowWidth, GV.Variables.windowHeight),
				Color.White * 0.75f
			);

			gameOverHeader.Draw(); // Draw header to screen

			if (titleText != null) titleText.Draw();

			if (buttons != null) {
				foreach (Button button in buttons)
					button.Draw(); // Draw button
			}

			GV.MonoGameImplement.SpriteBatch.End();
		}

		public static void Update() {
			gameOverHeader.Update();

			if (titleText != null) titleText.Update();

			if (buttons != null) UpdateButtons();
		}

		private static void UpdateButtons() {
			var controlState = GV.PeripheralIO.currentControlState; // Store current input control state
			bool altPressed = GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.LeftAlt, Keys.RightAlt);

			if (inputTimeout > 0) {
				if (!controlState.GamepadNotPressed() && controlState.KeyboardNotPressed())
					inputTimeout = 0; // Erase timeout if keypress removed, else continue it
				else inputTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			} else if (!altPressed) { // Skip any input checks if alt is pressed
				bool previousButton = controlState.Left || controlState.Up, nextButton = controlState.Right || controlState.Down;
				bool enterPressed = GV.PeripheralIO.currentKBState.IsKeyDown(Keys.Enter); // Chose to accept command/click-button

				if (previousButton ^ nextButton) {
					if (previousButton) buttons.MoveToPreviousButton();
					else				buttons.MoveToNextButton();

					inputTimeout = 300; // Don't accept input for 300 seconds 
				} else if (!(previousButton || nextButton) && (controlState.Jump || enterPressed)) {
					switch (buttons.ActiveButtonIndex) {
						case 0: // Retry from last save
							GV.MonoGameImplement.gameState = GameState.GameRunning;
							GameRunning.LoadSave(GV.MonoGameImplement.saveIndex);
							break;
						case 1: // Go to home screen
							GV.MonoGameImplement.saveIndex = -1; // Invalidate
							GV.MonoGameImplement.gameState = GameState.Home;
							Home.Reset(); // Reset new game state
							break;
						case 2: // Exit game
							GV.hollowAether.god.EndGame();
							break;
					}

					inputTimeout = 300; // Don't accept input for 300 seconds
				}
			}
		}

		public static void Reset() {
			titleText = null; buttons = null; // Delete (allocate for deletion) existing unecessary class members

			CreatePushableText(ref gameOverHeader, "Game Over", DEFAULT_HEADER_FONT_ID, Color.Red, GV.Variables.windowHeight / 24, 3);

			gameOverHeader.PushPack.PushCompleted += (self, args) => {
				float verticalPosition = gameOverHeader.Position.Y + gameOverHeader.Size.Y + 24; // new
				CreatePushableText(ref titleText, DeathTitle, DEFAULT_TITLE_FONT_ID, Color.Yellow, verticalPosition, 2);

				titleText.PushPack.PushCompleted += (self2, args2) => {
					String[] buttonStrings = { "Retry", "Return To Home", "Exit Game" };
					Point buttonSize = new Point(64*6, 32*3); // Set button dimensions

					Vector2 sequentialOffset = new Vector2(0, buttonSize.Y * 1.15f); // Value to offset sprite by for each button

					Vector2 position = new Vector2((GV.Variables.windowWidth - buttonSize.X) / 2, titleText.Position.Y + titleText.Size.Y);
					// Initial position is centered and directly below title text position. Add offsets to this as needed including initial

					position.Y += 0.15f * buttonSize.Y; // Give initial offset to sprite

					Button[] _buttons = new Button[buttonStrings.Length]; // Create array to hold any generated buttons

					foreach (int X in Enumerable.Range(0, 3)) {
						_buttons[X] = new TextButton(position, buttonStrings[X], buttonSize.X, buttonSize.Y);
						position += sequentialOffset; // Add offset to sprite position before creating new button
					};

					buttons = new ButtonList(0, 3, _buttons); // Store new buttons to local class
				};
			};
		}

		private static void CreatePushableText(ref PushableText address, String text, String font, Color color, float targetYPosition, float over=3) {
			Vector2 size = GV.MonoGameImplement.fonts[font].MeasureString(text); // Size of text
			Vector2 position = new Vector2((GV.Variables.windowWidth - size.X) / 2, -size.Y); // Center
			address = new PushableText(position, text, color, font); // Set initial position
			address.PushTo(new Vector2(position.X, targetYPosition), over); // Push to target position
		}

		public static void FullScreenReset(bool isFullScreen) {
			if (GV.MonoGameImplement.gameState == GameState.PlayerDeceased)
				Reset(); // Reset only when current game state is valid
		}

		public static String DeathTitle { get; } = "Human Is Dead. Mismatch.";

		private static PushableText titleText, gameOverHeader;

		private static ButtonList buttons;

		private static int inputTimeout = 0;

		public const String DEFAULT_TITLE_FONT_ID = @"playerdeadtitle";

		public const String DEFAULT_HEADER_FONT_ID = @"playerdeadheader";

		private static String overlayTexture = @"backgrounds\windows\playerdeadoverlay";
	}
}
