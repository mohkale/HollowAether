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
#endregion

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets {
	/// <summary> Custom Built Triangle Class, for use in TriBBox Class </summary>
	public partial class IBTriangle : IBoundary {
		/// <summary> Class containing general triangle intersection Methods, forming backbone
		/// of class; takes advantage of mathematical Check Sides with Dot Product algorithm </summary>
		/// <remarks>Note that the bulk of the class is allocated to vector intersection checks</remarks>
		public class TriangleBackbone {
			public TriangleBackbone(Vector2 A, Vector2 B, Vector2 C) {
				ventrices = new Vector2[] { A, B, C }; // Store vector points of triangle
				Container = GlobalVars.Physics.GetContainerRectFromVectors(A, B, C);
			}

			private float Side(int VectorIndex1, int VectorIndex2, Vector2 point) {
				Vector2 A = this[VectorIndex1], B = this[VectorIndex2]; // Desired vectors for size calculation

				return (B.Y - A.Y) * (point.X - A.X) + (-B.X + A.X) * (point.Y - A.Y); // Return dot product
			}
			
			private float SquarePointToSegmentDistance(int VectorIndex1, int VectorIndex2, Vector2 point) {
				Func<Vector2, Vector2, Vector2, float> CalculateSquareLength = (v1, v2, v3) => {
					return (v1.X - v3.X) * (v2.X - v3.X) + (v1.Y - v3.Y) * (v2.Y - v3.Y);
				}; // Lambda Function to simplify SquareLength Calculations

				Vector2 A = this[VectorIndex1], B = this[VectorIndex2]; // Store vector instances used by method

				float A_B_SquareLength = CalculateSquareLength(B, B, A); // Square Length from Point A to Point B

				float dotProduct = CalculateSquareLength(point, B, A) / A_B_SquareLength;

				if		(dotProduct < 0) return CalculateSquareLength(point, point, A);
				else if (dotProduct > 1) return CalculateSquareLength(point, point, B);
				else return CalculateSquareLength(A, A, point) - dotProduct * dotProduct * A_B_SquareLength;
			}

			/// <summary> Determines if given point lies within a Triangle </summary>
			/// <param name="intVect">Intersection Vector</param>
			/// <returns>Boolean indicating intersection</returns>
			public bool NaivePointIntersectionCheck(Vector2 intVect) {
				// Stores whether this.side >= 0 for index pairs (0, 1) && (1, 2) && (2, 0) in respective order
				var checks = (from X in Enumerable.Range(0, 3) select Side(X, (X + 1 < 3) ? X + 1 : 0, intVect) >= 0).ToArray();

				return checks.Aggregate((a, b) => a && b); // returns true only if all 3 sides evaluate to >= 0. Else false
			}
			
			public bool TriBoundaryBoxIntersectionCheck(Vector2 point) { return Container.Contains(point); }

			public bool Intersects(Vector2 point) { // Compound intersection check with Vector point
				if (!TriBoundaryBoxIntersectionCheck(point)) return false; /* Prevents Pointless CPU Waste */ else {
					return NaivePointIntersectionCheck(point) // If point is whithin triangle
						|| SquarePointToSegmentDistance(0, 1, point) <= EPSILON_SQRD  // Or lies on edge
						|| SquarePointToSegmentDistance(1, 2, point) <= EPSILON_SQRD  // Of any of the
						|| SquarePointToSegmentDistance(2, 0, point) <= EPSILON_SQRD; // Triangles Lines
				}
			}
			
			/// <param name="A1">Line 01 Start Point</param> <param name="A2">Line 01 End Point</param>
			/// <param name="B1">Line 02 Start Point</param> <param name="B2">Line 02 End Point</param>
			/// <remarks>from http://stackoverflow.com/questions/14480124/how-do-i-detect-triangle-and-rectangle-intersection </remarks>
			public bool LineIntersects(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2) {
				Func<Vector2, Vector2, float, float> DotPerp = (vect1, vect2, denom) => {
					return ((vect1.X * vect2.Y) - (vect1.Y * vect2.X)) / denom;
				};

				Vector2[] BCD = new Vector2[] { A2 - A1, B1 - A1, B2 - B1 };

				float BDPerp = DotPerp(BCD[0], BCD[2], 1); // Whether vectors are perpendicular

				if (BDPerp == 0) { return false; } /* Parallel hence INFINITE Intersection points */ else {
					foreach (int X in Enumerable.Range(1, 2)) {
						float TU = DotPerp(BCD[(X + 1 < 3) ? X : 1], BCD[(X + 1 < 3) ? X + 1 : 0], BDPerp);
						if (TU < 0 || TU > 1) { return false; } // No intersection between lines has occured
					};

					return true; // If all checks have succesfully finished, then collision has occured
				}
			}

			public bool LineIntersects(Line A, Line B) { return LineIntersects(A.pointA, A.pointB, B.pointA, B.pointB); }

			public void Offset(Vector2 offset) {
				foreach (int X in Enumerable.Range(0, ventrices.Length))
					ventrices[X] += offset; // Push each vertex

				Container.Offset(offset); // Offset container rectangle
			}

			public Rectangle Container { get; private set; }

			public Vector2 this[int index] { get { return ventrices[index]; } set { ventrices[index] = value; } }

			public Vector2[] ventrices; // Get all vertxes stored by triangle

			public static float EPSILON = 0.001f, EPSILON_SQRD = EPSILON * EPSILON;
		}

		/// <summary>Paramterless Constructor, must call Initialize</summary>
		protected IBTriangle() { }
		
		public IBTriangle(Vector2 A, Vector2 B, Vector2 C) { this.Initialize(A, B, C); }

		/// <summary> Constructor Taking Various Integers To Form Vectorial Points for Triangle </summary>
		/// <param name="X1">X Value of Point A in Triangle</param> <param name="Y1">Y Value of Point A in Triangle</param>
		/// <param name="X2">X Value of Point B in Triangle</param> <param name="Y2">Y Value of Point B in Triangle</param>
		/// <param name="X3">X Value of Point C in Triangle</param> <param name="Y3">Y Value of Point C in Triangle</param>
		public IBTriangle(int X1, int Y1, int X2, int Y2, int X3, int Y3) 
			: this(new Vector2(X1, Y1), new Vector2(X2, Y2), new Vector2(X3, Y3)) { }
		
		protected void Initialize(Vector2 A, Vector2 B, Vector2 C) {
			backBone = new TriangleBackbone(A, B, C);
		}
		
		public void Offset(Vector2 offset) { backBone.Offset(offset); }

		public void Offset(float X = 0, float Y = 0) { Offset(new Vector2(X, Y)); }

		public void Draw(Color? color = null, int lineThickness = 1) {
			foreach (Line line in GV.Convert.TriangleTo3Lines(this)) {
				GV.DrawMethods.DrawLine(line.pointA, line.pointB, color, lineThickness);
			}
		}
		
		public static IBTriangle operator +(IBTriangle tri, Vector2 vect) {
			var sum = new IBTriangle(tri[0], tri[1], tri[2]);
			sum.Offset(vect); return sum;
		}
		
		public static IBTriangle operator +(IBTriangle tri, int value) {
			var sum = new IBTriangle(tri[0], tri[1], tri[2]);
			sum.Offset(new Vector2(value)); return sum;
		}

		public override string ToString() {
			return String.Format("TRIANGLE : A {0} | B {1} | C {2}", this[0], this[1], this[2]);
		}
		
		public Vector2 this[int index] { get { return backBone.ventrices[index]; } }

		public Vector2[] Ventrices { get { return backBone.ventrices; } } // Ventrices
		
		public IBRectangle Container { get { return backBone.Container; } }

		public TriangleBackbone backBone; // TriangleClass Backbone Declaration
	}

	/// <summary>Variation of triangle class which is exclusively right angled</summary>
	public class IBRightAngledTriangle : IBTriangle {
		/// <summary>Enumeration storing various right angled triangle types</summary>
		public enum BlockType {
			/// <summary>Type = ◢</summary>
			A,
			/// <summary>Type = ◣</summary>
			B,
			/// <summary>Type = ◥</summary>
			C,
			/// <summary>Type = ◤</summary>
			D
		}

		/// <summary>Default Right Angled Triangle Constructor</summary>
		/// <param name="position">Top left position of triangle</param>
		/// <param name="width">Width of triangle</param>
		/// <param name="height">Height of triangle</param>
		/// <param name="_type">BlockType of given right angled triangle</param>
		public IBRightAngledTriangle(Vector2 position, int width, int height, BlockType _type) : base() {
			#region AllignmentSet
			switch (_type) {
				case BlockType.A:
				default:
					_rightAllignment = true;
					_topAllignment = false;
					break;
				case BlockType.B:
					_rightAllignment = false;
					_topAllignment = false;
					break;
				case BlockType.C:
					_rightAllignment = true;
					_topAllignment = true;
					break;
				case BlockType.D:
					_rightAllignment = false;
					_topAllignment = true;
					break;
			}
			#endregion
			BuildBoundary(position, width, height);
		}

		/// <summary>Alternative Right Angled Triangle Constructor</summary>
		/// <param name="position">Top left position of triangle</param>
		/// <param name="width">Width of triangle</param>
		/// <param name="height">Height of triangle</param>
		/// <param name="rightAllign">Whether to allign the triangle to the right or not</param>
		/// <param name="topAllign">Whether to allign the triangle to the top or not</param>
		public IBRightAngledTriangle(Vector2 position, int width, int height, bool rightAllign = true, bool topAllign = false) : base() {
			#region AllignmentSet
			_rightAllignment = rightAllign;
			_topAllignment = topAllign;
			#endregion
			BuildBoundary(position, width, height);
		}

		/// <summary>Calculates three vectors needed for given allignment</summary>
		/// <param name="position">Top Left corner of triangle</param>
		/// <param name="width">Width of triangle</param> 
		/// <param name="height">Height of triangle</param>
		private void BuildBoundary(Vector2 position, int width, int height) {
			Vector2 A, B, C; // Vectorial positions of triangle axes

			if (rightAllignment && bottomAllignment) {
				A = position + new Vector2(width, height);
				B = position + new Vector2(0, height);
				C = position + new Vector2(width, 0);

				type = BlockType.A;
			} else {
				A = position; // Set A to top left corner of sprite :)

				if (leftAllignment && topAllignment) {
					B = position + new Vector2(width, 0);
					C = position + new Vector2(0, height);
				} else {
					B = position + new Vector2(width, height);

					if (leftAllignment && bottomAllignment) {
						C = position + new Vector2(0, height);
					} else { // (rightAllignment && topAllignment)
						C = position + new Vector2(width, 0);
					}
				}
			}

			this.Initialize(A, B, C);
		}

		public static BlockType AllignmentsToBlockType(bool rightAllign, bool topAllign) {
			if (rightAllign && !topAllign) {
				return BlockType.A;
			} else if (!rightAllign && !topAllign) {
				return BlockType.B;
			} else if (rightAllign && topAllign) {
				return BlockType.C;
			} else { // !rightAllign && topAllign
				return BlockType.D;
			}
		}

		/// <summary>What type of right angle this is</summary>
		public BlockType type;

		#region PrivateAllignmentVars
		private bool _rightAllignment;
		private bool _topAllignment;
		#endregion

		#region AllignmentAttributes
		/// <summary>Whether triangle is alligned to the right</summary>
		public bool rightAllignment { get { return _rightAllignment; } }
		/// <summary>Whether triangle is alligned to the left</summary>
		public bool leftAllignment { get { return !_rightAllignment; } }
		/// <summary>Wheter triangle is alligned to the top</summary>
		public bool topAllignment { get { return _topAllignment; } }
		/// <summary>Whether triangle is alligned to the bottom</summary>
		public bool bottomAllignment { get { return !_topAllignment; } }
		#endregion
	}
}