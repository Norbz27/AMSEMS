using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace AMSEMS
{
    public class RJToggleButton : CheckBox
    {
        //Fields
        private Color onBackColor = Color.Green;
        private Color onToggleColor = Color.WhiteSmoke;
        private Color offBackColor = Color.Gray;
        private Color offToggleColor = Color.Gainsboro;
        private bool solidStyle = true;

        private int toggleX = 2; // X-coordinate of the toggle circle
        private Timer animationTimer;

        //Constructor
        public RJToggleButton()
        {
            this.MinimumSize = new Size(45, 22);
            animationTimer = new Timer();
            animationTimer.Interval = 10; // Interval for animation
            animationTimer.Tick += AnimationTimer_Tick;
        }

        //Methods
        private GraphicsPath GetFigurePath()
        {
            int arcSize = this.Height - 1;
            Rectangle leftArc = new Rectangle(0, 0, arcSize, arcSize);
            Rectangle rightArc = new Rectangle(this.Width - arcSize - 2, 0, arcSize, arcSize);

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(leftArc, 90, 180);
            path.AddArc(rightArc, 270, 180);
            path.CloseFigure();

            return path;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            int toggleSize = this.Height - 5;
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            pevent.Graphics.Clear(this.Parent.BackColor);

            if (this.Checked) //ON
            {
                // Draw the control surface
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(onBackColor), GetFigurePath());
                else
                    pevent.Graphics.DrawPath(new Pen(onBackColor, 2), GetFigurePath());

                // Draw the toggle with animation
                pevent.Graphics.FillEllipse(new SolidBrush(onToggleColor),
                    new Rectangle(toggleX, 2, toggleSize, toggleSize));
            }
            else //OFF
            {
                // Draw the control surface
                if (solidStyle)
                    pevent.Graphics.FillPath(new SolidBrush(offBackColor), GetFigurePath());
                else
                    pevent.Graphics.DrawPath(new Pen(offBackColor, 2), GetFigurePath());

                // Draw the toggle with animation
                pevent.Graphics.FillEllipse(new SolidBrush(offToggleColor),
                    new Rectangle(toggleX, 2, toggleSize, toggleSize));
            }
        }

        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);

            // Start the animation when the button state changes
            animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int toggleSize = this.Height - 5;
            int targetX = this.Checked ? this.Width - toggleSize - 2 : 2;

            // Gradually move the toggle circle towards the target position
            if (toggleX != targetX)
            {
                int step = (targetX > toggleX) ? 1 : -1;
                toggleX += step;

                // Redraw the control to show the animation
                this.Invalidate();
            }
            else
            {
                // Stop the animation timer when the toggle circle reaches its target position
                animationTimer.Stop();
            }
        }
        //Properties
        [Category("RJ Code Advance")]
        public Color OnBackColor
        {
            get { return onBackColor; }
            set
            {
                onBackColor = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color OnToggleColor
        {
            get { return onToggleColor; }
            set
            {
                onToggleColor = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color OffBackColor
        {
            get { return offBackColor; }
            set
            {
                offBackColor = value;
                this.Invalidate();
            }
        }

        [Category("RJ Code Advance")]
        public Color OffToggleColor
        {
            get { return offToggleColor; }
            set
            {
                offToggleColor = value;
                this.Invalidate();
            }
        }

        [Browsable(false)]
        public override string Text
        {
            get { return base.Text; }
            set { }
        }

        [Category("RJ Code Advance")]
        [DefaultValue(true)]
        public bool SolidStyle
        {
            get { return solidStyle; }
            set
            {
                solidStyle = value;
                this.Invalidate();
            }
        }
    }
}
