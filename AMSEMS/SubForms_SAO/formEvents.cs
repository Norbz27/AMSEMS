﻿using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formEvents : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        int month, year, day;
        private KryptonGroupBox[] eventDetails;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public formEvents()
        {
            InitializeComponent();
            // Scroll to the end
            panel1.VerticalScroll.Value = panel1.VerticalScroll.Maximum;
            panel1.AutoScroll = false;

            // Hide the vertical scrollbar
            panel1.VerticalScroll.Visible = false;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                // This method runs in a background thread
                DisplayOnMainThread(); // Load data and update UI on the main thread
                System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
            }
            catch (Exception ex)
            {
                // Handle the exception
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayOnMainThread()
        {
            if (this.InvokeRequired)
            {
                // Call the same method on the main thread
                this.Invoke((MethodInvoker)delegate
                {
                    DisplayOnMainThread();
                });
            }
            else
            {
                // Your UI update code
                display();
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                if (e.Error is TargetInvocationException && e.Error.InnerException != null)
                {
                    // Unwrap and show the inner exception
                    MessageBox.Show("An error occurred: " + e.Error.InnerException.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void formEvents_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();

        }
        private void display()
        {
            DateTime now = DateTime.Now;

            month = now.Month;
            year = now.Year;
            day = now.Day;
            calendar();
            DisplayEventsDetails();
        }
        public void RefreshCalendar()
        {
            // Clear and refresh the calendar view
            daysContainer.Controls.Clear();
            calendar();
            DisplayEventsDetails();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            daysContainer.Controls.Clear();
            if (month >= 12)
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

        private async void btnToday_Click(object sender, EventArgs e)
        {
            daysContainer.Controls.Clear();
            ptbLoading2.Visible = true;
            await Task.Delay(2000);
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
                DateTime eventday = new DateTime(year, month, i);
                ucDays.DisplayEventsForDate(eventday);

                daysContainer.Controls.Add(ucDays);
            }
            ptbLoading2.Visible = false;
        }

        public async void calendar()
        {
            if (daysContainer.InvokeRequired)
            {
                daysContainer.Invoke((MethodInvoker)delegate
                {
                    // Call the same method on the main thread
                    calendar();
                });
            }
            else
            {
                // Your UI update code
                daysContainer.Controls.Clear();
                ptbLoading2.Visible = true;
                await Task.Delay(1000);
                lblMonthYear.Text = DateTimeFormatInfo.CurrentInfo.GetMonthName(month) + " " + year;

                DateTime startofthemonth = new DateTime(year, month, 1);
                int days = DateTime.DaysInMonth(year, month) + 1;
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
                ptbLoading2.Visible = false;
            }

        }

        private void btnCreateEvent_Click(object sender, EventArgs e)
        {
            DateTime date = DateTime.Now;
            formAddEvent formAddEvent = new formAddEvent();
            formAddEvent.getForm2(this);
            formAddEvent.ShowDialog();
        }

        private void formEvents_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
                e.Cancel = true;  
            }
        }

        public async void DisplayEventsDetails()
        {
            if (flowLayoutPanel1.InvokeRequired)
            {
                flowLayoutPanel1.Invoke((MethodInvoker)delegate
                {
                    // Call the same method on the main threadz
                    DisplayEventsDetails();
                });
            }
            else
            {
                flowLayoutPanel1.Controls.Clear(); // Clear existing event details
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT Event_ID,Event_Name, Color, Start_Date FROM tbl_events WHERE Start_Date > CAST(GETDATE() AS DATE) ORDER BY Start_Date DESC", cn))
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        eventDetails = new KryptonGroupBox[0];

                        int labelCount = 0;
                        while (dr.Read())
                        {
                            string eventid = dr["Event_ID"].ToString();
                            string eventname = dr["Event_Name"].ToString();
                            string color = dr["Color"].ToString();
                            DateTime date = DateTime.Parse(dr["Start_Date"].ToString());
                            string formattedDate = date.ToString("dddd, MMMM d, yyyy");
                            string day = date.Day.ToString();

                            EventDetailsApperance(eventid, eventname, color, formattedDate, day);

                            labelCount++;
                        }
                    }
                    //using (SqlCommand cm = new SqlCommand("SELECT Activity_ID,Activity_Name, Color, Date, Time FROM tbl_activities WHERE Date > CAST(GETDATE() AS DATE) ORDER BY Date DESC", cn))
                    //using (SqlDataReader dr = cm.ExecuteReader())
                    //{
                    //    eventDetails = new KryptonGroupBox[0];

                    //    int labelCount = 0;
                    //    while (dr.Read())
                    //    {
                    //        string eventid = dr["Event_ID"].ToString();
                    //        string eventname = dr["Event_Name"].ToString();
                    //        string color = dr["Color"].ToString();
                    //        DateTime date = DateTime.Parse(dr["Start_Date"].ToString());
                    //        string formattedDate = date.ToString("dddd, MMMM d, yyyy");
                    //        string day = date.Day.ToString();

                    //        EventDetailsApperance(eventid, eventname, color, formattedDate, day);

                    //        labelCount++;
                    //    }
                    //}
                }
                ptbLoading.Visible = false;
            }
        }

        private void btnCreateActivity_Click(object sender, EventArgs e)
        {
            formAddActivity formAddActivity = new formAddActivity();
            formAddActivity.getForm2(this, false);
            formAddActivity.ShowDialog();
        }

        public void EventDetailsApperance(string eventid, string eventname, string color, string date, string day)
        {

            Color Backcolor = ColorTranslator.FromHtml(color);
            KryptonGroupBox kryptonGroupBox6 = new KryptonGroupBox();
            Panel panel12 = new Panel();
            //KryptonLabel kryptonLabel4 = new KryptonLabel();
            KryptonLabel kryptonLabel2 = new KryptonLabel();
            KryptonLabel kryptonLabel11 = new KryptonLabel();
            KryptonLabel kryptonLabel10 = new KryptonLabel();

            this.flowLayoutPanel1.Controls.Add(kryptonGroupBox6);
            this.flowLayoutPanel1.Controls.Add(panel12);
            kryptonGroupBox6.CaptionVisible = false;
            kryptonGroupBox6.CausesValidation = false;

            kryptonGroupBox6.Name = "kryptonGroupBox6";
            kryptonGroupBox6.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonGroupBox6.Panel.Click += (s, e) => KryptonGroupBox_Click(eventid);

            //kryptonGroupBox6.Panel.Controls.Add(kryptonLabel4);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel2);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel11);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel10);
            kryptonGroupBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonGroupBox6.Dock = DockStyle.Top;
            kryptonGroupBox6.Margin = new System.Windows.Forms.Padding(5);
            kryptonGroupBox6.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            kryptonGroupBox6.Size = new System.Drawing.Size(208, 92);
            kryptonGroupBox6.StateCommon.Back.Color1 = Backcolor;
            kryptonGroupBox6.StateCommon.Back.Color2 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.Color1 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.Color2 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            kryptonGroupBox6.StateCommon.Border.Rounding = 10;
            kryptonGroupBox6.StateCommon.Border.Width = 1;

            kryptonLabel2.Location = new System.Drawing.Point(15, 8);
            kryptonLabel2.Name = "kryptonLabel2";
            kryptonLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel2.Size = new System.Drawing.Size(44, 39);
            kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 18F, System.Drawing.FontStyle.Bold);
            kryptonLabel2.Values.Text = day;

            //kryptonLabel4.Cursor = System.Windows.Forms.Cursors.Default;
            //kryptonLabel4.Location = new System.Drawing.Point(136, 53);
            //kryptonLabel4.Name = "kryptonLabel4";
            //kryptonLabel4.Size = new System.Drawing.Size(53, 19);
            //kryptonLabel4.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            //kryptonLabel4.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            //kryptonLabel4.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            //kryptonLabel4.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            //kryptonLabel4.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            //kryptonLabel4.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            //kryptonLabel4.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            //kryptonLabel4.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            //kryptonLabel4.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            //kryptonLabel4.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            //kryptonLabel4.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            //kryptonLabel4.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            //kryptonLabel4.Values.Text = "9:00 AM";

            kryptonLabel11.Location = new System.Drawing.Point(57, 16);
            kryptonLabel11.AutoSize = false;
            kryptonLabel11.Name = "kryptonLabel11";
            kryptonLabel11.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel11.Size = new System.Drawing.Size(160, 23);
            kryptonLabel11.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel11.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel11.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F, System.Drawing.FontStyle.Bold);
            kryptonLabel11.Values.Text = eventname;

            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Default;
            kryptonLabel10.Location = new System.Drawing.Point(15, 53);
            kryptonLabel10.Name = "kryptonLabel10";
            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel10.Size = new System.Drawing.Size(117, 19);
            kryptonLabel10.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            kryptonLabel10.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            kryptonLabel10.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel10.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel10.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            kryptonLabel10.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.Values.Text = date;

            panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            panel12.Dock = System.Windows.Forms.DockStyle.Top;
            panel12.Location = new System.Drawing.Point(0, 0);
            panel12.Name = "panel12";
            panel12.Cursor = System.Windows.Forms.Cursors.Hand;
            panel12.Size = new System.Drawing.Size(234, 10);
            panel12.TabIndex = 19;

            panel12.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel10.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel11.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel2.Click += (s, e) => KryptonGroupBox_Click(eventid);

            this.flowLayoutPanel1.Invoke((MethodInvoker)delegate
            {
                // Inside this block, you can update UI controls safely
                this.flowLayoutPanel1.Controls.Add(kryptonGroupBox6);
                this.flowLayoutPanel1.Controls.Add(panel12);
            });
        }
        //public void ActivityDetailsApperance(string activityid, string activityname, string color, string date, string day, string time)
        //{

        //    Color Backcolor = ColorTranslator.FromHtml(color);
        //    KryptonGroupBox kryptonGroupBox6 = new KryptonGroupBox();
        //    Panel panel12 = new Panel();
        //    KryptonLabel kryptonLabel4 = new KryptonLabel();
        //    KryptonLabel kryptonLabel2 = new KryptonLabel();
        //    KryptonLabel kryptonLabel11 = new KryptonLabel();
        //    KryptonLabel kryptonLabel10 = new KryptonLabel();

        //    this.flowLayoutPanel1.Controls.Add(kryptonGroupBox6);
        //    this.flowLayoutPanel1.Controls.Add(panel12);
        //    kryptonGroupBox6.CaptionVisible = false;
        //    kryptonGroupBox6.CausesValidation = false;

        //    kryptonGroupBox6.Name = "kryptonGroupBox6";
        //    kryptonGroupBox6.Click += (s, e) => KryptonGroupBox_Click(activityid);
        //    kryptonGroupBox6.Panel.Click += (s, e) => KryptonGroupBox_Click(activityid);

        //    kryptonGroupBox6.Panel.Controls.Add(kryptonLabel4);
        //    kryptonGroupBox6.Panel.Controls.Add(kryptonLabel2);
        //    kryptonGroupBox6.Panel.Controls.Add(kryptonLabel11);
        //    kryptonGroupBox6.Panel.Controls.Add(kryptonLabel10);
        //    kryptonGroupBox6.Cursor = System.Windows.Forms.Cursors.Hand;
        //    kryptonGroupBox6.Dock = DockStyle.Top;
        //    kryptonGroupBox6.Margin = new System.Windows.Forms.Padding(5);
        //    kryptonGroupBox6.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
        //    kryptonGroupBox6.Size = new System.Drawing.Size(208, 92);
        //    kryptonGroupBox6.StateCommon.Back.Color1 = Backcolor;
        //    kryptonGroupBox6.StateCommon.Back.Color2 = Backcolor;
        //    kryptonGroupBox6.StateCommon.Border.Color1 = Backcolor;
        //    kryptonGroupBox6.StateCommon.Border.Color2 = Backcolor;
        //    kryptonGroupBox6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
        //    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
        //    | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
        //    kryptonGroupBox6.StateCommon.Border.Rounding = 10;
        //    kryptonGroupBox6.StateCommon.Border.Width = 1;

        //    kryptonLabel2.Location = new System.Drawing.Point(15, 8);
        //    kryptonLabel2.Name = "kryptonLabel2";
        //    kryptonLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
        //    kryptonLabel2.Size = new System.Drawing.Size(44, 39);
        //    kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 18F, System.Drawing.FontStyle.Bold);
        //    kryptonLabel2.Values.Text = day;

        //    kryptonLabel4.Cursor = System.Windows.Forms.Cursors.Default;
        //    kryptonLabel4.Location = new System.Drawing.Point(136, 53);
        //    kryptonLabel4.Name = "kryptonLabel4";
        //    kryptonLabel4.Size = new System.Drawing.Size(53, 19);
        //    kryptonLabel4.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel4.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
        //    kryptonLabel4.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
        //    kryptonLabel4.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel4.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel4.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel4.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel4.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel4.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
        //    kryptonLabel4.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel4.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel4.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel4.Values.Text = time;

        //    kryptonLabel11.Location = new System.Drawing.Point(57, 16);
        //    kryptonLabel11.AutoSize = false;
        //    kryptonLabel11.Name = "kryptonLabel11";
        //    kryptonLabel11.Cursor = System.Windows.Forms.Cursors.Hand;
        //    kryptonLabel11.Size = new System.Drawing.Size(160, 23);
        //    kryptonLabel11.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel11.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel11.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F, System.Drawing.FontStyle.Bold);
        //    kryptonLabel11.Values.Text = activityname;

        //    kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Default;
        //    kryptonLabel10.Location = new System.Drawing.Point(15, 53);
        //    kryptonLabel10.Name = "kryptonLabel10";
        //    kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Hand;
        //    kryptonLabel10.Size = new System.Drawing.Size(117, 19);
        //    kryptonLabel10.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel10.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
        //    kryptonLabel10.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
        //    kryptonLabel10.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel10.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel10.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel10.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel10.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
        //    kryptonLabel10.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
        //    kryptonLabel10.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
        //    kryptonLabel10.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel10.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
        //    kryptonLabel10.Values.Text = date;

        //    panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
        //    panel12.Dock = System.Windows.Forms.DockStyle.Top;
        //    panel12.Location = new System.Drawing.Point(0, 0);
        //    panel12.Name = "panel12";
        //    panel12.Cursor = System.Windows.Forms.Cursors.Hand;
        //    panel12.Size = new System.Drawing.Size(234, 10);
        //    panel12.TabIndex = 19;

        //    panel12.Click += (s, e) => KryptonGroupBox1_Click(activityid);
        //    kryptonLabel10.Click += (s, e) => KryptonGroupBox1_Click(activityid);
        //    kryptonLabel11.Click += (s, e) => KryptonGroupBox1_Click(activityid);
        //    kryptonLabel2.Click += (s, e) => KryptonGroupBox1_Click(activityid);
        //    kryptonLabel4.Click += (s, e) => KryptonGroupBox1_Click(activityid);

        //    this.flowLayoutPanel1.Invoke((MethodInvoker)delegate
        //    {
        //        // Inside this block, you can update UI controls safely
        //        this.flowLayoutPanel1.Controls.Add(kryptonGroupBox6);
        //        this.flowLayoutPanel1.Controls.Add(panel12);
        //    });
        //}
        //private void KryptonGroupBox1_Click(string actid)
        //{
        //    formAddActivity formAddActivity = new formAddActivity();
        //    formAddActivity.displayInformation(actid);
        //    formAddActivity.getForm2(this, false);
        //    formAddActivity.ShowDialog();
        //}

        private void KryptonGroupBox_Click(string eventid)
        {
            formEventDetails formEventDetails = new formEventDetails();
            formEventDetails.displayDetails(eventid);
            formEventDetails.getForm2(this);
            formEventDetails.ShowDialog();
        }
    }
}
