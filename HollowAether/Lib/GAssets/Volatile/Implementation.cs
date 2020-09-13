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
using GV = HollowAether.Lib.GlobalVars;
#endregion

#region HollowAetherImports
using HollowAether.Lib.Exceptions.CE;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.MapZone;
#endregion

namespace HollowAether.Lib.GAssets {
	/// <summary>Volatile namespace to seperate manager from implementation</summary>
	namespace Volatile {
		public abstract class VolatilityManager {
			protected VolatilityManager(object arg, bool implementerReadOnly) {
				ImplementerCanBeChanged = !implementerReadOnly;
				if (arg != null) InitialiseVolatility(arg);
			}

			public static VolatilityManager GetManager(VolatilityType type, object arg=null, bool readOnly=false) {
				switch (type) {
					case VolatilityType.Timeout: return new TimeoutVolatilityManager(arg, readOnly);
					case VolatilityType.Other:   return new OtherVolatilityManager(arg,   readOnly);
					default: throw new HollowAetherException($"Unknown Volatility Type {type}");
				}
			}

			public void InitialiseVolatility(object volatilityArg) {
				if (ValidArg(volatilityArg)) Implementer = volatilityArg; else {
					throw new HollowAetherException($"Invalid Volatility Arg");
				}
			}

			public void ChangeImplementer(object arg) {
				if (ImplementerCanBeChanged) InitialiseVolatility(arg); else {
					throw new HollowAetherException($"Manager Implementation Can't Be Changed");
				}
			}

			public void Delete(IMonoGameObject self) {
				GV.MonoGameImplement.removalBatch.Add(self);
				Deleting(); // Trigger deleting event handler
			}

			public void Update(IMonoGameObject self) {
				if (ReadyToDelete(self)) Delete(self); else UponNonDeletion(self);
			}

			public abstract bool ValidArg(object arg);

			protected abstract bool ReadyToDelete(IMonoGameObject self);

			protected abstract void UponNonDeletion(IMonoGameObject self);

			public object Implementer { get; protected set; } // VolatilityArg

			public bool ImplementerCanBeChanged { get; private set; }

			public event Action Deleting = () => { };
		}

		public sealed class OtherVolatilityManager : VolatilityManager {
			public OtherVolatilityManager(object arg=null, bool implementerReadOnly=true) 
				: base(arg, implementerReadOnly) { }

			public override bool ValidArg(object arg) {
				return arg is ImplementVolatility;
			}

			protected override bool ReadyToDelete(IMonoGameObject _object) {
				return ((ImplementVolatility)Implementer)((IMonoGameObject)_object);
			}

			protected override void UponNonDeletion(IMonoGameObject self) {
				// Do Nothing // throw new NotImplementedException();
			}
		}

		public sealed class TimeoutVolatilityManager : VolatilityManager {
			public TimeoutVolatilityManager(object arg=null, bool implementerReadOnly=true)
				: base(arg, implementerReadOnly) { }

			public void IncrementImplementer(int value) {
				ChangeImplementer((int)Implementer + value);
			}

			public void DecrementImplementer(int value) {
				ChangeImplementer((int)Implementer - value);
			}

			protected override bool ReadyToDelete(IMonoGameObject self) {
				return (int)Implementer < 0;
			}

			protected override void UponNonDeletion(IMonoGameObject self) {
				Implementer  = (int)Implementer - GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds;
			}

			public override bool ValidArg(object arg) {
				try { Convert.ToInt32(arg); return true; } catch { return false; }
			}
		}
	}

	/// <summary>Enumeration representing acceptable volatility types</summary>
	public enum VolatilityType { Timeout, Other } // other uses ImplementVolatility Enumeration
	// FrameTimeout is buggy and needs re-assesment, ignore use until such a time when it functions correctly

	/// <summary>Acceptable delegate to pass as arg to volatileManager or IVolatile class Initializer</summary>
	/// <param name="iv">IVolatile class instance for check purposes; doesn't have to be used</param>
	/// <returns>Boolean telling class wether or not the class is ready to be deleted</returns>
	public delegate bool ImplementVolatility(IMonoGameObject _volatile); // for other check

	#region Implementations
	public abstract partial class VolatileSprite : Sprite, IVolatile {
		public VolatileSprite(Vector2 position, int width, int height, bool animationRunning = true)
			: base(position, width, height, animationRunning) {}

		public void InitializeVolatility(VolatilityType vt, object arg) {
			VolatilityManager = Volatile.VolatilityManager.GetManager(vt, arg);
		}

		public override void Update(bool updateAnimation) {
			VolatilityManager.Update(this);
			base.Update(updateAnimation);
		}
		
		public Volatile.VolatilityManager VolatilityManager { get; private set; }
	}

	public abstract class VolatileCollideableSprite : CollideableSprite, IVolatile {
		public VolatileCollideableSprite(Vector2 position, int width, int height, bool animationRunning = true)
			: base(position, width, height, animationRunning) { }

		public void InitializeVolatility(VolatilityType vt, object arg) {
			VolatilityManager = Volatile.VolatilityManager.GetManager(vt, arg);
		}

		public override void Update(bool updateAnimation) {
			VolatilityManager.Update(this);
			base.Update(updateAnimation);
		}
		
		public Volatile.VolatilityManager VolatilityManager { get; private set; }
	}

	public abstract class VolatileBodySprite : BodySprite, IVolatile {
		public VolatileBodySprite(Vector2 position, int width, int height, bool animationRunning = true)
			: base(position, width, height, animationRunning) { }

		public void InitializeVolatility(VolatilityType vt, object arg) {
			VolatilityManager = Volatile.VolatilityManager.GetManager(vt, arg);
		}

		public override void Update(bool updateAnimation) {
			VolatilityManager.Update(this);
			base.Update(updateAnimation);
		}

		public Volatile.VolatilityManager VolatilityManager { get; private set; }
	}
	#endregion
}
