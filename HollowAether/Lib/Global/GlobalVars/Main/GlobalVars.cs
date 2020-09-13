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
using Microsoft.Xna.Framework.Content;
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
		static GlobalVars() {
			foreach (String path in (from X in FileIO.assetPaths.Keys select Path.Combine(Directory.GetCurrentDirectory(), FileIO.assetPaths[X])))
				if (!Directory.Exists(path)) Directory.CreateDirectory(path); // Make if not found: pretty much for saves

			MapZone.globalAssets = new AssetContainer {
				{ "defaultAnimationRunning", new BooleanAsset("defaultAnimationRunning", true)      },
				{ "background",              new StringAsset("background", "default_value")         },
				{ "playerStartPosition",     new PositionAsset("playerStartPosition", Vector2.Zero) }
			};

			LoadAnimations(FileIO.assetPaths["Animations"]); // Load animations
		}

		public static void LoadAnimations(String path) {
			foreach (String animFile in StartUpMethods.SystemParserScriptInterpreter(path)) {
				Dictionary<String, AnimationSequence> fileSequences = Animation.FromFile(animFile);

				foreach (String key in fileSequences.Keys) {
					String nKey = $"{InputOutputManager.GetFileTitleFromPath(animFile)}\\{key}".ToLower();
					MonoGameImplement.importedAnimations[nKey] = fileSequences[key]; // add animation to importable
				}
			}
		}

		public static HollowAetherGame hollowAether = new HollowAetherGame();
	}
}
