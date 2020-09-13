using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.IO;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using HollowAether.Lib.InputOutput;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.MapZone;
using System.Windows.Controls;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class ScriptEditor : Form {
		public ScriptEditor(String fpath, bool? _isZoneFile=null, bool readNow = true) {
			InitializeComponent();

			if (!InputOutputManager.FileExists(fpath)) {
				MessageBox.Show(
					$"Could Not Find File '{fpath}'", 
					"File Doesn't Exist Error",
					MessageBoxButtons.OK, 
					MessageBoxIcon.Error
				);

				Dispose(true);
			}

			filePath = fpath; width = Width; height = Height;

			//initialFileContents = InputOutputManager.ReadEncryptedFile(fpath, GlobalVars.oneTimePad);
			if (readNow) ReadFile(InputOutputManager.ReadEncryptedFile(fpath, GlobalVars.Encryption.oneTimePad), _isZoneFile);
			//if (_isZoneFile.HasValue) isZoneFile = _isZoneFile.Value; else DetermineFileType();
		}

		public static ScriptEditor EditorFromFileContents(String contents, String fpath, bool? _isZoneFile = null) {
			ScriptEditor editor = new ScriptEditor(fpath, readNow: false); // Create new zone editor but don't read file
			editor.ReadFile(contents, _isZoneFile); return editor; // Pass contents as file contents then return editor
		}

		public void ReadFile(String fContents, bool? _isZoneFile) {
			initialFileContents = fContents; // Store file contents passed via arg
			if (_isZoneFile.HasValue) isZoneFile = _isZoneFile.Value; else DetermineFileType();
		}

		public void ChangeFileContents(String text) {
			((Controls[indexOfTextEditorControl] as ElementHost).Child as TextEditor).Text = text;
		}

		/// <summary>Builds AvalonEditor and set's syntax for desired type</summary>
		private void ScriptEditor_Load(object sender, EventArgs e) {
			TextEditor editor = new TextEditor() {HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled};

			bool syntaxSet = (isZoneFile) ? BuildZoneSyntaxHighlighting(ref editor) : BuildMapSyntaxHighlighting(ref editor);

			#region MessageBox
			if (!syntaxSet)
				MessageBox.Show("Couldn't Find Appropriate Syntax File", 
					"Syntax Find Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			#endregion

			indexOfTextEditorControl = Controls.Count;

			editor.FontFamily = new System.Windows.Media.FontFamily("Courier New");
			editor.FontSize = 10;

			this.Controls.Add(new ElementHost() {
				Size = new Size(Width - 18 - 5, Height - 60), Child = editor,
				Location = new Point(5, menuStrip.Height + Padding.Top)
			});

			editor.Text = initialFileContents; // store contents of file
		}

		/// <summary>Thrown when form dimensions have changed</summary>
		private void ScriptEditor_SizeChanged(object sender, EventArgs e) {
			Controls[indexOfTextEditorControl].Size = GetTextEditorSize();
		}

		private void ScriptEditor_KeyDown(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Down) {
				Console.WriteLine("Push Down");
			}
		}

		/// <summary>Determines filetype of file when not expressly given</summary>
		private void DetermineFileType() {
			/*if (Map.CheckMapHeader(initialFileContents)) { isZoneFile = false; } 
			else if (Map.CheckMapHeader(initialFileContents)) { isZoneFile = true; } 
			else {
				#region ShowError
				MessageBox.Show($"File '{filePath}' Isn't Of Type Map Or Header", "File Read Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Dispose(true);
				#endregion
			}*/
		}

		/// <summary>Builds editor syntax coloring for map files</summary>
		/// <param name="editor">TextEditor control</param>
		/// <returns>Boolean indicating whether syntax was set</returns>
		private bool BuildMapSyntaxHighlighting(ref TextEditor editor) {
			if (!InputOutputManager.FileExists(mapSyntaxFile)) return false;

			try {
				using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(mapSyntaxFile)) {
					editor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"[^ \n\r]+\\[^ \n\r]+"), //(@"\w:\\[^\r\n]+"),
					Color = editor.SyntaxHighlighting.GetNamedColor("String")
				});

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"([Tt]rue)|([Ff]alse)"),
					Color = editor.SyntaxHighlighting.GetNamedColor("Boolean")
				});

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"([Xx|Yy]:\d+)"),
					Color = editor.SyntaxHighlighting.GetNamedColor("Vector")
				});

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"([Ss]tart[Zz]one)|([Dd]efine[Ff]lag)"),
					Color = editor.SyntaxHighlighting.GetNamedColor("Command")
				});

				return true;
			} catch { return false; }
		}

		/// <summary>Builds editor syntax coloring for zone files</summary>
		/// <param name="editor">TextEditor control</param>
		/// <returns>Boolean indicating whether syntax was set</returns>
		private bool BuildZoneSyntaxHighlighting(ref TextEditor editor) {
			if (!InputOutputManager.FileExists(zoneSyntaxFile)) return false;

			try {
				using (System.Xml.XmlReader reader = System.Xml.XmlReader.Create(zoneSyntaxFile)) {
					editor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
				}

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"\[[^\n\r]+\]"),
					Color = editor.SyntaxHighlighting.GetNamedColor("ID")
				});

				editor.SyntaxHighlighting.MainRuleSet.Rules.Add(new HighlightingRule() {
					Regex = new System.Text.RegularExpressions.Regex(@"\([^\n\r]+\)"),
					Color = editor.SyntaxHighlighting.GetNamedColor("Attribute")
				});

				editor.TextArea.TextView.Redraw();
				return true;
			} catch { return false; }
		}

		/// <summary>Get's new editor size after form resize</summary>
		private Size GetTextEditorSize() {
			return new Size(Width - 18 - 5, Height - 60 - menuStrip.Height - Padding.Top);
		}

		public String GetEditorTextContents() {
			return ((Controls[indexOfTextEditorControl] as ElementHost).Child as TextEditor).Text;
		}

		private static String mapSyntaxFile = InputOutput.InputOutputManager.Join(Editor.levelEditorBasePath, "MapSyntax.XSHD");
		private static String zoneSyntaxFile = InputOutput.InputOutputManager.Join(Editor.levelEditorBasePath, "ZoneSyntax.XSHD");
		private int width, height, indexOfTextEditorControl;

		public bool isZoneFile;
		private String initialFileContents, filePath;
	}
}
