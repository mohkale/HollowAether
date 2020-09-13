#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
#endregion

#region HollowAetherImports
using HollowAether.Lib;
using HollowAether.Text;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Forms;
using HollowAether.Lib.InputOutput;
using MapZone = HollowAether.Lib.MapZone;
using CS = HollowAether.Text.ColoredString;
using U = HollowAether.Lib.Misc.CommandLineHelp;
#endregion
using HollowAether.Lib.InputOutput.Parsers;

using Microsoft.Xna.Framework;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether {
	public static class Program {
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool SetProcessDPIAware();

#if WINDOWS || LINUX
		[STAThread] static void Main(params String[] args) {
			Application.EnableVisualStyles(); Application.SetCompatibleTextRenderingDefault(false);
			if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware(); // Dots per Inch
			// Builds Generic Application Preferences; Must Come Before First Object Creation //

			try { ParseCommandLine(args); } catch (Exception e) {
				// Run A New Error Message Form With All Found Exceptions

				if (throwException) throw;

				if (GlobalVars.hollowAether.god != null)
					GlobalVars.hollowAether.god.EndGame();

				ErrorMessage form = new ErrorMessage(e);
				
				while (e.InnerException != null) {
					form.AddException(e.InnerException);
					e = e.InnerException; // add inner
				}
				
				Application.Run(form);
			}
		}
#else
		#region NonCompatibleOSHandler
		[STAThread] static void Main() {
			String incorrectOS = "Sorry, HollowAether Isn't Compatible With Your Current OS :(";
			MessageBox.Show(incorrectOS, "OS Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
		#endregion
#endif

		private static void ParseCommandLine(String[] args) {
			if (args.Length == 0) { /*RunGame();*/ return; } // If no args given then run the game and return to caller

			String str = (from X in args select (X.Count(N => N == ' ') >= 1) ? $"\"{X}\"" : X).Aggregate((Y, Z) => $"{Y} {Z}");
			// Parses through every word in args and tries to re-interpret them as they would appear if passed from the command line

			Dictionary<String, String[]> parsedCLArguments = StartUpMethods.InputParserScriptInterpreter(str); // parses CLA
			// bool runGameAfterArgs = false, runLevelEditorAfterArgs = false, dontRun = false; // arg interpretor bools

			#region Implement
			foreach (String flag in parsedCLArguments.Keys) {
				switch (flag.ToLower()) {
					case "\0": // default args
						break;
					#region RunGame
					case "r":
					case "run":
					case "rungame":
					case "run_game":
					#endregion
						runGameAfterArgs = true;
						break;
					#region LevelEditor
					case "le":
					case "leveleditor":
					case "level_editor":
					#endregion
						runLevelEditorAfterArgs = true;
						break;
					#region DontRun
					case "dr":
					case "dontrun":
					case "dont_run":
					#endregion
						dontRun = true;
						break;
					#region runZone
					case "rz":
					case "runzone":
					case "run_zone":
					#endregion
						runZoneAfterArgs = true;
						break;
					#region EncryptFile
					case "ef":
					case "encryptfile":
					case "encrypt_file":
					#endregion
						EncryptFile(parsedCLArguments[flag]);
						break;
					#region EncryptDirectory
					case "ed":
					case "encryptdir":
					case "encrypt_dir":
					case "encryptdirectory":
					case "encrypt_directory":
					#endregion
						EncryptDirectory(parsedCLArguments[flag]);
						break;
					#region DecryptFile
					case "df":
					case "decryptfile":
					case "decrypt_file":
					#endregion
						DecryptFile(parsedCLArguments[flag]);
						break;
					#region DecryptDirectory
					case "dd":
					case "decryptdir":
					case "decrypt_dir":
					case "decryptdirectory":
					case "decrypt_directory":
					#endregion
						DecryptDirectory(parsedCLArguments[flag]);
						break;
					#region Map
					case "m":
					case "map":
					#endregion
						SetMap(parsedCLArguments[flag]);
						break;
					#region FullScreen
					case "f":
					case "fullscreen":
					case "full_screen":
					#endregion
						SetFullScreen();
						break;
					#region Windowed
					case "w":
					case "windowed":
					#endregion
						SetWindowed();
						break;
					#region 60FPS
					case "60fps":
					case "fps60":
					#endregion
						GlobalVars.MonoGameImplement.framesPerSecond = 60;
						break;
					#region 30FPS
					case "30fps":
					case "fps30":
					#endregion
						GlobalVars.MonoGameImplement.framesPerSecond = 30;
						break;
					#region 20FPS
					case "20fps":
					case "fps20":
					#endregion
						GlobalVars.MonoGameImplement.framesPerSecond = 20;
						break;
					#region 15FPS
					case "15fps":
					case "fps15":
					#endregion
						GlobalVars.MonoGameImplement.framesPerSecond = 15;
						break;
					#region Zoom
					case "z":
					case "zoom":
					#endregion
						SetZoom(parsedCLArguments[flag]);
						break;
					#region SetZone
					case "sz":
					case "setzone":
					case "set_zone":
					#endregion
						SetZone(parsedCLArguments[flag]);
						break;
					#region ThrowException
					case "te":
					case "throwexception":
					case "throw_exception":
					#endregion
						throwException = true;
						break;
					#region EncryptPrint
					case "ep":
					case "encryptprint":
					case "encrypt_print":
					#endregion
						EncryptEcho(parsedCLArguments[flag]);
						break;
					#region DecryptPrint
					case "dp":
					case "decryptprint":
					case "decrypt_print":
					#endregion
						DecryptEcho(parsedCLArguments[flag]);
						break;
					#region Help
					case "h":
					case "help":
					#endregion
						PrintHelp();
						dontRun = true;
						break;
					default:
						Console.WriteLine($"CLIWarning: Flag '{flag}' Not Found");
						break;
				}
			}
			#endregion
			
			if (dontRun) return; // don't run takes command precedence when passed from the command line

			int C = new int[] { runZoneAfterArgs ? 1 : 0, runGameAfterArgs ? 1 : 0, runLevelEditorAfterArgs ? 1 : 0 }.Aggregate((a, b) => a + b);
			// Check whether more then one of the necessary HollowAether run bools is true by aggregating them using an integer
			
			if (C >= 2) { throw new HollowAetherException("More Then One Necessary HollowAether Argument Passed"); }
			
			if (C == 0 || runGameAfterArgs) {
				RunGame();
			} else if (runLevelEditorAfterArgs) {
				Application.Run(new Editor(GlobalVars.FileIO.DefaultMapPath));
			} else { // RunZoneAfterArgs
				//RunZone(parsedCLArguments[]);
			}
		}

		#region CommandLineImplementers
		/// <summary>Runs HollowAether Game</summary>
		private static void RunGame() {
			GlobalVars.hollowAether.Run();
		}

		/// <summary>Set's Hollow Aether Game To Full Screen</summary>
		private static void InitializeFullScreen() {
			GlobalVars.MonoGameImplement.FullScreen = true;
		}

		/// <summary>Decrypts A Given File Using OneTimePad</summary>
		private static void DecryptFile(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new DecryptFileException("No Arguments Given");
			if (commandLineArgs.Length > 2) throw new DecryptFileException("Too Many Args Given");

			String path = commandLineArgs[0], tar = (commandLineArgs.Length == 2) ? commandLineArgs[1] : null;
			if (!InputOutputManager.FileExists(path)) throw new DecryptFileArgNotFoundException(path);

			if (String.IsNullOrWhiteSpace(tar)) // Sets target file when no given target file is passed
				tar = InputOutputManager.ChangeExtension(path, "D"+InputOutputManager.GetExtension(path).Replace(".",""));

			InputOutputManager.WriteFile(tar, InputOutputManager.ReadEncryptedFile(path, GlobalVars.Encryption.oneTimePad));
		}

		/// <summary>Encrypts A Given File Using OneTimePad</summary>
		private static void EncryptFile(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new EncryptFileException("No Arguments Given");
			if (commandLineArgs.Length > 2) throw new EncryptFileException("Too Many Args Given");

			String path = commandLineArgs[0], tar = (commandLineArgs.Length == 2) ? commandLineArgs[1] : null;
			if (!InputOutputManager.FileExists(path)) throw new EncryptFileArgNotFoundException(path);
			
			if (String.IsNullOrWhiteSpace(tar)) // Sets target file when no given target file is passed
				tar = InputOutputManager.ChangeExtension(path, "E"+InputOutputManager.GetExtension(path).Replace(".",""));

			InputOutputManager.WriteEncryptedFile(tar, InputOutputManager.ReadFile(path), GlobalVars.Encryption.oneTimePad);
		}

		/// <summary>Decrypts all files including sub files in a given directory using OneTimePad</summary>
		private static void DecryptDirectory(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new DecryptDirectoryException("No Arguments Given");
			if (commandLineArgs.Length > 2) throw new DecryptDirectoryException("Too Many Args Given");

			String path = commandLineArgs[0], tar = (commandLineArgs.Length == 2) ? commandLineArgs[1] : null;
			if (!InputOutputManager.DirectoryExists(path)) throw new DecryptDirectoryArgNotFoundException(path);

			if (String.IsNullOrWhiteSpace(tar)) tar = InputOutputManager.Join(path, "Decrypted");
			InputOutputManager.SequenceConstruct(tar); // Construct all folders upto target
			
			foreach (String sysPath in StartUpMethods.SystemParserScriptInterpreter(path)) {
				String truncatedPath = sysPath.Replace(path + "\\", ""); // just path after
				String containingDirs = InputOutputManager.GetDirectoryName(truncatedPath);
				String fileTar = InputOutputManager.Join(tar, truncatedPath); // store in tar folder
				InputOutputManager.SequenceConstruct(InputOutputManager.GetDirectoryName(fileTar));

				DecryptFile(new String[] { sysPath, fileTar }); // Decrypt given file
			}
		}

		/// <summary>Encrypts all files including sub files in a given directory using OneTimePad</summary>
		private static void EncryptDirectory(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new DecryptDirectoryException("No Arguments Given");
			if (commandLineArgs.Length > 2) throw new DecryptDirectoryException("Too Many Args Given");

			String path = commandLineArgs[0], tar = (commandLineArgs.Length == 2) ? commandLineArgs[1] : null;
			if (!InputOutputManager.DirectoryExists(path)) throw new DecryptDirectoryArgNotFoundException(path);

			if (String.IsNullOrWhiteSpace(tar)) tar = InputOutputManager.Join(path, "Encrypted");
			InputOutputManager.SequenceConstruct(tar); // Construct all folders upto target

			foreach (String sysPath in StartUpMethods.SystemParserScriptInterpreter(path)) {
				String truncatedPath = sysPath.Replace(path + "\\", ""); // just path after
				String containingDirs = InputOutputManager.GetDirectoryName(truncatedPath);
				String fileTar = InputOutputManager.Join(tar, truncatedPath); // store in tar folder
				InputOutputManager.SequenceConstruct(InputOutputManager.GetDirectoryName(fileTar));

				EncryptFile(new String[] { sysPath, fileTar }); // Decrypt given file
			}
		}

		/// <summary>Set's map file used by program</summary>
		private static void SetMap(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new SetGameZoomException("No Arguments Given");
			if (commandLineArgs.Length > 1)  throw new SetGameZoomException("Too Many Args Given");

			/*if (!InputOutputManager.FileExists(commandLineArgs[0]))
				throw new MapNotFoundException(commandLineArgs[0]);*/

			GlobalVars.FileIO.DefaultMapPath = commandLineArgs[0];
		}

		/// <summary>Set's zoom used by hollow aether</summary>
		private static void SetZoom(String[] commandLineArgs) {
			if (commandLineArgs.Length == 0) throw new SetGameZoomException("No Arguments Given");
			if (commandLineArgs.Length > 1) throw new SetGameZoomException("Too Many Args Given");

			try { GlobalVars.MonoGameImplement.gameZoom = float.Parse(commandLineArgs[0]); } catch (FormatException e) {
				throw new SetGameZoomArgumentIncorrectTypeException(commandLineArgs[0], e);
			}
		}

		/// <summary>Makes game full screen</summary>
		private static void SetFullScreen() { GlobalVars.MonoGameImplement.FullScreen = true; }

		/// <summary>Makes game windowed</summary>
		private static void SetWindowed() { GlobalVars.MonoGameImplement.FullScreen = false; }

		/// <summary>Set's the zone index in the current zone file to target with the game</summary>
		private static void SetZone(String[] args) {
			if (!runGameAfterArgs && !runZoneAfterArgs) Console.WriteLine("CLIWarning: Setting Zone When RunGame Hasn't Been Set");
			if (args.Length == 0) throw new HollowAetherException($"SetZone command has no args given to it");

			String mergedArgs = args.Aggregate((a, b) => $"{a} {b}"); // compile args to single string of desired length											  

			try { argZoneIndex = Parser.Vector2Parser(mergedArgs); } catch (Exception e) {
				throw new HollowAetherException($"Couldn't convert given argument '{mergedArgs}' to a Vector2 Value", e);
			}
		}

		public static void EncryptEcho(String[] args) {
			if (args.Length == 0) throw new HollowAetherException($"Encrypt Echo command has no args given to it");
			if (!InputOutputManager.FileExists(args[0])) throw new System.IO.FileNotFoundException(args[0]);

			Console.WriteLine(GlobalVars.Encryption.oneTimePad.Encrypt(InputOutputManager.ReadFile(args[0])));
		}

		public static void DecryptEcho(String[] args) {
			if (args.Length == 0) throw new HollowAetherException($"Decrypt Echo command has no args given to it");
			if (!InputOutputManager.FileExists(args[0])) throw new System.IO.FileNotFoundException(args[0]);

			Console.WriteLine(InputOutputManager.ReadEncryptedFile(args[0], GlobalVars.Encryption.oneTimePad));
		}

		private static void RunZone(String[] args) {
			Console.WriteLine("Run zone not yet done");
		}

		/// <summary>Prints help in nice pretty colors</summary>
		private static void PrintHelp() {
			foreach (Object str in U.GenerateHelpString()) {
				if (str is String) Console.Write(str);
				else ((ColoredString)str).Write();
			}

			Console.WriteLine(); // Leave line break after string
		}
		#endregion

		/// <summary>Bool indicating whether or not to run anything</summary>
		private static bool dontRun = false;

		/// <summary>Bool indicating whether to not catch exceptions and let them be thrown</summary>
		private static bool throwException = false;

		/// <summary>Bool indicating whether to run the game window</summary>
		private static bool runGameAfterArgs = false;

		/// <summary>Bool indicating whether to uns a single zone</summary>
		private static bool runZoneAfterArgs = false;

		/// <summary>Bool indicating whether to run Game level editor</summary>
		private static bool runLevelEditorAfterArgs = false;

		public static Vector2? argZoneIndex = null;
		public static String argZoneFile; // When just running a zone
	}
}