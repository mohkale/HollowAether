#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
#endregion

#region XNAImports
using Microsoft.Xna.Framework;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.GAssets;
using GV = HollowAether.Lib.GlobalVars;
#endregion

namespace HollowAether {
	public class AssetContainer : Dictionary<String, Asset> {
		#region BaseConstructorImplement
		public AssetContainer() : base() { }
		public AssetContainer(int capacity) : base(capacity) { }
		public AssetContainer(IEqualityComparer<String> comp) : base(comp) { }
		public AssetContainer(IDictionary<String, Asset> dict) : base(dict) { }
		public AssetContainer(int cap, IEqualityComparer<String> comp) : base(cap, comp) { }
		public AssetContainer(SerializationInfo info, StreamingContext context) : base(info, context) { }
		public AssetContainer(IDictionary<String, Asset> dictionary, IEqualityComparer<String> comparer) : base(dictionary, comparer) { }
		#endregion
	}

	#region AssetDefinitions
	/// <summary>Holder of assets defined in zone files</summary>
	public class Asset : Object, ICloneable {
		/// <summary>Default asset constructor</summary>
		/// <param name="type">Type of asset</param>
		/// <param name="_ID">ID of asset</param>
		/// <param name="assetValue">Value of asset</param>
		public Asset(Type type, String _ID, Object assetValue) {
			_assetType = type;
			_assetID = _ID;
			SetAsset(assetValue);
		}

		/// <summary>Converts instance to human readable string</summary>
		public override string ToString() { return $"({assetID}): Asset of Type '{assetType}'"; }

		public Object Clone() { return (Asset)MemberwiseClone(); }

		/// <summary>Sets value of asset</summary>
		/// <param name="value">Value to set asset</param>
		public void SetAsset(Object value) {
			_asset = value; // Set asset for later checks
			CheckType(); // Check type
		}

		/// <summary>Checks whether given type is compatible with current asset</summary>
		/// <param name="checkType">Type to compare to current asset type</param>
		protected virtual void CheckType() {
			if (_asset.GetType() != assetType)
				throw new AssetAssignmentException(assetType, _asset.GetType());
		}

		/// <summary>Checks Whether Asset Type Matches Argument Type</summary>
		/// <param name="type">Type to compare with</param>
		/// <returns>Boolean indicating match</returns>
		public bool TypesMatch(Type type) {
			return GV.Misc.DoesExtend(type, _assetType);
		}

		/// <summary>Checks Whether Asset Type Matches Argument Type</summary>
		/// <param name="arg">Argument to compare types with</param>
		/// <returns>Boolean indicating match</returns>
		public bool TypesMatch(Object arg) {
			return TypesMatch(arg.GetType());
		}

		/// <summary>Checks Whether Asset Type Matches Argument Type</summary>
		/// <typeparam name="T">Type to compare with asset</typeparam>
		/// <returns>Boolean indicating match</returns>
		public bool TypesMatch<T>() {
			return TypesMatch(typeof(T));
		}

		public virtual object GetValue() {
			return _asset;
		}

		public virtual void Delete() {
			_asset = null;
		}

		protected Object _asset;     // Value of asset
		protected Type _assetType; // Type of Asset
		protected String _assetID;   // ID given to Asset

		public Type assetType { get { return _assetType; } }

		public String assetID { get { return _assetID; } }

		public Object asset { get { return GetValue(); } set { SetAsset(value); } }

		public Boolean IsImportedAsset { get; set; } = false;
	}

	public class BooleanAsset : Asset {
		public BooleanAsset(String id, bool value) : base(typeof(Boolean), id, value) { }

		public BooleanAsset(String id, Object value) : base(typeof(Boolean), id, value) { }

		public override string ToString() {
			return $"({assetID}): Boolean Asset of Type '{assetType}'";
		}
	}

	public class FlagAsset : Asset {
		public FlagAsset(String id, bool initialValue = false) : base(typeof(Flag), id, new Flag(initialValue)) {
			InitialValue = initialValue;
		}

		public FlagAsset(String id, Object initialValue) : base(typeof(Flag), id, new Flag((bool)initialValue)) {
			InitialValue = (bool)initialValue;
		}

		public override string ToString() {
			return $"({assetID}): Flag Asset of Type '{assetType}'";
		}

		public bool InitialValue { get; private set; }
	}

	public class TextureAsset : Asset {
		public TextureAsset(String id, String textureKey) : base(typeof(String), id, textureKey) { }

		public TextureAsset(String id, Object textureKey) : base(typeof(String), id, (String)textureKey) { }

		public override string ToString() {
			return $"({assetID}): Texture Asset of Type '{assetType}'";
		}

		protected override void CheckType() {
			base.CheckType(); // Checks whether value types match

			if (!GV.MonoGameImplement.textures.ContainsKey(asset.ToString()))
				throw new HollowAetherException($"Texture defined in zone '{_asset}' Not Found");
		}
	}

	public class PositionAsset : Asset {
		public PositionAsset(String id, Vector2 position) : base(typeof(Vector2), id, position) { }

		public PositionAsset(String id, Object position) : base(typeof(Vector2), id, (Vector2)position) { }

		public override string ToString() {
			return $"({assetID}): Position Asset of Type '{assetType}'";
		}
	}

	public class IntegerAsset : Asset {
		public IntegerAsset(String id, int value) : base(typeof(int), id, value) { }

		public IntegerAsset(String id, Object value) : base(typeof(int), id, (int)value) { }

		public override string ToString() {
			return $"({assetID}): Integer Asset of Type '{assetType}'";
		}
	}

	public class FloatAsset : Asset {
		public FloatAsset(String id, float value) : base(typeof(float), id, value) { }

		public FloatAsset(String id, Object value) : base(typeof(float), id, (float)value) { }

		public override string ToString() {
			return $"({assetID}): Float Asset of Type '{assetType}'";
		}
	}

	public class StringAsset : Asset {
		public StringAsset(String id, String value) : base(typeof(String), id, value) { }

		public StringAsset(String id, Object value) : base(typeof(String), id, (String)value) { }

		public override string ToString() {
			return $"({assetID}): String Asset of Type '{assetType}'";
		}
	}

	public class AnimationAsset : Asset {
		public AnimationAsset(String id, AnimationSequence value) : base(typeof(AnimationSequence), id, value) { }

		public AnimationAsset(String id, Object value) : base(typeof(AnimationSequence), id, (AnimationSequence)value) { }

		public override string ToString() { return $"({assetID}): Animation Asset of Type '{assetType}'"; }
	}
	#endregion
}
