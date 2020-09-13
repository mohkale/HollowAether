using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib.GAssets {
	/*public class PushArgs {
		public PushArgs(Vector2 newPos, Vector2 currentPos, float over, bool skipCheck = true) {
			if (!skipCheck && PushValid(newPos, currentPos)) // Invalid push
				throw new HollowAetherException($"One vector value must match");

			pushingHorizontally = currentPos.X != newPos.X; // pushingVertically = !currentPos.Y != newPos.Y

			float maxDisplacement = (pushingHorizontally) ? currentPos.X - newPos.X : currentPos.Y - newPos.Y;

			pushPosition = newPos; // Store variables needed to push sprite in desired direction 
			pushAcceleration = GV.Physics.CalculateGravity(maxDisplacement, over);
			pushVelocity = GV.Physics.GetJumpVelocity(pushAcceleration, maxDisplacement);

			if (pushAcceleration < 0 && pushVelocity < 0) pushVelocity = Math.Abs(pushVelocity);
			else if (pushAcceleration > 0 && pushVelocity > 0) pushVelocity = -pushVelocity;
			// If velocity and acceleration act in same direction, sprite will never stop travelling

			pushingDirection = (pushVelocity < 0) ? -1 : +1; // Determine which direction init displacement is
			PushCompleted = UponCompletion; // Default event handler needed for initialisation of push args
			SpriteOffset  = (s, v) => { s.Position += v; }; // Event needs at least one handler
			SpriteSet     = (s, v, o) => { s.Position = v; }; // Event needs at least one handler
		}

		private static void UponCompletion(IPushable _object, PushArgs self) { self.pushVelocity = 0; }

		public static bool PushValid(Vector2 newPos, Vector2 currentPos) {
			return currentPos != newPos && (currentPos.X == newPos.X ^ currentPos.Y == newPos.Y);
		}

		public bool Update(IPushable self, float elapsedTime) {
			pushVelocity = (float)Math.Round(pushVelocity + (pushAcceleration * elapsedTime), 3);

			float displacement = (float)Math.Round(pushVelocity * elapsedTime, 5); // Store locally

			SpriteOffset(self, new Vector2(
				(pushingHorizontally) ? displacement : 0,
				(pushingHorizontally) ? 0 : displacement
			));
			
			if (pushVelocity == 0 || pushingDirection < 0 && pushVelocity > 0 || pushingDirection > 0 && pushVelocity < 0) {
				// If velocity has changed direction then Apex height reached
				SpriteSet(self, pushPosition, self.Position); PushCompleted(self, this);
				return false; // Return false to allow caller to delete push args
			}

			return true; // Push still valid/continuing thus return true
		}

		private float pushVelocity, pushAcceleration;
		private Vector2 pushPosition;
		private bool pushingHorizontally;
		private int pushingDirection;

		public event Action<IPushable, PushArgs> PushCompleted;
		public event Action<IPushable, Vector2>  SpriteOffset;
		public event Action<IPushable, Vector2, Vector2>  SpriteSet;
	}*/

	public class PushArgs {
		public PushArgs(Vector2 newPos, Vector2 currentPos, float over, bool skipCheck = true) {
			if (!skipCheck && PushValid(newPos, currentPos)) // Invalid push
				throw new HollowAetherException($"One vector value must match");

			pushTarget = newPos; // Store final position sprite should've reached by the end
			Vector2 positionDifference = currentPos - newPos; // Determine Width Height Travel

			float travelLength = (float)Math.Sqrt(Math.Pow(positionDifference.X, 2) + Math.Pow(positionDifference.Y, 2));

			double theta        = Math.Atan2(positionDifference.Y, positionDifference.X);
			Vector2 sinCosRatio = new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

			sinCosRatio.X = (float)Math.Round(sinCosRatio.X, 5); // In case of rounding error
			sinCosRatio.Y = (float)Math.Round(sinCosRatio.Y, 5); // In case of rounding error

			float scalarPushAcceleration = GV.Physics.CalculateGravity(travelLength, over); // acceleration
			float scalarPushVelocity     = GV.Physics.GetJumpVelocity(scalarPushAcceleration, travelLength);

			pushAcceleration = sinCosRatio * scalarPushAcceleration; // Get acceleration with direction
			pushVelocity	 = sinCosRatio * scalarPushVelocity; // Get velocity with direction included

			pushVelocity.X = ((pushAcceleration.X < 0 && pushVelocity.X < 0) || (pushAcceleration.X > 0 && pushVelocity.X > 0)) ? -pushVelocity.X : pushVelocity.X;
			pushVelocity.Y = ((pushAcceleration.Y < 0 && pushVelocity.Y < 0) || (pushAcceleration.Y > 0 && pushVelocity.Y > 0)) ? -pushVelocity.Y : pushVelocity.Y;

			pushingDirection = new Vector2(
				(int)GV.BasicMath.Clamp<float>(pushVelocity.X, -1, 1),
				(int)GV.BasicMath.Clamp<float>(pushVelocity.Y, -1, 1)
			);
			
			PushCompleted = UponCompletion; // Default event handler needed for initialisation of push args
			SpriteOffset  = (s, v)    => { s.Position += v; }; // Event needs at least one handler
			SpriteSet     = (s, v, o) => { s.Position = v;  }; // Event needs at least one handler
		}

		public static bool PushValid(Vector2 newPos, Vector2 currentPos) {
			return currentPos != newPos;
		}

		private void UponCompletion(IPushable _object, PushArgs self) { pushVelocity = Vector2.Zero; }

		public bool Update(IPushable self, float elapsedTime) {
			pushVelocity += pushAcceleration * elapsedTime;

			pushVelocity.X = (float)Math.Round(pushVelocity.X, 3);
			pushVelocity.Y = (float)Math.Round(pushVelocity.Y, 3);

			Vector2 displacement = pushVelocity * elapsedTime;

			displacement.X = (float)Math.Round(displacement.X, 5);
			displacement.Y = (float)Math.Round(displacement.Y, 5);

			SpriteOffset(self, displacement);

			bool horizontallyChanged = !NotPushedHorizontally && DirectionChanged(pushVelocity.X, pushingDirection.X);
			bool verticallyChanged   = !NotPushedVertically   && DirectionChanged(pushVelocity.Y, pushingDirection.Y);

			if (horizontallyChanged) { pushingDirection.X = 0; pushVelocity.X = 0; pushAcceleration.X = 0; }
			if (verticallyChanged)   { pushingDirection.Y = 0; pushVelocity.Y = 0; pushAcceleration.Y = 0; }

			if (pushVelocity == Vector2.Zero) {
				SpriteSet(self, pushTarget, self.Position); PushCompleted(self, this);
				return false; // Return false to allow caller to delete push args
			} else return true; // Push Not yet finished
		}

		private static bool DirectionChanged(float velocity, float thrownDirection) {
			return (velocity > 0 && thrownDirection < 0) || (velocity < 0 && thrownDirection > 0);
		}

		private bool NotPushedHorizontally { get { return pushingDirection.X == 0; } }

		private bool NotPushedVertically { get { return pushingDirection.Y == 0; } }

		private Vector2 pushTarget, pushVelocity, pushAcceleration, pushingDirection;

		public event Action<IPushable, PushArgs>         PushCompleted;
		public event Action<IPushable, Vector2>          SpriteOffset;
		public event Action<IPushable, Vector2, Vector2> SpriteSet;
	}
}
