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
    public partial class formStudentForm : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formStudentForm()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void btnAddSection_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Section");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddYearLvl_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Year Level");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Program");
            formAddSchoolSetting.ShowDialog();
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayPSY();
        }
        
        public void displayPSY()
        {
            cn.Open();
            cm = new SqlCommand("Select Description from tbl_program", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                ctMProgram.Items.Add(dr["Description"].ToString());
            }
            dr.Close();
            cn.Close();

            cn.Open();
            cm = new SqlCommand("Select Description from tbl_year_level", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                ctMYlvl.Items.Add(dr["Description"].ToString());
            }
            dr.Close();
            cn.Close();

            cn.Open();
            cm = new SqlCommand("Select Description from tbl_Section", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                ctMSection.Items.Add(dr["Description"].ToString());
            }
            dr.Close();
            cn.Close();
        }
    }
}
