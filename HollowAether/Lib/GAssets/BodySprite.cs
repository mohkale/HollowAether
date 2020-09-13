#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.MapZone;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GAssets {
	/// <summary>Collideable Sprite which can experience the effects of gravity</summary>
	public abstract partial class BodySprite : CollideableSprite, IBody {
		public BodySprite() : base() { }

		public BodySprite(Vector2 position, int width, int height, bool aRunning=true) : base(position, width, height, aRunning) { }
	
		/// <summary>Method to implement downwards gravitational pull</summary>
		/// <remarks>Note this only allows the player to fall, running through or into blocks is for something else</remarks>
		public virtual void ImplementGravity() {
			if (Falling(1, 0) || Falling(-1, 0)) { // If u haven't intersected with any known collideable blocks
				// Velocity = Acceleration * Time; Displacement = Velocity * Time^2 = Acceleration * Time^2
				if (velocity.Y == 0) velocity.Y = defaultFallingVelocity; // Begun falling, start with default

				velocity.Y += GravitationalAcceleration * elapsedTime; // Increment velocity by change in acceleration

				OffsetSpritePosition(Y: YDisplacement); // Push sprite by desired vertical offset

				if (FixDownwardCollisionAfterFalling()) {
					StopFalling(); // Player is no longer falling so reset falling
					VerticalDownwardBoundaryFix(); // Fix downward collision once occured
				}
			}
		}

		protected virtual bool FixDownwardCollisionAfterFalling() {
			return !(Falling(1, 0) ^ Falling(-1, 0)) && HasIntersectedTypes(GV.MonoGameImplement.BlockTypes);
		}

		/// <summary>Method to check wether player is currently above a block or not</summary>
		public virtual bool Falling() { return !(HasIntersectedTypes(GV.MonoGameImplement.BlockTypes)); }

		/// <summary>Method to check wether player is currently above a block or not</summary>
		/// <param name="offset">Value by which to offset boundary for check</param>
		public bool Falling(Vector2 offset) { return CheckOffsetMethodCaller(offset, () => Falling()); }

		public bool Falling(float X, float Y) { return Falling(new Vector2(X, Y)); }

		/// <summary>Method to implement when player has stopped falling</summary>
		/// <remarks>Interpret any downward force damage here then call base.StopFalling() to reset vars</remarks>
		protected virtual void StopFalling() { velocity.Y = 0; }

		private float? SharedCollisionHandler(ICollideable[] collided, Direction direction, Func<IBoundaryContainer, bool> checker) {
			if (collided.Length > 1) collided = (from X in collided where checker(X.Boundary) select X).ToArray(); // Shorten block span
			return LoopGetPositionFromDirection(collided, direction); // Get new position after collision with collided items & return
		}

		/// <summary>Fixes sprite position when it has intersected a block</summary>
		protected virtual void VerticalDownwardBoundaryFix() {
			ICollideable[] collided = CompoundIntersects(GV.MonoGameImplement.BlockTypes).Cast<ICollideable>().ToArray();
			float? position = SharedCollisionHandler(collided, Direction.Top, GenericVerticalBoundaryFixValidityCheck);

			if (position.HasValue) {
				SetPosition(Y: position.Value); // If new position has value, set sprite to new sprite position
			}
		}

		protected virtual void HorizontalBoundaryFix(bool movingLeft) {
			Direction directionToCorrect = (movingLeft) ? Direction.Right : Direction.Left; // Determine which direction to correct in
			ICollideable[] collided = CompoundIntersects(GV.MonoGameImplement.BlockTypes).Cast<ICollideable>().ToArray(); // Collideable

			float? position = SharedCollisionHandler(collided, directionToCorrect, GenericHorizontalBoundaryFixValidityCheck);

			if (position.HasValue) SetPosition(X: position.Value); // If new position has value, set sprite to new sprite position
		}

		/// <summary>Method to check which boundaries are valid for a downward boundary check</summary>
		/// <param name="container">Boundary to check compatibility with</param>
		/// <returns>Whether boundary is valid for check</returns>
		protected virtual bool GenericVerticalBoundaryFixValidityCheck(IBoundaryContainer container) {
			Rectangle A = container.SpriteRect, B = SpriteRect; // B = self
			return B.X >= A.X - B.Width && B.X + B.Width <= A.X + A.Width; 
			// True when intersected blocks X values holds self X values
		}

		protected bool GenericHorizontalBoundaryFixValidityCheck(IBoundaryContainer container) {
			Rectangle target = container.SpriteRect, self = container.SpriteRect; // Store vars
			return self.Y > target.Y - self.Height && self.Y < target.Y + target.Height;
		}

		/// <summary>Loops through all collided monogame objects and gets desired position</summary>
		/// <param name="collided">All collided IMGO's which've been intersected with</param>
		/// <param name="direction">The direction to which a new position is required</param>
		/// <returns>New position which sprite should be pushed to</returns>
		protected float? LoopGetPositionFromDirection(ICollideable[] collided, Direction direction) {
			var position = IntersectingPositions.Empty; // to hold intersecting positions

			foreach (ICollideable _object in collided) // For all collided IMonoGameObjects
				position.Set(direction, Boundary.GetNonIntersectingPositions(_object.Boundary)[direction]);

			return position[direction]; //
		}

		protected float? LoopGetPositionFromDirection(Vector2 offset, ICollideable[] collided, Direction direction) {
			return CheckOffsetMethodCaller(offset, () => LoopGetPositionFromDirection(collided, direction));
		}

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Update root method
		}

		/// <summary>Calculates the horizontal displacement of the sprite with a given velocity</summary>
		public float GetHorizontalDisplacement() {
			return (Math.Abs(velocity.X) > TerminalVelocity.X ? TerminalVelocity.X : velocity.X) * elapsedTime;
		}

		/// <summary>Calculates the vertical displacement of the sprite with a given velocity</summary>
		public float GetVerticalDisplacement() {
			return (Math.Abs(velocity.Y) > TerminalVelocity.Y ? TerminalVelocity.Y : velocity.Y) * elapsedTime;
		}
		
		/// <summary>Velocity of body/mesh</summary>
		public Vector2 velocity = Vector2.Zero;

		/// <summary>Default falling velocity of sprite (starting from 0 too subtle)</summary>
		protected float defaultFallingVelocity = GV.MonoGameImplement.gravity * 2; // 2 seconds after falling

		/// <summary>Gets horizontal displacement offered by velocity</summary>
		public float XDisplacement { get { return GetHorizontalDisplacement(); } }

		/// <summary>Gets vertical displacement offered by velocity</summary>
		public float YDisplacement { get { return GetVerticalDisplacement(); } }

		/// <summary>Combines both horizontal and vertical displacement into a Vector2</summary>
		public Vector2 Displacement { get { return velocity * elapsedTime; } }

		/// <summary>The greatest velocity of the given body/mesh</summary>
		public virtual Vector2 TerminalVelocity { get; protected set; } = new Vector2(0, 125);
		
		/// <summary>Acceleration of given sprite when not at rest/mobile</summary>
		public virtual Vector2 Acceleration { get; protected set; } = new Vector2(0, 0);

		public virtual float GravitationalAcceleration { get; protected set; } = GV.MonoGameImplement.gravity;
	}
}

