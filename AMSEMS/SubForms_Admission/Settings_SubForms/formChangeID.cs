using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
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
            cm = new SqlCommand("Select ID from tbl_admin_accounts where Unique_ID = '" + FormAdmissionNavigation.id + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            tbSchoolID.Text = dr["ID"].ToString();
            dr.Close();
            cn.Close();


        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbCurPass.Text.Equals(String.Empty) || tbSchoolID.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("Select ID from tbl_admin_accounts where Password = @CurrentPassword AND Unique_ID = @ConditionID", cn);
                    cm.Parameters.AddWithValue("@CurrentPassword", tbCurPass.Text);
                    cm.Parameters.AddWithValue("@ConditionID", FormAdmissionNavigation.id);

                    using (SqlDataReader reader = cm.ExecuteReader())
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
                            cm = new SqlCommand("UPDATE tbl_admin_accounts SET ID = @NewValue WHERE Unique_ID = @ConditionValue", cn);
                            cm.Parameters.AddWithValue("@NewValue", tbSchoolID.Text);
                            cm.Parameters.AddWithValue("@ConditionValue", FormAdmissionNavigation.id);
                            cm.ExecuteNonQuery();
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
                            //formDashboard.id2 = tbSchoolID.Text;
                            this.Close();
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
