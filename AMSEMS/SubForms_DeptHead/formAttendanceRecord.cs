using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceRecord : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        formConfigFee formConfigFee;

        string event_id, date;

        public formAttendanceRecord()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            formConfigFee = new formConfigFee();
        }
        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbEvents.Items.Clear();
                    cbSection.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand(@"SELECT Event_ID, Event_Name FROM tbl_events WHERE Attendance = 'True' AND (Exclusive = 'All Students' OR Exclusive = @Department OR Exclusive = 'Specific Students') ORDER BY Start_Date;", cn);
                    cm.Parameters.AddWithValue("@Department", FormDeptHeadNavigation.depdes);
                    dr = cm.ExecuteReader();

                    while (dr.Read())
                    {
                        cbEvents.Items.Add(dr["Event_Name"].ToString());
                    }

                    dr.Close();

                    if (cbEvents.Items.Count > 0)
                    {
                        cbEvents.SelectedIndex = 0;
                    }

                    cm = new SqlCommand("Select Description from tbl_section", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbSection.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
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

                        lblAmPenaltyFee.Text = "₱ " + penaltyAM;
                        lblPmPenaltyFee.Text = "₱ " + penaltyPM;
                    }
                    else
                    {
                        lblAmPenaltyFee.Text = "₱ 00.00";
                        lblPmPenaltyFee.Text = "₱ 00.00";
                    }
                }
            }
        }


        public void displayTable()
        {
            if (dgvRecord.InvokeRequired)
            {
                dgvRecord.Invoke(new Action(() => displayTable()));
                return;
            }
            try
            {
                dgvRecord.Rows.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    string query = "";

                    // Check if the event is for Specific Students
                    if (IsEventForSpecificStudents(event_id))
                    {
                        query = @"SELECT
                                e.Event_ID,
                                s.ID AS id,
                                UPPER(s.Firstname) AS fname,
                                UPPER(s.Middlename) AS mname,
                                UPPER(s.Lastname) AS lname,
                                sec.Description AS secdes,
                                ISNULL(FORMAT(att.Date_Time, 'yyyy-MM-dd'), '-------') AS Date_Time,
                                COALESCE(att.Penalty_AM, 0) AS Penalty_AM,
                                ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), @AM_Pen) AS AM_IN_Penalty,
                                ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), @AM_Pen) AS AM_OUT_Penalty,
                                COALESCE(att.Penalty_PM, 0) AS Penalty_PM,
                                ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), @PM_Pen) AS PM_IN_Penalty,
                                ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), @PM_Pen) AS PM_OUT_Penalty,
                                UPPER(teach.Lastname) AS teachlname
                            FROM
                                tbl_events e
                            LEFT JOIN
                                tbl_student_accounts s ON CHARINDEX(s.FirstName + ' ' + s.LastName, e.Specific_Students) > 0
                            LEFT JOIN
                                tbl_attendance AS att ON s.ID = att.Student_ID AND e.Event_ID = att.Event_ID AND CONVERT(DATE, att.Date_Time) = @Date
                            LEFT JOIN
                                tbl_departments d ON s.Department = d.Department_ID
                            LEFT JOIN
                                tbl_Section AS sec ON s.Section = sec.Section_ID
                            LEFT JOIN
                                tbl_teacher_accounts AS teach ON att.Checker = teach.ID
                            WHERE
                                e.Event_ID = @EventID
                                AND e.Exclusive = 'Specific Students'
                                AND s.ID IS NOT NULL
                                AND s.Department = @Dep
                            ORDER BY
                                s.ID, att.Date_Time;";
                    }
                    else
                    {
                        query = @"SELECT stud.ID AS id, 
                                UPPER(stud.Firstname) AS fname,
                                UPPER(stud.Middlename) AS mname,
                                UPPER(stud.Lastname) AS lname,
                                sec.Description AS secdes,
                                FORMAT(att.Date_Time, 'yyyy-MM-dd') AS Date_Time,
                                COALESCE(att.Penalty_AM, 0) AS Penalty_AM,
                                ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), @AM_Pen) AS AM_IN_Penalty,
                                ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), @AM_Pen) AS AM_OUT_Penalty,
                                COALESCE(att.Penalty_PM, 0) AS Penalty_PM,
                                ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), @PM_Pen) AS PM_IN_Penalty,
                                ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), @PM_Pen) AS PM_OUT_Penalty,
                                UPPER(teach.Lastname) AS teachlname 
                                FROM tbl_student_accounts AS stud
                                LEFT JOIN tbl_attendance AS att ON stud.ID = att.Student_ID AND att.Event_ID = @EventID AND CONVERT(DATE, att.Date_Time) = @Date
                                LEFT JOIN tbl_Section AS sec ON stud.Section = sec.Section_ID
                                LEFT JOIN tbl_teacher_accounts AS teach ON att.Checker = teach.ID 
                                WHERE stud.Department = @Dep AND stud.Status = 1";
                    }

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        string modifiedStringAM = lblAmPenaltyFee.Text.Replace("₱ ", "");
                        string modifiedStringPM = lblPmPenaltyFee.Text.Replace("₱ ", "");
                        cmd.Parameters.AddWithValue("@EventID", event_id);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@AM_Pen", modifiedStringAM);
                        cmd.Parameters.AddWithValue("@PM_Pen", modifiedStringPM);
                        cmd.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int rowIndex = dgvRecord.Rows.Add(false);

                                string name = dr["fname"].ToString() + " " + dr["mname"].ToString() + " " + dr["lname"].ToString();

                                double totalfee = 0;

                                if (double.TryParse(dr["AM_IN_Penalty"].ToString(), out double amInPenalty) && !string.IsNullOrEmpty(dr["AM_IN"].ToString()))
                                    totalfee += amInPenalty;

                                if (double.TryParse(dr["AM_OUT_Penalty"].ToString(), out double amOutPenalty) && !string.IsNullOrEmpty(dr["AM_OUT"].ToString()))
                                    totalfee += amOutPenalty;

                                if (double.TryParse(dr["PM_IN_Penalty"].ToString(), out double pmInPenalty) && !string.IsNullOrEmpty(dr["PM_IN"].ToString()))
                                    totalfee += pmInPenalty;

                                if (double.TryParse(dr["PM_OUT_Penalty"].ToString(), out double pmOutPenalty) && !string.IsNullOrEmpty(dr["PM_OUT"].ToString()))
                                    totalfee += pmOutPenalty;

                                string formattedTotalFee = totalfee.ToString("F2");

                                string formattedDate = Dt.Value.ToString("MM-dd-yyyy");

                                dgvRecord.Rows[rowIndex].Cells["ID"].Value = dr["id"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["name"].Value = name;
                                dgvRecord.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["event_date"].Value = formattedDate;
                                dgvRecord.Rows[rowIndex].Cells["penalty_am"].Value = lblAmPenaltyFee.Text;
                                dgvRecord.Rows[rowIndex].Cells["am_login"].Value = dr["AM_IN"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["am_logout"].Value = dr["AM_OUT"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["penalty_pm"].Value = lblPmPenaltyFee.Text;
                                dgvRecord.Rows[rowIndex].Cells["pm_login"].Value = dr["PM_IN"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["pm_logout"].Value = dr["PM_OUT"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["checker"].Value = dr["teachlname"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["penalty_total"].Value = "₱ " + formattedTotalFee;

                                rowIndex++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private bool IsEventForSpecificStudents(string eventId)
        {
            // Implement your logic to check if the event is for Specific Students
            // For example, check if the Exclusive column is 'Specific Students'
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Exclusive FROM tbl_events WHERE Event_ID = @EventID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    object result = cmd.ExecuteScalar();
                    return result != null && result.ToString() == "Specific Students";
                }
            }
        }

        private void formAttendanceRecord_Load(object sender, EventArgs e)
        {
            displayFilter();
            displayFees();
            displayTable();
        }

        private void Dt_ValueChanged(object sender, EventArgs e)
        {
            date = Dt.Value.ToString();
            displayFees();
            displayTable();
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

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCBFilter(cbSection.Text);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplyCBFilter(searchKeyword);
        }

        private void btnPenaltyFee_Click(object sender, EventArgs e)
        {
            formConfigFee.displayCBData(cbEvents.Text);
            formConfigFee.getForm(this);
            formConfigFee.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            displayFees();
            displayTable();
        }

        private void ApplyCBFilter(string selectedIndex)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvRecord.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(selectedIndex, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break; // No need to check other cells in the row
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }
    }
}
