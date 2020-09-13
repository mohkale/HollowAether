#region SystemImports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
#endregion

namespace HollowAether.Lib.Forms {
	public partial class ErrorMessage : Form {
		public ErrorMessage(params Exception[] _exceptions) {
			InitializeComponent();
			exceptions.AddRange(_exceptions);
		}

		private void EMB_Load(object sender, EventArgs e) {
			BuildTextBox();

			ContextMenu cm = new ContextMenu();

			cm.MenuItems.Add(new MenuItem("Copy", new EventHandler(CopyButton_Click)));
			cm.MenuItems.Add(new MenuItem("Delete", new EventHandler(ClearButton_Click)));
			
			textBox.ContextMenu = cm;
		}

		public void BuildTextBox() {
			StringBuilder builder = new StringBuilder();
			textBox.SelectionAlignment = HorizontalAlignment.Center;
			builder.AppendLine($"{exceptions.Count.ToString().PadLeft(3, '0')} Exceptions Found");

			foreach (Exception exc in exceptions) {
				builder.AppendLine(GetSeperator()); // Add ----- etc.

				#region DetailDefinitions
				String destination = (exc.TargetSite != null) ? exc.TargetSite.ToString() : "Unknown";
				String message = (exc.Message != null) ? exc.Message : "Unknown"; // Store a given message
				//String lineAndClass = (exc.StackTrace != null) ? exc.StackTrace.ToString().Split('\\').Last() : "Unknown:Unknown";
				String[] lineAndClassList = (exc.StackTrace != null) ? exc.StackTrace.Split('\n')[0].Split('\\').Last().Split(' ') : null;
				String lineAndClass = (lineAndClassList != null) ? lineAndClassList[0] + ' ' + lineAndClassList[1].PadLeft(3).Replace('\r',' ') : "Unknown:Unknown";
				String lineInFile = null, pathToFile = null, argRange = null, commandAssistanceString = null, commandLineCmd = null;
				#endregion

				#region ExtractDetailsFromException
				if (exc is AnimationException) {

				} else if (exc is CollisionException) {

				} else if (exc is FatalException) {

				} else if (exc is HollowAetherException) {

				} else if (exc is NullReferenceException) {
					
				} else { // Is Exception

				}
				#endregion

				#region AppendToBuilder
				builder.AppendLine($"Exception Of Type {exc.GetType()}");
				builder.AppendLine($"Thrown In Method: {destination}");

				if (commandLineCmd != null) {
					builder.AppendLine($"In Command Line Command: '{commandLineCmd}'");
					builder.AppendLine($"Which Can Accept: {argRange} Arguments");
					builder.AppendLine($"Command Help: {commandAssistanceString}");
				}

				builder.AppendLine($"In Class\\Line: {lineAndClass}");
				builder.AppendLine($"Message: {message}");

				if (pathToFile != null && lineInFile != null) {
					builder.AppendLine($"In File: '{pathToFile}'. At Line: {lineInFile.PadLeft(3, '0')}");
				} else if (pathToFile != null) {
					builder.AppendLine($"In File: '{pathToFile}'");
				} else if (lineInFile != null) {
					builder.AppendLine($"At Line: {lineInFile.PadLeft(3, '0')}");
				}
				#endregion
			}

			textBox.Text = builder.ToString(); // Set Text To Builder Contents
		}

		public String GetSeperator(int span=50) {
			return $"\n{(from X in Enumerable.Range(0, span) select "—").Aggregate((a, b) => a + b)}";
		}

		public void Clear() {
			textBox.Text = String.Empty;
		}

		public void AddException(Exception exc) {
			exceptions.Add(exc);
		}

		private void EMB_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode ==  System.Windows.Forms.Keys.Escape) {
				Application.Exit();
			}
		}

		private void AbortButton_Click(object sender, EventArgs e) {
			Application.Exit();
		}

		private void CopyButton_Click(object sender, EventArgs e) {
			String text;

			if (String.IsNullOrWhiteSpace(textBox.SelectedText))
				text = textBox.Text; // If textbox doesn't have any selected characters
			else text = textBox.SelectedText; // If text box has selected characters

			try { Clipboard.SetText(text); } catch { Clipboard.Clear(); }
		}

		private void ClearButton_Click(object sender, EventArgs e) {
			Clear();
		}

		protected override bool ShowWithoutActivation {
			get { return true; }
		}

		private List<Exception> exceptions = new List<Exception>();
	}
}
