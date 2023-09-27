using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAcademicYearSetting : KryptonForm
    {
        private SqlConnection cn;
        private string id;
        private bool showMessageOnToggle = false;

        public formAcademicYearSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormAdminNavigation.id;
        }

        private void tgbtnDisableAcc_CheckedChanged(object sender, EventArgs e)
        {
            bool accountDisable = tgbtnDisableAcc.Checked;
            int newStatus = accountDisable ? 2 : 1; // 2 for disabled, 1 for active

            if (showMessageOnToggle)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to change the accounts' status?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string updateQuery = "UPDATE tbl_admin_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_deptHead_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_guidance_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_sao_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_teacher_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_student_accounts SET status = @NewValue;";
                        using (SqlCommand cm = new SqlCommand(updateQuery, cn))
                        {
                            cm.Parameters.AddWithValue("@NewValue", newStatus);
                            cm.ExecuteNonQuery();
                        }
                        cn.Close();
                    }
                }
                else
                {
                    using (cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string updateQuery = "UPDATE tbl_admin_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_deptHead_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_guidance_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_sao_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_teacher_accounts SET status = @NewValue;" +
                                            "UPDATE tbl_student_accounts SET status = @NewValue;";
                        using (SqlCommand cm = new SqlCommand(updateQuery, cn))
                        {
                            cm.Parameters.AddWithValue("@NewValue", 1);
                            cm.ExecuteNonQuery();
                        }
                        cn.Close();
                    }
                }
                // Reset the flag
                showMessageOnToggle = false;
            }
        }

        private bool AreAllAccountsDisabled()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string checkQuery = "SELECT COUNT(*) FROM (" +
                                    "SELECT Status FROM tbl_admin_accounts " +
                                    "UNION " +
                                    "SELECT Status FROM tbl_deptHead_accounts " +
                                    "UNION " +
                                    "SELECT Status FROM tbl_guidance_accounts " +
                                    "UNION " +
                                    "SELECT Status FROM tbl_sao_accounts " +
                                    "UNION " +
                                    "SELECT Status FROM tbl_teacher_accounts " +
                                    "UNION " +
                                    "SELECT Status FROM tbl_student_accounts) AS CombinedRoles " +
                                    "WHERE Status = 2";

                using (SqlCommand command = new SqlCommand(checkQuery, cn))
                {
                    int inactiveAccountCount = (int)command.ExecuteScalar();
                    return (inactiveAccountCount > 0);
                }
            }
        }

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            // Set the initial state of the toggle button based on account status
            tgbtnDisableAcc.Checked = AreAllAccountsDisabled();
        }

        private void tgbtnDisableAcc_Click(object sender, EventArgs e)
        {
            // Set the flag to show the message box on toggle
            showMessageOnToggle = true;
        }
    }
}
