using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceChecker : KryptonForm
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;

        public bool isCollapsed;
        public formAttendanceChecker()
        {
            InitializeComponent();
            this.splitContainer1.Panel2Collapsed = true;
            isCollapsed = true;
      
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CollapseForm();
        }

        public void CollapseForm()
        {
            if (isCollapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
                timer1.Stop();
                isCollapsed = false;
                
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                timer1.Stop();
                isCollapsed = true;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            formAttendanceCheckerSettings formAttendanceCheckerSettings = new formAttendanceCheckerSettings();
            formAttendanceCheckerSettings.ShowDialog();
        }

        private void formAttendanceChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void formAttendanceChecker_Load(object sender, EventArgs e)
        {
            formAttendanceCheckerSettings formAttendanceCheckerSettings = new formAttendanceCheckerSettings();
            formAttendanceCheckerSettings.ShowDialog();
        }
    }
}
