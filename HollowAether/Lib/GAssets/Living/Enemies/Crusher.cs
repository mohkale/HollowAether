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
	public partial class Crusher : Enemy {
		public Crusher() : this(Vector2.Zero, 1) { }

		public Crusher(Vector2 position, int level) : base(position, SPRITE_WIDTH, SPRITE_HEIGHT, level, true) {
			Initialize(@"enemies\crusher");
			initialPosition     = position;
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(SpriteRect));
		}

		protected override void BuildSequenceLibrary() {
			Animation["Idle"] = GV.MonoGameImplement.importedAnimations[@"crusher\idle"]; // AnimationSequence.FromRange(FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 3, FRAME_WIDTH, FRAME_HEIGHT, 25, true, 0);
		}

		protected override void DoEnemyStuff() {
			if (!falling) {
				Rectangle playerRect = GV.MonoGameImplement.Player.Boundary.Container; // Store player rectangle

				bool validLeft  = playerRect.Left > SpriteRect.Left  || playerRect.Right > SpriteRect.Left;
				bool validRight = playerRect.Left < SpriteRect.Right || playerRect.Right < SpriteRect.Right;

				if (playerRect.Bottom > SpriteRect.Bottom && validLeft && validRight)
					falling = true; // Initiate falling motion until reaching block 
			} else {
				if (returningToPosition) {
					float displacement = - verticalReturnVelocity * elapsedTime;

					if (GV.MonoGameImplement.Player.Intersects(this)) {
						String[] labels = GV.MonoGameImplement.Player.TrueBoundary.GetIntersectingBoundaryLabels(boundary);
						if (labels.Contains("Bottom")) GV.MonoGameImplement.Player.OffsetSpritePosition(Y: displacement);
						// If player is standing atop crusher, push player up by same amount. Cheaty fix to annoying bug
					}

					OffsetSpritePosition(Y: displacement); // Push by displacement

					if (Position.Y <= initialPosition.Y) {
						// Has returned to initial expected position
						SetPosition(initialPosition); // To initial
						returningToPosition = false; //  Reset vars
						falling = false; // ------------ Reset vars
					}
				} else if (falling) ImplementGravity();
			}

			if (falling && !returningToPosition && GV.MonoGameImplement.Player.Intersects(this)) {
				String[] labels = GV.MonoGameImplement.Player.TrueBoundary.GetIntersectingBoundaryLabels(boundary);

				float bottomHeightDifferential = SpriteRect.Bottom - GV.MonoGameImplement.Player.Position.Y;

				if (labels.Contains("Top") && !GV.MonoGameImplement.Player.Falling(0, bottomHeightDifferential)) {
					GV.MonoGameImplement.Player.OffsetSpritePosition(Y: bottomHeightDifferential); // Push to block
					GV.MonoGameImplement.Player.Attack(GV.MonoGameImplement.GameHUD.Health); // Kill player
				}
				// When crusher is directly atop players head, he's been crushed\(valid collision has happened) & should be killed
			}
		}

		protected override uint GetHealth() { return 250; }

		protected override void StopFalling() {
			base.StopFalling(); // Base stop methods

			returningToPosition = true; // Begin return
		}

		private bool returningToPosition = false;

		private bool falling;

		private Vector2 initialPosition;

		private float verticalReturnVelocity = 50;

		protected override bool CanGenerateHealth { get; set; }

		protected override bool CausesContactDamage { get; set; } = false;

		public override float GravitationalAcceleration { get; protected set; } = 2 * GV.MonoGameImplement.gravity;

		public const int FRAME_WIDTH = 24, FRAME_HEIGHT = 32;

		public const int SPRITE_WIDTH = (int)(1.75 * FRAME_WIDTH), SPRITE_HEIGHT = (int)(1.75 * FRAME_HEIGHT);
	}
}
