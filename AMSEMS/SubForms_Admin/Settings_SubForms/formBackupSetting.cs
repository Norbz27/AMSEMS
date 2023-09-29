using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AMSEMS.SubForms_Admin
{
    public partial class formBackupSetting : KryptonForm
    {
        private SqlConnection cn;
        private SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool showMessageOnToggle = false;
        private bool previousToggleState; // Store the previous toggle state

        public formBackupSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            progressBar.Visible = false;
        }
        
        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {

        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                // Set the save dialog properties
                saveFileDialog.Filter = "SQL Server Backup Files (*.bak)|*.bak";
                saveFileDialog.Title = "Backup Database";
                saveFileDialog.FileName = "AMSEMS"; // Default file name

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Start the background worker to perform the backup
                    progressBar.Style = ProgressBarStyle.Marquee;
                    progressBar.Visible = true;
                    backgroundWorker.RunWorkerAsync(saveFileDialog.FileName);
                }
            }
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method will run in the background thread
            string backupFilePath = (string)e.Argument;
            string databaseName = "db_Amsems"; // Replace with your database name

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                {
                    connection.Open();

                    string backupQuery = $"BACKUP DATABASE {databaseName} TO DISK = '{backupFilePath}'";
                    using (SqlCommand command = new SqlCommand(backupQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                e.Result = ex.Message;
            }
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This method runs when the background worker completes its work
            progressBar.Visible = false;

            if (e.Result != null)
            {
                MessageBox.Show($"Error: {e.Result.ToString()} Save it on other location", "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Database backup completed successfully!", "Backup Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set the open dialog properties
                openFileDialog.Filter = "SQL Server Backup Files (*.bak)|*.bak";
                openFileDialog.Title = "Select Database Backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Start the background worker to perform the restore
                    progressBar.Style = ProgressBarStyle.Marquee;
                    progressBar.Visible = true;
                    backgroundWorker.RunWorkerAsync(openFileDialog.FileName);
                }
            }
        }
    }
}
