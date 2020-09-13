using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.Exceptions;
using Microsoft.Xna.Framework;

namespace HollowAether.Lib.GAssets.Boundary {
	static class BoundaryShared {
		/// <summary>Gets Rectangle encompassing all boundaries within instance</summary>
		/// <param name="container">Instance of collision boundary class</param>
		/// <returns>Rectangle containing area of all boundaries within a given container</returns>
		public static IBRectangle GetBoundaryArea(IBoundaryContainer container) {
			if (container.Boundaries.Length == 0) return container.SpriteRect; // throw new HollowAetherException("No Boundary");

			IBRectangle compound = container.Boundaries[0].Container; // Begin from first

			foreach (IBoundary boundary in container.Boundaries) {
				compound = IBRectangle.Union(compound, boundary.Container);
			}

			return compound;
		}

		public static DeltaPositions GetDeltaPositions(Rectangle spriteRect, IBRectangle containerRect) {
			return new DeltaPositions(
				deltaX: spriteRect.Width - containerRect.Width,
				deltaY: spriteRect.Height - containerRect.Height,
				deltaTop: containerRect.Top - spriteRect.Top,
				deltaBottom: spriteRect.Bottom - containerRect.Bottom,
				deltaLeft: containerRect.Left - spriteRect.Left,
				deltaRight: spriteRect.Right - containerRect.Right
			);
		}
	}
}
