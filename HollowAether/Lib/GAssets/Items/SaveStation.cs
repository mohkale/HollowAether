using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GV = HollowAether.Lib.GlobalVars;

using HollowAether.Lib.GAssets;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GAssets.Items {
	public partial class SaveStation : CollideableSprite, IInteractable {
		public SaveStation() : base() { }

		public SaveStation(Vector2 position, int width=FRAME_WIDTH, int height=FRAME_HEIGHT) : base(position, width, height, true) {
			// Initialize(@"cs\npcsym"); // Initialise sprite from construction
		}

		protected override void BuildSequenceLibrary() {
			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(
				FRAME_WIDTH, FRAME_HEIGHT, 6, 1, 8, FRAME_WIDTH, FRAME_HEIGHT, 1, true, 0
			);
		}

		public void Interact() {
			if (CanInteract) {
				HUD.ContextMenu.CreateOKCancel("You've reached a save point\nWould you like to save now?",
					(ok) => { if (ok) InputOutput.SaveManager.SaveCurrentGameState(GV.MonoGameImplement.saveIndex); }
				);

				CanInteract = false; interaction_timeout = INTERACTION_TIMEOUT; // Set interaction off
			}
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates animation. Doesn't update whilst hood is running as designated

			if (!CanInteract && interaction_timeout > 0) interaction_timeout -= elapsedMilitime; else CanInteract = true;
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(SpriteRect));
		}

		public bool CanInteract { get; set; } = true;

		public bool Interacted { get; } = false; // Always can interact

		private static readonly int INTERACTION_TIMEOUT = 1500;

		private int interaction_timeout = 0;

		public const int FRAME_WIDTH=32, FRAME_HEIGHT=32;
	}
}
