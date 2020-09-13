namespace HollowAether.Lib.Forms.LevelEditor {
	partial class NewAssetDialog {
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
			this.IDLabe = new System.Windows.Forms.Label();
			this.typeLabel = new System.Windows.Forms.Label();
			this.typeComboBox = new System.Windows.Forms.ComboBox();
			this.valueLabel = new System.Windows.Forms.Label();
			this.idTextBox = new System.Windows.Forms.TextBox();
			this.valueTextBox = new System.Windows.Forms.TextBox();
			this.saveButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// IDLabe
			// 
			this.IDLabe.AutoSize = true;
			this.IDLabe.Location = new System.Drawing.Point(117, 9);
			this.IDLabe.Name = "IDLabe";
			this.IDLabe.Size = new System.Drawing.Size(26, 20);
			this.IDLabe.TabIndex = 0;
			this.IDLabe.Text = "ID";
			// 
			// typeLabel
			// 
			this.typeLabel.AutoSize = true;
			this.typeLabel.Location = new System.Drawing.Point(354, 9);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(43, 20);
			this.typeLabel.TabIndex = 1;
			this.typeLabel.Text = "Type";
			// 
			// typeComboBox
			// 
			this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.typeComboBox.FormattingEnabled = true;
			this.typeComboBox.Location = new System.Drawing.Point(243, 33);
			this.typeComboBox.Name = "typeComboBox";
			this.typeComboBox.Size = new System.Drawing.Size(270, 28);
			this.typeComboBox.TabIndex = 2;
			// 
			// valueLabel
			// 
			this.valueLabel.AutoSize = true;
			this.valueLabel.Location = new System.Drawing.Point(599, 12);
			this.valueLabel.Name = "valueLabel";
			this.valueLabel.Size = new System.Drawing.Size(50, 20);
			this.valueLabel.TabIndex = 1;
			this.valueLabel.Text = "Value";
			// 
			// idTextBox
			// 
			this.idTextBox.Location = new System.Drawing.Point(13, 33);
			this.idTextBox.Name = "idTextBox";
			this.idTextBox.Size = new System.Drawing.Size(224, 26);
			this.idTextBox.TabIndex = 4;
			// 
			// valueTextBox
			// 
			this.valueTextBox.Location = new System.Drawing.Point(519, 33);
			this.valueTextBox.Name = "valueTextBox";
			this.valueTextBox.Size = new System.Drawing.Size(224, 26);
			this.valueTextBox.TabIndex = 4;
			// 
			// saveButton
			// 
			this.saveButton.Location = new System.Drawing.Point(13, 70);
			this.saveButton.Name = "saveButton";
			this.saveButton.Size = new System.Drawing.Size(350, 30);
			this.saveButton.TabIndex = 5;
			this.saveButton.Text = "Save";
			this.saveButton.UseVisualStyleBackColor = true;
			this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Location = new System.Drawing.Point(396, 70);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(350, 30);
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// NewAssetDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(758, 112);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.saveButton);
			this.Controls.Add(this.valueTextBox);
			this.Controls.Add(this.idTextBox);
			this.Controls.Add(this.typeComboBox);
			this.Controls.Add(this.valueLabel);
			this.Controls.Add(this.typeLabel);
			this.Controls.Add(this.IDLabe);
			this.Name = "NewAssetDialog";
			this.Text = "New Asset";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NewAssetDialog_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label IDLabe;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.ComboBox typeComboBox;
		private System.Windows.Forms.Label valueLabel;
		private System.Windows.Forms.TextBox idTextBox;
		private System.Windows.Forms.TextBox valueTextBox;
		private System.Windows.Forms.Button saveButton;
		private System.Windows.Forms.Button cancelButton;
	}
}