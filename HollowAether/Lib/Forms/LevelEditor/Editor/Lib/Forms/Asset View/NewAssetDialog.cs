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
	public partial class NewAssetDialog : Form {
		public NewAssetDialog() {
			InitializeComponent();

			typeComboBox.Items.AddRange(new string[] {
				"String", "Animation", "Vector", "Integer", "Float", "Bool"
			});

			typeComboBox.SelectedIndex = 0;
		}

		private void saveButton_Click(object sender, EventArgs e) {
			DialogResult = DialogResult.OK;
			Close(); // Closes form instance
		}

		private void cancelButton_Click(object sender, EventArgs e) {
			Close();
		}

		private void NewAssetDialog_FormClosing(object sender, FormClosingEventArgs e) {
			if (DialogResult != DialogResult.OK) DialogResult = DialogResult.Cancel;
		}

		public string AssetType { get { return typeComboBox.SelectedItem.ToString(); } }

		public string AssetID { get { return idTextBox.Text; } }

		public string AssetValue { get { return valueTextBox.Text; } }
	}
}
