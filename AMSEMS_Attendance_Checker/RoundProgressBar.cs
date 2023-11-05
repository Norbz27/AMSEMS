using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class RoundProgressBar : UserControl
    {
        private int progress;
        private const int maxProgress = 100;

        public RoundProgressBar()
        {
            InitializeComponent();
            progress = 0;
            Timer timer = new Timer();
            timer.Interval = 100; // Adjust interval for progress speed
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            progress = (progress + 1) % (maxProgress + 1); // Update progress
            Invalidate(); // Redraw the control
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int diameter = Math.Min(Width, Height) - 10;
            int x = (Width - diameter) / 2;
            int y = (Height - diameter) / 2;
            Rectangle rect = new Rectangle(x, y, diameter, diameter);

            // Background circle
            e.Graphics.DrawEllipse(Pens.LightGray, rect);

            // Progress circle
            float angle = 360 * progress / (float)maxProgress;
            e.Graphics.DrawPie(Pens.Blue, rect, -90, angle);
        }
    }
}
