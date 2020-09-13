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

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class SetZoneDimensions : Form {
		SetZoneDimensions(string zonePath, int width, int height) {
			InitializeComponent();

			widthTextBox.Text = width.ToString();
			heightTextBox.Text = height.ToString();
			zoneTextBox.Text = zonePath;
		}

		private void SetZoneDimensions_Load(object sender, EventArgs e) {

		}

		public static Tuple<int, int> Run(string zonePath, int width, int height) {
			SetZoneDimensions dimensions = new SetZoneDimensions(zonePath, width, height);

			DialogResult result = dimensions.ShowDialog(); // Display form instance as a dialog

			int? newWidth = null, newHeight = null; // To store new variables

			if (result == DialogResult.OK) {
				newWidth  = GV.Misc.StringToInt(dimensions.widthTextBox.Text);
				newHeight = GV.Misc.StringToInt(dimensions.heightTextBox.Text);

				if (!newWidth.HasValue)  DisplayMessagebox(dimensions.widthTextBox.Text,  "Width");
				if (!newHeight.HasValue) DisplayMessagebox(dimensions.heightTextBox.Text, "Height");
			}

			return new Tuple<int, int>(
				(newWidth.HasValue)  ? newWidth.Value  : width,
				(newHeight.HasValue) ? newHeight.Value : height
			);
		}

		private static void DisplayMessagebox(string text, string attrib) {
			MessageBox.Show($"Couldn't Convert {attrib} Attribute '{text}' To a Number", "Conversion Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}

		private void saveButton_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK; this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.Cancel; this.Close();
		}

		private void SetZoneDimensions_FormClosing(object sender, FormClosingEventArgs e) {
			if (DialogResult != DialogResult.OK && DialogResult != DialogResult.Cancel) {
				DialogResult = DialogResult.Cancel; // Just in case, not set to value
			}
		}
	}
}
