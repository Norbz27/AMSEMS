﻿using iTextSharp.text.pdf;
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
using Excel = Microsoft.Office.Interop.Excel;
using OfficeOpenXml;

namespace AMSEMS.SubForms_Admin
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool headerCheckboxAdded = false; // Add this flag
                                               
        private CheckBox headerCheckbox = new CheckBox();
        public formSubjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

            dgvSubjects.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvSubjects.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // Initialize the header checkbox in the constructor
            headerCheckbox.Size = new Size(15, 15);
            headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;

            // Add the header checkbox to the DataGridView controls
            dgvSubjects.Controls.Add(headerCheckbox);
        }

        private void formSubjects_Load(object sender, EventArgs e)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnAdd, "Add Subject");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelArchive, "Archive");
            toolTip.SetToolTip(btnSetActive, "Set Active");
            toolTip.SetToolTip(btnSetInactive, "Set Inactive");

            btnAll.Focus();
            displayFilter();
            loadCMSControls();

            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
        }

        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbAcadLevel.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Academic_Level_Description from tbl_Academic_Level", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbAcadLevel.Items.Add(dr["Academic_Level_Description"].ToString());
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
                dgvSubjects.Rows.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["ID"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

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
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID  where s.Status = 1");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID  where s.Status = 2");
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
                            String primaryKeyValue = rowToDelete.Cells["ID"].Value.ToString();
                            bool deletionSuccessful = DeleteStudentRecord(primaryKeyValue);

                            if (deletionSuccessful)
                            {
                                displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                                MessageBox.Show("Subject deleted successfully.");
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
        private bool DeleteStudentRecord(String code)
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
        
        private async Task<string> GetSelectedItemDescriptionAsync(string selectedItem, string tbl)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();

                cm = new SqlCommand("Select Academic_Level_Description from " + tbl + " where Academic_Level_Description = @SelectedItem", cn);
                cm.Parameters.AddWithValue("@SelectedItem", selectedItem);

                string description = null;

                using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                {
                    if (await dr.ReadAsync())
                    {
                        description = dr["Academic_Level_Description"].ToString();
                    }
                }

                return description;
            }
        }
        private async void cbAcad_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)sender;
            string filtertbl = "tbl_Academic_Level";

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemET = cbAcadLevel.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Academic_Level");

                // Construct the query based on the selected descriptions
                string query = "Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID " +
                    "where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvSubjects.Rows.Clear();
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["ID"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
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
            if (e.ColumnIndex == dgvSubjects.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvSubjects.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    // Toggle the checkbox value
                    checkBoxCell.Value = !(bool)checkBoxCell.Value;

                    // Check the state of checkboxes and show/hide the panel accordingly
                    UpdatePanelVisibility();
                }
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");

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
        private void UpdatePanelVisibility()
        {
            bool anyChecked = false;

            // Iterate through the DataGridView rows to check if any checkboxes are checked
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell &&
                    (bool)checkBoxCell.Value)
                {
                    anyChecked = true;
                    break; // Exit the loop if at least one checkbox is checked
                }
            }

            // Show or hide the panel based on the state of the checkboxes
            pnControl.Visible = anyChecked;
        }

        private void dgvTeachers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell belongs to the "Select" checkbox column
            if (dgvSubjects.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvSubjects.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvSubjects.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvTeachers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvSubjects.Columns["Select"].Index)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);

                if (!headerCheckboxAdded) // Check if the checkbox has already been added
                {
                   

                    // Center the checkbox within the header cell
                    int x = e.CellBounds.X + (e.CellBounds.Width - headerCheckbox.Width) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - headerCheckbox.Height) / 2;

                    headerCheckbox.Location = new Point(x, y);
                    headerCheckbox.Checked = AreAllCheckboxesChecked();


                    dgvSubjects.Controls.Add(headerCheckbox);

                    headerCheckboxAdded = true; // Set the flag to true
                }
            }
        }
        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvSubjects.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell &&
                    !(bool)checkBoxCell.Value)
                {
                    return false; // At least one checkbox is not checked
                }
            }

            return true; // All checkboxes are checked

        }

        private void btnMultiDel_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Create a list to store the rows to be removed
            List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected

                    // Get the student ID or relevant data from the row
                    string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name
                                                                     // Ask for confirmation from the user
                    DialogResult result = MessageBox.Show($"Delete subject with ID {id}?", "Confirm Deletion", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        // Call your DeleteTeacherRecord method to delete the record
                        bool success = DeleteStudentRecord(id);

                        if (success)
                        {
                            // Add the row to the list of rows to be removed
                            rowsToRemove.Add(row);
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete record with ID: " + id);
                        }
                    }
                }
            }

            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");

            headerCheckbox.Checked = false;
            dgvSubjects.Refresh();
            pnControl.Hide();
        }
        private void btnSetInactive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected
                    break; // Exit the loop as soon as the first selected row is found
                }
            }

            if (hasSelectedRow)
            {
                // Ask for confirmation from the user
                DialogResult result = MessageBox.Show("Set selected subjects as Inactive?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvSubjects.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateSubjectStatus(id, 2);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record with ID: " + id);
                            }
                        }
                    }

                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                    headerCheckbox.Checked = false;
                }
            }

        }

        private void btnSetActive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected
                    break; // Exit the loop as soon as the first selected row is found
                }
            }

            if (hasSelectedRow)
            {
                // Ask for confirmation from the user
                DialogResult result = MessageBox.Show("Set selected subjects as Active?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvSubjects.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString();// Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateSubjectStatus(id, 1);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record with ID: " + id);
                            }
                        }
                    }
                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                    headerCheckbox.Checked = false;
                }
            }

        }
        private bool UpdateSubjectStatus(string saoID, int status)
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE tbl_subjects SET Status = @Status WHERE Course_code = @code";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@code", saoID);
                        command.Parameters.AddWithValue("@Status", status);
                        command.ExecuteNonQuery();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnSelArchive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected
                    break; // Exit the loop as soon as the first selected row is found
                }
            }

            if (hasSelectedRow)
            {
                // Ask for confirmation from the user
                DialogResult result = MessageBox.Show("Are you sure to archive this Subjects?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvSubjects.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString();// Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = AddtoArchive(id);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                MessageBox.Show("Failed to archive record with Course code: " + id);
                            }
                        }
                    }
                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                    headerCheckbox.Checked = false;
                }
            }
        }
        private bool AddtoArchive(string courseCode)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    // Check if a record with the same ID already exists in tbl_student_accounts
                    string checkExistingQuery = "SELECT COUNT(*) FROM tbl_archived_subjects WHERE Course_code = @ID";
                    using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                    {
                        checkExistingCommand.Parameters.AddWithValue("@ID", courseCode);
                        int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                        if (existingRecordCount == 0)
                        {
                            // Update the status to 1 before inserting
                            string updateStatusQuery = "UPDATE tbl_subjects SET Status = 2 WHERE Course_code = @ID";
                            using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                            {
                                updateStatusCommand.Parameters.AddWithValue("@ID", courseCode);
                                updateStatusCommand.ExecuteNonQuery();
                            }

                            string insertQuery = "INSERT INTO tbl_archived_subjects (Course_code,Course_Description,Units,Image,Status,Academic_Level) SELECT Course_code,Course_Description,Units,Image,Status,Academic_Level FROM tbl_subjects WHERE Course_code = @code";
                            using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                            {
                                sqlCommand.Parameters.AddWithValue("@code", courseCode);
                                sqlCommand.ExecuteNonQuery();
                            }

                            string deleteQuery = "DELETE FROM tbl_subjects WHERE Course_code = @code";

                            using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                            {
                                command.Parameters.AddWithValue("@code", courseCode);
                                command.ExecuteNonQuery();
                            }
                            return true;
                        }
                        else
                        {
                            // A record with the same ID already exists in tbl_student_accounts
                            MessageBox.Show("A record with the same ID already exists. No archiving performed.");
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                    return false;
                }
            }
        }

        public void loadCMSControls()
        {
            // Assuming you have a ContextMenuStrip named "contextMenuStrip1"

            int itemCount1 = CMSAcadLvl.Items.Count;

            // Start from the last item (excluding the first item at index 0)
            for (int i = itemCount1 - 1; i > 0; i--)
            {
                CMSAcadLvl.Items.RemoveAt(i);
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("Select Academic_Level_ID,Academic_Level_Description from tbl_academic_level", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        int itemId = Convert.ToInt32(dr["Academic_Level_ID"]);
                        var item = new ToolStripMenuItem(dr["Academic_Level_Description"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 138;
                        item.Height = 26;

                        // Store the table name and ID in the Tag property
                        item.Tag = new Tuple<string, int>("Academic_Level", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSAcadLvl.Items.Add(item);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }
        private void ContextMenuItem_Click(object sender, EventArgs e)
        {
            // Cast the sender to ToolStripMenuItem to access its properties
            ToolStripMenuItem clickedMenuItem = (ToolStripMenuItem)sender;

            // Get the text of the clicked menu item
            string menuItemText = clickedMenuItem.Text;

            // Get the table name and ID from the Tag property
            Tuple<string, int> tagInfo = (Tuple<string, int>)clickedMenuItem.Tag;
            string column = tagInfo.Item1;
            int itemId = tagInfo.Item2;

            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected
                    break; // Exit the loop as soon as the first selected row is found
                }
            }

            if (hasSelectedRow)
            {
                // Ask for confirmation from the user
                DialogResult result = MessageBox.Show("Update Subjects Info?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rows = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvSubjects.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the teacher ID or relevant data from the row
                            int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateTeacherInfo(id, itemId, column);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rows.Add(row);
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record with ID: " + id);
                            }
                        }
                    }
                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                }
            }
        }
        private bool UpdateTeacherInfo(int teacherID, int itemID, string column)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string updateQuery = "UPDATE tbl_subjects SET " + column + " = @ItemID WHERE Course_code = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, cn))
                    {
                        command.Parameters.AddWithValue("@ID", teacherID);
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.ExecuteNonQuery();
                        headerCheckbox.Checked = false;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating record: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnMultiEditAcad_Click(object sender, EventArgs e)
        {
            CMSAcadLvl.Show(btnMultiEditAcad, new System.Drawing.Point(0, btnMultiEditAcad.Height));
        }

        private void cbAcadLevel_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
