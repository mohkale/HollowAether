using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	static class CollisionChecker { // IBoundary Exclusive
		public static bool InterpretCollision(IBoundary A, IBoundary B) {
			if (!A.Container.Intersects(B.Container))
				return false; // If no container collision then return

			#region CrossStitchingLogic
			Func<IBoundary, String> ToTypeString = (boundary) => {
				if (boundary is IBRectangle)			 return "Rectangle";
				else if (boundary is IBTriangle)		 return "Triangle";
				else if (boundary is IBCircle)			 return "Circle";
				else if (boundary is IBRotatedRectangle) return "RRectangle";
				else /* Impossible Return CMD */		 return String.Empty;
			};

			switch (ToTypeString(A)) {
				case "Rectangle":
					#region PrimaryRectangleBody
					switch (ToTypeString(B)) {
						case "Rectangle":
							return RectangleToRectangle((IBRectangle)A, (IBRectangle)B);
						case "Triangle":
							return RectangleToTriangle((IBRectangle)A, (IBTriangle)B);
						case "Circle":
							return RectangleToCircle((IBRectangle)A, (IBCircle)B);
						case "RRectangle":
							return RectangleToRotatedRectangle((IBRectangle)A, (IBRotatedRectangle)B);
						default: return false; // Pretty much impossible, but just in case
					}
					#endregion
				case "Triangle":
					#region PrimaryTriangleBody
					switch (ToTypeString(B)) {
						case "Rectangle":
							return TriangleToRectangle((IBTriangle)A, (IBRectangle)B);
						case "Triangle":
							return TriangleToTriangle((IBTriangle)A, (IBTriangle)B);
						case "Circle":
							return TriangleToCircle((IBTriangle)A, (IBCircle)B);
						case "RRectangle":
							return TriangleToRotatedRectangle((IBTriangle)A, (IBRotatedRectangle)B);
						default: return false; // Pretty much impossible, but just in case
					}
					#endregion
				case "Circle":
					#region PrimaryCircleBody
					switch (ToTypeString(B)) {
						case "Rectangle":
							return CircleToRectangle((IBCircle)A, (IBRectangle)B);
						case "Triangle":
							return CircleToTriangle((IBCircle)A, (IBTriangle)B);
						case "Circle":
							return CircleToCircle((IBCircle)A, (IBCircle)B);
						case "RRectangle":
							return CircleToRotatedRectangle((IBCircle)A, (IBRotatedRectangle)B);
						default: return false; // Pretty much impossible, but just in case
					}
				#endregion
				case "RRectangle":
					#region PrimaryRotatedRectangleBody
					switch (ToTypeString(B)) {
						case "Rectangle":
							return RotatedRectangleToRectangle((IBRotatedRectangle)A, (IBRectangle)B);
						case "Triangle":
							return RotatedRectangleToTriangle((IBRotatedRectangle)A, (IBTriangle)B);
						case "Circle":
							return RotatedRectangleToCircle((IBRotatedRectangle)A, (IBCircle)B);
						case "RRectangle":
							return RotatedRectangleToRotatedRectangle((IBRotatedRectangle)A, (IBRotatedRectangle)B);
						default: return false; // Pretty much impossible, but just in case
					};
					#endregion
				default: return false; // Pretty much impossible, but just in case
			}
			#endregion
		}

		static bool RectangleToRectangle(IBRectangle A, IBRectangle B) { return A.Intersects(B); } // Complete

		static bool RectangleToTriangle(IBRectangle A, IBTriangle B) {
			for (int X = 0; X < 3; X++) { // For primary Vector in Ventrices
				for (int Y = 0; Y < 3; Y++) { // For secondary Vector in Ventrices
					if (B[X] == B[Y]) { continue; } // If Vects Match Skip Forward

					foreach (var RectLine in GV.Convert.RectangleTo4Lines(A)) { // For Each Line In Given Rectangle
						if (B.backBone.LineIntersects(RectLine.pointA, RectLine.pointB, B[X], B[Y]))
							return true; // Rectangle line intersects current triangle instances' line
					} // Return True on first instance of intersection between triangle lines
				} // -- Intersection Check to See If Rect Intersects With Any of the Triangles Lines
			} // ------------- Evaluates to Nothing If Rectangle is Contained Entirely Within Triangle

			foreach (Vector2 vect in GV.Convert.RectangleTo4Vects(A)) { if (B.Contains(vect)) return true; }
			// Checks wether rectangle is contained entirely within the current triangle instance
			return false; // If all of the above methods evaluate to false, return false
		} // complete
		
		static bool RectangleToCircle(IBRectangle A, IBCircle B) {
			float closestX = GlobalVars.BasicMath.Clamp(B.X, A.Left, A.Right);
			float closestY = GlobalVars.BasicMath.Clamp(B.Y, A.Top, A.Bottom);

			float distanceX = B.X - closestX, distanceY = B.Y - closestY; // Distances displacement
			return Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2) < Math.Pow(B.radius, 2);
		}  // Complete

		static bool RectangleToRotatedRectangle(IBRectangle A, IBRotatedRectangle B) {
			return RotatedRectangleToRectangle(B, A);
		} // Complete

		static bool TriangleToRectangle(IBTriangle A, IBRectangle B) {
			return RectangleToTriangle(B, A); // Reverse then call
		} // Complete

		static bool TriangleToTriangle(IBTriangle A, IBTriangle B) {
			for (int X = 0; X < 3; X++) { // For primary Vector in Ventrices
				for (int Y = 0; Y < 3; Y++) { // For secondary Vector in Ventrices
					if (A[X] == A[Y]) { continue; } // When vects match skip forward

					foreach (Line line in GV.Convert.TriangleTo3Lines(B)) {
						if (A.backBone.LineIntersects(line, new Line(A[X], A[Y])))
							return true; // argument tri-line intersects current triangles line
					} // Return True on first instance of intersection between triangle lines
				} // -- Checks to see matching collision between different triangle lines
			} // ---- Evaluates to Nothing If Argument Triangle is Contained Entirely Within This

			foreach (Vector2 vect in B.Ventrices) { if (A.Contains(vect)) return true; }
			// Checks wether ArgTri is contained entirely within the current triangle instance
			return false; // If all of the above methods evaluate to false, return false
		} // Complete
		
		static bool TriangleToCircle(IBTriangle A, IBCircle B) {
			foreach (Vector2 ventrice in A.Ventrices) {
				if (B.Contains(ventrice)) return true;
			}

			foreach (Line line in GV.Convert.TriangleTo3Lines(A)) {
				if (B.Intersects(line)) return true;
			}

			return false;
		} // Complete

		static bool TriangleToRotatedRectangle(IBTriangle A, IBRotatedRectangle B) {
			return RotatedRectangleToTriangle(B, A);
		} // Complete

		static bool CircleToRectangle(IBCircle A, IBRectangle B) {
			return RectangleToCircle(B, A); // Reverse Then Call
		} // Complete

		static bool CircleToTriangle(IBCircle A, IBTriangle B) {
			return TriangleToCircle(B, A);
		} // Complete

		static bool CircleToCircle(IBCircle A, IBCircle B) {
			return (B.origin - A.origin).Length() < (B.radius - A.radius);
		} // Complete

		static bool CircleToRotatedRectangle(IBCircle A, IBRotatedRectangle B) {
			return RotatedRectangleToCircle(B, A);
		} // Complete

		static bool RotatedRectangleToRectangle(IBRotatedRectangle A, IBRectangle B) {
			return A.Intersects((Rectangle)B);
		} // Complete

		static bool RotatedRectangleToTriangle(IBRotatedRectangle A, IBTriangle B) {
			foreach (Line line in GV.Convert.TriangleTo3Lines(B))
				if (A.Intersects(line)) return true;

			return false; // No collision with any lines
		} // Complete

		static bool RotatedRectangleToCircle(IBRotatedRectangle A, IBCircle B) {
			foreach (Line line in GV.Convert.RotatedRectangleTo4Lines(A))
				if (B.Intersects(line)) return true; // Any lines intersect

			// Try detect if circle within rectangle or otherwise

			return false; // No intersection detected
		} // Partially Complete

		static bool RotatedRectangleToRotatedRectangle(IBRotatedRectangle A, IBRotatedRectangle B) {
			return A.Intersects(B);
		} // Complete
	}

	#region CollisionDefinitions
	public partial class IBTriangle {
		/// <summary>Checks for intersection between triangle and another boundary type</summary>
		/// <param name="boundary">Other boundary type to check collision with</param>
		/// <returns>Boolean indicating sucesful collision with boundary</returns>
		public bool Intersects(IBoundary boundary) {
			return CollisionChecker.InterpretCollision(this, boundary);
		}

		/// <summary>Checks for intersection with non boundary type rectangle</summary>
		/// <param name="target">Target rectangle to check for collision with</param>
		/// <returns>Boolean indicating sucesfull collision with rectangle</returns>
		public bool Intersects(Rectangle target) {
			return Intersects((IBoundary)(new IBRectangle(target)));
		}

		/// <summary>Checks for triangle intersection with target</summary>
		/// <param name="target">Target line to check for collision</param>
		/// <returns>Boolean indicating triangular collision</returns>
		public bool Intersects(Line target) {
			foreach (Line line in GV.Convert.TriangleTo3Lines(this)) {
				//if (backBone.LineIntersects(line.pointA, line.pointB, target.pointA, target.pointB, false))
				if (line.Intersects(target)) return true; // Upon first sucesfull line collision return true
			}

			return false; // No collision found
		}

		/// <summary> Checks whether triangle contains a given point </summary>
		/// <param name="point">Point which can be within triangle</param>
		/// <returns>Boolean Representing whether Point Lies Within Triangle</returns>
		public bool Contains(Vector2 point) { return Container.Contains(point.ToPoint()) && backBone.Intersects(point); }
	}

	public partial struct IBRectangle {
		/// <summary>Checks for intersection between rectangle and another boundary type</summary>
		/// <param name="boundary">Other boundary type to check collision with</param>
		/// <returns>Boolean indicating sucesful collision with boundary</returns>
		public bool Intersects(IBoundary boundary) {
			return CollisionChecker.InterpretCollision(this, boundary);
		}

		/// <summary>Checks for intersection with non boundary type rectangle</summary>
		/// <param name="target">Target rectangle to check for collision with</param>
		/// <returns>Boolean indicating sucesfull collision with rectangle</returns>
		public bool Intersects(Rectangle target) {
			return Intersects((IBoundary)(new IBRectangle(target)));
		}

		/// <summary>Checks for rectangular intersection with a given line</summary>
		/// <param name="target">Target line to check for intersecction with</param>
		/// <returns>Boolean indicating sucesfull collision</returns>
		public bool Intersects(Line target) {
			foreach (Line line in GV.Convert.RectangleTo4Lines(this)) {
				if (target.Intersects(line)) return true;
			}

			return false; // If not yet returned then return false
		}
	}

	public partial struct IBCircle  {
		/// <summary>Checks for intersection between circle and another boundary type</summary>
		/// <param name="boundary">Other boundary type to check collision with</param>
		/// <returns>Boolean indicating sucesful collision with boundary</returns>
		public bool Intersects(IBoundary target) {
			return CollisionChecker.InterpretCollision(this, target);
		}

		/// <summary>Checks for intersection with non boundary type rectangle</summary>
		/// <param name="target">Target rectangle to check for collision with</param>
		/// <returns>Boolean indicating sucesfull collision with rectangle</returns>
		public bool Intersects(Rectangle target) {
			return Intersects((IBoundary)(new IBRectangle(target)));
		}

		/// <summary>Checks for intersection between circle and a given line using quadratic approach</summary>
		/// <param name="line">Line to check for collision with</param>
		/// <returns>Boolean indicating sucesfull collision with circle</returns>
		/// <remarks>Method gotten from Stackoverflow Question ID 1073336</remarks>
		public bool Intersects(Line line) {
			Vector2 d = line.pointB - line.pointA, f = line.pointA - origin; // Vectorial Displacement Definition
			float a = Vector2.Dot(d, d), b = 2*Vector2.Dot(f, d), c = Vector2.Dot(f, f) - (float)Math.Pow(radius, 2);
			float discriminant = (float)Math.Pow(b, 2) - (4 * a * c); // B^2 - 4xAxC => Quadratic Discriminant

			if (discriminant < 0) { return false; /* -Ve discriminant has no root */ } else {

				discriminant = (float)Math.Sqrt(discriminant); // Root of discriminant

				float root1 = (-b - discriminant) / (2 * a), root2 = (-b + discriminant) / (2 * a);

				return (root1 >= 0 && root1 <= 1) || (root2 >= 0 && root2 <= 1);
			}
		}

		/// <summary> Checks whether Circle contains a given point </summary>
		/// <param name="point">Point which can be within circle</param>
		/// <returns>Boolean Representing whether Point Lies Within Circle</returns>
		public bool Contains(Vector2 vect) {
			return (vect - origin).Length() <= radius;
		}
	}

	public partial class IBRotatedRectangle {
		public bool Intersects(IBoundary other) {
			return CollisionChecker.InterpretCollision(this, other);
		}

		public bool Intersects(Rectangle rect) { return Intersects(new IBRotatedRectangle(rect, 0.0f)); }

		public bool Intersects(Point point) { return Intersects(point.X, point.Y); }

		public bool Intersects(int X, int Y) { return Intersects(new IBRotatedRectangle(X, Y, 1, 1, 0.0f)); }

		public bool Intersects(IBRotatedRectangle rect) {
			Vector2[] rectAxes = new Vector2[] {
				this.RotatedTopRightPoint - this.RotatedTopLeftPoint,
				this.RotatedTopRightPoint - this.RotatedBottomRightPoint,
				rect.RotatedTopLeftPoint  - rect.RotatedTopRightPoint,
				rect.RotatedTopLeftPoint  - rect.RotatedBottomLeftPoint
			};

			foreach (Vector2 axis in rectAxes) {
				if (!IsAxisCollision(rect, axis))
					return false;
			}

			return true; // Collision has occured
		}

		public bool Intersects(Line line) {
			foreach (Line rectLine in GV.Convert.RotatedRectangleTo4Lines(this))
				if (line.Intersects(rectLine)) return true;

			return false;
		}
	}
	#endregion
}
