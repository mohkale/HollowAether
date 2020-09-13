#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.Exceptions.CE {
	class AnimationException : HollowAetherException {
		public AnimationException() : base() { }

		public AnimationException(String msg) : base(msg) { }

		public AnimationException(String msg, Exception inner) : base(msg, inner) { }

		public AnimationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	class AnimationFileNotFoundException : AnimationException {
		public AnimationFileNotFoundException(String fpath) 
			: base($"Animation File '{fpath}' Not Found") {}

		public AnimationFileNotFoundException(String fpath, Exception inner) 
			: base($"Animation File '{fpath}' Not Found", inner) {}
	}

	class AnimationIncorrectHeaderException : AnimationException {
		public AnimationIncorrectHeaderException(String fpath)
			: base($"Animation File '{fpath}' Lacks The Correct Header") { }
		public AnimationIncorrectHeaderException(String fpath, Exception inner)
			: base($"Animation File '{fpath}' Lacks The Correct Header", inner) { }
	}

	class AnimationFrameDefinitionFormatException : AnimationException {
		public AnimationFrameDefinitionFormatException(String line)
			: base($"Couldn't Extract Frame From Line '{line}'") { }
		public AnimationFrameDefinitionFormatException(String line, Exception inner)
			: base($"Couldn't Extract Frame From Line '{line}'", inner) { }
	}
}
