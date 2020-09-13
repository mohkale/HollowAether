namespace HollowAether.Lib.Forms {
	partial class SetInitialZone {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SetInitialZone));
			this.label2 = new System.Windows.Forms.Label();
			this.yLabel = new System.Windows.Forms.Label();
			this.yTextBox = new System.Windows.Forms.TextBox();
			this.xLabel = new System.Windows.Forms.Label();
			this.xTextBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SaveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Location = new System.Drawing.Point(348, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(2, 22);
			this.label2.TabIndex = 14;
			// 
			// yLabel
			// 
			this.yLabel.AutoSize = true;
			this.yLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.yLabel.Location = new System.Drawing.Point(356, 16);
			this.yLabel.Name = "yLabel";
			this.yLabel.Size = new System.Drawing.Size(22, 22);
			this.yLabel.TabIndex = 11;
			this.yLabel.Text = "Y";
			// 
			// yTextBox
			// 
			this.yTextBox.Location = new System.Drawing.Point(384, 14);
			this.yTextBox.Name = "yTextBox";
			this.yTextBox.Size = new System.Drawing.Size(302, 26);
			this.yTextBox.TabIndex = 9;
			this.yTextBox.Text = "000";
			this.yTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// xLabel
			// 
			this.xLabel.AutoSize = true;
			this.xLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.xLabel.Location = new System.Drawing.Point(12, 14);
			this.xLabel.Name = "xLabel";
			this.xLabel.Size = new System.Drawing.Size(22, 22);
			this.xLabel.TabIndex = 12;
			this.xLabel.Text = "X";
			// 
			// xTextBox
			// 
			this.xTextBox.Location = new System.Drawing.Point(40, 12);
			this.xTextBox.Name = "xTextBox";
			this.xTextBox.Size = new System.Drawing.Size(302, 26);
			this.xTextBox.TabIndex = 10;
			this.xTextBox.Text = "000";
			this.xTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(356, 58);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(330, 35);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// SaveButton
			// 
			this.SaveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.SaveButton.Location = new System.Drawing.Point(12, 58);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(330, 35);
			this.SaveButton.TabIndex = 8;
			this.SaveButton.Text = "Save";
			this.SaveButton.UseVisualStyleBackColor = true;
			// 
			// SetInitialZone
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(699, 109);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.yLabel);
			this.Controls.Add(this.yTextBox);
			this.Controls.Add(this.xLabel);
			this.Controls.Add(this.xTextBox);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.SaveButton);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SetInitialZone";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Set Initial Zone";
			this.Load += new System.EventHandler(this.SetInitialZone_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label yLabel;
		private System.Windows.Forms.TextBox yTextBox;
		private System.Windows.Forms.Label xLabel;
		private System.Windows.Forms.TextBox xTextBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button SaveButton;
	}
}