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
	/// <summary>Definition for coin like object. Can be used for dash points as well</summary>
	public abstract class TreasureItem : VolatileBodySprite, ITimeoutSupportedAutoInteractable {
		/// <param name="position">Initial position for sprite</param>
		/// <param name="randMove">Whether to let sprite have initial random velocity</param>
		/// <param name="width">Width of sprite</param> <param name="height">Height of sprite</param>
		/// <param name="texture">Texture of sprite</param> <param name="timeout">Time for sprite to exist</param>
		public TreasureItem(Vector2 position, int width, int height, int timeout, bool randMove=true)
			: base(position, width, height) {
			InitializeVolatility(VolatilityType.Timeout, timeout); // Exists for 5 secs
			if (randMove) CalculateMiscellaneousPush(); // Give coin horizontal velocity
			Layer = 0.65f;
		}

		private void CalculateMiscellaneousPush() {
			int randVelocity = GV.Variables.random.Next(MinRandMoveVelocity, MaxRandMoveVelocity); // Random velocity to move
			float angle = GV.BasicMath.DegreesToRadians(GV.Variables.random.Next(MinRandMoveAngle, MaxRandMoveAngle));
			inertialDirection = (GV.Variables.random.Next(0, 2) == 0) ? +1 : -1; // Direction coin moving is randomised
			
			velocity.X = -inertialDirection * randVelocity * (float)Math.Cos(angle); // Set initial horizontal velocity 
			velocity.Y = randVelocity       *      -1      * (float)Math.Sin(angle); // Set initial vertical   velocity
			// Y Velocity is always negative so coin always travels upwards at beginning/start of coins movement
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed time as well, important, must come first.

			ImplementGravity(); // Implement change in displacement & velocity due to gravitational acceleration
			OffsetSpritePosition(XDisplacement); // Implement horizontal displacement due to initial velocity
			UpdateHorizontalVelocity(); // Implement inertial accelearation acting against initial velocity

			if ((Falling(1, 0) ^ Falling(-1, 0)) && HasIntersectedTypes(GV.MonoGameImplement.BlockTypes))
				LinearCollisionHandler(); // If falling on either side but still intersected then handle

			if (InteractionTimeout > 0) 
				InteractionTimeout -= GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			else if(InteractCondition()) Interact(); // If player collides with coin then interact if viable
		}

		public virtual bool InteractCondition() {
			return CanInteract && !Interacted && GV.MonoGameImplement.Player.Intersects(boundary);
		}

		public virtual void Interact() {
			VolatilityManager.Delete(this); // Delete when interacted
			Interacted = true; // Has been interacted with
		}

		private void CheckHorizontalCollision() {
			if ((Falling(1, 0) ^ Falling(-1, 0)) && HasIntersectedTypes(GV.MonoGameImplement.BlockTypes))
				LinearCollisionHandler(); // If falling on either side but still intersected then handle
		}

		protected virtual void LinearCollisionHandler() {
			Direction direction = (inertialDirection < 0) ? Direction.Left : Direction.Right; // Get direction moving
			var collided = CompoundIntersects(GV.MonoGameImplement.BlockTypes).Cast<ICollideable>().ToArray();
			float? newXPos = LoopGetPositionFromDirection(collided, direction); // Get new viable player position
			if (newXPos.HasValue) SetPosition(X: newXPos.Value); // Set coin position to edge of intersected item
			
			velocity.X = 0; // For now stop coin moving. Later on can add affect to bounce coin upon collision
		}

		public void UpdateHorizontalVelocity() {
			if ((inertialDirection == 1 && velocity.X < 0) || (inertialDirection == -1 && velocity.X > 0)) {
				float horizontalOffset = horizontalInertialAcceleration * inertialDirection * elapsedTime;

				if (GV.Misc.SignChangedFromAddition(velocity.X, horizontalOffset))
					velocity.X = 0; // If sign has changed between additions
				else velocity.X += horizontalOffset; // Add change due to acceleration
			}
		}

		private int inertialDirection;

		private float horizontalInertialAcceleration = 5f; // For Misc Push

		public override Vector2 TerminalVelocity { get; protected set; } = new Vector2(200);
		
		public bool Interacted { get; protected set; } = false;

		public bool CanInteract { get; protected set; } = true;

		public int InteractionTimeout { get; set; } = 0;
 
		protected virtual int MinRandMoveAngle { get; set; } = 30;

		protected virtual int MaxRandMoveAngle { get; set; } = 85;

		protected virtual int MinRandMoveVelocity { get; set; } = 50;

		protected virtual int MaxRandMoveVelocity { get; set; } = 85;
	}
}
