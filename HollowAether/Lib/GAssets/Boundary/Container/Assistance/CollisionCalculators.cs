using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.Boundary {
	public static class CollisionCalculators {
		public delegate IntersectingPositions CollisionCalculator(IBoundary self, IBoundary target);

		public static IntersectingPositions CalculateCollision(IBoundary A, IBoundary B) {
			Func<IBoundary, String> BoundaryToString = (X) => {
				if (X is IBRectangle)     return "Rectangle";
				else if (X is IBTriangle) return "Triangle";
				else if (X is IBCircle)   return "Circle";
				else                      return String.Empty;
			};

			switch (BoundaryToString(A)) {
				case "Rectangle":
					#region RectangleCalcBody
					switch (BoundaryToString(B)) {
						case "Rectangle":
							return CollisionCalculators.RectangleToRectangle(A, B);
						case "Triangle":
							return RectangleToTriangle(A, B);
						case "Circle":
						default: return IntersectingPositions.Empty;
					}
				#endregion
				case "Triangle":
					#region TriangleCalcBody
					switch (BoundaryToString(B)) {
						case "Rectangle":
							return TriangleToRectangle(A, B);
						case "Triangle":
							return TriangleToTriangle(A, B);
						case "Circle":
						default: return IntersectingPositions.Empty;
					}
				#endregion
				case "Circle":
					return IntersectingPositions.Empty;
				default: return null;
			}
		}

		static CollisionCalculator RectangleToRectangle = (self, target) => {
			IBRectangle IBself = (IBRectangle)self, IBtarget = (IBRectangle)target;

			return new IntersectingPositions() {
				{ Direction.Top, IBtarget.Top }, { Direction.Bottom, IBtarget.Bottom },
				{ Direction.Left, IBtarget.Left }, { Direction.Right, IBtarget.Right }
			};
		};

		static CollisionCalculator RectangleToTriangle = (self, target) => {
			IBRectangle IBself = (IBRectangle)self; IBTriangle IBtarget = (IBTriangle)target;

			List<Vector2> verticalIntersectionPoints = new List<Vector2>(4), horizontalIntersectionPoints = new List<Vector2>(4);

			foreach (Line X in GV.Convert.TriangleTo3Lines(IBtarget)) {
				foreach (Line Y in GV.Convert.RectangleTo4Lines(IBself)) {
					if (X.Intersects(Y)) {
						Vector2? intersectionPoint = X.GetIntersectionPoint(Y);

						if (!intersectionPoint.HasValue || float.IsNaN(intersectionPoint.Value.X) || float.IsNaN(intersectionPoint.Value.Y))
							continue; // No valid intersection value has been found, therefore skip addition to container values 

						if (Y.type == Line.LineType.Normal) {
							horizontalIntersectionPoints.Add(intersectionPoint.Value);
						} else { // Y.type == Line.LineType.XEquals
							verticalIntersectionPoints.Add(intersectionPoint.Value);
						}
					}
				}
			}

			return new IntersectingPositions() {
				{ Direction.Top,    (int)((from X in verticalIntersectionPoints select X.Y).Aggregate(Math.Min)) },
				{ Direction.Bottom, (int)((from X in verticalIntersectionPoints select X.Y).Aggregate(Math.Max))  },
				{ Direction.Left,   (int)((from X in horizontalIntersectionPoints select X.X).Aggregate(Math.Min)) },
				{ Direction.Right,  (int)((from X in horizontalIntersectionPoints select X.X).Aggregate(Math.Max)) }
			};
		};

		static CollisionCalculator TriangleToRectangle = (self, target) => {
			IBTriangle IBself = (IBTriangle)self; IBRectangle IBtarget = (IBRectangle)target;

			return IntersectingPositions.Empty;
		};

		static CollisionCalculator TriangleToTriangle = (self, target) => {
			IBTriangle IBself = (IBTriangle)self; IBTriangle IBtarget = (IBTriangle)target;

			return IntersectingPositions.Empty;
		};
	}
}
