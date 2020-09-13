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
		public static class FileIO {
			public static SettingsManager settingsManager;

			public static Dictionary<String, String> assetPaths = new Dictionary<String, String>() {
				{ "Root", "Assets" }, { "Save", @"Assets\Saves" },
				{ "Animations", @"Assets\Animations" },
				{ "Map", @"Assets\Map" },
				{ "EditorRoot", Forms.Editor.levelEditorBasePath },
				{ "EditorTextures", Forms.Editor.textureBasePath },
				{ "EditorMaps", Forms.Editor.mapZoneStorePath },
			};

			public static String DefaultMapPath = InputOutputManager.Join(assetPaths["Map"], "Map.map");
		}
	}
}
