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

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccounts_Students : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        string selectedItem;
        static string account;
        static int role;
        public formAccounts_Students()
        {
            InitializeComponent();
            
            cn = new SqlConnection(SQL_Connection.connection);
            lblAccountName.Text = account;
            btnAll.Focus();
        }
        public static void setAccountName(string accountName)
        {
            account =  accountName;
        }

        public static void setRole(int roleID)
        {
            role = roleID;
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            formStudentForm formStudentForm = new formStudentForm();
            formStudentForm.setData(role, "Submit", this);
            formStudentForm.ShowDialog();
        }

        private void formAccounts_Students_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnAddStudent, "Add Account");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export to PDF");

            btnAll.Focus();

            displayFilter();

            dgvStudents.Rows.Clear();
            displayTable("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_status as st on sa.Status = st.Status_ID");
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
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_section", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbSection.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Description from tbl_year_level", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbYearlvl.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void displayTable(string query)
        {
            try
            {
                dgvStudents.Rows.Clear();
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
                                dgvStudents.Rows.Add(
                                    count++,
                                    dr["ID"].ToString(),
                                    dr["RFID"].ToString(),
                                    dr["Firstname"].ToString(),
                                    dr["Lastname"].ToString(),
                                    dr["pDes"].ToString(),
                                    dr["sDes"].ToString(),
                                    dr["yDes"].ToString(),
                                    dr["stDes"].ToString()
                                );
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                contextMenuStrip2.Show(dgvStudents, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            displayTable("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                        "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "left join tbl_status as st on sa.Status = st.Status_ID");
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Active");
        }

        private void btnInactive_Click(object sender, EventArgs e)
        {
            ApplyStatusFilter("Inactive");

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
                        formStudentForm formStudentForm = new formStudentForm();
                        formStudentForm.setData(role, "Update", this);
                        formStudentForm.getStudID(dgvStudents.Rows[rowIndex].Cells[1].Value.ToString());
                        formStudentForm.ShowDialog();
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
                                displayTable("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_status as st on sa.Status = st.Status_ID");
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

        private void btnExport_Click(object sender, EventArgs e)
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
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            formImportView form2 = new formImportView();
            form2.setRole(role);
            form2.ShowDialog();
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

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemP = cbProgram.Text;
                string selectedItemY = cbYearlvl.Text;
                string selectedItemS = cbSection.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
                string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
                string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");

                // Construct the query based on the selected descriptions
                string query = "Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                    "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                    "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                    "left join tbl_status as st on sa.Status = st.Status_ID " +
                    "where (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
                    "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
                    "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                        cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                        cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvStudents.Rows.Clear();
                            int count = 1;
                            while (dr.Read())
                            {
                                dgvStudents.Rows.Add(
                                    count++,
                                    dr["ID"].ToString(),
                                    dr["RFID"].ToString(),
                                    dr["Firstname"].ToString(),
                                    dr["Lastname"].ToString(),
                                    dr["pDes"].ToString(),
                                    dr["sDes"].ToString(),
                                    dr["yDes"].ToString(),
                                    dr["stDes"].ToString()
                                );
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

                cm = new SqlCommand("Select Description from "+tbl+" where Description = @SelectedItem", cn);
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

            string query = "Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa " +
                "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                "left join tbl_status as st on sa.Status = st.Status_ID " +
                "where (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
                "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
                "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription) " +
                "AND (@StatusDescription IS NULL OR st.Description = @StatusDescription)";

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                    cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                    cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);
                    cmd.Parameters.AddWithValue("@StatusDescription", statusDescription);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvStudents.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            dgvStudents.Rows.Add(
                                count++,
                                dr["ID"].ToString(),
                                dr["RFID"].ToString(),
                                dr["Firstname"].ToString(),
                                dr["Lastname"].ToString(),
                                dr["pDes"].ToString(),
                                dr["sDes"].ToString(),
                                dr["yDes"].ToString(),
                                dr["stDes"].ToString()
                            );
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
            displayTable("Select ID,RFID,Firstname,Lastname,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_status as st on sa.Status = st.Status_ID");

            cbProgram.Text = String.Empty;
            cbSection.Text = String.Empty;
            cbYearlvl.Text = String.Empty;
            tbSearch.Text = String.Empty;
            btnAll.Focus();
        }
    }
}
