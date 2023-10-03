﻿using ComponentFactory.Krypton.Toolkit;
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
        public String id;
        public static String id2 { get; set; }
        static FormAdminNavigation form;
        public formDashboard(String id1)
        {

            InitializeComponent();

            id = id1;
        }
        public static void setForm(FormAdminNavigation form1)
        {
            form = form1;
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            if(id.Equals(String.Empty))
                loadData(id2);
            else
                loadData(id);
            DisplayData();

            displayChart();
            displayAccounts();
            form.loadData();
        }

        public void displayChart()
        {
            chart1.Invalidate();
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                chart1.Series["s1"].IsValueShownAsLabel = true;
                string query = "SELECT d.Description as Dep, COUNT(*) AS StudentCount FROM tbl_student_accounts as sa inner join tbl_Departments as d on sa.Department = d.Department_ID GROUP BY d.Description";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    // Execute the query and retrieve data
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve data and display it on the chart
                            string program = reader["Dep"].ToString();
                            int studentCount = Convert.ToInt32(reader["StudentCount"]);

                            // Add data to the chart
                            chart1.Series[0].Points.AddXY(program, studentCount);
                        }
                    }
                }
            }
        }

        public void loadData(String id)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_admin_accounts where Unique_ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();

            }
        }
        public void displayAccounts()
        {
            dgvAccounts.Rows.Clear();
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("SELECT TOP 10 ca.Firstname as Fname, ca.Lastname as Lname, r.Description AS RoleDescription, ca.DateTime FROM (SELECT Firstname, Lastname, Role, DateTime FROM tbl_student_accounts UNION SELECT Firstname, Lastname, Role, DateTime FROM tbl_deptHead_accounts UNION SELECT Firstname, Lastname, Role, DateTime FROM tbl_guidance_accounts UNION SELECT Firstname, Lastname, Role, DateTime FROM tbl_sao_accounts UNION SELECT Firstname, Lastname, Role, DateTime FROM tbl_teacher_accounts) AS ca JOIN tbl_role AS r ON ca.Role = r.Role_ID ORDER BY ca.DateTime DESC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvAccounts.Rows.Add(
                        dr["Fname"].ToString() + " " + dr["Lname"].ToString(),
                        dr["RoleDescription"].ToString()
                    );
                }
                dr.Close();
                cn.Close();
            }
        }
        public void DisplayData()
        {
            displayAccounts();

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                //Students
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_student_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActStud.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_student_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacStud.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();

                //Teachers

                cm = new SqlCommand("select count(*) as countActive from tbl_teacher_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActTeach.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_teacher_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacTeach.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();

                //Departments
                cm = new SqlCommand("select count(*) as countActive from tbl_deptHead_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActDep.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_deptHead_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacDep.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();

                //Guidance
                cm = new SqlCommand("select count(*) as countActive from tbl_guidance_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActGui.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_guidance_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacGui.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();

                //SAO
                cm = new SqlCommand("select count(*) as countActive from tbl_sao_accounts where Status = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActSAO.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_sao_accounts where Status = 2", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacSAO.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();
            }
        }

        public void controls()
        {
            form.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            form.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            form.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            form.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            form.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            form.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnViewStud_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Students.setAccountName("Students Account");
            SubForms_Admin.formAccounts_Students.setRole(5);
            form.OpenChildForm(new SubForms_Admin.formAccounts_Students());
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Students.setAccountName("Students Account");
            SubForms_Admin.formAccounts_Students.setRole(5);
            form.OpenChildForm(new SubForms_Admin.formAccounts_Students());
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void btnViewTeachers_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Teachers.setAccountName("Teachers Accounts");
            SubForms_Admin.formAccounts_Teachers.setRole(6);
            form.OpenChildForm(new SubForms_Admin.formAccounts_Teachers());
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void btnViewDep_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_DeptHead.setAccountName("Department Head Accounts");
            SubForms_Admin.formAcctounts_DeptHead.setRole(2);
            form.OpenChildForm(new SubForms_Admin.formAcctounts_DeptHead());
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void btnViewGui_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_Guidance.setAccountName("Guidance Associate Accounts");
            SubForms_Admin.formAcctounts_Guidance.setRole(3);
            form.OpenChildForm(new SubForms_Admin.formAcctounts_Guidance());
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void btnViewSAO_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_SAO.setAccountName("Student Affairs Officer Accounts");
            SubForms_Admin.formAccounts_SAO.setRole(4);
            form.OpenChildForm(new SubForms_Admin.formAccounts_SAO());
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void btnAddStud_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formStudentForm formStudentForm = new formStudentForm();
            formStudentForm.setData2(5,"Submit",this, true);
            formStudentForm.ShowDialog();
        }

        private void btnAddTeach_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formTeacherForm formTeacherForm = new formTeacherForm();
            formTeacherForm.setData3(6, "Submit", this, true);
            formTeacherForm.ShowDialog();
        }

        private void btnAddDep_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formTeacherForm formTeacherForm = new formTeacherForm();
            formTeacherForm.setData3(2, "Submit", this, true);
            formTeacherForm.ShowDialog();
        }

        private void btnAddGui_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formGeneratedForm formGeneratedForm = new formGeneratedForm();
            formGeneratedForm.setData3(3, "Submit", this, true);
            formGeneratedForm.ShowDialog();
        }

        private void btnAddSAO_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formGeneratedForm formGeneratedForm = new formGeneratedForm();
            formGeneratedForm.setData3(4, "Submit", this, true);
            formGeneratedForm.ShowDialog();
        }
    }
}
