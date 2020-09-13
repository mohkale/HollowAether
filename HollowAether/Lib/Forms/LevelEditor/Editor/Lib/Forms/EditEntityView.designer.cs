namespace HollowAether.Lib.Forms.LevelEditor{
	partial class EditEntityView {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> Clean up any resources being used. </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditEntityView));
			this.tileIDTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.resetButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.attributesPanel = new System.Windows.Forms.Panel();
			this.clearAllButton = new System.Windows.Forms.Button();
			this.deleteTileButton = new System.Windows.Forms.Button();
			this.entityTypeTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// tileIDTextBox
			// 
			this.tileIDTextBox.Location = new System.Drawing.Point(12, 34);
			this.tileIDTextBox.Name = "tileIDTextBox";
			this.tileIDTextBox.Size = new System.Drawing.Size(265, 26);
			this.tileIDTextBox.TabIndex = 1;
			this.tileIDTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(12, 91);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(264, 35);
			this.saveButton.TabIndex = 2;
			this.saveButton.Text = "Save Changes";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// resetButton
			// 
			this.resetButton.Location = new System.Drawing.Point(12, 173);
			this.resetButton.Name = "resetButton";
			this.resetButton.Size = new System.Drawing.Size(264, 35);
			this.resetButton.TabIndex = 2;
			this.resetButton.Text = "Reset Tile";
			this.resetButton.UseVisualStyleBackColor = true;
			this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(12, 257);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(264, 35);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// attributesPanel
			// 
			this.attributesPanel.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
			this.attributesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.attributesPanel.Location = new System.Drawing.Point(294, 34);
			this.attributesPanel.Name = "attributesPanel";
			this.attributesPanel.Size = new System.Drawing.Size(327, 258);
			this.attributesPanel.TabIndex = 3;
			// 
			// clearAllButton
			// 
			this.clearAllButton.Location = new System.Drawing.Point(12, 132);
			this.clearAllButton.Name = "clearAllButton";
			this.clearAllButton.Size = new System.Drawing.Size(264, 35);
			this.clearAllButton.TabIndex = 2;
			this.clearAllButton.Text = "Clear All";
			this.clearAllButton.UseVisualStyleBackColor = true;
			this.clearAllButton.Click += new System.EventHandler(this.clearAllButton_Click);
			// 
			// deleteTileButton
			// 
			this.deleteTileButton.Location = new System.Drawing.Point(12, 214);
			this.deleteTileButton.Name = "deleteTileButton";
			this.deleteTileButton.Size = new System.Drawing.Size(264, 35);
			this.deleteTileButton.TabIndex = 2;
			this.deleteTileButton.Text = "Delete Tile";
			this.deleteTileButton.UseVisualStyleBackColor = true;
			this.deleteTileButton.Click += new System.EventHandler(this.deleteTileButton_Click);
			// 
			// entityTypeTextBox
			// 
			this.entityTypeTextBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.entityTypeTextBox.Location = new System.Drawing.Point(0, 0);
			this.entityTypeTextBox.Name = "entityTypeTextBox";
			this.entityTypeTextBox.ReadOnly = true;
			this.entityTypeTextBox.Size = new System.Drawing.Size(633, 26);
			this.entityTypeTextBox.TabIndex = 4;
			this.entityTypeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// EditEntityView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 304);
			this.Controls.Add(this.entityTypeTextBox);
			this.Controls.Add(this.attributesPanel);
			this.Controls.Add(this.deleteTileButton);
			this.Controls.Add(this.clearAllButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.resetButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.tileIDTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(655, 360);
			this.Name = "EditEntityView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Entity";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditEntityView_FormClosing);
			this.Load += new System.EventHandler(this.EditExistingEntityView_Load);
			this.Shown += new System.EventHandler(this.EditExistingEntityView_Shown);
			this.Resize += new System.EventHandler(this.EditEntityView_Resize);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.TextBox tileIDTextBox;
		public System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button resetButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Panel attributesPanel;
		private System.Windows.Forms.Button clearAllButton;
		public System.Windows.Forms.Button deleteTileButton;
		private System.Windows.Forms.TextBox entityTypeTextBox;
	}
}