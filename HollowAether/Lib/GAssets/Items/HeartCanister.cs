using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.Items {
	public partial class HeartCanister : CollideableSprite, IInteractable, IItem, IContextualised {
		protected override void BuildSequenceLibrary() {
			throw new NotImplementedException();
		}

		protected override void BuildBoundary() {
			throw new NotImplementedException();
		}

		public void Interact() {

		}

		public bool OutputReady() {
			return false;
		}

		public String OutputString { get; set; }
		public bool Interacted { get; set; } = false;
		public bool CanInteract { get; set; } = true;
		public String ItemID { get; private set; }
	}
}
