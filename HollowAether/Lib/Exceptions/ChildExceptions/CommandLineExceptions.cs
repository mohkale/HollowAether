using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HollowAether.Lib.Exceptions.CE {
	/// <summary>Root Exception For All Command Line Exceptions</summary>
	class CommandLineException : HollowAetherException {
		public CommandLineException(int _minArgs, int _maxArgs, String _cmdAssist, String command) : base() {
			minArgs = _minArgs; maxArgs = _maxArgs; commandAssistance = _cmdAssist; cmd = command;
		}
		public CommandLineException(String msg, int _minArgs, int _maxArgs, String _cmdAssist, String command) : base(msg) {
			minArgs = _minArgs; maxArgs = _maxArgs; commandAssistance = _cmdAssist; cmd = command;
		}
		public CommandLineException(String msg, Exception inner, int _minArgs, int _maxArgs, String _cmdAssist, String command) : base(msg, inner) {
			minArgs = _minArgs; maxArgs = _maxArgs; commandAssistance = _cmdAssist; cmd = command;
		}
		public CommandLineException(SerializationInfo info, StreamingContext context, int _minArgs, int _maxArgs, String _cmdAssist, String command)
			: base(info, context) { minArgs = _minArgs; maxArgs = _maxArgs; commandAssistance = _cmdAssist; cmd = command; }

		public int maxArgs, minArgs;
		public String commandAssistance, cmd; // = ' "Arg1" ?"OptionalArg2" '
	}

	#region RootExceptions
	/// <summary>Encrypt File Command Line Exception</summary>
	class EncryptFileException : CommandLineException {
		public EncryptFileException()
			: base(1, 2, "\"Path\\To\\File\\To\\Encrypt\" ?\"Path\\To\\Target\\File\"", "encryptFile") { }
		public EncryptFileException(String msg)
			: base(msg, 1, 2, "\"Path\\To\\File\\To\\Encrypt\" ?\"Path\\To\\Target\\File\"", "encryptFile") { }
		public EncryptFileException(String msg, Exception inner)
			: base(msg, inner, 1, 2, "\"Path\\To\\File\\To\\Encrypt\" ?\"Path\\To\\Target\\File\"", "encryptFile") { }
	}

	/// <summary>Decrypt File Command Line Exception</summary>
	class DecryptFileException : CommandLineException {
		public DecryptFileException()
			: base(1, 2, "\"Path\\To\\File\\To\\Decrypt\" ?\"Path\\To\\Target\\File\"", "decryptFile") { }
		public DecryptFileException(String msg)
			: base(msg, 1, 2, "\"Path\\To\\File\\To\\Decrypt\" ?\"Path\\To\\Target\\File\"", "decryptFile") { }
		public DecryptFileException(String msg, Exception inner)
			: base(msg, inner, 1, 2, "\"Path\\To\\File\\To\\Decrypt\" ?\"Path\\To\\Target\\File\"", "decryptFile") { }
	}

	/// <summary>Encrypt Directory Command Line Exception</summary>
	class EncryptDirectoryException : CommandLineException {
		public EncryptDirectoryException()
			: base(1, 2, "\"Path\\To\\Directory\\To\\Encrypt\" ?\"Path\\To\\Target\\Directory\"", "encryptDirectory") { }
		public EncryptDirectoryException(String msg)
			: base(msg, 1, 2, "\"Path\\To\\Directory\\To\\Encrypt\" ?\"Path\\To\\Target\\Directory\"", "encryptDirectory") { }
		public EncryptDirectoryException(String msg, Exception inner)
			: base(msg, inner, 1, 2, "\"Path\\To\\Directory\\To\\Encrypt\" ?\"Path\\To\\Target\\Directory\"", "encryptDirectory") { }
	}

	/// <summary>Decrypt Directory Command Line Exception</summary>
	class DecryptDirectoryException : CommandLineException {
		public DecryptDirectoryException()
			: base(1, 2, "\"Path\\To\\Directory\\To\\Decrypt\" ?\"Path\\To\\Target\\Directory\"", "decryptDirectory") { }
		public DecryptDirectoryException(String msg)
			: base(msg, 1, 2, "\"Path\\To\\Directory\\To\\Decrypt\" ?\"Path\\To\\Target\\Directory\"", "decryptDirectory") { }
		public DecryptDirectoryException(String msg, Exception inner)
			: base(msg, inner, 1, 2, "\"Path\\To\\Directory\\To\\Decrypt\" ?\"Path\\To\\Target\\Directory\"", "decryptDirectory") { }
	}

	/// <summary>Set target map Command Line Exception</summary>
	class SetMapException : CommandLineException {
		public SetMapException() : base(1, 1, "\"Path\\To\\Map-File\"", "map") { }
		public SetMapException(String msg) : base(msg, 1, 1, "\"Path\\To\\Map-File\"", "map") { }
		public SetMapException(String msg, Exception inner) : base(msg, inner, 1, 1, "\"Path\\To\\Map-File\"", "map") { }
	}

	/// <summary>Set game zoom Command Line Exception</summary>
	class SetGameZoomException : CommandLineException {
		public SetGameZoomException() : base(1, 1, "new-game-zoom", "zoom") { }
		public SetGameZoomException(String msg) : base(msg, 1, 1, "new-game-zoom", "zoom") { }
		public SetGameZoomException(String msg, Exception inner) : base(msg, inner, 1, 1, "new-game-zoom", "zoom") { }
	}

	/* // Kind of Impossible to throw, but in case later needed I'll leave it here
	
	class SetFullScreenException : CommandLineException {
		public SetFullScreenException() : base(0, 0, null) { }
		public SetFullScreenException(String msg) : base(msg, 1, 1, null) { }
		public SetFullScreenException(String msg, Exception inner) : base(msg, inner, 1, 1, null) { }
	}

	class SetWindowedScreenException : CommandLineException {
		public SetWindowedScreenException() : base(0, 0, null) { }
		public SetWindowedScreenException(String msg) : base(msg, 1, 1, null) { }
		public SetWindowedScreenException(String msg, Exception inner) : base(msg, inner, 1, 1, null) { }
	}

	class SetFPSException : CommandLineException {
		public SetFPSException() : base(0, 0, null) { }
		public SetFPSException(String msg) : base(msg, 1, 1, null) { }
		public SetFPSException(String msg, Exception inner) : base(msg, inner, 1, 1, null) { }
	}*/
	#endregion


	#region ArgumentExceptions
	/// <summary>Thrown when an argument for encrypt file doesn't exist</summary>
	class EncryptFileArgNotFoundException : EncryptFileException {
		public EncryptFileArgNotFoundException(String fpath) : base($"Could Not Find File '{fpath}'") { }
		public EncryptFileArgNotFoundException(String fpath, Exception inner) : base($"Could Not Find File '{fpath}'", inner) { }
	}

	/// <summary>Thrown when an argument for decrypt file doesn't exist</summary>
	class DecryptFileArgNotFoundException : EncryptFileException {
		public DecryptFileArgNotFoundException(String fpath) : base($"Could Not Find File '{fpath}'") { }
		public DecryptFileArgNotFoundException(String fpath, Exception inner) : base($"Could Not Find File '{fpath}'", inner) { }
	}

	/// <summary>Thrown when an argument for encrypt directory doesn't exist</summary>
	class EncryptDirectoryArgNotFoundException : EncryptDirectoryException {
		public EncryptDirectoryArgNotFoundException(String dpath) : base($"Could Not Found The Directory '{dpath}'") { }
		public EncryptDirectoryArgNotFoundException(String dpath, Exception inner) : base($"Could Not Found The Directory '{dpath}'", inner) { }
	}

	/// <summary>Thrown when an argument for decrypt directory doesn't exist</summary>
	class DecryptDirectoryArgNotFoundException : EncryptDirectoryException {
		public DecryptDirectoryArgNotFoundException(String dpath) : base($"Could Not Found The Directory '{dpath}'") { }
		public DecryptDirectoryArgNotFoundException(String dpath, Exception inner) : base($"Could Not Found The Directory '{dpath}'", inner) { }
	}

	/// <summary>Thrown when set zoom argument isn't valid</summary>
	class SetGameZoomArgumentIncorrectTypeException : SetGameZoomException {
		public SetGameZoomArgumentIncorrectTypeException(String argument)
			: base($"Argument '{argument}' Could Not Be Converted To A Float") { }
		public SetGameZoomArgumentIncorrectTypeException(String argument, Exception inner)
			: base($"Argument '{argument}' Could Not Be Converted To A Float", inner) { }
	}
	#endregion

	#region ArgumentValidationExceptions
	//public class EncryptFileTargetNotFoundException : 
	#endregion
}
