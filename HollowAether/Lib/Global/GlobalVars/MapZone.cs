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
		public static class MapZone {
			public static Asset GetAsset(String ID, AssetContainer localAssets) {
				try {
					if (globalAssets.ContainsKey(ID)) return globalAssets[ID];

					if (localAssets.ContainsKey(ID)) return localAssets[ID];

					throw new HollowAetherException($"Asset '{ID}' Not In Local Or Global Store");
				} catch (Exception e) {
					throw new HollowAetherException($"Could Not Find Asset '{ID}' Value", e);
				}
			}

			public static IEnumerable<FlagAsset> GetFlagAssets() {
				foreach (Asset asset in globalAssets.Values) {
					if (asset is FlagAsset) yield return (asset as FlagAsset);
				}
			}

			public static FlagAsset GetFlagAsset(String assetID) {
				foreach (FlagAsset asset in GetFlagAssets()) {
					if (asset.assetID == assetID) return asset;
				}

				throw new HollowAetherException($"Flag '{assetID}' Not Found");
			}

			public static bool FlagExists(String assetID) {
				foreach (FlagAsset flag in GetFlagAssets()) {
					if (flag.assetID == assetID) return true;
				}

				return false; // Not in flags
			}

			public static void ClearFlags() {
				String[] flagAssets = new String[globalAssets.Count];
				int counter = 0; // Counter to decide when to store

				foreach (KeyValuePair<String, Asset> asset in globalAssets) {
					if (asset.Value.TypesMatch<Flag>()) { flagAssets[counter++] = asset.Key; }
				}

				foreach (String key in flagAssets) {
					if (key != null) globalAssets.Remove(key);
				}
			}

			public static Dictionary<String, GameEntity> GlobalEntities = new Dictionary<String, GameEntity>();
			public static AssetContainer globalAssets;
		}
	}
}
