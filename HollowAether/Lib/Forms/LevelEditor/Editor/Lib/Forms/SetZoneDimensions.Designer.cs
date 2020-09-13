namespace HollowAether.Lib.Forms.LevelEditor {
	partial class SetZoneDimensions {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetZoneDimensions));
			this.widthLabel = new System.Windows.Forms.Label();
			this.zoneTextBox = new System.Windows.Forms.TextBox();
			this.widthTextBox = new System.Windows.Forms.TextBox();
			this.heightLabel = new System.Windows.Forms.Label();
			this.heightTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// widthLabel
			// 
			this.widthLabel.AutoSize = true;
			this.widthLabel.Location = new System.Drawing.Point(12, 58);
			this.widthLabel.Name = "widthLabel";
			this.widthLabel.Size = new System.Drawing.Size(50, 20);
			this.widthLabel.TabIndex = 0;
			this.widthLabel.Text = "Width";
			// 
			// zoneTextBox
			// 
			this.zoneTextBox.Location = new System.Drawing.Point(13, 13);
			this.zoneTextBox.Name = "zoneTextBox";
			this.zoneTextBox.ReadOnly = true;
			this.zoneTextBox.Size = new System.Drawing.Size(812, 26);
			this.zoneTextBox.TabIndex = 1;
			this.zoneTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// widthTextBox
			// 
			this.widthTextBox.Location = new System.Drawing.Point(68, 55);
			this.widthTextBox.Name = "widthTextBox";
			this.widthTextBox.Size = new System.Drawing.Size(757, 26);
			this.widthTextBox.TabIndex = 2;
			this.widthTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// heightLabel
			// 
			this.heightLabel.AutoSize = true;
			this.heightLabel.Location = new System.Drawing.Point(12, 95);
			this.heightLabel.Name = "heightLabel";
			this.heightLabel.Size = new System.Drawing.Size(56, 20);
			this.heightLabel.TabIndex = 0;
			this.heightLabel.Text = "Height";
			// 
			// heightTextBox
			// 
			this.heightTextBox.Location = new System.Drawing.Point(68, 92);
			this.heightTextBox.Name = "heightTextBox";
			this.heightTextBox.Size = new System.Drawing.Size(757, 26);
			this.heightTextBox.TabIndex = 2;
			this.heightTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(13, 128);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(400, 30);
			this.saveButton.TabIndex = 3;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(425, 128);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(400, 30);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// SetZoneDimensions
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(837, 170);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.heightTextBox);
			this.Controls.Add(this.widthTextBox);
			this.Controls.Add(this.zoneTextBox);
			this.Controls.Add(this.heightLabel);
			this.Controls.Add(this.widthLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SetZoneDimensions";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Set Zone Dimensions";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetZoneDimensions_FormClosing);
			this.Load += new System.EventHandler(this.SetZoneDimensions_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label widthLabel;
		private System.Windows.Forms.TextBox zoneTextBox;
		private System.Windows.Forms.TextBox widthTextBox;
		private System.Windows.Forms.Label heightLabel;
		private System.Windows.Forms.TextBox heightTextBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
	}
}