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
using Microsoft.Office.Interop.Excel;
using System.Threading;
using iTextSharp.xmp.options;
using ToolTip = System.Windows.Forms.ToolTip;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formStudentBalanceFee : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private CancellationTokenSource cancellationTokenSource;
        formMakePayment formMakePayment;
        formTransactionHistory formTransactionHistory;
        public formStudentBalanceFee()
        {
            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
            lblDep.Text = FormDeptHeadNavigation.depdes.ToUpper();
            formMakePayment = new formMakePayment();
            formTransactionHistory = new formTransactionHistory();

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnExport, "Export to");
            toolTip.SetToolTip(btnRefresh, "Refresh");
            toolTip.SetToolTip(btnPay, "Pay Balance");
        }
        public void displayFilter()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cbSection.Items.Clear();
                cbSection.Items.Add("All");
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

                if (cbSection.Items.Count > 0)
                {
                    cbSection.SelectedIndex = 0;
                }

                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    string query = "";

                    query = "SELECT (Academic_Year_Start+'-'+Academic_Year_End) AS School_Year FROM tbl_acad ORDER BY Status";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                cbYear.Items.Add(dr["School_Year"].ToString());
                            }
                        }
                    }
                }
                if(cbYear.Items.Count > 0)
                    cbYear.SelectedIndex = 0;
            }
        }
        public void displayOverallSummary()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                string selectedYear = "";

                string query = "SELECT Acad_ID FROM tbl_acad WHERE (Academic_Year_Start+'-'+Academic_Year_End) = @schyear";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@schyear", cbYear.Text);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            selectedYear = dr["Acad_ID"].ToString();
                        }
                    }
                }
                // Calculate total balance fee
                cm = new SqlCommand("SELECT ISNULL(SUM(b.Balance_Fee), 0) AS Total_Balance_Fee FROM tbl_student_accounts s LEFT JOIN tbl_balance_fees b ON s.ID = b.Student_ID LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID WHERE s.Department = @dep AND (@sec = 'All' OR sec.Description = @sec) AND School_Year = @SchYear", cn);
                cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                cm.Parameters.AddWithValue("@sec", cbSection.Text);
                cm.Parameters.AddWithValue("@SchYear", selectedYear);
                decimal totalBalanceFee = Convert.ToDecimal(cm.ExecuteScalar());
                lblCollectableFee.Text = "₱ " + totalBalanceFee.ToString("F2");

                // Calculate total paid amount
                cm = new SqlCommand("SELECT ISNULL(SUM(t.Payment_Amount), 0) AS Total_Paid_Amount FROM tbl_student_accounts s LEFT JOIN tbl_transaction t ON s.ID = t.Student_ID LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID WHERE s.Department = @dep AND (@sec = 'All' OR sec.Description = @sec) AND School_Year = @SchYear", cn);
                cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                cm.Parameters.AddWithValue("@sec", cbSection.Text);
                cm.Parameters.AddWithValue("@SchYear", selectedYear);
                decimal totalPaidAmount = Convert.ToDecimal(cm.ExecuteScalar());
                lblCollectedFee.Text = "₱ " + totalPaidAmount.ToString("F2");

                // Calculate and display the difference
                decimal remainingBalance = totalBalanceFee - totalPaidAmount;

                // Count students who have paid and who have not
                cm = new SqlCommand("SELECT COUNT(DISTINCT s.ID) AS TotalStudents, COUNT(DISTINCT t.Student_ID) AS StudentsWithPayments FROM tbl_student_accounts s LEFT JOIN tbl_transaction t ON s.ID = t.Student_ID LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID WHERE s.Department = @dep AND (@sec = 'All' OR sec.Description = @sec) AND s.Status = 1", cn);
                cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                cm.Parameters.AddWithValue("@sec", cbSection.Text);
                dr = cm.ExecuteReader();

                if (dr.Read())
                {
                    int totalStudents = Convert.ToInt32(dr["TotalStudents"]);
                    int studentsWithPayments = Convert.ToInt32(dr["StudentsWithPayments"]);
                    int studentsWithoutPayments = totalStudents - studentsWithPayments;

                    lblTotalStudents.Text = totalStudents.ToString();
                }
                dr.Close();
            }
        }


        private void ApplyCBFilter(string selectedIndex)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvBalFees.Rows)
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
        public async void displayBalanceFees()
        {
            dgvBalFees.Rows.Clear();
            ptbLoading.Visible = true;
            await Task.Delay(2000);
            int noBalanceCount = 0;
            int paidCount = 0;
            int unpaidCount = 0;
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                string selectedYear = "";

                string query = "SELECT Acad_ID FROM tbl_acad WHERE (Academic_Year_Start+'-'+Academic_Year_End) = @schyear";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@schyear", cbYear.Text);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            selectedYear = dr["Acad_ID"].ToString();
                        }
                    }
                }

                cm = new SqlCommand(@"SELECT
                                    COALESCE(bf.Student_ID, t.Student_ID) AS Student_ID,
                                    s.Lastname AS lname,
                                    s.Firstname AS fname,
                                    sec.Description AS section,
                                    COALESCE(SUM(bf.Balance_Fee), 0) AS Total_Balance_Fee,
                                    COALESCE(SUM(t.Payment_Amount), 0) AS Total_Payment_Amount,
                                    CASE
                                        WHEN COALESCE(SUM(bf.Balance_Fee), 0) < COALESCE(SUM(t.Payment_Amount), 0)
                                            THEN 0
                                        ELSE COALESCE(SUM(bf.Balance_Fee), 0) - COALESCE(SUM(t.Payment_Amount), 0)
                                    END AS Remaining_Balance,
                                    BalDate
                                FROM (
                                    SELECT
                                        Student_ID,
                                        SUM(Balance_Fee) AS Balance_Fee,
                                        School_Year AS BalDate
                                    FROM
                                        dbo.tbl_balance_fees
                                    GROUP BY
                                        Student_ID,
                                        School_Year
                                ) bf
                                FULL JOIN (
                                    SELECT
                                        Student_ID,
                                        SUM(Payment_Amount) AS Payment_Amount
                                    FROM
                                        dbo.tbl_transaction
                                    GROUP BY
                                        Student_ID
                                ) t ON bf.Student_ID = t.Student_ID
                                JOIN dbo.tbl_student_accounts s ON COALESCE(bf.Student_ID, t.Student_ID) = s.ID
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID
                                WHERE
                                    s.Status = 1
                                    AND s.Department = @dep
                                    AND (@sec = 'All' OR sec.Description = @sec)
                                    AND BalDate = @SchoolYear
                                GROUP BY
                                    COALESCE(bf.Student_ID, t.Student_ID),
                                    s.Lastname,
                                    s.Firstname,
                                    sec.Description,
                                    BalDate
                                ORDER BY
                                    s.Lastname;", cn);
                cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                cm.Parameters.AddWithValue("@sec", cbSection.Text);
                cm.Parameters.AddWithValue("@SchoolYear", selectedYear);
                using (dr = cm.ExecuteReader())
                {
                    double totalCollectable = 0;
                    while (dr.Read())
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }
                        int rowIndex = dgvBalFees.Rows.Add(false);

                        double balance = (dr["Remaining_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Remaining_Balance"]);
                        double amount_paid = (dr["Total_Payment_Amount"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Total_Payment_Amount"]);


                        dgvBalFees.Rows[rowIndex].Cells["ID"].Value = dr["Student_ID"].ToString();
                        dgvBalFees.Rows[rowIndex].Cells["lname"].Value = dr["lname"].ToString().ToUpper();
                        dgvBalFees.Rows[rowIndex].Cells["fname"].Value = dr["fname"].ToString().ToUpper();
                        dgvBalFees.Rows[rowIndex].Cells["section"].Value = dr["section"].ToString();
                        dgvBalFees.Rows[rowIndex].Cells["balancefee"].Value = "₱ " + Convert.ToDecimal(balance).ToString("F2");
                        dgvBalFees.Rows[rowIndex].Cells["paidfee"].Value = "₱ " + Convert.ToDecimal(amount_paid).ToString("F2");

                        dgvBalFees.Columns["balancefee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["balancefee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["paidfee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["paidfee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Rows[rowIndex].Cells["status"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);

                        if (balance <= 0 && amount_paid <= 0)
                        {
                            dgvBalFees.Rows[rowIndex].Cells["status"].Value = "No balance";
                            noBalanceCount++;
                        }
                        else if (balance <= 0)
                        {
                            dgvBalFees.Rows[rowIndex].Cells["status"].Value = "Paid";
                            paidCount++;
                        }
                        else
                        {
                            dgvBalFees.Rows[rowIndex].Cells["status"].Value = "Unpaid";
                            unpaidCount++;
                        }
                        rowIndex++;
                        totalCollectable += balance;
                    }
                    int paid = noBalanceCount + paidCount;
                    lblStudentPaid.Text = paid.ToString();
                    lblStudentNotPaid.Text = unpaidCount.ToString();
                }
            }
            ptbLoading.Visible = false;

            string targetStatus = "Unpaid";  // The status you want to search for

            foreach (DataGridViewRow row in dgvBalFees.Rows)
            {
                // Get the value in the "Status" column
                DataGridViewCell statusCell = row.Cells["status"]; // Replace "Status" with the actual column name
                string statusValue = statusCell.Value?.ToString();

                // Show or hide the row based on the search result
                row.Visible = (statusValue != null && statusValue.Equals(targetStatus, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void formAttendanceReport_Load(object sender, EventArgs e)
        {
            displayFilter();
            displayOverallSummary();

          
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
                ExportToPDF(dgvBalFees, saveFileDialog.FileName);
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
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("Students Balance Fee Report", headerFont1);
            Paragraph titleParagraph2 = new Paragraph("Section:" + cbSection.Text, headerFont2);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 1);
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding


            // Iterate through DataGridView columns (excluding the last column)
            for (int columnIndex = 0; columnIndex < dataGridView.Columns.Count - 1; columnIndex++)
            {
                DataGridViewColumn column = dataGridView.Columns[columnIndex];
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                pdfTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                // Iterate through DataGridView cells (excluding the last cell in each row)
                for (int columnIndex = 0; columnIndex < row.Cells.Count - 1; columnIndex++)
                {
                    DataGridViewCell cell = row.Cells[columnIndex];
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString(), cellFont));
                    pdfTable.AddCell(pdfCell);
                }
            }


            document.Add(pdfTable);
            document.Close();
        }

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayBalanceFees();
            displayOverallSummary();
        }

        private void btnPaid_Click(object sender, EventArgs e)
        {
            string targetStatus = "Paid";  // The status you want to search for

            foreach (DataGridViewRow row in dgvBalFees.Rows)
            {
                // Get the value in the "Status" column
                DataGridViewCell statusCell = row.Cells["status"]; // Replace "Status" with the actual column name
                string statusValue = statusCell.Value?.ToString();

                // Show or hide the row based on the search result
                row.Visible = (statusValue != null && statusValue.Equals(targetStatus, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvBalFees.Rows)
            {
                row.Visible = true;
            }
        }

        private void btnNotPaid_Click(object sender, EventArgs e)
        {
            string targetStatus = "Unpaid";  // The status you want to search for

            foreach (DataGridViewRow row in dgvBalFees.Rows)
            {
                // Get the value in the "Status" column
                DataGridViewCell statusCell = row.Cells["status"]; // Replace "Status" with the actual column name
                string statusValue = statusCell.Value?.ToString();

                // Show or hide the row based on the search result
                row.Visible = (statusValue != null && statusValue.Equals(targetStatus, StringComparison.OrdinalIgnoreCase));
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            displayFilter();
        }

        private void formStudentBalanceFee_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private void dgvBalFees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvBalFees.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvBalFees.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvBalFees, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void btnMakePayment_Click(object sender, EventArgs e)
        {
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
                        formMakePayment.getForm(this);
                        formMakePayment.searchStudent(dgvBalFees.Rows[rowIndex].Cells[0].Value.ToString());
                        formMakePayment.ShowDialog();
                    }
                }
            }
        }

        private void btnTransacHistory_Click(object sender, EventArgs e)
        {
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
                        formTransactionHistory.displayStudInfo(dgvBalFees.Rows[rowIndex].Cells[0].Value.ToString());
                        formTransactionHistory.ShowDialog();
                    }
                }
            }
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            formMakePayment.getForm(this);
            formMakePayment.ShowDialog();
        }

        private void cbYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayBalanceFees();
            displayOverallSummary();
        }
    }
}
