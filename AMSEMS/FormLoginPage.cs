﻿using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class FormLoginPage : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        private bool isLoggingIn = false;
        public FormLoginPage()
        {
            //fontInstaller();
            cn = new SqlConnection(SQL_Connection.connection);
            sQLite_Connection = new SQLite_Connection();
            sQLite_Connection.InitializeDatabase();
            InitializeComponent();
        }
        private void tbID_Enter(object sender, EventArgs e)
        {
            if (tbID.Text == "School ID")
            {
                tbID.Text = string.Empty;
                tbID.StateCommon.Content.Color1 = Color.DimGray;
                tbID.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbID.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbID_Leave(object sender, EventArgs e)
        {
            if (tbID.Text == string.Empty)
            {
                tbID.Text = "School ID";
                tbID.StateCommon.Content.Color1 = Color.Silver;
                tbID.StateCommon.Border.Color1 = Color.Gray;
                tbID.StateCommon.Border.Color2 = Color.Gray;
            }
        }

        private void tbPass_Enter(object sender, EventArgs e)
        {
            if (tbPass.Text == "Password")
            {
                tbPass.Text = string.Empty;
                tbPass.StateCommon.Content.Color1 = Color.DimGray;
                tbPass.UseSystemPasswordChar = true;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 8F);
                tbPass.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbPass.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbPass_Leave(object sender, EventArgs e)
        {
            if (tbPass.Text == string.Empty)
            {
                tbPass.Text = "Password";
                tbPass.StateCommon.Content.Color1 = Color.Silver;
                tbPass.UseSystemPasswordChar = false;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 10F);
                tbPass.StateCommon.Border.Color1 = Color.Gray;
                tbPass.StateCommon.Border.Color2 = Color.Gray;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggingIn) return; // If already logging in, exit

            isLoggingIn = true;
            UseWaitCursor = true;

            try
            {
                await LoginAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                isLoggingIn = false;
            }
        }

        private async void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (isLoggingIn) return; // If already logging in, exit

                isLoggingIn = true;
                UseWaitCursor = true;

                try
                {
                    await LoginAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    UseWaitCursor = false;
                    isLoggingIn = false;
                }
            }
        }

        private async Task LoginAsync()
        {
            //if (!CheckForInternetConnection())
            //{
            //    MessageBox.Show("Unstable Connection!! Can't connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    await cn.OpenAsync();

                    using (SqlCommand cmd = new SqlCommand("SELECT Role, Unique_ID FROM (" +
                        "SELECT Role, Unique_ID FROM tbl_admin_accounts WHERE ID = @ID AND Password = @Password AND Status = 1 " +
                        "UNION " +
                        "SELECT Role, Unique_ID FROM tbl_deptHead_accounts WHERE ID = @ID AND Password = @Password AND Status = 1 " +
                        "UNION " +
                        "SELECT Role, Unique_ID FROM tbl_guidance_accounts WHERE ID = @ID AND Password = @Password AND Status = 1 " +
                        "UNION " +
                        "SELECT Role, Unique_ID FROM tbl_sao_accounts WHERE ID = @ID AND Password = @Password AND Status = 1 " +
                        "UNION " +
                        "SELECT Role, Unique_ID FROM tbl_acadHead_accounts WHERE ID = @ID AND Password = @Password AND Status = 1 " +
                        "UNION " +
                        "SELECT Role, Unique_ID FROM tbl_teacher_accounts WHERE ID = @ID AND Password = @Password AND Status = 1) AS CombinedRoles", cn))
                    {
                        cmd.Parameters.AddWithValue("@ID", tbID.Text);
                        cmd.Parameters.AddWithValue("@Password", tbPass.Text);
                        // Add parameters and execute the query here
                        DataTable dt = new DataTable();
                        using (SqlDataAdapter ad = new SqlDataAdapter(cmd))
                        {
                            await Task.Run(() => ad.Fill(dt));
                        }

                        if (dt.Rows.Count == 0)
                        {
                            MessageBox.Show("Invalid Account, No Data Found!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            int role = Convert.ToInt32(dt.Rows[0]["Role"]);
                            string uniqueID = dt.Rows[0]["Unique_ID"].ToString();
                            Form mainForm = null;

                            switch (role)
                            {
                                case 1:
                                    mainForm = new FormAdmissionNavigation(uniqueID);
                                    break;
                                case 2:
                                    mainForm = new FormDeptHeadNavigation(uniqueID);
                                    break;
                                case 3:
                                    mainForm = new FormGuidanceNavigation(uniqueID);
                                    break;
                                case 4:
                                    mainForm = new FormSAONavigation(uniqueID);
                                    break;
                                case 6:
                                    mainForm = new FormTeacherNavigation(uniqueID);
                                    break;
                                case 7:
                                    mainForm = new FormAcadHeadNavigatio(uniqueID);
                                    break;
                                default:
                                    MessageBox.Show("Invalid Role!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    break;
                            }

                            if (mainForm != null)
                            {
                                btnLogin.Enabled = false;
                                mainForm.Show();
                                this.Hide();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var mobileClient = new WebClient())
                using (var webConnection = mobileClient.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void FormLoginPage_Load(object sender, EventArgs e)
        {

        }

        private void FormLoginPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
