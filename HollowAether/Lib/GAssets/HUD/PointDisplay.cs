using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HollowAether.Lib.Exceptions;
using GV = HollowAether.Lib.GlobalVars;

namespace HollowAether.Lib.GAssets.HUD {
	class PointDisplay : IPushable {
		static PointDisplay() {
			displayBase        = GV.TextureCreation.GenerateBlankTexture(063, 033, 000);
			displayBaseOverlay = GV.TextureCreation.GenerateBlankTexture(255, 255, 255);
			barDarkOverlay     = GV.TextureCreation.GenerateBlankTexture(255, 211, 090);
			barBrightOverlay   = GV.TextureCreation.GenerateBlankTexture(255, 248, 073);
			maxCornerYellow    = GV.TextureCreation.GenerateBlankTexture(255, 237, 066);
			maxCenterRed       = GV.TextureCreation.GenerateBlankTexture(255, 076, 025);
			timeLoadingDark    = GV.TextureCreation.GenerateBlankTexture(073, 073, 255);
			timeLoadingBright  = GV.TextureCreation.GenerateBlankTexture(073, 215, 255);
		}

		public PointDisplay(Vector2 position) {
			Position = position;
			BuildBarRects();
		}

		public void PushTo(Vector2 position, float over = 0.8f) {
			if (PushArgs.PushValid(position, Position))
				Push(new PushArgs(position, Position, over));
		}

		public void Push(PushArgs args) { if (!BeingPushed) PushPack = args; }

		public void Update(bool updateAnimation) {
			float elapsedTime = GV.MonoGameImplement.gameTime.ElapsedGameTime.Milliseconds * (float)Math.Pow(10, -3);

			if (BeingPushed && !PushPack.Update(this, elapsedTime)) {
				PushPack = null; // Delete push pack
			}
		}


		public void Draw() {
			DrawRect(displayBase, baseUpperRect); DrawRect(displayBase, baseLowerRect); // Draw base

			DrawRect(displayBaseOverlay, baseOverlayRectA); DrawRect(displayBaseOverlay, baseOverlayRectB); // Draw overlay

			float burstPointPercentage = GV.MonoGameImplement.Player.burstPoints / (float)Player.maxBurstPoints;
			float tNM = GV.MonoGameImplement.Player.timeNotMoving; // Store locally to shorten access length, etc.
			float timeoutPercentage = tNM / Player.TIME_BEFORE_POINT_GEN;

			if (burstPointPercentage == 1) DrawMaxIndicator(); // if 100% bar complete, draw indicators
			else if (tNM > 0 && tNM < Player.TIME_BEFORE_POINT_GEN) {
				// If less than 100% burst points, draw timer behind regular display bar
				DrawPercentageBar(timeoutPercentage, timeLoadingBright, timeLoadingDark);
			}

			DrawPercentageBar(burstPointPercentage, barDarkOverlay, barBrightOverlay);
		}

		private void DrawPercentageBar(float percentage, Texture2D textureDark, Texture2D textureBright) {
			percentage = GV.BasicMath.Clamp<float>(percentage, 0, 1); // Prevent > one
			int width = (int)(percentage * variedBarBase.Width); // Calculate partial width

			Rectangle A = variedBarBase, B = variedBarOverlay; // Creates shallow clones
			A.Width = width; B.Width = width; // Change doesn't affect original reference

			DrawRect(textureDark, A); DrawRect(textureBright, B); // Draws bar textures
		}

		private void DrawMaxIndicator() {
			DrawRect(maxCenterRed, maxCenterRectUpper);
			DrawRect(maxCenterRed, maxCenterRectLower);
			DrawRect(maxCornerYellow, maxCornerRectTL);
			DrawRect(maxCornerYellow, maxCornerRectTR);
			DrawRect(maxCornerYellow, maxCornerRectBL);
			DrawRect(maxCornerYellow, maxCornerRectBR);
			DrawRect(maxCornerYellow, maxCornerRectCTL);
			DrawRect(maxCornerYellow, maxCornerRectCTR);
			DrawRect(maxCornerYellow, maxCornerRectCBL);
			DrawRect(maxCornerYellow, maxCornerRectCBR);
		}

		private void DrawRect(Texture2D texture, Rectangle rect) {
			GV.MonoGameImplement.SpriteBatch.Draw(texture, position + rect.Location.ToVector2(), rect, Color.White);
		}

