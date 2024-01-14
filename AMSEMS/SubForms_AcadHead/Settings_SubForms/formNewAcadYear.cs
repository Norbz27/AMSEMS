using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formNewAcadYear : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        string header;
        string data;
        Boolean isUpdateTrue = false;

        formAcademicYearSetting form;
        public formNewAcadYear(formAcademicYearSetting form)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.form = form;
        }

        public void displayData()
        {

            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    dgvAcadYear.Rows.Clear();
                     
                    string selectQuery = "SELECT Acad_ID, Academic_Year_Start, Academic_Year_End, s.Description FROM tbl_acad a LEFT JOIN tbl_status s ON a.Status = s.Status_ID";

                    cm = new SqlCommand(selectQuery, cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        dgvAcadYear.Rows.Add(dr["Acad_ID"].ToString(),
                            dr["Academic_Year_Start"].ToString() +"-"+ dr["Academic_Year_End"].ToString(),
                            dr["Description"].ToString());
                    }
                    dr.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

    
        private void btnDone_Click(object sender, EventArgs e)
        {
            form.loadAcad();
            this.Close();
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvAcadYear.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                Rectangle cellBounds = dgvAcadYear.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                contextMenuStrip2.Show(dgvAcadYear, cellBounds.Left, cellBounds.Bottom);
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
                        data = dataGridView.Rows[rowIndex].Cells[0].Value.ToString();
                        formChangeAcademicYear formChangeAcademicYear = new formChangeAcademicYear(this, false);
                        formChangeAcademicYear.getInfoEdit(data);
                        formChangeAcademicYear.Show();
                        isUpdateTrue = true;
                    }
                }
            }
        }

        private void formAddSchoolSetting_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void btnAdd_Click_1(object sender, EventArgs e)
        {
            formChangeAcademicYear formChangeAcademicYear = new formChangeAcademicYear(this, true);
            formChangeAcademicYear.Show();
        }

        private void activetoolStripMenuItem1_Click(object sender, EventArgs e)
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
                        data = dataGridView.Rows[rowIndex].Cells[0].Value.ToString();

                        // Ask for confirmation before updating the status
                        DialogResult result = MessageBox.Show("Are you sure you want to set this academic year as current?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            using (cn = new SqlConnection(SQL_Connection.connection))
                            {
                                cn.Open();
                                // Set the status to 2 for all records
                                string updateQuery = "UPDATE tbl_acad SET Status = 2";
                                using (SqlCommand updateCommand = new SqlCommand(updateQuery, cn))
                                {
                                    updateCommand.ExecuteNonQuery();
                                }

                                // Set the status to 1 for the selected Acad_ID
                                string updateQuery2 = "UPDATE tbl_acad SET Status = 1 WHERE Acad_ID = @acadid";
                                using (SqlCommand updateCommand = new SqlCommand(updateQuery2, cn))
                                {
                                    updateCommand.Parameters.AddWithValue("@acadid", data);
                                    updateCommand.ExecuteNonQuery();
                                }

                                MessageBox.Show("Set academic year as current successfully!");
                                displayData();
                            }
                        }
                    }
                }
            }

        }

        private void formNewAcadYear_Load(object sender, EventArgs e)
        {
            displayData();
        }
    }
}
