using ComponentFactory.Krypton.Toolkit;
using System;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceCheckerSettings : KryptonForm
    {
        formAttendanceChecker formAttendanceChecker;
        SQLite_Connection sQLite_Connection;
        string event_code = null;
        string teach_id;
        private bool showMessageOnToggle = false;
        public formAttendanceCheckerSettings()
        {
            InitializeComponent();
            formAttendanceChecker = new formAttendanceChecker();
            sQLite_Connection = new SQLite_Connection();
        }
        public void getTeachID(string id)
        {
            teach_id = id;
        }

        private void formAttendanceCheckerSettings_Load(object sender, EventArgs e)
        {

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
                    formAttendanceChecker.getTeachID(teach_id);
                    formAttendanceChecker.Show();
                }

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

        private void tbEventCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
