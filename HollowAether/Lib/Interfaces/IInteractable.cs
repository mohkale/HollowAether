using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib {

	public interface IInteractable {
		void Interact();
		bool Interacted { get; }
		bool CanInteract { get; }
	}

	public interface ITimeoutSupportedInteractable : IInteractable {
		int InteractionTimeout { get; set; }
	}

	public interface IAutoInteractable : IInteractable {
		bool InteractCondition();
	}

	public interface ITimeoutSupportedAutoInteractable : IAutoInteractable {
		int InteractionTimeout { get; set; }
	}
}
