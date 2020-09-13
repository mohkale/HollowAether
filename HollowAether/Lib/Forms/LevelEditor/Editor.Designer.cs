namespace HollowAether.Lib.Forms {
	partial class Editor {
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
			this.headerMenuStrip = new System.Windows.Forms.MenuStrip();
			this.fileTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.newMapTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newZoneTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.loadTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.modifyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpTSMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mapLabelTextBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.addZoneButton = new System.Windows.Forms.Button();
			this.zoneViewList = new System.Windows.Forms.ListView();
			this._01 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this._02 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.currentPositionTextBox = new System.Windows.Forms.TextBox();
			this.listViewContext = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.animationsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setStartZoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.headerMenuStrip.SuspendLayout();
			this.listViewContext.SuspendLayout();
			this.SuspendLayout();
			// 
			// headerMenuStrip
			// 
			this.headerMenuStrip.BackColor = System.Drawing.SystemColors.Control;
			this.headerMenuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.headerMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileTSMenuItem,
            this.editTSMenuItem,
            this.creditsToolStripMenuItem,
            this.helpTSMenuItem});
			this.headerMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.headerMenuStrip.Name = "headerMenuStrip";
			this.headerMenuStrip.Size = new System.Drawing.Size(1128, 33);
			this.headerMenuStrip.TabIndex = 0;
			this.headerMenuStrip.Text = "Header Menu";
			// 
			// fileTSMenuItem
			// 
			this.fileTSMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator4,
            this.saveToolStripMenuItem,
            this.saveAsTSMenuItem,
            this.toolStripSeparator1,
            this.loadTSMenuItem,
            this.toolStripSeparator2,
            this.exitTSMenuItem});
			this.fileTSMenuItem.Name = "fileTSMenuItem";
			this.fileTSMenuItem.Size = new System.Drawing.Size(50, 29);
			this.fileTSMenuItem.Text = "File";
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapTSMenuItem,
            this.newZoneTSMenuItem});
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(211, 30);
			this.toolStripMenuItem1.Text = "New";
			// 
			// newMapTSMenuItem
			// 
			this.newMapTSMenuItem.Name = "newMapTSMenuItem";
			this.newMapTSMenuItem.Size = new System.Drawing.Size(137, 30);
			this.newMapTSMenuItem.Text = "Map";
			this.newMapTSMenuItem.Click += new System.EventHandler(this.newMapTSMenuItem_Click);
			// 
			// newZoneTSMenuItem
			// 
			this.newZoneTSMenuItem.Name = "newZoneTSMenuItem";
			this.newZoneTSMenuItem.Size = new System.Drawing.Size(137, 30);
			this.newZoneTSMenuItem.Text = "Zone";
			this.newZoneTSMenuItem.Click += new System.EventHandler(this.newZoneTSMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(208, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsTSMenuItem
			// 
			this.saveAsTSMenuItem.Name = "saveAsTSMenuItem";
			this.saveAsTSMenuItem.Size = new System.Drawing.Size(211, 30);
			this.saveAsTSMenuItem.Text = "Save As";
			this.saveAsTSMenuItem.Click += new System.EventHandler(this.saveAsTSMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
			// 
			// loadTSMenuItem
			// 
			this.loadTSMenuItem.Name = "loadTSMenuItem";
			this.loadTSMenuItem.Size = new System.Drawing.Size(211, 30);
			this.loadTSMenuItem.Text = "Load";
			this.loadTSMenuItem.Click += new System.EventHandler(this.loadTSMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(208, 6);
			// 
			// exitTSMenuItem
			// 
			this.exitTSMenuItem.Name = "exitTSMenuItem";
			this.exitTSMenuItem.Size = new System.Drawing.Size(211, 30);
			this.exitTSMenuItem.Text = "Exit";
			this.exitTSMenuItem.Click += new System.EventHandler(this.exitTSMenuItem_Click);
			// 
			// editTSMenuItem
			// 
			this.editTSMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem1,
            this.animationsToolStripMenuItem,
            this.setStartZoneToolStripMenuItem});
			this.editTSMenuItem.Name = "editTSMenuItem";
			this.editTSMenuItem.Size = new System.Drawing.Size(54, 29);
			this.editTSMenuItem.Text = "Edit";
			// 
			// modifyToolStripMenuItem1
			// 
			this.modifyToolStripMenuItem1.Name = "modifyToolStripMenuItem1";
			this.modifyToolStripMenuItem1.Size = new System.Drawing.Size(211, 30);
			this.modifyToolStripMenuItem1.Text = "Modify";
			this.modifyToolStripMenuItem1.Click += new System.EventHandler(this.modifyToolStripMenuItem1_Click);
			// 
			// creditsToolStripMenuItem
			// 
			this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
			this.creditsToolStripMenuItem.Size = new System.Drawing.Size(79, 29);
			this.creditsToolStripMenuItem.Text = "Credits";
			// 
			// helpTSMenuItem
			// 
			this.helpTSMenuItem.Name = "helpTSMenuItem";
			this.helpTSMenuItem.Size = new System.Drawing.Size(61, 29);
			this.helpTSMenuItem.Text = "Help";
			this.helpTSMenuItem.Click += new System.EventHandler(this.helpTSMenuItem_Click);
			// 
			// mapLabelTextBox
			// 
			this.mapLabelTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.mapLabelTextBox.Location = new System.Drawing.Point(59, 44);
			this.mapLabelTextBox.Name = "mapLabelTextBox";
			this.mapLabelTextBox.ReadOnly = true;
			this.mapLabelTextBox.Size = new System.Drawing.Size(313, 26);
			this.mapLabelTextBox.TabIndex = 1;
			this.mapLabelTextBox.Text = "No Map Loaded";
			this.mapLabelTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 47);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(40, 20);
			this.label1.TabIndex = 2;
			this.label1.Text = "Map";
			// 
			// addZoneButton
			// 
			this.addZoneButton.Enabled = false;
			this.addZoneButton.Location = new System.Drawing.Point(12, 698);
			this.addZoneButton.Name = "addZoneButton";
			this.addZoneButton.Size = new System.Drawing.Size(360, 34);
			this.addZoneButton.TabIndex = 4;
			this.addZoneButton.Text = "Add New Zone";
			this.addZoneButton.UseVisualStyleBackColor = true;
			this.addZoneButton.Click += new System.EventHandler(this.addZoneButton_Click);
			// 
			// zoneViewList
			// 
			this.zoneViewList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this._01,
            this._02});
			this.zoneViewList.Enabled = false;
			this.zoneViewList.FullRowSelect = true;
			this.zoneViewList.Location = new System.Drawing.Point(12, 87);
			this.zoneViewList.MultiSelect = false;
			this.zoneViewList.Name = "zoneViewList";
			this.zoneViewList.Size = new System.Drawing.Size(360, 600);
			this.zoneViewList.TabIndex = 5;
			this.zoneViewList.UseCompatibleStateImageBehavior = false;
			this.zoneViewList.View = System.Windows.Forms.View.Details;
			this.zoneViewList.SelectedIndexChanged += new System.EventHandler(this.zoneViewList_SelectedIndexChanged);
			this.zoneViewList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.zoneViewList_KeyDown);
			this.zoneViewList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.zoneViewList_MouseClick);
			this.zoneViewList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.zoneViewList_MouseDoubleClick);
			// 
			// _01
			// 
			this._01.Text = "Zone";
			this._01.Width = 288;
			// 
			// _02
			// 
			this._02.Text = "Visibility";
			this._02.Width = 67;
			// 
			// currentPositionTextBox
			// 
			this.currentPositionTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.currentPositionTextBox.Location = new System.Drawing.Point(390, 44);
			this.currentPositionTextBox.Name = "currentPositionTextBox";
			this.currentPositionTextBox.ReadOnly = true;
			this.currentPositionTextBox.Size = new System.Drawing.Size(726, 26);
			this.currentPositionTextBox.TabIndex = 6;
			this.currentPositionTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// listViewContext
			// 
			this.listViewContext.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.listViewContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.deleteToolStripMenuItem});
			this.listViewContext.Name = "listViewContext";
			this.listViewContext.Size = new System.Drawing.Size(155, 64);
			// 
			// modifyToolStripMenuItem
			// 
			this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
			this.modifyToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
			this.modifyToolStripMenuItem.Text = "Modify";
			this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(154, 30);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// animationsToolStripMenuItem
			// 
			this.animationsToolStripMenuItem.Name = "animationsToolStripMenuItem";
			this.animationsToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
			this.animationsToolStripMenuItem.Text = "Animations";
			this.animationsToolStripMenuItem.Click += new System.EventHandler(this.animationsToolStripMenuItem_Click);
			// 
			// setStartZoneToolStripMenuItem
			// 
			this.setStartZoneToolStripMenuItem.Name = "setStartZoneToolStripMenuItem";
			this.setStartZoneToolStripMenuItem.Size = new System.Drawing.Size(211, 30);
			this.setStartZoneToolStripMenuItem.Text = "Set Start Zone";
			this.setStartZoneToolStripMenuItem.Click += new System.EventHandler(this.setStartZoneToolStripMenuItem_Click);
			// 
			// Editor
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Control;
			this.ClientSize = new System.Drawing.Size(1128, 744);
			this.Controls.Add(this.currentPositionTextBox);
			this.Controls.Add(this.zoneViewList);
			this.Controls.Add(this.addZoneButton);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.mapLabelTextBox);
			this.Controls.Add(this.headerMenuStrip);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.headerMenuStrip;
			this.MaximizeBox = false;
			this.Name = "Editor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Level Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Editor_FormClosing);
			this.Load += new System.EventHandler(this.Editor_Load);
			this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Editor_DragDrop);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Editor_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.LEKeyDown);
			this.headerMenuStrip.ResumeLayout(false);
			this.headerMenuStrip.PerformLayout();
			this.listViewContext.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip headerMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileTSMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsTSMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem loadTSMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripMenuItem exitTSMenuItem;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem newMapTSMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newZoneTSMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem editTSMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpTSMenuItem;
		private System.Windows.Forms.ToolStripMenuItem creditsToolStripMenuItem;
		private System.Windows.Forms.TextBox mapLabelTextBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button addZoneButton;
		private System.Windows.Forms.ListView zoneViewList;
		private System.Windows.Forms.ColumnHeader _01;
		private System.Windows.Forms.ColumnHeader _02;
		private System.Windows.Forms.TextBox currentPositionTextBox;
		private System.Windows.Forms.ContextMenuStrip listViewContext;
		private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem animationsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setStartZoneToolStripMenuItem;
	}
}