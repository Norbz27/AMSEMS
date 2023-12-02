using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.Office.Interop.Excel;
using iTextSharp.text.html;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectOverview : Form
    {
        SQLite_Connection conn;

        static FormTeacherNavigation form;
        static string ccode;
        static string classcode;
        static string subacadlvl;
        formAddStudentToSubject form2;

        private List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
        public formSubjectOverview()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
            form2 = new formAddStudentToSubject(this);
            dgvStudents.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAttendanceReport.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
        }
        public static void setCode(string ccode1, string classcode1, string subacadlvl1)
        {
            ccode = ccode1;
            classcode = classcode1;
            subacadlvl = subacadlvl1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;

            if (col == "del")
            {
                // Get the index of the clicked row
                int rowIndex = e.RowIndex;

                // Check if the clicked cell is in a valid row
                if (rowIndex >= 0 && rowIndex < dgvStudents.Rows.Count)
                {
                    // Add the row to the list for deletion
                    rowsToDelete.Add(dgvStudents.Rows[rowIndex]);

                    // Remove the row from the DataGridView
                    dgvStudents.Rows.RemoveAt(rowIndex);
                    btnSave1.Enabled = true;
                    btnCancel1.Visible = true;
                }
            }
        }
        private void btnAddStud_Click(object sender, EventArgs e)
        {
            form2.setSubject(ccode, subacadlvl, classcode);
            form2.ShowDialog();
        }

        private void tbSearchStud1_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearchStud1.Text.Trim();
            ApplySearchFilter(searchKeyword);
        }

        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break;
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }
        private void btnSave1_Click(object sender, EventArgs e)
        {
            btnCancel1.Visible = false;
            btnSave1.Enabled = false;
            if (rowsToDelete.Count > 0)
            {
                // Ask the user for confirmation
                DialogResult result = MessageBox.Show("Are you sure you want to delete the selected rows?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                    {
                        cn.Open();
                        using (SQLiteCommand command = new SQLiteCommand(cn))
                        {
                            string tableName = "tbl_" + classcode;
                            string deleteSql = $"DELETE FROM {tableName} WHERE StudentID = @StudID;";

                            command.CommandText = deleteSql;

                            foreach (DataGridViewRow row in rowsToDelete)
                            {
                                string studentIDToDelete = row.Cells["id"].Value.ToString();

                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@StudID", studentIDToDelete);
                                command.ExecuteNonQuery();
                            }
                        }
                        cn.Close();
                        rowsToDelete.Clear(); // Clear the list after deletion
                    }
                }
            }
        }

        public void displaysubjectinfo()
        {
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string query = "SELECT s.Course_Code AS Ccode, Course_Description, sec.Description AS secdes,Image FROM tbl_subjects s RIGHT JOIN tbl_class_list cl ON s.Course_code = cl.Course_Code LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE s.Course_Code = @Ccode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            string CourseCode = rd["Ccode"].ToString();
                            string CourseDes = rd["Course_Description"].ToString();
                            string secDes = rd["secdes"].ToString();
                            object image = rd["Image"];
                            Image img = conn.ConvertToImage(image);

                            lblSec.Text = secDes;
                            lblSubjectName.Text = CourseDes;
                            ptbSubjectPic.Image = img;
                        }
                    }
                }
            }
        }
        public void displayStudents()
        {
            dgvStudents.Rows.Clear();
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string tblname = "tbl_" + classcode;
                string query = $"SELECT StudentID, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM {tblname} cl LEFT JOIN tbl_students_account s ON cl.StudentID = s.ID";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string studid = rd["StudentID"].ToString();
                            string studname = rd["Name"].ToString();
                            int rowIndex = dgvStudents.Rows.Add(false);
                            dgvStudents.Rows[rowIndex].Cells["studid"].Value = studid;
                            dgvStudents.Rows[rowIndex].Cells["studname"].Value = studname;
                        }
                    }
                }
            }
        }
        public void displayStudentsAttendanceReport()
        {
            dgvAttendanceReport.Rows.Clear();

            for (int i = dgvAttendanceReport.Columns.Count - 1; i >= 2; i--)
            {
                dgvAttendanceReport.Columns.RemoveAt(i);
            }

            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();

                // Fetch distinct attendance dates
                string dateQuery = @"
            SELECT DISTINCT Attendance_date
            FROM tbl_subject_attendance
            WHERE Class_Code = @Class_Code";

                using (SQLiteCommand dateCommand = new SQLiteCommand(dateQuery, con))
                {
                    dateCommand.Parameters.AddWithValue("@Class_Code", classcode);

                    using (SQLiteDataReader dateReader = dateCommand.ExecuteReader())
                    {
                        while (dateReader.Read())
                        {
                            string attendanceDate = dateReader["Attendance_date"].ToString();

                            // Dynamically add columns to the DataGridView
                            dgvAttendanceReport.Columns.Add(attendanceDate, attendanceDate);
                        }
                    }
                }

                // Fetch student attendance details
                string attendanceQuery = @"
            SELECT sa.Student_ID,
                   UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name,
                   sa.Attendance_date,
                   sa.Student_Status
            FROM tbl_subject_attendance sa
            LEFT JOIN tbl_students_account s ON sa.Student_ID = s.ID
            WHERE sa.Class_Code = @Class_Code ORDER BY Name";

                Dictionary<string, int> studentRowIndexMap = new Dictionary<string, int>();

                using (SQLiteCommand attendanceCommand = new SQLiteCommand(attendanceQuery, con))
                {
                    attendanceCommand.Parameters.AddWithValue("@Class_Code", classcode);

                    using (SQLiteDataReader rd = attendanceCommand.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string studid = rd["Student_ID"].ToString();
                            string studname = rd["Name"].ToString();
                            string attendanceDate = rd["Attendance_date"].ToString();
                            string studentStatus = rd["Student_Status"].ToString();

                            int rowIndex;

                            // Check if the student is already added
                            if (studentRowIndexMap.ContainsKey(studid))
                            {
                                rowIndex = studentRowIndexMap[studid];
                            }
                            else
                            {
                                // Add the student to the DataGridView
                                rowIndex = dgvAttendanceReport.Rows.Add(false);
                                dgvAttendanceReport.Rows[rowIndex].Cells["attstudid"].Value = studid;
                                dgvAttendanceReport.Rows[rowIndex].Cells["attstudname"].Value = studname;

                                // Update the map with the new row index
                                studentRowIndexMap[studid] = rowIndex;
                            }

                            // Check if the column exists before trying to set its value
                            if (dgvAttendanceReport.Columns.Contains(attendanceDate))
                            {
                                // Find the dynamically added column index
                                int columnIndex = dgvAttendanceReport.Columns[attendanceDate].Index;

                                // Set the student status in the corresponding column
                                dgvAttendanceReport.Rows[rowIndex].Cells[columnIndex].Value = studentStatus;
                            }
                        }
                    }
                }
            }
        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Students")
            {
                displayStudents();
            }
            else if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Attendance Report")
            {
                displayStudentsAttendanceReport();
            }
        }

        private void btnNewAttendance_Click(object sender, EventArgs e)
        {
            btnNewAttendance.Enabled = false;
            btnSave2.Enabled = true;
            btnCancel2.Visible = true;
            DateTime dateTime = Dt.Value;
            string formattedDate = dateTime.ToString("MMM dd, yy");
            string formattedTime = dateTime.ToString("hh:mm tt");
            string newColumnName = formattedDate + " " + formattedTime;

            // Add a new column to the DataGridView
            dgvAttendanceReport.Columns.Add(newColumnName, newColumnName);

            // Auto-size the newly added column to fit its content
            int columnIndex = dgvAttendanceReport.Columns.Count - 1;
            dgvAttendanceReport.AutoResizeColumn(columnIndex, DataGridViewAutoSizeColumnMode.AllCells);

            // Subscribe to the CellDoubleClick event only once
            dgvAttendanceReport.CellDoubleClick += dgvAttendanceReport_CellDoubleClick;
            dgvAttendanceReport.CellValidating += dgvAttendanceReport_CellValidating;
            dgvAttendanceReport.CellEndEdit += dgvAttendanceReport_CellEndEdit;
            dgvAttendanceReport.EditingControlShowing += dgvAttendanceReport_EditingControlShowing;
        }
        private void dgvAttendanceReport_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            // Subscribe to the KeyPress event for the editing control
            e.Control.KeyPress -= new KeyPressEventHandler(Column_KeyPress);
            e.Control.KeyPress += new KeyPressEventHandler(Column_KeyPress);
        }
        private void dgvAttendanceReport_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is in the newly added column
            string newColumnName = GetNewColumnName(); // Use the method to get the latest column name
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvAttendanceReport.Columns[newColumnName].Index)
            {
                // Set the DataGridView cell to edit mode
                dgvAttendanceReport.BeginEdit(true);
            }
        }
        private void Column_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow backspace
            if (e.KeyChar == '\b')
            {
                return;
            }

            // Convert input to uppercase
            e.KeyChar = char.ToUpper(e.KeyChar);

            // Check if the pressed key is not 'P' or 'A'
            if (e.KeyChar != 'P' && e.KeyChar != 'A')
            {
                e.Handled = true;
            }
        }
        private void dgvAttendanceReport_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Check if the cell is in the newly added column
            string newColumnName = GetNewColumnName();
            if (e.ColumnIndex == dgvAttendanceReport.Columns[newColumnName].Index)
            {
                // Validate the entered value
                string newValue = e.FormattedValue.ToString().ToUpper(); // Convert to uppercase for case-insensitive comparison
                if (newValue != "P" && newValue != "A")
                {
                    e.Cancel = true; // Cancel the editing
                }
            }
        }

        private void dgvAttendanceReport_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the cell is in the newly added column
            string newColumnName = GetNewColumnName();
            if (e.ColumnIndex == dgvAttendanceReport.Columns[newColumnName].Index)
            {
                // Ensure that the value is a single character
                string currentValue = dgvAttendanceReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString();
                if (!string.IsNullOrEmpty(currentValue) && currentValue.Length > 1)
                {
                    dgvAttendanceReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = currentValue[0]; // Keep only the first character
                }
            }
        }

        private string GetNewColumnName()
        {
            if (dgvAttendanceReport.Columns.Count > 0)
            {
                return dgvAttendanceReport.Columns[dgvAttendanceReport.Columns.Count - 1].Name;
            }
            return string.Empty;
        }

        private void btnSave2_Click(object sender, EventArgs e)
        {
            btnNewAttendance.Enabled = true;
            // Assuming your DataGridView is named dgvAttendanceReport
            if (dgvAttendanceReport.Columns.Count < 3)
            {
                MessageBox.Show("No attendance.");
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(conn.connectionString))
            {
                connection.Open();

                foreach (DataGridViewColumn column in dgvAttendanceReport.Columns)
                {
                    if (column.Index >= 2) // Start from the 3rd column
                    {
                        string attendanceDate = column.HeaderText;

                        foreach (DataGridViewRow row in dgvAttendanceReport.Rows)
                        {
                            string studentID = row.Cells["attstudid"].Value.ToString();

                            // Check if the cell value is null
                            object cellValue = row.Cells[column.Index].Value;
                            string studentStatus = (cellValue != null) ? cellValue.ToString() : "A";

                            // Check if the record already exists
                            string selectQuery = @"
                        SELECT COUNT(*)
                        FROM tbl_subject_attendance
                        WHERE Attendance_date = @Attendance_date
                        AND Student_ID = @Student_ID
                        AND Student_Status = @Student_Status;
                    ";

                            using (SQLiteCommand selectCommand = new SQLiteCommand(selectQuery, connection))
                            {
                                selectCommand.Parameters.AddWithValue("@Attendance_date", attendanceDate);
                                selectCommand.Parameters.AddWithValue("@Student_ID", studentID);
                                selectCommand.Parameters.AddWithValue("@Student_Status", studentStatus);

                                int existingRecords = Convert.ToInt32(selectCommand.ExecuteScalar());

                                // If no duplicate record, insert the data
                                if (existingRecords == 0)
                                {
                                    string insertQuery = @"
                                INSERT INTO tbl_subject_attendance (Class_Code, Attendance_date, Student_ID, Student_Status)
                                VALUES (@Class_Code, @Attendance_date, @Student_ID, @Student_Status);
                            ";

                                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                                    {
                                        insertCommand.Parameters.AddWithValue("@Class_Code", classcode);
                                        insertCommand.Parameters.AddWithValue("@Attendance_date", attendanceDate);
                                        insertCommand.Parameters.AddWithValue("@Student_ID", studentID);
                                        insertCommand.Parameters.AddWithValue("@Student_Status", studentStatus);

                                        insertCommand.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                connection.Close();
            }

            MessageBox.Show("Data saved successfully.");
        }

        private void btnCancel1_Click(object sender, EventArgs e)
        {
            displayStudents();
            btnCancel1.Visible = false;
            btnSave1.Enabled = false;
        }

        private void btnCancel2_Click(object sender, EventArgs e)
        {
            displayStudentsAttendanceReport();
            btnCancel2.Visible = false;
            btnSave2.Enabled = false;
            btnNewAttendance.Enabled = true;
        }
    }
}
