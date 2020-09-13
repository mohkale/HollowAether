namespace HollowAether.Lib.Forms.LevelEditor {
	partial class AssetView {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AssetView));
			this.viewComboBox = new System.Windows.Forms.ComboBox();
			this.defineNewAssetButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// viewComboBox
			// 
			this.viewComboBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.viewComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.viewComboBox.FormattingEnabled = true;
			this.viewComboBox.Location = new System.Drawing.Point(0, 0);
			this.viewComboBox.Name = "viewComboBox";
			this.viewComboBox.Size = new System.Drawing.Size(1098, 28);
			this.viewComboBox.TabIndex = 0;
			this.viewComboBox.TextChanged += new System.EventHandler(this.viewComboBox_TextChanged);
			// 
			// defineNewAssetButton
			// 
			this.defineNewAssetButton.Location = new System.Drawing.Point(12, 352);
			this.defineNewAssetButton.Name = "defineNewAssetButton";
			this.defineNewAssetButton.Size = new System.Drawing.Size(524, 30);
			this.defineNewAssetButton.TabIndex = 1;
			this.defineNewAssetButton.Text = "Define New Asset ";
			this.defineNewAssetButton.UseVisualStyleBackColor = true;
			this.defineNewAssetButton.Click += new System.EventHandler(this.defineNewAssetButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(562, 352);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(524, 30);
			this.saveButton.TabIndex = 1;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			// 
			// AssetView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1098, 394);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.defineNewAssetButton);
			this.Controls.Add(this.viewComboBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(1120, 450);
			this.Name = "AssetView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Asset View";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AssetView_FormClosing);
			this.Load += new System.EventHandler(this.AssetView_Load);
			this.Resize += new System.EventHandler(this.AssetView_Resize);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ComboBox viewComboBox;
		private System.Windows.Forms.Button defineNewAssetButton;
		public System.Windows.Forms.Button saveButton;
	}
}