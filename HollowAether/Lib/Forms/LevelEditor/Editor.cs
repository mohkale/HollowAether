#region SystemImports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
#endregion

#region HollowAetherImports
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.InputOutput.Parsers;

using LE = HollowAether.Lib.Forms.LevelEditor;
#endregion

namespace HollowAether.Lib.Forms {
	public partial class Editor : Form {
		private class ZoneListViewItem : ListViewItem {
			public ZoneListViewItem(Vector2 position, Map.ZoneArgs args) : base(args.path) {
				SubItems.Insert(1, new ListViewSubItem(this, args.visible ? "V" : "N"));
				zoneArgs = args; // Store both zone file path and visibility indicator
				this.position = position; // Position of given in a map setting
			}

			public Map.ZoneArgs zoneArgs;
			public Vector2 position;

			public String ZonePath { get { return zoneArgs.path; }
				set {
					zoneArgs.path = value;
					// Check move zone
				}
			}

			public bool ZoneVisible { get { return zoneArgs.visible; }
				set {
					zoneArgs.visible = value; // Store update value for zone file
					SubItems[1].Text = value ? "V" : "N"; // Update column value
					//SubItems[1] = new ListViewSubItem(this, value ? "V" : "N");
				}
			}
		}

		public Editor(String mapFilePath = null) {
			InitializeComponent();

			#region Image Load

			Tuple<Image, String>[] images = GV.Content.LoadImages(); // Get images
			GV.LevelEditor.textures = new Dictionary<String, Tuple<Image, String>>();

			foreach (Tuple<Image, String> tuple in images) {
				string key = tuple.Item2.Replace(GV.hollowAether.Content.RootDirectory, "");
				key = key.Substring(key.IndexOf('\\') + 1); // Remove Holder Folder From Key
				GV.LevelEditor.textures[IOMan.RemoveExtension(key).ToLower()] = tuple; 
			}
			#endregion

			//Zone.keepBlocks				  = true;
			//Zone.dontExecuteBlockStatements = true;
			Zone.StoreAssetTrace = true;

			mapFilePath = (String.IsNullOrEmpty(mapFilePath)) ? defaultMapPath : mapFilePath;
			// If map path passed as argument, set that as map, otherwise use default path

			if (IOMan.FileExists(mapFilePath)) LoadMapOrZoneFile(mapFilePath); else {
				MessageBox.Show($"Map '{mapFilePath}' Not Found", "Map Not Found", 0, MessageBoxIcon.Warning);
			}
		}

		private void Editor_Load(object sender, EventArgs e) {
			GV.LevelEditor.tileMap = new LE.TileMap();

			GV.LevelEditor.tileMap.FormClosing += (s, e2) => {
				e2.Cancel = true; // Cancel form disposal
				GV.LevelEditor.tileMap.Hide(); // Hide
			};
		}

		private void Editor_FormClosing(object sender, FormClosingEventArgs e) {
			CheckSave();
		}

