using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using HollowAether.Lib.GAssets;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class EditFrameDialog : Form {
		public EditFrameDialog(AnimationSequence seq, int index) {
			InitializeComponent();
			sequence = seq; // Store reference
			frameIndex = index; // Store index

			sizeBeforeResize = MinimumSize;
		}

		private void EditFrameDialog_Load(object sender, EventArgs e) {
			#region TileBoxDef
			tileBox = new TileBox() {
				Width  = tilePanel.Width,
				Height = tilePanel.Height
			};

			tilePanel.Controls.Add(tileBox);
			#endregion

			textureComboBox.Items.AddRange(GV.LevelEditor.textures.Keys.ToArray());
			textureComboBox.SelectedIndex = 0; // Select First Texture By Default

			resetButton.PerformClick();
		}

		public EditFrameDialog(AnimationSequence seq, int index, string currentTexture) : this(seq, index) {
			textureComboBox.SelectedItem = currentTexture;
		}

		private Frame TextBoxesToFrame() {
			int x       = CheckGetInt(xTextBox.Text);
			int y       = CheckGetInt(yTextBox.Text);
			int width   = CheckGetInt(widthTextBox.Text);
			int height  = CheckGetInt(heightTextBox.Text);
			int bWidth  = CheckGetInt(blockWidthTextBox.Text);
			int bHeight = CheckGetInt(blockHeightTextBox.Text);
			int count   = CheckGetInt(countTextBox.Text);

			return new Frame(x, y, width, height, bWidth, bHeight, count);
		}

		private void EditFrameDialog_Resize(object sender, EventArgs e) {
			Size deltaSize = Size - sizeBeforeResize;
			sizeBeforeResize = Size; // New size b4

			variablesGroupBox.Location += deltaSize;
			tilePanel.Size             += deltaSize;
			tileBox.Size			   += deltaSize;
		}

		private void comboBox1_TextChanged(object sender, EventArgs e) {
			String textureKey = textureComboBox.SelectedItem.ToString();

			tileBox.Texture = GV.LevelEditor.textures[textureKey].Item1;
		}

		private void saveButton_Click(object sender, EventArgs e) {
			sequence.Frames[frameIndex] = TextBoxesToFrame();
			Close(); // Close form once updated values have been saved
		}

		private void resetButton_Click(object sender, EventArgs e) {
			Frame f = frame; // Resets all values in displayed form

			xTextBox.Text           = f.xPosition.ToString();
			yTextBox.Text           = f.yPosition.ToString();
			widthTextBox.Text       = f.frameWidth.ToString();
			heightTextBox.Text      = f.frameHeight.ToString();
			blockWidthTextBox.Text  = f.blockWidth.ToString();
			blockHeightTextBox.Text = f.blockHeight.ToString();
			countTextBox.Text       = f.RunCount.ToString();
		}

		private void cancelButton_Click(object sender, EventArgs e) { Close(); }

		private void showTileMapButton_Click(object sender, EventArgs e) {
			GV.LevelEditor.tileMap.Show();

			String textureKey = textureComboBox.SelectedItem.ToString();

			GV.LevelEditor.tileMap.ChangeImage(textureKey);
		}

		private int CheckGetInt(string text) {
			int? value = GV.Misc.StringToInt(text);

			if (value.HasValue) return value.Value; else {
				return 0; // 0 = default value for error inputs
			}
		}

		private void xTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void yTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void widthTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void heightTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void blockWidthTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void blockHeightTextBox_TextChanged(object sender, EventArgs e) {
			tileBox.AssignFrame(TextBoxesToFrame());
		}

		private void countTextBox_TextChanged(object sender, EventArgs e) {
			// No Need To Handle Anything For This Event Handler
		}

		private Size sizeBeforeResize;

		private Frame frame { get { return sequence[frameIndex]; } }

		private AnimationSequence sequence;

		private TileBox tileBox;

		private int frameIndex = -1;
	}
}
