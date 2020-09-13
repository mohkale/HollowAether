using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace HollowAether.Lib.GAssets {
	public enum Direction { Top, Bottom, Left, Right }

	public class IntersectingPositions : Dictionary<Direction, float?> {
		public IntersectingPositions() : base() { }
		public IntersectingPositions(int capacity) : base(capacity) { }
		public IntersectingPositions(IDictionary<Direction, float?> dict) : base(dict) { }
		public IntersectingPositions(IEqualityComparer<Direction> comparer) : base(comparer) { }
		public IntersectingPositions(SerializationInfo info, StreamingContext context) : base(info, context) { }
		public IntersectingPositions(int capacity, IEqualityComparer<Direction> comparer) : base(capacity, comparer) { }
		public IntersectingPositions(IDictionary<Direction, float?> dict, IEqualityComparer<Direction> comparer) : base(dict, comparer) { }

		private delegate float SetMethod(float X, float Y);

		public override string ToString() {
			Func<KeyValuePair<Direction, float?>, String> KVPToString = (KVP) => {
				String val = (KVP.Value.HasValue) ? KVP.Value.ToString() : "Null";
				return $"({KVP.Key} : {val})";
			};

			return $"IP{{{(from X in this select KVPToString(X)).Aggregate((a, b) => $"{a}, {b}")}}}";
		}

		public void Set(Direction key, float? newVal) {
			if (!newVal.HasValue) return; // No value to add
			
			SetMethod Call = (Equals(key, Direction.Top) || Equals(key, Direction.Left)) 
				? new SetMethod(Math.Min) : new SetMethod(Math.Max);

			if (!this[key].HasValue) { this[key] = newVal.Value; } else {
				this[key] = Call(this[key].Value, newVal.Value);
			}
		}

		public void Set(IntersectingPositions other) {
			foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
				Set(direction, other[direction]);
			}
		}

		public static IntersectingPositions operator -(IntersectingPositions A, IntersectingPositions B) {
			IntersectingPositions N = A.MemberwiseClone() as IntersectingPositions;

			foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
				if (A[direction].HasValue && B[direction].HasValue)
					N[direction] -= B[direction].Value;
			}

			return N;
		}

		public static IntersectingPositions operator +(IntersectingPositions A, IntersectingPositions B) {
			IntersectingPositions N = A.MemberwiseClone() as IntersectingPositions;

			foreach (Direction direction in Enum.GetValues(typeof(Direction))) {
				if (A[direction].HasValue && B[direction].HasValue)
					N[direction] += B[direction].Value;
			}

			return N;
		}

		public static IntersectingPositions Empty {
			get {
				return new IntersectingPositions { { Direction.Top, null }, { Direction.Bottom, null }, { Direction.Left, null }, { Direction.Right, null } };
			}
		}
	}
}
