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
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using GV = HollowAether.Lib.GlobalVars;
#endregion

using Converter = HollowAether.Lib.InputOutput.Parsers.Converters;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;

namespace HollowAether.Lib.MapZone {
	public class Map {
		public struct ZoneArgs {
			public ZoneArgs(String _path, bool _visible) {
				path = _path; visible = _visible;
			}

			public Zone ToInstance(bool initNow) {
				return new Zone(this, initNow);
			}

			public bool visible;
			public String path;
		}

		public Map(String fpath = null) {
			FilePath = (String.IsNullOrWhiteSpace(fpath)) ? GV.FileIO.DefaultMapPath : fpath;
			
			if (!IOMan.FileExists(FilePath)) throw new MapNotFoundException(FilePath);
		}

		public void Initialize(bool buildCurrentZone = true) {
			InputOutput.Parsers.MapParser.Parse(this);

			startZoneIndex = _currentZoneIndex; // Store start index for ToFileMethod

			if (buildCurrentZone) {
				if (Program.argZoneIndex.HasValue) _currentZoneIndex = Program.argZoneIndex.Value;
				// If index passed via command line has value then set the default fpath to it

				if (!_currentZoneIndex.HasValue)
					throw new HollowAetherException($"No Start Zone Set or Assigned");
				else if (!MapZoneCollection.ContainsKey(_currentZoneIndex.Value))
					throw new HollowAetherException($"{_currentZoneIndex.Value} Not in Map");

				currentZone = new Zone(this[_currentZoneIndex.Value], true); // Current Zone
			}
		}

		public String ToFileContents() {
			StringBuilder builder = new StringBuilder($"{Map.MAP_FILE_HEADER}\n\n");

			builder.AppendLine($"[StartZone{Converter.ValueToString(typeof(Vector2), startZoneIndex)}]\n");

			foreach (Asset asset in GV.MapZone.globalAssets.Values) {
				if (asset is FlagAsset) {
					bool value = (asset.asset as Flag).Value; // Get default value
					builder.AppendLine($"[DefineFlag({asset.assetID}={value})]");
				}
			}

			builder.AppendLine(); // Add line break after flag & start zone definition

			foreach (var MZCKeyValues in MapZoneCollection) {
				String visibility = MZCKeyValues.Value.visible ? "V" : "H"; // Is visible
				String pos = Converter.ValueToString(typeof(Vector2), MZCKeyValues.Key);
				builder.AppendLine($"{visibility}-{pos} \\{MZCKeyValues.Value.path}");
			}

			return builder.ToString(); // Convert to string and return builder
		}

		public void SetStartZone(Vector2 arg) { startZoneIndex = arg; }

		public void SetCurrentZone(Vector2 newIndex, bool skipCheck = false, bool build = true) {
			if (!skipCheck && !MapZoneCollection.ContainsKey(newIndex)) // Doesn't Exist
				throw new ZoneWithGivenMapIndexNotFoundException(newIndex);

			_currentZoneIndex = newIndex; // Set new index to argument index 

			if (build) {
				CurrentZone.RemoveZoneFromSpriteBatch(); // Remove all objects from game
				currentZone = new Zone(this[_currentZoneIndex.Value], true); // Current Zone
				CurrentZone.Initialize(); // Initialize zone contents by reading zone file
				CurrentZone.AddZoneToSpriteBatch(); // Add objects from new zone to game
			}
		}

		public Vector2? ZoneExistsAdjacentTo(Vector2 index) {
			foreach (bool xOffset in new bool[] { true, false }) {
				foreach (int N in new int[] { +1, -1 }) {
					Vector2 pos = new Vector2() {
						X = ( xOffset) ? index.X + N : index.X,
						Y = (!xOffset) ? index.Y + N : index.Y,
					};

					if (ContainsZone(pos)) return pos;
				}
			}

			return null; // Null means not index was found
		}

		public static bool CreateEmptyMap(String fpath) {
			try { // Used with level editor, has no real purpose in actual game logic
				IOMan.WriteEncryptedFile(fpath, $"{MAP_FILE_HEADER} # Header\n", GV.Encryption.oneTimePad);
			} catch { return false; }

			return true; // Succesfully made 
		}

		public bool ContainsZone(String filePath) {
			foreach (ZoneArgs args in MapZoneCollection.Values) {
				if (args.path == FilePath) return true;
			}

			return false;
		}

		public bool ContainsZone(Vector2 position) {
			return MapZoneCollection.ContainsKey(position);
		}

		public void AddZone(Vector2 key, String zonePath, bool zoneVisible) {
			MapZoneCollection.Add(key, new ZoneArgs(zonePath, zoneVisible));
		}

		public Vector2 GetZoneIndexFromFilePath(String filePath) {
			foreach (KeyValuePair<Vector2, ZoneArgs> KVP in MapZoneCollection) {
				if (KVP.Value.path == filePath) return KVP.Key;
			}

			throw new HollowAetherException();
		}

		public Dictionary<Vector2, ZoneArgs>.Enumerator EnumerateCollection() {
			return MapZoneCollection.GetEnumerator();
		}

		public ZoneArgs GetZoneArgsFromVector(Vector2 position) {
			return MapZoneCollection[position];
		}

		public ZoneArgs GetZoneArgsFromFilePath(Vector2 position) {
			return MapZoneCollection[position];
		}

		public void DeleteZone(Vector2 zoneIndex) {
			MapZoneCollection.Remove(zoneIndex);
		}

		public ZoneArgs this[Vector2 position] {
			get { return MapZoneCollection[position]; }
		}

		/// <summary>Update current zone parameter values</summary>
		public void Update() { CurrentZone.Update(); }

		public static readonly String MAP_FILE_HEADER = "MAP";

		/// <summary>Current zone in map</summary>
		private Vector2? _currentZoneIndex = Vector2.Zero, startZoneIndex;

		public Vector2 GetStartZoneIndexOrEmpty() {
			return startZoneIndex.HasValue ? startZoneIndex.Value : Vector2.Zero;
		}

		/// <summary>Dictionary of zones contained within map</summary>
		private Dictionary<Vector2, ZoneArgs> MapZoneCollection = new Dictionary<Vector2, ZoneArgs>();

		/// <summary>File path for given map</summary>
		public String FilePath { get; private set; }

		/// <summary>Number of zones in map</summary>
		public int Count { get { return MapZoneCollection.Count; } }

		/// <summary>All indexes of all known zones contained within map as key collection</summary>
		public Dictionary<Vector2, ZoneArgs>.KeyCollection ZoneIndexes { get { return MapZoneCollection.Keys; } }

		/// <summary>Index of current zone accesible within map which is regularly being update</summary>
		public Vector2 Index {
			get { if (_currentZoneIndex.HasValue) return _currentZoneIndex.Value; else throw new HollowAetherException($"Index Not Set"); }
			set { SetCurrentZone(value); } // Stores current zone index & creates new zone instance to replace existing current zone instance
		}

		private Zone currentZone;

		/// <summary>Instance of zone class representing current zone within map</summary>
		public Zone CurrentZone { get { return currentZone; } private set { currentZone = value; } }
	}
}