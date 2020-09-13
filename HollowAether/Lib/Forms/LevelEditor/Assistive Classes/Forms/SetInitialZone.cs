using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

#region XNAImports
using Microsoft.Xna.Framework;
#endregion

#region HollowAetherImports
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using HollowAether.Lib.MapZone;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;
#endregion

using HollowAether.Lib.InputOutput.Parsers;

namespace HollowAether.Lib.Forms {
	public partial class SetInitialZone : Form {
		private SetInitialZone(Vector2 currentValue) {
			InitializeComponent();

			xTextBox.Text = currentValue.X.ToString().PadLeft(3, '0');
			yTextBox.Text = currentValue.Y.ToString().PadLeft(3, '0');
		}

		private void SetInitialZone_Load(object sender, EventArgs e) { }

		public static Vector2? Get(Vector2? currentValue=null) {
			SetInitialZone zone = new SetInitialZone(currentValue.HasValue ? currentValue.Value : Vector2.Zero);
			DialogResult result = zone.ShowDialog(); // Display newly made instance of self as a dialog 
			
			if (result == DialogResult.OK) {
				int? xPos = GV.Misc.StringToInt(zone.xTextBox.Text), yPos = GV.Misc.StringToInt(zone.yTextBox.Text);

				if (xPos.HasValue && yPos.HasValue) return new Vector2(xPos.Value, yPos.Value); else {
					if (!xPos.HasValue) MessageBox.Show($"Couldn't cast X-Input to integer", "Num Error");
					if (!yPos.HasValue) MessageBox.Show($"Couldn't cast Y-Input to integer", "Num Error");
				}
			} else if (result != DialogResult.Cancel) Console.WriteLine($"Warning: Unknown Dialog Result In SetInitialZone {result}");

			return null; // If couldn't determine user choice from input values, return null
		}
	}
}