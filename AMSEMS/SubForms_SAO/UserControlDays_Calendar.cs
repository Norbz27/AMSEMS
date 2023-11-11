using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class UserControlDays_Calendar : UserControl
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        public static int static_day, static_year, static_month;
        private Label[] eventLabels;
        formEvents form;

        public UserControlDays_Calendar(formEvents form)
        {
            InitializeComponent();
            this.form = form;
        }
        private void UserControlDays_Calendar_Load(object sender, EventArgs e)
        {

        }
        public void refresh()
        {
            form.RefreshCalendar();
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
                lblmonth.Text = month.ToString();
                lblyear.Text = year.ToString();
                this.lblDays.Image = null;
                this.lblDays.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
                this.lblDays.StateCommon.TextColor = System.Drawing.Color.Black;
            }

        }


        public void DisplayEventsForDate(DateTime date)
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    // Call the same method on the main thread
                    DisplayEventsForDate(date);
                });
            }
            else
            {
                try
                {
                    using (cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        cm = new SqlCommand("SELECT Event_ID, Event_Name, Color FROM tbl_events WHERE Start_Date = @date", cn);
                        cm.Parameters.AddWithValue("@date", date);
                        dr = cm.ExecuteReader();

                        // Clear existing event labels before adding new ones.
                        foreach (Label label in panel1.Controls.OfType<Label>().ToList())
                        {
                            panel1.Controls.Remove(label);
                            label.Dispose();
                        }

                        int labelCount = 0;
                        while (dr.Read())
                        {
                            Color color = ColorTranslator.FromHtml(dr["Color"].ToString());
                            string eventID = dr["Event_ID"].ToString(); // Capture the event ID here.

                            // Create a new label for each event.
                            Label lblEvent = new Label();
                            lblEvent.Click += (s, e) => EventDetails_Click(eventID); // Use the captured event ID.
                            panel1.Controls.Add(lblEvent);
                            lblEvent.BackColor = color;
                            lblEvent.Dock = DockStyle.Top;
                            lblEvent.Name = $"lblEvent_{labelCount}";
                            lblEvent.Text = dr["Event_Name"].ToString();
                            lblEvent.Font = new System.Drawing.Font("Poppins", 8F);
                            lblEvent.ForeColor = System.Drawing.Color.White;
                            lblEvent.TextAlign = ContentAlignment.MiddleCenter;

                            // Position the labels dynamically.
                            int topOffset = 28 + labelCount * 19;
                            lblEvent.Location = new System.Drawing.Point(0, topOffset);

                            labelCount++;
                        }

                        // Close the data reader and connection after you're done with it.
                        dr.Close();
                        cn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private Label[] AddLabelToArray(Label[] labelArray, Label newLabel)
        {
            Label[] newArray = new Label[labelArray.Length + 1];
            for (int i = 0; i < labelArray.Length; i++)
            {
                newArray[i] = labelArray[i];
            }
            newArray[labelArray.Length] = newLabel;
            return newArray;
        }

        private void addEventToolStripMenuItem_Click(object sender, EventArgs e)
        {
            static_day = Convert.ToInt32(lblDays.Text);
            static_month = Convert.ToInt32(lblmonth.Text);
            static_year = Convert.ToInt32(lblyear.Text);

            formAddEvent formAddEvent = new formAddEvent();
            formAddEvent.getForm(this);
            formAddEvent.ShowDialog();
        }
        private void EventDetails_Click(string eventid)
        {
            formEventDetails formEventDetails = new formEventDetails();
            formEventDetails.displayDetails(eventid);
            formEventDetails.getForm(this);
            formEventDetails.ShowDialog();
        }
    }
}
