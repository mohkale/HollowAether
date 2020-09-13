namespace HollowAether.Lib.Forms.LevelEditor {
	partial class TileMap {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.blockHeightLabel = new System.Windows.Forms.Label();
			this.blockHeightTextBox = new System.Windows.Forms.TextBox();
			this.blockWidthLabel = new System.Windows.Forms.Label();
			this.blockWidthTextBox = new System.Windows.Forms.TextBox();
			this.textureDisplay = new System.Windows.Forms.ComboBox();
			this.zoomInButton = new System.Windows.Forms.Button();
			this.zoomOutButton = new System.Windows.Forms.Button();
			this.canvasRegionPanel = new System.Windows.Forms.Panel();
			this.SuspendLayout();
			// 
			// blockHeightLabel
			// 
			this.blockHeightLabel.AutoSize = true;
			this.blockHeightLabel.Location = new System.Drawing.Point(456, 15);
			this.blockHeightLabel.Name = "blockHeightLabel";
			this.blockHeightLabel.Size = new System.Drawing.Size(67, 20);
			this.blockHeightLabel.TabIndex = 10;
			this.blockHeightLabel.Text = "BHeight";
			// 
			// blockHeightTextBox
			// 
			this.blockHeightTextBox.Location = new System.Drawing.Point(529, 12);
			this.blockHeightTextBox.Name = "blockHeightTextBox";
			this.blockHeightTextBox.Size = new System.Drawing.Size(72, 26);
			this.blockHeightTextBox.TabIndex = 9;
			this.blockHeightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.blockHeightTextBox.TextChanged += new System.EventHandler(this.blockHeightTextBox_TextChanged);
			// 
			// blockWidthLabel
			// 
			this.blockWidthLabel.AutoSize = true;
			this.blockWidthLabel.Location = new System.Drawing.Point(311, 15);
			this.blockWidthLabel.Name = "blockWidthLabel";
			this.blockWidthLabel.Size = new System.Drawing.Size(61, 20);
			this.blockWidthLabel.TabIndex = 8;
			this.blockWidthLabel.Text = "BWidth";
			// 
			// blockWidthTextBox
			// 
			this.blockWidthTextBox.Location = new System.Drawing.Point(378, 12);
			this.blockWidthTextBox.Name = "blockWidthTextBox";
			this.blockWidthTextBox.Size = new System.Drawing.Size(72, 26);
			this.blockWidthTextBox.TabIndex = 7;
			this.blockWidthTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.blockWidthTextBox.TextChanged += new System.EventHandler(this.blockWidthTextBox_TextChanged);
			// 
			// textureDisplay
			// 
			this.textureDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.textureDisplay.FormattingEnabled = true;
			this.textureDisplay.Location = new System.Drawing.Point(12, 12);
			this.textureDisplay.Name = "textureDisplay";
			this.textureDisplay.Size = new System.Drawing.Size(291, 28);
			this.textureDisplay.TabIndex = 6;
			this.textureDisplay.TextChanged += new System.EventHandler(this.textureDisplay_TextChanged);
			// 
			// zoomInButton
			// 
			this.zoomInButton.Location = new System.Drawing.Point(12, 47);
			this.zoomInButton.Name = "zoomInButton";
			this.zoomInButton.Size = new System.Drawing.Size(31, 180);
			this.zoomInButton.TabIndex = 11;
			this.zoomInButton.Text = "+";
			this.zoomInButton.UseVisualStyleBackColor = true;
			this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
			// 
			// zoomOutButton
			// 
			this.zoomOutButton.Location = new System.Drawing.Point(12, 235);
			this.zoomOutButton.Name = "zoomOutButton";
			this.zoomOutButton.Size = new System.Drawing.Size(31, 180);
			this.zoomOutButton.TabIndex = 11;
			this.zoomOutButton.Text = "-";
			this.zoomOutButton.UseVisualStyleBackColor = true;
			this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
			// 
			// canvasRegionPanel
			// 
			this.canvasRegionPanel.AutoScroll = true;
			this.canvasRegionPanel.BackColor = System.Drawing.Color.Black;
			this.canvasRegionPanel.Location = new System.Drawing.Point(50, 47);
			this.canvasRegionPanel.MinimumSize = new System.Drawing.Size(551, 365);
			this.canvasRegionPanel.Name = "canvasRegionPanel";
			this.canvasRegionPanel.Size = new System.Drawing.Size(551, 365);
			this.canvasRegionPanel.TabIndex = 12;
			// 
			// TileMap2
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(613, 424);
			this.Controls.Add(this.canvasRegionPanel);
			this.Controls.Add(this.zoomOutButton);
			this.Controls.Add(this.zoomInButton);
			this.Controls.Add(this.blockHeightLabel);
			this.Controls.Add(this.blockHeightTextBox);
			this.Controls.Add(this.blockWidthLabel);
			this.Controls.Add(this.blockWidthTextBox);
			this.Controls.Add(this.textureDisplay);
			this.MinimumSize = new System.Drawing.Size(635, 480);
			this.Name = "TileMap2";
			this.Text = "Tile Map";
			this.Load += new System.EventHandler(this.TileMap2_Load);
			this.Resize += new System.EventHandler(this.TileMap2_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label blockHeightLabel;
		private System.Windows.Forms.TextBox blockHeightTextBox;
		private System.Windows.Forms.Label blockWidthLabel;
		private System.Windows.Forms.TextBox blockWidthTextBox;
		private System.Windows.Forms.ComboBox textureDisplay;
		private System.Windows.Forms.Button zoomInButton;
		private System.Windows.Forms.Button zoomOutButton;
		private System.Windows.Forms.Panel canvasRegionPanel;
	}
}