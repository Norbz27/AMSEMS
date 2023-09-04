using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formSubjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void formSubjects_Load(object sender, EventArgs e)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAdd, "Add Subject");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export");

            btnAll.Focus();
            displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID");
        }

        public void displayTable(string query)
        {
            try
            {
                dgvSubjects.Rows.Clear();
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
                                dgvSubjects.Rows.Add(
                                    count++,
                                    dr["Course_code"].ToString(),
                                    dr["Course_Description"].ToString(),
                                    dr["Units"].ToString(),
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
            displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID where s.Status = 1");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID where s.Status = 2");
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }
        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvSubjects.Rows)
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
            formImportViewSubjects formImportViewSubjects = new formImportViewSubjects();
            formImportViewSubjects.ShowDialog();
        }
        void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("List of Subjects:", headerFont);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 1); // Exclude the last column
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding


            // Set column widths for specific columns (2nd and 6th columns) to autosize
            float[] columnWidths = new float[dataGridView.Columns.Count - 1];
            columnWidths[0] = 25; // No column width
            columnWidths[1] = 70; // ID column width
            columnWidths[2] = 70; // First Name column autosize
            columnWidths[3] = 70; // Last Name column autosize
            columnWidths[4] = 45; // Status column width
            pdfTable.SetWidths(columnWidths);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Index < dataGridView.Columns.Count - 1) // Exclude the last column
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                    cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                    pdfTable.AddCell(cell);
                }
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                for (int i = 0; i < row.Cells.Count - 1; i++) // Exclude the last column
                {
                    PdfPCell pdfCell = new PdfPCell(new Phrase(row.Cells[i].Value.ToString(), cellFont));
                    pdfTable.AddCell(pdfCell);
                }
            }

            document.Add(pdfTable);
            document.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formSubjectsForm formSubjectsForm = new formSubjectsForm();
            formSubjectsForm.setData("Submit", this);
            formSubjectsForm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
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
                        formSubjectsForm formSubjectsForm = new formSubjectsForm();
                        formSubjectsForm.setData("Update", this);
                        formSubjectsForm.getStudID(dgvSubjects.Rows[rowIndex].Cells[1].Value.ToString());
                        formSubjectsForm.ShowDialog();
                        UseWaitCursor = false;
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
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

                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to delete this record?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            int primaryKeyValue = Convert.ToInt32(rowToDelete.Cells["ID"].Value);
                            bool deletionSuccessful = DeleteStudentRecord(primaryKeyValue);

                            if (deletionSuccessful)
                            {
                                displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID");
                                MessageBox.Show("Account deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error deleting teacher.");
                            }
                            UseWaitCursor = false;
                        }
                    }
                }
            }
        }
        private bool DeleteStudentRecord(int code)
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM tbl_subjects WHERE Course_code = @code";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@code", code);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    // Log or display the error message
                    MessageBox.Show("Error deleting record: " + ex.Message);
                    return false;
                }
            }
        }

        private void dgvSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvSubjects.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvSubjects, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,st.Description as stDes from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID");

            tbSearch.Text = String.Empty;
            btnAll.Focus();
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvSubjects, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
        }
    }
}
