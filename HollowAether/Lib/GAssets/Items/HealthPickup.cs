using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.Items {
	public class HealthPickup : VolatileCollideableSprite, ITimeoutSupportedAutoInteractable {
		public HealthPickup(Vector2 position, bool singleHeart=false) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT, true) {
			heartSingle = singleHeart; // Store to determine frame deets
			InitializeVolatility(VolatilityType.Timeout, 5000); 
			Initialize("cs\\npcsym");
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);

			if (InteractionTimeout > 0)
				InteractionTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			else if (InteractCondition()) Interact();
		}

		private bool heartSingle;

		public bool InteractCondition() {
			return CanInteract && !Interacted && GV.MonoGameImplement.Player.Intersects(boundary);
		}

		public void Interact() {
			VolatilityManager.Delete(this); // Allocate for deletion
			GameWindow.GameRunning.InvokeGotHealthPickup(this);
		}

		protected override void BuildSequenceLibrary() {
			Frame f1 = new Frame(2 + (heartSingle ? 0 : 2), 5, 32, 32, runCount: 5);
			Frame f2 = new Frame(3 + (heartSingle ? 0 : 2), 5, 32, 32, runCount: 3);
	
			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = new AnimationSequence(0, f1, f2);
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(this.SpriteRect));
		}

		public bool Interacted { get; } = false;

		public bool CanInteract { get; } = true;

		public int InteractionTimeout { get; set; }

		public int Value { get { return heartSingle ? 1 : 3; } }

		public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
	}
}
