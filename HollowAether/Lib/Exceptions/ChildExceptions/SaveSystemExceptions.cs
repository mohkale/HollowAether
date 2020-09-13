#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.Exceptions.CE {
	class SaveSystemException : HollowAetherException {
		public SaveSystemException() : base() { }
		public SaveSystemException(String msg) : base(msg) { }
		public SaveSystemException(String msg, Exception inner) : base(msg, inner) { }
		public SaveSystemException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	class SaveFileInvalidHeaderException : SaveSystemException {
		public SaveFileInvalidHeaderException(String fPath) : base($"Save File '{fPath}' Lacks Correct Header") { }
		public SaveFileInvalidHeaderException(String fPath, Exception inner) : base($"Save File '{fPath}' Lacks Correct Header", inner) { }
	}

	class SaveFileDoesntExistAtChosenSlotException : SaveSystemException {
		public SaveFileDoesntExistAtChosenSlotException(int index, String path) : base($"Save file {index} at path '{path}' doesn't exist") { }
		public SaveFileDoesntExistAtChosenSlotException(int index, String path, Exception inner) : base($"Save file {index} at path '{path}' doesn't exist", inner) { }
	}
	
	class SaveSlotOutOfRangeException : SaveSystemException {
		public SaveSlotOutOfRangeException(int chosen, int max) : base($"The maximum index value for Saves is {max}, {chosen + 1} is invalid") { }
		public SaveSlotOutOfRangeException(int chosen, int max, Exception inner) : base($"The maximum index value for Saves is {max}, {chosen + 1} is invalid", inner) { }
	}

	class SaveFileCorruptedException : SaveSystemException {
		public SaveFileCorruptedException(String headerMissing) : base($"Save file lacks '{headerMissing}' section") { }
		public SaveFileCorruptedException(String headerMissing, Exception inner) : base($"Save file lacks '{headerMissing}' section", inner) { }
	}
}
