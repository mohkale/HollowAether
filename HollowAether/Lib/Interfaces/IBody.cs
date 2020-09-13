#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace HollowAether.Lib {
	/// <summary>Interface for any class which can experience gravity</summary>
	public interface IBody { void ImplementGravity(); }
}
