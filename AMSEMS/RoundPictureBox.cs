using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AMSEMS
{
    internal class RoundPictureBox : PictureBox
    {
        private Color borderColor = Color.LightGray;
        private int borderWidth = 2;

        public Color BorderColor
        {
            get { return borderColor; }
            set { borderColor = value; Invalidate(); }
        }

        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath grpath = new GraphicsPath();
            grpath.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new Region(grpath);

            base.OnPaint(pe);

            // Draw the border
            using (Pen borderPen = new Pen(borderColor, borderWidth))
            {
                pe.Graphics.DrawEllipse(borderPen, borderWidth / 2, borderWidth / 2, ClientSize.Width - borderWidth, ClientSize.Height - borderWidth);
            }
        }
    }
}
