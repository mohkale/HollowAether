#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.Exceptions.CE {
	class FatalException : HollowAetherException {
		public FatalException() : base() {

		}

		public FatalException(String msg) : base(msg) {

		}

		public FatalException(String msg, Exception inner) : base(msg, inner) {

		}

		public FatalException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}
