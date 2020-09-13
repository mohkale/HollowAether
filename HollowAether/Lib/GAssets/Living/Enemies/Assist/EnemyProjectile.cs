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
	public abstract class EnemyProjectile : VolatileCollideableSprite, IDamagingToPlayer {
		public EnemyProjectile(Vector2 position, Vector2 target, float velocity, int width, int height, bool animationRunning) 
			: base(position, width, height, animationRunning) {
			Vector2 widthHeight =  target - position; // Get width height dimensions for object travel

			float theta = (float)Math.Atan2(widthHeight.Y, widthHeight.X); // Get angle towards target

			Velocity = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta)) * new Vector2(velocity);

			InitializeVolatility(VolatilityType.Other, new ImplementVolatility(ReadyToDeleteCheck));
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation);

			OffsetSpritePosition(Velocity * elapsedTime);
		}

		public abstract int GetDamage();

		protected override void BuildBoundary() {
			boundary = new SequentialBoundaryContainer(SpriteRect, new IBRectangle(Position.X, Position.Y, Width, Height));
		}

		private static bool ReadyToDeleteCheck(IMonoGameObject self) {
			return false;
		}

		protected Vector2 Velocity;
	}

	public class Fireball : EnemyProjectile {
		public Fireball(Vector2 position, Vector2 target, float velocity) 
			: base(position, target, velocity, 15, 15, true) {
			Initialize(@"fx\fireball");
		}

		protected override void BuildSequenceLibrary() {
			Animation[GV.MonoGameImplement.defaultAnimationSequenceKey] = AnimationSequence.FromRange(
				FRAME_WIDTH, FRAME_HEIGHT, 0, 0, 4, FRAME_WIDTH, FRAME_HEIGHT, 2, true, 0
			);
		}

		public override int GetDamage() {
			return 2;
		}

		public const int FRAME_WIDTH = 35, FRAME_HEIGHT = 35;
	}
}
