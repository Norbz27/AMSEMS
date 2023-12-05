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

namespace AMSEMS.SubForm_Guidance
{
    public partial class formStudAttRecord : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;


        string stud_id, date;

        public bool paystatus = false;

        formAbsReport form;
        public formStudAttRecord()
        {
            InitializeComponent();

        }
        public void getForm(formAbsReport form, string studid)
        {
            this.form = form;
            stud_id = studid;
        } 
        private void formEventConfig_Load(object sender, EventArgs e)
        {
            displayPSY();
            getStudID();
            displayAbsencesRecord();
        }
        public void displayPSY()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_Role where Role_ID = 5", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbRole.Text = dr["Description"].ToString();
                }
                dr.Close();
                cn.Close();
            }
        }

        public void getStudID()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    cm = new SqlCommand("Select Profile_pic from tbl_student_accounts where ID = " + stud_id + "", cn);

                    byte[] imageData = (byte[])cm.ExecuteScalar();

                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            ptbProfile.Image = image;
                        }
                    }
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Middlename,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes, d.Description as depDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                        "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "left join tbl_Departments as d on sa.Department = d.Department_ID " +
                        "left join tbl_status as st on sa.Status = st.Status_ID where ID = " + stud_id + "", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    tbID.Text = dr["ID"].ToString();
                    tbFname.Text = dr["Firstname"].ToString();
                    tbLname.Text = dr["Lastname"].ToString();
                    tbMname.Text = dr["Middlename"].ToString();
                    tbProgram.Text = dr["pDes"].ToString();
                    tbSec.Text = dr["sDes"].ToString();
                    tbYlevel.Text = dr["yDes"].ToString();
                    tbDep.Text = dr["depDes"].ToString();
                    dr.Close();
                    cn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        public void displayAbsencesRecord()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    string query = "SELECT sub.Course_Code AS ccode, sub.Course_Description AS cdes, att.Attendance_date AS abdate FROM tbl_subjects sub RIGHT JOIN tbl_class_list cl ON sub.Course_code = cl.Course_Code RIGHT JOIN tbl_subject_attendance att ON cl.CLass_Code = att.Class_Code WHERE att.Student_Status = 'A' AND Student_ID = @studid";
                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@studid", stud_id);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvAbsencesRecord.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvAbsencesRecord.Rows[rowIndex].Cells["ccode"].Value = dr["ccode"].ToString();
                                dgvAbsencesRecord.Rows[rowIndex].Cells["cdes"].Value = dr["cdes"].ToString();
                                dgvAbsencesRecord.Rows[rowIndex].Cells["absencedate"].Value = dr["abdate"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void btnPdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvAbsencesRecord, saveFileDialog.FileName);
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
            Paragraph titleParagraph = new Paragraph("Attendance Report", headerFont1);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);

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
