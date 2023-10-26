using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("SELECT Event_Name,Color FROM tbl_events WHERE Start_Date = @date", cn);
                cm.Parameters.AddWithValue("@date", date);
                dr = cm.ExecuteReader();

                eventLabels = new Label[0];

                // Clear existing event labels before adding new ones.
                foreach (Label label in eventLabels)
                {
                    this.Controls.Remove(label);
                }

                int labelCount = 0;
                while (dr.Read())
                {
                    Color color = ColorTranslator.FromHtml(dr["Color"].ToString());
                    // Create a new label for each event.
                    Label lblEvent = new Label();
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

                    // Add the label to the array and the control.
                    eventLabels = AddLabelToArray(eventLabels, lblEvent);


                    labelCount++;
                }
                dr.Close();
                cn.Close();
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
            formAddEvent.Show();
        }
    }
}
