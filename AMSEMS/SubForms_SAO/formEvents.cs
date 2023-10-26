using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formEvents : Form
    {
        int month, year, day;
        public formEvents()
        {
            InitializeComponent();
        }

        private void formEvents_Load(object sender, EventArgs e)
        {
            display();
        }
        private void display()
        {
            DateTime now = DateTime.Now;

            month = now.Month;
            year = now.Year;
            day = now.Day;
            calendar();
           
        }
        public void RefreshCalendar()
        {
            // Clear and refresh the calendar view
            daysContainer.Controls.Clear();
            calendar();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            daysContainer.Controls.Clear();
            if(month >= 12) 
            {
                month = 1;
                year++;
            }
            else
            {
                month++;
            }
            calendar();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            daysContainer.Controls.Clear();
            if (month <= 1)
            {
                month = 12;
                year--;
            }
            else
            {
                month--;
            }
            calendar();
        }

        private void btnToday_Click(object sender, EventArgs e)
        {
            daysContainer.Controls.Clear();
            DateTime now = DateTime.Now;

            month = now.Month;
            year = now.Year;
            day = now.Day;
            lblMonthYear.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(this.month) + " " + this.year;
            //First day  of the month

            DateTime startofthemonth = new DateTime(this.year, this.month, 1);

            //get the count of the day of the month

            int days = DateTime.DaysInMonth(this.year, this.month) + 1;

            //convert the startofthemonth to Integer

            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank_Calendar ucBlank = new UserControlBlank_Calendar();
                daysContainer.Controls.Add(ucBlank);
            }
            for (int i = 1; i < days; i++)
            {
                UserControlDays_Calendar ucDays = new UserControlDays_Calendar(this);
                ucDays.days(i, day, month, year);
                daysContainer.Controls.Add(ucDays);
            }
        }

        public void calendar()
        {
            lblMonthYear.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(month) + " " + year;
            //First day  of the month

            DateTime startofthemonth = new DateTime(year, month, 1);

            //get the count of the day of the month

            int days = DateTime.DaysInMonth(year, month) + 1;

            //convert the startofthemonth to Integer

            int dayoftheweek = Convert.ToInt32(startofthemonth.DayOfWeek.ToString("d")) + 1;

            for (int i = 1; i < dayoftheweek; i++)
            {
                UserControlBlank_Calendar ucBlank = new UserControlBlank_Calendar();
                daysContainer.Controls.Add(ucBlank);
            }
            for (int i = 1; i < days; i++)
            {
                UserControlDays_Calendar ucDays = new UserControlDays_Calendar(this);
                ucDays.days(i, day, month, year);
                DateTime eventday = new DateTime(year, month, i);
                ucDays.DisplayEventsForDate(eventday);

                daysContainer.Controls.Add(ucDays);
            }

        }
    }
}
