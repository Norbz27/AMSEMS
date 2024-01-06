using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool headerCheckboxAdded = false; // Add this flag
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CheckBox headerCheckbox;
        private CancellationTokenSource cancellationTokenSource;
        private string schoolYear, semester;
        public formSubjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            cancellationTokenSource = new CancellationTokenSource();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvSubjects.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvSubjects.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // Initialize the header checkbox in the constructor


        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            displayTable();

            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This method runs on the UI thread
            // Update the UI or perform other tasks after the background work completes
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
            }
        }

        private void formSubjects_Load(object sender, EventArgs e)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnExport, "Export");

            if (FormDeptHeadNavigation.acadlevel == "SHS")
            {
                lblSem.Text = "Quarter";
                dgvSubjects.Columns[5].HeaderText = "Quarter";
            }
            else
            {
                lblSem.Text = "Semester";
                dgvSubjects.Columns[5].HeaderText = "Semester";
            }
            displayAcademicYear();
            backgroundWorker.RunWorkerAsync();

        }
        public void displayAcademicYear()
        {
            cbSem.Items.Clear();
            cbSchoolYear.Items.Clear();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";

                query = "SELECT Acad_ID, Academic_Year_Start +'-'+ Academic_Year_End AS acad_year FROM tbl_acad WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cbSchoolYear.Text = dr["acad_year"].ToString();
                            schoolYear = dr["Acad_ID"].ToString();
                        }
                    }
                }
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter WHERE Status = 1";
                }
                else
                {
                    query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester WHERE Status = 1";
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cbSem.Text = dr["Description"].ToString();
                            semester = dr["ID"].ToString();
                        }
                    }
                }

                query = "SELECT Acad_ID, Academic_Year_Start +'-'+ Academic_Year_End AS acad_year FROM tbl_acad";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSchoolYear.Items.Add(dr["acad_year"].ToString());
                        }
                    }
                }

                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter";
                }
                else
                {
                    query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester";
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSem.Items.Add(dr["Description"].ToString());
                        }
                    }
                }
            }
        }

        private async void cbSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvSubjects.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                headerCheckboxAdded = false;

                ptbLoading.Visible = false;
            }
        }

        private async void cbSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvSubjects.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                headerCheckboxAdded = false;

                ptbLoading.Visible = false;
            }
        }

        public async void displayTable()
        {
            if (dgvSubjects == null || dgvSubjects.IsDisposed || !dgvSubjects.IsHandleCreated)
            {
                return;
            }
            if (dgvSubjects.InvokeRequired)
            {
                dgvSubjects.Invoke(new Action(() => displayTable()));
                return;
            }
            try
            {
                dgvSubjects.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                headerCheckboxAdded = false;

                ptbLoading.Visible = false;
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

            Paragraph titleParagraph1 = new Paragraph("Printed Date: " + DateTime.Now.ToString("MMMM dd, yyyy"), headerFont);
            titleParagraph1.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph1);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 1); // Exclude the last column
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding


            // Set column widths for specific columns (2nd and 6th columns) to autosize
            float[] columnWidths = new float[dataGridView.Columns.Count - 1];
            columnWidths[0] = 0; // No column width
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
                        formSubjectsForm formSubjectsForm = new formSubjectsForm();
                        formSubjectsForm.setData("Update", this);
                        formSubjectsForm.getStudID(dgvSubjects.Rows[rowIndex].Cells[1].Value.ToString());
                        formSubjectsForm.ShowDialog();
                        UseWaitCursor = false;
                    }
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
            displayTable();
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

        private void btnExpExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save As Excel File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Create a new Excel application
                        Excel.Application excelApp = new Excel.Application();
                        excelApp.Visible = false; // You can set this to true for debugging purposes

                        // Create a new Excel workbook
                        Excel.Workbook workbook = excelApp.Workbooks.Add();

                        // Create a new Excel worksheet
                        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.Add();

                        // Customizing the table appearance
                        Excel.Range tableRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[dgvSubjects.Rows.Count + 1, dgvSubjects.Columns.Count - 2]]; // Exclude the first and last columns

                        int excelColumnIndex = 1; // Start from the first Excel column
                        foreach (DataGridViewColumn column in dgvSubjects.Columns)
                        {
                            if (column.Index > 0 && column.Index < dgvSubjects.Columns.Count - 1) // Skip the first and last columns
                            {
                                worksheet.Cells[1, excelColumnIndex] = column.HeaderText; // Set the header in the first row
                                worksheet.Cells[1, excelColumnIndex].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(68, 114, 196)); // Background color: #4472C4
                                worksheet.Cells[1, excelColumnIndex].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White); // Text color: White
                                excelColumnIndex++;
                            }
                        }

                        int rowIndex = 2; // Start from the second row
                        foreach (DataGridViewRow row in dgvSubjects.Rows)
                        {
                            excelColumnIndex = 1; // Reset Excel column index for each row
                            for (int i = 1; i < row.Cells.Count - 1; i++) // Skip the first and last cell in each row
                            {
                                worksheet.Cells[rowIndex, excelColumnIndex] = row.Cells[i].Value.ToString();
                                excelColumnIndex++;
                            }
                            rowIndex++;
                        }

                        // Apply the "Blue, Table Style Medium 2" table style
                        tableRange.Worksheet.ListObjects.Add(Excel.XlListObjectSourceType.xlSrcRange, tableRange, null, Excel.XlYesNoGuess.xlYes, null).TableStyle = "TableStyleMedium2";

                        // Save the Excel file
                        workbook.SaveAs(filePath);

                        // Close Excel and release resources
                        workbook.Close();
                        excelApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                        MessageBox.Show("Data exported to Excel successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAssignSubject_Click(object sender, EventArgs e)
        {
            formAssignTeacherToSub formAssignTeacherToSub = new formAssignTeacherToSub();
            formAssignTeacherToSub.ShowDialog();
        }
    }
}
