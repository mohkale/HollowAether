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

		public static class TextureCreation {
			/// <summary>Builds a monochramatic texture of a given color</summary>
			/// <param name="color">Color of texture</param>
			/// <returns>Newly defined texture matching specifications</returns>
			public static Texture2D GenerateBlankTexture(Color color, int width=1, int height=1) {
				Texture2D texture = new Texture2D(hollowAether.GraphicsDevice, width, height);

				texture.SetData<Color>((from X in Enumerable.Range(0, width * height) select color).ToArray());

				return texture; // Generates and returns texture of a single color to caller.
			}

			/// <summary>Builds a monochramatic texture of a given color determined by RGB valued</summary>
			/// <param name="red">Red Hue</param> <param name="blue">Blue Hue</param>
			/// <param name="green">Green Hue</param> <param name="alpha">Transparency</param>
			/// <returns>Newly defined texture matching specifications</returns>
			public static Texture2D GenerateBlankTexture(float red, float green, float blue, float alpha = 1f, int width=1, int height=1) {
				return GenerateBlankTexture(new Color(red, green, blue, alpha), width, height);
			}

			/// <summary>Builds a monochramatic texture of a given color determined by RGB valued</summary>
			/// <param name="red">Red Hue</param> <param name="blue">Blue Hue</param>
			/// <param name="green">Green Hue</param> <param name="alpha">Transparency</param>
			/// <returns>Newly defined texture matching specifications</returns>
			public static Texture2D GenerateBlankTexture(int red, int green, int blue, int alpha = 255, int width=1, int height=1) {
				return GenerateBlankTexture(new Color(red, green, blue, alpha), width=1, height=1);
			}
		}
	}
}
