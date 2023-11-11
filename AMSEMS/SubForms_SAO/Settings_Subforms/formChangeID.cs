using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formChangeID : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        formAccountSetting form;
        public formChangeID(formAccountSetting form)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.form = form;

            cn.Open();
            cm = new SqlCommand("Select ID from tbl_sao_accounts where Unique_ID = '" + FormSAONavigation.id + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            tbSchoolID.Text = dr["ID"].ToString();
            dr.Close();
            cn.Close();


        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbCurPass.Text.Equals(String.Empty) || tbSchoolID.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();
                    using (SqlCommand cmCheckPassword = new SqlCommand("SELECT ID FROM tbl_sao_accounts WHERE Password = @CurrentPassword AND Unique_ID = @ConditionID", cn))
                    {
                        cmCheckPassword.Parameters.AddWithValue("@CurrentPassword", tbCurPass.Text);
                        cmCheckPassword.Parameters.AddWithValue("@ConditionID", FormSAONavigation.id);

                        using (SqlDataReader reader = cmCheckPassword.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                lblSchoolID.Text = "School ID";
                                lblSchoolID.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                lblSchoolID.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                lblCurPalss.Text = "Current Password - Password do not match";
                                lblCurPalss.StateCommon.ShortText.Color1 = System.Drawing.Color.IndianRed;
                                lblCurPalss.StateCommon.ShortText.Color2 = System.Drawing.Color.IndianRed;
                            }
                            else
                            {
                                reader.Close();
                                using (SqlCommand cmUpdateID = new SqlCommand("UPDATE tbl_sao_accounts SET ID = @NewValue WHERE Unique_ID = @ConditionValue", cn))
                                {
                                    cmUpdateID.Parameters.AddWithValue("@NewValue", tbSchoolID.Text);
                                    cmUpdateID.Parameters.AddWithValue("@ConditionValue", FormSAONavigation.id);
                                    cmUpdateID.ExecuteNonQuery();
                                    MessageBox.Show("School ID Changed!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    lblSchoolID.Text = "School ID";
                                    lblSchoolID.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                    lblSchoolID.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                    lblCurPalss.Text = "Current Password";
                                    lblCurPalss.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
                                    lblCurPalss.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;

                                    UserID user = new UserID();
                                    user.setID(tbSchoolID.Text);
                                    form.loadData();
                                    formDashboard.id2 = tbSchoolID.Text;
                                    this.Close();
                                }
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

        private void tbSchoolID_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a numeric digit (0-9) or the Backspace key.
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a digit or Backspace, handle the event to suppress the input.
                e.Handled = true;
            }
        }
    }
}
