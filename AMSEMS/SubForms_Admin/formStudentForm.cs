using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
        public void clearSetting()
        {
            cbProgram.Items.Clear();
            cbSection.Items.Clear();
            cbYearlvl.Items.Clear();
        }

        public void displayPSY()
        {
            clearSetting();
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (tbFname.Text == "" && tbLname.Text == "" && tbMname.Text == "" && tbID.Text == "" && tbPass.Text == "" && tbRole.Text == "" && cbProgram.Text == "" && cbYearlvl.Text == "" && cbSection.Text == "")
            {
                MessageBox.Show("Empty Fields!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                byte[] picData;
                if (openFileDialog1.FileName == String.Empty)
                {
                    picData = null;
                }
                else
                {
                    picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                }
                cn.Open();
                cm = new SqlCommand();
                cm.Connection = cn;
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "sp_AddAccounts";
                cm.Parameters.AddWithValue("@ID", tbID.Text);
                cm.Parameters.AddWithValue("@RFID", tbRFID.Text);
                cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                cm.Parameters.AddWithValue("@Password", tbPass.Text);
                cm.Parameters.AddWithValue("@Profile_pic", picData);
                cm.Parameters.AddWithValue("@Program", cbProgram.Text);
                cm.Parameters.AddWithValue("@Section", cbSection.Text);
                cm.Parameters.AddWithValue("@Year_Level", cbYearlvl.Text);
                cm.Parameters.AddWithValue("@Role", tbRole.Text);
                cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                cm.ExecuteNonQuery();
                cn.Close();
                MessageBox.Show("Account Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearTexts();
            }   
        }
        public void clearTexts()
        {
            tbID.Text = "";
            tbMname.Text = "";
            tbLname.Text = "";
            tbPass.Text = "";
            tbRFID.Text = "";
            cbProgram.Text = "";
            cbSection.Text = "";
            cbYearlvl.Text = "";
            openFileDialog1.FileName = null;
            ptbProfile.Image = global::AMSEMS.Properties.Resources.man__3_;
        }

        public void getStudID(String ID)
        {
            try
            {
                cn.Open();
                cm = new SqlCommand("Select Profile_pic from tbl_student_accounts where ID = " + ID + "", cn);

                byte[] imageData = (byte[])cm.ExecuteScalar();

                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);
                        ptbProfile.Image = image;
                    }
                }
                else
                {
                    // Display a default image or handle the case where there's no image in the database
                }
                cn.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
