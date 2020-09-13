using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.InputOutput.Parsers;
using HollowAether.Lib.MapZone;

#region XNAImports
using Microsoft.Xna.Framework;
using HollowAether.Lib.GAssets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.InputOutput {
	public static class SaveManager {
		public struct SaveFile {
			public SaveFile(INIParser.SectionContainer section) {
				bool gsExists = section.ContainsKey("GameState");
				bool psExists = section.ContainsKey("PlayerState");
				bool paExists = section.ContainsKey("PerksAcquired");
				bool ftExists = section.ContainsKey("FlagsTripped");

				if (gsExists && psExists && paExists && ftExists) {
					currentZone    = Parser.Vector2Parser(section["GameState"]["Zone"]);
					playerPosition = Parser.Vector2Parser(section["PlayerState"]["Position"]);

					flags = (from X in section["FlagsTripped"] select new Tuple<String, bool>(X.Key, Parser.BooleanParser(X.Value))).ToArray();

					int? healthMaybe     = GV.Misc.StringToInt(section["PlayerState"]["Health"]);

					if (healthMaybe.HasValue) health = healthMaybe.Value; else {
						throw new SaveSystemException($"Invalid Health Value '{section["PlayerState"]["Health"]}'");
					}

					int? heartCountMaybe = GV.Misc.StringToInt(section["PlayerState"]["HeartCount"]);

					if (heartCountMaybe.HasValue) heartCount = heartCountMaybe.Value; else {
						throw new SaveSystemException($"Invalid Heart Count Value '{section["PlayerState"]["HeartCount"]}'");
					}

					perks = (from X in section["PerksAcquired"] select (PlayerPerks)Enum.Parse(typeof(PlayerPerks), X.Value)).ToArray();
				} else { // Doesn't exist, throw error
					if      (!gsExists)  throw new SaveFileCorruptedException($"GameState");
					else if (!psExists)  throw new SaveFileCorruptedException($"PlayerState");
					else if (!paExists)  throw new SaveFileCorruptedException($"PerksAcquired");
					else /*(!ftExists)*/ throw new SaveFileCorruptedException($"FlagsTripped");
				}
			}

			public SaveFile(Vector2 zoneIndex, Vector2 playerPos, int playerHealth, int numOfHearts, PlayerPerks[] _perks, Tuple<String, bool>[] _flags) {
				currentZone    = zoneIndex;
				playerPosition = playerPos;
				health         = playerHealth;
				heartCount     = numOfHearts;
				perks          = _perks;
				flags          = _flags;
			}

			public INIParser.SectionContainer ToINIContainer() {
				INIParser.SectionContainer container = new INIParser.SectionContainer();

				container.AddSection("GameState");
				container["GameState"]["Zone"] = Converters.ValueToString(typeof(Vector2), currentZone);

				container.AddSection("PlayerState"); // Current player details
				container["PlayerState"]["Health"]     = health.ToString(); // Int representing health
				container["PlayerState"]["Position"]   = Converters.ValueToString(typeof(Vector2), playerPosition);
				container["PlayerState"]["HeartCount"] = heartCount.ToString(); // Int representing number of hearts

				container.AddSection("PerksAcquired");

				foreach (int X in Enumerable.Range(0, perks.Length)) {
					container["PerksAcquired"][$"Perk{X + 1}"] = perks[X].ToString();
				}

				container.AddSection("FlagsTripped"); // Section for any tripped flags etc.

				foreach (Tuple<String, bool> flagTuple in flags) {
					container["FlagsTripped"][flagTuple.Item1] = Converters.ValueToString(typeof(bool), flagTuple.Item2);
				}

				return container;
			}

			public static SaveFile GetCurrentSave() {
				Vector2 playerPos = GV.MonoGameImplement.Player.Position;

				return new SaveFile() {
					currentZone    = GV.MonoGameImplement.map.Index,
					health         = GV.MonoGameImplement.GameHUD.GetDisplayedHealth(),
					playerPosition = new Vector2((float)Math.Round(playerPos.X), (float)Math.Round(playerPos.Y)),
					heartCount     = GV.MonoGameImplement.GameHUD.HeartsCount,
					perks          = GV.MonoGameImplement.Player.Perks,
					flags		   = (from X in GV.MapZone.GetFlagAssets() where (X.asset as Flag).ValueChangedFromDefault()
						select new Tuple<String, bool>(X.assetID, (X.asset as Flag).Value)).ToArray()
				};
			}

			public static SaveFile GetEmptySave() {
				return new SaveFile(
					GV.MonoGameImplement.map.GetStartZoneIndexOrEmpty(),
					new Vector2(0, 0), 
					32, 8,
					new PlayerPerks[] { },
					new Tuple<string, bool>[] { }
				);
			}

			public Vector2 currentZone, playerPosition;
			public int health, heartCount;
			public PlayerPerks[] perks;
			public Tuple<String, bool>[] flags;
		}

		public static void CreateBlankSave(int slotIndex) {
			String path = GetSaveFilePathFromSlotIndex(slotIndex);
			string contents = $"{SAVE_HEADER}\n\n{SaveFile.GetEmptySave().ToINIContainer().ToFileContents()}";
			InputOutputManager.WriteEncryptedFile(path, contents, GV.Encryption.oneTimePad);
		}

		public static void SaveCurrentGameState(int slotIndex) {
			String path = GetSaveFilePathFromSlotIndex(slotIndex); // Get file path
			String contents = $"{SAVE_HEADER}\n\n{SaveFile.GetCurrentSave().ToINIContainer().ToFileContents()}";
			InputOutputManager.WriteEncryptedFile(path, contents, GV.Encryption.oneTimePad); // Write file
			InputOutputManager.WriteFile(path+".NEC", contents); // Save unencrypted as well for now
		}

		public static SaveFile GetSave(int slotIndex) {
			String path = GetSaveFilePathFromSlotIndex(slotIndex);

			if (!InputOutputManager.FileExists(path)) // If save doesn't exist
				throw new SaveFileDoesntExistAtChosenSlotException(slotIndex, path);

			String decryptedContents = InputOutputManager.ReadEncryptedFile(path, GV.Encryption.oneTimePad);

			INIParser.SectionContainer contents = INIParser.Parse(decryptedContents, path, false, true);

			return new SaveFile(contents); // Read file contents and parse to desired data structure
		}

		public static void DeleteSave(int slotIndex) {
			String path = GetSaveFilePathFromSlotIndex(slotIndex);

			if (InputOutputManager.FileExists(path)) // Shouldn't need to
				System.IO.File.Delete(path); // If exists delete
		}

		public static bool SaveExists(int index) {
			return InputOutputManager.FileExists(GetSaveFilePathFromSlotIndex(index));
		}

		public static String GetSaveFilePathFromSlotIndex(int slotIndex) {
			if (slotIndex > SAVE_SLOTS || slotIndex < 0)
				throw new SaveSlotOutOfRangeException(slotIndex, SAVE_SLOTS);

			return InputOutputManager.Join(GV.FileIO.assetPaths["Save"], $"Save {slotIndex+1}.SVE");
		}

		public const int SAVE_SLOTS = 3;

		public const String SAVE_HEADER = "SVE";

		/// <summary>Holds current game slot statically for many uses</summary>
		/// <remarks>Will throw exception with default value</remarks>
		public static int CurrentSlot { get; private set; } = -1;
	}
}
