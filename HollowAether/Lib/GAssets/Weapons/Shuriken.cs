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

namespace HollowAether.Lib.GAssets.Weapons {
	public sealed class Shuriken : Weapon {
		private sealed class Projectile : VolatileCollideableSprite, IDamagingToEnemies {
			public Projectile(Vector2 initPosition) : base(initPosition, FRAME_WIDTH, FRAME_HEIGHT, true) {
				Initialize("weapons\\shuriken"); // Initialise from construction with default texture

				InitializeVolatility(VolatilityType.Timeout, EXISTANCE_TIME);

				float theta = GV.MonoGameImplement.Player.GetAnglePointedToByInput();

				Vector2 sinCosRatio = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

				velocity = sinCosRatio * DefaultVelocity; acceleration = sinCosRatio * DefaultAcceleration;
			}

			public int GetDamage() {
				return 1;
			}

			public override void Update(bool updateAnimation) {
				base.Update(updateAnimation);

				OffsetSpritePosition(velocity * elapsedTime);
				velocity += acceleration * elapsedTime;
			}

			protected override void BuildBoundary() {
				boundary = new SequentialBoundaryContainer(SpriteRect, GetBoundaryRect(Position));
			}

			protected override void BuildSequenceLibrary() {
				Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = GetAnimationSequence();
			}

			private const int EXISTANCE_TIME = 5000;

			private Vector2 velocity, acceleration;

			public static readonly int DefaultAcceleration = 50;

			public static readonly int DefaultVelocity = 25;
		}

		public Shuriken(Vector2 position) : base(position, FRAME_WIDTH, FRAME_HEIGHT, true) { Initialize($"weapons\\shuriken"); }

		protected override void BuildBoundary() { boundary = new SequentialBoundaryContainer(SpriteRect, GetBoundaryRect(Position)); }

		protected override void BuildSequenceLibrary() { Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = GetAnimationSequence(); }

		private static IBRectangle GetBoundaryRect(Vector2 position) {
			Vector2 boundaryPosition = position + new Vector2(BOUNDARY_X_OFFSET, BOUNDARY_Y_OFFSET); // Position with offset
			return new IBRectangle(boundaryPosition.X, boundaryPosition.Y, ACTUAL_WEAPON_WIDTH, ACTUAL_WEAPON_HEIGHT);
		}

		private static AnimationSequence GetAnimationSequence() {
			return AnimationSequence.FromRange(FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 4, FRAME_WIDTH, FRAME_HEIGHT, 1, true, 0);
		}

		public override void Attack(Player player) {
			base.Attack(player); // Update base attack features

			GV.MonoGameImplement.monogameObjects.AddNameless(new Projectile(Position));
		}
	
		protected override void UpdateAttack() {
			if (Attacking) {
				elapsedTimeBetweenThrows += elapsedMilitime;

				if (elapsedTimeBetweenThrows >= DELAY_BETWEEN_THROWS)
					FinishAttacking(); // Allow attack again
			}
		}

		protected override void FinishAttacking() {
			base.FinishAttacking();
			elapsedTimeBetweenThrows = 0;
		}

		public override void Throw(float direction) {
			base.Throw(direction); // No real point
			Attack(GV.MonoGameImplement.Player);
			StopThrowing(); // Can't throw and attack
		}

		public override void SetWeaponPositionRelativeToPlayer(Player player) {
			Vector2 positionBeforeSizeOffset = player.Position + (player.FacingRight ? Vector2.Zero : new Vector2(player.SpriteRect.Width, 0));
			Position = positionBeforeSizeOffset - (SpriteRect.Size.ToVector2() / 2); // Subtract half dimensions from either desired size
		}

		public override void AttachToPlayer(Player player) { SetWeaponPositionRelativeToPlayer(player); }

		public override bool CanBeThrown { get; protected set; } = true;

		private Point ActualWeaponSize { get { return new Point(ACTUAL_WEAPON_WIDTH, ACTUAL_WEAPON_HEIGHT); } }

		private const int BOUNDARY_X_OFFSET=8, BOUNDARY_Y_OFFSET=8;

		private const int FRAME_WIDTH = 28, FRAME_HEIGHT = 28;

		private const int ACTUAL_WEAPON_WIDTH = 12, ACTUAL_WEAPON_HEIGHT = 12;

		private const int DELAY_BETWEEN_THROWS = 750;

		private int elapsedTimeBetweenThrows;
	}
}
