using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class formEventConfig : KryptonForm
    {
        string event_code = null;
        string att_status = null;

        DataSet ds = new DataSet();

        SqlConnection cnn;
        SqlCommand cm;
        SqlDataReader dr;
        private BackgroundWorker backgroundWorker;

        bool isFormAttendanceCheckerShown = false;
        public formEventConfig()
        {
            InitializeComponent();
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

        private void formAttendanceCheckerSettings_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void tbEventCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void cbAttendanceStat_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
       
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            
        }

        private void cbAttendanceStat_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        public void getAttendanceStatus(bool status)
        {
            tgbtnEnable_Att.Checked = status;
        }

        private void tgbtnEnable_Att_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void tbEventCode_KeyDown(object sender, KeyEventArgs e)
        {
         
        }

        private async void btnUpdateRecord_Click(object sender, EventArgs e)
        {
          
        }
       
        private async void btnUploadData_Click(object sender, EventArgs e)
        {
           
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {

        }
    }
}
