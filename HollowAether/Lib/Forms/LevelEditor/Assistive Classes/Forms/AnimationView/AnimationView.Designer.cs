namespace HollowAether.Lib.Forms.LevelEditor {
	partial class AnimationView {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AnimationView));
			this.animationTreeView = new System.Windows.Forms.TreeView();
			this.newAnimationButton = new System.Windows.Forms.Button();
			this.AnimationGroupPanel = new System.Windows.Forms.Panel();
			this.textureDisplayComboBox = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// animationTreeView
			// 
			this.animationTreeView.Location = new System.Drawing.Point(12, 12);
			this.animationTreeView.Name = "animationTreeView";
			this.animationTreeView.Size = new System.Drawing.Size(223, 408);
			this.animationTreeView.TabIndex = 0;
			this.animationTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.animationTreeView_NodeMouseDoubleClick);
			// 
			// newAnimationButton
			// 
			this.newAnimationButton.Location = new System.Drawing.Point(12, 426);
			this.newAnimationButton.Name = "newAnimationButton";
			this.newAnimationButton.Size = new System.Drawing.Size(223, 37);
			this.newAnimationButton.TabIndex = 1;
			this.newAnimationButton.Text = "Create New Animation";
			this.newAnimationButton.UseVisualStyleBackColor = true;
			this.newAnimationButton.Click += new System.EventHandler(this.newAnimationButton_Click);
			// 
			// AnimationGroupPanel
			// 
			this.AnimationGroupPanel.AutoScroll = true;
			this.AnimationGroupPanel.BackColor = System.Drawing.SystemColors.HighlightText;
			this.AnimationGroupPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.AnimationGroupPanel.Location = new System.Drawing.Point(241, 49);
			this.AnimationGroupPanel.Name = "AnimationGroupPanel";
			this.AnimationGroupPanel.Size = new System.Drawing.Size(1080, 413);
			this.AnimationGroupPanel.TabIndex = 2;
			// 
			// textureDisplayComboBox
			// 
			this.textureDisplayComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.textureDisplayComboBox.FormattingEnabled = true;
			this.textureDisplayComboBox.Location = new System.Drawing.Point(241, 12);
			this.textureDisplayComboBox.Name = "textureDisplayComboBox";
			this.textureDisplayComboBox.Size = new System.Drawing.Size(1080, 28);
			this.textureDisplayComboBox.TabIndex = 3;
			this.textureDisplayComboBox.TextChanged += new System.EventHandler(this.textureDisplayComboBox_TextChanged);
			// 
			// AnimationView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1331, 475);
			this.Controls.Add(this.textureDisplayComboBox);
			this.Controls.Add(this.AnimationGroupPanel);
			this.Controls.Add(this.newAnimationButton);
			this.Controls.Add(this.animationTreeView);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AnimationView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Animation View";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AnimationView_FormClosing);
			this.Load += new System.EventHandler(this.AnimationView_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TreeView animationTreeView;
		private System.Windows.Forms.Button newAnimationButton;
		private System.Windows.Forms.Panel AnimationGroupPanel;
		private System.Windows.Forms.ComboBox textureDisplayComboBox;
	}
}