using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether {
	/// <summary>Holder class that can contain a bool</summary>
	public class Flag : Object {
		public Flag(bool initialValue = false) {
			_value = defaultValue = initialValue;
			FlagChanged = (s, e) => { s._value = e; };
		}
		
		public override string ToString() {
			return $"Flag '{_value}'";
		}

		public void ChangeFlag(bool updatedValue) {
			Value = updatedValue;
		}

		public bool ValueChangedFromDefault() {
			return defaultValue != Value;
		}

		private void _ChangeFlag(bool updatedValue) {
			if (_value != updatedValue) {
				FlagChanged(this, updatedValue);
			}
		}

		public static implicit operator Boolean(Flag self) {
			return self._value;
		}

		/// <summary>Event called when flag value has been changed/updated etc.</summary>
		public event FlagChanged_EventHandler FlagChanged;

		public delegate void FlagChanged_EventHandler(Flag self, bool flagValue);

		/// <summary>Actual value represented by flag instance</summary>
		private bool _value;

		public bool Value { get { return _value; } set { _ChangeFlag(value); } }

		public readonly bool defaultValue; // Default Value Assigned At Flag Start
	}
}
