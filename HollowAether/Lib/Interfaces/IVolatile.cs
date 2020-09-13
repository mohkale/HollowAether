using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	public interface IVolatile {
		void InitializeVolatility(VolatilityType vt, object arg);
		GAssets.Volatile.VolatilityManager VolatilityManager { get; }
	}
}
