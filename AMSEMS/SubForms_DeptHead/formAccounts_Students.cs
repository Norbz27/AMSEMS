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

namespace AMSEMS.SubForms_DeptHead
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
        string tersem, shsquart;

        private bool headerCheckboxAdded = true;

        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CancellationTokenSource cancellationTokenSource;
        public formAccounts_Students()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            cancellationTokenSource = new CancellationTokenSource();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvStudents.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));

        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            displayFilter();
            displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes, ac.Academic_Level_Description as acadDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID left join tbl_academic_level as ac on d.AcadLevel_ID = ac.Academic_Level_ID WHERE sa.Status = 1 AND sa.Department = @dep");

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
        public static void setAccountName(string accountName)
        {
            account = accountName;
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
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnReload, "Refresh");

            backgroundWorker.RunWorkerAsync();

        }

        public void displayFilter()
        {
            if (cbProgram.InvokeRequired || cbSection.InvokeRequired || cbYearlvl.InvokeRequired)
            {
                cbProgram.Invoke(new Action(() => displayFilter()));
                cbSection.Invoke(new Action(() => displayFilter()));
                cbYearlvl.Invoke(new Action(() => displayFilter()));
                return;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbProgram.Items.Clear();
                    cbSection.Items.Clear();
                    cbYearlvl.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Distinct Description from tbl_program p Join tbl_academic_level ac ON p.AcadLevel_ID = ac.Academic_Level_ID LEFT JOIN tbl_student_accounts st ON p.Program_ID = st.Program WHERE Academic_Level_Description = @acadlevel AND st.Department = @depid", cn);
                    cm.Parameters.AddWithValue("@acadlevel", FormDeptHeadNavigation.acadlevel);
                    cm.Parameters.AddWithValue("@depid", FormDeptHeadNavigation.dep);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbProgram.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    cm = new SqlCommand("Select Distinct Description from tbl_section s Join tbl_academic_level ac ON s.AcadLevel_ID = ac.Academic_Level_ID LEFT JOIN tbl_student_accounts st ON s.Section_ID = st.Section WHERE Academic_Level_Description = @acadlevel AND st.Department = @depid", cn);
                    cm.Parameters.AddWithValue("@acadlevel", FormDeptHeadNavigation.acadlevel);
                    cm.Parameters.AddWithValue("@depid", FormDeptHeadNavigation.dep);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbSection.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    cm = new SqlCommand("Select Distinct Description from tbl_year_level y Join tbl_academic_level ac ON y.AcadLevel_ID = ac.Academic_Level_ID LEFT JOIN tbl_student_accounts st ON y.Level_ID = st.Year_Level WHERE Academic_Level_Description = @acadlevel AND st.Department = @depid", cn);
                    cm.Parameters.AddWithValue("@acadlevel", FormDeptHeadNavigation.acadlevel);
                    cm.Parameters.AddWithValue("@depid", FormDeptHeadNavigation.dep);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbYearlvl.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    cm = new SqlCommand("Select Ter_Academic_Sem, SHS_Academic_Sem from tbl_acad where Acad_ID = 1", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    tersem = dr["Ter_Academic_Sem"].ToString();
                    shsquart = dr["SHS_Academic_Sem"].ToString();
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public async void displayTable(string query)
        {
            if (dgvStudents.InvokeRequired)
            {
                dgvStudents.Invoke(new Action(() => displayTable(query)));
                return;
            }
            try
            {
                query = query + " ORDER BY Lastname;";
                dgvStudents.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvStudents.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                                dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
                                dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();

                                if (dr["yDes"] != DBNull.Value)
                                {
                                    if (dr["acadDes"].ToString().Equals("Tertiary"))
                                        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + tersem + "S";
                                    else
                                        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + shsquart + "Q";
                                }
                                else
                                {
                                    dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                                }

                                dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
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
                        formStudentForm.getStudID(dgvStudents.Rows[rowIndex].Cells[0].Value.ToString());
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
                                displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes, ac.Academic_Level_Description as acadDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID left join tbl_academic_level as ac on d.AcadLevel_ID = ac.Academic_Level_ID WHERE sa.Status = 1 AND sa.Department = @dep");
                                MessageBox.Show("Account deleted successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Error deleting student.");
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
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
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 2); // Exclude the first and last columns
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding

            // Set column widths for specific columns (2nd and 6th columns) to autosize
            float[] columnWidths = new float[dataGridView.Columns.Count - 2];
            columnWidths[0] = 70; // No column width
            columnWidths[1] = 70; // ID column width
            columnWidths[2] = 70; // RFID column width
            columnWidths[3] = 70; // First Name column autosize
            columnWidths[4] = 70; // Last Name column autosize
            columnWidths[5] = 86; // Program column width
            columnWidths[6] = 60; // Section column width
            columnWidths[7] = 40; // Year Level column width
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
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
                string query = "Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID " +
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
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvStudents.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                                dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
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

            string query = "Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID " +
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
                            // Add a row and set the checkbox column value to false (unchecked)
                            int rowIndex = dgvStudents.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
                            dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString().ToUpper();
                            dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString().ToUpper();
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
            displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes, ac.Academic_Level_Description as acadDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID left join tbl_academic_level as ac on d.AcadLevel_ID = ac.Academic_Level_ID WHERE sa.Status = 1 AND sa.Department = @dep");

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
        private void cbProgram_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
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
                        string query = "SELECT ID, UPPER(Firstname) as Firstname, UPPER(Middlename) as Middlename, UPPER(Lastname) as Lastname FROM tbl_student_accounts";

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

        private void formAccounts_Students_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }
    }
}
