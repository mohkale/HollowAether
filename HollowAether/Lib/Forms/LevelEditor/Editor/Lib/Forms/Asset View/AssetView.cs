using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GV = HollowAether.Lib.GlobalVars;

using HollowAether.Lib.InputOutput;
using HollowAether.Lib.GAssets;
using HollowAether.Lib.MapZone;

using Converters = HollowAether.Lib.InputOutput.Parsers.Converters;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class AssetView : Form {
		public AssetView(Zone zone) {
			InitializeComponent();
			sizeBeforeResize = MinimumSize;
			Open = true; this.zone = zone;
		}

		private void AssetView_Load(object sender, EventArgs e) {
			viewComboBox.Items.AddRange(new string[] {
				"All", "Global", "Local"
			});

			saveButton.Click += (s, e2) => {
				ChangesMade = false;
			};

			#region Assets View Definition
			DataGridViewTextBoxColumn name = new DataGridViewTextBoxColumn() {
				ReadOnly = true, HeaderText = "Name", Width = 250
			};

			DataGridViewTextBoxColumn set = new DataGridViewTextBoxColumn() {
				ReadOnly = false, HeaderText = "Set", Width = 250
			};

			DataGridViewTextBoxColumn _default = new DataGridViewTextBoxColumn() {
				ReadOnly = true, HeaderText = "Default", Width = 250
			};

			DataGridViewTextBoxColumn type = new DataGridViewTextBoxColumn() {
				ReadOnly = true, HeaderText = "Type", // Takes up remaining space
				Width = assetsView.Width - name.Width - _default.Width - set.Width - 3
			};

			assetsView.CellValueChanged += AssetsView_CellValueChanged;

			assetsView.CellBeginEdit += AssetsView_CellBeginEdit;

			assetsView.Columns.AddRange(new DataGridViewColumn[] {
				name, type, _default, set // All columns
			});

			assetsView.ContextMenu = GetContextMenu();

			assetsView.MouseDown += AssetsView_MouseDown;

			assetsView.MouseUp += AssetsView_MouseUp;
			#endregion

			Controls.Add(assetsView); // Store assets view

			viewComboBox.SelectedIndex = 0;
		}

		private void AssetsView_MouseUp(object sender, MouseEventArgs e) {
			if (clickedMouseButton == MouseButtons.Right) {
				assetsView.ContextMenu.Show(this, e.Location, LeftRightAlignment.Right);
			}

			clickedMouseButton = null; // Reset to null
		}

		private void AssetsView_MouseDown(object sender, MouseEventArgs e) {
			clickedMouseButton = e.Button;
		}

		private MouseButtons? clickedMouseButton = null;

		private ContextMenu GetContextMenu() {
			MenuItem item = new MenuItem("Delete");

			item.Click += DeleteContextMenuItem_Click;

			ContextMenu menu = new ContextMenu(new MenuItem[] { item });

			return menu;
		}

		private void DeleteContextMenuItem_Click(object sender, EventArgs e) {
			if (assetsView.SelectedCells.Count > 0) {
				DataGridViewCell selectedCell = assetsView.SelectedCells[0];

				DialogResult result = MessageBox.Show(
					$"Are you sure you want to delete asset \"{selectedCell.Value}\"", 
					"Are You Sure?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning
				);

				if (result == DialogResult.Yes) {
					if (GV.MapZone.globalAssets.ContainsKey(selectedCell.Value.ToString())) {
						MessageBox.Show("Cannot Delete a Global Asset", "Forbidden", MessageBoxButtons.OK, MessageBoxIcon.Error);
					} else {
						Trace.DeleteAsset(selectedCell.Value.ToString());
						assetsView.Rows.RemoveAt(selectedCell.RowIndex);
					}
				}
			}
		}

		private void AssetView_FormClosing(object sender, FormClosingEventArgs e) {
			if (!ChangesMade) { Open = false; } else {
				DialogResult result = MessageBox.Show(
					"Would You Like To Save, Before Closing Asset View?",
					"Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning
				);

				if (result == DialogResult.Yes && ChangesMade) saveButton.PerformClick();

				if (result == DialogResult.Cancel) e.Cancel = true; else Open = false;
			}
		}

		private void AssetView_Resize(object sender, EventArgs e) {
			Size deltaSize = Size - sizeBeforeResize;

			assetsView.Size += deltaSize; // Change in size

			sizeBeforeResize = Size; // Current size
		}

		public static IEnumerable<Tuple<AssetTrace.AssetTraceArgs,  DataGridViewRow>> TraceToDatagridRows(AssetTrace trace) {
			foreach (AssetTrace.AssetTraceArgs args in trace.store) {
				yield return new Tuple<AssetTrace.AssetTraceArgs, DataGridViewRow>(
					args, AssetTraceArgsToDataGridViewRow(args) // Make Tuple
				);
			}
		}

		private static DataGridViewRow AssetTraceArgsToDataGridViewRow(AssetTrace.AssetTraceArgs args) {
			DataGridViewRow row = new DataGridViewRow();

			row.Cells.AddRange(new DataGridViewCell[] {
				new DataGridViewTextBoxCell() { Value = args.ID   },
				new DataGridViewTextBoxCell() { Value = args.Type },
			});

			row.Cells.Add(new DataGridViewTextBoxCell() { Value = (args.HasDefaultValue) ? args.DefaultValue : ""});
			row.Cells.Add(new DataGridViewTextBoxCell() { Value = (args.ValueAssigned)   ? args.Value        : ""});

			if (!args.HasDefaultValue) {
				row.Cells[2].Style.BackColor = Color.LightGray;
				row.Cells[2].Style.ForeColor = Color.DarkGray;
			}

			return row;
		}

		private DataGridViewRow[] GetAllDataGridRows() {
			return (from X in TraceToDatagridRows(Trace) select X.Item2).ToArray();
		}

		private DataGridViewRow[] GetAllGlobalDataGridRows() {
			return (from X in TraceToDatagridRows(Trace) where GV.MapZone.globalAssets.ContainsKey(X.Item1.ID) select X.Item2).ToArray();
		}

		private DataGridViewRow[] GetAllLocalDataGridRows() {
			return (from X in TraceToDatagridRows(Trace) where !GV.MapZone.globalAssets.ContainsKey(X.Item1.ID) select X.Item2).ToArray();
		}

		private void AssetsView_CellValueChanged(object sender, DataGridViewCellEventArgs e) {
			string id       = assetsView[0, e.RowIndex].Value.ToString();
			string type     = assetsView[1, e.RowIndex].Value.ToString();
			string newValue = assetsView[3, e.RowIndex].Value.ToString();

			if (newValue == valueBefore_AssetsView_CellValueChanged) return; // Cancel assignment

			if (String.IsNullOrWhiteSpace(newValue)) { /* Deleted */
				Trace.DeleteAssetValue(id); ChangesMade = true;
			} else {
				if (Converters.IsValidConversion(type, newValue)) {
					Trace.SetValue(id, newValue); ChangesMade = true;
				} else {
					MessageBox.Show(
						$"Couldn't Convert '{newValue}' To Instance Of Type '{type}'", // Failed
						"Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
					);

					assetsView[e.ColumnIndex, e.RowIndex].Value = valueBefore_AssetsView_CellValueChanged;
				}
			}
		}

		private void AssetsView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e) {
			valueBefore_AssetsView_CellValueChanged = assetsView[e.ColumnIndex, e.RowIndex].Value.ToString();
		}

		private void viewComboBox_TextChanged(object sender, EventArgs e) {
			assetsView.Rows.Clear(); // Remove all displayed controls

			switch (viewComboBox.Text) {
				case "All":    assetsView.Rows.AddRange(GetAllDataGridRows());       break;
				case "Global": assetsView.Rows.AddRange(GetAllGlobalDataGridRows()); break;
				case "Local":  assetsView.Rows.AddRange(GetAllLocalDataGridRows());  break;
			}
		}

		private void defineNewAssetButton_Click(object sender, EventArgs e) {
			NewAssetDialog dialog = new NewAssetDialog();
			DialogResult result = dialog.ShowDialog(); 

			if (result == DialogResult.OK) {
				string id = dialog.AssetID, type=dialog.AssetType, value=dialog.AssetValue;

				bool idInvalid   = String.IsNullOrWhiteSpace(id);
				bool valueExists = !String.IsNullOrWhiteSpace(value);

				if (!idInvalid) {
					if (!Trace.Exists(id)) {
						if (!valueExists) {
							Trace.AddEmptyAsset(id, type, definition: true); // No value assigned so create as empty

							ChangesMade = true;

							assetsView.Rows.Add(AssetTraceArgsToDataGridViewRow(Trace.store.Last()));
						} else if (Converters.IsValidConversion(type, value)) {
							Trace.AddDefineAsset(id, type, value); // Value assigned at creation

							ChangesMade = true;

							assetsView.Rows.Add(AssetTraceArgsToDataGridViewRow(Trace.store.Last()));
						} else {
							MessageBox.Show(
								$"Couldn't Convert '{value}' To Instance Of Type '{type}'",
								"Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
							);
						}
					} else { // Trace Exists
						MessageBox.Show(
							"Asset With Given ID Already Exists, Operation Cancelled",
							"Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
						);
					}
				} else { // Is Invalid
					MessageBox.Show(
						"Invalid Id Passed To Asset View, Operation Cancelled",
						"Operation Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation
					);
				}
			}
		}

		DataGridView assetsView = new DataGridView() {
			Location = new Point(13, 35),
			Size = new Size(1073, 304),
			ColumnHeadersHeight = 30,
			RowHeadersVisible = false,
			AllowUserToAddRows = false
		};

		private Size sizeBeforeResize;

		public static bool Open = false;

		private Zone zone;

		public bool ChangesMade { get; private set; } = false;

		private string valueBefore_AssetsView_CellValueChanged;

		public AssetTrace Trace { get { return zone.ZoneAssetTrace; } }
	}
}
