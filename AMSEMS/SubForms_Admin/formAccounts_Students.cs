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
    public partial class formAccounts_Students : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formAccounts_Students(String accountName, int role)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
            cn = new SqlConnection(SQL_Connection.connection);

        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            formStudentForm formStudentForm = new formStudentForm(5, "Submit");
            formStudentForm.ShowDialog();
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAddStudent, "Add Account");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export Excel File");

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cbProgram.Items.Clear();
                cbSection.Items.Clear();
                cbYearlvl.Items.Clear();
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_program", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbProgram.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Description from tbl_section", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbSection.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Description from tbl_year_level", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbYearlvl.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();
            }

            dgvStudents.Rows.Clear();
            displayTable();
        }

        public void displayTable()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                int count = 1;
                cn.Open();
                cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "inner join tbl_program as p on sa.Program = p.Program_ID inner join tbl_Section as se on sa.Section = se.Section_ID " +
                        "inner join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "inner join tbl_status as st on sa.Status = st.Status_ID", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["pDes"].ToString(), dr["sDes"].ToString(), dr["yDes"].ToString(), dr["stDes"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                formStudentForm formStudentForm = new formStudentForm(5, "Update");
                formStudentForm.getStudID(dgvStudents.Rows[e.RowIndex].Cells[1].Value.ToString());
                formStudentForm.ShowDialog();
            }
        }
    }
}
