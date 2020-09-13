using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib {
	public class RemovalBatch {
		/// <summary>Batch of items to remove from stored MGObjects</summary>
		/// <param name="capacity">Minimum batch length before resizing</param>
		public RemovalBatch(int capacity=35) {
			toRemove = new List<IMonoGameObject>(capacity);
		}

		/// <summary>Marks object which is to be removed</summary>
		/// <param name="monogameObjectID">ID of target object</param>
		public void Add(IMonoGameObject monogameObject) {
			toRemove.Add(monogameObject);
		}

		/// <summary>Executes removal batch</summary>
		public void Execute() {
			foreach (IMonoGameObject _object in toRemove) {
				GV.MonoGameImplement.monogameObjects.Remove(_object); // delete object
			}

			toRemove.Clear(); // Remove all stored elements in batch
		}

		private List<IMonoGameObject> toRemove;
	}

	public class AdditionBatch {
		public AdditionBatch() {
			batch = new List<Tuple<bool, IMonoGameObject>>();
		}

		public void Add(String ID, IMonoGameObject _object) {
			_object.SpriteID = ID; // Store to object instance
			batch.Add(new Tuple<bool, IMonoGameObject>(true, _object));
		}

		public void AddRangeNameless(params IMonoGameObject[] args) {
			batch.AddRange((from X in args select new Tuple<bool, IMonoGameObject>(false, X)));
		}

		public void AddNameless(IMonoGameObject _object) {
			batch.Add(new Tuple<bool, IMonoGameObject>(false, _object));
		}

		public void Execute() {
			foreach (var X in batch) {
				if (X.Item1) { GV.MonoGameImplement.monogameObjects.Add(X.Item2); }
				else { GV.MonoGameImplement.monogameObjects.AddNameless(X.Item2); }
			}

			batch.Clear();
		}

		private List<Tuple<bool, IMonoGameObject>> batch;
	}
}
