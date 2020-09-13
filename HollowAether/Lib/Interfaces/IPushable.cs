using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GV = HollowAether.Lib.GlobalVars;
using HollowAether.Lib.Exceptions;
using HollowAether.Lib.GAssets;

namespace HollowAether.Lib {
	public interface IPushable {
		void Push(PushArgs args);
		void PushTo(Vector2 position, float over);
		PushArgs PushPack { get; }
		bool BeingPushed { get; }
		Vector2 Position { get; set; }
	}
}
