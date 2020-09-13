using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GV = HollowAether.Lib.GlobalVars;

using HollowAether.Lib.GAssets;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GAssets.Weapons {
	public class Sword : SwordLikeWeapon {
		public Sword(Vector2 position) : base(position, 28, 28, true) { Initialize(@"weapons\sword"); }

		protected override void FinishAttacking() { base.FinishAttacking(); }

		public override int HorizontalBoundaryOffset { get; protected set; } = 8;

		public override float AngularThrowVelocity { get; protected set; } = (float)Math.PI * 4;

		public override int FrameWidth { get; protected set; } = 28;

		public override int FrameHeight { get; protected set; } = 28;

		public override int ActualWeaponWidth { get; protected set; } = 13;

		public override int ActualWeaponHeight { get; protected set; } = 28;
	}
}
