using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using HollowAether.Lib.MapZone;
using GV = HollowAether.Lib.GlobalVars;
using EntityGenerator = HollowAether.Lib.GlobalVars.EntityGenerators;
using Converters = HollowAether.Lib.InputOutput.Parsers.Converters;

namespace HollowAether.Lib.Forms.LevelEditor {
	/// <summary>Form to allow modification of existing tiles</summary>
	partial class EditEntityView : Form {
		public struct EditEntityViewResultsContainer {
			public ReturnState state;
			public string id;
			public GameEntity updatedEntity;
		}

		public enum ReturnState { Changed, Delete, Cancel }

		EditEntityView(GameEntity entity, Zone zone) {
			InitializeComponent(); // Initialize Form Components

			entityTypeTextBox.Text = entity.EntityType;

			this.entity		  = entity.Clone() as GameEntity;
			this.backupEntity = entity.Clone() as GameEntity;
			this.zone		  = zone;

			BuildEntityAttributeView(); // Builds table to hold entity attributes for entity
			SetEntityAttributeViewItems(entity); // Assign Attributes For Current Entity Item, etc.

			Size = sizeBeforeResize = MinimumSize; // Store the default size of the form

			dataViewTable.Focus(); // Set focus to entity attributes view / 1st attribute value

			dataViewTable.CellBeginEdit += DataViewTableValueColumnChanged_CellBeginEdit;
			dataViewTable.CellEndEdit   += DataViewTableValueColumnChanged_CellEndEdit;

			ActiveControl = dataViewTable; // Set initial focus to data view table
		}

		private void DataViewTableValueColumnChanged_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			valueBefore_DataViewTableValueColumnChanged = dataViewTable[e.ColumnIndex, e.RowIndex].Value.ToString();
		}

		private void DataViewTableValueColumnChanged_CellEndEdit(object sender, DataGridViewCellEventArgs e) {
			DataGridViewCell cell = dataViewTable[e.ColumnIndex, e.RowIndex]; // Store New Cell

			string attributeKey = dataViewTable[0, e.RowIndex].Value.ToString();

			EntityAttribute attrib = entity[attributeKey]; // Store Entity Attribute reference
			
			if (cell.Value == null) { attrib.Delete(); /* User has erased entity attribute value */ } else {
				string value = cell.Value.ToString().Trim(); // Remove forward and backward whitespace

				if (String.IsNullOrWhiteSpace(value)) attrib.Delete(); else {
					if (value.First() == '[' && value.Last() == ']') {
						#region IsAsset_Implement
						string Id = value.Substring(1, value.Length - 2);

						bool existsGlobally = GV.MapZone.globalAssets.ContainsKey(Id);
						bool existsLocally = zone.assets.ContainsKey(Id); // Within zone

						if (existsGlobally || existsLocally) {
							Asset asset = (existsGlobally) ? GV.MapZone.globalAssets[Id] : zone.assets[Id];

							if (asset.TypesMatch(attrib.Type)) attrib.Value = asset;
							else {
								MessageBox.Show(
									$"Cannot Assign Asset Of Type '{asset.assetType}' to Attribute Of Type '{attrib.Type}'",
									"error", MessageBoxButtons.OK, MessageBoxIcon.Error // Alert that change cannot be done
								);

								cell.Value = valueBefore_DataViewTableValueColumnChanged; // Reset to before val
							}
						} else {
							MessageBox.Show($"Couldn't Find Asset With Name '{Id}'", "error", MessageBoxButtons.OK);

							cell.Value = valueBefore_DataViewTableValueColumnChanged; // Reset to before val
						}
						#endregion
					} else {
						try { attrib.Value = Converters.StringToAssetValue(attrib.Type, value); } catch {
							MessageBox.Show(
								$"Couldn't Convert '{value}' To Object Of Type '{attrib.Type}'",
								"error", MessageBoxButtons.OK // Display that new value could not be done
							);

							cell.Value = valueBefore_DataViewTableValueColumnChanged; // Reset to before val
						}
					}

					try {
						if (value.First() == '[' && value.Last() == ']') {
							#region IsAsset_Implement
							string Id = value.Substring(1, value.Length - 1);

							bool existsGlobally = GV.MapZone.globalAssets.ContainsKey(Id);
							bool existsLocally = zone.assets.ContainsKey(Id); // Within zone

							if (existsGlobally || existsLocally) {
								attrib.Value = (existsGlobally) ? GV.MapZone.globalAssets[Id] : zone.assets[Id];
							} else {

							}
							#endregion
						} else attrib.Value = Converters.StringToAssetValue(attrib.Type, value);
					} catch {
						MessageBox.Show(
							$"Couldn't Convert '{value}' To Object Of Type '{attrib.Type}'",
							"error", MessageBoxButtons.OK // Display that new value could not be done
						);

						cell.Value = valueBefore_DataViewTableValueColumnChanged; // Reset to before val
					}
				}
			}
		}

