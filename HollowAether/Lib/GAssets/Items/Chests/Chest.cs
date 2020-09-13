using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public abstract class Chest : BodySprite, IInteractable {
		public Chest(Vector2 position) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT) {
			Initialize("items\\chest");
			Layer = 0.4f;
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);

			ImplementGravity(); // Gravity not automatically implemented
			CanInteract = !(Falling(0, 1)); // Don't allow interaction when falling
		}

		public void Interact() {
			if (CanInteract && !Interacted) {
				Animation.ChainFinished += ChainFinishedHandler; // Handler attatched
				Animation.AttatchAnimationChain(new AnimationChain(Animation["opened"]));
				Interacted = true; // Don't interact again
			}
		}

		protected abstract void ReleaseContents();

		private void ChainFinishedHandler(AnimationChain chain) {
			ReleaseContents(); /* Begin contents release */
			Animation.SetAnimationSequence("finished");
			Animation.ChainFinished -= ChainFinishedHandler;
		}

		protected override void BuildSequenceLibrary() {
			Animation["begun"]    = new AnimationSequence(0, new Frame(0, 0, FRAME_WIDTH, FRAME_HEIGHT)); // Ignore Block Dimensions
			Animation["finished"] = new AnimationSequence(0, new Frame(7, 0, FRAME_WIDTH, FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT));
			Animation["opened"]   = AnimationSequence.FromRange(FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 8, FRAME_WIDTH, FRAME_HEIGHT, 5);

			Animation.SetAnimationSequence("begun"); // Set animation sequence to inital animation frame. Ignore others for now.
		}

		public bool Interacted { get; private set; } = false;

		public bool CanInteract { get; private set; } = true;

		public const int SPRITE_WIDTH = 42, SPRITE_HEIGHT = 24;
		public const int FRAME_WIDTH  = 126, FRAME_HEIGHT = 72;
	}
}
