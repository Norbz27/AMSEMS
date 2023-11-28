using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formStudentForm : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        string Pass;

        int roleID1;
        string choice1;
        bool istrue = false;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        formAccounts_Students form1;
        formDashboard form2;
        public formStudentForm()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

        }
        public void setData(int roleID, String choice, formAccounts_Students form)
        {
            form1 = form;
            roleID1 = roleID;
            choice1 = choice;
        }
        public void setData2(int roleID, String choice, formDashboard form, bool istrue)
        {
            form2 = form;
            roleID1 = roleID;
            choice1 = choice;
            this.istrue = istrue;
        }

        private void btnAddSection_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(this, new formTeacherForm());
            formAddSchoolSetting.setDisplayData("Section");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddYearLvl_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(this, new formTeacherForm());
            formAddSchoolSetting.setDisplayData("Year Level");
            formAddSchoolSetting.ShowDialog();
        }

        private void btnAddProgram_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(this, new formTeacherForm());
            formAddSchoolSetting.setDisplayData("Program");
            formAddSchoolSetting.ShowDialog();
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayPSY();
            int passwordLength = 12;
            if (choice1.Equals("Update"))
            {
                tbPass.Text = Pass;
                lblpassA.Hide();
                tbID.Enabled = false;
                tbRFID.Focus();
            }
            else
            {
                tbPass.Text = GeneratePassword(passwordLength);
                lblpassA.Show();
                tbID.Enabled = true;
            }

            btnSubmit.Text = choice1;
        }
        public void clearSetting()
        {
            cbProgram.Items.Clear();
            cbSection.Items.Clear();
            cbYearlvl.Items.Clear();
            cbDep.Items.Clear();
        }

        public void displayPSY()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
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

                cm = new SqlCommand("Select Description from tbl_year_level", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbYearlvl.Items.Add(dr["Description"].ToString());
                }
                dr.Close();

                cm = new SqlCommand("Select Description from tbl_Section", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbSection.Items.Add(dr["Description"].ToString());
                }
                dr.Close();

                cm = new SqlCommand("Select Description from tbl_Departments", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbDep.Items.Add(dr["Description"].ToString());
                }
                dr.Close();

                cm = new SqlCommand("Select Description from tbl_Role where Role_ID = " + roleID1 + "", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbRole.Text = dr["Description"].ToString();
                }
                dr.Close();
                cn.Close();
            }
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
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbFname.Text.Equals(String.Empty) || tbLname.Text.Equals(String.Empty) || tbMname.Text.Equals(String.Empty) || tbID.Text.Equals(String.Empty) || tbPass.Text.Equals(String.Empty) || tbRole.Text.Equals(String.Empty) || cbProgram.Text.Equals(String.Empty) || cbYearlvl.Text.Equals(String.Empty) || cbSection.Text.Equals(String.Empty) || cbDep.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tbPass.Text.Length <= 6)
                {
                    MessageBox.Show("Password should be above 6 characters!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (btnSubmit.Text.Equals("Update"))
                    {
                        cm = new SqlCommand("SELECT Profile_pic FROM tbl_student_accounts WHERE ID = @ID", cn);
                        cm.Parameters.AddWithValue("@ID", tbID.Text);

                        ad = new SqlDataAdapter(cm);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);

                        byte[] picData = null;

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // Read the image data from the retrieved row
                            picData = (byte[])ds.Tables[0].Rows[0]["Profile_pic"];
                        }

                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }
                        cn.Open();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "sp_UpdateAccounts";
                        cm.Parameters.AddWithValue("@ID", tbID.Text);
                        cm.Parameters.AddWithValue("@RFID", tbRFID.Text);
                        cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                        cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                        cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                        cm.Parameters.AddWithValue("@Password", tbPass.Text);
                        cm.Parameters.AddWithValue("@Profile_pic", picData);
                        cm.Parameters.AddWithValue("@Program", cbProgram.Text);
                        cm.Parameters.AddWithValue("@Section", cbSection.Text);
                        cm.Parameters.AddWithValue("@Year_Level", cbYearlvl.Text);
                        cm.Parameters.AddWithValue("@Department", cbDep.Text);
                        cm.Parameters.AddWithValue("@Role", tbRole.Text);
                        cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Account Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        cm = new SqlCommand("Select ID from tbl_student_accounts where ID = '" + tbID.Text + "'", cn);
                        ad = new SqlDataAdapter(cm);
                        ad.Fill(ds);
                        int i = ds.Tables[0].Rows.Count;

                        if (i != 0)
                        {
                            MessageBox.Show("An Account is already Present!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ds.Tables[0].Rows.Clear();
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
                            cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                            cm.Parameters.AddWithValue("@Password", tbPass.Text);
                            cm.Parameters.AddWithValue("@Profile_pic", picData);
                            cm.Parameters.AddWithValue("@Program", cbProgram.Text);
                            cm.Parameters.AddWithValue("@Section", cbSection.Text);
                            cm.Parameters.AddWithValue("@Year_Level", cbYearlvl.Text);
                            cm.Parameters.AddWithValue("@Department", cbDep.Text);
                            cm.Parameters.AddWithValue("@Role", tbRole.Text);
                            cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Account Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            ds.Tables[0].Rows.Clear();
                            clearTexts();
                        }
                    }
                }
                if (!istrue)
                    form1.displayTable();
                else
                {
                    form2.DisplayData();
                    form2.displayAccounts();
                    form2.displayChart();
                }

            }
        }
        public void clearTexts()
        {
            tbID.Text = "";
            tbFname.Text = "";
            tbMname.Text = "";
            tbLname.Text = "";
            tbPass.Text = "";
            tbRFID.Text = "";
            cbProgram.Text = "";
            cbSection.Text = "";
            cbYearlvl.Text = "";
            cbDep.Text = "";
            openFileDialog1.FileName = null;
            ptbProfile.Image = global::AMSEMS.Properties.Resources.man__3_;

            int passwordLength = 12;
            tbPass.Text = GeneratePassword(passwordLength);
        }

        public void getStudID(String ID)
        {
            using (cn)
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
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Middlename,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes, d.Description as depDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                        "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "left join tbl_Departments as d on sa.Department = d.Department_ID " +
                        "left join tbl_status as st on sa.Status = st.Status_ID where ID = " + ID + "", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    tbID.Text = dr["ID"].ToString();
                    tbRFID.Text = dr["RFID"].ToString();
                    tbFname.Text = dr["Firstname"].ToString();
                    tbLname.Text = dr["Lastname"].ToString();
                    tbMname.Text = dr["Middlename"].ToString();
                    Pass = dr["Password"].ToString();
                    cbProgram.Text = dr["pDes"].ToString();
                    cbSection.Text = dr["sDes"].ToString();
                    cbYearlvl.Text = dr["yDes"].ToString();
                    cbDep.Text = dr["depDes"].ToString();
                    tbStatus.Text = dr["stDes"].ToString();
                    dr.Close();
                    cn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void btnAddDep_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(this, new formTeacherForm());
            formAddSchoolSetting.setDisplayData("Departments");
            formAddSchoolSetting.ShowDialog();
        }

        private void formStudentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!istrue)
            {
                form1.displayFilter();
                form1.loadCMSControls();
            }

        }
    }
}
