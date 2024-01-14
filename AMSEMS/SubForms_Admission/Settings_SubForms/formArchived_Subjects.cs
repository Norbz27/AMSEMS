using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formArchived_Subjects : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        formArchiveSetting form;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public formArchived_Subjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvSubjects.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvSubjects.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            displayFilter();

            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");

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

        private void formSubjects_Load(object sender, EventArgs e)
        {

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelArchive, "Retrieve");

            backgroundWorker.RunWorkerAsync();
        }

        public void displayFilter()
        {
            if (cbAcadLevel.InvokeRequired)
            {
                cbAcadLevel.Invoke(new Action(() => displayFilter()));
                return;
            }
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
            if (dgvSubjects.InvokeRequired)
            {
                dgvSubjects.Invoke(new Action(() => displayTable(query)));
                return;
            }
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
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString().ToUpper();
                                dgvSubjects.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["archived_date"].Value = dr["archdate"].ToString();

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
                                displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
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
                    string deleteQuery = "DELETE FROM tbl_archived_subjects WHERE Course_code = @code";

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
                string query = "Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID " +
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
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString().ToUpper();
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
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");

            tbSearch.Text = String.Empty;

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
                if (dgvSubjects.Rows.Count != 0)
                {
                    dgvSubjects.Controls.Add(headerCheckbox);
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

            DialogResult result = MessageBox.Show($"Do you want to delete selected archived account?", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
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

            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");

            dgvSubjects.Refresh();
            pnControl.Hide();
        }

        private void btnSelUnarchive_Click(object sender, EventArgs e)
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
                            bool success = Unarchive(id);

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
                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                }
            }
        }
        private bool Unarchive(string courseCode)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    // Check if a record with the same ID already exists in tbl_student_accounts
                    string checkExistingQuery = "SELECT COUNT(*) FROM tbl_subjects WHERE Course_code = @ID";
                    using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                    {
                        checkExistingCommand.Parameters.AddWithValue("@ID", courseCode);
                        int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                        if (existingRecordCount == 0)
                        {
                            // No existing record, proceed with unarchiving
                            // Update the status to 1 before inserting
                            //string updateStatusQuery = "UPDATE tbl_archived_subjects SET Status = 1 WHERE Course_code = @ID";
                            //using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                            //{
                            //    updateStatusCommand.Parameters.AddWithValue("@ID", courseCode);
                            //    updateStatusCommand.ExecuteNonQuery();
                            //}


                            // Insert the student record
                            string insertQuery =
                                "INSERT INTO tbl_subjects (Course_code,Course_Description,Units,Image,Status, Assigned_Teacher,Academic_Level) SELECT Course_code,Course_Description,Units,Image,Status, Assigned_Teacher,Academic_Level FROM tbl_archived_subjects WHERE Course_code = @ID; ";
                            using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                            {
                                sqlCommand.Parameters.AddWithValue("@ID", courseCode);
                                sqlCommand.ExecuteNonQuery();
                            }

                            //// Delete the record from the archived table
                            //string deleteQuery = "DELETE FROM tbl_archived_subjects WHERE Course_code = @ID";
                            //using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                            //{
                            //    command.Parameters.AddWithValue("@ID", courseCode);
                            //    command.ExecuteNonQuery();
                            //}

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

        private void formArchived_Subjects_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.loadData();
        }

        private void cb_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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

                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to retrieve this record?", "Confirm Retriving Subject", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            string primaryKeyValue = rowToDelete.Cells["ID"].Value.ToString();
                            bool deletionSuccessful = Unarchive(primaryKeyValue);

                            if (deletionSuccessful)
                            {
                                displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,FORMAT(Archived_Date, 'yyyy-MM-dd') AS archdate, al.Academic_Level_Description as Acad from tbl_archived_subjects as s left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID");
                                MessageBox.Show("Subject retrieved successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error retrieving subject.");
                            }
                            UseWaitCursor = false;

                        }
                    }
                }
            }
        }

        private void formArchived_Subjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }
    }
}
