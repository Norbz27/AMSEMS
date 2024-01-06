using ComponentFactory.Krypton.Toolkit;
using PusherServer;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAcademicYearSetting : KryptonForm
    {
        private SqlConnection cn;
        private SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool showMessageOnToggle = false;
        private bool previousToggleState; // Store the previous toggle state
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public formAcademicYearSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            GetCurrentStatusFromDatabase();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            loadAcad(); // Load data

            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This method runs on the UI thread
            // Update the UI or perform other tasks after the background work completes
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
            }
        }

        private void ToggleAccountStatus(int newStatus)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("UpdateAccountStatus", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@NewValue", newStatus);
                        cmd.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    // Handle any database connection or query exceptions here
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GetCurrentStatusFromDatabase()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string checkQuery = @"
                        SELECT COUNT(*) FROM (
                            SELECT Status FROM tbl_teacher_accounts
                            UNION
                            SELECT Status FROM tbl_student_accounts
                            UNION
                            SELECT Status FROM tbl_sao_accounts
                            UNION
                            SELECT Status FROM tbl_guidance_accounts
                            UNION
                            SELECT Status FROM tbl_deptHead_accounts
                        ) AS CombinedRoles
                        WHERE Status = 2";

                    using (SqlCommand command = new SqlCommand(checkQuery, cn))
                    {
                        int inactiveAccountCount = (int)command.ExecuteScalar();

                        // Get the total count of all accounts across all tables
                        string totalCountQuery = @"
                            SELECT COUNT(*) FROM (
                                SELECT Status FROM tbl_teacher_accounts
                                UNION
                                SELECT Status FROM tbl_student_accounts
                                UNION
                                SELECT Status FROM tbl_sao_accounts
                                UNION
                                SELECT Status FROM tbl_guidance_accounts
                                UNION
                                SELECT Status FROM tbl_deptHead_accounts
                            ) AS CombinedRoles";

                        using (SqlCommand totalCommand = new SqlCommand(totalCountQuery, cn))
                        {
                            int totalAccountCount = (int)totalCommand.ExecuteScalar();

                            // If all accounts are inactive (Status = 2) and the total count matches the inactive count, set previousToggleState to true (ON).
                            if (inactiveAccountCount > 0 && inactiveAccountCount == totalAccountCount)
                            {
                                previousToggleState = true;
                            }
                            else
                            {
                                previousToggleState = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any database connection or query exceptions here
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tgbtnDisableAcc_CheckedChanged(object sender, EventArgs e)
        {
            bool accountDisable = tgbtnDisableAcc.Checked;
            int newStatus = accountDisable ? 2 : 1; // 2 for disabled, 1 for active

            if (accountDisable)
            {
                showMessageOnToggle = true;
            }
            else
            {
                showMessageOnToggle = false;
            }

            if (showMessageOnToggle)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to change the status?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Update the status in the database
                    ToggleAccountStatus(newStatus);
                }
                else
                {
                    // Set the toggle back to its previous state
                    tgbtnDisableAcc.Checked = previousToggleState;
                }

                // Reset the flag
                showMessageOnToggle = false;
            }
            else
            {
                // Update the status in the database
                ToggleAccountStatus(newStatus);
            }
        }

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            // Set the initial state of the toggle switch based on the previousToggleState
            tgbtnDisableAcc.Checked = previousToggleState;

            // Ensure that showMessageOnToggle is set to false when the form1 loads
            showMessageOnToggle = false;

            backgroundWorker.RunWorkerAsync();
        }

        private void btnEditAcad_Click(object sender, EventArgs e)
        {
            formNewAcadYear formNewAcadYear = new formNewAcadYear(this);
            formNewAcadYear.ShowDialog();
        }

        public void loadAcad()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Academic_Year_Start,Academic_Year_End from tbl_acad WHERE Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblAcadYear.Text = dr["Academic_Year_Start"].ToString() + "-" + dr["Academic_Year_End"].ToString();
                dr.Close();
                cm = new SqlCommand("Select * from tbl_Semester WHERE Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblTerAcadSem.Text = dr["Description"].ToString();
                dr.Close();
                cm = new SqlCommand("Select * from tbl_Quarter WHERE Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblshsAcadSem.Text = dr["Description"].ToString();
                dr.Close();
                cn.Close();
            }
        }

        private void formAcademicYearSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void btnEditSHSAcadSem_Click(object sender, EventArgs e)
        {
            formSemester_Quarter formSemester_Quarter = new formSemester_Quarter(this, true);
            formSemester_Quarter.ShowDialog();
        }

        private void btnEditTerAcadSem_Click(object sender, EventArgs e)
        {
            formSemester_Quarter formSemester_Quarter = new formSemester_Quarter(this, false);
            formSemester_Quarter.ShowDialog();
        }
    }
}
