#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Encryption;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
#endregion

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class PeripheralIO {
			public static bool ImplementWaitForInputToBeRemoved(ref bool wait) {
				if (!wait) return false; else {
					if (currentControlState.GamepadAndKeyboardNotPressed())
						wait = false; // Input has been removed, stop waiting 

					return true; // True when u want caller to also return
				}
			}

			public static class Controls {
				#region KeyBoardControls 
				public static readonly Keys[] jumpKB       = { Keys.Space, Keys.Up    };
				public static readonly Keys[] interactKB   = { Keys.Enter             };
				public static readonly Keys[] dashKB       = { Keys.Q                 };
				public static readonly Keys[] leftKB       = { Keys.Left,  Keys.A     };
				public static readonly Keys[] rightKB      = { Keys.Right, Keys.D     };
				public static readonly Keys[] downKB       = { Keys.Down,  Keys.S     };
				public static readonly Keys[] upKB         = { Keys.Up,    Keys.W     };
				public static readonly Keys[] throwKB      = { Keys.X                 };
				public static readonly Keys[] attackKB     = { Keys.Z                 };
				public static readonly Keys[] weaponNextKB = { Keys.E                 };
				public static readonly Keys[] pauseGameKB  = { Keys.P				  };
				#endregion

				#region GamePadControls 
				public static readonly Buttons[] jumpGP       = { Buttons.A };
				public static readonly Buttons[] interactGP   = { Buttons.Y };
				public static readonly Buttons[] throwGP      = { Buttons.RightShoulder };
				public static readonly Buttons[] dashGP       = { /*Left Trigger &*/ Buttons.X };
				public static readonly Buttons[] weaponNextGP = { Buttons.LeftShoulder };
				public static readonly Buttons[] pauseGameGP  = { Buttons.Start, Buttons.Back };

				//public static Buttons[] leftGP     = { /*Left ThumbStick*/ };
				//public static Buttons[] rightGP    = { /*Left ThumbStick*/ };
				//public static Buttons[] downGP     = { /*Left ThumbStick*/ };
				//public static Buttons[] upGP       = { /*Left ThumbStick*/ };
				#endregion
			}

			public struct ControlState {
				public struct ControlsPressed {
					public override string ToString() {
						return $"GS(J:{Jump}, I:{Interact}, D:{Dash}, L:{Left}, R:{Right}, U:{Up}, D:{Down}), A:{Attack}, T:{Throw}";
					}

					public bool NotPressed() {
						return !(Jump || Interact || Dash || Attack || Throw || Left || Right || Up || Down || Pause);
					}

					public bool Jump, Interact, Dash, Left, Right, Up, Down, Throw, Attack, WeaponNext, Pause;

					public static ControlsPressed Empty {
						get { return new ControlsPressed(); }
					}
				}

				public void Update() {
					KeyboardState KB = currentKBState;
					GamePadState  GP = currentGPState;

					Keyboard = new ControlsPressed() {
						Jump       = CheckMultipleKeys(true, KB,     Controls.jumpKB),
						Interact   = CheckMultipleKeys(true, KB, Controls.interactKB),
						Dash       = CheckMultipleKeys(true, KB,     Controls.dashKB),
						Left       = CheckMultipleKeys(true, KB,     Controls.leftKB),
						Right      = CheckMultipleKeys(true, KB,    Controls.rightKB),
						Up         = CheckMultipleKeys(true, KB,       Controls.upKB),
						Down       = CheckMultipleKeys(true, KB,     Controls.downKB),
						Attack     = CheckMultipleKeys(true, KB,   Controls.attackKB),
						Throw      = CheckMultipleKeys(true, KB,    Controls.throwKB),
						WeaponNext = CheckMultipleKeys(true, KB, Controls.weaponNextKB),
						Pause      = CheckMultipleKeys(true, KB, Controls.pauseGameKB)
					};

					Gamepad = new ControlsPressed() {
						Jump       = CheckMultipleButtons(true, GP, Controls.jumpGP),
						Interact   = CheckMultipleButtons(true, GP, Controls.interactGP),
						Dash       = TriggerPressed(true, GP) || CheckMultipleButtons(true, GP, Controls.dashGP),
						Left       = GamePadThumstickPointingLeft(true, GP),
						Right      = GamePadThumstickPointingRight(true, GP),
						Up         = GamePadThumstickPointingUp(true, GP),
						Down       = GamePadThumstickPointingDown(true, GP),
						Attack     = TriggerPressed(false, GP),
						Throw      = CheckMultipleButtons(true, GP, Controls.throwGP),
						WeaponNext = CheckMultipleButtons(true, GP, Controls.weaponNextGP),
						Pause      = CheckMultipleButtons(true, GP, Controls.pauseGameGP)
					};
				}

				public bool KeyboardNotPressed() { return Keyboard.NotPressed(); }

				public bool GamepadNotPressed() { return Gamepad.NotPressed(); }

				public bool GamepadAndKeyboardNotPressed() { return GamepadNotPressed() && KeyboardNotPressed(); }

				public bool Left { get { return Keyboard.Left || Gamepad.Left; } }

				public bool Right { get { return Keyboard.Right || Gamepad.Right; } }

				public bool Up { get { return Keyboard.Up || Gamepad.Up; } }

				public bool Down { get { return Keyboard.Down || Gamepad.Down; } }

				public bool Jump { get { return Keyboard.Jump || Gamepad.Jump; } }

				public bool Interact { get { return Keyboard.Interact || Gamepad.Interact; } }

				public bool Dash { get { return Keyboard.Dash || Gamepad.Dash; } }

				public bool Attack { get { return Keyboard.Attack || Gamepad.Attack; } }

				public bool Throw { get { return Keyboard.Throw || Gamepad.Throw; } }

				public bool WeaponNext { get { return Keyboard.WeaponNext || Gamepad.WeaponNext; } }

				public bool Pause { get { return Keyboard.Pause || Gamepad.Pause; } }

				public ControlsPressed Keyboard, Gamepad;
			}

			public static bool TriggerPressed(bool leftTrigger, GamePadState? state = null) {
				GamePadTriggers triggers = (state.HasValue ? state.Value : currentGPState).Triggers;
				return (leftTrigger ? triggers.Left : triggers.Right) > 0.25; // Button Tolerance = 0.5
			}

			public static Vector2 GetThumbStickDisplacement(bool leftTS, GamePadState? state = null) {
				GamePadThumbSticks GPTS = (state.HasValue ? state.Value : currentGPState).ThumbSticks;
				return (leftTS) ? GPTS.Left : GPTS.Right; // Get left or right thumbstick indicator
			}

			public static bool[] ThumbstickPointing(bool leftTS, GamePadState? state = null) {
				Vector2 s = GetThumbStickDisplacement(leftTS, state); // displacement

				return new bool[] { s.Y > 0.25, s.X > 0.25, s.Y < -0.25, s.X < -0.25 };
				// Top, Right, Bottom, Left -> Clockwise rotation of TS with tolerance 0.25
			}

			public static bool GamePadThumstickPointingLeft(bool leftTS, GamePadState? state = null) {
				return GetThumbStickDisplacement(leftTS, state).X < -0.25;
			}

			public static bool GamePadThumstickPointingRight(bool leftTS, GamePadState? state = null) {
				return GetThumbStickDisplacement(leftTS, state).X > 0.25;
			}

			public static bool GamePadThumstickPointingUp(bool leftTS, GamePadState? state = null) {
				return GetThumbStickDisplacement(leftTS, state).Y > 0.25;
			}

			public static bool GamePadThumstickPointingDown(bool leftTS, GamePadState? state = null) {
				return GetThumbStickDisplacement(leftTS, state).Y < -0.25;
			}
			
			public static bool CheckMultipleKeys(bool ORCheck = true, KeyboardState? kb = null, params Keys[] keys) {
				KeyboardState keyboard = (kb.HasValue) ? kb.Value : currentKBState; // default = current

				foreach (Keys key in keys) {
					if (keyboard.IsKeyDown(key)) { if (ORCheck)  return true;  } 
					else					     { if (!ORCheck) return false; }
				}

				return !ORCheck;
			}

			public static bool CheckMultipleButtons(bool ORCheck, GamePadState? gp = null, params Buttons[] buttons) {
				GamePadState gamepad = (gp.HasValue) ? gp.Value : currentGPState; // default = current

				foreach (Buttons button in buttons) {
					if (gamepad.IsButtonDown(button)) { if (ORCheck)  return true; } 
					else							  { if (!ORCheck) return false; }
				}

				return !ORCheck;
			}

			public static bool gamePadConnected { get { return currentGPState.IsConnected; } }

			public static KeyboardState previousKBState, currentKBState;
			public static GamePadState previousGPState, currentGPState;

			public static ControlState previousControlState = new ControlState() {
				Keyboard = ControlState.ControlsPressed.Empty,
				Gamepad  = ControlState.ControlsPressed.Empty,
			};

			public static ControlState currentControlState = new ControlState() {
				Keyboard = ControlState.ControlsPressed.Empty,
				Gamepad = ControlState.ControlsPressed.Empty,
			};
		}

	}
}
