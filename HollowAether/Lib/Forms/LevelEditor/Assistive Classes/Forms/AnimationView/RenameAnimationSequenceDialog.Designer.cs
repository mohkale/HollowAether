namespace HollowAether.Lib.Forms.LevelEditor {
	partial class RenameAnimationSequenceDialog {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameAnimationSequenceDialog));
			this.animationContainerLabel = new System.Windows.Forms.Label();
			this.animationContainerTextBox = new System.Windows.Forms.TextBox();
			this.sequenceNameLabel = new System.Windows.Forms.Label();
			this.sequenceNameTextBox = new System.Windows.Forms.TextBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.resetButton = new System.Windows.Forms.Button();
			this.saveButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// animationContainerLabel
			// 
			this.animationContainerLabel.AutoSize = true;
			this.animationContainerLabel.Location = new System.Drawing.Point(13, 13);
			this.animationContainerLabel.Name = "animationContainerLabel";
			this.animationContainerLabel.Size = new System.Drawing.Size(153, 20);
			this.animationContainerLabel.TabIndex = 0;
			this.animationContainerLabel.Text = "Animation Container";
			// 
			// animationContainerTextBox
			// 
			this.animationContainerTextBox.Location = new System.Drawing.Point(173, 12);
			this.animationContainerTextBox.Name = "animationContainerTextBox";
			this.animationContainerTextBox.Size = new System.Drawing.Size(570, 26);
			this.animationContainerTextBox.TabIndex = 1;
			// 
			// sequenceNameLabel
			// 
			this.sequenceNameLabel.AutoSize = true;
			this.sequenceNameLabel.Location = new System.Drawing.Point(13, 50);
			this.sequenceNameLabel.Name = "sequenceNameLabel";
			this.sequenceNameLabel.Size = new System.Drawing.Size(128, 20);
			this.sequenceNameLabel.TabIndex = 0;
			this.sequenceNameLabel.Text = "Sequence Name";
			// 
			// sequenceNameTextBox
			// 
			this.sequenceNameTextBox.Location = new System.Drawing.Point(173, 49);
			this.sequenceNameTextBox.Name = "sequenceNameTextBox";
			this.sequenceNameTextBox.Size = new System.Drawing.Size(570, 26);
			this.sequenceNameTextBox.TabIndex = 1;
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(17, 93);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(258, 28);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// resetButton
			// 
			this.resetButton.Location = new System.Drawing.Point(281, 93);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(206, 28);
			this.resetButton.TabIndex = 2;
			this.resetButton.Text = "Reset";
			this.resetButton.UseVisualStyleBackColor = true;
			this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// saveButton
			// 
			this.saveButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.saveButton.Location = new System.Drawing.Point(493, 93);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(250, 28);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// RenameAnimationSequenceDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(755, 139);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.resetButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.sequenceNameTextBox);
			this.Controls.Add(this.animationContainerTextBox);
			this.Controls.Add(this.sequenceNameLabel);
			this.Controls.Add(this.animationContainerLabel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "RenameAnimationSequenceDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Rename Animation Sequence Dialog";
			this.Load += new System.EventHandler(this.RenameAnimationSequenceDialog_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label animationContainerLabel;
		private System.Windows.Forms.TextBox animationContainerTextBox;
		private System.Windows.Forms.Label sequenceNameLabel;
		private System.Windows.Forms.TextBox sequenceNameTextBox;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button resetButton;
		private System.Windows.Forms.Button saveButton;
	}
}