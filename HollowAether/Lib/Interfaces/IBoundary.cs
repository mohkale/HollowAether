#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

#region HollowAetherImports
using HollowAether.Lib.GAssets;
#endregion

namespace HollowAether.Lib {
	/// <summary> Interface for any class which can act as an indicator of collision </summary>
	public interface IBoundary {
		/// <summary>Whether one boundary collides with another</summary>
		/// <param name="boundary">Another boundary to collide with</param>
		bool Intersects(IBoundary boundary);

		/// <summary>Push boundary a given distance in a known direction</summary>
		/// <param name="vect">Amount by which to push sprite boundary</param>
		void Offset(Vector2 vect);

		/// <summary>Push boundary a given distance in a known direction</summary>
		/// <param name="X">Horizontal displacement</param> <param name="Y">Vertical displacement</param>
		void Offset(float X=0, float Y=0);

		/// <summary>Rectangle which can fully encompass boundary</summary>
		IBRectangle Container { get; }
	}
}
