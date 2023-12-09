using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formImportView : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        String selectedFilePath;
        int role;
        DateTime currentDateTime = DateTime.Now;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        //forms
        formAccounts_Teachers form1;
        formAccounts_Students form2;
        formAcctounts_DeptHead form3;
        formAcctounts_Guidance form4;
        formAccounts_SAO form5;

        public formImportView()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            backgroundWorker.DoWork += backgroundWorker1_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
        }

        public void setRole(int role)
        {
            this.role = role;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void reloadFormTeach(formAccounts_Teachers form)
        {
            this.form1 = form;
        }
        public void reloadFormStud(formAccounts_Students form)
        {
            this.form2 = form;
        }
        public void reloadFormDep(formAcctounts_DeptHead form)
        {
            this.form3 = form;
        }
        public void reloadFormGui(formAcctounts_Guidance form)
        {
            this.form4 = form;
        }
        public void reloadFormSao(formAccounts_SAO form)
        {
            this.form5 = form;
        }

        private void formImportView_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnImport, "Import Excel File");

            if (role == 2)
            {
                lblHeader1.Text = "Import Excel Data - Department Heads ";
            }
            else if (role == 3)
            {
                lblHeader1.Text = "Import Excel Data - Guidance Associate";
            }
            else if (role == 4)
            {
                lblHeader1.Text = "Import Excel Data - SAO";
            }
            else if (role == 5)
            {
                lblHeader1.Text = "Import Excel Data - Students";
            }
            else if (role == 6)
            {
                lblHeader1.Text = "Import Excel Data - Teachers";
            }
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
                    string rowNumber = row.Cells[0].Value?.ToString(); // Use the null-conditional operator to handle null values
                    string column1Value = row.Cells[1].Value?.ToString();
                    string column2Value = row.Cells[2].Value?.ToString();
                    string column3Value = row.Cells[3].Value?.ToString();
                    string column4Value = row.Cells[4].Value?.ToString();

                    if (!string.IsNullOrEmpty(rowNumber) && !string.IsNullOrEmpty(column1Value) &&
                        !string.IsNullOrEmpty(column2Value) && !string.IsNullOrEmpty(column3Value) &&
                        !string.IsNullOrEmpty(column4Value))
                    {

                        if (role == 2)
                        {
                            int deptHeadId = -1;

                            cn.Open();

                            // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
                            string checkQuery = "SELECT ID FROM tbl_deptHead_accounts WHERE ID = @ID"; // Modify this query as needed
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                            {
                                checkCommand.Parameters.AddWithValue("@ID", column1Value);
                                object result = checkCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    deptHeadId = Convert.ToInt32(result);
                                }
                            }

                            if (deptHeadId == -1)
                            {
                                //Generated Password
                                int passwordLength = 12;
                                string generatedPass = GeneratePassword(passwordLength); ;

                                // Construct your INSERT query
                                string insertQuery = $"INSERT INTO tbl_deptHead_accounts (ID, Firstname, Lastname, Middlename, Password, DateTime, Status) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password, @DateTime, @Status)";

                                using (SqlCommand command = new SqlCommand(insertQuery, cn))
                                {
                                    command.Parameters.AddWithValue("@ID", column1Value);
                                    command.Parameters.AddWithValue("@Firstname", column2Value);
                                    command.Parameters.AddWithValue("@Lastname", column3Value);
                                    command.Parameters.AddWithValue("@Middlename", column4Value);
                                    command.Parameters.AddWithValue("@Password", generatedPass);
                                    command.Parameters.AddWithValue("@DateTime", currentDateTime);
                                    command.Parameters.AddWithValue("@Status", 2);

                                    command.ExecuteNonQuery();

                                    form3.displayTable();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Department Head " + deptHeadId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            cn.Close();
                        }
                        else if (role == 3)
                        {
                            int guidanceId = -1;

                            cn.Open();

                            string checkQuery = "SELECT ID FROM tbl_guidance_accounts WHERE ID = @ID"; // Modify this query as needed
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                            {
                                checkCommand.Parameters.AddWithValue("@ID", column1Value);
                                object result = checkCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    guidanceId = Convert.ToInt32(result);
                                }
                            }

                            if (guidanceId == -1)
                            {
                                //Generated Password
                                int passwordLength = 12;
                                string generatedPass = GeneratePassword(passwordLength); ;

                                // Construct your INSERT query
                                string insertQuery = $"INSERT INTO tbl_guidance_accounts (ID, Firstname, Lastname, Middlename, Password, DateTime, Status) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password, @DateTime, @Status)";

                                using (SqlCommand command = new SqlCommand(insertQuery, cn))
                                {
                                    command.Parameters.AddWithValue("@ID", column1Value);
                                    command.Parameters.AddWithValue("@Firstname", column2Value);
                                    command.Parameters.AddWithValue("@Lastname", column3Value);
                                    command.Parameters.AddWithValue("@Middlename", column4Value);
                                    command.Parameters.AddWithValue("@Password", generatedPass);
                                    command.Parameters.AddWithValue("@DateTime", currentDateTime);
                                    command.Parameters.AddWithValue("@Status", 2);

                                    command.ExecuteNonQuery();
                                    form4.displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_guidance_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Guidance " + guidanceId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            cn.Close();
                        }
                        else if (role == 4)
                        {
                            int saoId = -1;

                            cn.Open();


                            string checkQuery = "SELECT ID FROM tbl_sao_accounts WHERE ID = @ID"; // Modify this query as needed
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                            {
                                checkCommand.Parameters.AddWithValue("@ID", column1Value);
                                object result = checkCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    saoId = Convert.ToInt32(result);
                                }
                            }

                            if (saoId == -1)
                            {
                                //Generated Password
                                int passwordLength = 12;
                                string generatedPass = GeneratePassword(passwordLength); ;

                                // Construct your INSERT query
                                string insertQuery = $"INSERT INTO tbl_sao_accounts (ID, Firstname, Lastname, Middlename, Password, DateTime, Status) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password, @DateTime, @Status)";

                                using (SqlCommand command = new SqlCommand(insertQuery, cn))
                                {
                                    command.Parameters.AddWithValue("@ID", column1Value);
                                    command.Parameters.AddWithValue("@Firstname", column2Value);
                                    command.Parameters.AddWithValue("@Lastname", column3Value);
                                    command.Parameters.AddWithValue("@Middlename", column4Value);
                                    command.Parameters.AddWithValue("@Password", generatedPass);
                                    command.Parameters.AddWithValue("@DateTime", currentDateTime);
                                    command.Parameters.AddWithValue("@Status", 2);

                                    command.ExecuteNonQuery();
                                    form5.displayTable("Select ID,Firstname,Lastname,Password,st.Description as stDes from tbl_sao_accounts as g left join tbl_status as st on g.Status = st.Status_ID");
                                }
                            }
                            else
                            {
                                MessageBox.Show("SAO " + saoId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            cn.Close();
                        }
                        else if (role == 5)
                        {
                            //int programId, sectionId, yearLevelId;
                            int studentId = -1;

                            cn.Open();


                            string checkQuery = "SELECT ID FROM tbl_student_accounts WHERE ID = @ID"; // Modify this query as needed
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                            {
                                checkCommand.Parameters.AddWithValue("@ID", column1Value);
                                object result = checkCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    studentId = Convert.ToInt32(result);
                                }
                            }

                            if (studentId == -1)
                            {
                                //Generated Password
                                int passwordLength = 12;
                                string generatedPass = GeneratePassword(passwordLength); ;

                                // Construct your INSERT query
                                string insertQuery = $"INSERT INTO tbl_student_accounts (ID, Firstname, Lastname, Middlename, Password, DateTime, Status) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password, @DateTime, @Status)";

                                using (SqlCommand command = new SqlCommand(insertQuery, cn))
                                {
                                    command.Parameters.AddWithValue("@ID", column1Value);
                                    command.Parameters.AddWithValue("@Firstname", column2Value);
                                    command.Parameters.AddWithValue("@Lastname", column3Value);
                                    command.Parameters.AddWithValue("@Middlename", column4Value);
                                    command.Parameters.AddWithValue("@Password", generatedPass);
                                    command.Parameters.AddWithValue("@DateTime", currentDateTime);
                                    command.Parameters.AddWithValue("@Status", 2);

                                    command.ExecuteNonQuery();
                                    form2.displayTable();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Student " + studentId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            cn.Close();
                        }
                        else if (role == 6)
                        {
                            int teacherId = -1;

                            cn.Open();

                            // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
                            string checkQuery = "SELECT ID FROM tbl_teacher_accounts WHERE ID = @ID"; // Modify this query as needed
                            using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                            {
                                checkCommand.Parameters.AddWithValue("@ID", column1Value);
                                object result = checkCommand.ExecuteScalar();
                                if (result != null && result != DBNull.Value)
                                {
                                    teacherId = Convert.ToInt32(result);
                                }
                            }

                            if (teacherId == -1)
                            {
                                //Generated Password
                                int passwordLength = 12;
                                string generatedPass = GeneratePassword(passwordLength); ;

                                // Construct your INSERT query
                                string insertQuery = $"INSERT INTO tbl_teacher_accounts (ID, Firstname, Lastname, Middlename, Password, DateTime, Status) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password, @DateTime, @Status)";

                                using (SqlCommand command = new SqlCommand(insertQuery, cn))
                                {
                                    command.Parameters.AddWithValue("@ID", column1Value);
                                    command.Parameters.AddWithValue("@Firstname", column2Value);
                                    command.Parameters.AddWithValue("@Lastname", column3Value);
                                    command.Parameters.AddWithValue("@Middlename", column4Value);
                                    command.Parameters.AddWithValue("@Password", generatedPass);
                                    command.Parameters.AddWithValue("@DateTime", currentDateTime);
                                    command.Parameters.AddWithValue("@Status", 2);

                                    command.ExecuteNonQuery();

                                    form1.displayTable();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Teacher " + teacherId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            cn.Close();
                        }

                    }
                }
            }

            MessageBox.Show("Successfully Imported Data", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        string GeneratePassword(int length)
        {
            StringBuilder password = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                int randomIndex = RandomGenerator.Next(AllowedChars.Length);
                password.Append(AllowedChars[randomIndex]);
            }

            return password.ToString();
        }

        private async void btnImport_Click(object sender, EventArgs e)
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
                        await Task.Delay(2000);
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
        //private void btnImport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        using (OpenFileDialog openFileDialogEXL = new OpenFileDialog())
        //        {
        //            openFileDialogEXL.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
        //            if (openFileDialogEXL.ShowDialog() == DialogResult.OK)
        //            {
        //                string selectedFilePath = openFileDialogEXL.FileName;

        //                Check the number of columns in the selected Excel file
        //                int columnCount = GetExcelColumnCount(selectedFilePath);

        //                if (columnCount == 3)
        //                {
        //                    Pass the selectedFilePath to Form2 and show Form2
        //                    tbFilePath.Text = selectedFilePath;
        //                    progressBar.Style = ProgressBarStyle.Marquee;
        //                    progressBar.Visible = true;
        //                    backgroundWorker.RunWorkerAsync(selectedFilePath);
        //                }
        //                else
        //                {
        //                    MessageBox.Show("The selected Excel file must have exactly 3 columns.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Handle any exceptions that may occur during file loading or data processing
        //        MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        //private int GetExcelColumnCount(string filePath)
        //{
        //    Excel.Application excelApp = new Excel.Application();
        //    Excel.Workbook workbook = excelApp.Workbooks.Open(filePath);
        //    Excel.Worksheet worksheet = workbook.Sheets[1];

        //    int columnCount = worksheet.UsedRange.Columns.Count;

        //    // Close Excel and release resources
        //    workbook.Close();
        //    excelApp.Quit();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

        //    return columnCount;
        //}

        private void tbFilePath_TextChanged(object sender, EventArgs e)
        {
            btnDone.Enabled = true;
        }

        private void btnDownload_LinkClicked(object sender, EventArgs e)
        {
            //string excelFilePath = @"F:\Book1.xlsx";

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
                    command.Parameters.AddWithValue("@ID", 3);

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
