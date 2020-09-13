using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib.GAssets.Boundary {
	public struct DeltaPositions {
		public DeltaPositions(float deltaX, float deltaY, float deltaTop, float deltaBottom, float deltaLeft, float deltaRight) {
			_deltaX = deltaX;
			_deltaY = deltaY;
			_deltaLeft = deltaLeft;
			_deltaRight = deltaRight;
			_deltaTop = deltaTop;
			_deltaBottom = deltaBottom;
		}

		public override string ToString() {
			return $"DeltaPosition: (W: {_deltaX}, H: {_deltaY}) T:{_deltaTop} L:{_deltaLeft} R:{_deltaRight} B:{_deltaBottom}";
		}

		public static DeltaPositions Empty { get { return new DeltaPositions(0,0,0,0,0,0); } }

		private float _deltaX, _deltaY, _deltaTop, _deltaBottom, _deltaLeft, _deltaRight;

		/// <summary>Total change in width between boundary and spriteRect</summary>
		public float width { get { return _deltaX; } }

		/// <summary>Total change in height between boundary and spriterect</summary>
		public float height { get { return _deltaY; } }

		/// <summary>Change between top of boundary and top of spriterect</summary>
		public float top { get { return _deltaTop; } }

		/// <summary>Change between bottom of boundary and bottom of spriterect</summary>
		public float bottom { get { return _deltaBottom; } }

		/// <summary>Change between left of boundary and left of spriterect</summary>
		public float left { get { return _deltaLeft; } }

		/// <summary>Change between right of boundary and right of spriterect</summary>
		public float right { get { return _deltaRight; } }
	}
}
