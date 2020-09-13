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
	public enum PlayerPerks {
		SetWaterGravityToNothing,
		PlayerImmortal,
		InfiniteShuriken
	}

	/// <summary>Player Object Instance</summary>
	public sealed partial class Player : BodySprite {
		#region Constructors
		static Player() { defaultJumpVelocity = -GV.Physics.GetJumpVelocity(GV.MonoGameImplement.gravity, maxJumpHeight); }

		public Player() : this(new Vector2()) {  }

		public Player(Vector2 position, bool aRunning = true) : base(position, 32, 32, aRunning) {
			Initialize(@"cs\main"); // Initialize directly from construction
			Layer = 0.5f; // set sprite layer to just above block
			ImplementPerks(PlayerPerks.SetWaterGravityToNothing);
		}
		#endregion
		
		public void Attack(int damage) {
			if (timeInvincible <= 0 && !CurrentWeapon.Attacking) { // Attack when player isn't invincible
				GV.MonoGameImplement.GameHUD.TryTakeDamage(damage); // Take from health

				if (GV.MonoGameImplement.GameHUD.Health == 0) GameWindow.GameRunning.InvokePlayerDeceased(); else {
					HitSprite hs = new HitSprite(Position, 18, 18, HitSprite.HitType.Red);
					GV.MonoGameImplement.additionBatch.AddNameless(hs); // Add to local store

					IMonoGameObject[] effect = FX.ValueChangedEffect.Create(
						Position /*- new Vector2(50)*/, -damage, colors: FX.ValueChangedEffect.FlickerColor.Red
					);

					foreach (IMonoGameObject effectSprite in effect) GV.MonoGameImplement.additionBatch.AddNameless(effectSprite);

					Animation.Opacity = 0.65f; // Make somewhat see through
					timeInvincible = 1200; // Can't be hurt for 1.2 seconds
				}
			}
		}

		public override void Initialize(string textureKey) {
			base.Initialize(textureKey);
			sword    = new Weapons.Sword(Vector2.Zero);
			shuriken = new Weapons.Shuriken(Vector2.Zero);

			weapons = new Weapon[] { sword, shuriken };

			Equip(sword); // Starter weapon = sword
		}

		public override void Draw() { base.Draw(); CurrentWeapon.Draw(); }

		public override void Update(bool updateAnimation) {
			base.Update(updateAnimation); // Updates elapsed gametime as well

			#region Inputs
			bool leftInput          = GV.PeripheralIO.currentControlState.Left;
			bool rightInput         = GV.PeripheralIO.currentControlState.Right;
			bool jumpInput          = GV.PeripheralIO.currentControlState.Jump;
			bool dashInput          = GV.PeripheralIO.currentControlState.Dash;
			bool previousLeftInput  = GV.PeripheralIO.previousControlState.Left;
			bool previousRightInput = GV.PeripheralIO.previousControlState.Right;
			bool prevJumpInput      = GV.PeripheralIO.previousControlState.Jump;
			bool weaponAttackInput  = GV.PeripheralIO.currentControlState.Attack;
			bool weaponThrowInput   = GV.PeripheralIO.currentControlState.Throw;
			bool weaponSwitchInput  = GV.PeripheralIO.currentControlState.WeaponNext;
			#endregion

			#region Weapons
			if (!thrownButtonReleased && !weaponThrowInput) thrownButtonReleased = true;

			if (weaponThrowInput && !CurrentWeapon.Thrown) {
				ThrowWeapon(); thrownButtonReleased = false;
			} else if (CurrentWeapon.Thrown) {
				if (CurrentWeapon.returningToPlayer && Intersects(CurrentWeapon)) {
					Equip(CurrentWeapon); // Re equip once intersected with player & looking for player
				} else if (thrownButtonReleased && weaponThrowInput) {
					CallThrownWeaponBack();
				}
			} 

			if (weaponSwitchTimeout > 0) {
				// If button released, erase timeout, else complete timeout

				if (!weaponSwitchInput) weaponSwitchTimeout = 0; else {
					weaponSwitchTimeout -= elapsedMilitime;
				}
			} else if (weaponSwitchInput) {
				SwitchToNextWeapon(); // Move to next weapon in inventory
				weaponSwitchTimeout = 500; // Half a second timeout
			}

			CurrentWeapon.Update(updateAnimation); // Update anim as well

			if (weaponAttackInput) AttackWeapon();
			#endregion

			#region CheckDamage
			if (timeInvincible > 0) {
				timeInvincible -= elapsedMilitime;

				if (timeInvincible <= 0) Animation.Opacity = 1.0f; // Make opaque
			} else {
				IMonoGameObject[] playerDamagingProjectiles = CompoundIntersects<IDamagingToPlayer>();

				if (playerDamagingProjectiles.Length > 0) {
					int damage = (from X in playerDamagingProjectiles select (X as IDamaging).GetDamage()).Aggregate((a, b) => a + b);
					Attack(damage); // Attack player with given damage. Note if simultaneously attacked by multiple then sum damage
				}
			}
			#endregion

			#region BurstPointGeneration
			if (CanGenerateBurstPoints()) { // Player is capable of generating burst points
				timeNotMoving += elapsedTime; // Increment the known time in which player is not moving

				if (timeNotMoving > TIME_BEFORE_POINT_GEN) { // Time before points start being generated
					pointGenerationRate += pointGenerationAcceleration * elapsedTime;
					generatingBurstPoints += pointGenerationRate * elapsedTime; // Increment generated burst points

					// if (generatingBurstPoints >= 1) {
						int incrementValue = (int)Math.Floor(generatingBurstPoints); // Value to increase
						generatingBurstPoints = generatingBurstPoints - incrementValue; // Set to point val

						burstPoints = (burstPoints + incrementValue < maxBurstPoints) ? burstPoints + incrementValue : maxBurstPoints;
					// }
				}
			} else { timeNotMoving = 0; generatingBurstPoints = 0; pointGenerationRate = DEFAULT_POINT_GENERATION_RATE; }
			#endregion

			#region Interaction
			if (interaction_timeout > 0) interaction_timeout -= elapsedMilitime; else if (GV.PeripheralIO.currentControlState.Interact) {
				foreach (var interactable in CompoundIntersects<IInteractable>()) {
					bool isAutoInteractable = interactable is IAutoInteractable;

					if (!isAutoInteractable) ((IInteractable)interactable).Interact();
				}
			}
			#endregion

			#region LinearMovement
			if ((previousLeftInput && rightInput) || (previousRightInput && leftInput)) {
				velocity.X = 0; // If direction of movement has changed
			}

			if (!Dashing) { // Only allow player movement control when not dashing
				if ((leftInput && rightInput) || !(leftInput || rightInput)) { // If both or neither
					StopRunning(); // Reset any forward velocity/acceleration until desired
					StallAnimation(); // Stop movement and keep facing in same direction
				} else { // Left input ^| Right input
					String facingDirection = (leftInput) ? "Left" : "Right";

					bool facingDirectionChanged = (FacingLeft && facingDirection == "Right") || (FacingRight && facingDirection == "Left");

					Animation.SetAnimationSequence($"running{facingDirection}"); // Set running in facing direction for animation

					if (facingDirectionChanged) { // Re-Adjust position of sword
						if (!(CurrentWeapon.Thrown || CurrentWeapon.Attacking)) CurrentWeapon.AttachToPlayer(this); // Also sets position
						//if (!CurrentWeapon.Thrown) CurrentWeapon.SetWeaponPositionRelativeToPlayer(this);
					}

					ImplementRunning(leftInput); // run in input direction
				}
			}
			#endregion

			#region Jumping
			if (prevJumpInput && !jumpInput) StopJumping(); else if (jumpInput) ImplementJumping(); // Implement jumping
			#endregion

			#region Dashing Or Gravity
			if (dashInput && CanDash()) {
				dashArgs = new DashArgs(this, Animation.sequenceKey.Contains("Left"));
				velocity = Vector2.Zero; // Switch off velocity temporarily
				StallAnimation(); // Stall animation in facing direction

				SubtractBurstPoints(GV.Variables.playerDashCost); // Cost to dash
			} else if (Dashing) ImplementDashing(); else ImplementGravity();
			#endregion

			if (GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Top")) {
				VerticalUpwardBoundaryFix();
				if (jumpInput || prevJumpInput) StopJumping();
			}
		}

		#region BurstPointManagement
		/// <summary>Gives player more burst points</summary>
		/// <param name="increment">Value to add</param>
		public void AddBurstPoints(int increment) {
			burstPoints = (burstPoints + increment < maxBurstPoints) ? burstPoints + increment : maxBurstPoints;
		}

		/// <summary>Takes burst points from user</summary>
		/// <param name="value">Value to subtract</param>
		public void SubtractBurstPoints(int value) {
			burstPoints = (burstPoints - value > 0) ? burstPoints - value : 0;
		}
		#endregion

		#region MovementImplementation
		/// <summary>Implements dashing</summary>
		private void ImplementDashing() {
			if (dashArgs.Update()) return; // Still dashing

			velocity = dashArgs.dashingVelocity;
			dashArgs = null; // Delete dash args
		}

		/// <summary>Makes player runi in desired direction</summary>
		/// <param name="runningLeft">Direction player is running. False = runningRight</param>
		private void ImplementRunning(bool runningLeft) {
			if (velocity.X == 0) velocity.X = defaultRunningVelocity; // At start = default

			velocity.X += Acceleration.X * elapsedTime; // Horizontal Velocity
			OffsetSpritePosition(X: (runningLeft ? -1 : 1) * XDisplacement);

			if (HasIntersectedTypes(new Vector2(0, -1), GV.MonoGameImplement.BlockTypes)) {
				IMonoGameObject[] collided = CompoundIntersects(GV.MonoGameImplement.BlockTypes); // All collided objects

				String[] intersectingLabels = GetIntersectedBoundaryLabels(collided, GV.MonoGameImplement.BlockTypes);

				bool leftIntersection = intersectingLabels.Contains("Left"), rightIntersection = intersectingLabels.Contains("Right");
				bool verticalIntersection = intersectingLabels.Contains("Top") || intersectingLabels.Contains("Bottom"); // Top || Bottom

				if (!(leftIntersection || rightIntersection) && verticalIntersection) {
					float cornerLeft = ((IBRectangle)TrueBoundary["Top"]).Left, cornerRight = ((IBRectangle)TrueBoundary["Top"]).Right;

					float furthestRight = (from X in collided select (X as ICollideable).Boundary.Container.Right).Aggregate(Math.Max);
					float furthestLeft  = (from X in collided select (X as ICollideable).Boundary.Container.Left ).Aggregate(Math.Min);

					bool pastLeft = cornerLeft > furthestLeft, beforeRight = cornerRight < furthestRight;

					if (pastLeft && beforeRight) {
						bool fixRight = cornerLeft - furthestLeft > furthestRight - cornerRight; // Determines
						// Whether further into block on left or on right & chooses appropriate collision handler
						if (fixRight) HorizontalMovingRightBoundaryFix(); else HorizontalMovingLeftBoundaryFix();
					} else if (pastLeft) HorizontalMovingLeftBoundaryFix(); else if (beforeRight) HorizontalMovingRightBoundaryFix();

				} else if (leftIntersection) HorizontalMovingLeftBoundaryFix(); else if (rightIntersection) HorizontalMovingRightBoundaryFix();

				StopRunning(); // Reset forward or backward velocity
			}
		}

		private void StopRunning() {
			velocity.X = 0;
		}

		private void ImplementJumping() {
			if (!Falling(1, 0) && !Falling(-1, 0)) {
				velocity.Y = defaultJumpVelocity; // Set players horizontal velocity
				OffsetSpritePosition(Y: YDisplacement); // Push sprites vertical position
				jumpButtonReleased = false; // Can't dash until jump button released
			}
		}

		private void StopJumping() {
			jumpButtonReleased = true; // Released so if player enters again, dashing is enabled and player dashes in desired direction
			velocity.Y = GV.Physics.GetEarlyJumpEndVelocity(velocity.Y, GV.MonoGameImplement.gravity, maxJumpHeight, minJumpHeight);
		}

		protected override void StopFalling() {
			base.StopFalling(); // Call child method
			jumpButtonReleased = false; // Prevent dash
		}
		#endregion

		#region ConditionalCalculators
		public bool CanDash() { return (Falling(-1, 0) || Falling(-1, 0)) && burstPoints >= GV.Variables.playerDashCost && !Dashing; }

		public bool CanJump() { return (!Falling(new Vector2(1, 0)) || !Falling(new Vector2(-1, 0))) && !jumpButtonReleased; }

		private bool CanGenerateBurstPoints() {
			return (
					GV.PeripheralIO.currentControlState.GamepadNotPressed() &&
					GV.PeripheralIO.currentControlState.KeyboardNotPressed()
				)

				&& !Dashing && burstPoints != maxBurstPoints && !Falling(0, 1);
		}
		#endregion

		#region IntersectionCheckers
		public static String[] GetIntersectedBoundaryLabels(Player self, IMonoGameObject[] objects, params Type[] types) {
			HashSet<String> intersected = new HashSet<String>(); // Holds intersected boundaries

			foreach (IMonoGameObject IMGO in objects) {
				if (!(IMGO is ICollideable) || (types.Length > 0 && !types.Contains(IMGO.GetType())))
					continue; // If isn't collideable or doesn't match argument types then continue

				intersected.UnionWith(self.TrueBoundary.GetIntersectingBoundaryLabels((IMGO as ICollideable).Boundary));
			}

			return intersected.ToArray(); // Convert to array and return to sender
		}

		public override bool Falling() {
			return base.Falling() && !GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Bottom"); 
		}

		public String[] GetIntersectedBoundaryLabels(params Type[] types) {
			return GetIntersectedBoundaryLabels(this, GV.MonoGameImplement.monogameObjects.ToArray(), types);
		}

		public String[] GetIntersectedBoundaryLabels(IMonoGameObject[] objects, params Type[] types) {
			return GetIntersectedBoundaryLabels(this, objects, types);
		}

		public String[] GetIntersectedBoundaryLabels(Vector2 offset, IMonoGameObject[] objects, params Type[] types) {
			return CheckOffsetMethodCaller(offset, () => GetIntersectedBoundaryLabels(this, objects, types));
		}

		public String[] GetIntersectedBoundaryLabels(Vector2 offset, params Type[] types) {
			return CheckOffsetMethodCaller(offset, () => GetIntersectedBoundaryLabels(types));
		}
		#endregion

		#region CollisionHandling
		#region CollisionHandlerDelegates
		private delegate bool ValidityCheck(IBoundaryContainer boundary, bool horizontalCollision, bool VerticalCollision);

		private delegate float FinalCalculator(bool horizontalCollision, bool verticalCollision, float newPosition);
		#endregion

		protected override bool FixDownwardCollisionAfterFalling() {
			return base.FixDownwardCollisionAfterFalling() && GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Bottom");
		}

		private float? BoundaryFixSharedHandler(Direction direction, ValidityCheck checker, FinalCalculator calc, Vector2? _offset=null) {
			Vector2 offset = (_offset.HasValue) ? _offset.Value : Vector2.Zero; // If has value set to it, otherwise set to empty

			String[] intersectedLabels = GetIntersectedBoundaryLabels(offset, GV.MonoGameImplement.BlockTypes); // Get All Labels 
			if (intersectedLabels.Length == 0) return null; // No collision has occured, return to sender. Almost impossible but just in case :)

			bool horizontalCollision = intersectedLabels.Contains("Left") || intersectedLabels.Contains("Right"); // Whether left or right
			bool verticalCollision   = intersectedLabels.Contains("Bottom"); // Contains bottom collision, just in case !leftRight but Bottom

			IMonoGameObject[] collided = CompoundIntersects(GV.MonoGameImplement.BlockTypes);

			if (collided.Length > 1) collided = (from X in collided where checker((X as ICollideable).Boundary, // Use alternative ->
				horizontalCollision, verticalCollision) select X).ToArray(); // Overloaded method accounting for labelled in boundary type

			if (collided.Length == 0) return null; // No collision has occured, return to sender just in case :)

			// Console.WriteLine((from X in collided select X.SpriteID).Aggregate((a, b) => $"{a}, {b}"));

			#region RebuildVariablesWithTrimmedCollidedList
			// This region is used to eliminate the bug where player is falling right on the edge between two blocks & passes into it
			intersectedLabels = GetIntersectedBoundaryLabels(offset, collided, GV.MonoGameImplement.BlockTypes); // Rebuilds intersected labels

			horizontalCollision = intersectedLabels.Contains("Left") || intersectedLabels.Contains("Right"); // Whether left or right
			verticalCollision   = intersectedLabels.Contains("Bottom"); // Contains bottom collision, just in case !leftRight but Bottom
			#endregion // May be repetitive, but effectively erases quite the pesky bug, DO NOT DELETE!!!

			float? setValue = LoopGetPositionFromDirection(collided.Cast<ICollideable>().ToArray(), direction); // Find New Position

			if (setValue.HasValue) return calc(horizontalCollision, verticalCollision, setValue.Value); else return null; 
		}

		private void VerticalBoundaryFixSharedHandler(Direction direction, float multiplier) {
			FinalCalculator calculator = (h, v, n) => n + (multiplier * (h ? leftRightBoundaryYOffset : 0));
			float? pos = BoundaryFixSharedHandler(direction, VerticalBoundaryFixValidityCheck, calculator);
			if (pos.HasValue) { SetPosition(Y: pos.Value); BuildBoundary(); }
		}

		private void HorizontalBoundaryFixSharedHandler(Direction direction, float multiplier) {
			FinalCalculator calculator = (h, v, n) => n + (multiplier * (v /*&& !h*/ ? leftRightBoundaryXOffset - 2 : 0));
			float? pos = BoundaryFixSharedHandler(direction, HorizontalBoundaryFixValidityCheck, calculator, new Vector2(0, 0));
			if (pos.HasValue) { SetPosition(X: pos.Value); BuildBoundary(); } // Set position and rebuild boundary for player now
			// Remark // Boundary rebuilt because of Weird rounding bug where value is off by 0.00004. For now do this, fix later.
		}

		protected override void VerticalDownwardBoundaryFix() { VerticalBoundaryFixSharedHandler(Direction.Top, +1); }

		private void VerticalUpwardBoundaryFix() { VerticalBoundaryFixSharedHandler(Direction.Bottom, -1); OffsetSpritePosition(0, 1); }

		private void HorizontalMovingLeftBoundaryFix() { HorizontalBoundaryFixSharedHandler(Direction.Right, -1); }

		private void HorizontalMovingRightBoundaryFix() { HorizontalBoundaryFixSharedHandler(Direction.Left, +1); }

		private bool VerticalBoundaryFixValidityCheck(IBoundaryContainer container, bool horizontal, bool vertical) {
			if (!horizontal && !vertical) return false; else /*(leftRight || bottom)*/ {
				// A = leftSpriteRect, B = rightSpriteRect, C = bottomSpriteRect, D = containerSpriteRect
				Rectangle A = TrueBoundary["Left"].Container,   B = TrueBoundary["Right"].Container;
				Rectangle C = TrueBoundary["Bottom"].Container, D = container.SpriteRect; // Get rects

				Func<Rectangle, bool> finder = (R) => R.X >= D.X - R.Width && R.X + R.Width <= D.X + D.Width;

				bool bottomValid     = finder(C); // Vertical valid, Top isn't really relevent here
				bool horizontalValid = finder(A) && finder(B); // F(A) = LeftValid, F(B) = RightValid

				return ( horizontal &&  vertical &&  bottomValid &&  horizontalValid)  // Bottom & Top Check 
					|| (!horizontal &&  vertical &&  bottomValid &&  true           )  // Bottom       Check
					|| ( horizontal && !vertical &&  true        &&  horizontalValid); // Top          Check
			}
		}

		private bool HorizontalBoundaryFixValidityCheck(IBoundaryContainer container, bool horizontal, bool vertical) {
			if (!horizontal && !vertical) return false; else /*(leftRight || bottom)*/ {
				// A = TopSpriteRect, B = BottomSpriteRect, C = Left/RightSpriteRect, D = containerSpriteRect
				Rectangle A = TrueBoundary["Top"].Container,  B = TrueBoundary["Bottom"].Container;
				Rectangle C = TrueBoundary["Left"].Container, D = container.SpriteRect; // Get rects
				// S.N. Used left for left & right, because they share same Y-Value which're all needed

				Func<Rectangle, bool> finder = (R) => D.Y + D.Height >= R.Y + R.Height && D.Y - R.Height < R.Y;

				return finder(C) || finder(B) || finder(A); // F(C) = HorizontalValid, F(B) || F(A) = VerticalValid
			}
		}
		#endregion

		#region Perks
		public void ImplementPerks(params PlayerPerks[] _perks) {
			foreach (PlayerPerks perk in _perks) {
				switch (perk) {

				}

				perks.Add(perk);
			}
		}

		public void RemovePerk(PlayerPerks perk) {

		}
		#endregion

		public override void OffsetSpritePosition(Vector2 XY) {
			base.OffsetSpritePosition(XY); // Takes care of sprite push

			if (!CurrentWeapon.Thrown) CurrentWeapon.OffsetSpritePosition(XY);
		}

		public override void SetPosition(Vector2 nPos) {
			base.SetPosition(nPos); // Sets position for sprite

			if (!CurrentWeapon.Thrown) CurrentWeapon.OffsetSpritePosition(nPos - Position);
		}

		#region Weapons
		private void Equip(Weapon weapon) {
			CurrentWeapon = weapon; // In case weapon was not equipped
			CurrentWeapon.Reset(this);
		}

		public float GetAnglePointedToByInput() {
			if (GV.PeripheralIO.gamePadConnected && GV.PeripheralIO.currentGPState.ThumbSticks.Right != Vector2.Zero) {
				Vector2 rightThumbstick = GV.PeripheralIO.currentGPState.ThumbSticks.Right;
				return (float)Math.Atan2(-rightThumbstick.Y, rightThumbstick.X); // Get angle
			} else { // Determine from regular input
				var _default = FacingLeft ? DashArgs.DashingDirection.Left : DashArgs.DashingDirection.Right;

				DashArgs.DashingDirection direction = DashArgs.GetDashingDirection(_default); // Get direction

				return GV.BasicMath.DegreesToRadians(45 * (int)direction); // Cast type to int and return to caller
			}
		}

		private void ThrowWeapon() { if (CurrentWeapon.CanThrow) CurrentWeapon.Throw(GetAnglePointedToByInput()); }

		private void AttackWeapon() { if (CurrentWeapon.CanAttack) CurrentWeapon.Attack(this); }

		private void CallThrownWeaponBack() { if (CurrentWeapon.CanCallThrownWeaponBack) CurrentWeapon.ReturnThrownWeaponToPlayer(); }

		private void SwitchToNextWeapon() {
			int currentIndex = Array.IndexOf(weapons, CurrentWeapon); // Current Weapon
			Equip(weapons[(currentIndex + 1 < weapons.Length) ? currentIndex + 1 : 0]);
		}
		#endregion

		#region Animation
		protected override void BuildSequenceLibrary() {
			Animation["idleLeft"]     = AnimationSequence.FromRange(32, 32, 0, 0, 1, 32, 32);
			Animation["idleRight"]    = AnimationSequence.FromRange(32, 32, 0, 1, 1, 32, 32);
			Animation["runningLeft"]  = AnimationSequence.FromRange(32, 32, 0, 0, 3, 32, 32);
			Animation["runningRight"] = AnimationSequence.FromRange(32, 32, 0, 1, 3, 32, 32);
			Animation["idleUpLeft"]   = AnimationSequence.FromRange(32, 32, 3, 0, 1, 32, 32);
			Animation["idleUpRight"]  = AnimationSequence.FromRange(32, 32, 3, 1, 1, 32, 32);
			
			SequenceKey = "idleRight"; // Default sequence is player looking to the right
		}

		/// <summary>Get rid of running animation</summary>
		public void StallAnimation() {
			String lastLookingDirection = (Animation.sequenceKey.Contains("Left")) ? "Left" : "Right";
			Animation.SetAnimationSequence($"idle{lastLookingDirection}"); // Pointing but stationary
		}
		#endregion

		protected override void BuildBoundary() {
			#region SizeCalculations/Definitions
			int leftRightWidth  = (SpriteRect.Width / 2) - leftRightBoundaryXOffset; // Left Right Boundary Width
			int leftRightHeight = SpriteRect.Height - (2 * leftRightBoundaryYOffset); // Left Right Boundary Height

			int topBottomHeight = (SpriteRect.Height - topBottomYOffset) / 2; // Top Bottom Boundary Height
			int topBottomWidth = SpriteRect.Width - (2 * topBottomXOffset); // Top Bottom Boundary Width

			Vector2 leftRightSize = new Vector2(leftRightWidth, leftRightHeight);
			Vector2 topBottomSize = new Vector2(topBottomWidth, topBottomHeight);
			#endregion

			#region PositionCalculations/Definitions
			Vector2 leftPos = new Vector2(SpriteRect.X + leftRightBoundaryXOffset, SpriteRect.Y + leftRightBoundaryYOffset);
			Vector2 topPos  = new Vector2(SpriteRect.X + topBottomXOffset,         SpriteRect.Y + topBottomYOffset        );

			Vector2 rightPos  = new Vector2(SpriteRect.X + leftRightBoundaryXOffset + leftRightWidth, SpriteRect.Y + leftRightBoundaryYOffset);
			Vector2 bottomPos = new Vector2(SpriteRect.X + topBottomXOffset, SpriteRect.Y + topBottomYOffset + topBottomHeight);
			#endregion

			boundary = new LabelledBoundaryContainer(SpriteRect) {
				{ "Left",   new IBRectangle(leftPos,   leftPos   + leftRightSize) },
				{ "Right",  new IBRectangle(rightPos,  rightPos  + leftRightSize) },
				{ "Top",    new IBRectangle(topPos,    topPos    + topBottomSize) },
				{ "Bottom", new IBRectangle(bottomPos, bottomPos + topBottomSize) }
			};
		}

		#region ClassElements
		/// <summary>Variables used to build player boundary rectangles in TrueBoundary/LabelledBoundaryContainer</summary>
		private const int leftRightBoundaryXOffset = 6, leftRightBoundaryYOffset = 10, topBottomXOffset = 10, topBottomYOffset = 2;

		/// <summary>Maximum height player can jump, min height player should jump</summary>
		private static int maxJumpHeight = 64, minJumpHeight = 15;

		/// <summary>Default velocity when player jumping & when player running</summary>
		private static float defaultJumpVelocity, defaultRunningVelocity = 50 * 1f;

		/// <summary>Maximum value velocity can reach in either direction</summary>
		public override Vector2 TerminalVelocity { get; protected set; } = new Vector2(125, 125);

		/// <summary>Default acceleration when running and falling (not including gravity)</summary>
		public override Vector2 Acceleration { get; protected set; } = new Vector2(50, 0);

		/// <summary>Boundary casted to actual type, nicer when accessing player exclusive keys</summary>
		public LabelledBoundaryContainer TrueBoundary { get { return (LabelledBoundaryContainer)boundary; } }

		/// <summary>Value points used to determine player dashing capabilities</summary>
		public int burstPoints = maxBurstPoints;

		/// <summary>Assists in generation of burst points by standing still</summary>
		private float generatingBurstPoints, pointGenerationRate = DEFAULT_POINT_GENERATION_RATE, pointGenerationAcceleration=0.2f;

		private const float DEFAULT_POINT_GENERATION_RATE = 1f;

		/// <summary>Time spent where player is not moving</summary>
		public float timeNotMoving = 0;

		public static readonly int TIME_BEFORE_POINT_GEN = 4;

		/// <summary>Maximum amount of burst points player is allowed to have </summary>
		public static int maxBurstPoints = 25;

		/// <summary>Arguments used for dashing. When null, not dashing.</summary>
		private DashArgs dashArgs;

		/// <summary>Whether player has released jump button</summary>
		private bool jumpButtonReleased;

		public int interaction_timeout = 0;

		private bool thrownButtonReleased = false;

		public bool FacingLeft { get { return Animation.sequenceKey.Contains("Left"); } }

		public bool FacingRight { get { return !FacingLeft; } }

		private List<PlayerPerks> perks = new List<PlayerPerks>();

		public Weapons.Sword sword;

		public Weapons.Shuriken shuriken;

		private Weapon[] weapons;

		private int weaponSwitchTimeout = 0;

		public Weapon CurrentWeapon { get; private set; }

		private int timeInvincible = 0;

		public PlayerPerks[] Perks { get { return perks.ToArray(); } }

		/// <summary>Whether player is cucrrently dashing</summary>
		private bool Dashing { get { return dashArgs != null; } }
		#endregion

		public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
	}
}
