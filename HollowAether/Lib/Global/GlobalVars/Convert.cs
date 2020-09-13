using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class Convert {
			public static Vector2[] RectangleTo4Vects(Rectangle rect) {
				return new Vector2[] {
					new Vector2(rect.Left, rect.Top),     // Top Left
					new Vector2(rect.Right, rect.Top),    // Top Right
					new Vector2(rect.Left, rect.Bottom),  // Bottom Left
					new Vector2(rect.Right, rect.Bottom), // Bottom Right
				};
			}

			public static Line[] RectangleTo4Lines(Rectangle rect) {
				Vector2[] rectVects = RectangleTo4Vects(rect);

				return _4VectorsTo4Lines(rectVects[0], rectVects[1], rectVects[2], rectVects[3]);
			}

			public static Line[] RotatedRectangleTo4Lines(IBRotatedRectangle rect) {
				Vector2[] rectVects = (from X in rect.GenerateRotatedPoints() select X).ToArray();

				return _4VectorsTo4Lines(rectVects[0], rectVects[1], rectVects[2], rectVects[3]);
			}

			public static Line[] TriangleTo3Lines(IBTriangle tri) {
				return new Line[] {
					new Line(tri.Ventrices[0], tri.Ventrices[1]),
					new Line(tri.Ventrices[0], tri.Ventrices[2]),
					new Line(tri.Ventrices[1], tri.Ventrices[2]),
				};
			}

			public static Line[] _4VectorsTo4Lines(Vector2 A, Vector2 B, Vector2 C, Vector2 D) {
				return new Line[] { new Line(A, B), new Line(A, C), new Line(B, D), new Line(C, D) };
			}
		}
	}
}
