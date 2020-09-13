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
using HollowAether.Lib.MapZone;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	public interface IUpdateable { } // Functionality in children

	public interface IAnimationUpdateable : IUpdateable {
		void Update(bool updateAnimation);
	}

	public interface IGeneralUpdateable : IUpdateable {
		void Update();
	}

	public interface IDrawable {
		void Draw();
	}

	/// <summary> Interface for any objects which follow the MonoGame class Model </summary>
	public interface IMonoGameObject : IDrawable, IAnimationUpdateable {
		void Initialize(String textureKey);
		void LateUpdate();
		/*void ImplementEntityAttribute(String attributeName, Object value);
		Type GetAttributeType(String attrName);*/
		void AttatchEntity(GameEntity entity);
		
		Vector2 Position { get; set; }
		String SpriteID { get; set; }
		Rectangle SpriteRect { get; }
		Animation Animation { get; }
	}
}
