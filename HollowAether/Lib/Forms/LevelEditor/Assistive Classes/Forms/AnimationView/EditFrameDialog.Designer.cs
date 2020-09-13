namespace HollowAether.Lib.Forms.LevelEditor {
	partial class EditFrameDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditFrameDialog));
			this.tilePanel = new System.Windows.Forms.Panel();
			this.variablesGroupBox = new System.Windows.Forms.GroupBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.resetButton = new System.Windows.Forms.Button();
			this.showTileMapButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.textureComboBox = new System.Windows.Forms.ComboBox();
			this.countTextBox = new System.Windows.Forms.TextBox();
			this.blockHeightTextBox = new System.Windows.Forms.TextBox();
			this.heightTextBox = new System.Windows.Forms.TextBox();
			this.yTextBox = new System.Windows.Forms.TextBox();
			this.countLabel = new System.Windows.Forms.Label();
			this.blockHeightLabel = new System.Windows.Forms.Label();
			this.heightLabel = new System.Windows.Forms.Label();
			this.blockWidthTextBox = new System.Windows.Forms.TextBox();
			this.widthTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.yLabel = new System.Windows.Forms.Label();
			this.widthLabel = new System.Windows.Forms.Label();
			this.xTextBox = new System.Windows.Forms.TextBox();
			this.xLabel = new System.Windows.Forms.Label();
			this.variablesGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// tilePanel
			// 
			this.tilePanel.Location = new System.Drawing.Point(12, 12);
			this.tilePanel.Name = "tilePanel";
			this.tilePanel.Size = new System.Drawing.Size(454, 454);
			this.tilePanel.TabIndex = 0;
			// 
			// variablesGroupBox
			// 
			this.variablesGroupBox.Controls.Add(this.cancelButton);
			this.variablesGroupBox.Controls.Add(this.resetButton);
			this.variablesGroupBox.Controls.Add(this.showTileMapButton);
			this.variablesGroupBox.Controls.Add(this.saveButton);
			this.variablesGroupBox.Controls.Add(this.textureComboBox);
			this.variablesGroupBox.Controls.Add(this.countTextBox);
			this.variablesGroupBox.Controls.Add(this.blockHeightTextBox);
			this.variablesGroupBox.Controls.Add(this.heightTextBox);
			this.variablesGroupBox.Controls.Add(this.yTextBox);
			this.variablesGroupBox.Controls.Add(this.countLabel);
			this.variablesGroupBox.Controls.Add(this.blockHeightLabel);
			this.variablesGroupBox.Controls.Add(this.heightLabel);
			this.variablesGroupBox.Controls.Add(this.blockWidthTextBox);
			this.variablesGroupBox.Controls.Add(this.widthTextBox);
			this.variablesGroupBox.Controls.Add(this.label1);
			this.variablesGroupBox.Controls.Add(this.yLabel);
			this.variablesGroupBox.Controls.Add(this.widthLabel);
			this.variablesGroupBox.Controls.Add(this.xTextBox);
			this.variablesGroupBox.Controls.Add(this.xLabel);
			this.variablesGroupBox.Location = new System.Drawing.Point(473, 12);
			this.variablesGroupBox.Name = "variablesGroupBox";
			this.variablesGroupBox.Size = new System.Drawing.Size(200, 454);
			this.variablesGroupBox.TabIndex = 1;
			this.variablesGroupBox.TabStop = false;
			this.variablesGroupBox.Text = "Variables";
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(6, 381);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(181, 33);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// resetButton
			// 
			this.resetButton.Location = new System.Drawing.Point(7, 342);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(181, 33);
			this.resetButton.TabIndex = 5;
			this.resetButton.Text = "Reset";
			this.resetButton.UseVisualStyleBackColor = true;
			this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// showTileMapButton
			// 
			this.showTileMapButton.Location = new System.Drawing.Point(10, 25);
			this.showTileMapButton.Name = "showTileMapButton";
			this.showTileMapButton.Size = new System.Drawing.Size(181, 33);
			this.showTileMapButton.TabIndex = 5;
			this.showTileMapButton.Text = "Show TileMap";
			this.showTileMapButton.UseVisualStyleBackColor = true;
			this.showTileMapButton.Click += new System.EventHandler(this.showTileMapButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(6, 303);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(181, 33);
			this.saveButton.TabIndex = 5;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// textureComboBox
			// 
			this.textureComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.textureComboBox.FormattingEnabled = true;
			this.textureComboBox.Location = new System.Drawing.Point(6, 420);
			this.textureComboBox.Name = "textureComboBox";
			this.textureComboBox.Size = new System.Drawing.Size(182, 28);
			this.textureComboBox.TabIndex = 4;
			this.textureComboBox.TextChanged += new System.EventHandler(this.comboBox1_TextChanged);
			// 
			// countTextBox
			// 
			this.countTextBox.Location = new System.Drawing.Point(61, 260);
			this.countTextBox.Name = "countTextBox";
			this.countTextBox.Size = new System.Drawing.Size(131, 26);
			this.countTextBox.TabIndex = 3;
			this.countTextBox.Text = "0";
			this.countTextBox.TextChanged += new System.EventHandler(this.countTextBox_TextChanged);
			// 
			// blockHeightTextBox
			// 
			this.blockHeightTextBox.Location = new System.Drawing.Point(62, 228);
			this.blockHeightTextBox.Name = "blockHeightTextBox";
			this.blockHeightTextBox.Size = new System.Drawing.Size(131, 26);
			this.blockHeightTextBox.TabIndex = 3;
			this.blockHeightTextBox.Text = "0";
			this.blockHeightTextBox.TextChanged += new System.EventHandler(this.blockHeightTextBox_TextChanged);
			// 
			// heightTextBox
			// 
			this.heightTextBox.Location = new System.Drawing.Point(61, 164);
			this.heightTextBox.Name = "heightTextBox";
			this.heightTextBox.Size = new System.Drawing.Size(131, 26);
			this.heightTextBox.TabIndex = 3;
			this.heightTextBox.Text = "0";
			this.heightTextBox.TextChanged += new System.EventHandler(this.heightTextBox_TextChanged);
			// 
			// yTextBox
			// 
			this.yTextBox.Location = new System.Drawing.Point(62, 100);
			this.yTextBox.Name = "yTextBox";
			this.yTextBox.Size = new System.Drawing.Size(131, 26);
			this.yTextBox.TabIndex = 3;
			this.yTextBox.Text = "0";
			this.yTextBox.TextChanged += new System.EventHandler(this.yTextBox_TextChanged);
			// 
			// countLabel
			// 
			this.countLabel.AutoSize = true;
			this.countLabel.Location = new System.Drawing.Point(1, 263);
			this.countLabel.Name = "countLabel";
			this.countLabel.Size = new System.Drawing.Size(52, 20);
			this.countLabel.TabIndex = 2;
			this.countLabel.Text = "Count";
			// 
			// blockHeightLabel
			// 
			this.blockHeightLabel.AutoSize = true;
			this.blockHeightLabel.Location = new System.Drawing.Point(-1, 231);
			this.blockHeightLabel.Name = "blockHeightLabel";
			this.blockHeightLabel.Size = new System.Drawing.Size(67, 20);
			this.blockHeightLabel.TabIndex = 2;
			this.blockHeightLabel.Text = "BHeight";
			// 
			// heightLabel
			// 
			this.heightLabel.AutoSize = true;
			this.heightLabel.Location = new System.Drawing.Point(1, 167);
			this.heightLabel.Name = "heightLabel";
			this.heightLabel.Size = new System.Drawing.Size(56, 20);
			this.heightLabel.TabIndex = 2;
			this.heightLabel.Text = "Height";
			// 
			// blockWidthTextBox
			// 
			this.blockWidthTextBox.Location = new System.Drawing.Point(62, 196);
			this.blockWidthTextBox.Name = "blockWidthTextBox";
			this.blockWidthTextBox.Size = new System.Drawing.Size(131, 26);
			this.blockWidthTextBox.TabIndex = 1;
			this.blockWidthTextBox.Text = "0";
			this.blockWidthTextBox.TextChanged += new System.EventHandler(this.blockWidthTextBox_TextChanged);
			// 
			// widthTextBox
			// 
			this.widthTextBox.Location = new System.Drawing.Point(61, 132);
			this.widthTextBox.Name = "widthTextBox";
			this.widthTextBox.Size = new System.Drawing.Size(131, 26);
			this.widthTextBox.TabIndex = 1;
			this.widthTextBox.Text = "0";
			this.widthTextBox.TextChanged += new System.EventHandler(this.widthTextBox_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(1, 199);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "BWidth";
			// 
			// yLabel
			// 
			this.yLabel.AutoSize = true;
			this.yLabel.Location = new System.Drawing.Point(2, 103);
			this.yLabel.Name = "yLabel";
			this.yLabel.Size = new System.Drawing.Size(20, 20);
			this.yLabel.TabIndex = 2;
			this.yLabel.Text = "Y";
			// 
			// widthLabel
			// 
			this.widthLabel.AutoSize = true;
			this.widthLabel.Location = new System.Drawing.Point(1, 135);
			this.widthLabel.Name = "widthLabel";
			this.widthLabel.Size = new System.Drawing.Size(50, 20);
			this.widthLabel.TabIndex = 0;
			this.widthLabel.Text = "Width";
			// 
			// xTextBox
			// 
			this.xTextBox.Location = new System.Drawing.Point(62, 68);
			this.xTextBox.Name = "xTextBox";
			this.xTextBox.Size = new System.Drawing.Size(131, 26);
			this.xTextBox.TabIndex = 1;
			this.xTextBox.Text = "0";
			this.xTextBox.TextChanged += new System.EventHandler(this.xTextBox_TextChanged);
			// 
			// xLabel
			// 
			this.xLabel.AutoSize = true;
			this.xLabel.Location = new System.Drawing.Point(2, 71);
			this.xLabel.Name = "xLabel";
			this.xLabel.Size = new System.Drawing.Size(20, 20);
			this.xLabel.TabIndex = 0;
			this.xLabel.Text = "X";
			// 
			// EditFrameDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(679, 481);
			this.Controls.Add(this.variablesGroupBox);
			this.Controls.Add(this.tilePanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(701, 537);
			this.Name = "EditFrameDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EditFrameDialog";
			this.Load += new System.EventHandler(this.EditFrameDialog_Load);
			this.Resize += new System.EventHandler(this.EditFrameDialog_Resize);
			this.variablesGroupBox.ResumeLayout(false);
			this.variablesGroupBox.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel tilePanel;
		private System.Windows.Forms.GroupBox variablesGroupBox;
		private System.Windows.Forms.TextBox heightTextBox;
		private System.Windows.Forms.TextBox yTextBox;
		private System.Windows.Forms.Label heightLabel;
		private System.Windows.Forms.TextBox widthTextBox;
		private System.Windows.Forms.Label yLabel;
		private System.Windows.Forms.Label widthLabel;
		private System.Windows.Forms.TextBox xTextBox;
		private System.Windows.Forms.Label xLabel;
		private System.Windows.Forms.ComboBox textureComboBox;
		private System.Windows.Forms.TextBox countTextBox;
		private System.Windows.Forms.Label countLabel;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button resetButton;
		private System.Windows.Forms.Button showTileMapButton;
		private System.Windows.Forms.TextBox blockHeightTextBox;
		private System.Windows.Forms.Label blockHeightLabel;
		private System.Windows.Forms.TextBox blockWidthTextBox;
		private System.Windows.Forms.Label label1;
	}
}