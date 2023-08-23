using ComponentFactory.Krypton.Toolkit;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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


        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        public formImportView(String selectedFilePath)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            this.selectedFilePath = selectedFilePath;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void formImportView_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnImport, "Import Excel File");
            tbFilePath.Text = selectedFilePath;

            var excelApp = new Microsoft.Office.Interop.Excel.Application();
            var workbook = excelApp.Workbooks.Open(selectedFilePath);
            var worksheet = (Worksheet)workbook.Sheets[1]; // Assuming the data is on the first sheet

            int rowCount = worksheet.UsedRange.Rows.Count;
            int columnCount = worksheet.UsedRange.Columns.Count;

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
                    string column5Value = row.Cells[5].Value.ToString();
                    string column6Value = row.Cells[6].Value.ToString();
                    string column7Value = row.Cells[7].Value.ToString();

                    int programId, sectionId, yearLevelId;
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

                        // Check if Program exists in the Programs table
                        string programQuery = $"SELECT Program_ID FROM tbl_program WHERE Description = @ProgramName";
                        using (SqlCommand programCommand = new SqlCommand(programQuery, cn))
                        {
                            programCommand.Parameters.AddWithValue("@ProgramName", column5Value);
                            programId = (int?)programCommand.ExecuteScalar() ?? -1;
                        }

                        if (programId == -1)
                        {
                            string insertProgramQuery = $"INSERT INTO tbl_program (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand insertProgramCommand = new SqlCommand(insertProgramQuery, cn))
                            {
                                insertProgramCommand.Parameters.AddWithValue("@Description", column5Value);
                                programId = Convert.ToInt32(insertProgramCommand.ExecuteScalar());
                            }
                        }

                        // Check if Program exists in the Section table
                        string sectionQuery = $"SELECT Section_ID FROM tbl_Section WHERE Description = @Description";
                        using (SqlCommand sectionCommand = new SqlCommand(sectionQuery, cn))
                        {
                            sectionCommand.Parameters.AddWithValue("@Description", column6Value);
                            sectionId = (int?)sectionCommand.ExecuteScalar() ?? -1;
                        }

                        if (sectionId == -1)
                        {
                            string insertsectionQuery = $"INSERT INTO tbl_Section (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand insertsectionCommand = new SqlCommand(insertsectionQuery, cn))
                            {
                                insertsectionCommand.Parameters.AddWithValue("@Description", column6Value);
                                sectionId = Convert.ToInt32(insertsectionCommand.ExecuteScalar());
                            }
                        }

                        // Check if Program exists in the Year Level table
                        string yearLevelQuery = $"SELECT Level_ID FROM tbl_year_level WHERE Description = @Description";
                        using (SqlCommand yearLevelCommand = new SqlCommand(yearLevelQuery, cn))
                        {
                            yearLevelCommand.Parameters.AddWithValue("@Description", column7Value);
                            yearLevelId = (int?)yearLevelCommand.ExecuteScalar() ?? -1;
                        }

                        if (yearLevelId == -1)
                        {
                            string insertyearLevelQuery = $"INSERT INTO tbl_year_level (Description) VALUES (@Description); SELECT SCOPE_IDENTITY();";
                            using (SqlCommand insertyearLevelCommand = new SqlCommand(insertyearLevelQuery, cn))
                            {
                                insertyearLevelCommand.Parameters.AddWithValue("@Description", column7Value);
                                yearLevelId = Convert.ToInt32(insertyearLevelCommand.ExecuteScalar());
                            }
                        }
                        //Generated Password
                        int passwordLength = 12;
                        string generatedPass = GeneratePassword(passwordLength); ;

                        // Construct your INSERT query
                        string insertQuery = $"INSERT INTO tbl_student_accounts (ID, Firstname, Lastname, Middlename, Password, Program, Section, Year_Level) VALUES (@ID, @Firstname, @Lastname, @Middlename, @Password,@ProgramId, @SectionId, @YearLevelId)";

                        using (SqlCommand command = new SqlCommand(insertQuery, cn))
                        {
                            command.Parameters.AddWithValue("@ID", column1Value);
                            command.Parameters.AddWithValue("@Firstname", column2Value);
                            command.Parameters.AddWithValue("@Lastname", column3Value);
                            command.Parameters.AddWithValue("@Middlename", column4Value);
                            command.Parameters.AddWithValue("@Password", generatedPass);
                            command.Parameters.AddWithValue("@ProgramId", programId);
                            command.Parameters.AddWithValue("@SectionId", sectionId);
                            command.Parameters.AddWithValue("@YearLevelId", yearLevelId);

                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student " + studentId + " is Active!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    cn.Close();
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
    }
}
