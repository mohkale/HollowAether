using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SysInfo = System.Windows.Forms.SystemInformation;
using HollowAether.Lib.GAssets;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.Forms.LevelEditor {
	public class AnimationViewDataStore : Panel {
		public class FramesContainer : Panel {
			public FramesContainer(AnimationSequence sequence, int tileDimensions) {
				Height = tileDimensions; // Encompasses Entire Height Of Frame Tiles
				BackColor = Color.Black; // Background Color Is Black, To Contrast Tile
				this.sequence = sequence; // Store reference to animation sequence for ops
				this.tileDimensions = tileDimensions;

				tileBoxes.AddRange(EnumerateFramesAsTileBoxes().ToArray());
				Controls.AddRange(tileBoxes.ToArray()); // Add all tiles to control
			}

			private IEnumerable<TileBox> EnumerateFramesAsTileBoxes() {
				int offset = 0; // Offset from horizontal axis
				ContextMenuStrip cm = GetContextMenuStrip();

				foreach (Frame frame in sequence.Frames) {
					yield return GetTileBox(frame, offset, cm);
					offset += tileDimensions + TILE_OFFSET;
				}
			}

			private TileBox GetTileBox(Frame frame, int offset, ContextMenuStrip cm) {
				return new TileBox(frame) {
					#region Body
					Width = tileDimensions,
					Height = tileDimensions,
					ContextMenuStrip = cm,
					Left = offset,
					BackColor = Color.Red,
					#endregion
				};
			}

			private ContextMenuStrip GetContextMenuStrip() {
				ToolStripMenuItem modify = new ToolStripMenuItem("Modify");
				modify.Click += Modify_ContextMenuEventHandler;

				ToolStripMenuItem delete = new ToolStripMenuItem("Delete");
				delete.Click += Delete_ContextMenuEventHandler;

				ToolStripMenuItem moveLeft = new ToolStripMenuItem("Move Left");
				moveLeft.Click += MoveLeft_ContextMenuEventHandler;

				ToolStripMenuItem moveRight = new ToolStripMenuItem("Move Right");
				moveRight.Click += MoveRight_ContextMenuEventHandler;

				ContextMenuStrip cm = new ContextMenuStrip();

				cm.Items.AddRange(new ToolStripItem[] {
					modify, delete, moveLeft, moveRight
				});

				return cm;
			}

			public void ModifyFrame_Call(int index, TileBox tileBox=null) {
				tileBox = (tileBox == null) ? tileBoxes[index] : tileBox;

				string textureKey = GV.LevelEditor.GetTextureKeyFromInstance(tileBox.Texture);

				EditFrameDialog edf = new EditFrameDialog(sequence, index, textureKey);
				edf.FormClosing += (s, e2) => { tileBox.AssignFrame(sequence[index]); };

				edf.Show(); // Display newly made Dialog form to user/caller etc.
			}

			#region ContextMenuEventHandlers
			private void Modify_ContextMenuEventHandler(object sender, EventArgs e) {
				TileBox tileBox = GetTileBoxFromToolStripItem(sender as ToolStripItem);
				if (tileBox != null)  ModifyFrame_Call(tileBoxes.IndexOf(tileBox), tileBox);
			}

			private void Delete_ContextMenuEventHandler(object sender, EventArgs e) {
				if (tileBoxes.Count == 1)
					MessageBox.Show("Cannot Have Animation With No Frames", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else {
					TileBox tileBox = GetTileBoxFromToolStripItem(sender as ToolStripItem);

					if (tileBox != null) {
						int index      = tileBoxes.IndexOf(tileBox);   // Get corresponding index
						bool lastValue = index == tileBoxes.Count - 1; // Is last tile

						#region DeleteReferences
						tileBoxes.RemoveAt(index);
						Controls.RemoveAt(index);
						sequence.Frames.RemoveAt(index);
						#endregion

						if (!lastValue) { // Have to readjust remaining tiles
							foreach (int X in Enumerable.Range(index, tileBoxes.Count - 1)) {
								tileBoxes[X].Left = (X) * (tileDimensions + TILE_OFFSET);
							}
						}
					}
				}
			}

			private void MoveLeft_ContextMenuEventHandler(object sender, EventArgs e) {
				TileBox tileBox = GetTileBoxFromToolStripItem(sender as ToolStripItem);

				if (tileBox != null) {
					int index = tileBoxes.IndexOf(tileBox); // Get corresponding index

					if (index != 0) { // When not first item, can be moved to left
						tileBoxes[index].Left = (index - 1) * (tileDimensions + TILE_OFFSET);
						tileBoxes[index - 1].Left = (index) * (tileDimensions + TILE_OFFSET);

						GV.CollectionManipulator.SwapListValues(sequence.Frames, index, index - 1);
						GV.CollectionManipulator.SwapListValues(tileBoxes, index, index - 1);
					}
				}
			}

			private void MoveRight_ContextMenuEventHandler(object sender, EventArgs e) {
				TileBox tileBox = GetTileBoxFromToolStripItem(sender as ToolStripItem);

				if (tileBox != null) {
					int index = tileBoxes.IndexOf(tileBox); // Get corresponding index

					if (index != tileBoxes.Count - 1) { // When not last item in tileboxes
						tileBoxes[index].Left = (index + 1) * (tileDimensions + TILE_OFFSET);
						tileBoxes[index + 1].Left = (index) * (tileDimensions + TILE_OFFSET);

						GV.CollectionManipulator.SwapListValues(sequence.Frames, index, index + 1);
						GV.CollectionManipulator.SwapListValues(tileBoxes, index,		index + 1);
					}
				}
			}
			#endregion

			private TileBox GetTileBoxFromToolStripItem(ToolStripItem strip) {
				if (strip != null && strip.Owner as ContextMenuStrip != null) {
					return (strip.Owner as ContextMenuStrip).SourceControl as TileBox;
				}

				return null; // Either strip or strip parent = null so skip
			}

			public void AppendFrame(Frame newFrame) {
				sequence.Frames.Add(newFrame); // Add Frame To Animation Sequence Instance

				int offset = (sequence.Frames.Count - 1) * (tileDimensions + TILE_OFFSET);
				TileBox tb = GetTileBox(newFrame, offset, GetContextMenuStrip());
				tb.AssignTexture(tileBoxes.First().Texture); // HAS TO EXIST BY DEFAULT

				tileBoxes.Add(tb); // Add new tile box to stored tileboxes
				Controls.Add(tb); // Display tile box on control / panel
			}

			public void AssignTexture(Image image) {
				foreach (TileBox box in Controls) {
					box.AssignTexture(image);
				}
			}

			private const int TILE_OFFSET = 5;
			private AnimationSequence sequence;
			private int tileDimensions;
			private List<TileBox> tileBoxes = new List<TileBox>();

			public int Count { get { return tileBoxes.Count; } }
		}

		public AnimationViewDataStore(String sequenceKey) : base() {
			Size = GetSize(); // Store size of panel
			Left = HORIZONTAL_OFFSET; // Add offset
			BorderStyle = BorderStyle.FixedSingle;
		
			BuildControls(sequenceKey);
			SequenceKey = sequenceKey;

			addFrameButton.Click += (s, e) => {
				container.AppendFrame(new Frame(0, 0, 32, 32));
				container.ModifyFrame_Call(container.Count - 1);
			};
		}

		public void BuildControls(string sequenceKey) {
			PathDetailsTextBox = new TextBox() { Width = 400, Height = CONTROLS_HEIGHT, ReadOnly = true };

			int buttonWidth = ((GetSize().Width - PathDetailsTextBox.Width) / 3) - BUTTON_HORIZONTAL_OFFSET; //- 1;

			addFrameButton		 = YieldButton("Add Frame",         PathDetailsTextBox.Right + BUTTON_HORIZONTAL_OFFSET, buttonWidth);
			renameAnimButton     = YieldButton("Rename Animation",  addFrameButton.Right     + BUTTON_HORIZONTAL_OFFSET, buttonWidth);
			deleteSequenceButton = YieldButton("Delete Sequence",   renameAnimButton.Right   + BUTTON_HORIZONTAL_OFFSET, buttonWidth+1);

			AnimationSequence sequence = GV.MonoGameImplement.importedAnimations[sequenceKey]; // Get animation sequence
			container = new FramesContainer(sequence, Height - (CONTROLS_HEIGHT + 4)) { Width = Width, Top = CONTROLS_HEIGHT + 4 };

			Controls.AddRange(new Control[] { PathDetailsTextBox, addFrameButton, deleteSequenceButton, renameAnimButton, container });
		}

		public void AssignTexture(Image image) {
			(Controls[Controls.Count - 1] as FramesContainer).AssignTexture(image);
		}

		private Button YieldButton(String text, int left, int width) {
			return new Button() {
				Height = CONTROLS_HEIGHT,
				BackColor = Color.FromArgb(255, 240, 240, 240),
				Width = width, Left = left, Text = text, Top = -1,
			};
		}

		public void RenameSequenceKey(String newKey) {
			SequenceKey = newKey;
		}

		public static Size GetSize() {
			return new Size(CONTAINER_WIDTH - SysInfo.VerticalScrollBarWidth - (2 * HORIZONTAL_OFFSET), CONTAINER_HEIGHT);
		}

		TextBox PathDetailsTextBox;
		FramesContainer container;
		public Button deleteSequenceButton, addFrameButton, renameAnimButton;

		public String SequenceKey { get { return PathDetailsTextBox.Text; } private set { PathDetailsTextBox.Text = value; } }

		public const int CONTAINER_WIDTH   = 1080, CONTAINER_HEIGHT = 150;
		public const int HORIZONTAL_OFFSET = 35, BUTTON_HORIZONTAL_OFFSET = 15;
		public const int CONTROLS_HEIGHT   = 26;
	}
}
