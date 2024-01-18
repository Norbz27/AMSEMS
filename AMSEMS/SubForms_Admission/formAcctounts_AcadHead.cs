using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAcctounts_AcadHead : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        static int role;
        static string accountName;

        private bool headerCheckboxAdded = false; // Add this flag
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CheckBox headerCheckbox = new CheckBox();
        private CancellationTokenSource cancellationTokenSource;
        public formAcctounts_AcadHead()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            cancellationTokenSource = new CancellationTokenSource();
            lblAccountName.Text = accountName;

            dgvacadHead.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvacadHead.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            // Initialize the header checkbox in the constructor
            headerCheckbox.Size = new Size(15, 15);
            headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");

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
        public static void setAccountName(String accountName1)
        {
            accountName = accountName1;
        }
        public static void setRole(int role1)
        {
            role = role1;
        }

        private void formAccounts_SAO_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnAdd, "Add Account");
            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelArchive, "Archive");
            toolTip.SetToolTip(btnSetActive, "Set Active");
            toolTip.SetToolTip(btnSetInactive, "Set Inactive");

            btnAll.Focus();
            backgroundWorker.RunWorkerAsync();
        }
        public async void displayTable(string query)
        {
            if (dgvacadHead.InvokeRequired)
            {
                dgvacadHead.Invoke(new Action(() => displayTable(query)));
                return;
            }
            try
            {
                query = query + " ORDER BY DateTime DESC";
                dgvacadHead.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvacadHead.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvacadHead.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvacadHead.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                                dgvacadHead.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
                                dgvacadHead.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvacadHead.Rows[rowIndex].Cells["option"].Value = option.Image;
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
                dgvacadHead.Controls.Add(headerCheckbox);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            tbSearch.Text = String.Empty;
            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID where st.Description = 'Active'");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID where st.Description = 'Inactive'");
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }
        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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
            //formImportView form2 = new formImportView();
            //form2.setRole(role);
            //form2.reloadFormSao(this);
            //form2.ShowDialog();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
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
            Paragraph titleParagraph = new Paragraph("List of Academic Head Accounts:", headerFont);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

            Paragraph titleParagraph1 = new Paragraph("Printed Date: " + DateTime.Now.ToString("MMMM dd, yyyy"), headerFont);
            titleParagraph1.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph1);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 2); // Exclude the first and last columns
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding

            // Set column widths for specific columns (2nd and 6th columns) to autosize
            float[] columnWidths = new float[dataGridView.Columns.Count - 2];
            columnWidths[0] = 30; // ID column autosize
            columnWidths[1] = 70; // First Name column autosize
            columnWidths[2] = 70; // Last Name column autosize
            pdfTable.SetWidths(columnWidths);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Index > 0 && column.Index < dataGridView.Columns.Count - 1) // Exclude the first and last columns
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                    cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                    pdfTable.AddCell(cell);
                }
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                for (int i = 1; i < row.Cells.Count - 1; i++) // Skip the first and last columns
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
            formGeneratedForm formGeneratedForm = new formGeneratedForm();
            formGeneratedForm.setData4(role, "Submit", this);
            formGeneratedForm.ShowDialog();
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
                        formGeneratedForm formGeneratedForm = new formGeneratedForm();
                        formGeneratedForm.setData4(role, "Update", this);
                        formGeneratedForm.getStudID(dgvacadHead.Rows[rowIndex].Cells[1].Value.ToString());
                        formGeneratedForm.ShowDialog();
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
                                displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
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
        private bool DeleteStudentRecord(int studentID)
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM tbl_acadHead_accounts WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", studentID);
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

        private void dgvGuidance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvacadHead.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvacadHead.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                contextMenuStrip2.Show(dgvacadHead, cellBounds.Left, cellBounds.Bottom);
            }
            if (e.ColumnIndex == dgvacadHead.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvacadHead.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
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
            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
        }

        private void btnExpPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvacadHead, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }
        private void UpdatePanelVisibility()
        {
            bool anyChecked = false;

            // Iterate through the DataGridView rows to check if any checkboxes are checked
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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
            if (dgvacadHead.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvacadHead.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvacadHead.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvacadHead.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvTeachers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvacadHead.Columns["Select"].Index)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);

                if (!headerCheckboxAdded) // Check if the checkbox has already been added
                {
                    int x = e.CellBounds.X + (e.CellBounds.Width - headerCheckbox.Width) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - headerCheckbox.Height) / 2;

                    headerCheckbox.Location = new Point(x, y);
                    headerCheckbox.Checked = AreAllCheckboxesChecked();

                    if (dgvacadHead.Rows.Count != 0)
                    {
                        dgvacadHead.Controls.Add(headerCheckbox);
                    }

                    headerCheckboxAdded = true; // Set the flag to true
                }
            }
        }
        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvacadHead.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvacadHead.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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

            DialogResult result = MessageBox.Show($"Do you want to delete selected account?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Iterate through the DataGridView rows to find selected rows
                foreach (DataGridViewRow row in dgvacadHead.Rows)
                {
                    // Check if the "Select" checkbox is checked in the current row
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                    if (chk.Value != null && (bool)chk.Value)
                    {
                        hasSelectedRow = true; // Set the flag to true if at least one row is selected

                        // Get the student ID or relevant data from the row
                        int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

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

            displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");

            headerCheckbox.Checked = false;
            dgvacadHead.Refresh();
            pnControl.Hide();

        }
        private void btnSetInactive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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
                DialogResult result = MessageBox.Show("Set selected accounts as Inactive?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvacadHead.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateSAOStatus(id, 2);

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

                    displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                    headerCheckbox.Checked = false;
                }
            }

        }

        private void btnSetActive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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
                DialogResult result = MessageBox.Show("Set selected accounts as Active?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvacadHead.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateSAOStatus(id, 1);

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
                    displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                    headerCheckbox.Checked = false;
                }
            }

        }
        private bool UpdateSAOStatus(string saoID, int status)
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE tbl_acadHead_accounts SET Status = @Status WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", saoID);
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
            foreach (DataGridViewRow row in dgvacadHead.Rows)
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
                DialogResult result = MessageBox.Show("Are you sure to archive this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvacadHead.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the sao ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = AddtoArchive(id);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                MessageBox.Show("Failed to archive record with ID: " + id);
                            }
                        }
                    }
                    displayTable("Select ID,Firstname,Lastname,Password, st.Description as stDes from tbl_acadHead_accounts as te left join tbl_status as st on te.Status = st.Status_ID");
                    headerCheckbox.Checked = false;
                }
            }
        }
        private bool AddtoArchive(string saoID)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    // Check if a record with the same ID already exists in tbl_student_accounts
                    string checkExistingQuery = "SELECT COUNT(*) FROM tbl_archived_acadHead_accounts WHERE ID = @ID";
                    using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                    {
                        checkExistingCommand.Parameters.AddWithValue("@ID", saoID);
                        int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                        if (existingRecordCount == 0)
                        {
                            // Update the status to 1 before inserting
                            string updateStatusQuery = "UPDATE tbl_acadHead_accounts SET Status = 2 WHERE ID = @ID";
                            using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                            {
                                updateStatusCommand.Parameters.AddWithValue("@ID", saoID);
                                updateStatusCommand.ExecuteNonQuery();
                            }

                            string insertQuery = "INSERT INTO tbl_archived_acadHead_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Role,Status,DateTime FROM tbl_acadHead_accounts WHERE ID = @ID";
                            using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                            {
                                sqlCommand.Parameters.AddWithValue("@ID", saoID);
                                sqlCommand.ExecuteNonQuery();
                            }

                            string deleteQuery = "DELETE FROM tbl_acadHead_accounts WHERE ID = @ID";

                            using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", saoID);
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


                        // Your SQL query to retrieve data from the database
                        string query = "SELECT ID, UPPER(Firstname) as Firstname, UPPER(Middlename) as Middlename, UPPER(Lastname) as Lastname FROM tbl_acadHead_accounts";

                        // Create a new Excel application
                        Excel.Application excelApp = new Excel.Application();
                        excelApp.Visible = false; // You can set this to true for debugging purposes

                        // Create a new Excel workbook
                        Excel.Workbook workbook = excelApp.Workbooks.Add();

                        // Create a new Excel worksheet
                        Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.Add();

                        // Customizing the table appearance
                        Excel.Range tableRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]]; // Assuming there are four columns

                        // Execute the query and populate the Excel worksheet
                        using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                        {
                            connection.Open();
                            using (SqlCommand command = new SqlCommand(query, connection))
                            {
                                SqlDataReader reader = command.ExecuteReader();

                                for (int i = 1; i <= reader.FieldCount; i++)
                                {
                                    worksheet.Cells[1, i] = reader.GetName(i - 1);
                                    worksheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(68, 114, 196));
                                    worksheet.Cells[1, i].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                                }

                                int rowIndex = 2;
                                while (reader.Read())
                                {
                                    for (int i = 1; i <= reader.FieldCount; i++)
                                    {
                                        worksheet.Cells[rowIndex, i] = reader.GetValue(i - 1).ToString();
                                    }
                                    rowIndex++;
                                }

                                reader.Close();
                            }
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
        private void activetoolStripMenuItem2_Click(object sender, EventArgs e)
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

                        DialogResult confirmationResult = MessageBox.Show("Set accounts as Active?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            string primaryKeyValue = rowToDelete.Cells["ID"].Value.ToString();
                            bool deletionSuccessful = UpdateSAOStatus(primaryKeyValue, 1);

                            if (deletionSuccessful)
                            {
                                displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record.");
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
        }

        private void inactiveToolStripMenuItem_Click(object sender, EventArgs e)
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

                        DialogResult confirmationResult = MessageBox.Show("Set accounts as Inactive?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            string primaryKeyValue = rowToDelete.Cells["ID"].Value.ToString();
                            bool deletionSuccessful = UpdateSAOStatus(primaryKeyValue, 2);

                            if (deletionSuccessful)
                            {
                                displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                            }
                            else
                            {
                                MessageBox.Show("Failed to update record.");
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
        }

        private void archiveToolStripMenuItem_Click(object sender, EventArgs e)
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

                        // Ask for confirmation from the user
                        DialogResult result = MessageBox.Show("Are you sure to archive this accounts?", "Confirm Update", MessageBoxButtons.YesNo);

                        if (result == DialogResult.Yes)
                        {
                            string primaryKeyValue = rowToDelete.Cells["ID"].Value.ToString();
                            bool deletionSuccessful = AddtoArchive(primaryKeyValue);

                            if (deletionSuccessful)
                            {
                                displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_acadHead_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                                MessageBox.Show("Account archived successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error archiving account.");
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
        }

        private void formAccounts_SAO_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }

        private void dgvacadHead_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                headerCheckbox.Checked = true;

            }

            switch (e.KeyCode)
            {
                case Keys.F1:
                    formGeneratedForm formGeneratedForm = new formGeneratedForm();
                    formGeneratedForm.setData4(role, "Submit", this);
                    formGeneratedForm.ShowDialog();
                    break;
                case Keys.F10:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToPDF(dgvacadHead, saveFileDialog.FileName);
                        MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        Process.Start(saveFileDialog.FileName);
                    }
                    break;
                case Keys.F11:
                    try
                    {
                        using (SaveFileDialog saveFileDialog1 = new SaveFileDialog())
                        {
                            saveFileDialog1.Filter = "Excel Files|*.xlsx";
                            saveFileDialog1.Title = "Save As Excel File";

                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                string filePath = saveFileDialog1.FileName;


                                // Your SQL query to retrieve data from the database
                                string query = "SELECT ID, UPPER(Firstname) as Firstname, UPPER(Middlename) as Middlename, UPPER(Lastname) as Lastname FROM tbl_acadHead_accounts";

                                // Create a new Excel application
                                Excel.Application excelApp = new Excel.Application();
                                excelApp.Visible = false; // You can set this to true for debugging purposes

                                // Create a new Excel workbook
                                Excel.Workbook workbook = excelApp.Workbooks.Add();

                                // Create a new Excel worksheet
                                Excel.Worksheet worksheet = (Excel.Worksheet)workbook.Worksheets.Add();

                                // Customizing the table appearance
                                Excel.Range tableRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, 4]]; // Assuming there are four columns

                                // Execute the query and populate the Excel worksheet
                                using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                                {
                                    connection.Open();
                                    using (SqlCommand command = new SqlCommand(query, connection))
                                    {
                                        SqlDataReader reader = command.ExecuteReader();

                                        for (int i = 1; i <= reader.FieldCount; i++)
                                        {
                                            worksheet.Cells[1, i] = reader.GetName(i - 1);
                                            worksheet.Cells[1, i].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.FromArgb(68, 114, 196));
                                            worksheet.Cells[1, i].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
                                        }

                                        int rowIndex = 2;
                                        while (reader.Read())
                                        {
                                            for (int i = 1; i <= reader.FieldCount; i++)
                                            {
                                                worksheet.Cells[rowIndex, i] = reader.GetValue(i - 1).ToString();
                                            }
                                            rowIndex++;
                                        }

                                        reader.Close();
                                    }
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
                    break;

            }
        }
    }
}
