namespace HollowAether.Lib.Forms.LevelEditor {
	partial class ZoneEditor {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ZoneEditor));
			this.menuStrip = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setZoneSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setBackgroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.assetViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.oneXZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.twoXZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.threeXZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fourXZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zoomSubMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayGroupBox = new System.Windows.Forms.GroupBox();
			this.tileBoxContainerPanel = new System.Windows.Forms.Panel();
			this.resetEntityButton = new System.Windows.Forms.Button();
			this.resizeZoneButton = new System.Windows.Forms.Button();
			this.viewAssetsButton = new System.Windows.Forms.Button();
			this.executeButton = new System.Windows.Forms.Button();
			this.openEditMenu = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.entityTypeComboBox = new System.Windows.Forms.ComboBox();
			this.editScriptButton = new System.Windows.Forms.Button();
			this.formatGroupBox = new System.Windows.Forms.GroupBox();
			this.clickTypePanel = new System.Windows.Forms.Panel();
			this.highlightRadioButton = new System.Windows.Forms.RadioButton();
			this.rectangleRadioButton = new System.Windows.Forms.RadioButton();
			this.clickRadioButton = new System.Windows.Forms.RadioButton();
			this.selectRadioButton = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.zoomOutButton = new System.Windows.Forms.Button();
			this.zoomInButton = new System.Windows.Forms.Button();
			this.layerTextBox = new System.Windows.Forms.TextBox();
			this.dividerLabel02 = new System.Windows.Forms.Label();
			this.dividerLabel01 = new System.Windows.Forms.Label();
			this.onlyCurrentEntityCheckBox = new System.Windows.Forms.CheckBox();
			this.backgroundCheckBox = new System.Windows.Forms.CheckBox();
			this.tileGridCheckBox = new System.Windows.Forms.CheckBox();
			this.pasteRadioButton = new System.Windows.Forms.RadioButton();
			this.deleteRadioButton = new System.Windows.Forms.RadioButton();
			this.copyRadioButton = new System.Windows.Forms.RadioButton();
			this.fillRadioButton = new System.Windows.Forms.RadioButton();
			this.drawRadioButton = new System.Windows.Forms.RadioButton();
			this.showTileMapButton = new System.Windows.Forms.Button();
			this.zoneCanvasPanel = new System.Windows.Forms.Panel();
			this.menuStrip.SuspendLayout();
			this.displayGroupBox.SuspendLayout();
			this.formatGroupBox.SuspendLayout();
			this.clickTypePanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip
			// 
			this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.zoneToolStripMenuItem,
            this.zoomToolStripMenuItem});
			this.menuStrip.Location = new System.Drawing.Point(0, 0);
			this.menuStrip.Name = "menuStrip";
			this.menuStrip.Size = new System.Drawing.Size(986, 33);
			this.menuStrip.TabIndex = 0;
			this.menuStrip.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(50, 29);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(134, 30);
			this.saveToolStripMenuItem.Text = "Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// zoneToolStripMenuItem
			// 
			this.zoneToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setZoneSizeToolStripMenuItem,
            this.setBackgroundToolStripMenuItem,
            this.assetViewToolStripMenuItem});
			this.zoneToolStripMenuItem.Name = "zoneToolStripMenuItem";
			this.zoneToolStripMenuItem.Size = new System.Drawing.Size(64, 29);
			this.zoneToolStripMenuItem.Text = "Zone";
			// 
			// setZoneSizeToolStripMenuItem
			// 
			this.setZoneSizeToolStripMenuItem.Name = "setZoneSizeToolStripMenuItem";
			this.setZoneSizeToolStripMenuItem.Size = new System.Drawing.Size(222, 30);
			this.setZoneSizeToolStripMenuItem.Text = "Set Zone Size";
			this.setZoneSizeToolStripMenuItem.Click += new System.EventHandler(this.setZoneSizeToolStripMenuItem_Click);
			// 
			// setBackgroundToolStripMenuItem
			// 
			this.setBackgroundToolStripMenuItem.Name = "setBackgroundToolStripMenuItem";
			this.setBackgroundToolStripMenuItem.Size = new System.Drawing.Size(222, 30);
			this.setBackgroundToolStripMenuItem.Text = "Set Background";
			this.setBackgroundToolStripMenuItem.Click += new System.EventHandler(this.setBackgroundToolStripMenuItem_Click);
			// 
			// assetViewToolStripMenuItem
			// 
			this.assetViewToolStripMenuItem.Name = "assetViewToolStripMenuItem";
			this.assetViewToolStripMenuItem.Size = new System.Drawing.Size(222, 30);
			this.assetViewToolStripMenuItem.Text = "Asset View";
			this.assetViewToolStripMenuItem.Click += new System.EventHandler(this.assetViewToolStripMenuItem_Click);
			// 
			// zoomToolStripMenuItem
			// 
			this.zoomToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.xToolStripMenuItem,
            this.oneXZoomMenuItem,
            this.twoXZoomMenuItem,
            this.threeXZoomMenuItem,
            this.fourXZoomMenuItem,
            this.zoomSubMenuItem});
			this.zoomToolStripMenuItem.Name = "zoomToolStripMenuItem";
			this.zoomToolStripMenuItem.Size = new System.Drawing.Size(72, 29);
			this.zoomToolStripMenuItem.Text = "Zoom";
			// 
			// xToolStripMenuItem
			// 
			this.xToolStripMenuItem.Name = "xToolStripMenuItem";
			this.xToolStripMenuItem.Size = new System.Drawing.Size(145, 30);
			this.xToolStripMenuItem.Text = "0.5x";
			this.xToolStripMenuItem.Click += new System.EventHandler(this.xHalfToolStripMenuItem_Click);
			// 
			// oneXZoomMenuItem
			// 
			this.oneXZoomMenuItem.Name = "oneXZoomMenuItem";
			this.oneXZoomMenuItem.Size = new System.Drawing.Size(145, 30);
			this.oneXZoomMenuItem.Text = "1.0x";
			this.oneXZoomMenuItem.Click += new System.EventHandler(this.oneXZoomMenuItem_Click);
			// 
			// twoXZoomMenuItem
			// 
			this.twoXZoomMenuItem.Name = "twoXZoomMenuItem";
			this.twoXZoomMenuItem.Size = new System.Drawing.Size(145, 30);
			this.twoXZoomMenuItem.Text = "2.0x";
			this.twoXZoomMenuItem.Click += new System.EventHandler(this.twoXZoomMenuItem_Click);
			// 
			// threeXZoomMenuItem
			// 
			this.threeXZoomMenuItem.Name = "threeXZoomMenuItem";
			this.threeXZoomMenuItem.Size = new System.Drawing.Size(145, 30);
			this.threeXZoomMenuItem.Text = "3.0x";
			this.threeXZoomMenuItem.Click += new System.EventHandler(this.threeXZoomMenuItem_Click);
			// 
			// fourXZoomMenuItem
			// 
			this.fourXZoomMenuItem.Name = "fourXZoomMenuItem";
			this.fourXZoomMenuItem.Size = new System.Drawing.Size(145, 30);
			this.fourXZoomMenuItem.Text = "4.0x";
			this.fourXZoomMenuItem.Click += new System.EventHandler(this.fourXZoomMenuItem_Click);
			// 
			// zoomSubMenuItem
			// 
			this.zoomSubMenuItem.Name = "zoomSubMenuItem";
			this.zoomSubMenuItem.Size = new System.Drawing.Size(145, 30);
			this.zoomSubMenuItem.Text = "Zoom";
			this.zoomSubMenuItem.Click += new System.EventHandler(this.zoomSubMenuItem_Click);
			// 
			// displayGroupBox
			// 
			this.displayGroupBox.Controls.Add(this.tileBoxContainerPanel);
			this.displayGroupBox.Controls.Add(this.resetEntityButton);
			this.displayGroupBox.Controls.Add(this.resizeZoneButton);
			this.displayGroupBox.Controls.Add(this.viewAssetsButton);
			this.displayGroupBox.Controls.Add(this.executeButton);
			this.displayGroupBox.Controls.Add(this.openEditMenu);
			this.displayGroupBox.Controls.Add(this.label1);
			this.displayGroupBox.Controls.Add(this.entityTypeComboBox);
			this.displayGroupBox.Controls.Add(this.editScriptButton);
			this.displayGroupBox.Location = new System.Drawing.Point(13, 37);
			this.displayGroupBox.Name = "displayGroupBox";
			this.displayGroupBox.Size = new System.Drawing.Size(179, 498);
			this.displayGroupBox.TabIndex = 1;
			this.displayGroupBox.TabStop = false;
			this.displayGroupBox.Text = "Display";
			// 
			// tileBoxContainerPanel
			// 
			this.tileBoxContainerPanel.BackColor = System.Drawing.Color.Black;
			this.tileBoxContainerPanel.Location = new System.Drawing.Point(6, 288);
			this.tileBoxContainerPanel.Name = "tileBoxContainerPanel";
			this.tileBoxContainerPanel.Size = new System.Drawing.Size(167, 167);
			this.tileBoxContainerPanel.TabIndex = 6;
			// 
			// resetEntityButton
			// 
			this.resetEntityButton.Location = new System.Drawing.Point(6, 220);
			this.resetEntityButton.Name = "resetEntityButton";
			this.resetEntityButton.Size = new System.Drawing.Size(167, 34);
			this.resetEntityButton.TabIndex = 5;
			this.resetEntityButton.Text = "Reset Entity";
			this.resetEntityButton.UseVisualStyleBackColor = true;
			this.resetEntityButton.Click += new System.EventHandler(this.resetEntityButton_Click);
			// 
			// resizeZoneButton
			// 
			this.resizeZoneButton.Location = new System.Drawing.Point(6, 180);
			this.resizeZoneButton.Name = "resizeZoneButton";
			this.resizeZoneButton.Size = new System.Drawing.Size(167, 34);
			this.resizeZoneButton.TabIndex = 5;
			this.resizeZoneButton.Text = "Resize";
			this.resizeZoneButton.UseVisualStyleBackColor = true;
			this.resizeZoneButton.Click += new System.EventHandler(this.resizeZoneButton_Click);
			// 
			// viewAssetsButton
			// 
			this.viewAssetsButton.Location = new System.Drawing.Point(6, 139);
			this.viewAssetsButton.Name = "viewAssetsButton";
			this.viewAssetsButton.Size = new System.Drawing.Size(167, 34);
			this.viewAssetsButton.TabIndex = 5;
			this.viewAssetsButton.Text = "View Assets";
			this.viewAssetsButton.UseVisualStyleBackColor = true;
			this.viewAssetsButton.Click += new System.EventHandler(this.viewAssetsButton_Click);
			// 
			// executeButton
			// 
			this.executeButton.Enabled = false;
			this.executeButton.Location = new System.Drawing.Point(6, 99);
			this.executeButton.Name = "executeButton";
			this.executeButton.Size = new System.Drawing.Size(167, 34);
			this.executeButton.TabIndex = 5;
			this.executeButton.Text = "Execute";
			this.executeButton.UseVisualStyleBackColor = true;
			this.executeButton.Click += new System.EventHandler(this.ExecuteButton_Click);
			// 
			// openEditMenu
			// 
			this.openEditMenu.Location = new System.Drawing.Point(6, 59);
			this.openEditMenu.Name = "openEditMenu";
			this.openEditMenu.Size = new System.Drawing.Size(167, 34);
			this.openEditMenu.TabIndex = 5;
			this.openEditMenu.Text = "Open Edit Menu";
			this.openEditMenu.UseVisualStyleBackColor = true;
			this.openEditMenu.Click += new System.EventHandler(this.openEditMenu_Click);
			// 
			// label1
			// 
			this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label1.Location = new System.Drawing.Point(0, 280);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(179, 2);
			this.label1.TabIndex = 3;
			// 
			// entityTypeComboBox
			// 
			this.entityTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.entityTypeComboBox.FormattingEnabled = true;
			this.entityTypeComboBox.Location = new System.Drawing.Point(6, 25);
			this.entityTypeComboBox.Name = "entityTypeComboBox";
			this.entityTypeComboBox.Size = new System.Drawing.Size(167, 28);
			this.entityTypeComboBox.TabIndex = 2;
			this.entityTypeComboBox.TextChanged += new System.EventHandler(this.entityTypeComboBox_TextChanged);
			// 
			// editScriptButton
			// 
			this.editScriptButton.Location = new System.Drawing.Point(6, 461);
			this.editScriptButton.Name = "editScriptButton";
			this.editScriptButton.Size = new System.Drawing.Size(167, 31);
			this.editScriptButton.TabIndex = 2;
			this.editScriptButton.Text = "Edit Script";
			this.editScriptButton.UseVisualStyleBackColor = true;
			this.editScriptButton.Click += new System.EventHandler(this.editScriptButton_Click);
			// 
			// formatGroupBox
			// 
			this.formatGroupBox.Controls.Add(this.clickTypePanel);
			this.formatGroupBox.Controls.Add(this.label2);
			this.formatGroupBox.Controls.Add(this.zoomOutButton);
			this.formatGroupBox.Controls.Add(this.zoomInButton);
			this.formatGroupBox.Controls.Add(this.layerTextBox);
			this.formatGroupBox.Controls.Add(this.dividerLabel02);
			this.formatGroupBox.Controls.Add(this.dividerLabel01);
			this.formatGroupBox.Controls.Add(this.onlyCurrentEntityCheckBox);
			this.formatGroupBox.Controls.Add(this.backgroundCheckBox);
			this.formatGroupBox.Controls.Add(this.tileGridCheckBox);
			this.formatGroupBox.Controls.Add(this.pasteRadioButton);
			this.formatGroupBox.Controls.Add(this.deleteRadioButton);
			this.formatGroupBox.Controls.Add(this.copyRadioButton);
			this.formatGroupBox.Controls.Add(this.fillRadioButton);
			this.formatGroupBox.Controls.Add(this.drawRadioButton);
			this.formatGroupBox.Controls.Add(this.showTileMapButton);
			this.formatGroupBox.Location = new System.Drawing.Point(844, 37);
			this.formatGroupBox.Name = "formatGroupBox";
			this.formatGroupBox.Size = new System.Drawing.Size(130, 498);
			this.formatGroupBox.TabIndex = 3;
			this.formatGroupBox.TabStop = false;
			this.formatGroupBox.Text = "Properties";
			// 
			// clickTypePanel
			// 
			this.clickTypePanel.Controls.Add(this.highlightRadioButton);
			this.clickTypePanel.Controls.Add(this.rectangleRadioButton);
			this.clickTypePanel.Controls.Add(this.clickRadioButton);
			this.clickTypePanel.Controls.Add(this.selectRadioButton);
			this.clickTypePanel.Location = new System.Drawing.Point(0, 177);
			this.clickTypePanel.Name = "clickTypePanel";
			this.clickTypePanel.Size = new System.Drawing.Size(130, 115);
			this.clickTypePanel.TabIndex = 9;
			// 
			// highlightRadioButton
			// 
			this.highlightRadioButton.AutoSize = true;
			this.highlightRadioButton.Location = new System.Drawing.Point(6, 57);
			this.highlightRadioButton.Name = "highlightRadioButton";
			this.highlightRadioButton.Size = new System.Drawing.Size(96, 24);
			this.highlightRadioButton.TabIndex = 1;
			this.highlightRadioButton.Text = "Highlight";
			this.highlightRadioButton.UseVisualStyleBackColor = true;
			this.highlightRadioButton.CheckedChanged += new System.EventHandler(this.highlightRadioButton_CheckedChanged);
			// 
			// rectangleRadioButton
			// 
			this.rectangleRadioButton.AutoSize = true;
			this.rectangleRadioButton.Location = new System.Drawing.Point(6, 85);
			this.rectangleRadioButton.Name = "rectangleRadioButton";
			this.rectangleRadioButton.Size = new System.Drawing.Size(107, 24);
			this.rectangleRadioButton.TabIndex = 1;
			this.rectangleRadioButton.Text = "Rectangle";
			this.rectangleRadioButton.UseVisualStyleBackColor = true;
			this.rectangleRadioButton.CheckedChanged += new System.EventHandler(this.rectangleRadioButton_CheckedChanged);
			// 
			// clickRadioButton
			// 
			this.clickRadioButton.AutoSize = true;
			this.clickRadioButton.Checked = true;
			this.clickRadioButton.Location = new System.Drawing.Point(6, 3);
			this.clickRadioButton.Name = "clickRadioButton";
			this.clickRadioButton.Size = new System.Drawing.Size(67, 24);
			this.clickRadioButton.TabIndex = 0;
			this.clickRadioButton.TabStop = true;
			this.clickRadioButton.Text = "Click";
			this.clickRadioButton.UseVisualStyleBackColor = true;
			this.clickRadioButton.CheckedChanged += new System.EventHandler(this.clickRadioButton_CheckedChanged);
			// 
			// selectRadioButton
			// 
			this.selectRadioButton.AutoSize = true;
			this.selectRadioButton.Location = new System.Drawing.Point(6, 30);
			this.selectRadioButton.Name = "selectRadioButton";
			this.selectRadioButton.Size = new System.Drawing.Size(79, 24);
			this.selectRadioButton.TabIndex = 1;
			this.selectRadioButton.Text = "Select";
			this.selectRadioButton.UseVisualStyleBackColor = true;
			this.selectRadioButton.CheckedChanged += new System.EventHandler(this.selectRadioButton_CheckedChanged);
			// 
			// label2
			// 
			this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.label2.Location = new System.Drawing.Point(0, 297);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 2);
			this.label2.TabIndex = 8;
			// 
			// zoomOutButton
			// 
			this.zoomOutButton.Location = new System.Drawing.Point(72, 429);
			this.zoomOutButton.Name = "zoomOutButton";
			this.zoomOutButton.Size = new System.Drawing.Size(52, 26);
			this.zoomOutButton.TabIndex = 6;
			this.zoomOutButton.Text = "-";
			this.zoomOutButton.UseVisualStyleBackColor = true;
			this.zoomOutButton.Click += new System.EventHandler(this.zoomOutButton_Click);
			// 
			// zoomInButton
			// 
			this.zoomInButton.Location = new System.Drawing.Point(7, 429);
			this.zoomInButton.Name = "zoomInButton";
			this.zoomInButton.Size = new System.Drawing.Size(52, 26);
			this.zoomInButton.TabIndex = 6;
			this.zoomInButton.Text = "+";
			this.zoomInButton.UseVisualStyleBackColor = true;
			this.zoomInButton.Click += new System.EventHandler(this.zoomInButton_Click);
			// 
			// layerTextBox
			// 
			this.layerTextBox.Location = new System.Drawing.Point(6, 397);
			this.layerTextBox.Name = "layerTextBox";
			this.layerTextBox.Size = new System.Drawing.Size(117, 26);
			this.layerTextBox.TabIndex = 5;
			this.layerTextBox.Text = "Layer";
			this.layerTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.layerTextBox.TextChanged += new System.EventHandler(this.layerTextBox_TextChanged);
			// 
			// dividerLabel02
			// 
			this.dividerLabel02.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dividerLabel02.Location = new System.Drawing.Point(0, 390);
			this.dividerLabel02.Name = "dividerLabel02";
			this.dividerLabel02.Size = new System.Drawing.Size(130, 2);
			this.dividerLabel02.TabIndex = 3;
			// 
			// dividerLabel01
			// 
			this.dividerLabel01.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.dividerLabel01.Location = new System.Drawing.Point(6, 172);
			this.dividerLabel01.Name = "dividerLabel01";
			this.dividerLabel01.Size = new System.Drawing.Size(130, 2);
			this.dividerLabel01.TabIndex = 3;
			// 
			// onlyCurrentEntityCheckBox
			// 
			this.onlyCurrentEntityCheckBox.AutoSize = true;
			this.onlyCurrentEntityCheckBox.Location = new System.Drawing.Point(6, 362);
			this.onlyCurrentEntityCheckBox.Name = "onlyCurrentEntityCheckBox";
			this.onlyCurrentEntityCheckBox.Size = new System.Drawing.Size(69, 24);
			this.onlyCurrentEntityCheckBox.TabIndex = 2;
			this.onlyCurrentEntityCheckBox.Text = "OCE";
			this.onlyCurrentEntityCheckBox.UseVisualStyleBackColor = true;
			this.onlyCurrentEntityCheckBox.CheckedChanged += new System.EventHandler(this.onlyCurrentEntityCheckBox_CheckedChanged);
			// 
			// backgroundCheckBox
			// 
			this.backgroundCheckBox.AutoSize = true;
			this.backgroundCheckBox.Enabled = false;
			this.backgroundCheckBox.Location = new System.Drawing.Point(6, 332);
			this.backgroundCheckBox.Name = "backgroundCheckBox";
			this.backgroundCheckBox.Size = new System.Drawing.Size(121, 24);
			this.backgroundCheckBox.TabIndex = 2;
			this.backgroundCheckBox.Text = "Background";
			this.backgroundCheckBox.UseVisualStyleBackColor = true;
			this.backgroundCheckBox.CheckedChanged += new System.EventHandler(this.backgroundCheckBox_CheckedChanged);
			// 
			// tileGridCheckBox
			// 
			this.tileGridCheckBox.AutoSize = true;
			this.tileGridCheckBox.Checked = true;
			this.tileGridCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.tileGridCheckBox.Location = new System.Drawing.Point(7, 302);
			this.tileGridCheckBox.Name = "tileGridCheckBox";
			this.tileGridCheckBox.Size = new System.Drawing.Size(93, 24);
			this.tileGridCheckBox.TabIndex = 2;
			this.tileGridCheckBox.Text = "Tile Grid";
			this.tileGridCheckBox.UseVisualStyleBackColor = true;
			this.tileGridCheckBox.CheckedChanged += new System.EventHandler(this.tileGridCheckBox_CheckedChanged);
			// 
			// pasteRadioButton
			// 
			this.pasteRadioButton.AutoSize = true;
			this.pasteRadioButton.Location = new System.Drawing.Point(6, 145);
			this.pasteRadioButton.Name = "pasteRadioButton";
			this.pasteRadioButton.Size = new System.Drawing.Size(75, 24);
			this.pasteRadioButton.TabIndex = 1;
			this.pasteRadioButton.Text = "Paste";
			this.pasteRadioButton.UseVisualStyleBackColor = true;
			this.pasteRadioButton.CheckedChanged += new System.EventHandler(this.pasteRadioButton_CheckedChanged);
			// 
			// deleteRadioButton
			// 
			this.deleteRadioButton.AutoSize = true;
			this.deleteRadioButton.Enabled = false;
			this.deleteRadioButton.Location = new System.Drawing.Point(6, 85);
			this.deleteRadioButton.Name = "deleteRadioButton";
			this.deleteRadioButton.Size = new System.Drawing.Size(81, 24);
			this.deleteRadioButton.TabIndex = 1;
			this.deleteRadioButton.Text = "Delete";
			this.deleteRadioButton.UseVisualStyleBackColor = true;
			this.deleteRadioButton.CheckedChanged += new System.EventHandler(this.deleteRadioButton_CheckedChanged);
			// 
			// copyRadioButton
			// 
			this.copyRadioButton.AutoSize = true;
			this.copyRadioButton.Enabled = false;
			this.copyRadioButton.Location = new System.Drawing.Point(6, 115);
			this.copyRadioButton.Name = "copyRadioButton";
			this.copyRadioButton.Size = new System.Drawing.Size(70, 24);
			this.copyRadioButton.TabIndex = 1;
			this.copyRadioButton.Text = "Copy";
			this.copyRadioButton.UseVisualStyleBackColor = true;
			this.copyRadioButton.CheckedChanged += new System.EventHandler(this.copyRadioButton_CheckedChanged);
			// 
			// fillRadioButton
			// 
			this.fillRadioButton.AutoSize = true;
			this.fillRadioButton.Enabled = false;
			this.fillRadioButton.Location = new System.Drawing.Point(6, 55);
			this.fillRadioButton.Name = "fillRadioButton";
			this.fillRadioButton.Size = new System.Drawing.Size(53, 24);
			this.fillRadioButton.TabIndex = 1;
			this.fillRadioButton.Text = "Fill";
			this.fillRadioButton.UseVisualStyleBackColor = true;
			this.fillRadioButton.CheckedChanged += new System.EventHandler(this.fillRadioButton_CheckedChanged);
			// 
			// drawRadioButton
			// 
			this.drawRadioButton.AutoSize = true;
			this.drawRadioButton.Checked = true;
			this.drawRadioButton.Location = new System.Drawing.Point(7, 25);
			this.drawRadioButton.Name = "drawRadioButton";
			this.drawRadioButton.Size = new System.Drawing.Size(71, 24);
			this.drawRadioButton.TabIndex = 1;
			this.drawRadioButton.TabStop = true;
			this.drawRadioButton.Text = "Draw";
			this.drawRadioButton.UseVisualStyleBackColor = true;
			this.drawRadioButton.CheckedChanged += new System.EventHandler(this.drawRadioButton_CheckedChanged);
			// 
			// showTileMapButton
			// 
			this.showTileMapButton.Location = new System.Drawing.Point(7, 461);
			this.showTileMapButton.Name = "showTileMapButton";
			this.showTileMapButton.Size = new System.Drawing.Size(117, 31);
			this.showTileMapButton.TabIndex = 0;
			this.showTileMapButton.Text = "Show TileMap";
			this.showTileMapButton.UseVisualStyleBackColor = true;
			this.showTileMapButton.Click += new System.EventHandler(this.showTileMapButton_Click);
			// 
			// zoneCanvasPanel
			// 
			this.zoneCanvasPanel.AutoScroll = true;
			this.zoneCanvasPanel.BackColor = System.Drawing.Color.Black;
			this.zoneCanvasPanel.Location = new System.Drawing.Point(198, 47);
			this.zoneCanvasPanel.Name = "zoneCanvasPanel";
			this.zoneCanvasPanel.Size = new System.Drawing.Size(640, 488);
			this.zoneCanvasPanel.TabIndex = 4;
			// 
			// ZoneEditor
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(986, 547);
			this.Controls.Add(this.zoneCanvasPanel);
			this.Controls.Add(this.formatGroupBox);
			this.Controls.Add(this.displayGroupBox);
			this.Controls.Add(this.menuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MainMenuStrip = this.menuStrip;
			this.MinimumSize = new System.Drawing.Size(1008, 603);
			this.Name = "ZoneEditor";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Zone Editor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ZoneEditor_FormClosing);
			this.Load += new System.EventHandler(this.ZoneEditor_Load);
			this.SizeChanged += new System.EventHandler(this.ZoneEditor_SizeChanged);
			this.menuStrip.ResumeLayout(false);
			this.menuStrip.PerformLayout();
			this.displayGroupBox.ResumeLayout(false);
			this.formatGroupBox.ResumeLayout(false);
			this.formatGroupBox.PerformLayout();
			this.clickTypePanel.ResumeLayout(false);
			this.clickTypePanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuStrip;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.GroupBox displayGroupBox;
		private System.Windows.Forms.Button editScriptButton;
		private System.Windows.Forms.ComboBox entityTypeComboBox;
		private System.Windows.Forms.GroupBox formatGroupBox;
		private System.Windows.Forms.Button showTileMapButton;
		private System.Windows.Forms.Panel zoneCanvasPanel;
		private System.Windows.Forms.RadioButton pasteRadioButton;
		private System.Windows.Forms.RadioButton deleteRadioButton;
		private System.Windows.Forms.RadioButton copyRadioButton;
		private System.Windows.Forms.RadioButton fillRadioButton;
		private System.Windows.Forms.RadioButton drawRadioButton;
		private System.Windows.Forms.ToolStripMenuItem zoneToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zoomToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem oneXZoomMenuItem;
		private System.Windows.Forms.ToolStripMenuItem twoXZoomMenuItem;
		private System.Windows.Forms.ToolStripMenuItem threeXZoomMenuItem;
		private System.Windows.Forms.ToolStripMenuItem fourXZoomMenuItem;
		private System.Windows.Forms.TextBox layerTextBox;
		private System.Windows.Forms.Label dividerLabel02;
		private System.Windows.Forms.Label dividerLabel01;
		private System.Windows.Forms.CheckBox onlyCurrentEntityCheckBox;
		private System.Windows.Forms.CheckBox backgroundCheckBox;
		private System.Windows.Forms.CheckBox tileGridCheckBox;
		private System.Windows.Forms.Button zoomOutButton;
		private System.Windows.Forms.Button zoomInButton;
		private System.Windows.Forms.ToolStripMenuItem zoomSubMenuItem;
		private System.Windows.Forms.Button openEditMenu;
		private System.Windows.Forms.Button viewAssetsButton;
		private System.Windows.Forms.Button resizeZoneButton;
		private System.Windows.Forms.Button resetEntityButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel clickTypePanel;
		private System.Windows.Forms.RadioButton highlightRadioButton;
		private System.Windows.Forms.RadioButton rectangleRadioButton;
		private System.Windows.Forms.RadioButton clickRadioButton;
		private System.Windows.Forms.Button executeButton;
		private System.Windows.Forms.RadioButton selectRadioButton;
		private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
		private System.Windows.Forms.Panel tileBoxContainerPanel;
		private System.Windows.Forms.ToolStripMenuItem setZoneSizeToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setBackgroundToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem assetViewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
	}
}