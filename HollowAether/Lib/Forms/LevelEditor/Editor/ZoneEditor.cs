using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.GAssets;
using System.Text.RegularExpressions;
using V2 = Microsoft.Xna.Framework.Vector2;
using HollowAether.Lib.InputOutput.Parsers;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class ZoneEditor : Form {
		public enum CustomSelectionRectBuilderTypes { EntityWrapper, TileHighliter, None }

		/// <summary>Constructs a new zone editor</summary>
		public ZoneEditor(Zone zoneInstance) {
			InitializeComponent();

			Size = sizeBeforeResize = MinimumSize; // Set size to initial size

			entityTypeComboBox.Items.AddRange(GV.EntityGenerators.entityTypes.Keys.ToArray());
			entityTypeComboBox.SelectedIndex = 0; // Select First Entity In Entity Combo Box

			#region BuildControls
			zoneCanvasPanel.Controls.Add(canvas);

			tileBox.Width  = tileBoxContainerPanel.Width;
			tileBox.Height = tileBoxContainerPanel.Height;

			tileBox.HaveErrorIndicator = false;

			tileBoxContainerPanel.Controls.Add(tileBox);
			#endregion

			AddEventHandlers(); // Set Event Handlers
			showTileMapButton_Click(this, null);

			if (zoneInstance != null) Initialize(zoneInstance); // Init zone
		}

		private void ZoneEditor_Load(object sender, EventArgs e) {
			// clickRadioButton.Checked = true;
		}

		~ZoneEditor() { }

		/// <summary>Builds event handlers for form variables</summary>
		private void AddEventHandlers() {
			entityTypeComboBox.MouseWheel += (s, e) => { ((HandledMouseEventArgs)e).Handled = true; };

			GV.LevelEditor.tileMap.FormClosing += (s, e) => { tileBox.DrawTile = false; };

			GV.LevelEditor.tileMap.Shown += (s, e) => { tileBox.DrawTile = true; };

			GV.LevelEditor.tileMap.ReShown +=  (s) => { tileBox.DrawTile = true; };

			GV.LevelEditor.tileMap.canvas.BuildSelectionRect += (t, e) => {
				RectangleF r = e.currentRectangle; // Store current rectangle

				tileBox.AssignTextureAndRect(
					GV.LevelEditor.tileMap.CurrentImage, // Assign current image
					new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height)
				);
			};

			canvas.DrawToCanvas += DrawBackground_DrawToCanvas;
			canvas.DrawToCanvas += DrawTiles_DrawToCanvas;
			
			canvas.BuildSelectionRect += BuildSelectionRect_TileHighliter_RectBuilderEventHandler;
			canvas.ClickBegun		  += CanvasClickBegun_TileHighliter_MouseBasedEventHandler;
			canvas.ClickComplete	  += CanvasClickComplete_TileHighligter_MouseBasedEventHandler;

			canvas.BuildSelectionRect += BuildSelectionRect_EntityWrapper_RectBuilderEventHandler;
			//canvas.ClickBegun		  += CanvasClickBegun_EntityWrapper_MouseBasedEventHandler;
			//canvas.ClickComplete	  += CanvasClickComplete_EntityWrapper_MouseBasedEventHandler;

			canvas.CanvasRegionExecute += CanvasRegionExecute_MouseBasedEventHandler;
		}

		/// <summary>Initializes and builds zone assets from a given zone instance</summary>
		/// <param name="zoneInstance">Instance of a zone file with which to build editor</param>
		public void Initialize(Zone zoneInstance) {
			canvas.SetWidth(zoneInstance.ZoneWidth);
			canvas.SetHeight(zoneInstance.ZoneHeight);

			zone = zoneInstance; // Store zone to editor instance
			Size = sizeBeforeResize = MinimumSize;

			tiles.Clear(); // Delete all current tiles

			foreach (var entity in zone.zoneEntities) {
				var tile = new EntityTile(entity.Value);
				tiles.Add(entity.Key, tile); // store tile
			}
		}

		#region EventHandlers
		/// <summary>Default event handler for when the template entity type is changed</summary>
		private void entityTypeComboBox_TextChanged(object sender, EventArgs e) {
			SetTemplateEntity(GV.EntityGenerators.StringToGameEntity(entityTypeComboBox.SelectedItem.ToString()));
		}

		/// <summary>Disposes of any unnecessary assets and resets tilemap rects</summary>
		private void ZoneEditor_FormClosing(object sender, FormClosingEventArgs e) {
			// Check if save, then save
			if (canvasChanged) {
				DialogResult result = MessageBox.Show(
					"Editor Canvas Has Changed, Would You Like To Save Before Closing",
					"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning
				);

				if (result == DialogResult.Yes) saveToolStripMenuItem_Click(sender, e);
				else if (result == DialogResult.Cancel) e.Cancel = true;
			}
		}

		private void layerTextBox_TextChanged(object sender, EventArgs e) {
			float? newValue = GV.Misc.StringToFloat(layerTextBox.Text); // Convert

			if (newValue.HasValue) newValue = newValue.Value; else newValue = null;
		}

		private void DrawTiles_DrawToCanvas(object sender, PaintEventArgs e) {
			EntityTile[] collection = tiles.Values.ToArray(), trimmedCollection; // Get all tiles within current zone and cast to array

			if (DrawOnlyCurrentEntity) collection = (from X in collection where X.EntityType == CurrentEntityType select X).ToArray();
			// If draw only current entity, then trim collection to only entities of a type corresponding to the entity type of the editor

			if (SelectedLayer.HasValue) { // Only draw values which approximate to a given value
				Func<EntityTile, bool> ApproximatelyEqual = (tile) => Math.Abs(tile.Layer - SelectedLayer.Value) < 0.05;
				trimmedCollection = (from X in collection where ApproximatelyEqual(X) select X).ToArray(); // Within range
			} else {
				Array.Sort(collection, (a, b) => a.Layer.CompareTo(b.Layer));
				trimmedCollection = collection; // Set trimmed to sorted array
			}

			foreach (EntityTile tile in trimmedCollection) { tile.Paint(e); } // Draw all tiles
		}

		private void DrawBackground_DrawToCanvas(object sender, PaintEventArgs e) {
			// throw new NotImplementedException();
		}

		#region CheckBoxChange
		/// <summary>Thrown when user decides to show or disable the grid on the editor</summary>
		private void tileGridCheckBox_CheckedChanged(object sender, EventArgs e) {
			canvas.DrawGrid = tileGridCheckBox.Checked;
		}

		/// <summary>Thrown when user decides to show or disable the background image on the editor</summary>
		private void backgroundCheckBox_CheckedChanged(object sender, EventArgs e) {

		}

		/// <summary>Thrown when the user decides to only show the current entity type on the screen</summary>
		private void onlyCurrentEntityCheckBox_CheckedChanged(object sender, EventArgs e) {
			canvas.Draw(); // Re invoke
		}
		#endregion

		#region ZoomChange
		/// <summary>Sets zoom to 0.5 or 1/2</summary>
		private void xHalfToolStripMenuItem_Click(object sender, EventArgs e) { Zoom = 0.5f; }

		/// <summary>Sets zoom to 1</summary>
		private void oneXZoomMenuItem_Click(object sender, EventArgs e) { Zoom = 1f; }

		/// <summary>Sets zoom to 2</summary>
		private void twoXZoomMenuItem_Click(object sender, EventArgs e) { Zoom = 2f; }

		/// <summary>Sets zoom to 3</summary>
		private void threeXZoomMenuItem_Click(object sender, EventArgs e) { Zoom = 3f; }

		/// <summary>Sets zoom to 4</summary>
		private void fourXZoomMenuItem_Click(object sender, EventArgs e) { Zoom = 4f; }

		/// <summary>Sets zoom to 0.5 or 1/2</summary>
		private void zoomSubMenuItem_Click(object sender, EventArgs e) {
			float value = SetZoomDialog.GetZoom(Zoom, MIN_ZOOM, MAX_ZOOM);

			if (value != Zoom) Zoom = value; // Set zoom to new arg value
		}
		#endregion

		#region ClickHandlers
		/// <summary>Opens a new script editor form with current zone</summary>
		private void editScriptButton_Click(object sender, EventArgs e) {
			new ScriptEditor(zone.FilePath, true, true).Show();
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			InputOutput.InputOutputManager.WriteEncryptedFile(zone.FilePath, CanvasToFile(), GV.Encryption.oneTimePad);
		}

		/// <summary>Gives focus to existing tile map or creates new tile map form</summary>
		private void showTileMapButton_Click(object sender, EventArgs e) {
			if (!GV.LevelEditor.tileMap.Visible) {
				GV.LevelEditor.tileMap.Show(); // Shows tile map

				if (GV.LevelEditor.tileMap.canvas.SelectRect == Rectangle.Empty) {
					GV.LevelEditor.tileMap.canvas.ExternalSetSelectionRect(new RectangleF(0, 0, 32, 32));
				}

				RectangleF r = GV.LevelEditor.tileMap.canvas.SelectRect;

				tileBox.AssignTextureAndRect(
					GV.LevelEditor.tileMap.CurrentImage, // Assign current image
					new Rectangle((int)r.X, (int)r.Y, (int)r.Width, (int)r.Height)
				);
			}
		}

		/// <summary>Zooms canvas in by half times default</summary>
		private void zoomInButton_Click(object sender, EventArgs e) { Zoom += 1; }

		/// <summary>Zooms canvas out by half times default</summary>
		private void zoomOutButton_Click(object sender, EventArgs e) { Zoom -= 1; }

		/// <summary>Opens template tile edit menu</summary>
		private void openEditMenu_Click(object sender, EventArgs e) {
			var dialogResult = EditEntityView.RunForTemplateEntity(templateGameEntity, zone);

			if (dialogResult.state == EditEntityView.ReturnState.Changed) {
				SetTemplateEntity(dialogResult.updatedEntity);
			}
		}

		/// <summary>Views all built in assets used by the program</summary>
		private void viewAssetsButton_Click(object sender, EventArgs e) {
			assetViewToolStripMenuItem_Click(sender, e);
		}

		/// <summary>Resets default template tile to builtin version</summary>
		private void resetEntityButton_Click(object sender, EventArgs e) {
			entityTypeComboBox_TextChanged(sender, e);
		}

		private void resizeZoneButton_Click(object sender, EventArgs e) {
			setZoneSizeToolStripMenuItem_Click(sender, e);
		}

		/// <summary>Executes click for non generic click in zone canvas</summary>
		private void ExecuteButton_Click(object sender, EventArgs e) {
			canvas.InvokeCanvasRegionExecuteForHighlitedSelectionRects();
		}

		private void setBackgroundToolStripMenuItem_Click(object sender, EventArgs e) {

		}

		private void assetViewToolStripMenuItem_Click(object sender, EventArgs e) {
			if (!AssetView.Open) { // Then Open New View
				AssetView view = new AssetView(zone);

				view.saveButton.Click += (s, e2) => {
					canvasChanged = true;
				};

				view.Show();
			}
		}

		private void setZoneSizeToolStripMenuItem_Click(object sender, EventArgs e) {
			Tuple<int, int> results = SetZoneDimensions.Run(zone.FilePath, zone.ZoneWidth, zone.ZoneHeight);
			Size newSize = new Size() { Width = results.Item1, Height = results.Item2 }; // To Size Instance

			if (newSize != zone.Size) { // If size has changed
				if (newSize.Width < zone.ZoneWidth || newSize.Height < zone.ZoneHeight) {
					Rectangle newZoneDimensions = new Rectangle() { Location = new Point(0, 0), Size = newSize }; // New Region

					var clipped = (from X in tiles where !X.Value.Region.IntersectsWith(newZoneDimensions) select X).ToArray();

					if (clipped.Length > 0) {
						string num = clipped.Length.ToString().PadLeft(3, '0');

						DialogResult result = MessageBox.Show(
							$"{num} Tiles Will Be Clipped To Do This. Are You Sure?", "Warning",
							MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation // Triangle!
						);

						if (result != DialogResult.Yes) return; // User has canceled zone resize

						foreach (KeyValuePair<String, EntityTile> entityKVP in clipped) {
							DeleteTile(entityKVP.Key, false); // Deletes Tile
						}
					}
				}

				if (newSize.Width  != zone.ZoneWidth)  { zone.ZoneWidth  = newSize.Width;  canvas.SetWidth(newSize.Width);   }
				if (newSize.Height != zone.ZoneHeight) { zone.ZoneHeight = newSize.Height; canvas.SetHeight(newSize.Height); }

				canvasChanged = true; // Check to save before closing
			}
		}
		#endregion

		#endregion

		#region NotEventHandlers

		#region TileMethods
		/// <summary>Adds an entity to the zone and canvas</summary>
		/// <param name="key">Key for new tile to add</param>
		/// <param name="entity">Entity to add to canvas</param>
		private void AddEntity(string key, GameEntity entity, bool redraw = true) {
			zone.zoneEntities.Add(key, entity);
			var  tile = new EntityTile(entity);
			tiles.Add(key,               tile);

			canvasChanged = true; // Check to save before closing

			if (redraw) canvas.Draw();
		}

		private void AddEntity(GameEntity entity, int idLength=10, bool redraw=true) {
			String key; // Id for newly defined tile

			do {
				key = GV.Misc.GenerateRandomString(idLength);
			} while (tiles.ContainsKey(key));

			AddEntity(key, entity, redraw); // Add entity to entity store
		}

		private void DeleteTile(string id, bool reDraw=true) {
			tiles.Remove(id); // Delete from displayed tiles
			zone.zoneEntities.Remove(id); // Delete from zone

			canvasChanged = true; // Check to save before closing

			if (reDraw) canvas.Draw(); // Re-invoke canvas to draw updated values
		}

		/// <summary>Gets all tiles existing at a given position</summary>
		/// <param name="pos">Position to check for</param>
		private IEnumerable<KeyValuePair<String, EntityTile>> GetTiles(PointF pos) {
			foreach (KeyValuePair<String, EntityTile> tile in tiles) {
				if (tile.Value.Region.Contains(pos)) yield return tile;
			}
		}

		/// <summary>Gets all tiles intersecting with a given rect</summary>
		/// <param name="rect">Rectangle to check against</param>
		private IEnumerable<KeyValuePair<String, EntityTile>> GetTiles(RectangleF rect) {
			foreach (KeyValuePair<String, EntityTile> tile in tiles) {
				if (tile.Value.Region.IntersectsWith(rect)) yield return tile;
			}
		}

		/// <summary>Edits an existing tile within the zone/canvas</summary>
		/// <param name="id">Id of selected tile to edit</param>
		private void EditEntityView_Execute(String id) {
			EntityTile tile = tiles[id]; GameEntity entity = tile.Entity; // Store reference to game-entity & Tile
			EditEntityView.EditEntityViewResultsContainer results = EditEntityView.RunForExistingEntity(id, entity, zone);

			if (results.state == EditEntityView.ReturnState.Changed) {
				foreach (String key in results.updatedEntity.GetEntityAttributes()) {
					entity[key].Value = results.updatedEntity[key].GetValue();
				}

				canvas.Draw(); // Re-invoke canvas to draw updated values
			} else if (results.state == EditEntityView.ReturnState.Delete) {
				DeleteTile(id, true); // Delete and then redraw canvas
			} // else if (results.state == EditEntityView.ReturnState.Cancel) { }
		}

		private GameEntity GetDrawEntity(RectangleF newRegion) {
			GameEntity clonedTemplate = templateGameEntity.Clone(newRegion) as GameEntity;

			bool supportsDefaultAnimation = clonedTemplate.GetEntityAttributes().Contains("DefaultAnimation");

			if (supportsDefaultAnimation && !clonedTemplate["DefaultAnimation"].IsAssigned) {
				bool inTileMap = tileBox.DrawTile && tileBox.TextureRegion != Rectangle.Empty;
				AnimationSequence sequence = new AnimationSequence(0, new Frame(tileBox.TextureRegion));
				if (inTileMap) clonedTemplate["DefaultAnimation"].SetAttribute(sequence);
			}

			if (!clonedTemplate["Texture"].IsAssigned && tileBox.DrawTile) {
				clonedTemplate["Texture"].Value = GV.LevelEditor.tileMap.CurrentImageTextureKey;
			}

			return clonedTemplate;
		}

		private void SetTemplateEntity(GameEntity entity) {
			templateGameEntity = entity;

			bool positionAssigned = templateGameEntity["Position"].IsAssigned;
			bool widthAssigned    = templateGameEntity["Width"].IsAssigned;
			bool heightAssigned   = templateGameEntity["Height"].IsAssigned;

			if (drawRadioButton.Checked) {
				if (positionAssigned || widthAssigned || heightAssigned) {
					canvas.SelectOption = CanvasRegionBox.SelectType.Custom;
					CustomSelectOption = CustomSelectionRectBuilderTypes.EntityWrapper;
				} else {
					canvas.SelectOption = CanvasRegionBox.SelectType.GridClick;
					CustomSelectOption = CustomSelectionRectBuilderTypes.None;
				}
			}
		}
		#endregion

		/// <summary>Event handler to resize canvas alongside form</summary>
		private void ZoneEditor_SizeChanged(object sender, EventArgs e) {
			Size deltaSize = Size - sizeBeforeResize; // Change in size

			zoneCanvasPanel.Size += deltaSize; // Increment the size of the zone canvas by the change in size
			formatGroupBox.Location = new Point(formatGroupBox.Location.X + deltaSize.Width, formatGroupBox.Location.Y);

			sizeBeforeResize = Size; // Set sizeBeforeResize to equal current size
		}

		/// <summary>Converts zone canvas to a file</summary>
		public String CanvasToFile() {
			String header = $"{Zone.ZONE_FILE_HEADER} (X:{zone.ZoneWidth} Y:{zone.ZoneHeight})";

			StringBuilder builder = new StringBuilder($"{header}\n\n");

			builder.AppendLine(zone.ZoneAssetTrace.ToFileContents());

			builder.Append("\n"); // Leave two line breaks

			foreach (KeyValuePair<String, EntityTile> tileKVP in tiles) {
				builder.AppendLine(tileKVP.Value.Entity.ToFileContents(tileKVP.Key));
			}

			return builder.ToString();
		}
		#endregion

		/// <summary>Canvas zoom</summary>
		public float Zoom { get { return canvas.ZoomX; } set {
				float newValue = GV.BasicMath.Clamp(value, MIN_ZOOM, MAX_ZOOM);
				canvas.Zoom = new SizeF(newValue, newValue); // Set zoom value
			}
		}

		/// <summary>Canvas region upon which zone is drawn</summary>
		private CanvasRegionBox canvas = new CanvasRegionBox() {
			DrawGrid = true, DrawBorder = true,
			SelectOption = CanvasRegionBox.SelectType.GridClick,
			dragableSelectionRectsAreVolatile = true
		};

		/// <summary>Whether to only draw the currently highlited entity</summary>
		public bool DrawOnlyCurrentEntity { get { return onlyCurrentEntityCheckBox.Checked; } }

		/// <summary>Entity value stored within the entity type combo box</summary>
		public string CurrentEntityType { get { return entityTypeComboBox.SelectedItem.ToString(); } }

		private float? SelectedLayer = null;

		/// <summary>Bottom left tile box, representing value on tile map</summary>
		private TileBox tileBox = new TileBox() { HaveErrorIndicator = false };

		public const float MAX_ZOOM = 15f, MIN_ZOOM = 0.5f;

		private Dictionary<String, EntityTile> tiles = new Dictionary<String, EntityTile>();

		private Size sizeBeforeResize;

		private GameEntity templateGameEntity;

		private Zone zone;

		private bool canvasChanged = false;
	}
}
