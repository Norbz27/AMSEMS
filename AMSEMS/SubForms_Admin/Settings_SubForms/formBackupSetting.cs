using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                    progressBar.Style = ProgressBarStyle.Continuous;
                    progressBar.Visible = true;
                    backgroundWorker.RunWorkerAsync(new BackupRestoreArgs(saveFileDialog.FileName, isBackup: true));
                }
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
                    backgroundWorker.RunWorkerAsync(new BackupRestoreArgs(openFileDialog.FileName, isBackup: false));
                }
            }
        }

        private async void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            await Task.Delay(2000);
            // This method will run in the background thread
            var args = (BackupRestoreArgs)e.Argument;
            string filePath = args.FilePath;
            string databaseName = "db_Amsems"; // Replace with your database name

            try
            {
                using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                {
                    connection.Open();

                    if (args.IsBackup)
                    {
                        // Backup
                        string backupQuery = $"BACKUP DATABASE {databaseName} TO DISK = '{filePath}'";
                        using (SqlCommand command = new SqlCommand(backupQuery, connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // Restore
                        // Kill all the active connections to the database before restoring
                        string killConnectionsQuery = $"ALTER DATABASE {databaseName} SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                        using (SqlCommand killConnectionsCommand = new SqlCommand(killConnectionsQuery, connection))
                        {
                            killConnectionsCommand.ExecuteNonQuery();
                        }

                        // Restore the database
                        string restoreQuery = $"RESTORE DATABASE {databaseName} FROM DISK = '{filePath}' WITH REPLACE";
                        using (SqlCommand restoreCommand = new SqlCommand(restoreQuery, connection))
                        {
                            restoreCommand.ExecuteNonQuery();
                        }

                        // Set the database back to multi-user mode
                        string setMultiUserQuery = $"ALTER DATABASE {databaseName} SET MULTI_USER";
                        using (SqlCommand setMultiUserCommand = new SqlCommand(setMultiUserQuery, connection))
                        {
                            setMultiUserCommand.ExecuteNonQuery();
                        }
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
                MessageBox.Show($"Error: {e.Result.ToString()} Save it on other location", "Operation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Use e.Result to access the argument passed to the DoWork event
                var args = (BackupRestoreArgs)e.Result;
                MessageBox.Show($"{(args.IsBackup ? "Backup" : "Restore")} completed successfully!", "Operation Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Custom class to pass arguments to the BackgroundWorker
        private class BackupRestoreArgs
        {
            public string FilePath { get; }
            public bool IsBackup { get; }

            public BackupRestoreArgs(string filePath, bool isBackup)
            {
                FilePath = filePath;
                IsBackup = isBackup;
            }
        }
    }
}
