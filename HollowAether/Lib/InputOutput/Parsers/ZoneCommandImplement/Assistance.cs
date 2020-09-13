using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.MapZone;

namespace HollowAether.Lib.InputOutput.Parsers {
	static partial class ZoneParser {
		private static void AddAsset(String assetID, Asset asset) {
			if (zone.assets.ContainsKey(assetID))
				throw new HollowAetherException($"Cannot Create Asset '{assetID}' When it already exists");

			zone.assets.Add(assetID, asset);
		}

		private static void SetAsset(String ID, object value) {
			if (GV.MapZone.globalAssets.ContainsKey(ID))
				GV.MapZone.globalAssets[ID].SetAsset(value);
			else if (zone.assets.ContainsKey(ID))
				zone.assets[ID].SetAsset(value);
			else
				throw new HollowAetherException();
		}

		private static bool AssetExists(String ID) {
			return (zone.assets.Keys.Contains(ID) || GlobalVars.MapZone.globalAssets.Keys.Contains(ID));
		}

		private static bool FlagExists(String ID) {
			return (GV.MapZone.globalAssets.ContainsKey(ID) && GV.MapZone.globalAssets[ID].TypesMatch<Flag>());
		}
	}
}
