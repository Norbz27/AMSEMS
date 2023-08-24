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

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccounts_Teachers : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formAccounts_Teachers(String accountName, int role)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
            cn = new SqlConnection(SQL_Connection.connection);

        }

        private void formAccounts_Teachers_Load(object sender, EventArgs e)
        {
            displayFilter();
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
        }

        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbET.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_Departments", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbET.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void displayTable(string query)
        {
            try
            {
                dgvTeachers.Rows.Clear();
                int count = 1;

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                dgvTeachers.Rows.Add(
                                    count++,
                                    dr["ID"].ToString(),
                                    dr["Firstname"].ToString(),
                                    dr["Lastname"].ToString(),
                                    dr["dDes"].ToString(),
                                    dr["stDes"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Active");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Inactive");
        }

        private async void ApplyStatusFilter(string statusDescription)
        {
            UseWaitCursor = true;
            string selectedItemET = cbET.Text;

            string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Departments");

            string query = "Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID " +
              "where (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
              "AND (@StatusDescription IS NULL OR st.Description = @StatusDescription)";

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);
                    cmd.Parameters.AddWithValue("@StatusDescription", statusDescription);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvTeachers.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            dgvTeachers.Rows.Add(
                                count++,
                                dr["ID"].ToString(),
                                dr["Firstname"].ToString(),
                                dr["Lastname"].ToString(),
                                dr["dDes"].ToString(),
                                dr["stDes"].ToString()
                            );
                        }
                    }
                }
            }
            UseWaitCursor = false;

        }
        private async Task<string> GetSelectedItemDescriptionAsync(string selectedItem, string tbl)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();

                cm = new SqlCommand("Select Description from " + tbl + " where Description = @SelectedItem", cn);
                cm.Parameters.AddWithValue("@SelectedItem", selectedItem);

                string description = null;

                using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        description = dr["Description"].ToString();
                    }
                }

                return description;
            }
        }

        private async void cbET_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            ComboBox comboBox = (ComboBox)sender;
            string filtertbl = "tbl_Departments";

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemET = cbET.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Departments");

                // Construct the query based on the selected descriptions
                string query = "Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID " +
                    "where (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvTeachers.Rows.Clear();
                            int count = 1;
                            while (dr.Read())
                            {
                                dgvTeachers.Rows.Add(
                                    count++,
                                    dr["ID"].ToString(),
                                    dr["Firstname"].ToString(),
                                    dr["Lastname"].ToString(),
                                    dr["dDes"].ToString(),
                                    dr["stDes"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }
        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvTeachers.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break; // No need to check other cells in the row
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formTeacherForm formTeacherForm = new formTeacherForm(6, "Submit", this);
            formTeacherForm.ShowDialog();
        }
    }
}
