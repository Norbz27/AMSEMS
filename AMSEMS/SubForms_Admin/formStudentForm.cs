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

        int roleID;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        public formStudentForm(int roleID)
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

            this.roleID = roleID;
            
        }

        private void btnAddSection_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Section", this);
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddYearLvl_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Year Level", this);
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting("Program", this);
            formAddSchoolSetting.ShowDialog();
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayPSY();
            int passwordLength = 12;
            tbPass.Text = GeneratePassword(passwordLength);
        }
        public void clear()
        {
            cbProgram.Items.Clear();
            cbSection.Items.Clear();
            cbYearlvl.Items.Clear();
        }

        public void displayPSY()
        {
            clear();
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
            cm = new SqlCommand("Select Description from tbl_year_level", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbYearlvl.Items.Add(dr["Description"].ToString());
            }
            dr.Close();
            cn.Close();

            cn.Open();
            cm = new SqlCommand("Select Description from tbl_Section", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                cbSection.Items.Add(dr["Description"].ToString());
            }
            dr.Close();
            cn.Close();

            cn.Open();
            cm = new SqlCommand("Select Description from tbl_Role where Role_ID = "+ roleID +"", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                tbRole.Text = dr["Description"].ToString();
            }
            dr.Close();
            cn.Close();
        }

        private void cbProgram_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ptbProfile.Image = Image.FromFile(openFileDialog1.FileName);
        }

        string GeneratePassword(int length)
        {
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = RandomGenerator.Next(AllowedChars.Length);
                password.Append(AllowedChars[randomIndex]);
            }

            return password.ToString();
        }
    }
}
