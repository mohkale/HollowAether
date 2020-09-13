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
		public static class Variables {
			public static Vector2 GetScaledScreenSize(float scale) {
				return new Vector2(Variables.windowWidth, Variables.windowHeight) * new Vector2(scale);
			}

			public static Vector2 windowSize { get { return new Vector2(windowWidth, windowHeight); } }

			public static Random random = new Random();
			public static readonly float degrees45ToRadians = (float)Math.Sin(Math.PI / 4);
			public static int windowWidth, windowHeight, displayWidth, displayHeight;
			public static int playerDashCost = 5;
		}
	}
}
