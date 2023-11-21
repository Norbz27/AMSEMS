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

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formStudentBalanceFee : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        public formStudentBalanceFee()
        {
            InitializeComponent();
        }
        public void displayFilter()
        {
     
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
        public void displayBalanceFees()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("SELECT s.ID AS id, s.Lastname AS lname, s.Firstname AS fname, SUM(b.Balance_Fee) AS Total_Balance_Fee, ISNULL(SUM(distinct t.Payment_Amount), 0) AS amount_paid FROM tbl_student_accounts s LEFT JOIN tbl_balance_fees b ON s.ID = b.Student_ID LEFT JOIN tbl_transaction t ON s.ID = t.Student_ID WHERE s.Department = @dep GROUP BY s.ID, s.Lastname, s.Firstname ORDER BY s.Lastname;", cn);
                cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                using (dr = cm.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        int rowIndex = dgvBalFees.Rows.Add(false);

                        double balance = (Double)dr["Total_Balance_Fee"];
                        double amount_paid = (Double)dr["amount_paid"];
                        double updated_balance = balance - amount_paid;

                        dgvBalFees.Rows[rowIndex].Cells["ID"].Value = dr["id"].ToString();
                        dgvBalFees.Rows[rowIndex].Cells["lname"].Value = dr["lname"].ToString();
                        dgvBalFees.Rows[rowIndex].Cells["fname"].Value = dr["fname"].ToString();
                        dgvBalFees.Rows[rowIndex].Cells["balancefee"].Value = "₱ " + Convert.ToDecimal(balance).ToString("F2");
                        dgvBalFees.Rows[rowIndex].Cells["paidfee"].Value = "₱ " + Convert.ToDecimal(amount_paid).ToString("F2");

                        dgvBalFees.Columns["balancefee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["balancefee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["paidfee"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        dgvBalFees.Columns["paidfee"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                        //dgvBalFees.Rows[rowIndex].Cells["balancefee"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);
                        //dgvBalFees.Rows[rowIndex].Cells["paidfee"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);
                        dgvBalFees.Rows[rowIndex].Cells["status"].Style.Font = new System.Drawing.Font("Poppins", 9F, FontStyle.Bold);
                        if (updated_balance <= 0)
                        {
                            dgvBalFees.Rows[rowIndex].Cells["status"].Value = "Paid";
                        }
                        else
                        {
                            dgvBalFees.Rows[rowIndex].Cells["status"].Value = "Unpaid";
                        }
                        rowIndex++;
                    }
                }
            }
        }

        private void formAttendanceReport_Load(object sender, EventArgs e)
        {
            displayFilter();
            displayBalanceFees();
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
            iTextSharp.text.Font headerFont3 = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("Attendance Report", headerFont1);
            Paragraph titleParagraph2 = new Paragraph(cbSection.Text, headerFont2);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);

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

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCBFilter(cbSection.Text);
        }

        private void btnPaid_Click(object sender, EventArgs e)
        {
            ApplyCBFilter("Paid");
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            displayBalanceFees();
        }

        private void btnNotPaid_Click(object sender, EventArgs e)
        {
            ApplyCBFilter("Unpaid");
        }
    }
}
