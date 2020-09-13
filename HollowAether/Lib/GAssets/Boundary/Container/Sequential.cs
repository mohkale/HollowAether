using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using HollowAether.Lib.GAssets.Boundary;
using System.Collections;

namespace HollowAether.Lib.GAssets {
	struct SequentialBoundaryContainer : IBoundaryContainer {
		public SequentialBoundaryContainer(Rectangle spriteRect, params IBoundary[] boundaries) {
			this.boundaries = new List<IBoundary>(boundaries);

			#region StructDefinitionDefaultValues
			_container = initialDimensions = Rectangle.Empty;
			_deltaPositions = DeltaPositions.Empty;
			SpriteRect = spriteRect;
			#endregion

			_container = GetBoundaryArea();
			BuildDeltaPositions(spriteRect);
		}

		public void BuildDeltaPositions(Rectangle? spriteRect) {
			if (boundaries.Count >= 1)
				_deltaPositions = (spriteRect.HasValue) ? BoundaryShared.GetDeltaPositions(spriteRect.Value, Container) 
					: _deltaPositions = DeltaPositions.Empty; // If has value build, otherwise create empty positions
		}

		public void Add(IBoundary boundary) {
			this.boundaries.Add(boundary);
			_container = GetBoundaryArea();
		}

		public void RemoveBoundary(IBoundary other) {
			try { this.boundaries.Remove(other); } catch { return; }
			_container = GetBoundaryArea();
		}

		public void Offset(float X=0, float Y=0) {
			foreach (IBoundary b in boundaries)
				b.Offset(X, Y);

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
			IBoundaryContainer returnValue = this;

			if (offset.HasValue)
				returnValue.Offset(offset.Value);

			return returnValue;
		}

		public IBRectangle GetBoundaryArea() {
			return BoundaryShared.GetBoundaryArea(this);
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<IBoundary> GetEnumerator() {
			return Boundaries.Cast<IBoundary>().GetEnumerator();
		}

		public IntersectingPositions GetNonIntersectingPositions(IBoundaryContainer other) {
			IntersectingPositions positions = IntersectingPositions.Empty;

			foreach (IBoundary b in Boundaries) {
				foreach (IBoundary b2 in other.Boundaries) {
					if (!b.Intersects(b2)) continue; // No collision, so skip 4ward
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

		private List<IBoundary> boundaries { get; set; }
		public IBRectangle Container { get { return _container; } }
		public DeltaPositions DeltaPositions { get { return _deltaPositions; } }

		public IBoundary[] Boundaries { get { return boundaries.ToArray(); } }

		public IBRectangle initialDimensions; // for collision purpose;
		private DeltaPositions _deltaPositions; // Changes between delta
		private IBRectangle _container; // bounding box for whole container
		public Rectangle SpriteRect { get; private set; }
	}
}
