using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using HollowAether.Lib.GAssets.Boundary;
using IP = HollowAether.Lib.GAssets.IntersectingPositions;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	/// <summary>Interface for a class which can hold a number of boundaries</summary>
	public interface IBoundaryContainer : IEnumerable<IBoundary>, IEnumerable {
		/// <summary>Adds boundary to boundary container</summary>
		/// <param name="boundary">Boundary to add</param>
		void Add(IBoundary boundary);

		/// <summary>Removes boundary from boundary container</summary>
		/// <param name="boundary">Boundary to remove</param>
		void RemoveBoundary(IBoundary boundary);

		/// <summary>Pushes all stored boundaries by a given amount</summary>
		/// <param name="X">Horizontal displacement</param> <param name="Y">Vertical displacement</param>
		void Offset(float X, float Y);

		/// <summary>Pushes all stored boundaries by a given amount</summary>
		/// <param name="offset">Value by which to offset boundaries</param>
		void Offset(Vector2 offset);

		/// <summary>Whether any boundary in two containers intersect</summary>
		/// <param name="other">Other boundary to check for collision with</param>
		bool Intersects(IBoundaryContainer other);

		/// <summary>Whether any boundary in two containers intersect with an offset</summary>
		/// <param name="other">Other boundary to check for collision with</param>
		/// <param name="offset">Value by which to offset</param>
		bool Intersects(IBoundaryContainer other, Vector2? offset);

		/// <summary>Creates a shallow clone of boundary</summary>
		/// <param name="offset">Allows to push boundary afterwards as well</param>
		IBoundaryContainer GetCopy(Vector2? offset);

		/// <summary>Get rectangle encompassing all boundaries within container</summary>
		IBRectangle GetBoundaryArea();

		/// <summary>Get all intersecting positions between two boundaries</summary>
		/// <param name="other">Other boundary</param>
		IP GetNonIntersectingPositions(IBoundaryContainer other);

		/// <summary>GetBoundaryArea()</summary>
		IBRectangle Container { get; }

		/// <summary>Duplicate of value stored in animation</summary>
		Rectangle SpriteRect { get; }

		/// <summary>All boundaries in container</summary>
		IBoundary[] Boundaries { get; }

		/// <summary>Difference between spriterect and boundary</summary>
		DeltaPositions DeltaPositions { get; }
	}

	/// <summary>Boundary container resembling dictionary like object</summary>
	public interface ILabelledBoundaryContainer : IBoundaryContainer {
		/// <summary>Gets any intersected boundary labels</summary>
		/// <param name="other">Boundary to check intersection with</param>
		String[] GetIntersectingBoundaryLabels(IBoundaryContainer other);

		/// <summary>Adds boundary</summary>
		/// <param name="name">ID of boundary</param>
		/// <param name="boundary">Value of boundary</param>
		void Add(String name, IBoundary boundary);

		/// <summary>Removes boundary from container</summary>
		/// <param name="name">ID of boundary</param>
		void RemoveBoundary(String name);
 	}
}
