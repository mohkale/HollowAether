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

		public static class DrawMethods {
			public static void DrawBoundary(IBoundaryContainer boundary, Color? color = null) {
				color = color.HasValue ? color.Value : Color.White;

				foreach (IBoundary cBoundary in boundary) {
					if (cBoundary is IBRectangle) {
						DrawRectangleFrame((IBRectangle)cBoundary, color);
					} else if (cBoundary is IBTriangle) {
						DrawTriangleFrame((IBTriangle)cBoundary, color);
					} else if (cBoundary is IBCircle) {
						((IBCircle)cBoundary).Draw(color.Value);
					} else if (cBoundary is IBRotatedRectangle) {
						((IBRotatedRectangle)cBoundary).Draw(color.Value);
					}
				}
			}

			public static void DrawTriangleFrame(IBTriangle triangle, Color? color = null, int lineThickenes = 1) {
				triangle.Draw(color, lineThickenes); // Taken care of in class
			}

			public static void DrawRectangleFrame(IBRectangle rectangle, Color? color = null, int lineThickness = 1) {
				foreach (Line line in Convert.RectangleTo4Lines(rectangle)) {
					DrawLine(line.pointA, line.pointB, color, lineThickness);
				}
			}

			public static void DrawRectangleFrame(IBRotatedRectangle rectangle, Color? color=null, int lineThickness = 1) {
				foreach (Line line in Convert.RotatedRectangleTo4Lines(rectangle)) {
					DrawLine(line.pointA, line.pointB, color, lineThickness);
				}
			}

			public static void DrawLine(Vector2 start, Vector2 end, Color? _color = null, int thickness = 1) {
				//GlobalVars.InitializeSpriteBatch(); // Taken care of by GameWindow

				Vector2 edge = end - start; Color defaultC = Color.White;
				float angle = (float)Math.Atan2(edge.Y, edge.X);
				Rectangle rect = new Rectangle((int)start.X, (int)start.Y, (int)edge.Length(), thickness);

				MonoGameImplement.SpriteBatch.Draw(
					texture: GlobalVars.MonoGameImplement.textures["debugFrame"], destinationRectangle: rect,
					color: (_color.HasValue) ? _color.Value : defaultC,
					rotation: angle, scale: new Vector2(0, 0), layerDepth: 0.6f
				);

				//GlobalVars.spriteBatch.End();
			}
		}
	}
}
