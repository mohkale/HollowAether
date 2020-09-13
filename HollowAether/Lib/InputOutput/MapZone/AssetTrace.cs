#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.GAssets;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
#endregion
using HollowAether.Lib.InputOutput.Parsers;
using HollowAether.Lib.Exceptions;


namespace HollowAether.Lib.MapZone {
	public class AssetTrace {
		public class AssetTraceArgs {
			public String ToFileContents() {
				if		(IsImported)   return $"ImportAsset: \"{Type}\" {Value} as [{ID}]";
				else if (IsDefinition) return $"DefineAsset: \"{Type}\" [{ID}] {Value}";
				else if (IsAssignment) return $"Set: [{ID}] {Value}";

				throw new HollowAetherException($"Asset '{ID}' Not Set, Imported Or Defined");
			}

			#region ArgumentType
			public bool IsImported { get; set; } = false;

			public bool IsDefinition { get; set; } = false;

			public bool IsAssignment { get; set; } = false;
			#endregion

			#region ValueDefintions
			public string DefaultValue { get; set; } = null;

			public string Type { get; set; }

			public string Value { get; set; }

			public string ID { get; set; }
			#endregion

			public bool HasDefaultValue { get { return DefaultValue != null; } }

			public bool ValueAssigned { get { return Value != null; } }
		}

		static AssetTrace() {
			globalAssetsClone = (from X in GV.MapZone.globalAssets.Values.ToArray() select X.Clone() as Asset).ToArray();
			// Clone all default global assets/values in existance. In most cases, these will be unassigned by default
		}

		public AssetTrace() {
			foreach (Asset asset in globalAssetsClone) {
				store.Add(new AssetTraceArgs() { // Note: can only be assigned
					ID = asset.assetID, Type = asset.assetType.Name, IsAssignment = true,
					DefaultValue = Converters.ValueToString(asset.assetType, asset.GetValue())
				});
			}
		}

		public void AddImportAsset(string ID, string type, string value) {
			store.Add(new AssetTraceArgs() { ID = ID, Type = type, Value = value, IsImported = true });
		}

		public void AddDefineAsset(string ID, string type, string value) {
			store.Add(new AssetTraceArgs() { ID = ID, Type = type, Value = value, IsDefinition = true });
		}

		public void AddSetAsset(string ID, string value) {
			store[GetIndexFromKey(ID)].Value = value;
		}

		public void AddEmptyAsset(string id, string type, bool definition = false, bool import = false, bool set = false) {
			store.Add(new AssetTraceArgs() {
				ID = id, Type = type, IsDefinition = definition,
				IsImported = import, IsAssignment = set
			});
		}

		public string ToFileContents() {
			StringBuilder builder = new StringBuilder();

			foreach (AssetTrace.AssetTraceArgs args in store) {
				if (args.ValueAssigned) builder.AppendLine(args.ToFileContents());
			}

			return builder.ToString();
		}

		public void DeleteAssetValue(string id) {
			store[GetIndexFromKey(id)].Value = null;
		}

		public void DeleteAsset(string id) {
			store.RemoveAt(GetIndexFromKey(id));
		}

		public void SetValue(string id, string value) {
			store[GetIndexFromKey(id)].Value = value;
		}

		private int GetIndexFromKey(string id) {
			foreach (int X in Enumerable.Range(0, store.Count)) {
				if (store[X].ID == id) return X; // Found at X
			}

			return -1;
		}

		public bool Exists(string id) {
			foreach (AssetTraceArgs args in store) {
				if (id == args.ID) return true;
			}

			return false; // Not Found
		}

		private static Asset[] globalAssetsClone;
		public List<AssetTraceArgs> store = new List<AssetTraceArgs>();
	}
}