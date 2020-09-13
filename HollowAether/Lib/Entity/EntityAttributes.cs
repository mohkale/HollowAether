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
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;
using Converters = HollowAether.Lib.InputOutput.Parsers.Converters;
#endregion

namespace HollowAether {
	public class EntityAttribute : Object, ICloneable {
		public EntityAttribute(Type attributeType, bool isReadOnly=false) {
			IsReadOnly = isReadOnly; // Store whether attribute can be altered
			Type = attributeType; // Store what type this entity attribute aceepts
		}

		public EntityAttribute(object defaultValue, Type attributeType, bool isReadOnly=false) 
			: this(attributeType, isReadOnly) { SetAttribute(defaultValue); }

		public object Clone() { return (EntityAttribute)MemberwiseClone(); }

		public object GetValue() {
			if (!this.IsAssigned) throw new EntityAttributeUnassignedValueRetrievalException(); else {
				return value; // Value has been assigned so return stored value.
			}
		}

		/// <summary>Returns Value if Value isn't Asset else Return Asset Value</summary>
		/// <returns>Actual Value Represented By Entity Attribute.</returns>
		public object GetActualValue() {
			object value = GetValue(); // Gets actual value or assets ref. Also checks for value not assigned etc. 
			return (this.IsAssetReference) ? (value as Asset).asset : value; // Get actual value pointed to by attr
		}

		public void Delete() {
			if (IsReadOnly) throw new EntityAttributeReadOnlyDeletionException();
			value = null; // Erase Value Of Entity Attribute By Setting To Null.
		}

		public void SetAttribute(object newValue) {
			if (IsAssigned && IsReadOnly) throw new EntityAttributeReadOnlyAssignmentAttemptException();

			if (newValue != null) {
				if (newValue is Asset) { // Is Asset Reference
					if (!(newValue as Asset).TypesMatch(this.Type))
						throw new EntityAttributeInvalidTypeAssignmentException((newValue as Asset).assetType, Type);
				} else {
					if (!GV.Misc.DoesExtend(newValue.GetType(), this.Type))
						throw new EntityAttributeInvalidTypeAssignmentException(newValue.GetType(), Type);
				}

				value = newValue; // If no exception thrown then new value is a valid value
			}
		}

		public string ToFileContents(bool throwExceptionIfUnassigned=true) {
			if (IsAssigned) {
				return Converters.ValueToString(Type, value);
				/*if (IsAssetReference) return $"[{(value as Asset).assetID}]"; else {
					return Converters.ValueToString(Type, value);
				}*/
			} else {
				if (!throwExceptionIfUnassigned) return String.Empty; else {
					throw new HollowAetherException($"Cannot Convert Unassigned Value");
				}
			}
		}

		public bool IsAssetReference { get { return value is Asset; } }

		public bool IsAssigned { get { return value != null; } }

		public object Value { get { return value; } set { SetAttribute(value); } }

		public readonly bool IsReadOnly;
		public readonly Type Type;
		private object value = null;
	}
}
