using ComponentFactory.Krypton.Toolkit;
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
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formConfigFee : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;


        string event_id, date;

        public bool isCollapsed;
        private List<string> suggestions = new List<string>{};
        private ListBox listBoxSuggestions;
        public formConfigFee()
        {
            InitializeComponent();
        }

        private void formEventConfig_Load(object sender, EventArgs e)
        {
            displayFees();
        }
        public void displayCBData(string eventName)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbEvents.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand(@"SELECT Event_ID, Event_Name FROM tbl_events WHERE Attendance = 'True' AND (Exclusive = 'All Students' OR Exclusive = @Department OR Exclusive = 'Specific Students') ORDER BY Start_Date;", cn);
                    cm.Parameters.AddWithValue("@Department", FormDeptHeadNavigation.depdes);
                    dr = cm.ExecuteReader();

                    while (dr.Read())
                    {
                        cbEvents.Items.Add(dr["Event_Name"].ToString());
                    }
                    dr.Close();

                    cbEvents.Text = eventName;
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void displayFees()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                string eventid = null;
                cn.Open();
                cm = new SqlCommand("Select Event_ID from tbl_events where Event_Name = @Eventname", cn);
                cm.Parameters.AddWithValue("@Eventname", cbEvents.Text);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        eventid = reader["Event_ID"].ToString();
                    }
                }

                cm = new SqlCommand("Select Penalty_AM, Penalty_PM from tbl_attendance where Event_ID = @eventid AND FORMAT(Date_Time, 'yyyy-MM-dd') = @date", cn);
                cm.Parameters.AddWithValue("@eventid", eventid);
                cm.Parameters.AddWithValue("@date", Dt.Value.ToString("yyyy-MM-dd"));
                using (dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // Check for DBNull.Value and display "00.00" in that case
                        string penaltyAM = dr["Penalty_AM"] != DBNull.Value ? Convert.ToDecimal(dr["Penalty_AM"]).ToString("0.00") : "00.00";
                        string penaltyPM = dr["Penalty_PM"] != DBNull.Value ? Convert.ToDecimal(dr["Penalty_PM"]).ToString("0.00") : "00.00";

                        tbAMFee.Text =  penaltyAM;
                        tbPMFee.Text =  penaltyPM;
                    }
                    else
                    {
                        tbAMFee.Text = "00.00";
                        tbPMFee.Text = "00.00";
                    }
                }
            }
        }
        private void cbExclusive_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                displayFees();
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT Event_ID, Event_Name, Start_Date, End_Date FROM tbl_events WHERE Event_Name = @EventName";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@EventName", cbEvents.Text);
                        SqlDataReader dr = cm.ExecuteReader();

                        if (dr.Read())
                        {
                            event_id = dr["Event_ID"].ToString();

                            // Reset MinDate and MaxDate to their default values
                            Dt.MinDate = DateTimePicker.MinimumDateTime;
                            Dt.MaxDate = DateTimePicker.MaximumDateTime;

                            // Ensure that the values are DateTime before assigning to MinDate and MaxDate
                            DateTime startDate = (DateTime)dr["Start_Date"];
                            DateTime endDate = (DateTime)dr["End_Date"];

                            // Optionally, check if the dates are valid before setting
                            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && startDate <= endDate)
                            {
                                Dt.Value = startDate;
                                Dt.MinDate = startDate;
                                Dt.MaxDate = endDate;
                            }

                            date = Dt.Value.ToString();
                        }

                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                string eventid = null;
                cn.Open();
                cm = new SqlCommand("Select Event_ID from tbl_events where Event_Name = @Eventname", cn);
                cm.Parameters.AddWithValue("@Eventname", cbEvents.Text);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        eventid = reader["Event_ID"].ToString();
                    }
                }

                if (!string.IsNullOrEmpty(eventid))
                {
                    cm = new SqlCommand("UPDATE tbl_attendance SET Penalty_AM = ISNULL(@penaltyAm, 0), Penalty_PM = ISNULL(@penaltyPm, 0) WHERE Event_ID = @eventid AND FORMAT(Date_Time, 'yyyy-MM-dd') = @date", cn);
                    cm.Parameters.AddWithValue("@penaltyAm", tbAMFee.Text);
                    cm.Parameters.AddWithValue("@penaltyPm", tbPMFee.Text);
                    cm.Parameters.AddWithValue("@eventid", eventid);
                    cm.Parameters.AddWithValue("@date", Dt.Value.ToString("yyyy-MM-dd"));
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();
                }
            }
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                // If the pressed key is not a digit or Backspace, suppress it
                e.Handled = true;
            }
        }
    }
}
