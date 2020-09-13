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
	public partial class TileMap : Form {
		public TileMap() {
			InitializeComponent();

			sizeBeforeResizing = Size; // Store initial size of tile map at form construction

			gapBetweenZoomInAndZoomOutButtons = zoomOutButton.Top - zoomInButton.Bottom; // Store gap region

			textureDisplay.MouseWheel += (s, e) => { ((HandledMouseEventArgs)e).Handled = true; };

			FormClosing += (s, e) => { if (e.CloseReason != CloseReason.ApplicationExitCall) e.Cancel = true; closedOnce = true; };

			canvas.DrawToCanvas += (s, e) => {
				//Console.WriteLine(canvas.SelectRect);
			};
		}

		private void TileMap2_Load(object sender, EventArgs e) {
			canvas.Width = canvasRegionPanel.Width;
			canvas.Height = canvasRegionPanel.Height;
			
			canvas.DrawToCanvas += Canvas_DrawToCanvas;
			canvasRegionPanel.Controls.Add(canvas);

			textureDisplay.Items.AddRange(GV.LevelEditor.textures.Keys.ToArray()); // Add all textures to combo box
			textureDisplay.SelectedIndex = 0;                    // Select first texture referenced by the combobox 

			blockWidthTextBox.Text = canvas.GridWidth.ToString();
			blockHeightTextBox.Text = canvas.GridHeight.ToString();
		}

		private void Canvas_DrawToCanvas(object sender, PaintEventArgs e) {
			if (CurrentImage != null) e.Graphics.DrawImage(CurrentImage, Point.Empty);
		}

		private void TileMap2_Resize(object sender, EventArgs e) {
			Size deltaSize = Size - sizeBeforeResizing;

			canvasRegionPanel.Size += deltaSize;
			textureDisplay.Width += deltaSize.Width;

			blockWidthLabel.Location    = new Point(blockWidthLabel.Location.X    + deltaSize.Width, blockWidthLabel.Location.Y);
			blockHeightLabel.Location   = new Point(blockHeightLabel.Location.X   + deltaSize.Width, blockHeightLabel.Location.Y);
			blockWidthTextBox.Location  = new Point(blockWidthTextBox.Location.X  + deltaSize.Width, blockWidthTextBox.Location.Y);
			blockHeightTextBox.Location = new Point(blockHeightTextBox.Location.X + deltaSize.Width, blockHeightTextBox.Location.Y);

			if (deltaSize.Height != 0) {
				int heightDifferential = canvasRegionPanel.Bottom - zoomInButton.Location.Y; // Displacement from top to bottom of column
				
				zoomInButton.Height = zoomOutButton.Height = (heightDifferential - gapBetweenZoomInAndZoomOutButtons) / 2; // actual
				zoomOutButton.Location = new Point(zoomOutButton.Location.X, zoomInButton.Bottom + gapBetweenZoomInAndZoomOutButtons);
			}

			sizeBeforeResizing = Size; // Store new size
		}

		private void zoomInButton_Click(object sender, EventArgs e) {
			float newZoom = GV.BasicMath.Clamp(canvas.ZoomX+1, 0.5f, 15f);
			canvas.Zoom = new SizeF(newZoom, newZoom); // Set new zoom
		}

		private void zoomOutButton_Click(object sender, EventArgs e) {
			float newZoom = GV.BasicMath.Clamp(canvas.ZoomX - 1, 0.5f, 15f);
			canvas.Zoom = new SizeF(newZoom, newZoom); // Set new zoom
		}

		private void textureDisplay_TextChanged(object sender, EventArgs e) {
			SetCurrentImage(GetTextValueFromComboBox());
		}

		private void blockWidthTextBox_TextChanged(object sender, EventArgs e) {
			int? value = GV.Misc.StringToInt(blockWidthTextBox.Text);

			if (value.HasValue) { canvas.GridWidth = value.Value; canvas.Deselect(); } else {
				MessageBox.Show("Given Value Could Not Be Turned To An Int", "Value Error");
				blockWidthTextBox.Text = canvas.GridWidth.ToString(); // Set text to current
			}
		}

		private void blockHeightTextBox_TextChanged(object sender, EventArgs e) {
			int? value = GV.Misc.StringToInt(blockHeightTextBox.Text);

			if (value.HasValue) { canvas.GridHeight = value.Value; canvas.Deselect(); } else {
				MessageBox.Show("Given Value Could Not Be Turned To An Int", "Value Error");
				blockHeightTextBox.Text = canvas.GridHeight.ToString(); // Set text to current
			}
		}

		private string GetTextValueFromComboBox() {
			return textureDisplay.SelectedItem.ToString();
		}

		public void ChangeImage(String textureKey) {
			SetCurrentImage(textureKey);
	
			textureDisplay.SelectedText = textureKey;
		}

		public new void Show() {
			base.Show();

			if (closedOnce) ReShown(this);
		}

		private void SetCurrentImage(string textureKey) {
			try {
				CurrentImage		   = GV.LevelEditor.textures[textureKey].Item1;
				CurrentImageTextureKey = textureKey;
				canvas.Width		   = CurrentImage.Width;
				canvas.Height		   = CurrentImage.Height;
				canvas.Zoom			   = canvas.Zoom; // Fixes Bug
				canvas.Deselect();
			} catch (KeyNotFoundException e) {
				CurrentImage = null;
			} finally {
				canvas.Draw();
			}
		}

		private int gapBetweenZoomInAndZoomOutButtons;
		private Size sizeBeforeResizing; // When resizing

		public CanvasRegionBox canvas = new CanvasRegionBox() {
			GridSize = new SizeF(32, 32),
			DrawGrid = true, DrawBorder = true,
			SelectOption = CanvasRegionBox.SelectType.GridDrag,
			dragableSelectionRectsAreVolatile = false
		};

		private bool closedOnce = false;

		public event Action<object> ReShown = (s) => { };

		public Image CurrentImage { get; private set; }

		public String CurrentImageTextureKey { get; private set; }
	}
}
