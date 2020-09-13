using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using System.Text.RegularExpressions;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.Exceptions.CE;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.MapZone;

namespace HollowAether.Lib.InputOutput.Parsers {
	static partial class ZoneParser {
		private static void ImportAsset(String line) {
			if (!Regex.IsMatch(line, String.Format(@"{0}\w+{0} [\w\\\\]+ as \[\w+\]", String.Format(@"[\{0}\{1}]", '"', "'"))))
				throw new MapSyntaxException(zone.FilePath, $"Syntax of Import Asset statement is incorrect", lineIndex);

			String assetType  = SpeechMarkParser(line), assetID = GetAssetIDs(line)[0]; // stores values
			String assetValue = line.Replace($"\"{assetType}\" ", "").Replace($" as [{assetID}]", "");

			try {
				Asset asset = Converters.StringToAsset(assetType, assetID, assetValue);
				AddAsset(assetID, asset); // Add asset to stored local assets

				if (Zone.StoreAssetTrace) {
					zone.ZoneAssetTrace.AddImportAsset(assetID, assetType, assetValue);
				}
			} catch (Exception e) {
				throw new ZoneCouldNotFindAssetToImportException(zone.FilePath, typeof(String), assetID, lineIndex, e);
			}
		}

		private static void DefineAsset(String line) {
			String assetID = GetAssetIDs(line)[0]; // ID given to said asset, found in [square brackets]
			String assetType = SpeechMarkParser(line); // Type of asset, extracted from "" in line
			String value = line.Replace($"[{assetID}] ", "").Replace($"\"{assetType}\"", "").Trim();

			Asset asset = Converters.StringToAsset(assetType, assetID, value);
			AddAsset(assetID, asset); // stores asset in local store

			if (Zone.StoreAssetTrace) { zone.ZoneAssetTrace.AddDefineAsset(assetID, assetType, value); }
		}

		private static void Set(String line) {
			String[] IDs = GetAssetIDs(line);

			#region ThrowEception
			if (IDs.Length == 0)
				throw new ZoneSyntaxException(zone.FilePath, $"No Asset ID Given In Line '{line}'", lineIndex);

			if (!AssetExists(IDs[0]))
				throw new ZoneAssetNotFoundException(zone.FilePath, IDs[0], lineIndex);
			#endregion

			/*if (Regex.IsMatch(line, @"[Bb]ind[Tt]o")) { // BindTo set type
				#region ThrowEception
				if (IDs.Length != 2)
					throw new ZoneSyntaxException(zone.FilePath, $"Only One ID found in line {line}", lineIndex);
				#endregion

				String[] _bindToFlag = GetAssetIDs(BracketParser(line));

				#region ThrowEception
				if (_bindToFlag.Length == 0)
					throw new ZoneSyntaxException(zone.FilePath, $"Could not find target flag to bind to in Line '{line}'", lineIndex);
				#endregion

				String bindToFlag = _bindToFlag[0]; // Stores ID of flag to bind to

				#region ThrowEception
				if (!AssetExists(bindToFlag)) // Checks whether an asset with given ID is in memory
					throw new ZoneAssetNotFoundException(zone.FilePath, bindToFlag, lineIndex);
				if (!FlagExists(bindToFlag)) // Already know flag exists, so checks whether asset is flag
					throw new ZoneSyntaxException(zone.FilePath, $"Bind Target Isn't of type Flag", lineIndex);
				#endregion

				((Flag)GV.MapZone.globalAssets[bindToFlag].asset).FlagChanged += (flag, nVal) => {
					GV.MapZone.GetAsset(IDs[0], zone.assets).asset = nVal;
				}; // Append new event handler
			} else*/ if (true) { // General Value Set Type
				String value = line.Replace($"[{IDs[0]}] ", "").Trim(); // get value from line

				#region ThrowException
				if (value.Length == 0)
					throw new ZoneSyntaxException(zone.FilePath, $"Cannot extract 'Set' value from line {line}", lineIndex);
				#endregion

				Type type = GlobalVars.MapZone.GetAsset(IDs[0], zone.assets).assetType;

				if (type == typeof(Flag)) // Flag set only changed flag value
					((Flag)GV.MapZone.globalAssets[IDs[0]].asset).Value = BooleanParser(value);
				else SetAsset(IDs[0], Converters.StringToAssetValue(type, value));

				if (Zone.StoreAssetTrace) { zone.ZoneAssetTrace.AddSetAsset(IDs[0], value); }
			}
		}

		private static void GameEntity(String line) {
			String type = SpeechMarkParser(line), ID = GetAssetIDs(line)[0];

			if (!Regex.IsMatch(line, "\"[^\n\t\"]+\".as.\\[[^\n\t\\]]+\\]")) {
				throw new ZoneSyntaxException(zone.FilePath, "Syntax of entity command is incorrect", lineIndex);
			}

			//zone.monogameObjects.Add(ID, GV.EntityGenerators.StringToMonoGameObject(type, ID));

			zone.zoneEntities.Add(ID, GV.EntityGenerators.StringToGameEntity(type));

			foreach (Match m in Regex.Matches(line, @"<[^\n\r\t<]+")) {
				String[] split = m.Value.Trim('<', ' ').Split('=');

				if (split.Length < 2)
					throw new ZoneSyntaxException(zone.FilePath, "Syntax Of Attribute Not Correct", lineIndex);

				String attrName = split[0], value = (split.Length > 2) ? // If spaces in value string
					GV.CollectionManipulator.GetSubArray(split, 1, split.Length - 1).Aggregate((a, b) => $"{a} {b}") : split[1];

				GameEntityAttribute($"[{ID}]({attrName}) {value}"); // Interpret as attribute contained in string
			}
		}

		private static void GameEntityAttribute(String line) {
			String[] _IDs = GetAssetIDs(line); // Get All IDs in a given string

			if (_IDs.Length == 0) throw new ZoneSyntaxException(zone.FilePath, $"No Asset ID Given In Line '{line}'", lineIndex);
			
			bool storedInZone = zone.zoneEntities.ContainsKey(_IDs[0]), storedGlobally = GV.MapZone.GlobalEntities.ContainsKey(_IDs[0]);

			if (!(storedInZone || storedGlobally)) throw new ZoneAssetNotFoundException(zone.FilePath, _IDs[0], lineIndex);

			String attribute = BracketParser(line).Trim();

			if (attribute.Length == 0) throw new ZoneSyntaxException(zone.FilePath, $"Attribute Not Given In Line '{line}'", lineIndex);
			
			Type attributeType = (storedInZone ? zone.zoneEntities : GV.MapZone.GlobalEntities)[_IDs[0]][attribute].Type;
			Object value; // Value for attribute in a given entity/stored monogame object instance

			if (_IDs.Length == 2) { value = GV.MapZone.GetAsset(_IDs[1], zone.assets); } else if (_IDs.Length == 1) {
				value = Converters.StringToAssetValue(attributeType, line.Replace($"[{_IDs[0]}]", "").Replace($"({attribute})", "").Trim());
			} else { throw new ZoneSyntaxException(zone.FilePath, $"Too many Assets Found In Line '{line}'", lineIndex); }

			(storedInZone ? zone.zoneEntities : GV.MapZone.GlobalEntities)[_IDs[0]].AssignAttribute(attribute, value);
		}
	}
}
