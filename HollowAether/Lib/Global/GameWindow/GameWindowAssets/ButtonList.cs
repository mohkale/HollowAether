using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HollowAether.Lib.Exceptions;

namespace HollowAether.Lib.GameWindow {
	public class ButtonList : IEnumerable<Button> {
		public ButtonList(int markActive=0, int buttonSpan=3, params Button[] args) {
			if (args.Length == 0) throw new HollowAetherException($"Button List needs at least one button");

			buttons = new List<Button>(buttonSpan); // General length 3
			AddButtons(args); // Store all buttons within list locally
			activeButtonIndex = markActive; // Store current active index
			buttons[activeButtonIndex].Active = true; // Set to true
		}

		public IEnumerator<Button> GetEnumerator() { return buttons.GetEnumerator(); }

		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public void AddButtons(params Button[] args) { buttons.AddRange(args); }

		public void RemoveButton(Button button) { buttons.Remove(button); }

		public void MoveToNextButton() {
			buttons[activeButtonIndex].Toggle(); // De activate chosen button by changing animation
			activeButtonIndex = (activeButtonIndex + 1 >= buttons.Count) ? 0 : activeButtonIndex + 1;
			buttons[activeButtonIndex].Toggle(); // Activate newly selected button by changing animation
		}

		public void MoveToPreviousButton() {
			buttons[activeButtonIndex].Toggle(); // De activate chosen button by changing animation
			activeButtonIndex = (activeButtonIndex - 1 < 0) ? buttons.Count - 1 : activeButtonIndex - 1;
			buttons[activeButtonIndex].Toggle(); // Activate newly selected button by changing animation
		}

		public void Update(bool updateAnimation) {
			foreach (Button button in buttons) button.Update(updateAnimation);
		}

		public void Draw() {
			foreach (Button button in buttons) button.Draw();
		}

		public Button Last() { return buttons[buttons.Count - 1]; }

		public Button this[int X] { get { return buttons[X]; } }

		public int Length { get { return buttons.Count; } }

		public int ActiveButtonIndex { get { return activeButtonIndex; } set {
			if (activeButtonIndex >= buttons.Count)
				throw new HollowAetherException("Button Count Out Of Range");

			while (activeButtonIndex != value) MoveToNextButton();
		} }

		public Button ActiveButton { get { return buttons[activeButtonIndex]; } }

		private int activeButtonIndex;
		private List<Button> buttons = new List<Button>();
	}
}
