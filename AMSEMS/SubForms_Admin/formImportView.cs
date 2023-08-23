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
                    string yearLevelQuery = $"SELECT Section_ID FROM tbl_year_level WHERE Description = @Description";
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
                            sectionId = Convert.ToInt32(insertyearLevelCommand.ExecuteScalar());
                        }
                    }

                    // Construct your INSERT query
                    //string insertQuery = $"INSERT INTO YourTable (Column1, Column2, RowNumber) VALUES ('{column1Value}', '{column2Value}', '{rowNumber}')";

                    //using (SqlCommand command = new SqlCommand(insertQuery, cn))
                    //{
                    //    command.ExecuteNonQuery();
                    //}
                }
            }
        }
    }
}
