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
            formStudentForm formStudentForm = new formStudentForm(5);
            formStudentForm.ShowDialog();
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            dgvStudents.Rows.Clear();
            displayTable();
        }

        public void displayTable()
        {
            int count = 1;
            cn.Open();
            cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Program,Section,Year_Level,Role,Status from tbl_student_accounts", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Program"].ToString(), dr["Section"].ToString(), dr["Year_Level"].ToString(), dr["Status"].ToString());
            }
            dr.Close();
            cn.Close();
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                formStudentForm formStudentForm = new formStudentForm(5);
                formStudentForm.getStudID(dgvStudents.Rows[e.RowIndex].Cells[1].Value.ToString());
                formStudentForm.ShowDialog();
            }
        }
    }
}
