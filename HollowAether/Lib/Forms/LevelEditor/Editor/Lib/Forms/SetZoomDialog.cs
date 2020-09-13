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
using M = HollowAether.Lib.GlobalVars.BasicMath;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class SetZoomDialog : Form {
		SetZoomDialog(float current, float min, float max) {
			InitializeComponent();

			minZoomTextBox.Text = min.ToString();
			maxZoomTextBox.Text = max.ToString();
			zoomTextBox.Text = current.ToString();
		}

		private void SetZoomDialog_Load(object sender, EventArgs e) {

		}

		public static float GetZoom(float current, float min, float max) {
			SetZoomDialog dialog = new SetZoomDialog(current, min, max);

			DialogResult result = dialog.ShowDialog(); // Get result

			if (result != DialogResult.OK) return current; else {
				float? currentValue = GV.Misc.StringToFloat(dialog.zoomTextBox.Text);

				if (currentValue.HasValue) return M.Clamp(currentValue.Value, min, max); else {
					MessageBox.Show(
						$"Couldn't cast value to float, zoom set to {current}", "Error",
						MessageBoxButtons.OKCancel, MessageBoxIcon.Error // Show Msg
					);
				}

				return current; // return value passed to method at start
			}
		}

		private void saveButton_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			Close(); // Close form after click
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
