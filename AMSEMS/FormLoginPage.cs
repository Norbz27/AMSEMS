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
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

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
            UseWaitCursor = true;
            try
            {
                login();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                UseWaitCursor = true;
                try
                {
                    login();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    UseWaitCursor = false;
                }
            }
        }

        public void login()
        {
            if (CheckForInternetConnection())
            {
                using (cn)
                {
                    try
                    {
                        cn.Open();
                        using (SqlDataAdapter ad = new SqlDataAdapter("Select Role from tbl_admin_accounts where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'" +
                                                    " UNION " +
                                                    "Select Role from tbl_deptHead_accounts where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'" +
                                                    " UNION " +
                                                    "Select Role from tbl_guidance_accounts where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'" +
                                                    " UNION " +
                                                    "Select Role from tbl_sao_accounts where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'" +
                                                    " UNION " +
                                                    "Select Role from tbl_teacher_accounts where ID = '" + tbID.Text + "' and Password = '" + tbPass.Text + "'", cn))
                        {
                            DataTable dt = new DataTable();
                            ad.Fill(dt);

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("No Account Data Present!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                int role = Convert.ToInt32(dt.Rows[0][0]);

                                Form mainForm = null;

                                switch (role)
                                {
                                    case 1:
                                        mainForm = new FormAdminNavigation(tbID.Text);
                                        break;
                                    case 2:
                                        mainForm = new FormDeptHeadNavigation();
                                        break;
                                    case 3:
                                        mainForm = new FormGuidanceNavigation();
                                        break;
                                    case 4:
                                        mainForm = new FormSAONavigation();
                                        break;
                                    case 6:
                                        mainForm = new FormTeacherNavigation();
                                        break;
                                    default:
                                        MessageBox.Show("Invalid Role!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        break;
                                }

                                if (mainForm != null)
                                {
                                    mainForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        cn.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("No Internet Connection!! Cant connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void FormLoginPage_Load(object sender, EventArgs e)
        {

        }

        private void FormLoginPage_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
