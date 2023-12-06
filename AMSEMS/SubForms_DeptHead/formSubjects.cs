using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool headerCheckboxAdded = false; // Add this flag
        string tersem, shsquart;
        private ContextMenuStrip newContextMenuStrip;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CheckBox headerCheckbox = new CheckBox();
        private CancellationTokenSource cancellationTokenSource;
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
            headerCheckbox.Size = new Size(15, 15);
            headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;

            // Initialize the new ContextMenuStrip
            newContextMenuStrip = new ContextMenuStrip();

            // Set properties for the new ContextMenuStrip (optional)
            newContextMenuStrip.ShowImageMargin = true;
            newContextMenuStrip.ImageScalingSize = new Size(20, 20);

            // Add your menu items to the new ContextMenuStrip
            foreach (ToolStripItem item in CMSTeachers.Items)
            {
                newContextMenuStrip.Items.Add(item.Text, item.Image, (sender, e) => { /* Handle menu item click here */ });
            }

            // Attach the new ContextMenuStrip to the button
            btnSetTeach.ContextMenuStrip = newContextMenuStrip;

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            displayFilter();
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1 AND al.Academic_Level_Description = @acadlevel");

            loadCMSControls();

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

            backgroundWorker.RunWorkerAsync();

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
        public void displayFilter()
        {
            if (cbteach.InvokeRequired)
            {
                cbteach.Invoke(new Action(() => displayFilter()));
                return;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    ClrearText();
                    cn.Open();
                    cm = new SqlCommand("Select Lastname from tbl_teacher_accounts where Status = 1", cn);
                    cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbteach.Items.Add(dr["Lastname"].ToString());
                    }
                    dr.Close();

                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ClrearText()
        {
            cbteach.Items.Clear();
            cbteach.Text = "";
            tbSearch.Text = String.Empty;
        }
        public async void displayTable(string query)
        {
            if (dgvSubjects.InvokeRequired)
            {
                dgvSubjects.Invoke(new Action(() => displayTable(query)));
                return;
            }
            try
            {
                dgvSubjects.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(3000);
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@acadlevel", FormDeptHeadNavigation.acadlevel);
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
                                dgvSubjects.Rows[rowIndex].Cells["ID"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
                dgvSubjects.Controls.Add(headerCheckbox);
                ptbLoading.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                headerCheckboxAdded = false;
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


                    headerCheckboxAdded = true; // Set the flag to true
                }
            }
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
        private async void cbAcad_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            System.Windows.Forms.ComboBox comboBox = (System.Windows.Forms.ComboBox)sender;
            string filtertbl = string.Empty;
            if (comboBox == cbteach)
            {
                filtertbl = "tbl_teacher_accounts";
            }

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemET = cbteach.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_teacher_accounts");


                // Construct the query based on the selected descriptions
                string query = "Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID " +
                    "where (@Teacher IS NULL OR t.Lastname = @Teacher) AND (@acadlevel IS NULL OR alAcademic_Level_Description = @acadlevel)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@Teacher", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);
                        cmd.Parameters.AddWithValue("@acadlevel", string.IsNullOrEmpty(FormDeptHeadNavigation.acadlevel) ? DBNull.Value : (object)FormDeptHeadNavigation.acadlevel);


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

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
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
                if (tbl.Equals("tbl_teacher_accounts"))
                {
                    cm = new SqlCommand("Select Lastname from " + tbl + " where Lastname = @SelectedItem", cn);
                    cm.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    string description = null;

                    using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            description = dr["Lastname"].ToString();
                        }
                    }

                    return description;
                }
                else
                {
                    cm = new SqlCommand("Select Academic_level_Description from " + tbl + " where Academic_level_Description = @SelectedItem", cn);
                    cm.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    string description = null;

                    using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                    {
                        if (await dr.ReadAsync())
                        {
                            description = dr["Academic_level_Description"].ToString();
                        }
                    }

                    return description;
                }
            }
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            ClrearText();
            displayFilter();
            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1 AND al.Academic_Level_Description = @acadlevel");
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
        public void loadCMSControls()
        {
            // Assuming you have a ContextMenuStrip named "contextMenuStrip1"

            int itemCount1 = CMSTeachers.Items.Count;

            // Start from the last item (excluding the first item at index 0)
            for (int i = itemCount1 - 1; i > 0; i--)
            {
                CMSTeachers.Items.RemoveAt(i);
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("Select ID, Lastname from tbl_teacher_accounts where Status = 1", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        string itemId = dr["ID"].ToString();
                        var item = new ToolStripMenuItem(dr["Lastname"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 138;
                        item.Height = 26;

                        // Store the table name and ID in the Tag property
                        item.Tag = new Tuple<string, string>("Assigned_Teacher", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSTeachers.Items.Add(item);
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
                DialogResult result = MessageBox.Show("Update Accounts Info?", "Confirm Update", MessageBoxButtons.YesNo);
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
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateSubjectStatus method to update the record
                            bool success = UpdateSubjectInfo(id, itemId, column);

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
                    displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1 AND al.Academic_Level_Description = @acadlevel");
                }
            }
        }
        private bool UpdateSubjectInfo(string id, int itemID, string column)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string updateQuery = "UPDATE tbl_subjects SET " + column + " = @ItemID WHERE Course_code = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, cn))
                    {
                        command.Parameters.AddWithValue("@ID", id);
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

        private void btnSetTeach_Click(object sender, EventArgs e)
        {
            CMSTeachers.Show(btnSetTeach, new System.Drawing.Point(0, btnSetTeach.Height));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
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

        private void formSubjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }
    }
}
