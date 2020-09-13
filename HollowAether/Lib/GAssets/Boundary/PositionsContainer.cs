#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

namespace HollowAether.Lib.GAssets {
	#region PositionsContainerTypeDef
	public class PositionsContainer : Dictionary<String, List<Dictionary<String, int>>> {
		public PositionsContainer() : base() { }

		public PositionsContainer(int capacity) : base(capacity) { }

		public PositionsContainer(IEqualityComparer<String> comparer) : base(comparer) { }

		public PositionsContainer(Dictionary<String, List<Dictionary<String, int>>> dict) : base(dict) { }

		public PositionsContainer(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public PositionsContainer(int capacity, IEqualityComparer<String> comparer) : base(capacity, comparer) { }

		public PositionsContainer(Dictionary<String, List<Dictionary<String, int>>> dict, IEqualityComparer<String> comparer) : base(dict, comparer) { }
	}
	#endregion
}
