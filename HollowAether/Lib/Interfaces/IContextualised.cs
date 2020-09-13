using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib {
	// Outputs things to context window
	public interface IContextualised {
		bool OutputReady();
		String OutputString { get; }
	}
}