		private void BuildBarRects() {
			Point position = Point.Zero; // Start with initial point of nothing

			int BOD  = BASE_OVERLAY_DIMENSIONS, VBHO = VARIED_BAR_HEIGHT_OFFSET, VBOH = VARIED_BAR_OVERLAY_HEIGHT;
			int MCRW = MAX_CENTER_RECT_WIDTH;

			baseUpperRect = new Rectangle(position.X,       position.Y,           WIDTH,       HEIGHT);
			baseLowerRect = new Rectangle(position.X + BOD, baseUpperRect.Bottom, WIDTH - BOD,    BOD);

			baseOverlayRectA = new Rectangle(position.X, position.Y,                WIDTH - BOD, BOD);
			baseOverlayRectB = new Rectangle(position.X, position.Y + HEIGHT - BOD, WIDTH - BOD, BOD);


			Point variedBarSize = new Point(baseUpperRect.Width - BOD, baseUpperRect.Height - (2 * BOD) - VBHO);
			variedBarBase = new Rectangle(baseUpperRect.Location + new Point(0, BOD), variedBarSize);

			Point variedBarOverlaySize     = new Point(variedBarBase.Width,                                VBOH);
			Point variedBarOverlayPosition = new Point(variedBarBase.X,     variedBarBase.Center.Y - (VBOH / 2));
			
			variedBarOverlay = new Rectangle(variedBarOverlayPosition, variedBarOverlaySize);


			Point topLeftPosition    = new Point(baseOverlayRectA.X, baseOverlayRectA.Y);
			Point bottomLeftPosition = topLeftPosition + new Point(0, baseUpperRect.Height-BOD);

			maxCornerRectTL = new Rectangle(topLeftPosition,                                               new Point(BOD));
			maxCornerRectTR = new Rectangle(topLeftPosition + new Point(baseOverlayRectA.Width-BOD, 0),    new Point(BOD));
			maxCornerRectBL = new Rectangle(bottomLeftPosition,                                            new Point(BOD));
			maxCornerRectBR = new Rectangle(bottomLeftPosition + new Point(baseOverlayRectA.Width-BOD, 0), new Point(BOD));
	

			int maxCenterRectX = topLeftPosition.X + ((variedBarBase.Width - MCRW) / 2);

			maxCenterRectUpper = new Rectangle(new Point(maxCenterRectX, topLeftPosition.Y),    new Point(MCRW, BOD));
			maxCenterRectLower = new Rectangle(new Point(maxCenterRectX, bottomLeftPosition.Y), new Point(MCRW, BOD));

			maxCornerRectCTL = new Rectangle(maxCenterRectUpper.Location - new Point(BOD, 0),        new Point(BOD));
			maxCornerRectCTR = new Rectangle(maxCenterRectUpper.Location - new Point(BOD - MCRW, 0), new Point(BOD));
			maxCornerRectCBL = new Rectangle(maxCenterRectLower.Location - new Point(BOD, 0),        new Point(BOD));
			maxCornerRectCBR = new Rectangle(maxCenterRectLower.Location - new Point(BOD - MCRW, 0), new Point(BOD));
		}

		public void OffsetPosition(Vector2 offset) { position += offset.ToPoint().ToVector2(); }

		public void OffsetPosition(float X, float Y) { OffsetPosition(new Vector2(X, Y)); }

		Rectangle baseUpperRect, baseLowerRect, baseOverlayRectA, baseOverlayRectB, variedBarBase, variedBarOverlay;
		Rectangle maxCornerRectTL, maxCornerRectTR, maxCornerRectBL, maxCornerRectBR;
		Rectangle maxCornerRectCTL, maxCornerRectCTR, maxCornerRectCBL, maxCornerRectCBR;
		Rectangle maxCenterRectUpper, maxCenterRectLower;

		static Texture2D displayBase, displayBaseOverlay, barDarkOverlay, barBrightOverlay, maxCornerYellow, maxCenterRed, timeLoadingDark, timeLoadingBright;

		private const int BASE_OVERLAY_DIMENSIONS = 4, VARIED_BAR_HEIGHT_OFFSET = 3, VARIED_BAR_OVERLAY_HEIGHT = 12;

		private Vector2 position;

		public Vector2 Position { get { return position; } set { position = value; } }

		public PushArgs PushPack { get; private set; } = null;

		public const int WIDTH = 302, HEIGHT = 32, MAX_CENTER_RECT_WIDTH = 70;

		public bool BeingPushed { get { return PushPack != null; } }
	}
}
