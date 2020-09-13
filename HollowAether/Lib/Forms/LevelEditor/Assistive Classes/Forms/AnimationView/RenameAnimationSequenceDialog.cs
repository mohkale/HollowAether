using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class RenameAnimationSequenceDialog : Form {
		public RenameAnimationSequenceDialog(string key) {
			InitializeComponent();

			defaultSequenceKey = key;

			resetButton_Click(this, null);
		}

		private void RenameAnimationSequenceDialog_Load(object sender, EventArgs e) {

		}

		public static Tuple<DialogResult, string, string> Run(string current) {
			RenameAnimationSequenceDialog dialog = new RenameAnimationSequenceDialog(current);

			DialogResult result = dialog.ShowDialog(); // Show dialog and get result

			string animContainer = dialog.animationContainerTextBox.Text;
			string sequenceName  = dialog.sequenceNameTextBox.Text;

			string returnVal = $"{animContainer}\\{sequenceName}".ToLower();

			if (result == DialogResult.OK) {
				bool typeInvalid = String.IsNullOrWhiteSpace(animContainer) || String.IsNullOrWhiteSpace(sequenceName);

				if (returnVal.ToLower() == current.ToLower()) result = DialogResult.Cancel; else if (typeInvalid) {
					MessageBox.Show("One or More Inputs Are Empty", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					result = DialogResult.Cancel; // Invalid entry, means cancel dialog
				}
			}

			return new Tuple<DialogResult, string, string>(result, current, returnVal);
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			//Close();
		}

		private void saveButton_Click(object sender, EventArgs e) {
			//Close();
		}

		private void resetButton_Click(object sender, EventArgs e) {
			animationContainerTextBox.Text = defaultSequenceKey.Split('\\').First();
			sequenceNameTextBox.Text       = defaultSequenceKey.Split('\\').Last();
		}

		private string defaultSequenceKey;
	}
}
