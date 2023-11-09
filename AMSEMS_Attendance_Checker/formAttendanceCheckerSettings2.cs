using AMSEMS;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceCheckerSettings2 : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        string event_code = null;
        string att_status = null;

        SqlConnection cnn;
        SqlCommand cm;
        SqlDataReader dr;
        private BackgroundWorker backgroundWorker;

        formAttendanceChecker formAttendanceChecker; 
        bool isFormAttendanceCheckerShown = false;
        public formAttendanceCheckerSettings2()
        {
            InitializeComponent();
            sQLite_Connection = new SQLite_Connection("db_AMSEMS_CHECKER.db");
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
            DialogResult result = MessageBox.Show("Do you want to sync data from the cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptLoading.Visible = true; // Show loading image before starting the synchronization

                if (IsInternetConnected())
                {
                    try
                    {
                        using (cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            sQLite_Connection.ClearData();
                            await cnn.OpenAsync();
                            cm = new SqlCommand("SELECT Unique_ID, ID, RFID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime from tbl_student_accounts", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertStudentData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Program"].ToString(), dr["Section"].ToString(), dr["Year_Level"].ToString(), dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Event_ID, Event_Name, Start_Date, End_Date, Description, Color, Image from tbl_events", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertEventsData(dr["Event_ID"].ToString(), dr["Event_Name"].ToString(), dr["Start_Date"].ToString(), dr["End_Date"].ToString(), dr["Description"].ToString(), dr["Color"].ToString(), dr["Image"].ToString());
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
    }
}
