using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class Help : Form {
		public Help() {
			InitializeComponent();
		}

		private void Help_Load(object sender, EventArgs e) {

		}

		private void Help_FormClosing(object sender, FormClosingEventArgs e) {
			if (e.CloseReason == CloseReason.UserClosing) {
				e.Cancel = true;
				Hide();
			}
		}
	}
}
