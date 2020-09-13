namespace HollowAether.Lib.Forms.LevelEditor {
	partial class SetZoomDialog {
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
			this.zoomTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.minZoomTextBox = new System.Windows.Forms.TextBox();
			this.minimumValueLabel = new System.Windows.Forms.Label();
			this.maxZoomTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// zoomTextBox
			// 
			this.zoomTextBox.Location = new System.Drawing.Point(13, 13);
			this.zoomTextBox.Name = "zoomTextBox";
			this.zoomTextBox.Size = new System.Drawing.Size(753, 26);
			this.zoomTextBox.TabIndex = 0;
			this.zoomTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(16, 77);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(374, 31);
			this.saveButton.TabIndex = 1;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// minZoomTextBox
			// 
			this.minZoomTextBox.Location = new System.Drawing.Point(61, 45);
			this.minZoomTextBox.Name = "minZoomTextBox";
			this.minZoomTextBox.ReadOnly = true;
			this.minZoomTextBox.Size = new System.Drawing.Size(329, 26);
			this.minZoomTextBox.TabIndex = 2;
			// 
			// minimumValueLabel
			// 
			this.minimumValueLabel.AutoSize = true;
			this.minimumValueLabel.Location = new System.Drawing.Point(12, 48);
			this.minimumValueLabel.Name = "minimumValueLabel";
			this.minimumValueLabel.Size = new System.Drawing.Size(34, 20);
			this.minimumValueLabel.TabIndex = 3;
			this.minimumValueLabel.Text = "Min";
			// 
			// maxZoomTextBox
			// 
			this.maxZoomTextBox.Location = new System.Drawing.Point(437, 45);
			this.maxZoomTextBox.Name = "maxZoomTextBox";
			this.maxZoomTextBox.ReadOnly = true;
			this.maxZoomTextBox.Size = new System.Drawing.Size(329, 26);
			this.maxZoomTextBox.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(397, 48);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(38, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "Max";
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(401, 77);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(365, 31);
			this.cancelButton.TabIndex = 1;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// SetZoomDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			this.ClientSize = new System.Drawing.Size(778, 117);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.maxZoomTextBox);
			this.Controls.Add(this.minimumValueLabel);
			this.Controls.Add(this.minZoomTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.zoomTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "SetZoomDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "SetZoomDialog";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.SetZoomDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox zoomTextBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.TextBox minZoomTextBox;
		private System.Windows.Forms.Label minimumValueLabel;
		private System.Windows.Forms.TextBox maxZoomTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cancelButton;
	}
}