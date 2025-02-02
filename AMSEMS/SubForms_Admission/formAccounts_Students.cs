﻿using iTextSharp.text;
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

        private CheckBox headerCheckbox = new CheckBox();
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CancellationTokenSource cancellationTokenSource;
        public formAccounts_Students()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            cancellationTokenSource = new CancellationTokenSource();
            lblAccountName.Text = account;
            //btnAll.Focus();

            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvStudents.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvStudents.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));

            // Initialize the header checkbox in the constructor
            headerCheckbox.Size = new Size(15, 15);
            headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;

            dgvStudents.Focus();
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            loadCMSControls();
            displayFilter();
            if (cbStatus.InvokeRequired)
            {
                cbStatus.Invoke(new Action(() => { cbStatus.SelectedIndex = 0; }));
            }
            else
            {
                cbStatus.SelectedIndex = 0;
            }

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
            toolTip.SetToolTip(btnAddStudent, "Add Account");
            toolTip.SetToolTip(btnImport, "Import Excel File");
            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnDepartment, "Set Department");
            toolTip.SetToolTip(btnProgram, "Set Program");
            toolTip.SetToolTip(btnLevel, "Set Year Level");
            toolTip.SetToolTip(btnSection, "Set Section");
            toolTip.SetToolTip(btnMultiDel, "Delete");
            toolTip.SetToolTip(btnSelArchive, "Archive");
            toolTip.SetToolTip(btnSetActive, "Set Active");
            toolTip.SetToolTip(btnSetInactive, "Set Inactive");


            //btnAll.Focus();

            backgroundWorker.RunWorkerAsync();

        }

        public void displayFilter()
        {
            if (cbProgram.InvokeRequired || cbSection.InvokeRequired || cbYearlvl.InvokeRequired || cbDep.InvokeRequired)
            {
                cbProgram.Invoke(new Action(() => displayFilter()));
                cbSection.Invoke(new Action(() => displayFilter()));
                cbYearlvl.Invoke(new Action(() => displayFilter()));
                cbDep.Invoke(new Action(() => displayFilter()));
                return;
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbProgram.Items.Clear();
                    cbSection.Items.Clear();
                    cbYearlvl.Items.Clear();
                    cbDep.Items.Clear();
                    cn.Open();

                    cm = new SqlCommand("Select Description from tbl_program ORDER BY Description", cn);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbProgram.Items.Add(dr["Description"].ToString());
                        }
                    }

                    cm = new SqlCommand("Select Description from tbl_section ORDER BY Description", cn);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSection.Items.Add(dr["Description"].ToString());
                        }
                    }

                    cm = new SqlCommand("Select Description from tbl_year_level ORDER BY Description", cn);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbYearlvl.Items.Add(dr["Description"].ToString());
                        }
                    }

                    cm = new SqlCommand("Select Description from tbl_Departments ORDER BY Description", cn);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbDep.Items.Add(dr["Description"].ToString());
                        }
                    }

                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }


        public async void displayTable()
        {
            if (dgvStudents.InvokeRequired)
            {
                dgvStudents.Invoke(new Action(() => displayTable()));
                return;
            }
            try
            {
                UseWaitCursor = true;
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string selectedItemP = cbProgram.Text;
                string selectedItemY = cbYearlvl.Text;
                string selectedItemS = cbSection.Text;
                string selectedstatus = cbStatus.Text;

                string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
                string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
                string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");
                string descriptionD = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Departments");

                string query = "SELECT ID, RFID, Firstname, Lastname, Password, " +
                   "d.Description as dDes, p.Description as pDes, " +
                   "se.Description as sDes, yl.Description as yDes, " +
                   "st.Description as stDes, ac.Academic_Level_Description as acadDes " +
                   "FROM tbl_student_accounts as sa " +
                   "LEFT JOIN tbl_program as p ON sa.Program = p.Program_ID " +
                   "LEFT JOIN tbl_Section as se ON sa.Section = se.Section_ID " +
                   "LEFT JOIN tbl_year_level as yl ON sa.Year_level = yl.Level_ID " +
                   "LEFT JOIN tbl_Departments as d ON sa.Department = d.Department_ID " +
                   "LEFT JOIN tbl_status as st ON sa.Status = st.Status_ID " +
                   "LEFT JOIN tbl_academic_level as ac ON d.AcadLevel_ID = ac.Academic_Level_ID " +
                   "WHERE (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
                   "AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
                   "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
                   "AND (@StatusDescription = 'All' OR st.Description = @StatusDescription) " +
                   "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription) " +
                   "ORDER BY p.Description, Lastname";

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionD) ? DBNull.Value : (object)descriptionD);
                        cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                        cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);
                        cmd.Parameters.AddWithValue("@StatusDescription", selectedstatus);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvStudents.Rows.Clear();
                            int count = 1;
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
                                dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();
                                //if (dr["yDes"] != DBNull.Value)
                                //{
                                //    if (dr["acadDes"].ToString().Equals("Tertiary"))
                                //        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + tersem + "S";
                                //    else
                                //        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + shsquart + "Q";
                                //}
                                //else
                                //{
                                    dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                                //}
                                dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
                UseWaitCursor = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally
            {
                headerCheckboxAdded = false;
                ptbLoading.Visible = false;
                //dgvStudents.Controls.Add(headerCheckbox);
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
        //private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (cbStatus.Text.Equals("Active"))
        //    {
        //        ApplyStatusFilter("Active");
        //    }
        //    else if (cbStatus.Text.Equals("Inactive"))
        //    {
        //        ApplyStatusFilter("Inactive");
        //    }
        //    else
        //    {
        //        cbSection.Text = String.Empty;
        //        cbProgram.Text = String.Empty;
        //        cbDep.Text = String.Empty;
        //        cbYearlvl.Text = String.Empty;
        //        cbSection.Text = String.Empty;
        //        tbSearch.Text = String.Empty;

        //        displayTable("Select ID,RFID,Firstname,Lastname,Password,d.Description as dDes,p.Description as pDes,se.Description as sDes,yl.Description as yDes,st.Description as stDes, ac.Academic_Level_Description as acadDes from tbl_student_accounts as sa left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID left join tbl_year_level as yl on sa.Year_level = yl.Level_ID left join tbl_Departments as d on sa.Department = d.Department_ID left join tbl_status as st on sa.Status = st.Status_ID left join tbl_academic_level as ac on d.AcadLevel_ID = ac.Academic_Level_ID");
        //    }
        //}

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
                                displayTable();
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            formImportView form2 = new formImportView();
            form2.setRole(role);
            form2.reloadFormStud(this);
            form2.ShowDialog();
        }

        private async void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            if (cbStatus.SelectedIndex == 0)
            {
                cbProgram.Text = "";
                cbDep.Text = "";
                cbSection.Text = "";
                cbYearlvl.Text = "";
            }
            ptbLoading.Visible = true;
            await Task.Delay(2000);
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
            else if (comboBox == cbStatus)
            {
                filtertbl = "tbl_status";
            }

            if (!string.IsNullOrEmpty(filtertbl))
            {
                // Get the selected items from all ComboBoxes
                string selectedItemP = cbProgram.Text;
                string selectedItemY = cbYearlvl.Text;
                string selectedItemS = cbSection.Text;
                string selectedItemD = cbDep.Text;
                string selectedstatus = cbStatus.Text;

                // Get the corresponding descriptions for the selected items
                string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
                string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
                string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");
                string descriptionD = await GetSelectedItemDescriptionAsync(selectedItemD, "tbl_Departments");

                // Construct the query based on the selected descriptions
                string query = "SELECT ID, RFID, Firstname, Lastname, Password, " +
               "d.Description as dDes, p.Description as pDes, " +
               "se.Description as sDes, yl.Description as yDes, " +
               "st.Description as stDes, ac.Academic_Level_Description as acadDes " +
               "FROM tbl_student_accounts as sa " +
               "LEFT JOIN tbl_program as p ON sa.Program = p.Program_ID " +
               "LEFT JOIN tbl_Section as se ON sa.Section = se.Section_ID " +
               "LEFT JOIN tbl_year_level as yl ON sa.Year_level = yl.Level_ID " +
               "LEFT JOIN tbl_Departments as d ON sa.Department = d.Department_ID " +
               "LEFT JOIN tbl_status as st ON sa.Status = st.Status_ID " +
               "LEFT JOIN tbl_academic_level as ac ON d.AcadLevel_ID = ac.Academic_Level_ID " +
               "WHERE (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
               "AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
               "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
               "AND (@StatusDescription = 'All' OR st.Description = @StatusDescription) " +
               "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)" +
               "ORDER BY p.Description, Lastname";


                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
                        cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionD) ? DBNull.Value : (object)descriptionD);
                        cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
                        cmd.Parameters.AddWithValue("@StatusDescription", string.IsNullOrEmpty(selectedstatus) ? DBNull.Value : (object)selectedstatus);
                        cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);

                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            dgvStudents.Rows.Clear();
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
                                dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
                                dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();

                                //if (dr["yDes"] != DBNull.Value)
                                //{
                                //    if (dr["acadDes"].ToString().Equals("Tertiary"))
                                //        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + tersem + "S";
                                //    else
                                //        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + shsquart + "Q";
                                //}
                                //else
                                //{
                                    dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
                                //}

                                dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            UseWaitCursor = false;
            ptbLoading.Visible = false;
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

        //private async void ApplyStatusFilter()
        //{
        //    UseWaitCursor = true;
        //    string selectedItemP = cbProgram.Text;
        //    string selectedItemY = cbYearlvl.Text;
        //    string selectedItemS = cbSection.Text;
        //    string selectedstatus = cbStatus.Text;

        //    string descriptionP = await GetSelectedItemDescriptionAsync(selectedItemP, "tbl_program");
        //    string descriptionY = await GetSelectedItemDescriptionAsync(selectedItemY, "tbl_year_level");
        //    string descriptionS = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Section");
        //    string descriptionD = await GetSelectedItemDescriptionAsync(selectedItemS, "tbl_Departments");

        //    string query = "SELECT ID, RFID, Firstname, Lastname, Password, " +
        //       "d.Description as dDes, p.Description as pDes, " +
        //       "se.Description as sDes, yl.Description as yDes, " +
        //       "st.Description as stDes, ac.Academic_Level_Description as acadDes " +
        //       "FROM tbl_student_accounts as sa " +
        //       "LEFT JOIN tbl_program as p ON sa.Program = p.Program_ID " +
        //       "LEFT JOIN tbl_Section as se ON sa.Section = se.Section_ID " +
        //       "LEFT JOIN tbl_year_level as yl ON sa.Year_level = yl.Level_ID " +
        //       "LEFT JOIN tbl_Departments as d ON sa.Department = d.Department_ID " +
        //       "LEFT JOIN tbl_status as st ON sa.Status = st.Status_ID " +
        //       "LEFT JOIN tbl_academic_level as ac ON d.AcadLevel_ID = ac.Academic_Level_ID " +
        //       "WHERE (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) " +
        //       "AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) " +
        //       "AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) " +
        //       "AND (@StatusDescription = 'All' OR st.Description = @StatusDescription) " +
        //       "AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)";

        //    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
        //    {
        //        cn.Open();

        //        using (SqlCommand cmd = new SqlCommand(query, cn))
        //        {
        //            cmd.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(descriptionP) ? DBNull.Value : (object)descriptionP);
        //            cmd.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(descriptionD) ? DBNull.Value : (object)descriptionD);
        //            cmd.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(descriptionY) ? DBNull.Value : (object)descriptionY);
        //            cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(descriptionS) ? DBNull.Value : (object)descriptionS);
        //            cmd.Parameters.AddWithValue("@StatusDescription", selectedstatus);

        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                dgvStudents.Rows.Clear();
        //                int count = 1;
        //                while (dr.Read())
        //                {
        //                    // Add a row and set the checkbox column value to false (unchecked)
        //                    int rowIndex = dgvStudents.Rows.Add(false);

        //                    // Populate other columns, starting from index 1
        //                    dgvStudents.Rows[rowIndex].Cells["ID"].Value = dr["ID"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["RFID"].Value = dr["RFID"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["Fname"].Value = dr["Firstname"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["Lname"].Value = dr["Lastname"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["Dep"].Value = dr["dDes"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["program"].Value = dr["pDes"].ToString();
        //                    dgvStudents.Rows[rowIndex].Cells["section"].Value = dr["sDes"].ToString();
        //                    if (dr["yDes"] != DBNull.Value)
        //                    {
        //                        if (dr["acadDes"].ToString().Equals("Tertiary"))
        //                            dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + tersem + "S";
        //                        else
        //                            dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString() + "-" + shsquart + "Q";
        //                    }
        //                    else
        //                    {
        //                        dgvStudents.Rows[rowIndex].Cells["ylvl"].Value = dr["yDes"].ToString();
        //                    }
        //                    dgvStudents.Rows[rowIndex].Cells["status"].Value = dr["stDes"].ToString();

        //                    // Populate your control column here (change "ControlColumn" to your actual column name)
        //                    dgvStudents.Rows[rowIndex].Cells["option"].Value = option.Image;
        //                }
        //            }
        //        }
        //    }
        //    UseWaitCursor = false;
        //}

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
            displayTable();
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

                if (!headerCheckboxAdded) // Check if the checkbox has already been added
                {
                    // Center the checkbox within the header cell
                    //int x = e.CellBounds.X + (e.CellBounds.Width - headerCheckbox.Width) / 2;
                    //int y = e.CellBounds.Y + (e.CellBounds.Height - headerCheckbox.Height) / 2;

                    //headerCheckbox.Location = new Point(x, y);
                    //headerCheckbox.BackColor = Color.White;
                    //headerCheckbox.Checked = AreAllCheckboxesChecked();

                    //if (dgvStudents.Rows.Count != 0)
                    //{
                    //    dgvStudents.Controls.Add(headerCheckbox);
                    //}

                    ////headerCheckboxAdded = true;
                }
            }
        }
        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;
            UpdateCheckBoxes(headerCheckbox.Checked);
        }

        private void HeaderButton_Click(object sender, EventArgs e)
        {
            cbSelection.Checked = true;
            UpdateCheckBoxes(cbSelection.Checked);
        }
        private void btnCheckMissinginfo_Click(object sender, EventArgs e)
        {
            cbSelection.Checked = true;
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                // Check if the row has data in the specified columns
                object departmentValue = row.Cells["Dep"].Value;
                object sectionValue = row.Cells["section"].Value;
                object levelValue = row.Cells["ylvl"].Value;
                object programValue = row.Cells["program"].Value;

                bool hasEmptyColumn = string.IsNullOrEmpty(departmentValue?.ToString()) ||
                                      string.IsNullOrEmpty(sectionValue?.ToString()) ||
                                      string.IsNullOrEmpty(levelValue?.ToString()) ||
                                      string.IsNullOrEmpty(programValue?.ToString());

                // If none of the specified columns are empty, check the checkbox
                if (hasEmptyColumn)
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null)
                    {
                        checkBoxCell.Value = true;
                    }
                }
                else
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null)
                    {
                        checkBoxCell.Value = false;
                    }
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvStudents.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private void btnCheckCompleteInfo_Click(object sender, EventArgs e)
        {
            cbSelection.Checked = true;
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                // Check if the row has data in the specified columns
                object departmentValue = row.Cells["Dep"].Value;
                object sectionValue = row.Cells["section"].Value;
                object levelValue = row.Cells["ylvl"].Value;
                object programValue = row.Cells["program"].Value;

                bool hasEmptyColumn = string.IsNullOrEmpty(departmentValue?.ToString()) ||
                                      string.IsNullOrEmpty(sectionValue?.ToString()) ||
                                      string.IsNullOrEmpty(levelValue?.ToString()) ||
                                      string.IsNullOrEmpty(programValue?.ToString());

                // If none of the specified columns are empty, check the checkbox
                if (!hasEmptyColumn)
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null)
                    {
                        checkBoxCell.Value = true;
                    }
                }
                else
                {
                    DataGridViewCheckBoxCell checkBoxCell = row.Cells["Select"] as DataGridViewCheckBoxCell;
                    if (checkBoxCell != null)
                    {
                        checkBoxCell.Value = false;
                    }
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvStudents.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private void UpdateCheckBoxes(bool isChecked)
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = isChecked;
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

                    // Get the ID or relevant data from the row
                    int id = Convert.ToInt32(row.Cells["ID"].Value); // Replace "ID" with the actual column name

                    // Call a method to perform the deletion of the record
                    bool success = DeleteStudentRecord(id);

                    if (success)
                    {
                        // Add the row to the list of rows to be removed
                        rowsToRemove.Add(row);
                    }
                }
            }

            // Show a MessageBox outside the loop based on the overall result
            if (hasSelectedRow)
            {
                if (rowsToRemove.Count > 0)
                {
                    MessageBox.Show("Selected item(s) deleted successfully.", "Deletion Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No items selected for deletion.", "Deletion Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No items selected for deletion.", "Deletion Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            displayTable();

            cbSelection.Checked = false;
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

                    bool failedUpdate = false; // Flag to track whether any update operation fails

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the student ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateStudentStatus method to update the record
                            bool success = UpdateStudentStatus(id, 2);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                failedUpdate = true; // Set the flag to true if any update operation fails
                            }
                        }
                    }

                    // Refresh the displayed table after updating
                    displayTable();

                    // Clear the "Select All" checkbox
                    cbSelection.Checked = false;

                    // Show messages based on the results
                    if (failedUpdate)
                    {
                        MessageBox.Show("Some accounts failed to update. Please check and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Selected accounts set as Inactive successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
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
                    // Check if the "Department" column is not null and not empty
                    object departmentValue = row.Cells["Dep"].Value;
                    object sectionValue = row.Cells["section"].Value;
                    object levelValue = row.Cells["ylvl"].Value;
                    object programValue = row.Cells["program"].Value;

                    bool hasEmptyColumn = string.IsNullOrEmpty(departmentValue?.ToString()) ||
                                          string.IsNullOrEmpty(sectionValue?.ToString()) ||
                                          string.IsNullOrEmpty(levelValue?.ToString()) ||
                                          string.IsNullOrEmpty(programValue?.ToString());

                    if (!hasEmptyColumn)
                    {
                        hasSelectedRow = true; // Set the flag to true if at least one row is selected and there are no empty columns
                    }
                    else
                    {
                        MessageBox.Show("Cannot set account as active. Missing some information for selected account.");
                        return; // Exit the method if a row has empty columns
                    }
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

                    bool failedUpdate = false; // Flag to track whether any update operation fails

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Check if the "Department" column is not null and not empty
                            object departmentValue = row.Cells["Dep"].Value;
                            object sectionValue = row.Cells["section"].Value;
                            object levelValue = row.Cells["ylvl"].Value;
                            object programValue = row.Cells["program"].Value;

                            bool hasEmptyColumn = string.IsNullOrEmpty(departmentValue?.ToString()) ||
                                                  string.IsNullOrEmpty(sectionValue?.ToString()) ||
                                                  string.IsNullOrEmpty(levelValue?.ToString()) ||
                                                  string.IsNullOrEmpty(programValue?.ToString());

                            if (!hasEmptyColumn)
                            {
                                // Get the student ID or relevant data from the row
                                string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                                // Call your UpdateStudentStatus method to update the record
                                bool success = UpdateStudentStatus(id, 1);

                                if (success)
                                {
                                    // Add the row to the list of rows to be removed
                                    rowsToRemove.Add(row);
                                }
                                else
                                {
                                    failedUpdate = true; // Set the flag to true if any update operation fails
                                }
                            }
                        }
                    }

                    // Refresh the displayed table after updating
                    displayTable();

                    // Clear the "Select All" checkbox
                    cbSelection.Checked = false;

                    // Show messages based on the results
                    if (failedUpdate)
                    {
                        MessageBox.Show("Some accounts failed to update. Please check and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Selected accounts set as Active successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }


        private bool UpdateStudentStatus(string studentID, int status)
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
        private bool UpdateStudentInfo(string studentID, int itemID, string column)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string updateQuery = "UPDATE tbl_student_accounts SET " + column + " = @ItemID WHERE ID = @ID";

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

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            CMSDepartment.Show(btnDepartment, new System.Drawing.Point(0, btnDepartment.Height));
        }
        private void btnProgram_Click(object sender, EventArgs e)
        {
            CMSProgram.Show(btnProgram, new System.Drawing.Point(0, btnProgram.Height));
        }

        private void btnLevel_Click(object sender, EventArgs e)
        {
            CMSLevel.Show(btnLevel, new System.Drawing.Point(0, btnLevel.Height));
        }

        private void btnSection_Click(object sender, EventArgs e)
        {
            CMSSection.Show(btnSection, new System.Drawing.Point(0, btnSection.Height));
        }
        public void loadCMSControls()
        {
            // Assuming you have a ContextMenuStrip named "contextMenuStrip1"

            int itemCount1 = CMSDepartment.Items.Count;
            int itemCount2 = CMSProgram.Items.Count;
            int itemCount3 = CMSLevel.Items.Count;
            int itemCount4 = CMSSection.Items.Count;

            // Start from the last item (excluding the first item at index 0)
            for (int i = itemCount1 - 1; i > 0; i--)
            {
                CMSDepartment.Items.RemoveAt(i);
            }

            for (int i = itemCount2 - 1; i > 0; i--)
            {
                CMSProgram.Items.RemoveAt(i);
            }

            for (int i = itemCount3 - 1; i > 0; i--)
            {
                CMSLevel.Items.RemoveAt(i);
            }

            for (int i = itemCount4 - 1; i > 0; i--)
            {
                CMSSection.Items.RemoveAt(i);
            }

            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("Select Department_ID,Description from tbl_Departments ORDER BY Description", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        int itemId = Convert.ToInt32(dr["Department_ID"]);
                        var item = new ToolStripMenuItem(dr["Description"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 138;
                        item.Height = 26;

                        // Store the table name and ID in the Tag property
                        item.Tag = new Tuple<string, int>("Department", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSDepartment.Items.Add(item);
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Program_ID,Description from tbl_Program ORDER BY Description", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        int itemId = Convert.ToInt32(dr["Program_ID"]);
                        var item = new ToolStripMenuItem(dr["Description"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 198;
                        item.Height = 26;

                        // Store the table name and ID in the Tag property
                        item.Tag = new Tuple<string, int>("Program", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSProgram.Items.Add(item);
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Level_ID,Description from tbl_year_level ORDER BY Description", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        int itemId = Convert.ToInt32(dr["Level_ID"]);
                        var item = new ToolStripMenuItem(dr["Description"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 128;
                        item.Height = 26;

                        // Store the table name in the Tag property
                        item.Tag = new Tuple<string, int>("Year_Level", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSLevel.Items.Add(item);
                    }
                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Section_ID,Description from tbl_Section ORDER BY Description", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        // Add a new ToolStripMenuItem
                        int itemId = Convert.ToInt32(dr["Section_ID"]);
                        var item = new ToolStripMenuItem(dr["Description"].ToString());

                        // Set margin for the item (adjust the values as needed)
                        item.Margin = new Padding(10, 0, 0, 0);
                        item.AutoSize = false;
                        item.Width = 128;
                        item.Height = 26;

                        // Store the table name in the Tag property
                        item.Tag = new Tuple<string, int>("Section", itemId);

                        // Assign a common event handler for all menu items
                        item.Click += ContextMenuItem_Click;

                        // Add the item to the context menu
                        CMSSection.Items.Add(item);
                    }
                    dr.Close();
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error");
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

                    bool failedUpdate = false; // Flag to track whether any update operation fails

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the student ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your UpdateStudentStatus method to update the record
                            bool success = UpdateStudentInfo(id, itemId, column);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rows.Add(row);
                            }
                            else
                            {
                                failedUpdate = true; // Set the flag to true if any update operation fails
                            }
                        }
                    }

                    // Refresh the displayed table after updating
                    displayTable();

                    // Clear the "Select All" checkbox
                    cbSelection.Checked = false;

                    // Show messages based on the results
                    if (failedUpdate)
                    {
                        MessageBox.Show("Some accounts failed to update. Please check and try again.", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Selected accounts updated successfully.", "Update Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        private void btnSelArchive_Click(object sender, EventArgs e)
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
                DialogResult result = MessageBox.Show("Are you sure you want to archive the selected accounts?", "Confirm Update", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    // Create a list to store the rows to be removed
                    List<DataGridViewRow> rowsToRemove = new List<DataGridViewRow>();

                    bool failedArchive = false; // Flag to track whether any archive operation fails

                    // Iterate through the DataGridView rows to update selected rows
                    foreach (DataGridViewRow row in dgvStudents.Rows)
                    {
                        // Check if the "Select" checkbox is checked in the current row
                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)row.Cells["Select"]; // Replace "Select" with the actual checkbox column name
                        if (chk.Value != null && (bool)chk.Value)
                        {
                            // Get the ID or relevant data from the row
                            string id = row.Cells["ID"].Value.ToString(); // Replace "ID" with the actual column name

                            // Call your AddtoArchive method to update the record
                            bool success = AddtoArchive(id);

                            if (success)
                            {
                                // Add the row to the list of rows to be removed
                                rowsToRemove.Add(row);
                            }
                            else
                            {
                                failedArchive = true; // Set the flag to true if any archive operation fails
                            }
                        }
                    }

                    // Refresh the displayed table after archiving
                    displayTable();

                    // Clear the "Select All" checkbox
                    cbSelection.Checked = false;

                    // Show messages based on the results
                    if (failedArchive)
                    {
                        MessageBox.Show("Some accounts failed to archive. Please check and try again.", "Archive Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("Selected accounts archived successfully.", "Archive Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
        private bool AddtoArchive(string studentID)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    // Check if a record with the same ID already exists in tbl_student_accounts
                    string checkExistingQuery = "SELECT COUNT(*) FROM tbl_archived_student_accounts WHERE ID = @ID";
                    using (SqlCommand checkExistingCommand = new SqlCommand(checkExistingQuery, cn))
                    {
                        checkExistingCommand.Parameters.AddWithValue("@ID", studentID);
                        int existingRecordCount = (int)checkExistingCommand.ExecuteScalar();

                        if (existingRecordCount == 0)
                        {
                            // No existing record, proceed with unarchiving
                            // Update the status to 1 before inserting
                            string updateStatusQuery = "UPDATE tbl_student_accounts SET Status = 2 WHERE ID = @ID";
                            using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, cn))
                            {
                                updateStatusCommand.Parameters.AddWithValue("@ID", studentID);
                                updateStatusCommand.ExecuteNonQuery();
                            }

                            // Insert the student record
                            string insertQuery = "INSERT INTO tbl_archived_student_accounts (Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime) SELECT Unique_ID,ID,RFID,Firstname,Lastname,Middlename,Password,Profile_pic,Program,Section,Year_Level,Department,Role,Status,DateTime FROM tbl_student_accounts WHERE ID = @ID";
                            using (SqlCommand sqlCommand = new SqlCommand(insertQuery, cn))
                            {
                                sqlCommand.Parameters.AddWithValue("@ID", studentID);
                                sqlCommand.ExecuteNonQuery();
                            }

                            // Delete the record from the archived table
                            string deleteQuery = "DELETE FROM tbl_student_accounts WHERE ID = @ID";
                            using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", studentID);
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
                        string query = "SELECT ID, UPPER(Firstname) as Firstname, UPPER(Middlename) as Middlename, UPPER(Lastname) as Lastname FROM tbl_student_accounts sa LEFT JOIN tbl_program as p ON sa.Program = p.Program_ID LEFT JOIN tbl_Section as se ON sa.Section = se.Section_ID LEFT JOIN tbl_year_level as yl ON sa.Year_level = yl.Level_ID LEFT JOIN tbl_Departments as d ON sa.Department = d.Department_ID LEFT JOIN tbl_status as st ON sa.Status = st.Status_ID LEFT JOIN tbl_academic_level as ac ON d.AcadLevel_ID = ac.Academic_Level_ID  WHERE (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) AND (@StatusDescription = 'All' OR st.Description = @StatusDescription) AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)";

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
                                command.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(cbProgram.Text) ? DBNull.Value : (object)cbProgram.Text);
                                command.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(cbDep.Text) ? DBNull.Value : (object)cbDep.Text);
                                command.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(cbYearlvl.Text) ? DBNull.Value : (object)cbYearlvl.Text);
                                command.Parameters.AddWithValue("@StatusDescription", string.IsNullOrEmpty(cbStatus.Text) ? DBNull.Value : (object)cbStatus.Text);
                                command.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(cbSection.Text) ? DBNull.Value : (object)cbSection.Text);
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
                        DataGridViewRow rowToUpdate = dataGridView.Rows[rowIndex];

                        // Check if the "Department," "Section," "Year_Level," and "Program" columns are not null and not empty
                        object departmentValue = rowToUpdate.Cells["Dep"].Value;
                        object sectionValue = rowToUpdate.Cells["section"].Value;
                        object levelValue = rowToUpdate.Cells["ylvl"].Value;
                        object programValue = rowToUpdate.Cells["program"].Value;

                        bool hasEmptyColumn = string.IsNullOrEmpty(departmentValue?.ToString()) ||
                                              string.IsNullOrEmpty(sectionValue?.ToString()) ||
                                              string.IsNullOrEmpty(levelValue?.ToString()) ||
                                              string.IsNullOrEmpty(programValue?.ToString());

                        if (hasEmptyColumn)
                        {
                            MessageBox.Show("Cannot set the account as active. Missing some information for this account.");
                        }
                        else
                        {
                            DialogResult confirmationResult = MessageBox.Show("Set accounts as Active?", "Confirm Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (confirmationResult == DialogResult.Yes)
                            {
                                string primaryKeyValue = rowToUpdate.Cells["ID"].Value.ToString();
                                bool updateSuccessful = UpdateStudentStatus(primaryKeyValue, 1);

                                if (updateSuccessful)
                                {
                                    displayTable();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to update record.");
                                }
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
                            bool deletionSuccessful = UpdateStudentStatus(primaryKeyValue, 2);

                            if (deletionSuccessful)
                            {
                                displayTable();
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
                                displayTable();
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

        private void btnDataMigration_Click(object sender, EventArgs e)
        {
            formDataMigration formDataMigration = new formDataMigration();
            formDataMigration.ShowDialog();
        }

        private void formAccounts_Students_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                cbSelection.Checked = true;
            }
        }

        private void dgvStudents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.A)
            {
                cbSelection.Checked = true;

                UpdateCheckBoxes(cbSelection.Checked);
            }

            switch (e.KeyCode)
            {
                case Keys.F1:
                    formStudentForm formStudentForm = new formStudentForm();
                    formStudentForm.setData(role, "Submit", this);
                    formStudentForm.ShowDialog();
                    break;
                case Keys.F2:
                    formDataMigration formDataMigration = new formDataMigration();
                    formDataMigration.ShowDialog();
                    break;
                case Keys.F3:
                    formImportView form2 = new formImportView();
                    form2.setRole(role);
                    form2.reloadFormStud(this);
                    form2.ShowDialog();
                    break;
                case Keys.F10:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ExportToPDF(dgvStudents, saveFileDialog.FileName);
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
                                string query = "SELECT ID, UPPER(Firstname) as Firstname, UPPER(Middlename) as Middlename, UPPER(Lastname) as Lastname FROM tbl_student_accounts sa LEFT JOIN tbl_program as p ON sa.Program = p.Program_ID LEFT JOIN tbl_Section as se ON sa.Section = se.Section_ID LEFT JOIN tbl_year_level as yl ON sa.Year_level = yl.Level_ID LEFT JOIN tbl_Departments as d ON sa.Department = d.Department_ID LEFT JOIN tbl_status as st ON sa.Status = st.Status_ID LEFT JOIN tbl_academic_level as ac ON d.AcadLevel_ID = ac.Academic_Level_ID  WHERE (@ProgramDescription IS NULL OR p.Description = @ProgramDescription) AND (@DepartmentDescription IS NULL OR d.Description = @DepartmentDescription) AND (@YearLevelDescription IS NULL OR yl.Description = @YearLevelDescription) AND (@StatusDescription = 'All' OR st.Description = @StatusDescription) AND (@SectionDescription IS NULL OR se.Description = @SectionDescription)";

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
                                        command.Parameters.AddWithValue("@ProgramDescription", string.IsNullOrEmpty(cbProgram.Text) ? DBNull.Value : (object)cbProgram.Text);
                                        command.Parameters.AddWithValue("@DepartmentDescription", string.IsNullOrEmpty(cbDep.Text) ? DBNull.Value : (object)cbDep.Text);
                                        command.Parameters.AddWithValue("@YearLevelDescription", string.IsNullOrEmpty(cbYearlvl.Text) ? DBNull.Value : (object)cbYearlvl.Text);
                                        command.Parameters.AddWithValue("@StatusDescription", string.IsNullOrEmpty(cbStatus.Text) ? DBNull.Value : (object)cbStatus.Text);
                                        command.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(cbSection.Text) ? DBNull.Value : (object)cbSection.Text);
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
