using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.xmp.options;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formAbsReport : Form
    {
        string rep;
        string acadSchYeear, acadTerSem, acadShsSem;
        public formAbsReport(String rep)
        {
            InitializeComponent();
            this.rep = rep;
            lblRepHeader.Text = rep + " Absenteeism Report";
            
        }
        public void displayFilter()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT sec.Description AS secdes FROM tbl_Section sec LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID WHERE al.Academic_Level_Description = @acadlvl";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@acadlvl", rep);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        cbSection.Items.Clear();
                        int count = 1;
                        cbSection.Items.Add("All");
                        while (dr.Read())
                        {
                            cbSection.Items.Add(dr["secdes"].ToString());
                        }
                    }
                }

                query = "SELECT * FROM tbl_acad";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadSchYeear = dr["Academic_Year_Start"].ToString() +"-"+ dr["Academic_Year_End"].ToString();
                            acadShsSem = dr["Ter_Academic_Sem"].ToString();
                            acadTerSem = dr["SHS_Academic_Sem"].ToString();
                        }
                    }
                }
                cbStatus.SelectedIndex = 0;
                cbSection.SelectedIndex = 0;
                cbMonth.SelectedItem = DateTime.Now.ToString("MMMM");
            }
        }
        public void displayReport()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = @"SELECT 
                            s.ID,
                            UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                            sec.Description AS secdes,
                            cr.Absences AS ConsecutiveAbsentDays,
                            cr.Status AS ConsultationStatus
                        FROM 
                            tbl_subject_attendance sat
                            LEFT JOIN tbl_student_accounts s ON sat.Student_ID = s.ID 
                            LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                        LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                        	LEFT JOIN tbl_class_list cl ON sat.Class_Code = cl.CLass_Code
                            LEFT JOIN tbl_consultation_record cr ON s.ID = cr.Student_ID
                        WHERE 
                            al.Academic_Level_Description = @acadlvl 
                            AND (@SectionDescription = 'All' OR sec.Description = @SectionDescription) 
                            AND UPPER(FORMAT(CONVERT(DATE, sat.Attendance_date, 0), 'MMMM')) = @month
                            AND s.Status = 1
                            AND cl.School_Year = @schyear
                            AND cl.Semester = @sem
                            AND (@Status = 'All' OR cr.Status = @Status)
                        GROUP BY 
                            s.ID, s.Lastname, s.Firstname, s.Middlename, sec.Description, cr.Status, cr.Absences
                        HAVING 
                            COUNT(DISTINCT CASE WHEN sat.Student_Status = 'A' THEN sat.Attendance_date END) > 2
                        ORDER BY
                            Name;";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@acadlvl", rep);
                    cmd.Parameters.AddWithValue("@schyear", acadSchYeear);
                    if (rep.ToUpper().Equals("SHS"))
                        cmd.Parameters.AddWithValue("@sem", acadShsSem);
                    else
                        cmd.Parameters.AddWithValue("@sem", acadTerSem);
                    cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(cbSection.Text) ? DBNull.Value : (object)cbSection.Text);
                    cmd.Parameters.AddWithValue("@month", string.IsNullOrEmpty(cbMonth.Text) ? DBNull.Value : (object)cbMonth.Text);
                    cmd.Parameters.AddWithValue("@Status", cbStatus.SelectedItem.ToString());

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvAbesnteismRep.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            // Add a row and set the checkbox column value to false (unchecked)
                            int rowIndex = dgvAbesnteismRep.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvAbesnteismRep.Rows[rowIndex].Cells["studid"].Value = dr["ID"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["studname"].Value = dr["Name"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["absences"].Value = dr["ConsecutiveAbsentDays"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["status"].Value = dr["ConsultationStatus"].ToString();
                        }
                    }
                }
            }
        }

        private void formAbsReport_Load(object sender, EventArgs e)
        {
            displayFilter();
        }

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
        }

        private void dgvAbesnteismRep_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvAbesnteismRep.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvAbesnteismRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvAbesnteismRep, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
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
                        formStudAttRecord form = new formStudAttRecord();
                        form.getForm(this, dgvAbesnteismRep.Rows[rowIndex].Cells[0].Value.ToString());
                        form.ShowDialog();
                        UseWaitCursor = false;
                    }
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
        }

        private void btnExpPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvAbesnteismRep, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }
        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvAbesnteismRep.Rows)
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

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
        }

        private void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            try
            {
                Document document = new Document(PageSize.LETTER);
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

                document.Open();

                // Customizing the font and size
                iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
                iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
                iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

                // Add title "List of Students:"
                Paragraph titleParagraph = new Paragraph("Absenteeism Report", headerFont1);
                titleParagraph.Alignment = Element.ALIGN_CENTER;
                document.Add(titleParagraph);

                Paragraph titleParagraph1 = new Paragraph(DateTime.Now.ToString("dddd MMM dd, yyyy"), cellFont);
                titleParagraph1.Alignment = Element.ALIGN_CENTER;
                document.Add(titleParagraph1);

                PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 1);
                pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
                pdfTable.SpacingBefore = 10f; // Add space before the table
                pdfTable.DefaultCell.Padding = 3; // Cell padding

                // Add header cells excluding the last column
                foreach (DataGridViewColumn column in dataGridView.Columns)
                {
                    if (column.Index != dataGridView.Columns.Count - 1) // Exclude the last column
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                        cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                        pdfTable.AddCell(cell);
                    }
                }

                // Add data cells excluding the last column
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
            catch (Exception ex)
            {
                MessageBox.Show("Error exporting to PDF: " + ex.Message, "Export to PDF Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
        }
    }
}
