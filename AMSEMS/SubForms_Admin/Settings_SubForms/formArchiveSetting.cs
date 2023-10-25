using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Microsoft.VisualBasic.ApplicationServices;

namespace AMSEMS.SubForms_Admin
{
    public partial class formArchiveSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string id;
        private bool fileChosen = false;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public formArchiveSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormAdminNavigation.id;

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            loadData();

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

        public void loadData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_student_accounts", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedStud.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_teacher_accounts", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedTeach.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_sao_accounts", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedSAO.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_deptHead_accounts", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedDep.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_guidance_accounts", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedGui.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_archived_subjects", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblArchivedSub.Text = dr["cnt"].ToString() + " Archived";
                dr.Close();
                cn.Close();
            }
        }

        private void btnViewArcStud_Click(object sender, EventArgs e)
        {
            formArchived_Accounts_Students formArchived_Accounts_Students = new formArchived_Accounts_Students();
            formArchived_Accounts_Students.getForm(this);
            formArchived_Accounts_Students.ShowDialog();
        }

        private void btnViewArcDep_Click(object sender, EventArgs e)
        {
            formArchived_Accounts_DepHead formArchived_Accounts_DepHead = new formArchived_Accounts_DepHead();
            formArchived_Accounts_DepHead.getForm(this);
            formArchived_Accounts_DepHead.ShowDialog();
        }

        private void btnViewArcTeach_Click(object sender, EventArgs e)
        {
            formArchived_Accounts_Teachers formArchived_Accounts_Teachers = new formArchived_Accounts_Teachers();
            formArchived_Accounts_Teachers.getForm(this);
            formArchived_Accounts_Teachers.ShowDialog();
        }

        private void btnViewArcSAO_Click(object sender, EventArgs e)
        {
            formArchived_Accounts_SAO formArchived_Accounts_SAO = new formArchived_Accounts_SAO();
            formArchived_Accounts_SAO.getForm(this);
            formArchived_Accounts_SAO.ShowDialog();
        }

        private void btnViewArcGui_Click(object sender, EventArgs e)
        {
            formArchived_Accounts_Guidance formArchived_Accounts_Guidance = new formArchived_Accounts_Guidance();
            formArchived_Accounts_Guidance.getForm(this);
            formArchived_Accounts_Guidance.ShowDialog();
        }

        private void btnViewArcSub_Click(object sender, EventArgs e)
        {
            formArchived_Subjects formArchived_Subjects = new formArchived_Subjects();
            formArchived_Subjects.getForm(this);
            formArchived_Subjects.ShowDialog();
        }

        private void btnRetTeach_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();
                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_teacher_accounts";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_teacher_accounts SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}


                                // Insert the student record
                                string insertQuery = "SET IDENTITY_INSERT tbl_teacher_accounts ON; " +
                                    "INSERT INTO tbl_teacher_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime FROM tbl_archived_teacher_accounts;" +
                                    "SET IDENTITY_INSERT tbl_teacher_accounts OFF;";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_teacher_accounts";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }

        private void btnRetStud_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();

                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_student_accounts";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // No existing record, proceed with unarchiving
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_student_accounts SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}

                                // Insert the student record
                                string insertQuery = "SET IDENTITY_INSERT tbl_student_accounts ON; " +
                                    "INSERT INTO tbl_student_accounts (Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime) SELECT Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime FROM tbl_archived_student_accounts;" +
                                    "SET IDENTITY_INSERT tbl_student_accounts OFF;";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_student_accounts";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving record: " + ex.Message);
                }
            }
        }

        private void btnRetDep_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();
                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_sao_accounts";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // No existing record, proceed with unarchiving
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_deptHead_accounts SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}


                                // Insert the student record
                                string insertQuery = "SET IDENTITY_INSERT tbl_deptHead_accounts ON; " +
                                    "INSERT INTO tbl_deptHead_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime FROM tbl_archived_deptHead_accounts; " +
                                    "SET IDENTITY_INSERT tbl_deptHead_accounts OFF;";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_deptHead_accounts";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }

        private void btnRetSAO_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();
                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_sao_accounts";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // No existing record, proceed with unarchiving
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_sao_accounts SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}


                                // Insert the student record
                                string insertQuery = "SET IDENTITY_INSERT tbl_sao_accounts ON; " +
                                    "INSERT INTO tbl_sao_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime FROM tbl_archived_sao_accounts; " +
                                    "SET IDENTITY_INSERT tbl_sao_accounts OFF;";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_sao_accounts";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }

                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }

        private void btnRetGui_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();
                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_guidance_accounts";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // No existing record, proceed with unarchiving
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_guidance_accounts SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}


                                // Insert the student record
                                string insertQuery = "SET IDENTITY_INSERT tbl_guidance_accounts ON; " +
                                    "INSERT INTO tbl_guidance_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime FROM tbl_archived_guidance_accounts; " +
                                    "SET IDENTITY_INSERT tbl_guidance_accounts OFF;";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_guidance_accounts";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }

        private void btnRetSub_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    DialogResult result = MessageBox.Show("Are you sure to retrieve this subjects?", "Confirm Update", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        cn.Open();
                        // Check if a record with the same ID already exists in tbl_student_accounts
                        string checkExistingQuery = "SELECT COUNT(*) FROM tbl_subjects";
                        using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                        {
                            int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                            if (existingRecordCount == 0)
                            {
                                // No existing record, proceed with unarchiving
                                // Update the status to 1 before inserting
                                //string updateStatusQuery = "UPDATE tbl_archived_subjects SET Status = 1";
                                //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                                //{
                                //    updateStatusCommand.ExecuteNonQuery();
                                //}


                                // Insert the student record
                                string insertQuery =
                                    "INSERT INTO tbl_subjects (Course_code,Course_Description,Units,Image,Status,Academic_Level) SELECT Course_code,Course_Description,Units,Image,Status,Academic_Level FROM tbl_archived_subjects; ";
                                using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                                {
                                    sqlCommand.ExecuteNonQuery();
                                }

                                // Delete the record from the archived table
                                string deleteQuery = "DELETE FROM tbl_archived_subjects;";
                                using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                {
                                    command.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                // A record with the same ID already exists in tbl_student_accounts
                                MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                }
            }
        }

        private void formArchiveSetting_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void formArchiveSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }
    }
}
