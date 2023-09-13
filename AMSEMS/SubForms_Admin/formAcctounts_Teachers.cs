﻿using iTextSharp.text.pdf;
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
using static System.Collections.Specialized.BitVector32;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccounts_Teachers : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        static int role;
        static string accountName;
        public formAccounts_Teachers()
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
            cn = new SqlConnection(SQL_Connection.connection);
            dgvTeachers.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvTeachers.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
        }
        public static void setAccountName(String accountName1)
        {
            accountName = accountName1;
        }
        public static void setRole(int role1)
        {
            role = role1;
        }

        private void formAccounts_Teachers_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAdd, "Add Account");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export");

            btnAll.Focus();
            displayFilter();
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
        }

        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbET.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_Departments", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbET.Items.Add(dr["Description"].ToString());
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
                dgvTeachers.Rows.Clear();
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
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvTeachers.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvTeachers.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["dept"].Value = dr["dDes"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvTeachers.Rows[rowIndex].Cells["option"].Value = option.Image;
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
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Active");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Inactive");
        }

        private async void ApplyStatusFilter(string statusDescription)
        {
            UseWaitCursor = true;
            string selectedItemET = cbET.Text;

            string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Departments");

            string query = "Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID " +
              "where (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
              "AND (@StatusDescription IS NULL OR st.Description = @StatusDescription)";

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);
                    cmd.Parameters.AddWithValue("@StatusDescription", statusDescription);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvTeachers.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvTeachers.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvTeachers.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["dept"].Value = dr["dDes"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvTeachers.Rows[rowIndex].Cells["option"].Value = option.Image;
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

        private async void cbET_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            ComboBox comboBox = (ComboBox)sender;
            string filtertbl = "tbl_Departments";

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemET = cbET.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionET = await GetSelectedItemDescriptionAsync(selectedItemET, "tbl_Departments");

                // Construct the query based on the selected descriptions
                string query = "Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID " +
                    "where (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionET) ? DBNull.Value : (object)descriptionET);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvTeachers.Rows.Clear();
                            int count = 1;
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvTeachers.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvTeachers.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["dept"].Value = dr["dDes"].ToString();
                                dgvTeachers.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvTeachers.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
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
            foreach (DataGridViewRow row in dgvTeachers.Rows)
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
            formImportView form2 = new formImportView();
            form2.setRole(role);
            form2.ShowDialog();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            formTeacherForm formTeacherForm = new formTeacherForm();
            formTeacherForm.setData(role, "Submit", this);
            formTeacherForm.ShowDialog();
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
                        formTeacherForm formTeacherForm = new formTeacherForm();
                        formTeacherForm.setData(role, "Update", this);
                        formTeacherForm.getStudID(dgvTeachers.Rows[rowIndex].Cells[1].Value.ToString());
                        formTeacherForm.ShowDialog();
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
                                displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");
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
                    string deleteQuery = "DELETE FROM tbl_teacher_accounts WHERE ID = @ID";

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

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
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
            Paragraph titleParagraph = new Paragraph("List of Teachers:", headerFont);
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
            columnWidths[4] = 86; // Department column width
            columnWidths[5] = 45; // Status column width
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

        private void dgvTeachers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvTeachers.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvTeachers.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                contextMenuStrip2.Show(dgvTeachers, cellBounds.Left, cellBounds.Bottom);
            }

            if (e.ColumnIndex == dgvTeachers.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvTeachers.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
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
            displayTable("Select ID,Firstname,Lastname,Password,d.Description as dDes, st.Description as stDes from tbl_teacher_accounts as te left join tbl_Departments as d on te.Department = d.Department_ID left join tbl_status as st on te.Status = st.Status_ID");

            cbET.Text = String.Empty;
            tbSearch.Text = String.Empty;
            btnAll.Focus();
        }

        private void btnExpPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvTeachers, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void UpdatePanelVisibility()
        {
            bool anyChecked = false;

            // Iterate through the DataGridView rows to check if any checkboxes are checked
            foreach (DataGridViewRow row in dgvTeachers.Rows)
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
            if (dgvTeachers.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvTeachers.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvTeachers.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvTeachers.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvTeachers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvTeachers.Columns["Select"].Index)
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

                // Add the checkbox to the header cell
                dgvTeachers.Controls.Add(headerCheckbox);
            }
        }
        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvTeachers.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvTeachers.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvTeachers.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell &&
                    !(bool)checkBoxCell.Value)
                {
                    return false; // At least one checkbox is not checked
                }
            }

            return true; // All checkboxes are checked

        }
    }
}
