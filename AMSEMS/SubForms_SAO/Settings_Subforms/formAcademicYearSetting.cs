using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
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
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            loadAcad();

            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
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

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        public void loadAcad()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Academic_Year_Start,Academic_Year_End,Academic_Sem from tbl_acad where Acad_ID = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblAcadYear.Text = dr["Academic_Year_Start"].ToString() + "-" + dr["Academic_Year_End"].ToString();
                lblAcadSem.Text = dr["Academic_Sem"].ToString();
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
    }
}
