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
    public partial class formAccounts_Students : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formAccounts_Students(String accountName, int role)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
            cn = new SqlConnection(SQL_Connection.connection);

        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            formStudentForm formStudentForm = new formStudentForm(5, "Submit");
            formStudentForm.ShowDialog();
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAddStudent, "Add Account");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export Excel File");
            //toolTip.SetToolTip(option, "Export Excel File");

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cbProgram.Items.Clear();
                cbSection.Items.Clear();
                cbYearlvl.Items.Clear();
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_program", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbProgram.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Description from tbl_section", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbSection.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Description from tbl_year_level", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbYearlvl.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();
            }

            dgvStudents.Rows.Clear();
            displayTable();
        }

        public void displayTable()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                int count = 1;
                cn.Open();
                cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "inner join tbl_program as p on sa.Program = p.Program_ID inner join tbl_Section as se on sa.Section = se.Section_ID " +
                        "inner join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "inner join tbl_status as st on sa.Status = st.Status_ID", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["pDes"].ToString(), dr["sDes"].ToString(), dr["yDes"].ToString(), dr["stDes"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                Rectangle cellBounds = dgvStudents.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                contextMenuStrip2.Show(dgvStudents, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                dgvStudents.Rows.Clear();
                int count = 1;
                cn.Open();
                cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "inner join tbl_program as p on sa.Program = p.Program_ID inner join tbl_Section as se on sa.Section = se.Section_ID " +
                        "inner join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "inner join tbl_status as st on sa.Status = st.Status_ID", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["pDes"].ToString(), dr["sDes"].ToString(), dr["yDes"].ToString(), dr["stDes"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                dgvStudents.Rows.Clear();
                int count = 1;
                cn.Open();
                cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "inner join tbl_program as p on sa.Program = p.Program_ID inner join tbl_Section as se on sa.Section = se.Section_ID " +
                        "inner join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "inner join tbl_status as st on sa.Status = st.Status_ID where st.Description = 'Active'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["pDes"].ToString(), dr["sDes"].ToString(), dr["yDes"].ToString(), dr["stDes"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                dgvStudents.Rows.Clear();
                int count = 1;
                cn.Open();
                cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "inner join tbl_program as p on sa.Program = p.Program_ID inner join tbl_Section as se on sa.Section = se.Section_ID " +
                        "inner join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "inner join tbl_status as st on sa.Status = st.Status_ID where st.Description = 'Inactive'", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dgvStudents.Rows.Add(count++, dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["pDes"].ToString(), dr["sDes"].ToString(), dr["yDes"].ToString(), dr["stDes"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                // Get the ContextMenuStrip associated with the clicked item
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                if (menu != null)
                {
                    // Get the DataGridView that the context menu is associated with
                    DataGridView dataGridView = menu.SourceControl as DataGridView;

                    if (dataGridView != null)
                    {
                        int rowIndex = dataGridView.CurrentCell.RowIndex;
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];
                        formStudentForm formStudentForm = new formStudentForm(5, "Update");
                        formStudentForm.getStudID(dgvStudents.Rows[rowIndex].Cells[1].Value.ToString());
                        formStudentForm.ShowDialog();
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                // Get the ContextMenuStrip associated with the clicked item
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                if (menu != null)
                {
                    // Get the DataGridView that the context menu is associated with
                    DataGridView dataGridView = menu.SourceControl as DataGridView;

                    if (dataGridView != null)
                    {
                        int rowIndex = dataGridView.CurrentCell.RowIndex;
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];

                        // Ask for confirmation before deleting
                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            using (cn = new SqlConnection(SQL_Connection.connection))
                            {
                                try
                                {
                                    cn.Open();

                                    // Rest of your deletion code
                                    // Assuming your primary key column is named "ID"
                                    int primaryKeyValue = Convert.ToInt32(rowToDelete.Cells["ID"].Value);
                                    String deleteQuery = "DELETE FROM tbl_student_accounts WHERE ID = @ID";


                                    using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                    {
                                        // Add parameter for the primary key value
                                        command.Parameters.AddWithValue("@ID", primaryKeyValue);

                                        try
                                        {
                                            cn.Open();
                                            command.ExecuteNonQuery();

                                            MessageBox.Show("Student deleted successfully.");;
                                        }
                                        catch (Exception ex)
                                        {
                                            MessageBox.Show("Error deleting record: " + ex.Message);
                                        }
                                        finally
                                        {
                                            cn.Close();
                                        }
                                    }

                                    MessageBox.Show("Student deleted successfully.");
                                    // Refresh the DataGridView to reflect the changes
                                   // displayData();
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error deleting record: " + ex.Message);
                                }
                                finally
                                {
                                    cn.Close();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
