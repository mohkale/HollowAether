using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HollowAether.Lib.GAssets;
using System.Windows.Forms;
using System.Drawing;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.GAssets;
using HollowAether.Lib;
using T = System.Threading.Thread;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether {
	public class GameEntity : Object, ICloneable {
		private class EntityAttributeCollection : Dictionary<String, EntityAttribute>, ICloneable {
			public EntityAttributeCollection() : base() { }

			public object Clone() {
				EntityAttributeCollection clone = new EntityAttributeCollection();

				foreach (KeyValuePair<String, EntityAttribute> keyValuePair in this) {
					clone.Add(keyValuePair.Key, (EntityAttribute)keyValuePair.Value.Clone());
				}

				return clone; // Return cloned entity attribute collection
			}

			public EntityAttributeCollection CastClone() {
				return Clone() as EntityAttributeCollection;
			}
		}
		
		public GameEntity(string entityId) {
			foreach (KeyValuePair<String, EntityAttribute> defaultParams in GetDefaultEntityParameters()) {
				entityAttributes[defaultParams.Key] = defaultParams.Value; // Assign default Params
			}

			EntityType = entityId;
		}

		public GameEntity(string entityId, KeyValuePair<String, EntityAttribute>[] args) : this(entityId) {
			foreach (var defaultParams in args) { // Any argument attributes
				if (!entityAttributes.ContainsKey(defaultParams.Key)) entityAttributes.Add(defaultParams.Key, defaultParams.Value); else {
					entityAttributes[defaultParams.Key] = defaultParams.Value; // If has existing texture
				}
			}
		}

		/// <summary>Clones current entity instance</summary>
		/// <returns>Shallow clone of current Game Entity</returns>
		public Object Clone() {
			GameEntity self = MemberwiseClone() as GameEntity;

			self.entityAttributes = this.entityAttributes.CastClone();

			return self; // Return cloned game entity instance
		}

		public Object Clone(RectangleF newRegion) {
			GameEntity self = this.Clone() as GameEntity;

			self["Position"].Value = new Vector2(newRegion.X, newRegion.Y);
			self["Width"].Value = (int)newRegion.Width;
			self["Height"].Value = (int)newRegion.Height;

			return self; // Return new self adjusted to region
		}

		private static ICollection<KeyValuePair<String, EntityAttribute>> GetDefaultEntityParameters(
				Vector2? position = null, int width = 0, int height = 0, 
				string texture = "", bool animRunning=true, float layer=0.1f) {
			Vector2 positionVal = position.HasValue ? position.Value : Vector2.Zero; // Value From Argument

			return new KeyValuePair<String, EntityAttribute>[] {
				new KeyValuePair<string, EntityAttribute>("Position",		  GetEntityAttribute(positionVal, Vector2.Zero)),
				new KeyValuePair<string, EntityAttribute>("Width",			  GetEntityAttribute(width, 0)),
				new KeyValuePair<string, EntityAttribute>("Height",			  GetEntityAttribute(height, 0)),
				new KeyValuePair<string, EntityAttribute>("Texture",		  GetEntityAttribute(texture, String.Empty)),
				new KeyValuePair<string, EntityAttribute>("AnimationRunning", new EntityAttribute(animRunning, typeof(bool), false)),
				new KeyValuePair<string, EntityAttribute>("Layer",			  new EntityAttribute(layer, typeof(float), false)),
			};
		}

		private static EntityAttribute GetEntityAttribute<T>(T defaultValue, T nullValue, bool readOnly=false) {
			if (defaultValue.Equals(nullValue)) { return new EntityAttribute(typeof(T), readOnly); } else {
				return new EntityAttribute(defaultValue, typeof(T), readOnly); // With default value
			}
		}

		public void AddEntityAttribute(string key, EntityAttribute value) {
			if (entityAttributes.ContainsKey(key)) throw new HollowAetherException($"Entity Key '{key}' Already Exists");
			entityAttributes.Add(key, value); // Store new entity attribute to existing attribute store/collection
		}

		public void AssignAttribute(String key, object value) {
			GetAttribute(key).Value = value;
		}
	
		public String[] GetEntityAttributes() {
			return entityAttributes.Keys.ToArray();
		}

		public EntityAttribute GetAttribute(string key) {
			if (entityAttributes.ContainsKey(key)) return entityAttributes[key]; else {
				throw new HollowAetherException($"Entity '{EntityType}' Doesn't Possess Attribute '{key}'");
			}
		}

		public bool AttributeExists(string key) { return entityAttributes.ContainsKey(key); }

		public String ToFileContents(string assetID) {
			string entityName = T.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(EntityType);

			var attributeEnumerable = from X in entityAttributes where X.Value.IsAssigned select $"{X.Key}={X.Value.ToFileContents()}";
			
			var attributes = (attributeEnumerable).Aggregate((a, b) => $"{a}<{b}"); // Convert to attribute string

			return ($"GameEntity: \"{entityName}\" as [{assetID}] " + (attributes.Length > 0 ? "<" : "") + attributes).Trim();
		}

		public IMonoGameObject ToIMGO() {
			IMonoGameObject _object = GlobalVars.EntityGenerators.StringToMonoGameObject(EntityType);
			_object.AttatchEntity(this);
			return _object;
 		}

		public string EntityType { get; private set; }

		public EntityAttribute this[string key] { get { return GetAttribute(key); } }

		/// <summary>Key/Value store holding relevent entity attributes for game entity instance</summary>
		private EntityAttributeCollection entityAttributes = new EntityAttributeCollection();
	}
}
