using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        public formImportView()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

        }

        public void setRole(int role)
        {
            this.role = role;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void formImportView_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnImport, "Import Excel File");

            if (role == 2)
            {
                lblHeader1.Text = "Import Department Heads Excel Data";
            }
            else if (role == 3)
            {
                lblHeader1.Text = "Import Guidance Associate Excel Data";
            }
            else if (role == 4)
            {
                lblHeader1.Text = "Import SAO Excel Data";
            }
            else if (role == 5)
            {
                lblHeader1.Text = "Import Students Excel Data";
            }
            else if (role == 6)
            {
                lblHeader1.Text = "Import Teachers Excel Data";
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

            dgvStudents.RowCount = rowCount - 1; // Exclude the header row
            dgvStudents.ColumnCount = columnCount + 1; // Add 1 for the row numbers column

            for (int row = 0; row < rowCount - 1; row++)
            {
                dgvStudents[0, row].Value = row + 1; // First column with row numbers

                for (int col = 0; col < columnCount; col++)
                {
                    dgvStudents[col + 1, row].Value = data[row + 1, col + 1];
                }
            }

            excelApp.Quit();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            GC.Collect();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                if (!row.IsNewRow) // Skip the new row at the bottom of the DataGridView
                {
                    string rowNumber = row.Cells[0].Value.ToString(); // Assuming the row numbers are in the first column
                    string column1Value = row.Cells[1].Value.ToString();
                    string column2Value = row.Cells[2].Value.ToString();
                    string column3Value = row.Cells[3].Value.ToString();
                    string column4Value = row.Cells[4].Value.ToString();
                    //string column5Value = row.Cells[5].Value.ToString();
                    //string column6Value = row.Cells[6].Value.ToString();
                    //string column7Value = row.Cells[7].Value.ToString();

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
                            string insertQuery = $"INSERT INTO tbl_deptHead_accounts (ID, Firstname, Lastname, Middlename, Password) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password)";

                            using (SqlCommand command = new SqlCommand(insertQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", column1Value);
                                command.Parameters.AddWithValue("@Firstname", column2Value);
                                command.Parameters.AddWithValue("@Lastname", column3Value);
                                command.Parameters.AddWithValue("@Middlename", column4Value);
                                command.Parameters.AddWithValue("@Password", generatedPass);

                                command.ExecuteNonQuery();
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

                        // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
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
                            string insertQuery = $"INSERT INTO tbl_guidance_accounts (ID, Firstname, Lastname, Middlename, Password) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password)";

                            using (SqlCommand command = new SqlCommand(insertQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", column1Value);
                                command.Parameters.AddWithValue("@Firstname", column2Value);
                                command.Parameters.AddWithValue("@Lastname", column3Value);
                                command.Parameters.AddWithValue("@Middlename", column4Value);
                                command.Parameters.AddWithValue("@Password", generatedPass);

                                command.ExecuteNonQuery();
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

                        // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
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
                            string insertQuery = $"INSERT INTO tbl_sao_accounts (ID, Firstname, Lastname, Middlename, Password) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password)";

                            using (SqlCommand command = new SqlCommand(insertQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", column1Value);
                                command.Parameters.AddWithValue("@Firstname", column2Value);
                                command.Parameters.AddWithValue("@Lastname", column3Value);
                                command.Parameters.AddWithValue("@Middlename", column4Value);
                                command.Parameters.AddWithValue("@Password", generatedPass);

                                command.ExecuteNonQuery();
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

                        // Check if the student is already present in the Students table based on a unique identifier (e.g., ID or name)
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

                            //// Check if Program exists in the Programs table
                            //string programQuery = $"SELECT Program_ID FROM tbl_program WHERE Description = @ProgramName";
                            //using (SqlCommand programCommand = new SqlCommand(programQuery, cn))
                            //{
                            //    programCommand.Parameters.AddWithValue("@ProgramName", column5Value);
                            //    programId = (int?)programCommand.ExecuteScalar() ?? -1;
                            //}

                            //if (programId == -1)
                            //{
                            //    string insertProgramQuery = $"INSERT INTO tbl_program (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            //    using (SqlCommand insertProgramCommand = new SqlCommand(insertProgramQuery, cn))
                            //    {
                            //        insertProgramCommand.Parameters.AddWithValue("@Description", column5Value);
                            //        programId = Convert.ToInt32(insertProgramCommand.ExecuteScalar());
                            //    }
                            //}

                            //// Check if Program exists in the Section table
                            //string sectionQuery = $"SELECT Section_ID FROM tbl_Section WHERE Description = @Description";
                            //using (SqlCommand sectionCommand = new SqlCommand(sectionQuery, cn))
                            //{
                            //    sectionCommand.Parameters.AddWithValue("@Description", column6Value);
                            //    sectionId = (int?)sectionCommand.ExecuteScalar() ?? -1;
                            //}

                            //if (sectionId == -1)
                            //{
                            //    string insertsectionQuery = $"INSERT INTO tbl_Section (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            //    using (SqlCommand insertsectionCommand = new SqlCommand(insertsectionQuery, cn))
                            //    {
                            //        insertsectionCommand.Parameters.AddWithValue("@Description", column6Value);
                            //        sectionId = Convert.ToInt32(insertsectionCommand.ExecuteScalar());
                            //    }
                            //}

                            //// Check if Program exists in the Year Level table
                            //string yearLevelQuery = $"SELECT Level_ID FROM tbl_year_level WHERE Description = @Description";
                            //using (SqlCommand yearLevelCommand = new SqlCommand(yearLevelQuery, cn))
                            //{
                            //    yearLevelCommand.Parameters.AddWithValue("@Description", column7Value);
                            //    yearLevelId = (int?)yearLevelCommand.ExecuteScalar() ?? -1;
                            //}

                            //if (yearLevelId == -1)
                            //{
                            //    string insertyearLevelQuery = $"INSERT INTO tbl_year_level (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            //    using (SqlCommand insertyearLevelCommand = new SqlCommand(insertyearLevelQuery, cn))
                            //    {
                            //        insertyearLevelCommand.Parameters.AddWithValue("@Description", column7Value);
                            //        yearLevelId = Convert.ToInt32(insertyearLevelCommand.ExecuteScalar());
                            //    }
                            //}
                            //Generated Password
                            int passwordLength = 12;
                            string generatedPass = GeneratePassword(passwordLength); ;

                            // Construct your INSERT query
                            string insertQuery = $"INSERT INTO tbl_student_accounts (ID, Firstname, Lastname, Middlename, Password) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password)";

                            using (SqlCommand command = new SqlCommand(insertQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", column1Value);
                                command.Parameters.AddWithValue("@Firstname", column2Value);
                                command.Parameters.AddWithValue("@Lastname", column3Value);
                                command.Parameters.AddWithValue("@Middlename", column4Value);
                                command.Parameters.AddWithValue("@Password", generatedPass);
                                //command.Parameters.AddWithValue("@ProgramId", programId);
                                //command.Parameters.AddWithValue("@SectionId", sectionId);
                                //command.Parameters.AddWithValue("@YearLevelId", yearLevelId);

                                command.ExecuteNonQuery();
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
                            string insertQuery = $"INSERT INTO tbl_teacher_accounts (ID, Firstname, Lastname, Middlename, Password) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password)";

                            using (SqlCommand command = new SqlCommand(insertQuery, cn))
                            {
                                command.Parameters.AddWithValue("@ID", column1Value);
                                command.Parameters.AddWithValue("@Firstname", column2Value);
                                command.Parameters.AddWithValue("@Lastname", column3Value);
                                command.Parameters.AddWithValue("@Middlename", column4Value);
                                command.Parameters.AddWithValue("@Password", generatedPass);

                                command.ExecuteNonQuery();
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            using (OpenFileDialog openFileDialogEXL = new OpenFileDialog())
            {
                openFileDialogEXL.Filter = "Excel Files|*.xls;*.xlsx;*.xlsm";
                if (openFileDialogEXL.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialogEXL.FileName;

                    // Pass the selectedFilePath to Form2 and show Form2
                    tbFilePath.Text = selectedFilePath;
                    displayTable();
                }
            }
            UseWaitCursor = false;
        }

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
    }
}
