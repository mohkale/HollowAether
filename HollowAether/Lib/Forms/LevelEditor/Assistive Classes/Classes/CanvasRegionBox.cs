using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.Forms.LevelEditor {
	public partial class CanvasRegionBox : PictureBox {
		#region Class & Enum Definitions
		public enum SelectType { Drag, GridClick, GridDrag, GridHighlight, Custom }

		public class SelectionRectBuildersEventArgs {
			public SelectionRectBuildersEventArgs(PointF current, SizeF gridSize) {
				CurrentPoint = current;
				GridSize     = gridSize;

				CurrentGridPoint = new PointF() {
					X = GV.BasicMath.RoundDownToNearestMultiple(current.X, gridSize.Width,  false),
					Y = GV.BasicMath.RoundDownToNearestMultiple(current.Y, gridSize.Height, false)
				};
			}

			public SelectionRectBuildersEventArgs(PointF current, SizeF gridSize, RectangleF selectionRect) : this(current, gridSize) {
				currentRectangle = selectionRect;
			}

			public PointF GetOriginalPointBasedOnUseGrid() {
				return (UseGrid) ? OriginalGridPoint : OriginalPoint;
			}

			public PointF GetCurrentPointBasedOnUseGrid() {
				return (UseGrid) ? CurrentGridPoint : CurrentPoint;
			}

			public PointF CurrentGridPoint { get; private set; }

			public PointF CurrentPoint { get; private set; }

			public PointF OriginalGridPoint { get; set;}

			public PointF OriginalPoint { get; set; }

			public SizeF GridSize { get; private set; }

			public bool UseGrid { get; set; }

			public bool MousePressed { get; set; }

			public Rectangle CanvasDimensions { get; set; }

			public RectangleF currentRectangle = RectangleF.Empty;
		};
		#endregion

		public CanvasRegionBox() {
			Paint += (s, e) => { // Set Graphic Vars For Drawing
				e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
				e.Graphics.ScaleTransform(ZoomX, ZoomY); /*Implement Zoom X & Y*/
			};

			SizeMode = PictureBoxSizeMode.AutoSize;

			#region PaintEventHandlers
			Paint += (s, e) => { DrawToCanvas(s, e); };
			Paint += DrawGrid_PaintEventHandler;
			Paint += DrawBorder_PaintEventHandler;
			Paint += DrawCursor_PaintEventHandler;
			#endregion

			#region MouseEventHandlers
			MouseDown  += MouseDown_MouseBasedEventHandler;
			MouseUp    += MouseUp_MouseBasedEventHandler;
			MouseMove  += MouseMove_MouseBasedEventHandler;
			MouseLeave += MouseLeave_MouseBasedEventHandler;
			#endregion

			#region BuildSelectionRectEventHandlers
			BuildSelectionRect += DragableSelectionRectBuildEventHandler;
			BuildSelectionRect += GridClickSelectionRectBuildEventHandler;
			BuildSelectionRect += HighlightSelectionRectBuilder;
			#endregion

			CanvasRegionExecute += CanvasRegionBox_CanvasRegionExecute;

			ClickComplete += ClickComplete_MouseBasedEventHandler;
		}

		private void CanvasRegionBox_CanvasRegionExecute(SelectType arg1, RectangleF arg2, RectangleF[] arg3) {
			highlightedSelectRects.Clear();
		}

		~CanvasRegionBox() {
			gridPen.Dispose();
			cursorPen.Dispose();
			borderPen.Dispose();
		}

		public void Draw() { Refresh(); /*Invalidates & Updates*/ }

		public void ChangeSelectType(SelectType newType) {
			if (newType != selectOption) {
				selectOption = newType;
				mousePressed = false;
				clickPoint   = gridClickPoint = PointF.Empty;
				SelectRect   = RectangleF.Empty;
				highlightedSelectRects.Clear();
			}
		}

		#region EventHandlers

		#region SelectionRectBuilderEventHandlers

		#region ActualBuilders
		public static RectangleF ClickSelectionRectBuilder(PointF point, SizeF gridSize) {
			return new RectangleF(point, gridSize); // Display Single Grid Square
		}

		public static RectangleF DragableSelectionRectBuilder(bool useGrid, SizeF gridSize, PointF newPosition, PointF originalPosition) {
			SizeF displacement = new SizeF(newPosition.X - originalPosition.X, newPosition.Y - originalPosition.Y);
			PointF point = originalPosition; // Store Original Point From Which Rectangle will eventually portrude

			if (newPosition == originalPosition) displacement = (useGrid) ? gridSize : displacement; else {
				bool forwardXRegion = displacement.Width >= 0, forwardYRegion = displacement.Height >= 0; // Regions
				displacement = new SizeF(Math.Abs(displacement.Width), Math.Abs(displacement.Height)); // Make +ve

				if (!forwardXRegion) point.X -= displacement.Width; if (!forwardYRegion) point.Y -= displacement.Height;

				if (forwardXRegion || forwardYRegion) displacement += (useGrid ? gridSize : SizeF.Empty); else if (useGrid) {
					if (!forwardXRegion) displacement.Width += gridSize.Width; if (!forwardYRegion) displacement.Height += gridSize.Height;
					// If backwards & uses grid, include clicked grid square, either on the horizontal or vertical axes of square
				}
			}

			return new RectangleF(point, displacement); // Calculate and return appropriate rectangle
		}
		#endregion

		#region BuilderCallers/ActualEventHandlers
		public static void HighlightSelectionRectBuilder(SelectType type, SelectionRectBuildersEventArgs e) {
			if (type == SelectType.GridHighlight) e.currentRectangle = new RectangleF(e.CurrentGridPoint, e.GridSize);
		}

		public void DragableSelectionRectBuildEventHandler(SelectType type, SelectionRectBuildersEventArgs e) {
			PointF currentPoint = e.GetCurrentPointBasedOnUseGrid(), originalPoint = e.GetOriginalPointBasedOnUseGrid();

			if (mousePressed && (type == SelectType.GridDrag || type == SelectType.Drag)) {
				RectangleF rect = DragableSelectionRectBuilder(e.UseGrid, e.GridSize, currentPoint, originalPoint);
				e.currentRectangle = RectangleF.Intersect(rect, e.CanvasDimensions); // Get Clamped Rectangle
			} /*else if (type == SelectType.GridDrag && e.currentRectangle == RectangleF.Empty) {
				e.currentRectangle = ClickSelectionRectBuilder(currentPoint, e.GridSize);
			}*/
		}

		public static void GridClickSelectionRectBuildEventHandler(SelectType type, SelectionRectBuildersEventArgs e) {
			if (type == SelectType.GridClick) e.currentRectangle = new RectangleF(e.CurrentGridPoint, e.GridSize);
		}
		#endregion

		#endregion

		#region MouseBasedEventHandlers

		#region AssistanceMethods

		#region ConditionVars
		private bool UseGridForSelection() { return SelectOption == SelectType.GridClick || SelectOption == SelectType.GridDrag || SelectOption == SelectType.GridHighlight; }

		private bool IsSelectTypeDragable() { return SelectOption == SelectType.Drag || SelectOption == SelectType.GridDrag; }
		#endregion

		private PointF TranslateLocationToZoomedLocation(PointF point) {
			return new PointF(point.X / ZoomX, point.Y / ZoomY);
		}
		#endregion

		#region MouseDown, MouseMove & MouseUp
		private void MouseDown_MouseBasedEventHandler(object sender, MouseEventArgs e) {
			if (AllowHighlighting && !mousePressed) {
				mousePressed = true; // Set mouse state to has been pressed

				clickPoint = TranslateLocationToZoomedLocation(e.Location);

				gridClickPoint = new PointF() {
					X = GV.BasicMath.RoundDownToNearestMultiple(clickPoint.X, gridSize.Width, false),
					Y = GV.BasicMath.RoundDownToNearestMultiple(clickPoint.Y, gridSize.Height, false)
				};

				ClickBegun(SelectOption, clickPoint, gridClickPoint, e.Button);

				SelectRect = InvokeBuildSelectionRect(clickPoint, clickPoint, gridClickPoint);
			}
		}

		private void MouseMove_MouseBasedEventHandler(object sender, MouseEventArgs e) {
			if (AllowHighlighting) {
				PointF original = (mousePressed) ? clickPoint : e.Location;

				PointF location = TranslateLocationToZoomedLocation(e.Location);

				SelectRect = InvokeBuildSelectionRect(location, original, gridClickPoint);
			}
		}

		private void MouseUp_MouseBasedEventHandler(object sender, MouseEventArgs e) {
			if (AllowHighlighting && mousePressed) {
				mousePressed = false; // Click has been completed, return to caller
				clickPoint = gridClickPoint = PointF.Empty;

				if (selectOption != SelectType.GridHighlight) {
					CanvasRegionExecute(SelectOption, SelectRect, null);
				}

				ClickComplete(selectOption, SelectRect); // Execute event handler
			}
		}
		#endregion

		private void MouseLeave_MouseBasedEventHandler(object sender, EventArgs e) {
			if (AllowHighlighting && !(/*mousePressed && */IsSelectTypeDragable())) {
				SelectRect = RectangleF.Empty; // Delete Rect When Exiting Canvas
			}
		}

		private void ClickComplete_MouseBasedEventHandler(SelectType type, RectangleF rect) {
			if (type == SelectType.GridHighlight) highlightedSelectRects.Add(rect); // If Highligted then store

			if ((dragableSelectionRectsAreVolatile && IsSelectTypeDragable())) {
				Deselect(); // Deselect selected rectangle region and any highlited rectangle regions after click
			}
		}
		#endregion

		#region DrawEventHandlers

		#region AssistantMethods
		private IEnumerable<float> GetMultiplesInRange(int span, float increment) {
			foreach (int X in Enumerable.Range(0, span)) {
				yield return increment * X; // Generate
			}
		}

		private static void DrawRectangleF(Graphics graphics, Pen pen, RectangleF rect) {
			graphics.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
		}
		#endregion

		private void DrawBorder_PaintEventHandler(object sender, PaintEventArgs e) {
			if (drawBorder) { // If draw border has been set to true, Draw the border
				e.Graphics.DrawRectangle(borderPen, new Rectangle(Point.Empty, InitialSize));
			}
		}

		private void DrawGrid_PaintEventHandler(object sender, PaintEventArgs e) {
			if (drawGrid) { // If grid is supposed to be drawn
				int horizontalSpan = (int)Math.Ceiling(initialWidth / GridWidth);
				int verticalSpan = (int)Math.Ceiling(initialHeight / GridHeight);

				#region DrawLinesEncompassingDrawRegion
				foreach (float increment in GetMultiplesInRange(horizontalSpan, GridWidth)) {
					e.Graphics.DrawLine(gridPen, increment, 0, increment, initialHeight);
				}

				foreach (float increment in GetMultiplesInRange(verticalSpan, GridHeight)) {
					e.Graphics.DrawLine(gridPen, 0, increment, initialWidth, increment);
				}
				#endregion
			}
		}

		private void DrawCursor_PaintEventHandler(object sender, PaintEventArgs e) {
			if (AllowHighlighting) {
				DrawRectangleF(e.Graphics, cursorPen, SelectRect);

				if (selectOption == SelectType.GridHighlight) {
					foreach (RectangleF rect in highlightedSelectRects)
						DrawRectangleF(e.Graphics, cursorPen, rect);
				}
			}
		}
		#endregion

		#endregion
		
		#region Set&Deselect Methods
		public void SetWidth(int width) {
			base.Width = initialWidth = width;
			base.Width = (int)Math.Ceiling(ZoomX * Width);

			Draw(); // Re invoke the draw event for the canvas
		}

		public void SetHeight(int height) {
			base.Height = initialHeight = height;
			base.Width  = (int)Math.Ceiling(ZoomX * Width);

			Draw(); // Re invoke the draw event for the canvas
		}

		public void SetZoom(SizeF size) {
			zoom = size; // New Zoom Equivalent To The Argument

			Size = new Size(
				(int)Math.Ceiling(InitialSize.Width  * size.Width),
				(int)Math.Ceiling(InitialSize.Height * size.Height)
			);

			Draw(); // Re draw Canvas With Adjusted Size 
		}

		private void SetSelectionRect(RectangleF rect) {
			if (rect != SelectRect) { selectRect = rect; Draw(); }
		}

		public void ExternalSetSelectionRect(RectangleF rect) {
			SetSelectionRect(rect);
		}

		public void Deselect() {
			highlightedSelectRects.Clear();

			selectRect   = RectangleF.Empty;
			mousePressed = false;

			Draw(); // Redraw canvas
		}
		#endregion

		#region Events And Handlers
		public RectangleF InvokeBuildSelectionRect(PointF current, PointF original, PointF gridOriginal) {
			SelectionRectBuildersEventArgs args = new SelectionRectBuildersEventArgs(current, GridSize, SelectRect) {
				MousePressed	  = mousePressed,
				OriginalPoint	  = original,
				OriginalGridPoint = gridOriginal,
				UseGrid			  = UseGridForSelection(),
				CanvasDimensions  = new Rectangle(Point.Empty, InitialSize)
			};

			BuildSelectionRect(SelectOption, args); // Execute/invoke event handler for building selection rect

			return args.currentRectangle; // Return remaining rectangle value after execution of event handlers
		}

		public void InvokeCanvasRegionExecuteForHighlitedSelectionRects() {
			if (SelectOption == SelectType.GridHighlight) {
				CanvasRegionExecute(SelectOption, RectangleF.Empty, highlightedSelectRects.ToArray());
			}
		}

		/// <summary>Extension event to allow drawing to canvas outside of class</summary>
		public event PaintEventHandler DrawToCanvas = (s, e) => { };

		/// <summary>Event called when single click/drag/highlight is completed</summary>
		public event Action<SelectType, RectangleF, RectangleF[]> CanvasRegionExecute;

		/// <summary>Delegate For Building Selection Rects During Mouse Movement Within Canvas</summary>
		/// <param name="type">The selection type that the zonecanvas is currently using</param>
		/// <param name="e">Event args including relevent canvas data for building selection rects</param>
		public delegate void SelectionRectBuilders(SelectType type, SelectionRectBuildersEventArgs e);

		/// <summary>Event to build a selection rectangle for the canvas region</summary>
		public event SelectionRectBuilders BuildSelectionRect = (t, e) => { };

		/// <summary>Event called when a single click is completed</summary>
		public event Action<SelectType, RectangleF> ClickComplete = (t, r) => { };

		/// <summary>Event called when a click is started by the program</summary>
		public event Action<SelectType, PointF, PointF, MouseButtons> ClickBegun = (t, p, gp, b) => { };
		#endregion

		#region ZoomVars
		private SizeF zoom = new SizeF(1, 1);

		public SizeF Zoom { get { return zoom; } set { SetZoom(value); } }

		public float ZoomX { get { return zoom.Width; } set { Zoom = new SizeF(value, zoom.Height); } }

		public float ZoomY { get { return zoom.Height; } set { Zoom = new SizeF(zoom.Width, value); } }
		#endregion

		#region Pen&ColorVars
		private Pen gridPen = new Pen(Color.Gray, 0.25f);
		private Pen cursorPen = new Pen(Color.HotPink, 1.00f);
		private Pen borderPen = new Pen(Color.HotPink, 2.00f);

		public Color GridColor { get { return gridPen.Color; } set { gridPen.Color = value; } }

		public Color CursorColor { get { return cursorPen.Color; } set { cursorPen.Color = value; } }

		public Color BorderColor { get { return borderPen.Color; } set { borderPen.Color = value; } }
		#endregion

		#region SizeVars
		private int initialWidth, initialHeight;

		public Size InitialSize { get { return new Size(initialWidth, initialHeight); } }

		public new int Width { get { return base.Width; } set { SetWidth(value); } }

		public new int Height { get { return base.Height; } set { SetHeight(value); } }
		#endregion

		#region GridVars
		private bool drawGrid = true;

		private SizeF gridSize = new Size(32, 32);

		public bool DrawGrid { get { return drawGrid; } set { drawGrid = value; Draw(); } }

		public float GridWidth { get { return gridSize.Width; } set { GridSize = new SizeF(value, gridSize.Height); } }

		public float GridHeight { get { return gridSize.Height; } set { GridSize = new SizeF(gridSize.Width, value); } }

		public SizeF GridSize { get { return gridSize; } set { gridSize = value; Draw(); } }
		#endregion

		#region BorderVars
		private bool drawBorder = true;

		public bool DrawBorder { get { return drawBorder; } set { drawBorder = value; Draw(); } }
		#endregion

		#region Click&SelectVars
		private SelectType selectOption = SelectType.GridHighlight;

		private PointF clickPoint { get; set; } //= PointF.Empty;
		private PointF gridClickPoint = PointF.Empty;

		private bool allowHighlighting = true;

		private bool mousePressed = false;

		public bool dragableSelectionRectsAreVolatile = false;

		private RectangleF selectRect;

		private HashSet<RectangleF> highlightedSelectRects = new HashSet<RectangleF>();

		public bool AllowHighlighting { get { return allowHighlighting; } set { allowHighlighting = value; Draw(); } }

		public SelectType SelectOption { get { return selectOption; } set { ChangeSelectType(value); } }

		public RectangleF SelectRect { get { return selectRect; } private set { SetSelectionRect(value); } }
		#endregion
	}
}
