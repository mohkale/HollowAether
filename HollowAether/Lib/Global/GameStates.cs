#region SystemImports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace HollowAether.Lib {
	/// <summary>Enumeration to hold the current state of the game</summary>
	public enum GameState {
		GameRunning, /// <summary>Game state which actually manages game-play assets etc.</summary>
		Home, /// <summary>Main game state which the player is shown upon starting the game</summary>
		SaveLoad, /// <summary>State from which to load an existing save or choose to create a new save</summary>
		PlayerDeceased,
		Settings, /// <summary>State from where the player can modify window settings etc.</summary>
		Credits, /// <summary>Game state from which all contributors to the game will be mentioned</summary> 
		GamePaused /// <summary>Pause window shown; Where gamerunning is still drawn, but not updated</summary>
	}
}
