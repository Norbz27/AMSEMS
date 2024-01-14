using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formSchoolDetails : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string id;
        private bool fileChosen = false;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public formSchoolDetails()
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
                cm = new SqlCommand("Select count(*) as cnt from tbl_Departments", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblDep.Text = dr["cnt"].ToString() + " Total";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_program", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblProgram.Text = dr["cnt"].ToString() + " Total";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_Year_Level", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblLevel.Text = dr["cnt"].ToString() + " Total";
                dr.Close();

                cm = new SqlCommand("Select count(*) as cnt from tbl_section", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblSec.Text = dr["cnt"].ToString() + " Total";
                dr.Close();
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

        private void btnProgram_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(new formStudentForm(), new formTeacherForm(), this);
            formAddSchoolSetting.setDisplayData("Program");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnSec_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(new formStudentForm(), new formTeacherForm(), this);
            formAddSchoolSetting.setDisplayData("Section");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnDep_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting2 formAddSchoolSetting = new formAddSchoolSetting2(new formStudentForm(), new formTeacherForm(), new formSubjectsForm(), this);
            formAddSchoolSetting.setDisplayData("Departments");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting2 formAddSchoolSetting = new formAddSchoolSetting2(new formStudentForm(), new formTeacherForm(), new formSubjectsForm(), this);
            formAddSchoolSetting.setDisplayData("Year Level");
            formAddSchoolSetting.ShowDialog();
        }
    }
}
