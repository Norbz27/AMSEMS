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

namespace AMSEMS.SubForms_Admin
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        String id;
        static FormAdminNavigation form;
        public formDashboard(String id)
        {
            InitializeComponent();

            using(cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_admin_accounts where ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
                this.id = id;
            }
        }
        public static void setForm(FormAdminNavigation form1)
        {
            form = form1;
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            DisplayData();
        }

        public void DisplayData()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                //Students
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_student_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActStud.Text = dr["countActive"].ToString() + " Active";
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_student_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacStud.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();

                //Teachers
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_teacher_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActTeach.Text = dr["countActive"].ToString() + " Active";
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_teacher_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacTeach.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();

                //Departments
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_deptHead_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActDep.Text = dr["countActive"].ToString() + " Active";
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_deptHead_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacDep.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();

                //Guidance
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_guidance_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActGui.Text = dr["countActive"].ToString() + " Active";
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_guidance_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacGui.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();

                //SAO
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_sao_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActSAO.Text = dr["countActive"].ToString() + " Active";
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_sao_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacSAO.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();
            }
        }

        private void btnViewStud_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Students.setAccountName("Students Account");
            SubForms_Admin.formAccounts_Students.setRole(5);
            form.OpenChildForm(new SubForms_Admin.formAccounts_Students());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForms_Admin.formAccounts_Students());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }

        private void btnViewTeachers_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Teachers.setAccountName("Teachers Accounts");
            SubForms_Admin.formAccounts_Teachers.setRole(6);
            form.OpenChildForm(new SubForms_Admin.formAccounts_Teachers());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }

        private void btnViewDep_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_DeptHead.setAccountName("Department Head Accounts");
            SubForms_Admin.formAcctounts_DeptHead.setRole(2);
            form.OpenChildForm(new SubForms_Admin.formAcctounts_DeptHead());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }

        private void btnViewGui_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_Guidance.setAccountName("Guidance Associate Accounts");
            SubForms_Admin.formAcctounts_Guidance.setRole(3);
            form.OpenChildForm(new SubForms_Admin.formAcctounts_Guidance());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }

        private void btnViewSAO_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_SAO.setAccountName("Student Affairs Officer Accounts");
            SubForms_Admin.formAccounts_SAO.setRole(4);
            form.OpenChildForm(new SubForms_Admin.formAccounts_SAO());
            form.isCollapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }

        private void btnAddStud_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formStudentForm formStudentForm = new formStudentForm();
            formStudentForm.setData2(5,"Submit",this, true);
            formStudentForm.ShowDialog();
        }
    }
}
