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
using WinF = System.Windows.Forms;

using GV = HollowAether.Lib.GlobalVars;
using GPBIWT = HollowAether.Lib.GAssets.HUD.GamePadButtonIconWithText;
using GPB = HollowAether.Lib.GAssets.HUD.GamePadButtonIcon.GamePadButton;

namespace HollowAether.Lib.GameWindow {
	static class SaveLoad {
		static SaveLoad() { Initialize(); }

		static void Initialize() {
			int windowWidth = GV.Variables.windowWidth, windowHeight = GV.Variables.windowHeight;

			// buttons = new SaveButton[InputOutput.SaveManager.SAVE_SLOTS]; // Number of save slots
			// int buttonWidth = 1400, buttonHeight = 200, buttonYOffset = (int)(buttonHeight * 1.15f);
			int buttonWidth = windowWidth - 100, buttonHeight = windowHeight * 2/9, buttonYOffset = (int)(buttonHeight * 1.15f);

			#region Build Save Buttons
			Button[] _buttons = new Button[InputOutput.SaveManager.SAVE_SLOTS+1]; // Local button store
			Vector2 initialPosition = new Vector2((windowWidth - buttonWidth) / 2, windowHeight * 2 / 45);

			foreach (int X in Enumerable.Range(0, InputOutput.SaveManager.SAVE_SLOTS)) {
				_buttons[X] = new SaveButton(initialPosition, X, buttonWidth, buttonHeight) { Layer = 0.4f };

				_buttons[X].Click += SaveLoad_Click;

				initialPosition.Y += buttonYOffset; // Push to leave space for new button sprite
			}

			int backButtonWidth = (int)(Button.SPRITE_WIDTH*3.5f), backButtonHeight = (int)(Button.SPRITE_HEIGHT*3.5f);
			Vector2 backButtonPos = new Vector2((windowWidth - backButtonWidth) / 2, initialPosition.Y);

			#region Build Return To Homestate Position
			_buttons[_buttons.Length - 1] = new TextButton(backButtonPos, "Back", backButtonWidth, backButtonHeight) { Layer = 0.4f };
			_buttons.Last().Click += (self) => { ReturnToHomeState(); };
			#endregion

			buttons = new ButtonList(0, InputOutput.SaveManager.SAVE_SLOTS + 1, _buttons); // Create buttons list
			#endregion

			Vector2 generalVerticalOffset = new Vector2(0, GPBIWT.SPRITE_HEIGHT + 8f); // Extended offset = 8 pixels

			Vector2 loadButtonPosition   = new Vector2(windowWidth - (2 * GPBIWT.SPRITE_WIDTH),   windowHeight - generalVerticalOffset.Y);
			Vector2 backButtonPosition   = loadButtonPosition - new Vector2(GPBIWT.SPRITE_WIDTH, generalVerticalOffset.Y);
			Vector2 deleteButtonPosition = backButtonPosition - new Vector2(-GPBIWT.SPRITE_WIDTH, generalVerticalOffset.Y);

			displayedButtons = new GPBIWT[] {
				new GPBIWT(deleteButtonPosition, GPB.Y, "Delete", GPBIWT.TextPosition.BeforeButton) { Layer = 0.9f },
				new GPBIWT(backButtonPosition,   GPB.B, "Back",   GPBIWT.TextPosition.BeforeButton) { Layer = 0.9f },
				new GPBIWT(loadButtonPosition,   GPB.A, "Load",   GPBIWT.TextPosition.BeforeButton) { Layer = 0.9f }
			};
		}

		private static void SaveLoad_Click(Button self) {
			int index = (self as SaveButton).SaveIndex; // To Index

			if (!InputOutput.SaveManager.SaveExists(index))
				InputOutput.SaveManager.CreateBlankSave(index);

			GV.MonoGameImplement.gameState = GameState.GameRunning;

			GV.MonoGameImplement.saveIndex = index; // Set index
			GameRunning.LoadSave(index); // Load desired save file
		}

		public static void Draw() {
			GV.MonoGameImplement.InitializeSpriteBatch(false);

			buttons.Draw(); // Draw save and back buttons

			foreach (GPBIWT button in displayedButtons)
				button.Draw();

			GV.MonoGameImplement.SpriteBatch.End();
		}

		public static void Reset() { Initialize(); }
		
		public static void FullScreenReset(bool isFullScreen) {
			if (GV.MonoGameImplement.gameState == GameState.SaveLoad)
				Reset(); // If Current Game State, Then Reset
		}

		public static void Update() {
			foreach (Button button in buttons)		    button.Update(false);
			foreach (GPBIWT button in displayedButtons) button.Update(false);
			
			GV.PeripheralIO.ControlState controlState = GV.PeripheralIO.currentControlState;

			bool altPressed = GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.LeftAlt, Keys.RightAlt); // Whether alt key is pressed
			bool enter = GetEnterInput(), goBack = GetReturnInput(), deleteSave = GetDeleteSaveInput(); // Determines What To Do Below
			bool moveForward = controlState.Down || controlState.Right, moveBackward = controlState.Up || controlState.Left; // Buttons
			bool nothingPressed = !(altPressed || enter || goBack || deleteSave || moveForward || moveBackward); // If no input detected

			if (WaitForInputToBeRemoved) {
				if (nothingPressed)  WaitForInputToBeRemoved = false;
				return; // Input has been removed || Is still in affect
			}

			if (inputTimeout > 0) {
				if (nothingPressed) inputTimeout = 0; // Remove input time out, user can input again
				else inputTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			} else if (!nothingPressed) {
				if      (goBack)	   ReturnToHomeState();
				else if	(moveForward)  buttons.MoveToNextButton();
				else if (moveBackward) buttons.MoveToPreviousButton();
				else if (enter)		   buttons.ActiveButton.InvokeClick();
				else if (deleteSave)   DeleteSave(buttons.ActiveButtonIndex);

				inputTimeout = 150; // Dont accept input for given value in milleseconds
			}
		}

		#region GetButtonStates
		private static bool GetEnterInput() {
			return (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.Buttons.A == ButtonState.Pressed)
				|| GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Enter);
		}

		private static bool GetReturnInput() {
			return (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.Buttons.B == ButtonState.Pressed)
				|| GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Back, Keys.B);
		}

		private static bool GetDeleteSaveInput() {
			return (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.Buttons.Y == ButtonState.Pressed)
				|| GV.PeripheralIO.CheckMultipleKeys(true, null, Keys.Y);
		}
		#endregion

		private static void DeleteSave(int index) {
			if (InputOutput.SaveManager.SaveExists(index)) {
				WinF.DialogResult result = WinF.MessageBox.Show(
					"Are You Sure You Want To Delete Save {index}",
					"Confirm", WinF.MessageBoxButtons.YesNoCancel
				);

				if (result == WinF.DialogResult.Yes) {
					InputOutput.SaveManager.DeleteSave(index);
					(buttons[index] as SaveButton).DeleteDetails();
				}
			}
		}

		private static void ReturnToHomeState() {
			GV.MonoGameImplement.gameState = GameState.Home;
			Home.Reset(); // Reset home screen window
		}

		private static ButtonList buttons;

		private static GPBIWT[] displayedButtons;

		public static bool WaitForInputToBeRemoved { get; set; }

		private static int inputTimeout = 0;
		public static void FullScreenReset() { Reset(); }
	}
}
