using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS
{
    public partial class FormLoginPage : KryptonForm
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataAdapter ad;

        public FormLoginPage()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void tbID_Enter(object sender, EventArgs e)
        {
            if(tbID.Text == "School ID")
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        public void login()
        {
            if (CheckForInternetConnection())
            {
                try
                {
                    cn.Open();
                    ad = new SqlDataAdapter("Select count(*) from tbl_admin where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'", cn);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows[0][0].ToString() == "1")
                    {
                        FormAdminNavigation frmMainPage = new FormAdminNavigation();
                        frmMainPage.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Barangay ID or Password!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();
                }
            }
            else
            {
                MessageBox.Show("No Internet Connection!! Cant connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //if (tbID.Text.Equals("admin"))
            //{
            //    FormAdminNavigation formAdminNavigation = new FormAdminNavigation();
            //    formAdminNavigation.Show();
            //    this.Hide();
            //}
            //else if (tbID.Text.Equals("sao"))
            //{
            //    FormSAONavigation formSAONavigation = new FormSAONavigation();
            //    formSAONavigation.Show();
            //    this.Hide();
            //}
            //else if (tbID.Text.Equals("dept"))
            //{
            //    FormDeptHeadNavigation formDeptHeadNavigation = new FormDeptHeadNavigation();
            //    formDeptHeadNavigation.Show();
            //    this.Hide();
            //}
            //else if (tbID.Text.Equals("gui"))
            //{
            //    FormGuidanceNavigation formGuidanceNavigation = new FormGuidanceNavigation();
            //    formGuidanceNavigation.Show();
            //    this.Hide();
            //}
            //else if (tbID.Text.Equals("tech"))
            //{
            //    FormTeacherNavigation formTeacherNavigation = new FormTeacherNavigation();
            //    formTeacherNavigation.Show();
            //    this.Hide();
            //}
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (var stream = client.OpenRead("https://portal.azure.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
