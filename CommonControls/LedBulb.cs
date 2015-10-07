using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Modbus.Common {

	/// <summary>
	/// The LEDBulb is a .Net control for Windows Forms that emulates an
	/// LED light with two states On and Off.  The purpose of the control is to 
	/// provide a sleek looking representation of an LED light that is sizable, 
	/// has a transparent background and can be set to different colors.  
	/// </summary>
	public class LedBulb : Control {

		#region Public and Private Members

		private Color _color;
		private bool _on = true;

		/// <summary>
		/// Gets or Sets the color of the LED light
		/// </summary>
		[DefaultValue(typeof(Color), "153, 255, 54")]
		public Color Color { 
			get { return _color; } 
			set { 
				_color = value;
				this.DarkColor = ControlPaint.Dark(_color);
				this.DarkDarkColor = ControlPaint.DarkDark(_color);
				this.Invalidate();	// Redraw the control
			} 
		}
		
		/// <summary>
		/// Dark shade of the LED color used for gradient
		/// </summary>
		public Color DarkColor { get; protected set; }
		
		/// <summary>
		/// Very dark shade of the LED color used for gradient
		/// </summary>
		public Color DarkDarkColor { get; protected set; }

		/// <summary>
		/// Gets or Sets whether the light is turned on
		/// </summary>
		public bool On { get { return _on; } set { _on = value; this.Invalidate(); } }

		#endregion

		#region Constructor
		
		public LedBulb() {
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			this.Color = Color.FromArgb(255, 153, 255, 54);
		}
		
		#endregion

		#region Transpanency Methods
		
		protected override CreateParams CreateParams {
			get {
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x20;
				return cp;
			}
		}
		protected override void OnMove(EventArgs e) {
			RecreateHandle();
		}
		protected override void OnPaintBackground(PaintEventArgs e) {

		}
		
		#endregion

		#region Drawing Methods

		/// <summary>
		/// Handles the Paint event for this UserControl
		/// </summary>
		protected override void OnPaint(PaintEventArgs e){
			// Create an offscreen graphics object for double buffering
			Bitmap offScreenBmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
			using (System.Drawing.Graphics g = Graphics.FromImage(offScreenBmp)) {
				g.SmoothingMode = SmoothingMode.HighQuality;
				// Draw the control
				drawControl(g);
				// Draw the image to the screen
				e.Graphics.DrawImageUnscaled(offScreenBmp, 0, 0);
			}
		}

		/// <summary>
		/// Renders the control to an image
		/// </summary>
		/// <param name="g"></param>
		private void drawControl(Graphics g) {
			Color lightColor = (this.On)? this.Color : this.DarkColor;
			Color darkColor = (this.On) ? this.DarkColor : this.DarkDarkColor;
			
			Rectangle paddedRectangle = new Rectangle(this.Padding.Left, this.Padding.Top, this.Width - (this.Padding.Left + this.Padding.Right), this.Height - (this.Padding.Top + this.Padding.Bottom));
			int width = (paddedRectangle.Width < paddedRectangle.Height) ? paddedRectangle.Width : paddedRectangle.Height;
			Rectangle drawRectangle = new Rectangle(paddedRectangle.X, paddedRectangle.Y, width, width);

			// Draw the background ellipse
			if (drawRectangle.Width < 1) drawRectangle.Width = 1;
			if (drawRectangle.Height < 1) drawRectangle.Height = 1;
			g.FillEllipse(new SolidBrush(darkColor), drawRectangle);

			// Draw the glow gradient
			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(drawRectangle);
			PathGradientBrush pathBrush = new PathGradientBrush(path);
			pathBrush.CenterColor = lightColor;
			pathBrush.SurroundColors = new Color[] { Color.FromArgb(0, lightColor) };
			g.FillEllipse(pathBrush, drawRectangle);

			// Set the clip boundary  to the edge of the ellipse
			GraphicsPath gp = new GraphicsPath();
			gp.AddEllipse(drawRectangle);
			g.SetClip(gp);

			// Draw the white reflection gradient
			GraphicsPath path1 = new GraphicsPath();
			Rectangle whiteRect = new Rectangle(drawRectangle.X - Convert.ToInt32(drawRectangle.Width * .15F), drawRectangle.Y - Convert.ToInt32(drawRectangle.Width * .15F), Convert.ToInt32(drawRectangle.Width*.8F), Convert.ToInt32(drawRectangle.Height*.8F));
			path1.AddEllipse(whiteRect);
			PathGradientBrush pathBrush1 = new PathGradientBrush(path);
			pathBrush1.CenterColor = Color.FromArgb(180, 255, 255, 255);
			pathBrush1.SurroundColors = new Color[] { Color.FromArgb(0, 255, 255, 255) };
			g.FillEllipse(pathBrush1, whiteRect);

			// Draw the border
			float w = drawRectangle.Width;
			g.SetClip(this.ClientRectangle);
			if (this.On) g.DrawEllipse(new Pen(Color.FromArgb(85, Color.Black),1F), drawRectangle);
		}

		#endregion
	}
}
