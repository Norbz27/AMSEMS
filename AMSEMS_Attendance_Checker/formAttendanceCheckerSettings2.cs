using AMSEMS;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceCheckerSettings2 : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        string event_code = null;
        string att_status = null;

        DataSet ds = new DataSet();

        SqlConnection cnn;
        SqlCommand cm;
        SqlDataReader dr;
        private BackgroundWorker backgroundWorker;

        formAttendanceChecker formAttendanceChecker;
        bool isFormAttendanceCheckerShown = false;
        public formAttendanceCheckerSettings2()
        {
            InitializeComponent();
            sQLite_Connection = new SQLite_Connection();
            cnn = new SqlConnection(SQL_Connection.connection);

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                ptLoading.Visible = true;
            });
        }

        // BackgroundWorker RunWorkerCompleted event handler
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ptLoading.Visible = false;
        }
        private void formAttendanceCheckerSettings_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbAttendanceStat.Text) && !cbAttendanceStat.Text.Equals(att_status) || !string.IsNullOrEmpty(tbEventCode.Text) && !tbEventCode.Text.Equals(event_code))
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }
        public void getForm(formAttendanceChecker formAttendanceChecker)
        {
            this.formAttendanceChecker = formAttendanceChecker;
        }
        public void getSetting(string code, string status)
        {
            tbEventCode.Text = code;
            cbAttendanceStat.Text = status;
            event_code = code;
            att_status = status;
        }

        private void formAttendanceCheckerSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(tbEventCode.Text) && string.IsNullOrEmpty(cbAttendanceStat.Text))
            {
                MessageBox.Show("Please fill in the Event Code.", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else
            {
                string event_name = sQLite_Connection.GetEvent(tbEventCode.Text);

                if (string.IsNullOrEmpty(event_name))
                {
                    MessageBox.Show("Invalid event code. Please enter a valid event code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true; // Prevent the form from closing
                }
                else
                {
                    formAttendanceChecker.getAttendanceSettings(cbAttendanceStat.Text, tbEventCode.Text);
                }

            }
        }

        private void tbEventCode_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbEventCode.Text) && !tbEventCode.Text.Equals(event_code))
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }

        private void cbAttendanceStat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbAttendanceStat.Text) && !cbAttendanceStat.Text.Equals(att_status))
            {
                btnSave.Visible = true;
                btnCancel.Visible = true;
            }
            else
            {
                btnSave.Visible = false;
                btnCancel.Visible = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string code = tbEventCode.Text;

            // Check if the event code is valid
            if (!string.IsNullOrWhiteSpace(code))
            {
                string event_name = sQLite_Connection.GetEvent(code);

                if (!string.IsNullOrEmpty(event_name))
                {
                    formAttendanceChecker.getAttendanceSettings(cbAttendanceStat.Text, tbEventCode.Text);
                    formAttendanceChecker.setEvent(event_name);
                    formAttendanceChecker.displayAttendanceRecord();
                    formAttendanceChecker.displayStudents();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Invalid event code. Please enter a valid event code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please enter an event code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            cbAttendanceStat.Text = String.Empty;
            tbEventCode.Text = String.Empty;
        }

        private void cbAttendanceStat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        public void getAttendanceStatus(bool status)
        {
            tgbtnEnable_Att.Checked = status;
        }

        private void tgbtnEnable_Att_CheckedChanged(object sender, EventArgs e)
        {
            bool accountDisable = tgbtnEnable_Att.Checked;

            if (accountDisable)
            {
                formAttendanceChecker.SetAttendanceEnabled(true);
            }
            else
            {
                formAttendanceChecker.SetAttendanceEnabled(false);
            }
        }

        private void tbEventCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string code = tbEventCode.Text;

                // Check if the event code is valid
                if (!string.IsNullOrWhiteSpace(code))
                {
                    string event_name = sQLite_Connection.GetEvent(code);

                    if (!string.IsNullOrEmpty(event_name))
                    {
                        formAttendanceChecker.getAttendanceSettings(cbAttendanceStat.Text, tbEventCode.Text);
                        formAttendanceChecker.setEvent(event_name);
                        formAttendanceChecker.displayAttendanceRecord();
                        formAttendanceChecker.displayStudents();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid event code. Please enter a valid event code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please enter an event code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private async void btnUpdateRecord_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to update record from the cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptLoading.Visible = true; // Show loading image before starting the synchronization
                await Task.Delay(3000);
                if (IsInternetConnected())
                {
                    try
                    {
                        using (cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            sQLite_Connection.ClearData();
                            sQLite_Connection.ClearEventData();
                            await cnn.OpenAsync();
                            cm = new SqlCommand("SELECT Unique_ID, ID, RFID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime from tbl_student_accounts", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertStudentData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Program"].ToString(), dr["Section"].ToString(), dr["Year_Level"].ToString(), dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Event_ID, Event_Name, Start_Date, End_Date, Description, Color, Image, Attendance, Exclusive, Specific_Students, Selected_Departments from tbl_events", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertEventsData(dr["Event_ID"].ToString(), dr["Event_Name"].ToString(), dr["Start_Date"].ToString(), dr["End_Date"].ToString(), dr["Description"].ToString(), dr["Color"].ToString(), dr["Image"].ToString(), dr["Attendance"].ToString(), dr["Exclusive"].ToString(), dr["Specific_Students"].ToString(), dr["Selected_Departments"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Program_ID, Description from tbl_program", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertProgramData(dr["Program_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Section_ID, Description from tbl_section", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertSectionData(dr["Section_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Level_ID, Description from tbl_year_level", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertYearLevelData(dr["Level_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Department_ID, Description from tbl_departments", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertDepartmentData(dr["Department_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            await Task.Delay(3000);
                            MessageBox.Show("Successfully Sync Data.", "Sync Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        formAttendanceChecker.displayStudents();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No internet connection available. Please check your network connection.");
                }
                ptLoading.Visible = false;
            }
        }
        private bool IsInternetConnected()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("www.google.com"); // You can use a reliable external host for testing connectivity.

                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true; // Internet is reachable.
                }
                return false; // No internet connection.
            }
            catch
            {
                return false;
            }
        }

        private async void btnUploadData_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to upload attendance record to cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptLoading.Visible = true; // Show loading image before starting the synchronization
                await Task.Delay(3000);
                if (IsInternetConnected())
                {
                    try
                    {
                        using (cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            await cnn.OpenAsync();
                            DataTable attendance_record = sQLite_Connection.GetAttendanceRecord();

                            if (attendance_record.Rows.Count > 0)
                            {
                                foreach (DataRow row in attendance_record.Rows)
                                {
                                    string studID = row["Student_ID"].ToString();
                                    string eventID = row["Event_ID"].ToString();

                                    string formattedDateTime = row["Date_Time"] != DBNull.Value ? row["Date_Time"].ToString() : null;
                                    string amIn = row["AM_IN"] != DBNull.Value ? row["AM_IN"].ToString() : null;
                                    string amOut = row["AM_OUT"] != DBNull.Value ? row["AM_OUT"].ToString() : null;
                                    string pmIn = row["PM_IN"] != DBNull.Value ? row["PM_IN"].ToString() : null;
                                    string pmOut = row["PM_OUT"] != DBNull.Value ? row["PM_OUT"].ToString() : null;
                                    string checker = row["Checker"].ToString();

                                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbl_attendance WHERE Student_ID = @StudentID AND Event_ID = @EventID AND CONVERT(DATE, Date_Time) = CONVERT(DATE, @DateTime)", cnn))
                                    {
                                        checkCmd.Parameters.AddWithValue("@StudentID", studID);
                                        checkCmd.Parameters.AddWithValue("@EventID", eventID);
                                        checkCmd.Parameters.AddWithValue("@DateTime", formattedDateTime);

                                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                                        if (recordCount == 0)
                                        {
                                            // If the record doesn't exist, insert it
                                            string insertQuery = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, AM_IN, AM_OUT, PM_IN, PM_OUT, Checker) VALUES (@StudentID, @EventID, @DateTime, @AmIn, @AmOut, @PmIn, @PmOut, @Checker)";

                                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, cnn))
                                            {
                                                insertCmd.Parameters.AddWithValue("@StudentID", studID);
                                                insertCmd.Parameters.AddWithValue("@EventID", eventID);
                                                insertCmd.Parameters.AddWithValue("@DateTime", string.IsNullOrEmpty(formattedDateTime) ? (object)DBNull.Value : DateTime.Parse(formattedDateTime));
                                                insertCmd.Parameters.AddWithValue("@AmIn", string.IsNullOrEmpty(amIn) ? (object)DBNull.Value : DateTime.Parse(amIn));
                                                insertCmd.Parameters.AddWithValue("@AmOut", string.IsNullOrEmpty(amOut) ? (object)DBNull.Value : DateTime.Parse(amOut));
                                                insertCmd.Parameters.AddWithValue("@PmIn", string.IsNullOrEmpty(pmIn) ? (object)DBNull.Value : DateTime.Parse(pmIn));
                                                insertCmd.Parameters.AddWithValue("@PmOut", string.IsNullOrEmpty(pmOut) ? (object)DBNull.Value : DateTime.Parse(pmOut));
                                                insertCmd.Parameters.AddWithValue("@Checker", checker);

                                                insertCmd.ExecuteNonQuery();
                                            }
                                        }
                                        else
                                        {
                                            // If the record exists, update it
                                            string updateQuery = "UPDATE tbl_attendance SET " +
                                                "AM_IN = ISNULL(AM_IN, @AmIn), " +
                                                "AM_OUT = ISNULL(AM_OUT, @AmOut), " +
                                                "PM_IN = ISNULL(PM_IN, @PmIn), " +
                                                "PM_OUT = ISNULL(PM_OUT, @PmOut) " +
                                                "WHERE Student_ID = @StudentID AND Event_ID = @EventID AND Date_Time = @DateTime";

                                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, cnn))
                                            {
                                                updateCmd.Parameters.AddWithValue("@StudentID", studID);
                                                updateCmd.Parameters.AddWithValue("@EventID", eventID);
                                                insertCmd.Parameters.AddWithValue("@DateTime", string.IsNullOrEmpty(formattedDateTime) ? (object)DBNull.Value : DateTime.Parse(formattedDateTime));
                                                updateCmd.Parameters.AddWithValue("@AmIn", string.IsNullOrEmpty(amIn) ? (object)DBNull.Value : DateTime.Parse(amIn));
                                                updateCmd.Parameters.AddWithValue("@AmOut", string.IsNullOrEmpty(amOut) ? (object)DBNull.Value : DateTime.Parse(amOut));
                                                updateCmd.Parameters.AddWithValue("@PmIn", string.IsNullOrEmpty(pmIn) ? (object)DBNull.Value : DateTime.Parse(pmIn));
                                                updateCmd.Parameters.AddWithValue("@PmOut", string.IsNullOrEmpty(pmOut) ? (object)DBNull.Value : DateTime.Parse(pmOut));

                                                updateCmd.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                }



                                MessageBox.Show("Successfully Upload Data.", "Sync Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("No internet connection available. Please check your network connection.");
                }
                ptLoading.Visible = false;
            }
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            formAttendanceChecker.Logout();
        }
    }
}
