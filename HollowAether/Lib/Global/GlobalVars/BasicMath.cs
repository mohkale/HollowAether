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

		public static class BasicMath {
			public static T Clamp<T>(T value, T minimum, T maximum) where T : IComparable<T> {
				if (value.CompareTo(minimum) < 0) return minimum; else if (value.CompareTo(maximum) > 0) return maximum; else return value;
			}

			public static int RoundToNearestMultiple(int value, int factor, bool ignoreZero = true) {
				int factorCount = (int)Math.Round(value / (double)factor, MidpointRounding.ToEven);
				return (ignoreZero && factorCount == 0) ? (factorCount + 1) * factor : factorCount * factor;
			}

			public static int RoundDownToNearestMultiple(int value, int factor, bool ignoreZero = true) {
				int factorCount = (int)Math.Floor(value / (double)factor); // Number of times factors appear
				return (ignoreZero && factorCount == 0) ? (factorCount + 1) * factor : factorCount * factor;
			}

			public static int RoundDownToNearestMultiple(float value, float factor, bool ignoreZero = true) {
				int factorCount = (int)Math.Floor(value / factor); // Number of times factors appear
				return (int)Math.Round((ignoreZero && factorCount == 0) ? (factorCount + 1) * factor : factorCount * factor);
			}

			public static System.Drawing.Rectangle ScaleRectangle(System.Drawing.Rectangle rect, float XScale, float YScale) {
				int XPos = (int)(rect.X * XScale), YPos = (int)(rect.Y * YScale);
				int width = (int)(rect.Width * XScale), height = (int)(rect.Height * YScale);
				return new System.Drawing.Rectangle(XPos, YPos, width, height);
			}

			public static System.Drawing.Rectangle ScaleRectangle(System.Drawing.Rectangle rect, float scale) {
				return ScaleRectangle(rect, scale, scale);
			}

			public static System.Drawing.Rectangle ScaleRectangle(System.Drawing.Rectangle rect, int XScale, int YScale) {
				return ScaleRectangle(rect, (float)XScale, (float)YScale);
			}

			public static System.Drawing.Rectangle ScaleRectangle(System.Drawing.Rectangle rect, int scale) {
				return ScaleRectangle(rect, (float)scale);
			}

			public static float DegreesToRadians(float degrees) {
				return RadDegRatio * degrees;
			}

			public static float RadiansToDegrees(float radians) {
				return radians * DegRadRatio;
			}

			/*public static double NormaliseRadianAngle(float theta) {
				return theta - (2 * Math.PI) * Math.Floor((theta + Math.PI) / (2 * Math.PI));
			}*/

			public static double NormaliseRadianAngle(double theta) {
				while (theta < 0) theta += 2 * Math.PI;

				return theta;
			}

			public static double RecursiveNormaliseRadianAngle(double theta) {
				return (theta > 0) ? theta : RecursiveNormaliseRadianAngle(theta + (2 * Math.PI));
			}

			private static readonly float RadDegRatio = (float)(Math.PI / 180), DegRadRatio = (float)(180 / Math.PI);
		}
	}
}