		#region PublicDialogMethods
		public static EditEntityViewResultsContainer RunForTemplateEntity(GameEntity tile, Zone zone) {
			EditEntityView view = new EditEntityView(tile, zone);
			view.tileIDTextBox.ReadOnly = true; // Leave Blank

			view.ShowDialog(); // Display form instance as dialog

			return new EditEntityViewResultsContainer() {
				state		  = view.dialogReturnState,
				updatedEntity = view.entity,
				id			  = String.Empty
			};
		}

		public static EditEntityViewResultsContainer RunForExistingEntity(string id, GameEntity tile, Zone zone) {
			EditEntityView view = new EditEntityView(tile, zone);
			view.tileIDTextBox.Text = id; // Set id text

			view.ShowDialog(); // Display form instance as dialog

			return new EditEntityViewResultsContainer() {
				state		  = view.dialogReturnState,
				updatedEntity = view.entity,
				id			  = view.tileIDTextBox.Text.Trim()
			};
		}
		#endregion

		private void EditExistingEntityView_Load(object sender, EventArgs e) {

		}

		private void EditExistingEntityView_Shown(object sender, EventArgs e) {

		}

		#region ButtonClickEventHandlers
		private void resetButton_Click(object sender, EventArgs e) {
			entity = backupEntity.Clone() as GameEntity;
			SetEntityAttributeViewItems(backupEntity);
		}

		private void saveButton_Click(object sender, EventArgs e) {
			dialogReturnState = ReturnState.Changed; // State
			this.Close(); // Close form after values changed
		}

		/// <summary>Handler for close button press</summary>
		private void cancelButton_Click(object sender, EventArgs e) {
			dialogReturnState = ReturnState.Cancel; // State
			this.Close(); // Close form after values changed
		}

		/// <summary>Clears contents of data grid views value</summary>
		private void clearAllButton_Click(object sender, EventArgs e) {
			foreach (String key in entity.GetEntityAttributes()) {
				entity[key].Delete(); // Delete value assigned to attr
			}

			SetEntityAttributeViewItems(entity); // From current
		}

		private void deleteTileButton_Click(object sender, EventArgs e) {
			dialogReturnState = ReturnState.Delete; // State
			this.Close(); // Close form after values changed
		}
		#endregion

		/// <summary>Builds data grid view and adds columns</summary>
		private void BuildEntityAttributeView() {
			dataViewTable = new DataGridView() {
				MultiSelect				 = false, 
				ColumnHeadersHeight		 = 45,
				RowHeadersVisible		 = false,
				AllowUserToAddRows		 = false,
				BackgroundColor			 = Color.White,
				BorderStyle				 = BorderStyle.None,
				AllowUserToResizeColumns = false,
				MinimumSize				 = attributesPanel.Size,
				RowHeadersBorderStyle	 = DataGridViewHeaderBorderStyle.Sunken
			};

			#region ColumnDefintions
			var nameColumn  = new DataGridViewColumn() { HeaderText = "Name",  Name = "Attribute Name", Width = (int)(attributesPanel.Width / 2.5) };
			var typeColumn  = new DataGridViewColumn() { HeaderText = "Type",  Name = "Attribute Type", Width = (int)(attributesPanel.Width / 4.5) };
			var valueColumn = new DataGridViewColumn() { HeaderText = "Value", Name = "Value",          Width = attributesPanel.Width - (nameColumn.Width + typeColumn.Width + 3) };

			foreach (DataGridViewColumn column in new DataGridViewColumn[] { nameColumn, typeColumn, valueColumn }) {
				column.CellTemplate = new DataGridViewTextBoxCell();
				column.Resizable = DataGridViewTriState.True;
				column.Visible = true;
				dataViewTable.Columns.Add(column);
			}
			#endregion

			attributesPanel.Controls.Add(dataViewTable);
		} // Complete

		/// <summary>Reads data from tile entity and translates it to data grid</summary>
		private void SetEntityAttributeViewItems(GameEntity entity) {
			dataViewTable.Rows.Clear(); // int rowCounter = 0

			foreach (string key in entity.GetEntityAttributes()) {
				EntityAttribute attribute = entity[key]; // Get Entity Attribute

				Type type = attribute.Type; // Get Type Of Current Entity Attribute

				String attrType = type.ToString().Split('.').Last(); // Convert Attribute Type To Displayable String Value
				String value = (attribute.IsAssigned) ? attribute.ToFileContents() : "";
				
				dataViewTable.Rows.Add(key, attrType, value); // Add new row to table indicating entity value
			}
		}

		private void EditEntityView_Resize(object sender, EventArgs e) {
			Size deltaSize = Size - sizeBeforeResize; // Change in size
			sizeBeforeResize = Size; // Store new size, before resizing

			attributesPanel.Width		   += deltaSize.Width;
			dataViewTable.Size			   += deltaSize;
			dataViewTable.Columns[2].Width += deltaSize.Width;
		} 

		private void EditEntityView_FormClosing(object sender, FormClosingEventArgs e) {
			
		}

		string valueBefore_DataViewTableValueColumnChanged = String.Empty;

		private ReturnState dialogReturnState = ReturnState.Cancel;

		private GameEntity entity, backupEntity;

		public DataGridView dataViewTable;

		private Size sizeBeforeResize;

		private Zone zone;
	}
}
