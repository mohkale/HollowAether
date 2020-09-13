#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
#endregion

namespace HollowAether.Lib.InputOutput {
	/// <summary>Class to manage settings operations for the game</summary>
	public class SettingsManager {
		/// <summary> Constructor auto builds settings file</summary>
		public SettingsManager() { BuildSettings(); }

		/// <summary>Method to read, interpret or create anew, the players window settings</summary>
		public void BuildSettings() {
			Dictionary<String, String> settings; // holder for settings and value pairs
			
			try { settings = (settingsWriterStream.Length == 0) ? BuildDefaultSettings() : ReadSettings(); } 
			catch { throw new XmlException("Settings File Parse Error"); } // Get settings or throw error
			
			shouldBeFullScreen = settings["fullscreen"].ToLower() == "true";

			GlobalVars.MonoGameImplement.framesPerSecond = (float)Convert.ToDouble(settings["fps"]);
			// GlobalVars.username = settings["username"]; // UName not implemented

			int width = Convert.ToInt32(settings["width"]), height = Convert.ToInt32(settings["height"]);
			GlobalVars.hollowAether.SetWindowDimensions(width, height); // set window dimensions using game
		}

		/// <summary>Saves current window settings to save file</summary>
		public void Save() {
			WriteXML(GlobalVars.Variables.windowWidth, GlobalVars.Variables.windowHeight, 
				GlobalVars.hollowAether.IsFullScreen, GlobalVars.MonoGameImplement.framesPerSecond, "UNAME");
		}

		/// <summary>Reads settings from settings file in assets folder</summary>
		/// <returns>Dictionary containing settings parameters as keys and values as values</returns>
		private Dictionary<String, String> ReadSettings() {
			XmlReader reader = XmlReader.Create(settingsWriterStream);
			Dictionary<String, String> settings = new Dictionary<String, String>();
			String lastName = String.Empty;

			while (reader.Read()) {
				bool nodeSet = reader.NodeType == XmlNodeType.Element || reader.NodeType == XmlNodeType.Text;

				if (nodeSet && reader.Name != "Settings") {
					if (reader.Name == "dimensions" && reader.HasAttributes)
						settings.Add("fullscreen", reader.GetAttribute("fullscreen"));
					else if (reader.NodeType == XmlNodeType.Element) lastName = reader.Name;
					else settings.Add(lastName, reader.Value); // Add name to settings
				}
			}

			return settings;
		}

		/// <summary> Use when no settings file has been made </summary>
		private Dictionary<String, String> BuildDefaultSettings() {
			WriteXML(1000, 640, false, 60, Environment.UserName); // Default settings
			
			return new Dictionary<String, String>() {
				{ "username", Environment.UserName }, { "fps", "60" },
				{ "width", "1000" }, { "height", "640" },
				{ "fullscreen", false.ToString() }
			};
		}

		/// <summary>Creates an XML writer with easy to use settings</summary>
		private void CreateXMLWriter() {
			XmlWriterSettings settings = new XmlWriterSettings() {
				Indent = true, IndentChars = "  ", NewLineChars = "\r\n",
				NewLineHandling = NewLineHandling.Replace
			};

			settingsWriterStream.SetLength(0L);
			settingsWriter = XmlWriter.Create(settingsWriterStream, settings);
		}

		/// <summary>Actual method to store settings</summary>
		/// <param name="windowWidth">Width of current window (Out of full screen)</param>
		/// <param name="windowHeight">Height of current window (Out of full screen)</param>
		/// <param name="fullscreen">Wether the current window is full screen or not</param>
		/// <param name="fps">The amount of frames drawn to the screen per second</param>
		/// <param name="uname">The name of current computers user (could come in useful)</param>
		private void WriteXML(float windowWidth, float windowHeight, bool fullscreen, float fps, String uname) {
			CreateXMLWriter();

			settingsWriter.WriteStartDocument();
			settingsWriter.WriteStartElement("Settings");

			settingsWriter.WriteStartElement("dimensions");
			settingsWriter.WriteAttributeString("fullscreen", fullscreen.ToString());
			WriteElement("width", windowWidth.ToString());
			WriteElement("height", windowHeight.ToString());
			settingsWriter.WriteEndElement();

			WriteElement("fps", fps.ToString());
			WriteElement("username", uname);

			settingsWriter.WriteEndElement();

			settingsWriter.WriteEndDocument();
			settingsWriter.Close();
		}

		/// <summary> Writes a single tag to the settings file</summary>
		/// <param name="elementName">Name of element/tag to append</param>
		/// <param name="value">Value given to said element/tag</param>
		private void WriteElement(String elementName, String value=null) {
			settingsWriter.WriteStartElement(elementName);
			
			if (!String.IsNullOrWhiteSpace(value))
				settingsWriter.WriteString(value);
			
			settingsWriter.WriteEndElement();
		}
		
		private XmlWriter settingsWriter; // XMLwriter can write to the xml file.
		public FileStream settingsWriterStream = new FileStream(settingsPath, FileMode.OpenOrCreate);
		public static String settingsPath = Path.Combine(Directory.GetCurrentDirectory(), @"Assets\settings.xml");
		public bool shouldBeFullScreen = false;
	}
}
