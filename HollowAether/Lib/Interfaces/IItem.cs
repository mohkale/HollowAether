using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib {
	public interface IItem : IContextualised {
		String ItemID { get; }
	}
}
