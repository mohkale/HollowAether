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

		public static class ImageManipulation {
			/// <summary>Crops a bitmap image instance using a rectangle</summary>
			/// <param name="img">Bitmap instance to crop using a rectangle</param>
			/// <param name="cropArea">Rectangle representing area in image</param>
			/// <returns>Cropped Image</returns>
			public static System.Drawing.Image CropImage(System.Drawing.Bitmap img, System.Drawing.Rectangle cropArea) {
				return img.Clone(cropArea, img.PixelFormat);
			}

			/// <summary>Crops an Image instance using a rectangle</summary>
			/// <param name="img">Image instance to crop using a rectangle</param>
			/// <param name="cropArea">Rectangle representing area in image</param>
			/// <returns>Cropped Image</returns>
			public static System.Drawing.Image CropImage(System.Drawing.Image img, System.Drawing.Rectangle cropArea) {
				return CropImage(new System.Drawing.Bitmap(img), cropArea);
			}

			public static Texture2D CreateCircleTexture(int frameRadius, Color circleColor) {
				Texture2D circleFrame = new Texture2D(GlobalVars.hollowAether.GraphicsDevice, frameRadius, frameRadius);

				Color[] circleColorData = new Color[(int)Math.Pow(frameRadius, 2)]; // 1 Dimensional texture data

				float diam = frameRadius / 2f, diamSquared = (float)Math.Pow(diam, 2); // Used for checking
				Func<float, bool> Check = (posLengthSquared) => (posLengthSquared <= diamSquared);

				foreach (int X in Enumerable.Range(0, frameRadius)) {
					foreach (int Y in Enumerable.Range(0, frameRadius)) {
						Vector2 pos = new Vector2(X - diam, Y - diam); // Position in texture
						Color color = Check(pos.LengthSquared()) ? circleColor : Color.Transparent;

						circleColorData[X * frameRadius + Y] = color; // Set Desired Color
					}
				}

				circleFrame.SetData<Color>(circleColorData);
				return circleFrame; // return circle texture
			}
		}
	}
}
