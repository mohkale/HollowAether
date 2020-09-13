#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.Exceptions.CE {
	class CollisionException : HollowAetherException {
		public CollisionException() : base() {

		}

		public CollisionException(String msg) : base(msg) {

		}

		public CollisionException(String msg, Exception inner) : base(msg, inner) {

		}

		public CollisionException(SerializationInfo info, StreamingContext context) : base(info, context) {

		}
	}
}
