using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formSemester_Quarter : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        string header;
        string data;
        bool isSem;
        formAcademicYearSetting form;
        public formSemester_Quarter(formAcademicYearSetting form, bool isSem)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.form = form;
            this.isSem = isSem;
            if (isSem)
            {
                lblHeader1.Text = "Tertiary Academic Term";
            }
            else
            {
                lblHeader1.Text = "SHS Academic Term";
            }
        }

        public void displayData()
        {
            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    dgvAcadYear.Rows.Clear();

                    if (isSem)
                    {
                        string selectQuery = "SELECT Semester_ID, sem.Description AS sem, s.Description AS stat FROM tbl_Semester sem LEFT JOIN tbl_status s ON sem.Status = s.Status_ID  ";

                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dgvAcadYear.Rows.Add(dr["Semester_ID"].ToString(),
                                dr["sem"].ToString(),
                                dr["stat"].ToString());
                        }
                        dr.Close();
                    }
                    else
                    {
                        string selectQuery = "SELECT Quarter_ID, q.Description AS quar, s.Description AS stat FROM tbl_Quarter q LEFT JOIN tbl_status s ON q.Status = s.Status_ID  ";

                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dgvAcadYear.Rows.Add(dr["Quarter_ID"].ToString(),
                                dr["quar"].ToString(),
                                dr["stat"].ToString());
                        }
                        dr.Close();
                    }
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

        private void formAddSchoolSetting_FormClosed(object sender, FormClosedEventArgs e)
        {

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
                        DialogResult result = MessageBox.Show("Are you sure you want to set this "+lblHeader1.Text+" as current?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            using (cn = new SqlConnection(SQL_Connection.connection))
                            {
                                cn.Open();
                                if (isSem)
                                {
                                    // Set the status to 2 for all records
                                    string updateQuery = "UPDATE tbl_Semester SET Status = 2";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, cn))
                                    {
                                        updateCommand.ExecuteNonQuery();
                                    }

                                    // Set the status to 1 for the selected Acad_ID
                                    string updateQuery2 = "UPDATE tbl_Semester SET Status = 1 WHERE Semester_ID = @semid";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery2, cn))
                                    {
                                        updateCommand.Parameters.AddWithValue("@semid", data);
                                        updateCommand.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    string updateQuery = "UPDATE tbl_Quarter SET Status = 2";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery, cn))
                                    {
                                        updateCommand.ExecuteNonQuery();
                                    }

                                    // Set the status to 1 for the selected Acad_ID
                                    string updateQuery2 = "UPDATE tbl_Quarter SET Status = 1 WHERE Quarter_ID = @quarid";
                                    using (SqlCommand updateCommand = new SqlCommand(updateQuery2, cn))
                                    {
                                        updateCommand.Parameters.AddWithValue("@quarid", data);
                                        updateCommand.ExecuteNonQuery();
                                    }
                                }

                                MessageBox.Show("Set "+lblHeader1.Text+" as current successfully!");
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
