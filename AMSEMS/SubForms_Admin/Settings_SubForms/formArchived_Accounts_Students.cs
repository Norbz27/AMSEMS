using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS.SubForms_Admin
{
    public partial class formArchived_Accounts_Students : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        string selectedItem;
        static string account;
        static int role;
        public formArchived_Accounts_Students()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            dgvStudents.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelUnarchive, "Retrieve");

            displayFilter();


            displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");

        }

        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbProgram.Items.Clear();
                    cbSection.Items.Clear();
                    cbYearlvl.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_program", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbProgram.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    cm = new SqlCommand("Select Description from tbl_section", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbSection.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    cm = new SqlCommand("Select Description from tbl_year_level", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbYearlvl.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

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
            dgvStudents.Rows.Clear();

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
                            int rowIndex = dgvStudents.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                            // Populate your control column here (change "ControlColumn" to your actual column name)
                            dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
                        }
                    }
                }
            }

        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvStudents.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvStudents, cellBounds.Left, cellBounds.Bottom);
            }

            if (e.ColumnIndex == dgvStudents.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvStudents.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
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
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
                                displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
                                MessageBox.Show("Account deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error deleting student.");
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
                    string deleteQuery = "DELETE FROM tbl_student_accounts WHERE ID = @ID";

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
            Paragraph titleParagraph = new Paragraph("List of Students:", headerFont);
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
            string filtertbl = string.Empty;

            if (comboBox == cbProgram)
            {
                filtertbl = "tbl_program";
            }
            else if (comboBox == cbSection)
            {
                filtertbl = "tbl_Section";
            }
            else if (comboBox == cbYearlvl)
            {
                filtertbl = "tbl_year_level";
            }
            else if (comboBox == cbDep)
            {
                filtertbl = "tbl_Departments";
            }

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemP = cbProgram.Text;
                string selectedItemY = cbYearlvl.Text;
                string selectedItemS = cbSection.Text;
                string selectedItemD = cbDep.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
                string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
                string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");
                string descriptionD = await GetSelectedItemDescriptionAsync(selectedItemD, "tbl_Departments");

                // Construct the query based on the selected descriptions
                string query = "Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID " +
                "where (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
                "AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
                "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
                "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionD) ? DBNull.Value : (object)descriptionD);
                        cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                        cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvStudents.Rows.Clear();
                            int count = 1;
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvStudents.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
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


        private async void ApplyStatusFilter(string statusDescription)
        {
            UseWaitCursor = true;
            string selectedItemP = cbProgram.Text;
            string selectedItemY = cbYearlvl.Text;
            string selectedItemS = cbSection.Text;

            string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
            string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
            string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");
            string descriptionD = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Departments");

            string query = "Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID " +
                "where (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
                "AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
                "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
                "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription) " +
                "AND (@StatusDescription IS NULL OR st.Description = @StatusDescription)";

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                    cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionD) ? DBNull.Value : (object)descriptionD);
                    cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                    cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);
                    cmd.Parameters.AddWithValue("@StatusDescription", statusDescription);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvStudents.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            // Add a row and set the checkbox column value to false (unchecked)
                            int rowIndex = dgvStudents.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                            // Populate your control column here (change "ControlColumn" to your actual column name)
                            dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
                        }
                    }
                }
            }
            UseWaitCursor = false;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }

        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
            displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");

            cbProgram.Text = String.Empty;
            cbSection.Text = String.Empty;
            cbYearlvl.Text = String.Empty;
            tbSearch.Text = String.Empty;
        }

        private void btnExpPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvStudents, saveFileDialog.FileName);
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
            if (dgvStudents.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvStudents.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvStudents.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvStudents.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvStudents_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvStudents.Columns["Select"].Index)
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
                if (dgvStudents.Rows.Count != 0)
                {
                    dgvStudents.Controls.Add(headerCheckbox);
                }
            }
        }

        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvStudents.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                // Check if the "Select" checkbox is checked in the current row
                DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                if (chk.Value != null && (bool)chk.Value)
                {
                    hasSelectedRow = true; // Set the flag to true if at least one row is selected

                    // Get the student ID or relevant data from the row
                    int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name
                                                                            // Ask for confirmation from the user
                    DialogResult result = MessageBox.Show($"Delete account with ID {id}?", "Confirm Deletion", MessageBoxButtons.YesNo);
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

            displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");

            dgvStudents.Refresh();

            pnControl.Hide();

        }

        private void btnSetInactive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the student ID or relevant data from the row
                            int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                            // Call your UpdateStudentStatus method to update the record
                            bool success = UpdateStudentStatus(id, 2);

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

                    displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
                }
            }

        }

        private void btnSetActive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the student ID or relevant data from the row
                            int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                            // Call your UpdateStudentStatus method to update the record
                            bool success = UpdateStudentStatus(id, 1);

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
                    displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
                }
            }

        }
        private bool UpdateStudentStatus(int studentID, int status)
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();
                    string updateQuery = "UPDATE tbl_student_accounts SET Status = @Status WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ID", studentID);
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
        private bool UpdateStudentInfo(int studentID, int itemID, string column)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string updateQuery = "UPDATE tbl_student_accounts SET "+ column +" = @ItemID WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, cn))
                    {
                        command.Parameters.AddWithValue("@ID", studentID);
                        command.Parameters.AddWithValue("@ItemID", itemID);
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
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the student ID or relevant data from the row
                            int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                            // Call your UpdateStudentStatus method to update the record
                            bool success = UpdateStudentInfo(id, itemId, column);

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
                    displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
                }
            }
        }

        private void btnSelUnarchive_Click(object sender, EventArgs e)
        {
            // Check if at least one row is selected
            bool hasSelectedRow = false;

            // Iterate through the DataGridView rows to find selected rows
            foreach (DataGridViewRow row in dgvStudents.Rows)
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
                    foreach (DataGridViewRow row in dgvStudents.Rows)
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
                            }
                            else
                            {
                                MessageBox.Show("Failed to archive record with ID: " + id);
                            }
                        }
                    }
                    displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
                }
            }
        }
        private bool Unarchive(int studentID)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();

                    // Update the status to 1 before inserting
                    string updateStatusQuery = "UPDATE tbl_archived_student_accounts SET Status = 1 WHERE ID = @ID";
                    using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                    {
                        updateStatusCommand.Parameters.AddWithValue("@ID", studentID);
                        updateStatusCommand.ExecuteNonQuery();
                    }


                    // Insert the student record
                    string insertQuery = "SET IDENTITY_INSERT tbl_student_accounts ON; " +
                        "INSERT INTO tbl_student_accounts (Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime) SELECT Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime FROM tbl_archived_student_accounts WHERE ID = @ID; " +
                        "SET IDENTITY_INSERT tbl_student_accounts OFF;";
                    using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                    {
                        sqlCommand.Parameters.AddWithValue("@ID", studentID);
                        sqlCommand.ExecuteNonQuery();
                    }

                    // Delete the record from the archived table
                    string deleteQuery = "DELETE FROM tbl_archived_student_accounts WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                    {
                        command.Parameters.AddWithValue("@ID", studentID);
                        command.ExecuteNonQuery();
                    }

                    return true;
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
                                displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_archived_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID");
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
    }
}
