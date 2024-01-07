using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Entity;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool headerCheckboxAdded = false; // Add this flag
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CheckBox headerCheckbox;
        private CancellationTokenSource cancellationTokenSource;
        private string schoolYear, semester;
        public formSubjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            cancellationTokenSource = new CancellationTokenSource();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            dgvAssignedSub.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvAssignedSub.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dgvAssignedSub.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dgvUnassigned.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // Initialize the header checkbox in the constructor

        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            displayTable();

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

          
            displayAcademicYear();
            displayTableUnassigned();
            backgroundWorker.RunWorkerAsync();

        }
        public void displayAcademicYear()
        {
            cbSem.Items.Clear();
            cbSchoolYear.Items.Clear();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";

                query = "SELECT Acad_ID, Academic_Year_Start +'-'+ Academic_Year_End AS acad_year FROM tbl_acad WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cbSchoolYear.Text = dr["acad_year"].ToString();
                            schoolYear = dr["Acad_ID"].ToString();
                        }
                    }
                }
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter WHERE Status = 1";
                }
                else
                {
                    query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester WHERE Status = 1";
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            cbSem.Text = dr["Description"].ToString();
                            semester = dr["ID"].ToString();
                        }
                    }
                }

                query = "SELECT Acad_ID, Academic_Year_Start +'-'+ Academic_Year_End AS acad_year FROM tbl_acad";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSchoolYear.Items.Add(dr["acad_year"].ToString());
                        }
                    }
                }

                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter";
                }
                else
                {
                    query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester";
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSem.Items.Add(dr["Description"].ToString());
                        }
                    }
                }
            }
        }

        private async void cbSchoolYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvAssignedSub.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID LEFT JOIN tbl_teacher_accounts ta ON shs.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID LEFT JOIN tbl_teacher_accounts ta ON ter.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvAssignedSub.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvAssignedSub.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["assigned"].Value = dr["Name"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                               // dgvAssignedSub.Rows[rowIndex].Cells["option"].Value = option.Image;
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

        private async void cbSem_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgvAssignedSub.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID LEFT JOIN tbl_teacher_accounts ta ON shs.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID LEFT JOIN tbl_teacher_accounts ta ON ter.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvAssignedSub.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvAssignedSub.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["assigned"].Value = dr["Name"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                // dgvAssignedSub.Rows[rowIndex].Cells["option"].Value = option.Image;
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

        public async void displayTable()
        {
            if (dgvAssignedSub == null || dgvAssignedSub.IsDisposed || !dgvAssignedSub.IsHandleCreated)
            {
                return;
            }
            if (dgvAssignedSub.InvokeRequired)
            {
                dgvAssignedSub.Invoke(new Action(() => displayTable()));
                return;
            }
            try
            {
                dgvAssignedSub.Rows.Clear();
                ptbLoading.Visible = true;
                await Task.Delay(2000);
                string query = "";
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_shs_assigned_teacher_to_sub shs ON s.Course_code = shs.Course_code LEFT JOIN tbl_acad ad ON shs.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter sem ON shs.Quarter = sem.Quarter_ID LEFT JOIN tbl_teacher_accounts ta ON shs.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                else
                {
                    query = "SELECT s.Course_code,Course_Description, Units, UPPER(ta.Lastname +', '+ ta.Firstname) Name, (Academic_Year_Start +'-'+ Academic_Year_End) AS acad_year, sem.Description AS Sem FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sem ON ter.Semester = sem.Semester_ID LEFT JOIN tbl_teacher_accounts ta ON ter.Teacher_ID = ta.ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 AND (Academic_Year_Start +'-'+ Academic_Year_End) = @schyear AND sem.Description = @sem ORDER BY 1 DESC";
                }
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        cmd.Parameters.AddWithValue("@schyear", cbSchoolYear.Text);
                        cmd.Parameters.AddWithValue("@sem", cbSem.Text);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvAssignedSub.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvAssignedSub.Rows[rowIndex].Cells["code"].Value = dr["Course_code"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["assigned"].Value = dr["Name"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["schyear"].Value = dr["acad_year"].ToString();
                                dgvAssignedSub.Rows[rowIndex].Cells["sem"].Value = dr["Sem"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                // dgvAssignedSub.Rows[rowIndex].Cells["option"].Value = option.Image;
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

        public async void displayTableUnassigned()
        {
            if (dgvUnassigned == null || dgvUnassigned.IsDisposed || !dgvUnassigned.IsHandleCreated)
            {
                return;
            }
            if (dgvUnassigned.InvokeRequired)
            {
                dgvUnassigned.Invoke(new Action(() => displayTableUnassigned()));
                return;
            }
            try
            {
                dgvUnassigned.Rows.Clear();
                ptbLoading2.Visible = true;
                await Task.Delay(2000);
                string query = "";

                query = "SELECT DISTINCT s.Course_code,Course_Description, Units FROM tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 ORDER BY 1 DESC";
                
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                        cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvUnassigned.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvUnassigned.Rows[rowIndex].Cells["code2"].Value = dr["Course_code"].ToString();
                                dgvUnassigned.Rows[rowIndex].Cells["des2"].Value = dr["Course_Description"].ToString();
                                dgvUnassigned.Rows[rowIndex].Cells["units2"].Value = dr["Units"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                //dgvUnassigned.Rows[rowIndex].Cells["option"].Value = option.Image;
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

                ptbLoading2.Visible = false;
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
            foreach (DataGridViewRow row in dgvAssignedSub.Rows)
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
        void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            iTextSharp.text.Font headerFont2 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font headerFont3 = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("List of Subjects:", headerFont);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

            Paragraph titleParagraph1 = new Paragraph("Printed Date: " + DateTime.Now.ToString("MMMM dd, yyyy"), headerFont);
            titleParagraph1.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph1);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count); // Include all columns
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding

            // Add headers
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                pdfTable.AddCell(cell);
            }

            // Add data rows
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value?.ToString() ?? "", cellFont)); // Handle possible null values
                    pdfTable.AddCell(pdfCell);
                }
            }

            document.Add(pdfTable);
            document.Close();
        }


        private void btnReload_Click(object sender, EventArgs e)
        {
            displayTable();
        }

        private void btnExportPDF_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvAssignedSub, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            CMSExport.Show(btnExport, new System.Drawing.Point(0, btnExport.Height));
        }

        private void btnReload2_Click(object sender, EventArgs e)
        {
            displayTableUnassigned();
        }

        private void tbSearch2_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch2.Text.Trim();
            ApplySearchFilter2(searchKeyword);
        }
        private void ApplySearchFilter2(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvUnassigned.Rows)
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

        private void btnAssignSubject_Click(object sender, EventArgs e)
        {
            formAssignTeacherToSub formAssignTeacherToSub = new formAssignTeacherToSub();
            formAssignTeacherToSub.ShowDialog();
        }
    }
}
