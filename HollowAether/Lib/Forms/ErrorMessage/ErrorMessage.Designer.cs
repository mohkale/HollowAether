namespace HollowAether.Lib.Forms {
	partial class ErrorMessage {
		/// <summary>Required designer variable. </summary>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorMessage));
			this.textBox = new System.Windows.Forms.RichTextBox();
			this.CopyButton = new System.Windows.Forms.Button();
			this.ClearButton = new System.Windows.Forms.Button();
			this.AbortButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// textBox
			// 
			this.textBox.AcceptsTab = true;
			this.textBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.textBox.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBox.Location = new System.Drawing.Point(0, 0);
			this.textBox.Name = "textBox";
			this.textBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
			this.textBox.Size = new System.Drawing.Size(818, 480);
			this.textBox.TabIndex = 0;
			this.textBox.Text = "";
			// 
			// CopyButton
			// 
			this.CopyButton.Image = global::HollowAether.Properties.Resources.Copy;
			this.CopyButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.CopyButton.Location = new System.Drawing.Point(563, 491);
			this.CopyButton.Name = "CopyButton";
			this.CopyButton.Size = new System.Drawing.Size(100, 40);
			this.CopyButton.TabIndex = 1;
			this.CopyButton.Text = "Copy";
			this.CopyButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.CopyButton.UseVisualStyleBackColor = true;
			this.CopyButton.Click += new System.EventHandler(this.CopyButton_Click);
			// 
			// ClearButton
			// 
			this.ClearButton.Image = global::HollowAether.Properties.Resources.Clear;
			this.ClearButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ClearButton.Location = new System.Drawing.Point(706, 491);
			this.ClearButton.Name = "ClearButton";
			this.ClearButton.Size = new System.Drawing.Size(100, 40);
			this.ClearButton.TabIndex = 1;
			this.ClearButton.Text = "Clear";
			this.ClearButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.ClearButton.UseVisualStyleBackColor = true;
			this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
			// 
			// AbortButton
			// 
			this.AbortButton.Image = global::HollowAether.Properties.Resources.Abort;
			this.AbortButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.AbortButton.Location = new System.Drawing.Point(12, 491);
			this.AbortButton.Name = "AbortButton";
			this.AbortButton.Size = new System.Drawing.Size(100, 40);
			this.AbortButton.TabIndex = 1;
			this.AbortButton.Text = "Abort";
			this.AbortButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.AbortButton.UseVisualStyleBackColor = true;
			this.AbortButton.Click += new System.EventHandler(this.AbortButton_Click);
			// 
			// ErrorMessage
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(818, 544);
			this.Controls.Add(this.CopyButton);
			this.Controls.Add(this.ClearButton);
			this.Controls.Add(this.AbortButton);
			this.Controls.Add(this.textBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorMessage";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Error Message";
			this.Load += new System.EventHandler(this.EMB_Load);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EMB_KeyDown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox textBox;
		private System.Windows.Forms.Button AbortButton;
		private System.Windows.Forms.Button ClearButton;
		private System.Windows.Forms.Button CopyButton;
	}
}