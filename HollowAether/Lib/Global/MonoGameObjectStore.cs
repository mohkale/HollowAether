using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using HollowAether.Lib.Exceptions;

using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib {
	/// <summary></summary>
	/// <remarks>Uses two lists instead of dictionary, because faster at expense of memory</remarks>
	public class MonoGameObjectStore : IEnumerable<IMonoGameObject>, IEnumerable {
		public MonoGameObjectStore() {
			objects = new Dictionary<string, IMonoGameObject>();
			unnamedObjects = new List<IMonoGameObject>();
		}

		public int Count() {
			return unnamedObjects.Count + objects.Count;
		}

		public MonoGameObjectStore(int capacity) {
			objects = new Dictionary<string, IMonoGameObject>(capacity / 2);
			unnamedObjects = new List<IMonoGameObject>(capacity / 2);
		}

		public void Add(String key, IMonoGameObject _object) {
			if (Exists(key)) throw new HollowAetherException($"Cannot Add IMGO '{key}', When It Already Exists");

			_object.SpriteID = key; // in case isn't stored already
			objects.Add(key, _object); // Store to local object store
		}

		public void AddNameless(IMonoGameObject _object) {
			unnamedObjects.Add(_object);
		}

		public void AddRangeNameless(params IMonoGameObject[] args) {
			unnamedObjects.AddRange(args);
		}

		public void Add(IMonoGameObject _object) {
			if (String.IsNullOrWhiteSpace(_object.SpriteID)) // If invalid sprite ID detected
				throw new HollowAetherException($"New Object Of Type {_object} Lacks a Sprite ID");

			objects.Add(_object.SpriteID, _object); // Store to local object store
		}

		public bool Exists(String spriteID) {
			return objects.ContainsKey(spriteID);
		}

		public bool Exists(IMonoGameObject _object) {
			return objects.ContainsValue(_object) || unnamedObjects.Contains(_object);
		}

		public IEnumerator<IMonoGameObject> Generate() {
			return GetEnumerator();
		}

		public IEnumerable<IMonoGameObject> Generate(Type type) {
			foreach (IMonoGameObject X in this) {
				if (GV.Misc.DoesExtend(X.GetType(), type))
					yield return X; // Yield of child or same
			}
		}

		public IEnumerable<IMonoGameObject> Generate(params Type[] types) {
			foreach (IMonoGameObject _object in this) {
				if (GV.Misc.TypesContainsGivenType(_object.GetType(), types))
					yield return _object; 
			}
		}

		#region GenericLimitedImplimentation
		public IEnumerable<IMonoGameObject> Generate<T>() {
			return Generate(typeof(T));
		}

		public IEnumerable<IMonoGameObject> Generate<T1, T2>() {
			return Generate(typeof(T1), typeof(T2));
		}

		public IEnumerable<IMonoGameObject> Generate<T1, T2, T3>() {
			return Generate(typeof(T1), typeof(T2), typeof(T3));
		}

		public IEnumerable<IMonoGameObject> Generate<T1, T2, T3, T4>() {
			return Generate(typeof(T1), typeof(T2), typeof(T3), typeof(T4));
		}

		public IEnumerable<IMonoGameObject> Generate<T1, T2, T3, T4, T5>() {
			return Generate(typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));
		}
		#endregion

		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		public IEnumerator<IMonoGameObject> GetEnumerator() {
			foreach (IMonoGameObject named in objects.Values) {
				yield return named;
			}

			foreach (IMonoGameObject unnamed in unnamedObjects) {
				yield return unnamed;
			}
		}

		private int GetIndexFromSpriteID(String ID) {
			String[] keys = objects.Keys.ToArray(); // Get keys

			foreach (int X in Enumerable.Range(0, keys.Length)) {
				if (keys[X] == ID) return X; // Return index
			}

			throw new HollowAetherException($"Object with ID '{ID}' Not Found");
		}

		public IMonoGameObject Get(String ID) {
			return objects[ID];
		}

		public void Remove(String ID) {
			objects.Remove(ID); // Delete key
		}

		public void ClearAll() {
			objects.Clear();
			unnamedObjects.Clear();
		}

		public void Remove(IMonoGameObject _object) {
			try   { Remove(_object.SpriteID);       } 
			catch { unnamedObjects.Remove(_object); }
		}

		public IMonoGameObject this[String spriteID] {
			get { return Get(spriteID); }
		}

		public int Length { get { return Count(); } }

		private Dictionary<String, IMonoGameObject> objects;
		private List<IMonoGameObject>				unnamedObjects;
	}
}
