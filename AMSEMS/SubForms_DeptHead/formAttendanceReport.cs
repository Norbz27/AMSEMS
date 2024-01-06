using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Threading;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceReport : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private CancellationTokenSource cancellationTokenSource;

        public formAttendanceReport()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnExport, "Export to");
        }
        public void displayFilter()
        {
            cbSection.Items.Clear();

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("SELECT DISTINCT School_Year FROM tbl_events GROUP BY School_Year ORDER BY School_Year ASC", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbYear.Items.Add(dr["School_Year"].ToString());
                }
                dr.Close();
            }

            //int currentYear = DateTime.Now.Year;
            //for (int year = currentYear - 10; year <= currentYear; year++)
            //{
            //    cbYear.Items.Add(year.ToString());
            //}

            cbYear.SelectedIndex = 0;

            cbSection.Items.Add("All");
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Distinct Description from tbl_section s Join tbl_academic_level ac ON s.AcadLevel_ID = ac.Academic_Level_ID LEFT JOIN tbl_student_accounts st ON s.Section_ID = st.Section WHERE Academic_Level_Description = @acadlevel AND st.Department = @depid", cn);
                cm.Parameters.AddWithValue("@acadlevel", FormDeptHeadNavigation.acadlevel);
                cm.Parameters.AddWithValue("@depid", FormDeptHeadNavigation.dep);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbSection.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
            }

            if (cbSection.Items.Count > 0)
            {
                cbSection.SelectedIndex = 0;
            }
        }

        private void ApplyCBFilter(string selectedIndex)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvReport.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(selectedIndex, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break; // No need to check other cells in the row
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }
        public async void displayReport()
        {
            try
            {
                btnRefresh.Enabled = false;
                dgvReport.Columns.Clear();
                dgvReport.Rows.Clear();
                dgvReport.Columns.Add("id", "ID");
                dgvReport.Columns.Add("name", "Name");
                dgvReport.Columns.Add("section", "Section");

                List<string> columnNames = new List<string>();
                columnNames.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    await cn.OpenAsync();

                    await DisplayColumnsAsync(cn, columnNames);
                    await DisplayRowsAsync(cn, columnNames);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                btnRefresh.Enabled = true;
            }
        }

        private async Task DisplayColumnsAsync(SqlConnection cn, List<string> columnNames)
        {
            //int selectedMonth = 0;
            //if (DateTime.TryParseExact(cbMonth.Text, "MMMM", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedMonth))
            //{
            //    selectedMonth = parsedMonth.Month;
            //    // Rest of your code
            //}
            //else
            //{
            //    return;
            //}

            string selectedYear = cbYear.Text;
            string queryEvents = "SELECT Event_Name, a.Date_Time FROM tbl_attendance a " +
                                "LEFT JOIN tbl_events e ON e.Event_ID = a.Event_ID " +
                                "WHERE Attendance = 'True' AND (Exclusive = 'All Students' OR Exclusive = @Department OR Exclusive = 'Specific Students' OR CHARINDEX(@Department, Selected_Departments) > 0) AND e.School_Year = @SelectedYear " +
                                "ORDER BY Event_Name, a.Date_Time";

            using (SqlCommand cm = new SqlCommand(queryEvents, cn))
            {
                //cm.Parameters.AddWithValue("@SelectedMonth", selectedMonth);
                cm.Parameters.AddWithValue("@SelectedYear", selectedYear);
                cm.Parameters.AddWithValue("@Department", FormDeptHeadNavigation.depdes);

                using (SqlDataReader eventsReader = await cm.ExecuteReaderAsync())
                {
                    while (await eventsReader.ReadAsync())
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }
                        string eventName = eventsReader["Event_Name"].ToString();
                        DateTime date = (DateTime)eventsReader["Date_Time"];
                        string eventDate = date.ToString("MM-dd-yy");
                        string columnName = $"{eventName}  ({eventDate})";

                        if (!columnNames.Contains(columnName))
                        {
                            dgvReport.Columns.Add(columnName, columnName);
                            dgvReport.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            dgvReport.Columns[columnName].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            columnNames.Add(columnName);
                        }
                    }
                }
            }

            dgvReport.Columns.Add("total", "Total Penalty Fee");
            dgvReport.Columns["total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReport.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvReport.Columns["total"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private async Task DisplayRowsAsync(SqlConnection cn, List<string> columnNames)
        {
            string queryData = @"SELECT
                            s.ID AS ID,
                            UPPER(CONCAT(s.Lastname, ', ', s.Firstname, ' ', s.Middlename)) AS Name,
                            Description AS Section
                        FROM 
                            tbl_student_accounts s
                        LEFT JOIN 
                            tbl_Section sec ON s.Section = sec.Section_ID
                        WHERE
                            S.Department = @Dep AND (@sec = 'All' OR sec.Description = @sec) AND s.Status = 1
                        GROUP BY 
                            s.ID, 
                            UPPER(CONCAT(s.Lastname, ', ', s.Firstname, ' ', s.Middlename)),
                            Description
                        ORDER BY 
                            Name";

            using (SqlCommand cm = new SqlCommand(queryData, cn))
            {
                cm.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                cm.Parameters.AddWithValue("@sec", cbSection.Text);

                using (SqlDataReader dataReader = await cm.ExecuteReaderAsync())
                {
                    double overallTotalBalanceFee = 0;
                    while (await dataReader.ReadAsync())
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }
                        int id = Convert.ToInt32(dataReader["ID"]);
                        string name = dataReader["Name"].ToString().ToUpper();
                        string section = dataReader["Section"].ToString();

                        dgvReport.Rows.Add(id, name, section);

                        double totalBalanceFee = 0;

                        foreach (string columnName in columnNames)
                        {
                            string eventName = columnName.Split('(')[0].Trim();
                            DateTime date = DateTime.ParseExact(columnName.Split('(')[1].Split(')')[0].Trim(), "MM-dd-yy", null);

                            double balFee = await GetBalanceFeeForCellAsync(id, eventName, date);
                            if (dgvReport.Rows.Count > 0)
                            {
                                dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[columnName].Value = "₱ " + balFee.ToString("F2");
                            }
                            totalBalanceFee += balFee;
                        }

                        dgvReport.Rows[dgvReport.Rows.Count - 1].Cells["total"].Value = "₱ " + totalBalanceFee.ToString("F2");
                        dgvReport.Rows[dgvReport.Rows.Count - 1].Cells["total"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);
                        overallTotalBalanceFee += totalBalanceFee;
                    }
                    lblMonthlyTotalFees.Text = "₱ " + overallTotalBalanceFee.ToString("F2");
                }
            }
        }

        private async Task<double> GetBalanceFeeForCellAsync(int studentId, string eventName, DateTime date)
        {
            double balanceFee = 0;

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();

                string query = @"SELECT Balance_Fee FROM tbl_balance_fees
                        WHERE Student_ID = @StudentID
                        AND Event_ID = (SELECT Event_ID FROM tbl_events WHERE Event_Name = @EventName)
                        AND Date = @Date";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@EventName", eventName);
                    cmd.Parameters.AddWithValue("@Date", date);

                    object result = await cmd.ExecuteScalarAsync();

                    if (result != null && result != DBNull.Value)
                    {
                        balanceFee = Convert.ToDouble(result);
                    }
                }
            }

            return balanceFee;
        }

        private void formAttendanceReport_Load(object sender, EventArgs e)
        {
            displayFilter();
        }

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplyCBFilter(searchKeyword);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvReport, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }

        private void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            iTextSharp.text.Font headerFont2 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font headerFont3 = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("Attendance Report", headerFont1);
            Paragraph titleParagraph2 = new Paragraph(cbSection.Text, headerFont2);
            Paragraph titleParagraph3 = new Paragraph("School Year: " + cbYear.Text, headerFont3);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            titleParagraph3.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);
            document.Add(titleParagraph3);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding


            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                pdfTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString(), cellFont));
                    pdfTable.AddCell(pdfCell);
                }
            }

            document.Add(pdfTable);
            document.Close();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            ptbLoading.Visible = true;
            await Task.Delay(3000);
            displayReport();
            ptbLoading.Visible = false;
        }

        private void formAttendanceReport_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }
    }
}
