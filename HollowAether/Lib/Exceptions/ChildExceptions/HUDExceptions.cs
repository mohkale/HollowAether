using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib.Exceptions.CE {
	class HUDHealthRangeException : HollowAetherException {
		public HUDHealthRangeException(int HUDSpan, int newHUDHealth, String message) : base(message) {
			hudSpan = HUDSpan; hudHealth = newHUDHealth;
		}

		public HUDHealthRangeException(int HUDSpan, int newHUDHealth, String message, Exception inner) : base(message, inner) {
			hudSpan = HUDSpan; hudHealth = newHUDHealth;
		}

		int hudSpan, hudHealth;
	}

	class HUDHealthAdditionException : HUDHealthRangeException {
		public HUDHealthAdditionException(int HUDSpan, int newHUDHealth)
			: base(HUDSpan, newHUDHealth, $"Cannot increase Hud health to {newHUDHealth}. Max span is {HUDSpan}") { }

		public HUDHealthAdditionException(int HUDSpan, int newHUDHealth, Exception inner)
			: base(HUDSpan, newHUDHealth, $"Cannot increase Hud health to {newHUDHealth}. Max span is {HUDSpan}", inner) { }
	}

	class HUDHealthSubtractionException : HUDHealthRangeException {
		public HUDHealthSubtractionException(int HUDSpan, int newHUDHealth)
			: base(HUDSpan, newHUDHealth, $"Cannot health Hud health to {newHUDHealth}. Min span is 0") { }

		public HUDHealthSubtractionException(int HUDSpan, int newHUDHealth, Exception inner)
			: base(HUDSpan, newHUDHealth, $"Cannot increase Hud health to {newHUDHealth}. Min span is 0", inner) { }
	}
}
