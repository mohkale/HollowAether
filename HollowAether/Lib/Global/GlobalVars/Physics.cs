#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.Encryption;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
#endregion

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class Physics {
			public static float GetJumpVelocity(float gravity, float maxJumpHeight) {
				return (float)Math.Sqrt(2 * gravity * maxJumpHeight);
			}

			public static float CalculateGravity(float maxJumpHeight, float timeToApex) {
				return 2 * maxJumpHeight / (float)Math.Pow(timeToApex, 2);
			}

			public static float GetEarlyJumpEndVelocity(float currentVelocity, float gravity, float maxJumpHeight, float minJumpHeight) {
				return (float)Math.Sqrt(Math.Pow(currentVelocity, 2) + (2 * gravity * (maxJumpHeight - minJumpHeight)));
			}

			public static Rectangle GetContainerRectFromVectors(params Vector2[] points) {
				float[] xValues = (from V in points select V.X).ToArray(), yValues = (from V in points select V.Y).ToArray();
				float minX = xValues.Min(), maxX = xValues.Max(), minY = yValues.Min(), maxY = yValues.Max(); // Store points

				return new Rectangle((int)Math.Round(minX), (int)Math.Round(minY), (int)Math.Round(maxX - minX), (int)Math.Round(maxY - minY));
			}
		}
	}
}
