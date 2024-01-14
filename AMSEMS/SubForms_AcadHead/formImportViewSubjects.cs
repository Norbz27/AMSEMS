using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formImportViewSubjects : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        String selectedFilePath;
        int role;

        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();
        BackgroundWorker backgroundWorker;

        public formImportViewSubjects()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += backgroundWorker1_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void formImportView_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnImport, "Import Excel File");

        }

        public void displayTable()
        {
            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Open(tbFilePath.Text);
            var worksheet = (Worksheet)workbook.Sheets[1]; // Assuming the data is on the first sheet

            int rowCount = worksheet.UsedRange.Rows.Count;
            int columnCount = 4; // Set to 4 for 4 columns

            object[,] data = (object[,])worksheet.Range[worksheet.Cells[2, 1], worksheet.Cells[rowCount, columnCount]].Value;

            dgvImport.RowCount = rowCount - 1; // Exclude the header row
            dgvImport.ColumnCount = columnCount + 1; // Add 1 for the row numbers column

            for (int row = 0; row < rowCount - 1; row++)
            {
                dgvImport[0, row].Value = row + 1; // First column with row numbers

                for (int col = 0; col < columnCount; col++)
                {
                    dgvImport[col + 1, row].Value = data[row + 1, col + 1];
                }
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            GC.Collect();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvImport.Rows)
            {
                if (!row.IsNewRow) // Skip the new row at the bottom of the DataGridView
                {
                    string rowNumber = row.Cells[0].Value.ToString(); // Assuming the row numbers are in the first column
                    string column1Value = row.Cells[1].Value.ToString();
                    string column2Value = row.Cells[2].Value.ToString();
                    string column3Value = row.Cells[3].Value.ToString();
                    //string column4Value = row.Cells[4].Value.ToString();
                    //string column5Value = row.Cells[5].Value.ToString();
                    //string column6Value = row.Cells[6].Value.ToString();
                    //string column7Value = row.Cells[7].Value.ToString();


                    int subjects = -1;

                    cn.Open();

                    // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
                    string checkQuery = "SELECT Course_code FROM tbl_subjects WHERE Course_code = @code"; // Modify this query as needed
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                    {
                        checkCommand.Parameters.AddWithValue("@code", column1Value);
                        object result = checkCommand.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            subjects = Convert.ToInt32(result);
                        }
                    }

                    if (subjects == -1)
                    {

                        // Construct your INSERT query
                        string insertQuery = $"INSERT INTO tbl_subjects (Course_code, Course_Description, Units) VALUES (@Course_code, @Course_Description, @Units)";

                        using (SqlCommand command = new SqlCommand(insertQuery, cn))
                        {
                            command.Parameters.AddWithValue("@Course_code", column1Value);
                            command.Parameters.AddWithValue("@Course_Description", column2Value);
                            command.Parameters.AddWithValue("@Units", column3Value);

                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Subject " + subjects + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    cn.Close();

                }
            }
            MessageBox.Show("Successfully Imported Data", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialogEXL = new OpenFileDialog())
                {
                    openFileDialogEXL.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                    if (openFileDialogEXL.ShowDialog() == DialogResult.OK)
                    {
                        string selectedFilePath = openFileDialogEXL.FileName;

                        // Pass the selectedFilePath to Form2 and show Form2
                        tbFilePath.Text = selectedFilePath;
                        progressBar.Style = ProgressBarStyle.Marquee;
                        progressBar.Visible = true;
                        backgroundWorker.RunWorkerAsync(openFileDialogEXL.FileName);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during file loading or data processing
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tbFilePath_TextChanged(object sender, EventArgs e)
        {
            btnDone.Enabled = true;
        }

        private void btnDownload_LinkClicked(object sender, EventArgs e)
        {
            //string excelFilePath = @"F:\Subjects.xlsx";

            //byte[] excelBytes;

            //using (FileStream fs = new FileStream(excelFilePath, FileMode.Open, FileAccess.Read))
            //{
            //    excelBytes = new byte[fs.Length];
            //    fs.Read(excelBytes, 0, (int)fs.Length);
            //}

            //string fileName = Path.GetFileName(excelFilePath);

            //using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            //{
            //    connection.Open();
            //    using (SqlCommand cmd = new SqlCommand("INSERT INTO tbl_blob_files (Blob_Filename, Blob) VALUES (@FileName, @FileData)", connection))
            //    {
            //        cmd.Parameters.AddWithValue("@FileName", fileName);
            //        cmd.Parameters.AddWithValue("@FileData", excelBytes);
            //        cmd.ExecuteNonQuery();
            //    }
            //}

            string query = "SELECT Blob FROM tbl_blob_files WHERE ID = @ID"; // Replace with your actual query

            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", 4);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            byte[] excelData = (byte[])reader["Blob"];

                            // Call a function to save the excelData to a file
                            SaveExcelFile(excelData);
                        }
                    }
                }
            }

        }
        static void SaveExcelFile(byte[] excelData)
        {
            // Write the byte array to a temporary Excel file
            string tempFilePath = Path.Combine(Path.GetTempPath(), "temp_excel.xlsx");
            File.WriteAllBytes(tempFilePath, excelData);

            // Open the save file dialog
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Excel Files|*.xlsx|All Files|*.*";
                saveFileDialog.FileName = "Sample Excel Import.xlsx";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string savePath = saveFileDialog.FileName;
                    File.Copy(tempFilePath, savePath, true);
                }
            }

            // Clean up the temporary file
            File.Delete(tempFilePath);
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            dgvImport.Invoke((MethodInvoker)delegate
            {
                displayTable();
            });
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar.Visible = false;
        }

        private void tbFilePath_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
