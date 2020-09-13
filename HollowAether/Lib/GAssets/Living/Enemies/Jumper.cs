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

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	public partial class Jumper : Enemy, IPredefinedTexture {
		public Jumper() : this(Vector2.Zero, 1) { }

		public Jumper(Vector2 position, int level) : base(position, 2 * FRAME_WIDTH, 2 * FRAME_HEIGHT, level, true) {
			Initialize(@"enemies\jumper");
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey);

			maxJumpHeight = GetMaxJumpHeight(Level); // Set max jump height depending on enemy level.
			jumpVelocity  = new Vector2(0, -GV.Physics.GetJumpVelocity(GV.MonoGameImplement.gravity, maxJumpHeight));
		}

		protected override void BuildSequenceLibrary() {
			Animation["Idle"]     = GV.MonoGameImplement.importedAnimations[@"jumper\idle"];
			Animation["Laughing"] = GV.MonoGameImplement.importedAnimations[@"jumper\laughing"];
			Animation["Crouched"] = GV.MonoGameImplement.importedAnimations[@"jumper\crouched"];
			Animation["Jumping"]  = GV.MonoGameImplement.importedAnimations[@"jumper\jumping"];

			Animation.SetAnimationSequence("Laughing");
		}

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(SpriteRect));
		}

		protected override void DoEnemyStuff() {
			if (!Falling()) { // When not already jumping, check whether in range to jump upwards
				Vector2 playerCenter = GV.MonoGameImplement.Player.SpriteRect.Center.ToVector2();

				Vector2 distanceDifferential = (SpriteRect.Center.ToVector2() - playerCenter);

				if (distanceDifferential.Y < 5 * maxJumpHeight) { // If jumping can reach player
					float theta = (float)Math.Atan2(distanceDifferential.X, distanceDifferential.Y);
					int thetaDeg = (int)Math.Round(GV.BasicMath.RadiansToDegrees(theta)); // to degrees

					if (Math.Abs(thetaDeg) < 25) Jump(); // If angle between enemy and player is small enough
				}
			}

			ImplementGravity(); // Push back down if not atop block. If so, then stop jumping altogether

			if (velocity != Vector2.Zero) OffsetSpritePosition(velocity * elapsedTime); // Add displacement
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);
			if (!Alive) ImplementGravity();
		}

		public void Jump() {
			velocity = jumpVelocity; // Set velocity to enemy jump velocity

			AnimationChain chain = new AnimationChain(Animation["Crouched"]); // new chain
			chain.ChainFinished += () => { Animation.SetAnimationSequence("Jumping"); };
			Animation.AttatchAnimationChain(chain); // Attach chain to existing animation
			Animation.Update(); // Move to next frame in newly attatched animation chain
		}

		protected override void StopFalling() {
			velocity = Vector2.Zero; // This enemy doesn't have any horizontal velocity anyways
			Animation.SetAnimationSequence("Laughing"); // Reset to default animation when grounded
		}

		private int GetMaxJumpHeight(int level) {
			return GV.BasicMath.Clamp<int>(6 * GV.Variables.random.Next(Level - 2, Level + 3), 30, 250);
		}

		private int maxJumpHeight;

		private Vector2 jumpVelocity;

		protected override bool CanGenerateHealth { get; set; } = false;

		protected override bool CausesContactDamage { get; set; } = true;

		public const int FRAME_WIDTH = 16, FRAME_HEIGHT = 16;

		public const int SPRITE_WIDTH = FRAME_WIDTH * 2, SPRITE_HEIGHT = FRAME_HEIGHT * 2;
	}
}
