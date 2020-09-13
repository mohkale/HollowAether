using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib {
	interface IDamaging {
		int GetDamage();
	}

	interface IDamagingToPlayer : IDamaging {

	}

	interface IDamagingToEnemies : IDamaging {

	}
}
