using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using HollowAether.Lib.GAssets.Boundary;
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib.GAssets {
	public struct LabelledBoundaryContainer : ILabelledBoundaryContainer {
		public LabelledBoundaryContainer(Rectangle spriteRect, params Tuple<String, IBoundary>[] boundaries) {
			labelBoundaryCollection = new Dictionary<String, IBoundary>();
			initialDimensions = _container = Rectangle.Empty;
			_deltaPositions = DeltaPositions.Empty;
			SpriteRect = spriteRect;

			// This has been assigned to now.

			_container = GetBoundaryArea();
			BuildDeltaPositions(spriteRect);
		}

		public void BuildDeltaPositions(Rectangle? spriteRect) {
			_deltaPositions = (spriteRect.HasValue) ? BoundaryShared.GetDeltaPositions(spriteRect.Value, Container)
				: _deltaPositions = DeltaPositions.Empty; // If has value build, otherwise create empty positions
		}

		public void Add(IBoundary boundary) {
			Add(String.Empty, boundary);
		}

		public void Add(String name, IBoundary boundary) {
			labelBoundaryCollection[name] = boundary;
			_container = GetBoundaryArea();
			BuildDeltaPositions(SpriteRect);
		}

		public void RemoveBoundary(String name) {
			labelBoundaryCollection.Remove(name);
			_container = GetBoundaryArea();
			BuildDeltaPositions(SpriteRect);
		}

		public void RemoveBoundary(IBoundary other) {
			RemoveBoundary(GlobalVars.CollectionManipulator.DictionaryGetKeyFromValue(labelBoundaryCollection, other));
		}

		public void Offset(float X=0f, float Y=0f) {
			foreach (String key in labelBoundaryCollection.Keys)
				labelBoundaryCollection[key].Offset(X, Y);

			_container.Offset(X, Y);
		}

		public void Offset(Vector2 offset) {
			Offset(offset.X, offset.Y);
		}

		public bool Intersects(IBoundaryContainer other) {
			if (!other.Container.Intersects(this._container))
				return false; // prevent CPU waste

			foreach (IBoundary tarBoundary in other.Boundaries) {
				if (Intersects(tarBoundary, true)) return true;
			}

			return false;
		}

		public bool Intersects(IBoundaryContainer other, Vector2? offset) {
			if (offset.HasValue) Offset(offset.Value);
			bool success = Intersects(other);
			if (offset.HasValue) Offset(-offset.Value);

			return success; // return successful collision after offset
		}

		public bool Intersects(IBoundary boundary, bool checkContainerCollision=true) {
			if (checkContainerCollision) {
				if (!boundary.Container.Intersects(this._container))
					return false; // prevent CPU waste
				// else Ignore returning and carry on 
			}

			foreach (IBoundary selfBoundary in Boundaries) {
				if (selfBoundary.Intersects(boundary))
					return true; // first collision return
			}

			return false;
		}

		public IBoundaryContainer GetCopy(Vector2? offset) {
			LabelledBoundaryContainer newContainer = this;

			if (offset.HasValue)
				newContainer.Offset(offset.Value);

			return newContainer;
		}

		public IBRectangle GetBoundaryArea() {
			return BoundaryShared.GetBoundaryArea(this);
		}

		public IntersectingPositions GetNonIntersectingPositions(IBoundaryContainer other) {
			IntersectingPositions positions = IntersectingPositions.Empty;

			foreach (IBoundary b in Boundaries) {
				foreach (IBoundary b2 in other.Boundaries) {
					if (!b.Intersects(b2)) continue; // No intersection, skip 4ward
					positions.Set(CollisionCalculators.CalculateCollision(b, b2));
				}
			}

			positions -= new IntersectingPositions() {
				{Direction.Top, Container.Height + DeltaPositions.height },
				{Direction.Bottom, DeltaPositions.height },
				{Direction.Left, Container.Width + DeltaPositions.right },
				{Direction.Right, DeltaPositions.left },
			};

			return positions;
		}

		public IntersectingPositions GetNonIntersectingPositions(IBoundaryContainer other, params String[] boundaries) {
			if (boundaries.Length == 0) return GetNonIntersectingPositions(other);
			IntersectingPositions positions = IntersectingPositions.Empty;


			foreach (String boundaryKey in boundaries) {
				if (!labelBoundaryCollection.ContainsKey(boundaryKey))
					throw new HollowAetherException($"Boundary Key {boundaryKey} not found");

				foreach (IBoundary boundary in other) {
					positions.Set(CollisionCalculators.CalculateCollision(boundary, this[boundaryKey]));
				}
			}

			positions -= new IntersectingPositions() {
				{Direction.Top, Container.Height + DeltaPositions.height },
				{Direction.Bottom, DeltaPositions.height },
				{Direction.Left, Container.Width + DeltaPositions.right },
				{Direction.Right, DeltaPositions.left },
			};

			return positions;
		}

		public String[] GetIntersectingBoundaryLabels(IBoundaryContainer other) {
			HashSet<String> labels = new HashSet<String>();

			foreach (String key in this.labelBoundaryCollection.Keys) {
				foreach (IBoundary boundary in other) {
					if (this[key].Intersects(boundary))
						labels.Add(key);

					if (labels.Count == Boundaries.Count())
						return labels.ToArray();
				}
			}

			return labels.ToArray();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<IBoundary> GetEnumerator() {
			return Boundaries.Cast<IBoundary>().GetEnumerator();
		}

		public IBoundary this[String label] {
			get { return labelBoundaryCollection[label]; }
			set { labelBoundaryCollection[label] = value; }
		}

		public IBRectangle Container { get { return _container; } }
		public IBoundary[] Boundaries { get { return labelBoundaryCollection.Values.ToArray(); } }
		public DeltaPositions DeltaPositions { get { return _deltaPositions; } }

		private IBRectangle _container;
		public IBRectangle initialDimensions; // for collision purpose;
		private DeltaPositions _deltaPositions; // Changes between delta
		Dictionary<String, IBoundary> labelBoundaryCollection;
		public Rectangle SpriteRect { get; private set; }
	}
}
