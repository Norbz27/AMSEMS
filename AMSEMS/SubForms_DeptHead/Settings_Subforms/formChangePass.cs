using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text.pdf;
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

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formChangePass : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        public formChangePass()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (!tbNewPass.Text.Equals(tbConNewPass.Text))
            {
                lblConNewPass.Text = "Confirm New Password - Password do not match!";
                lblConNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.IndianRed;
                lblConNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.IndianRed;

                lblCurPass.Text = "Current Password";
                lblCurPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                lblCurPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                lblNewPass.Text = "New Password";
                lblNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                lblNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;
            }
            else if(tbNewPass.Text.Length < 6)
            {
                lblConNewPass.Text = "Confirm New Password";
                lblConNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                lblConNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                lblCurPass.Text = "Current Password";
                lblCurPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                lblCurPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                lblNewPass.Text = "New Password - Must be 6 or longer in length";
                lblNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.IndianRed;
                lblNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.IndianRed;
            }
            else
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    if (tbConNewPass.Text.Equals(String.Empty) || tbCurPass.Text.Equals(String.Empty) || tbNewPass.Text.Equals(String.Empty))
                    {
                        MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        cn.Open();
                        cm = new SqlCommand("Select ID from tbl_admin_accounts where Password = @CurrentPassword", cn);
                        cm.Parameters.AddWithValue("@CurrentPassword", tbCurPass.Text);

                        using (SqlDataReader reader = cm.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblConNewPass.Text = "Confirm New Password";
                                lblConNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblConNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                lblCurPass.Text = "Current Password - Password do not match!";
                                lblCurPass.StateCommon.ShortText.Color1 = System.Drawing.Color.IndianRed;
                                lblCurPass.StateCommon.ShortText.Color2 = System.Drawing.Color.IndianRed;

                                lblNewPass.Text = "New Password";
                                lblNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;
                            }
                            else
                            {
                                reader.Close();
                                cm = new SqlCommand("UPDATE tbl_admin_accounts SET Password = @NewValue WHERE Unique_ID = @ConditionValue", cn);
                                cm.Parameters.AddWithValue("@NewValue", tbNewPass.Text);
                                cm.Parameters.AddWithValue("@ConditionValue", FormAdminNavigation.id);
                                cm.ExecuteNonQuery();
                                MessageBox.Show("Password Changed!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                lblConNewPass.Text = "Confirm New Password";
                                lblConNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblConNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                lblCurPass.Text = "Current Password";
                                lblCurPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblCurPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                lblNewPass.Text = "New Password";
                                lblNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;
                                this.Close();
                            }
                        }
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
