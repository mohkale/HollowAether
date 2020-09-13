#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.Exceptions {
	/// <summary>Root exception for all custom HollowAether Exceptions</summary>
	public class HollowAetherException : Exception {
		public HollowAetherException() : base() { }

		public HollowAetherException(String msg) : base(msg) { }

		public HollowAetherException(String msg, Exception inner) : base(msg, inner) { }

		public HollowAetherException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
