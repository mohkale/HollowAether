using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib.GAssets.Items {
	public abstract class Gate : CollideableSprite, IInteractable {
		public Gate(Vector2 position, Vector2 newZoneTarget) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT) {
			TakesToZone = newZoneTarget;
			Initialize("cs\\npcsym");
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		public void Interact() {
			if (!Interacted && CanInteract) {
				GameWindow.GameRunning.invokeTransitionZone = true;
				GameWindow.GameRunning.transitionZoneEventArg_New = TakesToZone;

				Interacted = true; CanInteract = true; // Prevent re interaction
			}
		}

		public bool Interacted { get; private set; } = false;

		public abstract bool CanInteract { get; protected set; }

		public Vector2 TakesToZone { get; protected set; }

		public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 48;
	}
}
