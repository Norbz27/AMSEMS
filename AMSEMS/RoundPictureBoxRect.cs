using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AMSEMS
{
    internal class RoundPictureBoxRect : PictureBox
    {
        private int borderWidth = 2;
        private Pen borderPen = new Pen(Color.White); // Change the color as needed
        private int cornerRadius = 10;

        public int BorderWidth
        {
            get { return borderWidth; }
            set { borderWidth = value; Invalidate(); }
        }

        public int CornerRadius
        {
            get { return cornerRadius; }
            set { cornerRadius = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            GraphicsPath grpath = new GraphicsPath();
            int width = ClientSize.Width - borderWidth;
            int height = ClientSize.Height - borderWidth;
            int offset = borderWidth / 2;

            grpath.AddArc(offset, offset, cornerRadius * 2, cornerRadius * 2, 180, 90); // Top-left corner
            grpath.AddArc(width - cornerRadius * 2, offset, cornerRadius * 2, cornerRadius * 2, 270, 90); // Top-right corner
            grpath.AddArc(width - cornerRadius * 2, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90); // Bottom-right corner
            grpath.AddArc(offset, height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90); // Bottom-left corner

            grpath.CloseFigure();
            this.Region = new Region(grpath);

            base.OnPaint(pe);

            int borderOffset = borderWidth / 2;
            pe.Graphics.DrawPath(borderPen, grpath);
        }
    }
}
