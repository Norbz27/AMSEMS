using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class UserControlDays_Calendar : UserControl
    {
        public UserControlDays_Calendar()
        {
            InitializeComponent();
        }
        private void UserControlDays_Calendar_Load(object sender, EventArgs e)
        {

        }
        public void days(int numdays, int day, int month, int year)
        {
            DateTime now = DateTime.Now;

            if (numdays == day && month == now.Month && year == now.Year)
            {
                lblDays.Text = numdays.ToString();
                this.lblDays.Image = global::AMSEMS.Properties.Resources.new_moon_24;
                this.lblDays.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
                this.lblDays.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Bold);
                this.lblDays.StateCommon.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            }
            else
            {
                lblDays.Text = numdays.ToString();
                this.lblDays.Image = null;
                this.lblDays.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
                this.lblDays.StateCommon.TextColor = System.Drawing.Color.Black;
            }
        }
    }
}
