using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
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
            else if (tbNewPass.Text.Length < 6)
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
                        cm = new SqlCommand("Select ID from tbl_deptHead_accounts where Password = @CurrentPassword", cn);
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
                                string stud_id = reader["ID"].ToString();
                                reader.Close();

                                // Check if the password is already taken in tbl_teacher_accounts
                                cm = new SqlCommand("SELECT * FROM tbl_teacher_accounts WHERE Password = @Password AND ID <> @ID", cn);
                                cm.Parameters.AddWithValue("@ID", stud_id);
                                cm.Parameters.AddWithValue("@Password", tbNewPass.Text);
                                ad = new SqlDataAdapter(cm);
                                DataSet dsTeacher = new DataSet();
                                ad.Fill(dsTeacher);

                                // Check if the password is already taken in tbl_deptHead_accounts
                                cm = new SqlCommand("SELECT * FROM tbl_deptHead_accounts WHERE Password = @Password AND ID <> @ID", cn);
                                cm.Parameters.AddWithValue("@ID", stud_id);
                                cm.Parameters.AddWithValue("@Password", tbNewPass.Text);
                                ad = new SqlDataAdapter(cm);
                                DataSet dsDeptHead = new DataSet();
                                ad.Fill(dsDeptHead);

                                cm = new SqlCommand("SELECT * FROM tbl_student_accounts WHERE Password = @Password AND ID <> @ID", cn);
                                cm.Parameters.AddWithValue("@ID", stud_id);
                                cm.Parameters.AddWithValue("@Password", tbNewPass.Text);
                                ad = new SqlDataAdapter(cm);
                                DataSet dsStud = new DataSet();
                                ad.Fill(dsStud);

                                cm = new SqlCommand("SELECT * FROM tbl_sao_accounts WHERE Password = @Password AND ID <> @ID", cn);
                                cm.Parameters.AddWithValue("@ID", stud_id);
                                cm.Parameters.AddWithValue("@Password", tbNewPass.Text);
                                ad = new SqlDataAdapter(cm);
                                DataSet dsSao = new DataSet();
                                ad.Fill(dsSao);

                                cm = new SqlCommand("SELECT * FROM tbl_guidance_accounts WHERE Password = @Password AND ID <> @ID", cn);
                                cm.Parameters.AddWithValue("@ID", stud_id);
                                cm.Parameters.AddWithValue("@Password", tbNewPass.Text);
                                ad = new SqlDataAdapter(cm);
                                DataSet dsGuid = new DataSet();
                                ad.Fill(dsGuid);

                                if (dsTeacher.Tables[0].Rows.Count > 0 || dsDeptHead.Tables[0].Rows.Count > 0 || dsStud.Tables[0].Rows.Count > 0 || dsSao.Tables[0].Rows.Count > 0 || dsGuid.Tables[0].Rows.Count > 0)
                                {
                                    lblConNewPass.Text = "Confirm New Password";
                                    lblConNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                    lblConNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                    lblCurPass.Text = "Current Password - Password is already taken!";
                                    lblCurPass.StateCommon.ShortText.Color1 = System.Drawing.Color.IndianRed;
                                    lblCurPass.StateCommon.ShortText.Color2 = System.Drawing.Color.IndianRed;

                                    lblNewPass.Text = "New Password";
                                    lblNewPass.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                    lblNewPass.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;
                                }
                                else
                                {
                                    cm = new SqlCommand("UPDATE tbl_deptHead_accounts SET Password = @NewValue WHERE Unique_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbNewPass.Text);
                                    cm.Parameters.AddWithValue("@ConditionValue", FormDeptHeadNavigation.id);
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
                                }
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
