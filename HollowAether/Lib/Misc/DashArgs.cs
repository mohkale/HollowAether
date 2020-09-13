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

#region HollowAetherImports
using HollowAether.Lib.MapZone;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GAssets {
	public sealed partial class Player : BodySprite {
		class DashArgs {
			//public enum DashingDirection { Right, TopRight, Top, TopLeft, Left, BottomLeft, Bottom, BottomRight }

			public enum DashingDirection { Right, BottomRight, Bottom, BottomLeft, Left, TopLeft, Top, TopRight }

			DashArgs(Player self) {
				timeDashing = defaultDashingTime; // Assign to default dashing time. Will dash for this long.
				this.player = self; // Store reference to existing player instance

				FX.DashOverlay overlay = new FX.DashOverlay(self.Position, () => dashOverlayCompleted = true) { Layer = self.Layer + 0.005f };
				GV.MonoGameImplement.additionBatch.AddNameless(overlay); // Store to displayed object store
			}

			public DashArgs(Player self, DashingDirection direction) : this(self) { Initialise(direction); }

			public DashArgs(Player self, bool facingLeft) : this(self) {
				Initialise(GetDashingDirection(GetDefaultDashingDirection(facingLeft)));
			}

			private void Initialise(DashingDirection direction) {
				dashingDirection = direction; // Store direction of movement for current dash
				dashingVelocity = ConvertVelocityMagnitudeToVector(defaultDashingVelocity);

				switch (direction) {
					case DashingDirection.Left:  case DashingDirection.BottomLeft:  case DashingDirection.TopLeft:  xFactor = -1; break;
					case DashingDirection.Right: case DashingDirection.BottomRight: case DashingDirection.TopRight: xFactor = +1; break;
				}

				switch (direction) {
					case DashingDirection.Bottom: case DashingDirection.BottomLeft: case DashingDirection.BottomRight: yFactor = +1; break;
					case DashingDirection.Top:    case DashingDirection.TopLeft:    case DashingDirection.TopRight:    yFactor = -1; break;
				}
			}

			public DashingDirection GetDefaultDashingDirection(bool facingLeft) {
				return (facingLeft) ? DashingDirection.Left : DashingDirection.Right;
			}

			public static DashingDirection GetDashingDirection(DashingDirection _default) {
				bool leftInput   = GV.PeripheralIO.currentControlState.Left  || GV.PeripheralIO.GamePadThumstickPointingLeft(false);
				bool bottomInput = GV.PeripheralIO.currentControlState.Down  || GV.PeripheralIO.GamePadThumstickPointingDown(false);
				bool rightInput  = GV.PeripheralIO.currentControlState.Right || GV.PeripheralIO.GamePadThumstickPointingRight(false);
				bool topInput    = GV.PeripheralIO.currentControlState.Up    || GV.PeripheralIO.GamePadThumstickPointingUp(false);

				bool topLeftInput     = leftInput && topInput;
				bool topRightInput    = rightInput && topInput;
				bool bottomLeftInput  = leftInput && bottomInput;
				bool bottomRightInput = rightInput && bottomInput;

				if      (topRightInput)    return DashingDirection.TopRight;
				else if (topLeftInput)     return DashingDirection.TopLeft;
				else if (bottomRightInput) return DashingDirection.BottomRight;
				else if (bottomLeftInput)  return DashingDirection.BottomLeft;
				else if (leftInput)        return DashingDirection.Left;
				else if (rightInput)       return DashingDirection.Right;
				else if (topInput)         return DashingDirection.Top;
				else if (bottomInput)      return DashingDirection.Bottom;
				else                       return _default; // Facing direction
			}

			private Vector2 ConvertVelocityMagnitudeToVector(float velocity) {
				return (new Vector2(xFactor, yFactor)) * (new Vector2(velocity)) * sinCosRatio;
			}

			public bool Update() {
				if (!dashOverlayCompleted) return true; // Still dashing, wait till completion

				float deltaVelocity = dashingAcceleration * player.elapsedTime; // Increment dashing acceleration
				timeDashing -= player.elapsedTime; // Decrement time dashing in preperation to stop dashing forward
				dashingVelocity += ConvertVelocityMagnitudeToVector(deltaVelocity); // Get new velocity

				if (timeDashing < 0 || sinCosRatio == Vector2.Zero) { return false; } else {
					player.OffsetSpritePosition(dashingVelocity * player.elapsedTime);

					#region CollisionHandlers
					if (player.HasIntersectedTypes(GV.MonoGameImplement.BlockTypes)) {
						switch (dashingDirection) {
							case DashingDirection.Left:   FixHorizontal(false); break; // Left
							case DashingDirection.Right:  FixHorizontal(true);  break; // Right
							case DashingDirection.Bottom: FixVertical(false);   break; // Bottom
							case DashingDirection.Top:    FixVertical(true);    break; // Top

							#region Complicated
							case DashingDirection.TopLeft: // TopLeft
								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Left"))
									FixHorizontal(false);

								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Top"))
									FixVertical(true);

								break; // Before you judge me, note contents of method return may change after first boundary fix
 							case DashingDirection.TopRight: // TopRight
								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Right"))
									FixHorizontal(true);

								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Top"))
									FixVertical(true);

								break;
							case DashingDirection.BottomLeft: // BottomLeft
								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Left"))
									FixHorizontal(false);

								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Bottom"))
									FixVertical(false);

								break;
							case DashingDirection.BottomRight: // BottomRight
								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Right"))
									FixHorizontal(true);

								if (player.GetIntersectedBoundaryLabels(GV.MonoGameImplement.BlockTypes).Contains("Bottom"))
									FixVertical(false);

								break;
							#endregion
						}
					}
					#endregion

					return true; // Dashing is still happening, prevent deletion
				}
			}

			private void FixVertical(bool upward) {
				if (upward) player.VerticalUpwardBoundaryFix();
				else        player.VerticalDownwardBoundaryFix();

				dashingVelocity.Y = sinCosRatio.Y = 0;
			}

			private void FixHorizontal(bool right) {
				if (right) player.HorizontalMovingRightBoundaryFix();
				else       player.HorizontalMovingLeftBoundaryFix();

				dashingVelocity.X = sinCosRatio.X = 0;
			}

			private Vector2 sinCosRatio = new Vector2(
				(float)Math.Sin(GV.Variables.degrees45ToRadians),
				(float)Math.Cos(GV.Variables.degrees45ToRadians)
			);

			private bool dashOverlayCompleted = false;
			private float timeDashing; // The time the player has remaining to dash, when 0 dashing stops
			public Vector2 dashingVelocity; // The veolocity the player is dashing at a given point in time
			private DashingDirection dashingDirection; // Direction for dash args to point and then move towards
			private static readonly float defaultDashingVelocity = 75f, dashingAcceleration = 300f, defaultDashingTime = 1.2f;
			private int xFactor = 0, yFactor = 0; // Direction in vertical or horizontal plane for player to travel in
			private Player player; // Player reference
		}
	}
}
