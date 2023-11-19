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

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceReport : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        public formAttendanceReport()
        {
            InitializeComponent();
        }
        public void displayFilter()
        {
            cbMonth.Items.Clear();

            for(int i = 1; i <= 12; i++)
            {
                string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                cbMonth.Items.Add(monthName);
            }
            cbMonth.SelectedItem = DateTime.Now.ToString("MMMM");

            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 10; year <= currentYear; year++)
            {
                cbYear.Items.Add(year.ToString());
            }

            cbYear.SelectedItem = currentYear.ToString();

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_section", cn);
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

        public void displayReport()
        {
            dgvReport.Columns.Clear();
            dgvReport.Rows.Clear();
            dgvReport.Columns.Add("id", "ID");
            dgvReport.Columns.Add("name", "Name");
            dgvReport.Columns.Add("section", "Section");

            List<string> columnNames = new List<string>(); // Declare columnNames here

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                int selectedMonth = DateTime.ParseExact(cbMonth.Text, "MMMM", CultureInfo.InvariantCulture).Month;
                string selectedYear = cbYear.Text;
                string queryEvents = $"SELECT Event_Name, Date_Time FROM tbl_attendance a LEFT JOIN tbl_events e ON e.Event_ID = a.Event_ID WHERE MONTH(Date_Time) = {selectedMonth} AND YEAR(Date_Time) = '{selectedYear}' ORDER BY Event_Name, Date_Time";

                using (cm = new SqlCommand(queryEvents, cn))
                {
                    using (SqlDataReader eventsReader = cm.ExecuteReader())
                    {
                        while (eventsReader.Read())
                        {
                            string eventName = eventsReader["Event_Name"].ToString();
                            DateTime date = (DateTime)eventsReader["Date_Time"];
                            string eventDate = date.ToString("MM-dd-yy");
                            string columnName = $"{eventName}  ({eventDate})";

                            if (!columnNames.Contains(columnName))
                            {
                                dgvReport.Columns.Add(columnName, columnName);
                                dgvReport.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                columnNames.Add(columnName);
                            }
                        }
                    }
                }
                dgvReport.Columns.Add("total", "Total Penalty Fee");
                dgvReport.Columns["total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                // Fetch and display data rows
                string queryData = @"SELECT
                                    s.ID AS ID,
                                    UPPER(CONCAT(s.Firstname, ' ', s.Middlename, ' ', UPPER(s.Lastname))) AS Name,
                                    Description AS Section
                                FROM 
                                    tbl_student_accounts s
								LEFT JOIN 
									tbl_Section sec ON s.Section = sec.Section_ID
                                WHERE
                                    S.Department = 2
                                GROUP BY 
                                    s.ID, 
                                    UPPER(CONCAT(s.Firstname, ' ', s.Middlename, ' ', UPPER(s.Lastname))),
                                    Description
                                ORDER BY 
                                    Name";

                using (cm = new SqlCommand(queryData, cn))
                {
                    cm.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                    using (SqlDataReader dataReader = cm.ExecuteReader())
                    {
                        double overallTotalBalanceFee = 0;
                        while (dataReader.Read())
                        {
                            int id = Convert.ToInt32(dataReader["ID"]);
                            string name = dataReader["Name"].ToString();
                            string section = dataReader["Section"].ToString();

                            dgvReport.Rows.Add(id, name, section);

                            double totalBalanceFee = 0; // Initialize total balance fee for each row

                            foreach (string columnName in columnNames)
                            {
                                string eventName = columnName.Split('(')[0].Trim();
                                DateTime date = DateTime.ParseExact(columnName.Split('(')[1].Split(')')[0].Trim(), "MM-dd-yy", null);

                                // Fetch and add the balance fee data for each dynamic column
                                double balFee = GetBalanceFeeForCell(id, eventName, date);
                                dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[columnName].Value = "₱ " + balFee.ToString("F2");

                                // Accumulate the balance fee for the total column
                                totalBalanceFee += balFee;
                            }

                            // Set the total balance fee for the "total" column
                            dgvReport.Rows[dgvReport.Rows.Count - 1].Cells["total"].Value = "₱ " + totalBalanceFee.ToString("F2");
                            dgvReport.Rows[dgvReport.Rows.Count - 1].Cells["total"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);
                            overallTotalBalanceFee += totalBalanceFee;
                        }
                        
                        lblMonthlyTotalFees.Text = "₱ " + overallTotalBalanceFee.ToString("F2");
                    }
                }
            }
        }

        // Fetch balance fee for the specified cell
        private double GetBalanceFeeForCell(int studentId, string eventName, DateTime date)
        {
            double balanceFee = 0;

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                string query = @"SELECT Balance_Fee FROM tbl_balance_fees
                        WHERE Student_ID = @StudentID
                        AND Event_ID = (SELECT Event_ID FROM tbl_events WHERE Event_Name = @EventName)
                        AND Date = @Date";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentId);
                    cmd.Parameters.AddWithValue("@EventName", eventName);
                    cmd.Parameters.AddWithValue("@Date", date);

                    object result = cmd.ExecuteScalar();

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
            ApplyCBFilter(cbSection.Text);
        }

        private void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
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
            Paragraph titleParagraph3 = new Paragraph(cbMonth.Text + " " + cbYear.Text, headerFont3);
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
    }
}
