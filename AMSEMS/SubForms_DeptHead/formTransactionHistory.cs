using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using System.Data.Entity;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formTransactionHistory : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;


        string stud_id, date;

        public bool paystatus = false;

        formStudentBalanceFee formStudentBalanceFee;
        public formTransactionHistory()
        {
            InitializeComponent();

        }
        public void getForm(formStudentBalanceFee formStudentBalanceFee)
        {
            this.formStudentBalanceFee = formStudentBalanceFee;
        } 
        private void formEventConfig_Load(object sender, EventArgs e)
        {

        }
        public void displayPayRecord(string studid)
        {
            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand(@"SELECT Transaction_ID, Payment_Amount, FORMAT(Date, 'yyyy-MM-dd') AS date FROM tbl_transaction t WHERE Student_ID = @studid;", cn);
                    cm.Parameters.AddWithValue("@studid", studid);
                    using (dr = cm.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            int rowIndex = dgvPaymentRecord.Rows.Add(false);

                            double paidamount = (dr["Payment_Amount"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Payment_Amount"]);

                            dgvPaymentRecord.Rows[rowIndex].Cells["id"].Value = dr["Transaction_ID"].ToString();
                            dgvPaymentRecord.Rows[rowIndex].Cells["paidfee"].Value = paidamount;
                            dgvPaymentRecord.Rows[rowIndex].Cells["datepaid"].Value = dr["date"].ToString();
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void displayStudInfo(string studid)
        {
            try
            {
                displayPayRecord(studid);
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand(@"SELECT
                                        COALESCE(bf.Student_ID, t.Student_ID) AS Student_ID,
                                        s.Lastname AS lname,
                                        s.Firstname AS fname,
                                        s.Middlename AS mname,
                                        sec.Description AS secdes,
                                        COALESCE(SUM(bf.Balance_Fee), 0) AS Total_Balance_Fee,
                                        COALESCE(SUM(t.Payment_Amount), 0) AS Total_Payment_Amount,
                                        CASE
                                            WHEN COALESCE(SUM(bf.Balance_Fee), 0) < COALESCE(SUM(t.Payment_Amount), 0)
                                                THEN 0
                                            ELSE COALESCE(SUM(bf.Balance_Fee), 0) - COALESCE(SUM(t.Payment_Amount), 0)
                                        END AS Remaining_Balance
                                    FROM (
                                        SELECT
                                            Student_ID,
                                            SUM(Balance_Fee) AS Balance_Fee
                                        FROM
                                            dbo.tbl_balance_fees
                                        GROUP BY
                                            Student_ID
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
                                    LEFT JOIN tbl_total_penalty_fee tp ON s.ID = tp.Student_ID
                                    LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID
                                    WHERE
                                        s.Status = 1
                                        AND s.Department = @dep
                                        AND s.ID = @id
                                    GROUP BY
                                        COALESCE(bf.Student_ID, t.Student_ID),
                                        s.Lastname,
                                        s.Firstname,
                                        s.Middlename,
                                        sec.Description;", cn);
                    cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    cm.Parameters.AddWithValue("@id", studid);
                    using (dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            this.stud_id = dr["Student_ID"].ToString();
                            string name = dr["lname"].ToString() + ", " + dr["fname"].ToString() + " " + dr["mname"].ToString();
                            
                            double balance = (dr["Remaining_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Remaining_Balance"]);
                            double amount_paid = (dr["Total_Payment_Amount"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Total_Payment_Amount"]);

                            lblID.Text = stud_id;
                            lblName.Text = name.ToUpper();
                            lblSection.Text = dr["secdes"].ToString();
                            lblBalanceFee.Text = "₱ " + balance.ToString("F2");
                            lblAmountPaid.Text = "₱ " + amount_paid.ToString("F2");
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void btnPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvPaymentRecord, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }
        private void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();
            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("Student Transaction History", headerFont1);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

            // Add student details section as a title
            document.Add(new Paragraph("\nStudent Information", headerFont));

            // Add student information on separate lines
            Paragraph studentInfoLine1 = new Paragraph();
            studentInfoLine1.Add(new Chunk("Student ID: " + lblID.Text, cellFont));
            studentInfoLine1.Alignment = Element.ALIGN_LEFT;  // Align to the left
            document.Add(studentInfoLine1);

            Paragraph studentInfoLine2 = new Paragraph();
            studentInfoLine2.Add(new Chunk("Student Name: " + lblName.Text, cellFont));
            studentInfoLine2.Alignment = Element.ALIGN_LEFT;  // Align to the left
            document.Add(studentInfoLine2);

            Paragraph studentInfoLine6 = new Paragraph();
            studentInfoLine6.Add(new Chunk("Section: " + lblSection.Text, cellFont));
            studentInfoLine6.Alignment = Element.ALIGN_LEFT;  // Align to the left
            document.Add(studentInfoLine6); 

            // Add attendance table
            document.Add(new Paragraph("\nPayment Record", headerFont));

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
