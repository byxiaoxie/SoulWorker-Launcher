using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Launcher
{
    public partial class ProgressBarEx : Control
    {
		public ProgressBarEx()
		{
			this.bBlend = new Blend();
			this._Minimum = 0;
			this._Maximum = 100;
			this._Value = 0;
			this._Border = true;
			this._BorderColor = Color.Black;
			this._GradiantColor = Color.White;
			this._BackColor = Color.DarkGray;
			this._ProgressColor = Color.Lime;
			this._ShowPercentage = false;
			this._ShowText = false;
			this._ImageLayout = ProgressBarEx.ImageLayoutType.None;
			this._Image = null;
			this._RoundedCorners = true;
			this._ProgressDirection = ProgressBarEx.ProgressDir.Horizontal;
			base.SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
			base.TabStop = false;
			base.Size = new Size(200, 23);
			this.bBlend.Positions = new float[]
			{
				0f,
				0.2f,
				0.4f,
				0.6f,
				0.8f,
				1f
			};
			this.GradiantPosition = ProgressBarEx.GradiantArea.Top;
			base.BackColor = Color.Transparent;
			this._ForeColorBrush = new SolidBrush(base.ForeColor);
			this._BorderPen = new Pen(Color.Black);
		}

		[Category("Appearance")]
		[Description("The foreground color of the ProgressBars text.")]
		[Browsable(true)]
		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				if (value == Color.Transparent)
				{
					value = this._ForeColorBrush.Color;
				}
				base.ForeColor = value;
				this._ForeColorBrush.Color = value;
			}
		}

		[Category("Appearance")]
		[Description("The background color of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(typeof(Color), "DarkGray")]
		public Color BackgroundColor
		{
			get
			{
				return this._BackColor;
			}
			set
			{
				if (value == Color.Transparent)
				{
					value = this._BackColor;
				}
				this._BackColor = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("The progress color of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Lime")]
		public Color ProgressColor
		{
			get
			{
				return this._ProgressColor;
			}
			set
			{
				if (value == Color.Transparent)
				{
					value = this._ProgressColor;
				}
				this._ProgressColor = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("The gradiant highlight color of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(typeof(Color), "White")]
		public Color GradiantColor
		{
			get
			{
				return this._GradiantColor;
			}
			set
			{
				this._GradiantColor = value;
				this.Refresh();
			}
		}

		[Category("Behavior")]
		[Description("The minimum value of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(0)]
		public int Minimum
		{
			get
			{
				return this._Minimum;
			}
			set
			{
				if (value > this._Maximum)
				{
					value = checked(this._Maximum - 1);
				}
				this._Minimum = value;
				this.Refresh();
			}
		}

		[Category("Behavior")]
		[Description("The maximum value of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(100)]
		public int Maximum
		{
			get
			{
				return this._Maximum;
			}
			set
			{
				if (value <= this._Minimum)
				{
					value = checked(this._Minimum + 1);
				}
				this._Maximum = value;
				this.Refresh();
			}
		}

		[Category("Behavior")]
		[Description("The current value of the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(0)]
		public int Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				if (value < this._Minimum)
				{
					value = this._Minimum;
				}
				if (value > this._Maximum)
				{
					value = this._Maximum;
				}
				this._Value = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Draw a border around the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool Border
		{
			get
			{
				return this._Border;
			}
			set
			{
				this._Border = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("The color of the border around the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(typeof(Color), "Black")]
		public Color BorderColor
		{
			get
			{
				return this._BorderColor;
			}
			set
			{
				if (value == Color.Transparent)
				{
					value = this._BorderColor;
				}
				this._BorderColor = value;
				this._BorderPen.Color = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Shows the progress percentge as text in the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(false)]
		public bool ShowPercentage
		{
			get
			{
				return this._ShowPercentage;
			}
			set
			{
				this._ShowPercentage = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Shows the text of the Text property in the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(false)]
		public bool ShowText
		{
			get
			{
				return this._ShowText;
			}
			set
			{
				this._ShowText = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Determins the position of the gradiant shine in the ProgressBar.")]
		[Browsable(true)]
		[DefaultValue(typeof(ProgressBarEx.GradiantArea), "Top")]
		public ProgressBarEx.GradiantArea GradiantPosition
		{
			get
			{
				return this._GradiantPosition;
			}
			set
			{
				this._GradiantPosition = value;
				if (value == ProgressBarEx.GradiantArea.None)
				{
					this.bBlend.Factors = new float[6];
				}
				else if (value == ProgressBarEx.GradiantArea.Top)
				{
					this.bBlend.Factors = new float[]
					{
						0.8f,
						0.7f,
						0.6f,
						0.4f,
						0f,
						0f
					};
				}
				else if (value == ProgressBarEx.GradiantArea.Center)
				{
					this.bBlend.Factors = new float[]
					{
						0f,
						0.4f,
						0.6f,
						0.6f,
						0.4f,
						0f
					};
				}
				else
				{
					this.bBlend.Factors = new float[]
					{
						0f,
						0f,
						0.4f,
						0.6f,
						0.7f,
						0.8f
					};
				}
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("An image to display on the ProgressBarEx.")]
		[Browsable(true)]
		public Bitmap Image
		{
			get
			{
				return this._Image;
			}
			set
			{
				this._Image = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Determins how the image is displayed in the ProgressBarEx.")]
		[Browsable(true)]
		[DefaultValue(typeof(ProgressBarEx.ImageLayoutType), "None")]
		public ProgressBarEx.ImageLayoutType ImageLayout
		{
			get
			{
				return this._ImageLayout;
			}
			set
			{
				this._ImageLayout = value;
				if (this._Image != null)
				{
					this.Refresh();
				}
			}
		}

		[Category("Appearance")]
		[Description("True to draw corners rounded. False to draw square corners.")]
		[Browsable(true)]
		[DefaultValue(true)]
		public bool RoundedCorners
		{
			get
			{
				return this._RoundedCorners;
			}
			set
			{
				this._RoundedCorners = value;
				this.Refresh();
			}
		}

		[Category("Appearance")]
		[Description("Determins the direction of progress displayed in the ProgressBarEx.")]
		[Browsable(true)]
		[DefaultValue(typeof(ProgressBarEx.ProgressDir), "Horizontal")]
		public ProgressBarEx.ProgressDir ProgressDirection
		{
			get
			{
				return this._ProgressDirection;
			}
			set
			{
				this._ProgressDirection = value;
				this.Refresh();
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			Point point = new Point(0, 0);
			Point point2 = new Point(0, base.Height);
			if (this._ProgressDirection == ProgressBarEx.ProgressDir.Vertical)
			{
				point2 = new Point(base.Width, 0);
			}
			checked
			{
				using (GraphicsPath graphicsPath = new GraphicsPath())
				{
					Rectangle rectangle = new Rectangle(0, 0, base.Width, base.Height);
					int rad = (int)Math.Round((double)rectangle.Height / 2.5);
					if (rectangle.Width < rectangle.Height)
					{
						rad = (int)Math.Round((double)rectangle.Width / 2.5);
					}
					using (LinearGradientBrush linearGradientBrush = new LinearGradientBrush(point, point2, this._BackColor, this._GradiantColor))
					{
						linearGradientBrush.Blend = this.bBlend;
						if (this._RoundedCorners)
						{
							GraphicsPath graphicsPath2 = graphicsPath;
							this.MakePath(ref graphicsPath2, rectangle, rad);
							e.Graphics.FillPath(linearGradientBrush, graphicsPath);
						}
						else
						{
							e.Graphics.FillRectangle(linearGradientBrush, rectangle);
						}
					}
					if (this._Value > this._Minimum)
					{
						int num = (int)Math.Round(unchecked((double)base.Width / (double)(checked(this._Maximum - this._Minimum)) * (double)this._Value));
						if (this._ProgressDirection == ProgressBarEx.ProgressDir.Vertical)
						{
							num = (int)Math.Round(unchecked((double)base.Height / (double)(checked(this._Maximum - this._Minimum)) * (double)this._Value));
							rectangle.Y = rectangle.Height - num;
							rectangle.Height = num;
						}
						else
						{
							rectangle.Width = num;
						}
						using (LinearGradientBrush linearGradientBrush2 = new LinearGradientBrush(point, point2, this._ProgressColor, this._GradiantColor))
						{
							linearGradientBrush2.Blend = this.bBlend;
							if (this._RoundedCorners)
							{
								if (this._ProgressDirection == ProgressBarEx.ProgressDir.Horizontal)
								{
									rectangle.Height--;
								}
								else
								{
									rectangle.Width--;
								}
								using (GraphicsPath graphicsPath3 = new GraphicsPath())
								{
									GraphicsPath graphicsPath2 = graphicsPath3;
									this.MakePath(ref graphicsPath2, rectangle, rad);
									using (GraphicsPath graphicsPath4 = new GraphicsPath())
									{
										using (Region region = new Region(graphicsPath))
										{
											region.Intersect(graphicsPath3);
											graphicsPath4.AddRectangles(region.GetRegionScans(new Matrix()));
										}
										e.Graphics.FillPath(linearGradientBrush2, graphicsPath4);
										goto IL_272;
									}
								}
							}
							e.Graphics.FillRectangle(linearGradientBrush2, rectangle);
						}
					}
					IL_272:
					if (this._Image != null)
					{
						if (this._ImageLayout == ProgressBarEx.ImageLayoutType.Stretch)
						{
							e.Graphics.DrawImage(this._Image, 0, 0, base.Width, base.Height);
						}
						else if (this._ImageLayout == ProgressBarEx.ImageLayoutType.None)
						{
							e.Graphics.DrawImage(this._Image, 0, 0);
						}
						else
						{
							int x = (int)Math.Round(unchecked((double)base.Width / 2.0 - (double)this._Image.Width / 2.0));
							int y = (int)Math.Round(unchecked((double)base.Height / 2.0 - (double)this._Image.Height / 2.0));
							e.Graphics.DrawImage(this._Image, x, y);
						}
					}
					if (this._ShowPercentage | this._ShowText)
					{
						string text = "";
						if (this._ShowText)
						{
							text = this.Text;
						}
						if (this._ShowPercentage)
						{
							text = text + ((int)Math.Round(unchecked(100.0 / (double)(checked(this._Maximum - this._Minimum)) * (double)this._Value))).ToString() + "%";
						}
						using (StringFormat stringFormat = new StringFormat
						{
							Alignment = StringAlignment.Center,
							LineAlignment = StringAlignment.Center
						})
						{
							e.Graphics.DrawString(text, this.Font, this._ForeColorBrush, new Rectangle(0, 0, base.Width, base.Height), stringFormat);
						}
					}
					if (this._Border)
					{
						rectangle = new Rectangle(0, 0, base.Width - 1, base.Height - 1);
						if (this._RoundedCorners)
						{
							GraphicsPath graphicsPath2 = graphicsPath;
							this.MakePath(ref graphicsPath2, rectangle, rad);
							e.Graphics.DrawPath(this._BorderPen, graphicsPath);
						}
						else
						{
							e.Graphics.DrawRectangle(this._BorderPen, rectangle);
						}
					}
				}
			}
		}

		private void MakePath(ref GraphicsPath pth, Rectangle rc, int rad)
		{
			pth.Reset();
			pth.AddArc(rc.X, rc.Y, rad, rad, 180f, 90f);
			checked
			{
				pth.AddArc(rc.Right - rad, rc.Y, rad, rad, 270f, 90f);
				pth.AddArc(rc.Right - rad, rc.Bottom - rad, rad, rad, 0f, 90f);
				pth.AddArc(rc.X, rc.Bottom - rad, rad, rad, 90f, 90f);
				pth.CloseFigure();
			}
		}

		protected override void OnTextChanged(EventArgs e)
		{
			this.Refresh();
			base.OnTextChanged(e);
		}

		protected override void Dispose(bool disposing)
		{
			this._ForeColorBrush.Dispose();
			this._BorderPen.Dispose();
			base.Dispose(disposing);
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = Color.Transparent;
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("BackgroundImageLayout is not implemented.", true)]
		public new ImageLayout BackgroundImageLayout
		{
			get
			{
				return base.BackgroundImageLayout;
			}
			set
			{
				throw new NotImplementedException("BackgroundImageLayout is not implemented.");
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("BackgroundImage is not implemented.", true)]
		public new Image BackgroundImage
		{
			get
			{
				return null;
			}
			set
			{
				throw new NotImplementedException("BackgroundImage is not implemented.");
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("TabStop is not implemented.", true)]
		public new bool TabStop
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotImplementedException("TabStop is not implemented.");
			}
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Obsolete("TabIndex is not implemented.", true)]
		public new int TabIndex
		{
			get
			{
				return base.TabIndex;
			}
			set
			{
				throw new NotImplementedException("TabIndex is not implemented.");
			}
		}


		private Blend bBlend;
		private int _Minimum;
		private int _Maximum;
		private int _Value;
		private bool _Border;
		private Pen _BorderPen;
		private Color _BorderColor;
		private ProgressBarEx.GradiantArea _GradiantPosition;
		private Color _GradiantColor;
		private Color _BackColor;
		private Color _ProgressColor;
		private SolidBrush _ForeColorBrush;
		private bool _ShowPercentage;
		private bool _ShowText;
		private ProgressBarEx.ImageLayoutType _ImageLayout;
		private Bitmap _Image;
		private bool _RoundedCorners;
		private ProgressBarEx.ProgressDir _ProgressDirection;
		public enum GradiantArea
		{
			None,
			Top,
			Center,
			Bottom
		}

		public enum ImageLayoutType
		{
			None,
			Center,
			Stretch
		}

		public enum ProgressDir
		{
			Horizontal,
			Vertical
		}
	}
}
