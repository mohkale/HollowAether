using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CS = HollowAether.Text.ColoredString;

namespace HollowAether.Lib.Misc {
	/// <summary>Class to hold ugly undesireable code. Basically HollowAethers dirty laundry</summary>
	public static class CommandLineHelp {
		/// <remarks>Efficiency is down the drain here</remarks>
		public static IEnumerable<Object> GenerateHelpString() {
			List<Object> help = new List<Object>() {
				#region Usage
				"Usage:", CS.Red(" HollowAether "),

				"(", CS.Red("-r"), " ^| ", CS.Red("-lE"), " ^| ", CS.Red("-dR"), " ^| ", CS.Red("-rZ"), " ^| ", CS.Red("-h"), ") ",

				"[-", CS.Green("eF"), " ", CS.Yellow("file"), " [", CS.Yellow("tar_file"), "]] ",
				"[-", CS.Green("dF"), " ", CS.Yellow("file"), " [", CS.Yellow("tar_file"), "]] ",
				"[-", CS.Green("eD"), " ", CS.Yellow("dir"), " [", CS.Yellow("tar_dir"), "]] ",
				"[-", CS.Green("dD"), " ", CS.Yellow("dir"), " [", CS.Yellow("tar_dir"), "]] ",
				"[-", CS.Green("eP"), " ", CS.Yellow("file"), "] ",
				"[-", CS.Green("dP"), " ", CS.Yellow("file"), "] ",
				"[-", CS.Green("sZ"), " (", CS.Red("X"), ":", CS.Yellow("x_val"), " ", CS.Red("Y"), ":", CS.Yellow("y_val"), ")] ",
				"[-", CS.Green("m"), CS.Yellow(" map_file"), "] ",
				"[-", CS.Green("sA"), CS.Yellow(" anim_dir"), "] ",
				"[-", CS.Green("z"), CS.Yellow(" float"), "] ",
				"[-", CS.Green("60fps"), "] ",
				"[-", CS.Green("30fps"), "] ",
				"[-", CS.Green("20fps"), "] ",
				"[-", CS.Green("15fps"), "] ",
				"[-", CS.Green("f"), "] ",
				"[-", CS.Green("w"), "] ",
				"[-", CS.Green("tE"), "] \n\n",
				#endregion
			};// File encoding should be Unicode
			
			#region Blurb
			help.AddRange(new List<Object> { CS.Red("HollowAether: "), "Program Version 0.13 (", CS.Blue("C"), ") Mohsin L. C. Kale" });
			#endregion

			foreach (Object obj in help)
				yield return obj;
		}
	}
}
