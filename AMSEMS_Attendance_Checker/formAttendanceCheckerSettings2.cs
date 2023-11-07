using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceCheckerSettings2 : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        string event_code = null;

        formAttendanceChecker formAttendanceChecker; 
        bool isFormAttendanceCheckerShown = false;
        public formAttendanceCheckerSettings2()
        {
            InitializeComponent();
            sQLite_Connection = new SQLite_Connection("db_AMSEMS_CHECKER.db");
        }

        private void formAttendanceCheckerSettings_Load(object sender, EventArgs e)
        {

        }
        public void getForm(formAttendanceChecker formAttendanceChecker)
        {
            this.formAttendanceChecker = formAttendanceChecker;
        }
        public void getSetting(string code, string status)
        {
            tbEventCode.Text = code;
            cbAttendanceStat.Text = status;
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
            if (!string.IsNullOrEmpty(tbEventCode.Text))
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
            if (!string.IsNullOrEmpty(cbAttendanceStat.Text))
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
    }
}
