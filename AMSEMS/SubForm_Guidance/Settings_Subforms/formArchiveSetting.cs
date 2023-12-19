using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
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
            }
        }

        private void btnViewArcSHS_Click(object sender, EventArgs e)
        {
            formArchived_AbsReport formArchived_AbsReport = new formArchived_AbsReport("SHS");
            formArchived_AbsReport.ShowDialog();
        }

        private void btnViewArcTer_Click(object sender, EventArgs e)
        {
            formArchived_AbsReport formArchived_AbsReport = new formArchived_AbsReport("Tertiary");
            formArchived_AbsReport.ShowDialog();
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
