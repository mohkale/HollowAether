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
	/*public sealed class ItemChest : Chest {
		public ItemChest(Vector2 position) : base(position) { }

		protected override void ReleaseContents() {
			Animation["begun"] = AnimationSequence.FromRange(32, 32, 15, 0, 3); // new AnimationSequence(0, new Frame(0, 0, FRAME_WIDTH, FRAME_HEIGHT)); // Ignore Block Dimensions
			//Animation["finished"] = new AnimationSequence(0, new Frame(7, 0, FRAME_WIDTH, FRAME_HEIGHT, FRAME_WIDTH, FRAME_HEIGHT));
			//Animation["opened"] = AnimationSequence.FromRange(FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 8, FRAME_WIDTH, FRAME_HEIGHT, 5);

			Animation.SetAnimationSequence("begun"); // Set animation sequence to inital animation frame. Ignore others for now.
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		protected override void BuildSequenceLibrary() {
			base.BuildSequenceLibrary();
		}
	}*/
}
