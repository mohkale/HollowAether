using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
using SUM = HollowAether.StartUpMethods;
using HollowAether.Lib.GAssets;
using LE = HollowAether.Lib.Forms.LevelEditor;

using System;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class LevelEditor {
			public static string GetTextureKeyFromInstance(Image image) {
				foreach (var keyVal in textures) {
					if (keyVal.Value.Item1 == image)
						return keyVal.Key;
				}

				throw new HollowAetherException("TextyreKeyNotFound");
			}

			public static Dictionary<String, Tuple<Image, String>> textures;
			public static LE.TileMap tileMap;
		}
	}
}
