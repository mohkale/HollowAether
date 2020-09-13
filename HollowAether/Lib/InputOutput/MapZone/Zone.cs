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
	public class Zone {
		/// <summary>Used to check whether a given condition is valid</summary>
		/// <returns>Boolean indicating whether conditional expression has succeeded</returns>
		public delegate bool Condition();
		
		public Zone(String filePath, bool _visible=false, bool initialiseNow=false) {
			FilePath = GetRelativeFilePath(filePath);
			visible = _visible; // Whether zone is visible
			if (initialiseNow) Initialize(); // From construction
		}

		public Zone(Map.ZoneArgs args, bool initialiseNow=false) : this(args.path, args.visible, initialiseNow) { }

		public static String GetRelativeFilePath(String filePath) {
			if (IOMan.FileExists(filePath)) return filePath; else {
				String mapContainer = IOMan.GetDirectoryName(GV.MonoGameImplement.map.FilePath);
				String adjusted = System.IO.Path.GetFullPath(IOMan.Join(mapContainer, filePath));

				if (IOMan.FileExists(adjusted)) return adjusted; else {
					throw new ZoneNotFoundException(filePath);
				}
			}
		}

		public void Initialize() {
			if (!initialized) {
				ZoneParser.Parse(this);

				initialized = true; // Now initialised
			}

			/*foreach (IMonoGameObject _object in monogameObjects) {
				_object.Initialize(_object.Animation.TextureID);
			}*/
		}

		public bool Contains(Vector2 pos) {
			return new Rectangle(Point.Zero, new Point(ZoneWidth, ZoneHeight)).Contains(pos);
		}

		public static bool CreateEmptyZone(String fpath) {
			try {
				string fileContents = $"{ZONE_FILE_HEADER} # Header\n";
				IOMan.WriteEncryptedFile(fpath, fileContents, GV.Encryption.oneTimePad);
			} catch { return false; }
			
			return true; // Succesfully made 
		}

		public void Update() {
			/*if (runtimeEvents.Count < 1) return; // prevent wasted CPU usage

			List<Condition> removalBatch = new List<Condition>(runtimeEvents.Count);

			foreach (Condition conditionalEvent in runtimeEvents.Keys) {
				if (!conditionalEvent()) continue; // Condition Not Yet Valid

				foreach (Action func in runtimeEvents[conditionalEvent])
					func(); // Execute Command Here

				removalBatch.Add(conditionalEvent); // Mark Method for removal
			}

			foreach (Condition condition in removalBatch) {
				runtimeEvents.Remove(condition);
			}*/
		}

		public void AddZoneToSpriteBatch() {
			foreach (String entityKey in zoneEntities.Keys) {
				GameEntity entity = zoneEntities[entityKey];

				IMonoGameObject _object = entity.ToIMGO();
				_object.SpriteID = entityKey; // Set ID
				_object.Initialize(_object.Animation.TextureID);

				GV.MonoGameImplement.monogameObjects.Add(_object);
			}
		}

		public void RemoveZoneFromSpriteBatch() {
			foreach (string entityKey in zoneEntities.Keys) {
				if (GV.MonoGameImplement.monogameObjects.Exists(entityKey))
					GV.MonoGameImplement.monogameObjects.Remove(entityKey);
			}
		}

		public static String ZONE_FILE_HEADER = "ZNE";

		/// <summary>File path of zone file</summary>
		public String FilePath { get; private set; }

		/// <summary>Zone is visible in map HUD</summary>
		public bool visible;

		/// <summary>Map has been initialised</summary>
		public bool initialized;

		/// <summary>Local Asset Store</summary>
		public AssetContainer assets = new AssetContainer();

		/// <summary>Keep conditional blocks for Level Editor</summary>
		//public static bool keepBlocks = false; 

		/// <summary>Also for conditional block statements</summary>
		//public static bool dontExecuteBlockStatements = false;

		public static bool StoreAssetTrace { get; set; } //= false;

		public Dictionary<String, GameEntity> zoneEntities = new Dictionary<String, GameEntity>();

		// public MonoGameObjectStore monogameObjects = new MonoGameObjectStore();

		// private Dictionary<Condition, Action[]> runtimeEvents;

		public const int DEFAULT_ZONE_WIDTH = 736, DEFAULT_ZONE_HEIGHT = 736;

		public AssetTrace ZoneAssetTrace { get; private set; } = new AssetTrace();

		public int ZoneWidth { get; set; } = DEFAULT_ZONE_WIDTH;

		public int ZoneHeight { get; set; } = DEFAULT_ZONE_HEIGHT;

		public System.Drawing.Size Size { get { return new System.Drawing.Size(ZoneWidth, ZoneHeight); } }

		public Vector2 XSize { get { return new Vector2(ZoneWidth, ZoneHeight); } }
	}
}