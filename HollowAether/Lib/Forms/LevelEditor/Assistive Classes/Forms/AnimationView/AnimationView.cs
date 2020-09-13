using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
using Microsoft.Xna.Framework;

using HollowAether.Lib.GAssets;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class AnimationView : Form {
		class StringComparer : IComparer<String> {
			public int Compare(String a, String b) {
				return String.Compare(a, b, false);
			}

			public static StringComparer comparer = new StringComparer();
		}

		public AnimationView() {
			InitializeComponent();

			animationTreeView.BeforeExpand += animationTreeView_BeforeExpand;
			animationTreeView.MouseDown	   += animationTreeView_MouseDown;

			textureDisplayComboBox.MouseWheel += (s, e) => {
				((HandledMouseEventArgs)e).Handled = true;
			};
		}

		private void AnimationView_Load(object sender, EventArgs e) {
			animationTreeView.Nodes.Add("Root");

			textureDisplayComboBox.Items.AddRange(GlobalVars.LevelEditor.textures.Keys.ToArray());
			textureDisplayComboBox.SelectedIndex = 0; // Select first item referenced by combobox

			foreach (String key in GlobalVars.MonoGameImplement.importedAnimations.Keys) {
				String[] splitKey = key.Split('\\'); AddNodeToTreeView(splitKey[0], splitKey[1]);
				AnimationViewDataStore animationStore = new AnimationViewDataStore(key);
				animationStore.deleteSequenceButton.Click += DeleteSequenceButton_ClickEventHandler;
				animationStore.renameAnimButton.Click     += RenameSequenceButton_ClickEventHandler;

				dataStore.Add(key, animationStore); 
			}
		}

		private void animationTreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e) {
			if (e.Node.IsSelected) { // Do Nothing When Not Selected
				AnimationGroupPanel.Controls.Clear(); // Clear All Existing Controls
				Image texture = GetImageInComboBox();

				if (e.Node.Text == "Root") {
					TreeNode root = animationTreeView.Nodes[0]; // Root

					foreach (TreeNode node in BreadthFirstTraverseRoot(root))
						AddGroundNodeToDataViewStore(node, texture);
				} else if (e.Node.Nodes.Count > 0) {
					foreach (TreeNode node in e.Node.Nodes) 
						AddGroundNodeToDataViewStore(node, texture);
				} else AddGroundNodeToDataViewStore(e.Node, texture);
			}
		}

		private void AddGroundNodeToDataViewStore(TreeNode node, Image currentImage = null) {
			currentImage = currentImage == null ? GetImageInComboBox() : currentImage;

			AnimationViewDataStore store = dataStore[$"{node.Parent.Text}\\{node.Text}"];
			store.AssignTexture(currentImage); // Assign Current Texture To Data Store
			store.Top = GetDataStoreVerticalPosition(); // Set Upper Offset For Store

			AnimationGroupPanel.Controls.Add(store); // Store Control To Animation Group
		}

		private void textureDisplayComboBox_TextChanged(object sender, EventArgs e) {
			Image texture = GetImageInComboBox(); // Image Pointed To
			
			foreach (AnimationViewDataStore store in AnimationGroupPanel.Controls) {
				store.AssignTexture(texture);
			}
		}

		private IEnumerable<TreeNode> BreadthFirstTraverseRoot(TreeNode node) {
			if (node != null) {
				Queue<TreeNode> queue = new Queue<TreeNode>();
				queue.Enqueue(node); // Add root to queue as 1st
				TreeNode currentNode = node; // Current Node

				while (queue.Count != 0) {
					currentNode = queue.Dequeue(); // Get And Remove 1st Item

					if (currentNode.Nodes.Count == 0) yield return currentNode; else {
					// If Node Is At Tree Bedrock, I.E. It Has No Child Nodes, Return To Caller, Else
						foreach (TreeNode child in currentNode.Nodes) queue.Enqueue(child); // Store
					}
				}
			}
		}

		private void AddNodeToTreeView(String file, String name) {
			TreeNode rootNode = animationTreeView.Nodes[0]; // Get root

			if (!rootNode.IsExpanded) rootNode.Expand(); // Expand children

			int index = BinaryGetIndexOfDesiredNodeWithText(rootNode, file);

			if (index == -1)  // Not in node view, Add to node view
				index = AddToNodeView(rootNode, file); // Store file

			AddToNodeView(rootNode.Nodes[index], name);
		}

		private int BinaryGetIndexOfDesiredNodeWithText(TreeNode rootNode, string text) {
			if (rootNode.Nodes.Count == 0) return -1; else { // No Child
				int start = 0, end = rootNode.Nodes.Count - 1; // Doesn't Start At 0

				while (start <= end) {
					int index = (int)Math.Ceiling((float)(start + end)/2); // Test Index
					int compare = String.Compare(text, rootNode.Nodes[index].Text, true);
		
					if (compare == 0) return index; else {
						if (compare < 0) end   = index - 1;
						else			 start = index + 1;
					}
				}

				return -1; // When desired value was not found
			}
		}

		private int AddToNodeView(TreeNode node, string value) {
			int nodeCount = node.Nodes.Count; // Store collection count, because accessed often

			if (node.Nodes.Count == 0 || String.Compare(value, node.Nodes[nodeCount - 1].Text) >= 0) {
				// Either No Items So Add Or New Item Greater Then Final Item
				node.Nodes.Add(value); // Add node to end of root node
				return nodeCount; // Corresponds to newly added index
			} else if (String.Compare(value, node.Nodes[0].Text) <= 0) {
				node.Nodes.Insert(0, value); // Insert at the beginning if
				return 0; // Inserted at the beginning of the node collection
			} else {
				// Otherwise, search for the place to insert.
				String[] titles = (from X in YieldChildNodes(node) select X.Text).ToArray();
				int index		= Array.BinarySearch(titles, value, StringComparer.comparer);

				if (index < 0) { index = ~index; } // A -ve Num = Bitwise Complement Of Index > (item || count) 

				node.Nodes.Insert(index, value); // Add new node to desired index
				return index; // Return calculated index for newly inserted node
			}
		}

		private void DeleteSequenceButton_ClickEventHandler(object sender, EventArgs e) {
			DialogResult result = MessageBox.Show(
				"Are You Sure You Want To Delete This Animation Sequence", "Really?",
				MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation
			);

			if (result == DialogResult.OK) {
				AnimationViewDataStore store = (sender as Button).Parent as AnimationViewDataStore;
				int index = AnimationGroupPanel.Controls.IndexOf(store); // Index of deleting anim

				AnimationGroupPanel.Controls.RemoveAt(index); // Remove desired data store item
				
				for (int X = index; X < AnimationGroupPanel.Controls.Count; X++) {
					AnimationGroupPanel.Controls[X].Top = GetDataStoreVerticalPosition(X);
				}

				string key = GlobalVars.CollectionManipulator.DictionaryGetKeyFromValue(dataStore, store);
				dataStore.Remove(key);        GlobalVars.MonoGameImplement.importedAnimations.Remove(key);

				#region RemoveFromTreeNodeView
				DeleteSequenceFromTreeNodeView(key);

				/*int nodeIndex = BinaryGetIndexOfDesiredNodeWithText(animationTreeView.Nodes[0], key.Split('\\').First());

				TreeNode node = animationTreeView.Nodes[0].Nodes[nodeIndex];

				if (node.Nodes.Count == 1) node.Remove(); else {
					node.Nodes.RemoveAt(BinaryGetIndexOfDesiredNodeWithText(node, key.Split('\\').Last()));
				}*/
				#endregion
			}
		}

		private void RenameSequenceButton_ClickEventHandler(object sender, EventArgs e) {
			AnimationViewDataStore store = (sender as Button).Parent as AnimationViewDataStore;
			
			Tuple<DialogResult, string, string> result = RenameAnimationSequenceDialog.Run(store.SequenceKey);

			if (result.Item1 == DialogResult.OK) { // Dialog Succeeded
				if (dataStore.ContainsKey(result.Item3.ToLower())) {
					MessageBox.Show(
						$"An Animation With The Name \"{result.Item3}\" Already Exists",
						"Error", MessageBoxButtons.OK, MessageBoxIcon.Error
					);
				} else {
					string[] splitAnimName = result.Item3.Split('\\');
					store.RenameSequenceKey(result.Item3); // Set key

					DeleteSequenceFromTreeNodeView(result.Item2);
					AddNodeToTreeView(splitAnimName[0], splitAnimName[1]);

					#region RenameSequenceReferences
					dataStore.Remove(result.Item2);
					dataStore.Add(result.Item3, store);

					var sequence = GV.MonoGameImplement.importedAnimations[result.Item2];
					GV.MonoGameImplement.importedAnimations.Remove(result.Item2); // Del
					GV.MonoGameImplement.importedAnimations.Add(result.Item3, sequence);
					#endregion
				}
			}
		}

		private void newAnimationButton_Click(object sender, EventArgs e) {
			string value; int counter = 0;

			do {
				value = (counter++ == 0) ? $"NewAnimation\\NewAnimation" : $"NewAnimation\\NewAnimation_{counter}";
				value = value.ToLower(); // Blanket Make Return Value All Lower Case
			} while (GlobalVars.MonoGameImplement.importedAnimations.ContainsKey(value));

			AnimationSequence seq = new AnimationSequence(0, new Frame(0, 0, 32, 32));
			GlobalVars.MonoGameImplement.importedAnimations.Add(value, seq);

			String[] splitKey = value.Split('\\'); AddNodeToTreeView(splitKey[0], splitKey[1]);
			AnimationViewDataStore animationStore = new AnimationViewDataStore(value);
			animationStore.deleteSequenceButton.Click += DeleteSequenceButton_ClickEventHandler;
			animationStore.renameAnimButton.Click     += RenameSequenceButton_ClickEventHandler;

			dataStore.Add(value, animationStore); // Store new animation store instance

			Image currentImage =  GetImageInComboBox();

			animationStore.AssignTexture(currentImage); // Assign Current Texture To Data Store
			animationStore.Top = GetDataStoreVerticalPosition(); // Set Upper Offset For Store

			AnimationGroupPanel.Controls.Add(animationStore); // Store Control To Animation Group
		}

		private void DeleteSequenceFromTreeNodeView(string key) {
			int nodeIndex = BinaryGetIndexOfDesiredNodeWithText(animationTreeView.Nodes[0], key.Split('\\').First());

			TreeNode node = animationTreeView.Nodes[0].Nodes[nodeIndex];

			if (node.Nodes.Count == 1) node.Remove(); else {
				node.Nodes.RemoveAt(BinaryGetIndexOfDesiredNodeWithText(node, key.Split('\\').Last()));
			}
		}

		private static IEnumerable<TreeNode> YieldChildNodes(TreeNode root) {
			foreach (TreeNode node in root.Nodes) { yield return node; }
		}

		private Image GetImageInComboBox() {
			int selectedTextureIndex = textureDisplayComboBox.SelectedIndex;
			string path = textureDisplayComboBox.Items[selectedTextureIndex].ToString();
			return GlobalVars.LevelEditor.textures[path].Item1; // Get Corresponding texture
		}

		private static int GetDataStoreVerticalPosition(int Count) {
			return INITIAL_VERTICAL_OFFSET + (Count * (AnimationViewDataStore.CONTAINER_HEIGHT + SEQUENTIAL_VERTICAL_OFFSET));
		}

		private int GetDataStoreVerticalPosition() { return GetDataStoreVerticalPosition(AnimationGroupPanel.Controls.Count); }

		#region Expansion Prevention
		private void animationTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
			e.Cancel = cancelNodeExpansion;
			cancelNodeExpansion = false;
		}

		private void animationTreeView_MouseDown(object sender, MouseEventArgs e) {
			int ellapsedMilleSecs = (int)DateTime.Now.Subtract(mouseDownTimePoint).TotalMilliseconds;
			cancelNodeExpansion	  = (ellapsedMilleSecs < SystemInformation.DoubleClickTime);
			mouseDownTimePoint	  = DateTime.Now; // Reset Time Since Last Time Down Check
		}

		private bool cancelNodeExpansion = false;

		private DateTime mouseDownTimePoint = DateTime.Now;
		#endregion

		public const int INITIAL_VERTICAL_OFFSET = 15, SEQUENTIAL_VERTICAL_OFFSET = 15;

		private void AnimationView_FormClosing(object sender, FormClosingEventArgs e) {
			String path = GlobalVars.FileIO.assetPaths["Animations"];

			foreach (string subPath in IOMan.GetFiles(path)) {
				try   { System.IO.File.Delete(subPath);                       } 
				catch { Console.WriteLine($"Couldn't Delete Anim {subPath}"); }
			}

			String[] keys = GlobalVars.MonoGameImplement.importedAnimations.Keys.ToArray();
			Array.Sort(keys); // Sort all enteries within the dictionary appropriately

			string lastKnownFileName = null; Animation animation = new Animation(Vector2.Zero);

			foreach (string key in keys) {
				string currentFileName = key.Split('\\').First();
				string sequenceKey     = key.Split('\\').Last();

				if (currentFileName != lastKnownFileName) {
					if (!string.IsNullOrEmpty(lastKnownFileName)) {
						string container = IOMan.Join(path, lastKnownFileName + ".ANI"); // File Path For Animation
						IOMan.WriteEncryptedFile(container, animation.ToAnimationFile(), GV.Encryption.oneTimePad);
					}

					lastKnownFileName = currentFileName;
					animation = new Animation(Vector2.Zero);
				}

				var sequence = GlobalVars.MonoGameImplement.importedAnimations[key];
				animation.AddAnimationSequence(sequenceKey, sequence); // Add To Anim
			}

			if (!string.IsNullOrEmpty(lastKnownFileName)) {
				string container = IOMan.Join(path, lastKnownFileName + ".ANI"); // File Path For Animation
				IOMan.WriteEncryptedFile(container, animation.ToAnimationFile(), GV.Encryption.oneTimePad);
			}
		}

		private Dictionary<String, AnimationViewDataStore> dataStore = new Dictionary<String, AnimationViewDataStore>();
	}
}
