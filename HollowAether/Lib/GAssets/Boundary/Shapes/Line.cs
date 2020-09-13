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

namespace HollowAether.Lib {
	public struct Line {
		public enum LineType { Normal, XEquals } // Type of line this line can be. Normal is Y equals

		/// <summary>Constructs angled line between two positions</summary>
		/// <param name="P1X">X position of point 1</param> <param name="P1Y">Y position of point 1</param>
		/// <param name="P2X">X position of point 2</param> <param name="P2Y">Y position of point 2</param>
		public Line(float P1X, float P1Y, float P2X, float P2Y) : this(new Vector2(P1X, P1Y), new Vector2(P2X, P2Y)) { }

		/// <summary>Constructs angled line between two positions</summary>
		/// <param name="P1">Point 1</param> <param name="P2">Point 2</param>
		public Line(Vector2 P1, Vector2 P2) {
			if (P1.X == P2.X && P1.Y == P2.Y) {
				//Console.WriteLine("Warning: Line of length 0 created");
			}

			pointA = P1; pointB = P2; // Store Initial and Final Points
			gradient = CalculateGradient(P1, P2);
			yIntercept = CalculateYIntercept(P1, P2);
		}

		/// <summary>Calculates gradient between two lines</summary>
		/// <param name="pointA">First point</param> <param name="pointB">Second Point</param>
		/// <returns>Gradient of line between 2 points. Is infinity when equation is X equals</returns>
		public static float CalculateGradient(Vector2 pointA, Vector2 pointB) {
			return (pointA.Y - pointB.Y) / (pointA.X - pointB.X);
		}

		/// <summary>Calculate Y Point where line crosses axis</summary>
		/// <param name="pointA">First Point</param> <param name="pointB">Second Point</param>
		/// <returns>Point where line crosses y axis</returns>
		public static float CalculateYIntercept(Vector2 pointA, Vector2 pointB) {
			return pointA.Y - (CalculateGradient(pointA, pointB) * pointA.X); // C = Y - Mx
		}

		public override string ToString() {
			if (type == LineType.Normal)
				return $"Y = {gradient}x + {yIntercept} : {pointA} {pointB}";
			else
				return $"X = {pointA.X} : {pointA} {pointB}";
		}

		/// <summary>Check if line has collided with a given vector</summary>
		/// <param name="vect">Vector to check intersection/collision with</param>
		/// <param name="useBoundaries">Check wether intersection is within two argument points</param>
		/// <returns>Boolean indicating succesful collisions</returns>
		public bool Intersects(Vector2 vect, bool useBoundaries = true) {
			if (useBoundaries && vect.X > maxPointX && vect.X < minPointX && vect.Y > maxPointY && vect.Y < minPointY) {
				return false; // return Vector outside bounds of current points of line;
			} else if (this.type == LineType.Normal) {
				return vect.Y == (gradient * vect.X) + yIntercept; // return comparitive check
			} else { // if (this.type == LineType.XEquals)
				return vect.X == this.pointA.X; // if x points match then collision has occured
			}
		}

		/// <summary>Checks wether two lines intersect</summary>
		/// <param name="A">First line</param> <param name="B">Second line</param>
		/// <returns>Boolean indicating collision</returns>
		public static bool Intersects(Line A, Line B) {
			if (A.type == LineType.Normal && B.type == LineType.Normal) {
				return (GetIntersectionPoint(A, B).HasValue);
			} else if (A.type == B.type) { // both LineType.XEquals
				return A.pointA.X == B.pointA.X; // X values match
			} else { // One is LineType.XEquals & one is LineType.Normal
				if (A.type == LineType.Normal) { // B.type = LineType.XEquals
					return true; // Will always intersect at a given x value
				} else { // B.type = LineType.Normal & A.type = LineType.XEquals
					return true; // Will always intersect at a given x value
				}
			}
		}

		/// <summary>Checks whether this line intersects a given line </summary>
		/// <param name="line">Line to check intersection with</param>
		/// <returns>Boolean indicating collision</returns>
		public bool Intersects(Line line) { return Intersects(this, line); }

		public static Vector2? GetIntersectionPoint(Line A, Line B) {
			if (A.gradient == B.gradient) return null; // same gradient hence never or infinite collide

			if (A.type == LineType.Normal && B.type == LineType.Normal) {
				float X = (B.yIntercept - A.yIntercept) / (A.gradient - B.gradient);
				return new Vector2(X, A.GetYPoint(X)); // return intersection point
			} else if (A.type == B.type) {// both LineType.XEquals
				return null; // no collision
			} else {
				if (A.type == LineType.Normal) { // B.type = LineType.XEquals
					return new Vector2(B.pointA.X, A.GetYPoint(B.pointA.X));
				} else { // B.type = LineType.Normal & A.type = LineType.XEquals
					return new Vector2(A.pointA.X, B.GetYPoint(A.pointA.X));
				}
			}
		}

		/// <summary>Gets intersecting point between this line and another line</summary>
		/// <param name="A">Line to get collision with</param>
		/// <returns>Nullable vector indicating collision</returns>
		public Vector2? GetIntersectionPoint(Line A) {
			return GetIntersectionPoint(this, A);
		}

		/// <summary>Y point on a line with a given X value</summary>
		/// <returns>Y value</returns>
		public float GetYPoint(float X) {
			if (this.type == LineType.Normal)
				return (gradient * X) + yIntercept;
			else
				return float.PositiveInfinity;
		}


		/// <summary>X point on a line with a given Y value</summary>
		/// <returns>X value</returns>
		public float GetXPoint(float Y) {
			if (this.type == LineType.Normal)
				return (Y - yIntercept) / gradient;
			else
				return pointA.X;
		}

		public Vector2 pointA, pointB;

		public float gradient, yIntercept; // M

		public float maxPointY { get { return (pointA.Y > pointB.Y) ? pointA.Y : pointB.Y; } }
		public float maxPointX { get { return (pointA.X > pointB.X) ? pointA.X : pointB.X; } }
		public float minPointY { get { return (pointA.Y < pointB.Y) ? pointA.Y : pointB.Y; } }
		public float minPointX { get { return (pointA.X < pointB.X) ? pointA.X : pointB.X; } }
		public float Length { get { return (float)Math.Sqrt(Math.Pow(pointA.X - pointB.X, 2) + Math.Pow(pointA.Y - pointB.Y, 2)); } }
		public LineType type { get { return (float.IsInfinity(gradient) || float.IsInfinity(yIntercept)) ? LineType.XEquals : LineType.Normal; } }
	}
}
