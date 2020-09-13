#define DRAW_BOUNDARY

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
using HollowAether.Lib.MapZone;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether.Lib.GAssets {
	/// <summary>Sprite which any other collideable sprite can collide with</summary>
	public abstract partial class CollideableSprite : Sprite, ICollideable {
		public CollideableSprite() : base() { }

		public CollideableSprite(Vector2 position, int width, int height, bool aRunning = true) 
			: base(position, width, height, aRunning) { }

		/// <summary>Also builds boundary</summary>
		/// <param name="textureKey">Texture of given sprite</param>
		public override void Initialize(string textureKey) {
			base.Initialize(textureKey);
			BuildBoundary();
		}

		public override void Draw() {
			base.Draw();

			#if DRAW_BOUNDARY && DEBUG
			Color boundaryColor; // What to colour boundary

			if (this is Player)				boundaryColor = Color.Yellow;
			else if (this is IDamaging)		boundaryColor = Color.Red;
			else if (this is Enemy)			boundaryColor = Color.Red;
			else if (this is IInteractable) boundaryColor = Color.Green;
			else							boundaryColor = Color.White;
			
			GV.DrawMethods.DrawBoundary(boundary, boundaryColor);
			#endif
		}

		/// <summary>Checks for collision between two IMonoGameObjects</summary>
		/// <param name="A">Fist MonoGameObject</param><param name="B">Second MonoGameObject</param>
		/// <returns>Boolean indicating succesful collision between the two given monogame objects</returns>
		protected static bool Intersects(IMonoGameObject A, IMonoGameObject B) {
			if (A is ICollideable && B is ICollideable) return Intersects((ICollideable)A, (ICollideable)B); else return false;
		}

		protected static bool Intersects(ICollideable A, ICollideable B) {
			return Intersects(A.Boundary, B.Boundary);
		}

		/// <summary>Checks for collision between two boundaries</summary>
		/// <param name="A">First boundary</param><param name="B">Second boundary</param>
		/// <returns>Boolean indicating succesful collision between the two given boundaries</returns>
		public static bool Intersects(IBoundaryContainer A, IBoundaryContainer B) {
			return A.Intersects(B);
		}

		/// <summary>Checks for collision with another IMonoGameObject</summary>
		/// <param name="target">Target monogame object to check for collision with</param>
		/// <returns>Boolean indicating succesful collision with this</returns>
		public bool Intersects(IMonoGameObject target) {
			return Intersects(this, target);
		}

		/// <summary>Check for collision between this and another boundary</summary>
		/// <param name="target">Given target boundary to check for collision with</param>
		/// <returns>Boolean indicating succesful collision with this</returns>
		public bool Intersects(IBoundaryContainer target) {
			return Intersects(this.boundary, target);
		}

		protected static IMonoGameObject[] CompoundIntersects<T>(ICollideable self, IMonoGameObject[] objects) {
			return (from X in objects where X != self && X is ICollideable && X is T && Intersects(self, (ICollideable)X) select X).ToArray();
		}

		protected static IMonoGameObject[] CompoundIntersects<T>(ICollideable self) {
			return CompoundIntersects<T>(self, GV.MonoGameImplement.monogameObjects.ToArray());
		}

		protected static IEnumerable<IMonoGameObject> YieldCompoundIntersects(ICollideable self, IMonoGameObject[] objects, params Type[] types) {
			return (from X in objects where X != self && X is ICollideable && (types.Length == 0 || GV.Misc.TypesContainsGivenType(X.GetType(), types)) && Intersects(self, (ICollideable)X) select X);
		}

		protected static IMonoGameObject[] CompoundIntersects(ICollideable self, IMonoGameObject[] objects, params Type[] types) {
			return YieldCompoundIntersects(self, objects, types).ToArray();
		}

		protected static IMonoGameObject[] CompoundIntersects(ICollideable self, params Type[] types) {
			return YieldCompoundIntersects(self, GV.MonoGameImplement.monogameObjects.ToArray(), types).ToArray();
		}

		public IMonoGameObject[] CompoundIntersects(params Type[] types) {
			return CompoundIntersects(this, types);
		}

		public IMonoGameObject[] CompoundIntersects<T>() {
			return CompoundIntersects<T>(this);
		}

		public bool HasIntersectedTypes(params Type[] types) {
			if (types.Length == 0) throw new ArgumentException($"At least one type must be passed");

			foreach (ICollideable X in GV.MonoGameImplement.monogameObjects.Generate<ICollideable>()) {
				if (X != this && GV.Misc.TypesContainsGivenType(X.GetType(), types) && Intersects(this, X))
					return true; // Has intersected a desired type
			}

			return false; // Has not intersected desired type
		}

		public bool HasIntersectedType(Type type) {
			foreach (ICollideable X in GV.MonoGameImplement.monogameObjects.Generate<ICollideable>()) {
				if (X != this && GV.Misc.DoesExtend(X.GetType(), type) && Intersects(this, X))
					return true; // Has intersected a desired type
			}

			return false; // Has not intersected desired type
		}

		public bool HasIntersectedTypes(Vector2 offset, params Type[] types) {
			return CheckOffsetMethodCaller(offset, () => HasIntersectedTypes(types));
		}

		public bool HasIntersectedType(Type type, Vector2 offset) {
			return CheckOffsetMethodCaller(offset, () => HasIntersectedType(type));
		}

		/// <summary>Action method which accepts a type and returns a value of said type</summary>
		/// <typeparam name="V">Value type for delegate handler/method to return</typeparam>
		protected delegate V DelegatedAction<V>();

		protected V CheckOffsetMethodCaller<V>(Vector2 offset, DelegatedAction<V> method) {
			OffsetSpritePosition(offset); // Push by desired amount
			V result = method(); // Store Method Result Pre-Emptively
			OffsetSpritePosition(-offset); // Push back to original

			return result; // Return stored method result
		}

		/// <summary>Offsets both sprite and boundary positon</summary>
		/// <param name="XY">Vector by which to offset positions</param>
		public override void OffsetSpritePosition(Vector2 XY) {
			base.OffsetSpritePosition(XY);

			if (boundary != null) boundary.Offset(XY);
		}

		public override void SetPosition(Vector2 nPos) {
			base.SetPosition(nPos);
			BuildBoundary();
		}

		/// <summary>Builds sprite boundary container for collision detection</summary>
		/// <remarks>Default boundary will be the sprite rect I.E. width and height = position</remarks>
		protected abstract void BuildBoundary();
		
		
		/// <summary>Sprite collision boundary</summary>
		protected IBoundaryContainer boundary;
		
		/// <summary>Read only access for boundary outside of class</summary>
		public IBoundaryContainer Boundary { get { return boundary; } private set { boundary = value; } }
	}
}
