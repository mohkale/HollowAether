#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

#region HollowAetherImports
using HollowAether.Lib.GAssets;
#endregion

namespace HollowAether.Lib {
	/// <summary> Interface for any class which can be collided with </summary>
	public interface ICollideable {
		/// <summary>Checks whether object collides with another object</summary>
		/// <param name="target">Target monogame object which can be collided with</param>
		/// <returns>Boolean indicating sucesfull collision between targets</returns>
		bool Intersects(IMonoGameObject target);
		
		bool Intersects(IBoundaryContainer boundary);
		
		IMonoGameObject[] CompoundIntersects(params Type[] matchableTypes);

		//void BuildBoundary();

		//BoundaryContainer GetBoundary();
		IBoundaryContainer Boundary { get; }
	}
}
