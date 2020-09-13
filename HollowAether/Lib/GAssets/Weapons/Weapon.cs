using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;

using HollowAether.Lib.GAssets;
using HollowAether.Lib.Exceptions;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GAssets {
	public abstract class Weapon : CollideableSprite {
		public Weapon(Vector2 position, int width, int height, bool animationRunning)
			: base(position, width, height, animationRunning) { Layer = 0.65f; }

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates animation

			if (Thrown) UpdateThrown(); else if (Attacking) UpdateAttack(); else {
				ImplementFloatingEffect(); // Make go up and down
			}
		}

		private void ImplementFloatingEffect() {
			timeFloating += elapsedMilitime; // 

			OffsetSpritePosition(Y: floatingDirection * floatingVelocity * elapsedTime);

			if (timeFloating >= 2000) {
				timeFloating -= 2000; // Push time back
				floatingDirection = -floatingDirection;
			}
		}

		public int GetDamage() {
			return 25;
		}

		public abstract void SetWeaponPositionRelativeToPlayer(Player player);

		public virtual void Throw(float direction) {
			if (!CanBeThrown) throw new HollowAetherException($"Weapon '{this.GetType()}' Can't Be Thrown");
		}

		public virtual void StopThrowing() { Thrown = false; }

		public virtual void ReturnThrownWeaponToPlayer() {
			returningToPlayer = true;
		}

		protected virtual void UpdateThrown() { } // Optional

		protected abstract void UpdateAttack();

		public virtual void Reset(Player player) {
			if (Thrown) StopThrowing();

			AttachToPlayer(player);
		}

		public virtual void Attack(Player player) {
			Attacking = true;
		}

		public abstract void AttachToPlayer(Player player);

		protected virtual void FinishAttacking() {
			Attacking = false;
		}

		private float floatingVelocity = 3f;

		private int timeFloating = 0, floatingDirection = +1;

		public bool returningToPlayer = false;

		public bool Thrown { get; protected set; } = false;

		public abstract bool CanBeThrown { get; protected set; }

		public bool Attacking { get; protected set; } = false;

		public bool CanAttack { get { return !Attacking && !Thrown; } }

		public bool CanThrow { get { return CanBeThrown && CanAttack; } }

		public bool CanCallThrownWeaponBack { get { return CanBeThrown && Thrown && !returningToPlayer; } }
	}

	public abstract class SwordLikeWeapon : Weapon {
		public SwordLikeWeapon(Vector2 position, int width, int height, bool animationRunning)
			: base(position, width, height, animationRunning) { }

		public override void Attack(Player player) {
			base.Attack(player);

			swingDirection = GV.MonoGameImplement.Player.FacingLeft ? -1 : 1;
			TrueBoundary.Rotation = (float)(-swingDirection * Math.PI / 4);

			Animation.SetAnimationSequence(swingDirection > 0 ? "315" : "045");

			SwingAcceleration = new Vector2(-swingDirection *     SWING_ACCELERATION, -SWING_ACCELERATION);
			swingVelocity     = new Vector2(+swingDirection * SWING_INITIAL_VELOCITY, +SWING_INITIAL_VELOCITY);
		}

		protected override void UpdateAttack() {
			if (!backSwing) { TrueBoundary.Rotation += SWING_ANGULAR_SPEED * elapsedTime; } else {
				float newRotation = TrueBoundary.Rotation - (SWING_ANGULAR_SPEED * elapsedTime); // Store new rotation

				newRotation = (newRotation < 0) ? newRotation : (float)GV.BasicMath.NormaliseRadianAngle(newRotation);
				TrueBoundary.Rotation = newRotation; // Set rotation to new normalised rotation. Always positive, kay.
			}

			SetAnimationFromRotation(); // Set animation for sword from current sprite rotation.
			
			if (!backSwing && TrueBoundary.Rotation > MAX_SWING_ANGULAR_ROTATION) {
				backSwing = true; // Set back swing to true to reverse swing animation
			} else if (backSwing && TrueBoundary.Rotation < 0) FinishAttacking();

			swingVelocity += SwingAcceleration * elapsedTime; // Increment velocity
			OffsetSpritePosition(swingVelocity * elapsedTime); // Give displacement
		}

		protected override void FinishAttacking() {
			base.FinishAttacking();

			Player player = GV.MonoGameImplement.Player;

			backSwing = false; // Reset backswing for next attack
			AttachToPlayer(player); // Sets rotation & position
		}

		public override void Throw(float direction) {
			base.Throw(direction); // Throws exception if trying to throw non throwable

			Thrown = true; timeBeingThrown = 0; returningToPlayer = false; // Reset throw vars

			Vector2 sinCosRatio = new Vector2(
				(float)Math.Round(Math.Cos(direction), 5),
				(float)Math.Round(Math.Sin(direction), 5)
			);

			thrownLinearVelocity = sinCosRatio * new Vector2(DefaultLinearThrowVelocity);
			throwDeceleration    = sinCosRatio * new Vector2(DefaultThrowDeceleration);
			
			thrownDirection = new Vector2((thrownLinearVelocity.X > 0) ? +1 : -1, (thrownLinearVelocity.Y > 0) ? +1 : -1);
			
			initialHorizontalVelocity0 = thrownLinearVelocity.X == 0;
			initialVerticalVelocity0   = thrownLinearVelocity.Y == 0;
		}

		protected override void UpdateThrown() {
			TrueBoundary.Rotation += AngularThrowVelocity * elapsedTime; // Further weapon rotation as required
			SetAnimationFromRotation(); // Set animation for sword from current sprite rotation. 

			if (returningToPlayer) UpdateReturningToPlayer(); else {
				OffsetSpritePosition(thrownLinearVelocity * elapsedTime); // Offset due to linear velocity
				thrownLinearVelocity -= throwDeceleration * elapsedTime;  // Decelerate linear velocity as required

				bool directionChangedHorizontally = !initialHorizontalVelocity0 && DirectionChanged(thrownLinearVelocity.X, thrownDirection.X);
				bool directionChangedVertically   = !initialVerticalVelocity0   && DirectionChanged(thrownLinearVelocity.Y, thrownDirection.Y); 
				
				if (directionChangedHorizontally || directionChangedVertically) { ReturnThrownWeaponToPlayer(); }
			}
		}

		private static bool DirectionChanged(float velocity, float thrownDirection) {
			return (velocity > 0 && thrownDirection < 0) || (velocity < 0 && thrownDirection > 0);
		}

		public override void ReturnThrownWeaponToPlayer() {
			base.ReturnThrownWeaponToPlayer();

			SetAngleVelocityEtcToPlayer(); // Set initial return velocity to towards player
		}

		private void SetAngleVelocityEtcToPlayer() {
			Vector2 player = GV.MonoGameImplement.Player.Position; // Store position for theta calc
			float directionTheta = (float)Math.Atan2(player.Y - Position.Y, player.X - Position.X);
			Vector2 sinCosRatio = new Vector2((float)Math.Cos(directionTheta), (float)Math.Sin(directionTheta));

			thrownLinearVelocity = sinCosRatio * new Vector2(DefaultReturnThrowVelocity); // Return to player
		}

		private void UpdateReturningToPlayer() {
			OffsetSpritePosition(thrownLinearVelocity * elapsedTime); // Offset due to linear velocity
			thrownLinearVelocity -= throwDeceleration * elapsedTime; // Change acceleration appropriately

			SetAngleVelocityEtcToPlayer(); // Got tired of deciding when so just always do
		}

		public override void AttachToPlayer(Player player) {
			TrueBoundary.Rotation = (player.FacingRight) ? FACING_RIGHT_ROTATION : FACING_LEFT_ROTATION;
			SetAnimationFromRotation(); // Set animation for sword from current sprite rotation. 

			SetWeaponPositionRelativeToPlayer(player); // Whether to place weapon to left or right of player.
		}

		public override void SetWeaponPositionRelativeToPlayer(Player player) {
			Vector2 direction = new Vector2((player.FacingLeft) ? +1 : -1, -1); // Direction to push on x axis

			OffsetSpritePosition((player.Position + (direction * new Vector2(Size.X / 2, Size.Y / 4))) - Position);
		}

		protected void SetAnimationFromRotation() {
			int degrees = (int)Math.Round(GV.BasicMath.RadiansToDegrees(TrueBoundary.Rotation));

			foreach (int X in Enumerable.Range(0, 8)) {
				if (45 * X <= degrees && degrees < 45 * (X + 1)) { // Between range 45
					Animation.SetAnimationSequence((45 * X).ToString().PadLeft(3, '0'));
					break; // Once valid animation set, break method & return to caller
				}
			}
		}

		/// <summary>Recursively determines what rotation weapon is on and subsequently set weapon animation</summary>
		/// <param name="degrees">Actual degrees value. Allow parameterised to prevent repeated calculation</param>
		/// <param name="acuteCount">Current Rotation Multiple Of 45 Degrees. Max should be (360 / 45)-1 = 7</param>
		protected void RecursiveSetAnimationFromRotation(int? degrees=null, int acuteCount=7) {
			degrees = degrees.HasValue ? degrees.Value : (int)Math.Round(GV.BasicMath.RadiansToDegrees(TrueBoundary.Rotation));

			if (acuteCount == -1) return; // initial acute count was too small to effectively reach the appropriate angle

			if (45 * acuteCount <= degrees && degrees < 45 * (acuteCount + 1)) {
				// When acute is within range of 45 Degrees of the current weapon rotation
				Animation.SetAnimationSequence((45 * acuteCount).ToString().PadLeft(3, '0'));
			} else RecursiveSetAnimationFromRotation(degrees, acuteCount - 1);
		}

		protected override void BuildSequenceLibrary() {
			Animation["000"] = new AnimationSequence(0, new Frame(00, 00, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["045"] = new AnimationSequence(0, new Frame(01, 00, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["090"] = new AnimationSequence(0, new Frame(02, 00, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["135"] = new AnimationSequence(0, new Frame(03, 00, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["180"] = new AnimationSequence(0, new Frame(00, 01, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["225"] = new AnimationSequence(0, new Frame(01, 01, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["270"] = new AnimationSequence(0, new Frame(02, 01, FrameWidth, FrameHeight, FrameWidth, FrameHeight));
			Animation["315"] = new AnimationSequence(0, new Frame(03, 01, FrameWidth, FrameHeight, FrameWidth, FrameHeight));

			Animation.SetAnimationSequence("000"); // Default animation points directly up
		}

		protected override void BuildBoundary() {
			float rotation = (boundary != null) ? TrueBoundary.Rotation : 0; // Boundary being rebuilt, so maintain rotation

			Vector2 boundaryPosition = Position + (ActualSize.ToVector2() / 2) + new Vector2(HorizontalBoundaryOffset, 0);

			boundary = new SequentialBoundaryContainer(SpriteRect,
				new IBRotatedRectangle(new Rectangle(boundaryPosition.ToPoint(), ActualSize), rotation)
			);
		}

		#region Properties
		#region Boundaries
		private IBRotatedRectangle TrueBoundary { get { return (IBRotatedRectangle)Boundary.Boundaries[0]; } }

		public abstract int HorizontalBoundaryOffset { get; protected set; }

		#region Dimensions
		public abstract int ActualWeaponWidth { get; protected set; }

		public abstract int ActualWeaponHeight { get; protected set; }

		public abstract int FrameWidth { get; protected set; }

		public abstract int FrameHeight { get; protected set; }

		private Point ActualSize { get { return new Point(ActualWeaponWidth, ActualWeaponHeight); } }
		#endregion
		#endregion

		#region ThrowVars
		#region Kinematics
		public virtual float DefaultLinearThrowVelocity { get; protected set; } = 200f;

		public virtual float DefaultReturnThrowVelocity { get; protected set; } = 200f;

		protected virtual float DefaultThrowDeceleration { get; set; } = 65f;
		#endregion

		public override bool CanBeThrown { get; protected set; } = true;

		public abstract float AngularThrowVelocity { get; protected set; }
		#endregion

		#region SwingVars
		private const float SWING_ANGULAR_SPEED = (float)(Math.PI * 4.5); // 1 Swing every 250 ms
		
		private const float MAX_SWING_ANGULAR_ROTATION = (float)(Math.PI * 1.45f);

		private Vector2 SwingAcceleration { get; set; }
		#endregion

		#endregion

		private bool backSwing;

		private Vector2 swingVelocity;

		protected int timeBeingThrown;

		private int swingDirection = 0; // Direction sword is swung

		private const int SWING_DISPLACEMENT = 24; // On either axis

		private bool initialVerticalVelocity0, initialHorizontalVelocity0;

		private Vector2 thrownDirection, thrownLinearVelocity, throwDeceleration;

		private const float FACING_RIGHT_ROTATION = (float)(335 * (Math.PI / 180));

		private const float FACING_LEFT_ROTATION  = (float)(045 * (Math.PI / 180));

		private const float MILITIME_PER_SWING = (float)((MAX_SWING_ANGULAR_ROTATION + (Math.PI / 4)) / SWING_ANGULAR_SPEED);

		private static readonly float SWING_ACCELERATION = GV.Physics.CalculateGravity(SWING_DISPLACEMENT, MILITIME_PER_SWING);

		private static readonly float SWING_INITIAL_VELOCITY = GV.Physics.GetJumpVelocity(SWING_ACCELERATION, SWING_DISPLACEMENT);
	}
}
