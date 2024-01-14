using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formArchived_Accounts_DepHead : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        string selectedItem;
        static string account;
        static int role;
        formArchiveSetting form;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public formArchived_Accounts_DepHead()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvArch.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvArch.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            displayFilter();
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");

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

        public void getForm(formArchiveSetting form)
        {
            this.form = form;
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelUnarchive, "Retrieve");

            backgroundWorker.RunWorkerAsync();

        }

        public void displayFilter()
        {
            if (cbDep.InvokeRequired)
            {
                cbDep.Invoke(new Action(() => displayFilter()));
                return;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_Departments", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbDep.Items.Add(dr["Description"].ToString());
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
            if (dgvArch.InvokeRequired)
            {
                dgvArch.Invoke(new Action(() => displayTable(query)));
                return;
            }
            dgvArch.Rows.Clear();

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
                            int rowIndex = dgvArch.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvArch.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                            dgvArch.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                            dgvArch.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
                            dgvArch.Rows[rowIndex].Cells["dept"].Value = dr["dDes"].ToString();
                            dgvArch.Rows[rowIndex].Cells["archived_date"].Value = dr["arcdate"].ToString();

                            // Populate your control column here (change "ControlColumn" to your actual column name)
                            dgvArch.Rows[rowIndex].Cells["option"].Value = option.Image;
                        }
                    }
                }
            }

        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvArch.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvArch.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvArch, cellBounds.Left, cellBounds.Bottom);
            }

            if (e.ColumnIndex == dgvArch.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvArch.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    // Toggle the checkbox value
                    checkBoxCell.Value = !(bool)checkBoxCell.Value;

                    // Check the state of checkboxes and show/hide the panel accordingly
                    UpdatePanelVisibility();
                }
            }
            //System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))))
        }
        private void UpdatePanelVisibility()
        {
            bool anyChecked = false;

            // Iterate through the DataGridView rows to check if any checkboxes are checked
            foreach (DataGridViewRow row in dgvArch.Rows)
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
                                displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");
                                MessageBox.Show("Account deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error deleting record.");
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
                    string deleteQuery = "DELETE FROM tbl_archived_deptHead_accounts WHERE ID = @ID";

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

        private void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("List of Department Head:", headerFont);
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
            columnWidths[2] = 70; // RFID column width
            columnWidths[3] = 70; // First Name column autosize
            columnWidths[4] = 70; // Last Name column autosize
            columnWidths[5] = 86; // Program column width
            columnWidths[6] = 60; // Section column width
            columnWidths[7] = 40; // Year Level column width
            columnWidths[8] = 45; // Status column width
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

        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            ComboBox comboBox = (ComboBox)sender;
            string filtertbl = "tbl_Departments";

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemET = cbDep.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Departments");

                // Construct the query based on the selected descriptions
                string query = "Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID " +
                    "where (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvArch.Rows.Clear();
                            int count = 1;
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvArch.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvArch.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvArch.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                                dgvArch.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
                                dgvArch.Rows[rowIndex].Cells["dept"].Value = dr["dDes"].ToString();
                                dgvArch.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvArch.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
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

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }

        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvArch.Rows)
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

        private void btnReload_Click(object sender, EventArgs e)
        {
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");

            cbDep.Text = String.Empty;
            tbSearch.Text = String.Empty;
        }

        private void btnExpPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvArch, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void exportToToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dgvStudents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell belongs to the "Select" checkbox column
            if (dgvArch.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvArch.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvArch.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvArch.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvStudents_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvArch.Columns["Select"].Index)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);

                // Create a checkbox control and set its state
                CheckBox headerCheckbox = new CheckBox();
                headerCheckbox.Size = new Size(15, 15);

                // Center the checkbox within the header cell
                int x = e.CellBounds.X + (e.CellBounds.Width - headerCheckbox.Width) / 2;
                int y = e.CellBounds.Y + (e.CellBounds.Height - headerCheckbox.Height) / 2;

                headerCheckbox.Location = new Point(x, y);
                headerCheckbox.Checked = AreAllCheckboxesChecked();

                // Handle the checkbox click event
                headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;


                // Check if there are any rows in the DataGridView
                if (dgvArch.Rows.Count > 0)
                {
                    dgvArch.Controls.Add(headerCheckbox);
                }
            }
        }

        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvArch.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvArch.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvArch.Rows)
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

            DialogResult result = MessageBox.Show($"Do you want to delete selected archived account?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                // Iterate through the DataGridView rows to find selected rows
                foreach (DataGridViewRow row in dgvArch.Rows)
                {
                    // Check if the "Select" checkbox is checked in the current row
                    DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                    if (chk.Value != null && (bool)chk.Value)
                    {
                        hasSelectedRow = true; // Set the flag to true if at least one row is selected

                        // Get the student ID or relevant data from the row
                        int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                        bool success = DeleteStudentRecord(id);

                        if (success)
                        {
                            // Add the row to the list of rows to be removed
                            rowsToRemove.Add(row);
                            MessageBox.Show("Account deleted successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete record with ID: " + id);
                        }

                    }
                }
            }

            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");

            dgvArch.Refresh();

            pnControl.Hide();

        }

        private void btnSelUnarchive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvArch.Rows)
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
                DialogResult result = MessageBox.Show("Are you sure to retrieve this accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvArch.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the teacher ID or relevant data from the row
                            int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = Unarchive(id);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                                //MessageBox.Show("Account retrieved successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to archive record with ID: " + id);
                            }
                        }
                    }
                    displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");
                }
            }
        }
        private bool Unarchive(int ID)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    // Check if a record with the same ID already exists in tbl_student_accounts
                    string checkExistingQuery = "SELECT COUNT(*) FROM tbl_sao_accounts WHERE ID = @ID";
                    using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                    {
                        checkExistingCommand.Parameters.AddWithValue("@ID", ID);
                        int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                        if (existingRecordCount == 0)
                        {
                            // No existing record, proceed with unarchiving
                            // Update the status to 1 before inserting
                            //string updateStatusQuery = "UPDATE tbl_archived_deptHead_accounts SET Status = 1 WHERE ID = @ID";
                            //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                            //{
                            //    updateStatusCommand.Parameters.AddWithValue("@ID", ID);
                            //    updateStatusCommand.ExecuteNonQuery();
                            //}


                            // Insert the student record
                            string insertQuery = "SET IDENTITY_INSERT tbl_deptHead_accounts ON; " +
                                "INSERT INTO tbl_deptHead_accounts (Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime) SELECT Unique_ID,ID,Firstname,Lastname,Middlename,Password,Profile_pic,Department,Role,Status,DateTime FROM tbl_archived_deptHead_accounts WHERE ID = @ID; " +
                                "SET IDENTITY_INSERT tbl_deptHead_accounts OFF;";
                            using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                            {
                                sqlCommand.Parameters.AddWithValue("@ID", ID);
                                sqlCommand.ExecuteNonQuery();
                            }

                            // Delete the record from the archived table
                            string deleteQuery = "DELETE FROM tbl_archived_deptHead_accounts WHERE ID = @ID";
                            using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", ID);
                                command.ExecuteNonQuery();
                            }

                            return true;
                        }
                        else
                        {
                            // A record with the same ID already exists in tbl_student_accounts
                            MessageBox.Show("A record with the same ID already exists. No retrieve performed.");
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

        private void retrieveToolStripMenuItem_Click(object sender, EventArgs e)
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

                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to retrieve this record?", "Confirm Retriving Account", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            int primaryKeyValue = Convert.ToInt32(rowToDelete.Cells["ID"].Value);
                            bool deletionSuccessful = Unarchive(primaryKeyValue);

                            if (deletionSuccessful)
                            {
                                displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, FORMAT(Archived_Date, 'yyyy-MM-dd') AS arcdate from tbl_archived_deptHead_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID");
                                MessageBox.Show("Account retrieved successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error retrieving account.");
                            }
                            UseWaitCursor = false;

                        }
                    }
                }
            }
        }

        private void formArchived_Accounts_DepHead_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.loadData();
        }
        private void cb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void formArchived_Accounts_DepHead_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }
    }
}
