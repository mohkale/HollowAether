#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.MapZone;
#endregion

namespace HollowAether.Lib.Exceptions.CE {
	#region MapExceptions
	class MapException : HollowAetherException {
		public MapException(String fPath) { filePath = fPath; }
		public MapException(String fpath, String message) : base(message) { filePath = fpath; }
		public MapException(String fpath, String message, Exception inner) : base(message, inner) { filePath = fpath; }

		private String filePath;
	}

	class MapNotFoundException : MapException {
		public MapNotFoundException(String fpath) : base(fpath, $"Map At Given Path Not Found") { }
		public MapNotFoundException(String fpath, Exception inner) : base(fpath, $"Map At Given Path Not Found", inner) { }
	}

	class MapFileCouldntBeReadException : MapException {
		public MapFileCouldntBeReadException(String fpath) : base(fpath, $"Couldn't Read Given Map File") { }
		public MapFileCouldntBeReadException(String fpath, Exception inner) : base(fpath, $"Couldn't Read Given Map File", inner) { }
	}

	class MapHeaderException : MapException {
		public MapHeaderException(String fpath) : base(fpath, $"Incorrect Map Header In Map File") { }
		public MapHeaderException(String fpath, Exception inner) : base(fpath, $"Incorrect Map Header In Map File", inner) { }
	}

	class MapSyntaxException : MapException {
		public MapSyntaxException(String fpath, int lIndex) : base(fpath) { lineIndex = lIndex; }
		public MapSyntaxException(String fpath, String message, int lIndex) : base(fpath, message) { lineIndex = lIndex; }
		public MapSyntaxException(String fpath, String message, int lIndex, Exception inner) : base(fpath, message, inner) { lineIndex = lIndex; }

		public int lineIndex;
	}

	class MapLineConversionException : MapSyntaxException {
		public MapLineConversionException(String fPath, String line, String desiredType, int lineIndex)
			: base(fPath, $"Couldn't Convert Line '{line}' to Instance Of Type '{desiredType}'", lineIndex) { }
		public MapLineConversionException(String fPath, String line, String desiredType, int lineIndex, Exception inner)
			: base(fPath, $"Couldn't Convert Line '{line}' to Instance Of Type '{desiredType}'", lineIndex, inner) { }
	}

	class MapRepeatedFlagCreationException : MapSyntaxException {
		public MapRepeatedFlagCreationException(String filePath, String flagName, int lineIndex)
			: base(filePath, $"Cannot create flag '{flagName}' when it already exists", lineIndex) { }
		public MapRepeatedFlagCreationException(String filePath, String flagName, int lineIndex, Exception inner)
			: base(filePath, $"Cannot create flag '{flagName}' when it already exists", lineIndex, inner) { }
	}

	class MapFileDefinesZoneAtSamePositionTwiceException : MapSyntaxException {
		public MapFileDefinesZoneAtSamePositionTwiceException(String fpath, Vector2 index, int lineIndex)
			: base(fpath, $"Zone Has Been Defined At Position '{index}' Twice", lineIndex) { }
		public MapFileDefinesZoneAtSamePositionTwiceException(String fpath, Vector2 index, int lineIndex, Exception inner)
			: base(fpath, $"Zone Has Been Defined At Position '{index}' Twice", lineIndex, inner) { }
	}

	class MapUnknownVisibilityException : MapSyntaxException {
		public MapUnknownVisibilityException(String filePath, String type, int lineIndex)
			: base(filePath, $"Unknown Zone Identifier '{type}' Found in Map File", lineIndex) { }
		public MapUnknownVisibilityException(String filePath, String type, int lineIndex, Exception inner)
			: base(filePath, $"Unknown Zone Identifier '{type}' Found in Map File", lineIndex, inner) { }
	}
	#endregion

	#region ZoneExceptions
	class ZoneException : HollowAetherException {
		public ZoneException() : base() { }

		public ZoneException(String msg) : base(msg) { }

		public ZoneException(String msg, Exception inner) : base(msg, inner) { }

		public ZoneException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	class ZoneReadingException : ZoneException {
		public ZoneReadingException(String fpath) : base() { filePath = fpath; }
		public ZoneReadingException(String fpath, String message) : base(message) { filePath = fpath; }
		public ZoneReadingException(String fpath, String message, Exception inner) : base(message, inner) { filePath = fpath; }

		public String filePath;
	}

	class ZoneNotFoundException : ZoneReadingException {
		public ZoneNotFoundException(String fpath) : base(fpath, $"Zone At \"{fpath}\" Not Found") { }
		public ZoneNotFoundException(String fpath, Exception inner) : base(fpath, $"Zone At \"{fpath}\" Not Found", inner) { }
	}

	class ZoneFileCouldntBeReadException : ZoneReadingException {
		public ZoneFileCouldntBeReadException(String fpath) : base(fpath, $"Couldn't Read Given Zone File") { }
		public ZoneFileCouldntBeReadException(String fpath, Exception inner) : base(fpath, $"Couldn't Read Given Zone File", inner) { }
	}

	class ZoneHeaderException : ZoneReadingException {
		public ZoneHeaderException(String fpath) : base(fpath, $"Incorrect Map Header In Map File") { }
		public ZoneHeaderException(String fpath, Exception inner) : base(fpath, $"Incorrect Map Header In Map File") { }
	}

	class ZoneSyntaxException : ZoneReadingException {
		public ZoneSyntaxException(String fpath, int lIndex) : base(fpath) { lineIndex = lIndex; }
		public ZoneSyntaxException(String fpath, String message, int lIndex) : base(fpath, message) { lineIndex = lIndex; }
		public ZoneSyntaxException(String fpath, String message, int lIndex, Exception inner) : base(fpath, message, inner) { lineIndex = lIndex; }

		public int lineIndex;
	}


	class ZoneCouldNotFindAssetToImportException : ZoneSyntaxException {
		public ZoneCouldNotFindAssetToImportException(String filePath, Type assetType, String assetID, int lineIndex)
			: base(filePath, $"", lineIndex) { }
		public ZoneCouldNotFindAssetToImportException(String filePath, Type assetType, String assetID, int lineIndex, Exception inner)
			: base(filePath, $"", lineIndex, inner) { }
	}

	class ZoneAssetNotFoundException : ZoneSyntaxException {
		public ZoneAssetNotFoundException(String filePath, String assetID, int lineIndex)
			: base(filePath, $"", lineIndex) { }
		public ZoneAssetNotFoundException(String filePath, String assetID, int lineIndex, Exception inner)
			: base(filePath, $"", lineIndex, inner) { }
	}
	#endregion

	#region Misc
	class AssetAssignmentException : HollowAetherException {
		public AssetAssignmentException(Type desired, Type given)
			: base($"Cannot assign Asset of Type '{desired}' A Value of Type {given}") { }
		public AssetAssignmentException(Type desired, Type given, Exception inner)
			: base($"Cannot assign Asset of Type '{desired}' A Value of Type {given}", inner) { }
	}

	class ZoneWithGivenMapIndexNotFoundException : HollowAetherException {
		public ZoneWithGivenMapIndexNotFoundException(Vector2 missingIndex)
			: base($"Zone with key '{missingIndex}' Not Found") { index = missingIndex; }
		public ZoneWithGivenMapIndexNotFoundException(Vector2 missingIndex, Exception inner)
			: base($"Zone with key '{missingIndex}' Not Found", inner) { index = missingIndex; }

		public Vector2 index;
	}
	#endregion
}
