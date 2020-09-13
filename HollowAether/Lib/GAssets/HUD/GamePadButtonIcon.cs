using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib.GAssets.HUD {
	public class GamePadButtonIcon : Sprite, IPushable {
		public enum Theme { Regular, Dark }

		public enum GamePadButton {
			A,            // Active=(0, 0), Idle=(1, 0)
			X,            // Active=(0, 1), Idle=(1, 1)
			Y,            // Active=(0, 2), Idle=(1, 2)
			B,            // Active=(0, 3), Idle=(1, 3)
			LTrigger,     // Active=(3, 2), Idle=(2, 2)
			RTrigger,     // Active=(5, 2), Idle=(4, 2)
			LShoulder,    // Active=(3, 3), Idle=(2, 3)
			RShoulder,    // Active=(5, 3), Idle=(4, 3)
			UnknownLeft,  // Active=(2, 1), Idle=(3, 1)
			UnknownRight, // Active=(4, 1), Idle=(5, 1)
			Home,         // Active=(7, 1), Idle=Null
			Media,        // Active=(7, 0), Idle=Null
			Blank,        // Active=(6, 3), Idle=Null
			LThumbstick,  // Active=(6, 1), Idle=Null
			RThumbstick,  // Active=(6, 2), Idle=Null
			DPadIdle,     // Active=(2, 0), Idle=Null
			DPadLeft,     // Active=(3, 0), Idle=Null
			DPadRight,    // Active=(5, 0), Idle=Null
			DPadUp,       // Active=(4, 0), Idle=Null
			DPadDown,     // Active=(6, 0), Idle=Null
		}

		public GamePadButtonIcon(
				Vector2 position, GamePadButton _button,
				Theme theme=Theme.Regular, int width=SPRITE_WIDTH,
				int height=SPRITE_HEIGHT, bool active=true
			) : base(position, width, height, true) {
			button = _button; // Store button to sprite instance, if change reinitialise. Needed for animation.
			_active = active; // Store active to assign initial animation used by button sprite.
			initialPosition = position; // Store initial position to allow pushing to it at later points
			Initialize((theme == Theme.Regular) ? "sprites\\gamepadicons" : "sprites\\gamepadiconsdark");
		}
		
		public void PushTo(Vector2 position, float over=0.8f) {
			if (PushArgs.PushValid(position, Position))
				Push(new PushArgs(position, Position, over));
		}

		public void Push(PushArgs args) { if (!BeingPushed) PushPack = args; }

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed time as well

			if (BeingPushed && !PushPack.Update(this, elapsedTime)) {
				PushPack = null; // Delete push pack
			}
		}

		protected override void BuildSequenceLibrary() {
			int X1, Y, X2; // Frame X & Y positions

			switch ((int)button) {
				case 00: X1 = 00; Y = 00; break;
				case 01: X1 = 00; Y = 01; break;
				case 02: X1 = 00; Y = 02; break;
				case 03: X1 = 00; Y = 03; break;
				case 04: X1 = 02; Y = 02; break;
				case 05: X1 = 04; Y = 02; break;
				case 06: X1 = 02; Y = 03; break;
				case 07: X1 = 04; Y = 03; break;
				case 08: X1 = 00; Y = 00; break;
				case 09: X1 = 00; Y = 00; break;
				case 10: X1 = 07; Y = 01; break;
				case 11: X1 = 07; Y = 00; break;
				case 12: X1 = 06; Y = 03; break;
				case 13: X1 = 06; Y = 01; break;
				case 14: X1 = 06; Y = 02; break;
				case 15: X1 = 02; Y = 00; break;
				case 16: X1 = 03; Y = 00; break;
				case 17: X1 = 05; Y = 00; break;
				case 18: X1 = 04; Y = 00; break;
				case 19: X1 = 06; Y = 00; break;
				default: throw new HollowAetherException($"Button '{(int)button}' out of range");
			}

			X2 = ((int)button < 10) ? X1 + 1 : X1;

			Animation["active"] = new AnimationSequence(0, new Frame(X1, Y, 32, 32));
			Animation["idle"]   = new AnimationSequence(0, new Frame(X2, Y, 32, 32));

			UpdateAnimationSequence();
		}

		private void UpdateAnimationSequence() {
			Animation.SetAnimationSequence((Active) ? "active" : "idle");
		}

		public void ChangeButton(GamePadButton _button) {
			button = _button; // Re-store button
			BuildSequenceLibrary(); // Rebuild
		}

		public void Toggle() {
			_active = !_active; // Set to opposite of what it is
			UpdateAnimationSequence(); // Set to appropriate
		}

		private void SetActive(bool newVal) {
			if (_active != newVal) Toggle();
		}

		public GamePadButton Button { get { return button; } set { ChangeButton(value); } }

		public bool Active { get { return _active; } set { SetActive(value); } }

		public PushArgs PushPack { get; private set; } = null;

		public bool BeingPushed { get { return PushPack != null; } }

		public Vector2 initialPosition;

		private GamePadButton button;

		private bool _active;

		public const int SPRITE_WIDTH=40, SPRITE_HEIGHT=40;
	}
}
