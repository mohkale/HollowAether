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
using HollowAether.Lib.InputOutput.Parsers;
using HollowAether.Lib.Exceptions;
using V2 = Microsoft.Xna.Framework.Vector2;
using GV = HollowAether.Lib.GlobalVars;
using SRBEA = HollowAether.Lib.Forms.LevelEditor.CanvasRegionBox.SelectionRectBuildersEventArgs;
using ST = HollowAether.Lib.Forms.LevelEditor.CanvasRegionBox.SelectType;
using CSRBT = HollowAether.Lib.Forms.LevelEditor.ZoneEditor.CustomSelectionRectBuilderTypes;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class ZoneEditor {
		private void CanvasRegionExecute_MouseBasedEventHandler(ST type, RectangleF finalRect, RectangleF[] highlited) {
			if (highlited == null) { // Single Rect Vars
				if (drawRadioButton.Checked)   Draw_SingleRectPropertyImplementer(type,   finalRect);
				if (fillRadioButton.Checked)   Fill_SingleRectPropertyImplementer(type,   finalRect);
				if (copyRadioButton.Checked)   Copy_SingleRectPropertyImplementer(type,   finalRect);
				if (pasteRadioButton.Checked)  Paste_SingleRectPropertyImplementer(type,  finalRect);
				if (deleteRadioButton.Checked) Delete_SingleRectPropertyImplementer(type, finalRect);
			} else { // Multi Rect Vars
				if (drawRadioButton.Checked)   Draw_MultipleRectPropertyImplementer(type,   highlited);
				if (fillRadioButton.Checked)   Fill_MultipleRectPropertyImplementer(type,   highlited);
				if (copyRadioButton.Checked)   Copy_MultipleRectPropertyImplementer(type,   highlited);
				if (pasteRadioButton.Checked)  Paste_MultipleRectPropertyImplementer(type,  highlited);
				if (deleteRadioButton.Checked) Delete_MultipleRectPropertyImplementer(type, highlited);
			}
		}

		#region ClickComplete_PropertyImplementer

		#region Draw
		/// <summary>Adds a single tile to the canvas ata  specified region</summary>
		/// <param name="type"></param>
		/// <param name="finalRect"></param>
		private void Draw_SingleRectPropertyImplementer(ST type, RectangleF finalRect) {
			Rectangle rounded = new Rectangle((int)finalRect.X, (int)finalRect.Y, (int)finalRect.Width, (int)finalRect.Height);

			if (new Rectangle(Point.Empty, zone.Size).Contains(rounded)) 
				AddEntity(GetDrawEntity(rounded), redraw: true);
		} // Complete

		private void Draw_MultipleRectPropertyImplementer(ST type, RectangleF[] rects) {
			if (rects.Count() > 15) { // Warn about adding too many
				DialogResult result = MessageBox.Show(
					$"You're about to add {rects.Length.ToString().PadLeft(3, '0')}. Are you sure you want to do this?",
					"Are you sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation // Could be tasking
				);

				if (result != DialogResult.Yes) return; // Result = DialogResult.No || DialogResult.Cancel
			}

			foreach (RectangleF highlitedRegion in rects) {
				Rectangle rounded = new Rectangle((int)highlitedRegion.X, (int)highlitedRegion.Y, (int)highlitedRegion.Width, (int)highlitedRegion.Height);

				if (new Rectangle(Point.Empty, zone.Size).Contains(rounded))
					AddEntity(GetDrawEntity(rounded), redraw: false);
			}

			canvas.Draw(); // Redraw canvas with new tile
		} // Complete
		#endregion

		#region Fill
		private void Fill_SingleRectPropertyImplementer(ST type, RectangleF finalRect) {
			int width  = templateGameEntity["Width"].IsAssigned  ? (int)templateGameEntity["Width"].GetActualValue()  : (int)canvas.GridWidth;
			int height = templateGameEntity["Height"].IsAssigned ? (int)templateGameEntity["Height"].GetActualValue() : (int)canvas.GridHeight;

			int xSpan = (int)Math.Floor(finalRect.Width / width), ySpan = (int)Math.Floor(finalRect.Height / height);

			RectangleF[] rects = new RectangleF[xSpan * ySpan];
			int counter = 0; // Start adding to rects from 0

			foreach (int X in Enumerable.Range(0, xSpan)) {
				foreach (int Y in Enumerable.Range(0, ySpan)) {
					rects[counter++] = new RectangleF() {
						X = finalRect.X + (X * width),
						Y = finalRect.Y + (Y * width),
						Width = width, Height = height
					};
				}
			}

			Draw_MultipleRectPropertyImplementer(type, rects);
		} // Complete

		private void Fill_MultipleRectPropertyImplementer(ST type, RectangleF[] rects) {
			Draw_MultipleRectPropertyImplementer(type, rects);
		} // Complete
		#endregion

		#region Delete
		private void Delete_SingleRectPropertyImplementer(ST type, RectangleF finalRect) {
			if (type == ST.Custom && CustomSelectOption == CSRBT.TileHighliter && tileSelectionArgs != null) {
				if (tileSelectionArgs.Item4 == MouseButtons.Left && tileSelectionArgs.Item3 == tiles[tileSelectionArgs.Item1].Location) {
					// Basically, if tile highliting is active, user has left clicked tile, and tile hasn't been moved, try to delete

					DialogResult dialog = MessageBox.Show(
						$"Are You Sure You Want To Delete The Tile '{tileSelectionArgs.Item1}'", // Query with id
						"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk // Do if Yes, ignore others
					);

					if (dialog == DialogResult.Yes) DeleteTile(tileSelectionArgs.Item1); // Delete selected tile
				}
			} else {
				KeyValuePair<String, EntityTile>[] intersected = GetTiles(finalRect).ToArray();

				if (intersected.Length > 0) {
					DialogResult dialog = MessageBox.Show(
						$"Are You Sure You Want To Delete {intersected.Length.ToString().PadLeft(3, '0')} Tiles",
						"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk // Do if Yes, ignore others
					);

					if (dialog == DialogResult.Yes) {
						foreach (KeyValuePair<String, EntityTile> tile in intersected) {
							DeleteTile(tile.Key, false); // Don't redraw the canvas
						}

						canvas.Draw(); // Redraw canvas after deleting
					}
				}
			}
		} // Complete

		private void Delete_MultipleRectPropertyImplementer(ST type, RectangleF[] rects) {
			// Not supported by level editor, leave method in case implemented later
		}
		#endregion

		#region Copy
		private void Copy_SingleRectPropertyImplementer(ST type, RectangleF finalRect) {
			if (type == ST.Custom && CustomSelectOption == CSRBT.TileHighliter && tileSelectionArgs != null) {
				if (tileSelectionArgs.Item4 == MouseButtons.Right) return; // Right click menu
				
				GameEntity cloned = tiles[tileSelectionArgs.Item1].Entity.Clone() as GameEntity;

				if (cloned["Position"].IsAssigned) cloned["Position"].Delete();
				entityTypeComboBox.Text = cloned.EntityType; // Set copied entity type
				SetTemplateEntity(cloned); // Assign template entity to copied entity

				pasteRadioButton.Checked = clickRadioButton.Checked = true;


			}
		}

		private void Copy_MultipleRectPropertyImplementer(ST type, RectangleF[] rects) {
			// Not supported by level editor, leave method in case implemented later
		}
		#endregion

		#region Paste
		private void Paste_SingleRectPropertyImplementer(ST type, RectangleF finalRect) {
			Draw_SingleRectPropertyImplementer(type, finalRect);
		}

		private void Paste_MultipleRectPropertyImplementer(ST type, RectangleF[] rects) {
			// Not supported by level editor, leave method in case implemented later
		}
		#endregion


		#endregion

		#region TileHighliter_SelectionRectBuilder

		#region AssistanceMethods
		/// <summary>Creates Selection Tile Arguments For</summary>
		/// <param name="tileId">Id of selected tile</param>
		/// <param name="tile">Reference to selected tile</param>
		/// <param name="point">Point where tile is clicked</param>
		private void CreateNewTileSelectionArgs(String tileId, EntityTile tile, PointF point, MouseButtons button) {
			SizeF displacement = new SizeF() { // displacementFromTileOrigin
				Width = point.X - tile.Location.X, Height = point.Y - tile.Location.Y
			};

			tileSelectionArgs = new Tuple<string, SizeF, PointF, MouseButtons>(tileId, displacement, tile.Location, button);
		}

		/// <summary>Event called when user has tried to select more then one possible tile.
		/// Basically asks which tile they want to edit and then set's it to selected</summary>
		private void MultipleTileSelectionContextMenu_ClickEventHandler(object sender, MouseButtons button) {
			int index = multipleTileSelectionContextMenu.MenuItems.IndexOf(sender as MenuItem);

			#region UpdateClick&GridClickPoint
			PointF cursorPos = canvas.PointToClient(Cursor.Position); // Get current cursor position
			cursorPos = new PointF(cursorPos.X / canvas.ZoomX, cursorPos.Y / canvas.ZoomY); // zoomed

			cursorPos = (!tileGridCheckBox.Checked) ? cursorPos : new PointF() {
				X = GV.BasicMath.RoundDownToNearestMultiple(cursorPos.X, canvas.GridWidth, false),
				Y = GV.BasicMath.RoundDownToNearestMultiple(cursorPos.Y, canvas.GridHeight, false)
			};
			#endregion

			KeyValuePair<String, EntityTile> selectedKVP = canvasClickIntersectedTiles[index];
			CreateNewTileSelectionArgs(selectedKVP.Key, selectedKVP.Value, cursorPos, button);
		}
		#endregion

		/// <summary>When mouse-down on the canvas and the tile highligting algorithm has been selected</summary>
		/// <param name="type">The current selection type used by the canvas</param>
		/// <param name="clickPoint">The point on the canvas where the mouse was clicked</param>
		/// <param name="gridClickPoint">The closest grid point on the canvas where the mouse was clicked</param>
		private void CanvasClickBegun_TileHighliter_MouseBasedEventHandler(ST type, PointF clickPoint, PointF gridClickPoint, MouseButtons button) {
			if (type == ST.Custom && CustomSelectOption == CSRBT.TileHighliter) {
				canvasClickIntersectedTiles = GetTiles(clickPoint).ToArray(); // Get all intersected tiles

				if (canvasClickIntersectedTiles.Length > 1) { // If more then one potential collection
					multipleTileSelectionContextMenu = new ContextMenu(); // Show tile options

					foreach (KeyValuePair<String, EntityTile> KVP in canvasClickIntersectedTiles) {
						MenuItem item = new MenuItem(KVP.Key); // New item for the context menu
						item.Click += (s, e) => MultipleTileSelectionContextMenu_ClickEventHandler(s, button);
						multipleTileSelectionContextMenu.MenuItems.Add(item); // Add new item
					}

					Point roundedClickPoint = new Point() { X = (int)clickPoint.X, Y = (int)clickPoint.Y };
					multipleTileSelectionContextMenu.Show(canvas, roundedClickPoint); // Display menu
				} else if (canvasClickIntersectedTiles.Length == 1) {
					PointF point = (tileGridCheckBox.Checked) ? gridClickPoint : clickPoint;
					KeyValuePair<String, EntityTile> selectedKVP = canvasClickIntersectedTiles[0];
					CreateNewTileSelectionArgs(selectedKVP.Key, selectedKVP.Value, point, button);
				}
			}
		}

		/// <summary>Actually highlits any tiles within the canvas that the cursor is hovering</summary>
		/// <param name="type">The type of selection rect allocated by the canvas-region</param>
		private void BuildSelectionRect_TileHighliter_RectBuilderEventHandler(CanvasRegionBox.SelectType type, SRBEA e) {
			if (type == ST.Custom && CustomSelectOption == CSRBT.TileHighliter) {
				if (tileSelectionArgs != null) { // When no tile is being dragged or has been selected

					if (tileSelectionArgs.Item4 == MouseButtons.Left) { // Tile drag implementation
						PointF current = (tileGridCheckBox.Checked) ? e.CurrentGridPoint : e.CurrentPoint;
						tiles[tileSelectionArgs.Item1].Location = current - tileSelectionArgs.Item2;
					}

					e.currentRectangle = tiles[tileSelectionArgs.Item1].Region;
				} else { // Can be assumed that mouse is not down, thus just highlight any tiles
					IEnumerable<KeyValuePair<String, EntityTile>> tiles = GetTiles(e.CurrentPoint); // Tiles within range

					if (tiles.Count() == 0) { e.currentRectangle = RectangleF.Empty; } else { // Erase when empty
						e.currentRectangle = (from X in tiles select X.Value.Region).Aggregate(RectangleF.Union);
					}
				}
			}
		}

		/// <summary>Event called on click completeion for tile highligter algorithm</summary>
		/// <param name="type">The type of selection being used by the zone canvas</param>
		/// <param name="finalRect">The current selected rectangle region</param>
		private void CanvasClickComplete_TileHighligter_MouseBasedEventHandler(ST type, RectangleF finalRect) {
			bool execute = CustomSelectOption == CSRBT.TileHighliter && tileSelectionArgs != null;

			if (type == ST.Custom && execute) {
				#region NotMovedToOpenEditMenu_Implement
				/*if (tileSelectionArgs.Item3 == tiles[tileSelectionArgs.Item1].Location) {
					EditEntityView_Execute(tileSelectionArgs.Item1);
				}*/
				#endregion

				if (tileSelectionArgs.Item4 == MouseButtons.Right) {
					EditEntityView_Execute(tileSelectionArgs.Item1);
				}

				tileSelectionArgs = null; // Delete variable
				canvasClickIntersectedTiles = null; // delete
			}
		}
		#endregion

		#region EntityWrapper_SelectionRectBuilder
		
		private void BuildSelectionRect_EntityWrapper_RectBuilderEventHandler(ST type, SRBEA e) {
			if (CustomSelectOption == CSRBT.EntityWrapper && (type == ST.Custom || type == ST.GridHighlight)) {
				EntityAttribute positionAttrib = templateGameEntity["Position"];
				EntityAttribute widthAttrib    = templateGameEntity["Width"];
				EntityAttribute heightAttrib   = templateGameEntity["Height"];

				PointF location; // Value to hold which location to draw the selection rectangle

				if (!positionAttrib.IsAssigned) { location = (tileGridCheckBox.Checked) ? e.CurrentGridPoint : e.CurrentPoint; } else {
					var position = (Microsoft.Xna.Framework.Vector2)templateGameEntity["Position"].GetActualValue();
					location = new PointF(position.X, position.Y); // Cast vector2 position value to Microsoft PointF
				}

				int width  = (widthAttrib.IsAssigned)  ? (int)widthAttrib.GetActualValue()  : (int)canvas.GridWidth;
				int height = (heightAttrib.IsAssigned) ? (int)heightAttrib.GetActualValue() : (int)canvas.GridHeight;

				e.currentRectangle = new RectangleF(location, new SizeF(width, height)); // Set current rectangle
			}
		}

		#endregion

		#region RadioButtonCheckChange

		#region CanvasSelectTypeRadioButtons
		private void SetPropertyRadioButtons(bool draw, bool fill, bool delete, bool copy, bool paste) {
			drawRadioButton.Enabled   = draw;
			fillRadioButton.Enabled   = fill;
			deleteRadioButton.Enabled = delete;
			copyRadioButton.Enabled   = copy;
			pasteRadioButton.Enabled  = paste;
			executeButton.Enabled     = highlightRadioButton.Checked;
		}

		private void clickRadioButton_CheckedChanged(object sender, EventArgs e) {
			SetPropertyRadioButtons(true, false, false, false, true);

			if (!drawRadioButton.Checked && !pasteRadioButton.Checked)
				drawRadioButton.Select(); // first selectable property option

			canvas.SelectOption = CanvasRegionBox.SelectType.GridClick;
			canvas.Deselect(); // Deselect any selected regions within canvas

			SetTemplateEntity(templateGameEntity); // Also sets select option
		}

		private void highlightRadioButton_CheckedChanged(object sender, EventArgs e) {
			SetPropertyRadioButtons(true, true, false, false, false);

			if (!clickRadioButton.Checked && !fillRadioButton.Checked)
				drawRadioButton.Select(); // First selectable property option

			canvas.SelectOption = CanvasRegionBox.SelectType.GridHighlight;
			canvas.Deselect(); // Deselect any selected regions within canvas
		}

		private void rectangleRadioButton_CheckedChanged(object sender, EventArgs e) {
			SetPropertyRadioButtons(true, true, true, false, false);

			if (copyRadioButton.Checked || pasteRadioButton.Checked)
				clickRadioButton.Select(); // First selectable property option
			
			canvas.SelectOption = CanvasRegionBox.SelectType.GridDrag;
			canvas.Deselect(); // Deselect any selected regions within canvas
		}

		private void selectRadioButton_CheckedChanged(object sender, EventArgs e) {
			SetPropertyRadioButtons(false, false, true, true, false);

			if (!deleteRadioButton.Checked && !copyRadioButton.Checked)
				deleteRadioButton.Select(); // First selectable property option

			canvas.SelectOption = CanvasRegionBox.SelectType.Custom;
			CustomSelectOption  = CustomSelectionRectBuilderTypes.TileHighliter;
			canvas.Deselect(); // Deselect any selected regions within canvas
		}
		#endregion

		#region PropertyRadioButtons
		private void drawRadioButton_CheckedChanged(object sender, EventArgs e) {
			canvas.Deselect();
		}

		private void fillRadioButton_CheckedChanged(object sender, EventArgs e) {
			canvas.Deselect();
		}

		private void deleteRadioButton_CheckedChanged(object sender, EventArgs e) {
			canvas.Deselect();
		}

		private void copyRadioButton_CheckedChanged(object sender, EventArgs e) {
			canvas.Deselect();
		}

		private void pasteRadioButton_CheckedChanged(object sender, EventArgs e) {
			canvas.Deselect();
		}
		#endregion

		#endregion

		private Tuple<String, SizeF, PointF, MouseButtons> tileSelectionArgs = null;

		private CustomSelectionRectBuilderTypes CustomSelectOption;

		private ContextMenu multipleTileSelectionContextMenu;

		private KeyValuePair<String, EntityTile>[] canvasClickIntersectedTiles;
	}
}
