using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAcademicYearSetting : KryptonForm
    {
        private SqlConnection cn;
        private SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool showMessageOnToggle = false;
        private bool previousToggleState; // Store the previous toggle state

        public formAcademicYearSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            GetCurrentStatusFromDatabase();
        }

        private void ToggleAccountStatus(int newStatus)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string updateQuery = @"UPDATE tbl_deptHead_accounts SET status = @NewValue;
                        UPDATE tbl_guidance_accounts SET status = @NewValue;
                        UPDATE tbl_sao_accounts SET status = @NewValue;
                        UPDATE tbl_teacher_accounts SET status = @NewValue;
                        UPDATE tbl_student_accounts SET status = @NewValue;";
                    using (SqlCommand cm = new SqlCommand(updateQuery, cn))
                    {
                        cm.Parameters.AddWithValue("@NewValue", newStatus);
                        cm.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Handle any database connection or query exceptions here
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void GetCurrentStatusFromDatabase()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string checkQuery = @"
                    SELECT COUNT(*) FROM (
                        SELECT Status FROM tbl_deptHead_accounts
                        UNION
                        SELECT Status FROM tbl_guidance_accounts
                        UNION
                        SELECT Status FROM tbl_sao_accounts
                        UNION
                        SELECT Status FROM tbl_teacher_accounts
                        UNION
                        SELECT Status FROM tbl_student_accounts
                    ) AS CombinedRoles
                    WHERE Status = 2";

                    using (SqlCommand command = new SqlCommand(checkQuery, cn))
                    {
                        int inactiveAccountCount = (int)command.ExecuteScalar();

                        // If there are inactive accounts (Status = 2), set currentStatus to 2; otherwise, it remains 1 (active).
                        if (inactiveAccountCount > 0)
                        {
                            previousToggleState = true; // Set the previous state to "ON"
                        }
                        else
                        {
                            previousToggleState = false; // Set the previous state to "OFF"
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Handle any database connection or query exceptions here
                    MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tgbtnDisableAcc_CheckedChanged(object sender, EventArgs e)
        {
            bool accountDisable = tgbtnDisableAcc.Checked;
            int newStatus = accountDisable ? 2 : 1; // 2 for disabled, 1 for active

            if (accountDisable)
            {
                showMessageOnToggle = true;
            }
            else
            {
                showMessageOnToggle = false;
            }

            if (showMessageOnToggle)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to change the status?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Update the status in the database
                    ToggleAccountStatus(newStatus);
                }
                else
                {
                    // Set the toggle back to its previous state
                    tgbtnDisableAcc.Checked = previousToggleState;
                }

                // Reset the flag
                showMessageOnToggle = false;
            }
            else
            {
                // Update the status in the database
                ToggleAccountStatus(newStatus);
            }
        }

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            // Set the initial state of the toggle switch based on the previousToggleState
            tgbtnDisableAcc.Checked = previousToggleState;

            // Ensure that showMessageOnToggle is set to false when the form1 loads
            showMessageOnToggle = false;

            loadAcad();
        }

        private void btnEditAcad_Click(object sender, EventArgs e)
        {
            formChangeAcademicYear formChangeAcademicYear = new formChangeAcademicYear(this);
            formChangeAcademicYear.ShowDialog();
        }

        public void loadAcad()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Academic_Year_Start,Academic_Year_End,Academic_Sem from tbl_acad where Acad_ID = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblAcadYear.Text = dr["Academic_Year_Start"].ToString() + "-" + dr["Academic_Year_End"].ToString();
                lblAcadSem.Text = dr["Academic_Sem"].ToString();
                dr.Close();
                cn.Close();
            }
        }
    }
}
