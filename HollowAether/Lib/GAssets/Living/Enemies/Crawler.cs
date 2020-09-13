using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
#endregion

namespace HollowAether.Lib.GAssets {
	public partial class Crawler : Enemy, IPredefinedTexture {
		public Crawler(Vector2 position, int level) : base(position, 32, 32, level, true) {

		}

		protected override void BuildSequenceLibrary() {
			throw new NotImplementedException();
		}

		protected override void BuildBoundary() {
			throw new NotImplementedException();
		}

		protected override void DoEnemyStuff() {
			throw new NotImplementedException();
		}

		protected override bool CanGenerateHealth { get; set; } = false;

		protected override bool CausesContactDamage { get; set; } = true;
		
		public const int SPRITE_WIDTH = 32, SPRITE_HEIGHT = 32;
	}
}
