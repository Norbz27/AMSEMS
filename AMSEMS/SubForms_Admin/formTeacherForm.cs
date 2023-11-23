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
    public partial class formTeacherForm : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        String Pass;

        int roleID;
        String choice;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        formAccounts_Teachers form;
        formAcctounts_DeptHead form2;
        formDashboard form3;

        bool istrue = false;
        public formTeacherForm()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

        }
        public void setData(int roleID, String choice, formAccounts_Teachers form)
        {
            this.form = form;
            this.roleID = roleID;
            this.choice = choice;
        }

        public void setData2(int roleID, String choice, formAcctounts_DeptHead form)
        {
            this.form2 = form;
            this.roleID = roleID;
            this.choice = choice;
        }
        public void setData3(int roleID, String choice, formDashboard form, bool istrue)
        {
            form3 = form;
            this.roleID = roleID;
            this.choice = choice;
            this.istrue = istrue;
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayDept();
            if (roleID == 6)
            {
                lblInfo.Text = "Teacher Information";
            }
            else
            {
                lblInfo.Text = "Department Head Information";
            }
            int passwordLength = 12;
            if (choice.Equals("Update"))
            {
                tbPass.Text = Pass;
                lblpassA.Hide();
                tbID.Enabled = false;
            }
            else
            {
                tbPass.Text = GeneratePassword(passwordLength);
                lblpassA.Show();
                tbID.Enabled = true;
            }

            btnSubmit.Text = choice;
        }
        public void clearSetting()
        {
            cbDepartment.Items.Clear();
        }

        public void displayDept()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                clearSetting();
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_Departments", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbDepartment.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Description from tbl_Role where Role_ID = " + roleID + "", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbRole.Text = dr["Description"].ToString();
                }
                dr.Close();
                cn.Close();
            }
        }

        private void cbDept_KeyPress(object sender, KeyPressEventArgs e)
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
                if (tbFname.Text.Equals(String.Empty) || tbLname.Text.Equals(String.Empty) || tbMname.Text.Equals(String.Empty) || tbID.Text.Equals(String.Empty) || tbPass.Text.Equals(String.Empty) || tbRole.Text.Equals(String.Empty) || cbDepartment.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tbPass.Text.Length <= 6)
                {
                    MessageBox.Show("Password should be above 6 characters!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (roleID == 6)
                    {
                        if (btnSubmit.Text.Equals("Update"))
                        {
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_teacher_accounts WHERE ID = @ID", cn);
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
                            // Check if the password is already taken in tbl_teacher_accounts
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_teacher_accounts WHERE ID = @ID AND Password = @Password", cn);
                            cm.Parameters.AddWithValue("@ID", tbID.Text);
                            cm.Parameters.AddWithValue("@Password", tbPass.Text);
                            ad = new SqlDataAdapter(cm);
                            DataSet dsTeacher = new DataSet();
                            ad.Fill(dsTeacher);

                            // Check if the password is already taken in tbl_deptHead_accounts
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_deptHead_accounts WHERE ID = @ID AND Password = @Password", cn);
                            cm.Parameters.AddWithValue("@ID", tbID.Text);
                            cm.Parameters.AddWithValue("@Password", tbPass.Text);
                            ad = new SqlDataAdapter(cm);
                            DataSet dsDeptHead = new DataSet();
                            ad.Fill(dsDeptHead);

                            if (dsTeacher.Tables[0].Rows.Count > 0 || dsDeptHead.Tables[0].Rows.Count > 0)
                            {
                                MessageBox.Show("Password is already taken!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                cn.Open();
                                cm = new SqlCommand();
                                cm.Connection = cn;
                                cm.CommandType = CommandType.StoredProcedure;
                                cm.CommandText = "sp_UpdateAccounts";
                                cm.Parameters.AddWithValue("@ID", tbID.Text);
                                cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                                cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                                cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                                cm.Parameters.AddWithValue("@Password", tbPass.Text);
                                cm.Parameters.AddWithValue("@Profile_pic", picData);
                                cm.Parameters.AddWithValue("@Role", tbRole.Text);
                                cm.Parameters.AddWithValue("@Department", cbDepartment.Text);
                                cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                                cm.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Account Updated!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            cm = new SqlCommand("Select ID from tbl_teacher_accounts where ID = '" + tbID.Text + "' AND Password = '" + tbPass.Text + "'", cn);
                            ad = new SqlDataAdapter(cm);
                            ad.Fill(ds);
                            int i = ds.Tables[0].Rows.Count;

                            cm = new SqlCommand("Select ID from tbl_deptHead_accounts where ID = '" + tbID.Text + "' AND Password = '" + tbPass.Text + "'", cn);
                            ad = new SqlDataAdapter(cm);
                            ad.Fill(ds);
                            i += ds.Tables[0].Rows.Count;

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
                                cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                                cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                                cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                                cm.Parameters.AddWithValue("@Password", tbPass.Text);
                                cm.Parameters.AddWithValue("@Profile_pic", picData);
                                cm.Parameters.AddWithValue("@Role", tbRole.Text);
                                cm.Parameters.AddWithValue("@Department", cbDepartment.Text);
                                cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                                cm.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Account Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                ds.Tables[0].Rows.Clear();
                                clearTexts();
                            }
                        }
                        if (!istrue)
                            form.displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
                        else
                            form3.DisplayData();
                    }
                    else
                    {
                        if (btnSubmit.Text.Equals("Update"))
                        {
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_deptHead_accounts WHERE ID = @ID", cn);
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
                            // Check if the password is already taken in tbl_teacher_accounts
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_teacher_accounts WHERE ID = @ID AND Password = @Password", cn);
                            cm.Parameters.AddWithValue("@ID", tbID.Text);
                            cm.Parameters.AddWithValue("@Password", tbPass.Text);
                            ad = new SqlDataAdapter(cm);
                            DataSet dsTeacher = new DataSet();
                            ad.Fill(dsTeacher);

                            // Check if the password is already taken in tbl_deptHead_accounts
                            cm = new SqlCommand("SELECT Profile_pic FROM tbl_deptHead_accounts WHERE ID = @ID AND Password = @Password", cn);
                            cm.Parameters.AddWithValue("@ID", tbID.Text);
                            cm.Parameters.AddWithValue("@Password", tbPass.Text);
                            ad = new SqlDataAdapter(cm);
                            DataSet dsDeptHead = new DataSet();
                            ad.Fill(dsDeptHead);

                            if (dsTeacher.Tables[0].Rows.Count > 0 || dsDeptHead.Tables[0].Rows.Count > 0)
                            {
                                MessageBox.Show("Password is already taken!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                cn.Open();
                                cm = new SqlCommand();
                                cm.Connection = cn;
                                cm.CommandType = CommandType.StoredProcedure;
                                cm.CommandText = "sp_UpdateAccounts";
                                cm.Parameters.AddWithValue("@ID", tbID.Text);
                                cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                                cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                                cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                                cm.Parameters.AddWithValue("@Password", tbPass.Text);
                                cm.Parameters.AddWithValue("@Profile_pic", picData);
                                cm.Parameters.AddWithValue("@Role", tbRole.Text);
                                cm.Parameters.AddWithValue("@Department", cbDepartment.Text);
                                cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                                cm.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Account Updated!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            cm = new SqlCommand("Select ID from tbl_teacher_accounts where ID = '" + tbID.Text + "' AND Password = '" + tbPass.Text + "'", cn);
                            ad = new SqlDataAdapter(cm);
                            ad.Fill(ds);
                            int i = ds.Tables[0].Rows.Count;

                            cm = new SqlCommand("Select ID from tbl_deptHead_accounts where ID = '" + tbID.Text + "' AND Password = '" + tbPass.Text + "'", cn);
                            ad = new SqlDataAdapter(cm);
                            ad.Fill(ds);
                            i += ds.Tables[0].Rows.Count;

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
                                cm.Parameters.AddWithValue("@Firstname", tbFname.Text);
                                cm.Parameters.AddWithValue("@Lastname", tbLname.Text);
                                cm.Parameters.AddWithValue("@Middlename", tbMname.Text);
                                cm.Parameters.AddWithValue("@Password", tbPass.Text);
                                cm.Parameters.AddWithValue("@Profile_pic", picData);
                                cm.Parameters.AddWithValue("@Role", tbRole.Text);
                                cm.Parameters.AddWithValue("@Department", cbDepartment.Text);
                                cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                                cm.ExecuteNonQuery();
                                cn.Close();
                                MessageBox.Show("Account Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                ds.Tables[0].Rows.Clear();
                                clearTexts();
                            }
                        }
                        if (!istrue)
                            form2.displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
                        else
                            form3.DisplayData();
                    }

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
            cbDepartment.Text = "";
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
                    if (roleID == 6)
                    {
                        cn.Open();
                        cm = new SqlCommand("Select Profile_pic from tbl_teacher_accounts where ID = " + ID + "", cn);

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
                        cm = new SqlCommand("Select ID,Firstname,Lastname,Middlename,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID where ID = " + ID + "", cn);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        tbID.Text = dr["ID"].ToString();
                        tbFname.Text = dr["Firstname"].ToString();
                        tbLname.Text = dr["Lastname"].ToString();
                        tbMname.Text = dr["Middlename"].ToString();
                        Pass = dr["Password"].ToString();
                        cbDepartment.Text = dr["dDes"].ToString();
                        tbStatus.Text = dr["stDes"].ToString();
                        dr.Close();
                        cn.Close();
                    }
                    else
                    {
                        cn.Open();
                        cm = new SqlCommand("Select Profile_pic from tbl_deptHead_accounts where ID = " + ID + "", cn);

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
                        cm = new SqlCommand("Select ID,Firstname,Lastname,Middlename,Password,d.Description as dDes, st.Description as stDes from tbl_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID where ID = " + ID + "", cn);
                        dr = cm.ExecuteReader();
                        dr.Read();
                        tbID.Text = dr["ID"].ToString();
                        tbFname.Text = dr["Firstname"].ToString();
                        tbLname.Text = dr["Lastname"].ToString();
                        tbMname.Text = dr["Middlename"].ToString();
                        Pass = dr["Password"].ToString();
                        cbDepartment.Text = dr["dDes"].ToString();
                        tbStatus.Text = dr["stDes"].ToString();
                        dr.Close();
                        cn.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void btnAddDep_Click(object sender, EventArgs e)
        {
            formAddSchoolSetting formAddSchoolSetting = new formAddSchoolSetting(new formStudentForm(), this);
            formAddSchoolSetting.setDisplayData("Departments");
            formAddSchoolSetting.ShowDialog();
        }

        private void formTeacherForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!istrue)
            {
                if (roleID == 6)
                {
                    form.displayFilter();
                    form.loadCMSControls();
                }
                else
                {
                    form2.displayFilter();
                    form2.loadCMSControls();
                }
            }
        }
    }
}
