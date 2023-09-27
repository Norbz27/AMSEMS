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

        public formArchiveSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormAdminNavigation.id;

            loadData();
        }

        public void loadData()
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
    }
}