		#region EventHandler
		private void LEKeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape && CheckSave())
				Application.Exit(); // Exit on escape after save
		}

		private void Editor_DragEnter(object sender, DragEventArgs e) {
			if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
		}

		private void Editor_DragDrop(object sender, DragEventArgs e) {
			string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length > 1 || files.Length <= 0) return; // go back
			LoadMapOrZoneFile(files[0]); // Loads Map Or Zone File
		}

		private void zoneViewList_SelectedIndexChanged(object sender, EventArgs e) {
			if (zoneViewList.FocusedItem != null && !String.IsNullOrWhiteSpace(zoneViewList.FocusedItem.Text)) {
				currentPositionTextBox.Text = (zoneViewList.FocusedItem as ZoneListViewItem).position.ToString();
			}
		}

		private void zoneViewList_MouseDoubleClick(object sender, MouseEventArgs e) {
			if (e.Button == MouseButtons.Left && zoneViewList.FocusedItem != null) {
				ZoneListViewItem item = (ZoneListViewItem)zoneViewList.FocusedItem;

				bool recreateEditor = zoneEditor == null || zoneEditor.IsDisposed;

				if (recreateEditor) zoneEditor = new LE.ZoneEditor(item.zoneArgs.ToInstance(true));
				else				zoneEditor.Initialize(item.zoneArgs.ToInstance(true));

				zoneEditor.Show(); // Display newly made or existing form instance
			}
		}

		private void zoneViewList_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Enter) // If player has clicked enter while zone is focused
				zoneViewList_MouseDoubleClick(sender, new MouseEventArgs(MouseButtons.Left, 1, -1, -1, 0));
		}

		private void zoneViewList_MouseClick(object sender, MouseEventArgs e) {
			if (e.Button != MouseButtons.Right) return; // Skip 4-ward

			if (zoneViewList.FocusedItem.Bounds.Contains(e.Location)) {
				listViewContext.Show(Cursor.Position);
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
			if (zoneViewList.Focused) {
				String focusedPath = zoneViewList.FocusedItem.SubItems[0].Text;
				Vector2 removalIndex = map.GetZoneIndexFromFilePath(focusedPath);

				DialogResult result = MessageBox.Show(
					$"Would you like to delete from the file-system as well",
					"Please Confirm", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation
				);

				if (result != DialogResult.Cancel) {
					if (result == DialogResult.Yes) {
						string path = Zone.GetRelativeFilePath(map[removalIndex].path);
						if (IOMan.FileExists(path)) System.IO.File.Delete(path); // Delete
					}

					zoneViewList.Items.Remove(zoneViewList.FocusedItem);        // Delete from accessible display
					map.DeleteZone(removalIndex); // Delete from current map instance to ensure it no longer exists
					mapChanged = true;
				}
			}
		}

		private void addZoneButton_Click(object sender, EventArgs e) { newZoneTSMenuItem_Click(sender, e); }

		#region ToolStripEventHandlers
		private void newMapTSMenuItem_Click(object sender, EventArgs e) {
			if (mapLoaded) {
				DialogResult res = MessageBox.Show(
					"Creating A New Map Will Remove The Current Map",
					"Map Creation Warning", MessageBoxButtons.OKCancel
				);

				if (res == DialogResult.Cancel) return; else CheckSave(); // Save any changes
			}

			FolderBrowserDialog dialog = new FolderBrowserDialog();
			DialogResult result	       = dialog.ShowDialog(); // Show
			
			// Set Initial Zone Here

			if (result == DialogResult.OK) {
				String path = dialog.SelectedPath; // Path chosen by user
				IOMan.CheckConstruct(IOMan.Join(path, "Zones"));
				Map.CreateEmptyMap(IOMan.Join(path, "Map.Map")); // generate
				LoadMap(IOMan.Join(path, "Map.Map"), true); // Load map over existing map
			}

			dialog.Dispose(); // Free any un-necessary existing memory
		}

		private void animationsToolStripMenuItem_Click(object sender, EventArgs e) {
			LevelEditor.AnimationView view = new LE.AnimationView();
			view.ShowDialog();
		}

		private void newZoneTSMenuItem_Click(object sender, EventArgs e) { CreateZone(); }
		
		private void saveToolStripMenuItem_Click(object sender, EventArgs e) { Save(); }

		private void saveAsTSMenuItem_Click(object sender, EventArgs e) {
			using (FolderBrowserDialog browser = new FolderBrowserDialog() { ShowNewFolderButton = true }) {
				DialogResult pathResult = browser.ShowDialog(); // Display folder browser form as a dialog to retrieve desired target path 

				if (pathResult == DialogResult.OK) {
					IOMan.WriteEncryptedFile(IOMan.Join(browser.SelectedPath, "Map.map"), map.ToFileContents(), GV.Encryption.oneTimePad);

					bool copyZones = MessageBox.Show("Would you like to copy the zones in the map?", "Save As", MessageBoxButtons.YesNo) == DialogResult.Yes;
					
					if (copyZones) {
						foreach (Vector2 key in map.ZoneIndexes) {
							Map.ZoneArgs args = map[key]; // Get and store zone args for key
							String actualPath = IOMan.Join(IOMan.GetDirectoryName(map.FilePath), args.path);
							String targetPath = IOMan.Join(browser.SelectedPath, args.path);

							IOMan.SequenceConstruct(IOMan.GetDirectoryName(targetPath));
				
							System.IO.File.Copy(actualPath, targetPath); // Copy file
						}
					}
				}
			}
		}

		private void loadTSMenuItem_Click(object sender, EventArgs e) {
			OpenFileDialog dialog = new OpenFileDialog() {
				Filter = "Map files (*.MAP)|*.map|Zone files (*.ZNE)|*.ZNE|All files (*.*)|*.*",
				CheckFileExists = true, Title = "Select Map Or Zone File", Multiselect = false,
				InitialDirectory = mapZoneStorePath // Default search path is within CWD
			};

			if (dialog.ShowDialog() == DialogResult.OK) {
				LoadMapOrZoneFile(dialog.FileName); // Load map/zone file
				dialog.Dispose(); // Free up dialog memory resources
			}
		}

		private void modifyToolStripMenuItem_Click(object sender, EventArgs e) {
			if (zoneViewList.Focused) {
				var item = zoneViewList.FocusedItem as ZoneListViewItem;

				LE.ZoneFeatures.ZoneDetails details = LE.ZoneFeatures.Get(
					item.ZonePath, item.position, item.ZoneVisible, false
				);

				if (details.succeeded) {
					if (item.position != details.position) {
						currentPositionTextBox.Text = details.position.ToString();
					}

					item.ZonePath	 = details.filePath;
					item.ZoneVisible = details.visible;
					item.position	 = details.position;

					mapChanged = true; // Prompt save
				}
			}
		}

		private void modifyToolStripMenuItem1_Click(object sender, EventArgs e) {
			if (!zoneViewList.Focused) { MessageBox.Show($"No Zone To Modify Has Been Focused"); } else {
				modifyToolStripMenuItem_Click(sender, e); // Call existing modification event handler
			}
		}

		private void exitTSMenuItem_Click(object sender, EventArgs e) { Application.Exit(); }

		private void helpTSMenuItem_Click(object sender, EventArgs e) {
			(new LE.Help()).ShowDialog(); // Show help dialog
		}
		#endregion

		#endregion

		#region NotEventHandlers
		/// <summary>Checks whether or not to save & then prompts user</summary>
		/// <returns>Boolean indicating whether or not to exit</returns>
		private bool CheckSave() {
			if (!mapChanged) { return true; } else {
				DialogResult result = MessageBox.Show(
					"Editor Canvas Has Changed, Would You Like To Save",
					"Warning: Changes Haven't Been Saved",
					MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning
				); // Ask user and convert input to string

				if (result == DialogResult.Yes) { Save(); return true; } else {
					return false; // Don't save, O.K. to exit
				}
			}
		}

		/// <summary>Saves anything that's changed</summary>
		private void Save() {
			IOMan.WriteEncryptedFile(map.FilePath, map.ToFileContents(), GV.Encryption.oneTimePad);
			mapChanged = false;	// Allow closing of form because map has been saved.
		}

		private void LoadMapOrZoneFile(String path) {
			String headerString = IOMan.ReadEncryptedFile(path, GV.Encryption.oneTimePad).Split('\n')[0];
			bool isMap			= Parser.HeaderCheck(headerString, Map.MAP_FILE_HEADER);
			bool isZone			= Parser.HeaderCheck(headerString, Zone.ZONE_FILE_HEADER);

			if (isMap) LoadMap(path); else if (isZone) AddZoneToMap(path); else {
				String displayString = (headerString.Trim().Length == 0) ? 
					$"Given File '{path}' Is Empty" : $"Given File '{path}' Is Not Of Type Map Or Zone";

				MessageBox.Show(displayString, "Incorrect File Format", MessageBoxButtons.OK); // Display error to user and skip loading
			}
		}

		/// <summary>Method called to load a map</summary>
		/// <param name="fpath">Path to map file</param>
		private void LoadMap(String fpath, bool skipInitZoneSet=true) {
			if (mapLoaded) {
				DialogResult res = MessageBox.Show(
					"Loading A New Map Will Remove The Current Map",
					"Map Load Warning", MessageBoxButtons.OKCancel
				);

				if (res == DialogResult.Cancel) return; else CheckSave();
			}

			GV.MapZone.ClearFlags(); // Clear any stored map flags
			zoneViewList.Items.Clear(); // Clear any displayed zones
			map = new Map(fpath); // Set global map to new instance
			map.Initialize(!skipInitZoneSet); // Parse new map file instance 
			MapLoaded(fpath); // Map has been loaded
		}

		private void CreateZone() {
			if (!mapLoaded) MessageBox.Show($"No Map To Load Zone Into", "Error", MessageBoxButtons.OK); else {
				LE.ZoneFeatures.ZoneDetails details = LE.ZoneFeatures.Get(); // Get Zone Details

				if (details.succeeded) {
					String path = IOMan.Join(IOMan.GetDirectoryName(map.FilePath), details.filePath);

					if (IOMan.FileExists(path)) {
						DialogResult result = MessageBox.Show(
							"The given file already exists, would you like to load it",
							"File Exists Error", MessageBoxButtons.YesNo
						);

						if (result == DialogResult.No) return; 
					} else Zone.CreateEmptyZone(path);

					AddZoneToMap(path, details.position, details.visible);
				}
			}
		}

		private void AddZoneToMap(String fpath) {
			if (!mapLoaded) MessageBox.Show($"No Map To Load Zone Into", "Error", MessageBoxButtons.OK); else {
				if (map.ContainsZone(fpath)) MessageBox.Show("Zone Already In Editor", "Error", MessageBoxButtons.OK); else {
					LE.ZoneFeatures.ZoneDetails details = LE.ZoneFeatures.Get(fpath); // Get new zone details

					if (details.succeeded) {
						if (map.ContainsZone(details.position)) // Desired position already exists
							MessageBox.Show($"Zone at given position already exists", "Error", MessageBoxButtons.OK);
						else AddZoneToMap(fpath, details.position, details.visible);
					}
				}
			}
		}

		private void AddZoneToMap(String fpath, Vector2 index, bool visible) {
			if (!mapLoaded) MessageBox.Show($"No Map To Load Zone Into", "Error", MessageBoxButtons.OK); else {
				bool zoneExists = map.ContainsZone(fpath), indexExists = map.ZoneIndexes.Contains(index);

				if (zoneExists || indexExists) {
					String errorString = zoneExists ? "Zone Already In Editor" : "Given index already exists in map";
					MessageBox.Show(errorString, "Error", MessageBoxButtons.OK); // Display error to editor user
				} else {
					String path = IOMan.GetRelativeFilePath(fpath, IOMan.GetDirectoryName(map.FilePath));
					map.AddZone(index, path, visible); // Add zone to current map instance as new zone
					zoneViewList.Items.Add(new ZoneListViewItem(index, new Map.ZoneArgs(path, visible)));

					mapChanged = true; // Press user to save changes upon closing of form
				}
			}
		}

		/// <summary>Things To Do When Map Has Been Loaded</summary>
		private void MapLoaded(String mapPath) {
			mapLabelTextBox.Text  = mapPath;
			zoneViewList.Enabled  = true;
			addZoneButton.Enabled = true;
			mapLoaded			  = true;

			foreach (Vector2 zone in map.ZoneIndexes) {
				zoneViewList.Items.Add(new ZoneListViewItem(zone, map[zone]));
			}
		}
		#endregion

		#region PathDefinitions

		#region Directories
		public static String levelEditorBasePath = @"Assets\LevelBuilder";
		public static String textureBasePath     = IOMan.Join(levelEditorBasePath, "Textures");
		public static String saveBasePath        = IOMan.Join(levelEditorBasePath, "Save");
		public static String mapZoneStorePath    = IOMan.Join(levelEditorBasePath, "Store");
		//public static String animationStorePath  = IOMan.Join(mapZoneStorePath,    "Animations");
		public static String defaultMapPath      = IOMan.Join(levelEditorBasePath, "Map.map");
		#endregion

		#region Files
		public static String saveFilePath = IOMan.Join(saveBasePath, "Editor.SVE");
		#endregion

		#endregion

		private LE.ZoneEditor zoneEditor;
		private bool mapLoaded			 = false;
		private bool mapChanged			 = false;

		private Map map { get { return GV.MonoGameImplement.map; } set { GV.MonoGameImplement.map = value; } }

		private void setStartZoneToolStripMenuItem_Click(object sender, EventArgs e) {
			Vector2? value = SetInitialZone.Get(map.GetStartZoneIndexOrEmpty());

			if (value.HasValue) {
				if (!map.ZoneIndexes.Contains(value.Value)) {
					DialogResult result = MessageBox.Show(
						"Desired Zone Doesn't Exist Yet. Would You Like To Continue.",
						"", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information
					);

					if (result != DialogResult.Yes) return; // Input = No || Input = Cancel
				}

				map.SetStartZone(value.Value); // Set start zone to argument index
				mapChanged = true; // Query user to save before closing editor
			}
		}
	}
}
