using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class ZoneFeatures : Form {
		public struct ZoneDetails {
			public ZoneDetails(String fpath, Vector2 pos, bool _visible) {
				visible = _visible;
				filePath = fpath;
				position = pos;
				succeeded = true;
			}

			public bool visible;
			public bool succeeded;
			public String filePath;
			public Vector2 position;

			public static ZoneDetails Failed { get { return new ZoneDetails() { succeeded = false }; } }
		}

		public ZoneFeatures() { InitializeComponent(); }

		private static ZoneDetails Get(ref ZoneFeatures form) {
			DialogResult result = form.ShowDialog(); // Show and wait until done

			int? xPos = GlobalVars.Misc.StringToInt(form.xTextBox.Text);
			int? yPos = GlobalVars.Misc.StringToInt(form.yTextBox.Text);

			bool numFail       = !xPos.HasValue || !yPos.HasValue;
			bool fileTitleFail = String.IsNullOrWhiteSpace(form.fileNameTextBox.Text);

			if (result != DialogResult.OK || numFail || fileTitleFail) {
				if (numFail) {
					if (!xPos.HasValue) MessageBox.Show($"Couldn't cast X-Input to integer", "Num Error");
					if (!yPos.HasValue) MessageBox.Show($"Couldn't cast Y-Input to integer", "Num Error");
				}
				
				if (fileTitleFail)             MessageBox.Show($"Invalid title for zone file", "Title Error");

				return ZoneDetails.Failed; // Return failed zone detail structure
			}

			return new ZoneDetails(
				IOMan.Join("Zones", form.fileNameTextBox.Text.Replace(" ", "")), 
				new Vector2(xPos.Value, yPos.Value), 
				form.visibleRadioButton.Checked
			);
		}

		public static ZoneDetails Get() { return Get("File Name.ZNE", Vector2.Zero, true, false); }

		public static ZoneDetails Get(String filePath, Vector2? position=null, bool visible=true, bool pathReadonly=true) {
			ZoneFeatures form = new ZoneFeatures();

			#region Set Vars
			form.fileNameTextBox.Text = filePath; //IOMan.GetFileTitleFromPath(filePath);
			form.fileNameTextBox.ReadOnly = pathReadonly; // Path Can Be Edited

			Vector2 pos = position.HasValue ? position.Value : Vector2.Zero;

			form.xTextBox.Text = pos.X.ToString();
			form.yTextBox.Text = pos.Y.ToString();
			#endregion

			ZoneDetails returnVal = Get(ref form);
			form.Dispose(); // Free from memory

			return returnVal; // Return details
		}
	}
}
