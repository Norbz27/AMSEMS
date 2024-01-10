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
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using System.IO;
using System.Data.SqlClient;
using System.Net.NetworkInformation;
using System.Windows.Forms.DataVisualization.Charting;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.Common;
using Org.BouncyCastle.Ocsp;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Threading;
using AMSEMS.SubForm_Guidance;
using System.Net;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectOverview : Form
    {
        SQLite_Connection conn;

        static formSubjectInformation form;
        static string ccode;
        static string classcode;
        static string subacadlvl;
        formAddStudentToSubject form2;
        private int selectedColumnIndex;
        string rep;
        static string acadSchYeeardes, termdes;
        private List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
        private CancellationTokenSource cancellationTokenSource;
        private string acadSchYeear;
        private string acadShsSem;
        private string acadTerSem;

        public formSubjectOverview()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
            form2 = new formAddStudentToSubject(this);
            cancellationTokenSource = new CancellationTokenSource();
            dgvStudents.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAttendanceReport.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAbesnteismRep.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAttendance.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ptbLoading.Style = ProgressBarStyle.Marquee;
            dgvAttendanceReport.CellValueNeeded += dgvAttendanceReport_CellValueNeeded;
        }
        public static void setCode(string ccode1, string classcode1, string subacadlvl1, formSubjectInformation form1, string schyear1, string term1)
        {
            ccode = ccode1;
            classcode = classcode1;
            subacadlvl = subacadlvl1;
            form = form1;
            acadSchYeeardes = schyear1;
            termdes = term1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
            displayChart();
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
        private void tbSearchstudRep_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearchstudRep.Text.Trim();
            ApplySearchFilterRep(searchKeyword);
        }

        private void ApplySearchFilterRep(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvAttendanceReport.Rows)
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
                                // Iterate through the columns to find the "studid" column
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    if (cell.OwningColumn.Name.Equals("studid", StringComparison.OrdinalIgnoreCase))
                                    {
                                        string studentIDToDelete = cell.Value.ToString();
                                        command.Parameters.Clear();
                                        command.Parameters.AddWithValue("@StudID", studentIDToDelete);
                                        command.ExecuteNonQuery();
                                        break;  // Exit the inner loop once the column is found
                                    }
                                }
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
                string query = "SELECT s.Course_Code AS Ccode, Course_Description, sec.Description AS secdes,Image FROM tbl_subjects s RIGHT JOIN tbl_class_list cl ON s.Course_code = cl.Course_Code LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE s.Course_Code = @Ccode AND cl.Class_Code = @ClCode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@ClCode", classcode);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            string CourseCode = rd["Ccode"].ToString();
                            string CourseDes = rd["Course_Description"].ToString();
                            string secDes = rd["secdes"].ToString();
                            object image = rd["Image"];
                            System.Drawing.Image img = conn.ConvertToImage(image);

                            lblSec.Text = secDes;
                            lblSection.Text = secDes;
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
                string query = $"SELECT StudentID, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM {tblname} cl LEFT JOIN tbl_students_account s ON cl.StudentID = s.ID ORDER BY Name";

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
            dgvAttendanceReport.Columns.Add("present", "Classes Present");
            dgvAttendanceReport.Columns["present"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAttendanceReport.Columns["present"].ReadOnly = true;
            dgvAttendanceReport.Columns["present"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAttendanceReport.Columns.Add("absent", "Classes Absent");
            dgvAttendanceReport.Columns["absent"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAttendanceReport.Columns["absent"].ReadOnly = true;
            dgvAttendanceReport.Columns["absent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAttendanceReport.Columns.Add("total", "Total Classes");
            dgvAttendanceReport.Columns["total"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAttendanceReport.Columns["total"].ReadOnly = true;
            dgvAttendanceReport.Columns["total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();

                // Fetch distinct attendance dates
                string dateQuery = @"
            SELECT DISTINCT Attendance_date
            FROM tbl_subject_attendance
            WHERE Class_Code = @Class_Code
            ORDER BY Attendance_date";

                List<DateTime> sortedDates = new List<DateTime>();

                using (SQLiteCommand dateCommand = new SQLiteCommand(dateQuery, con))
                {
                    dateCommand.Parameters.AddWithValue("@Class_Code", classcode);

                    using (SQLiteDataReader dateReader = dateCommand.ExecuteReader())
                    {
                        while (dateReader.Read())
                        {
                            string attendanceDateString = dateReader["Attendance_date"].ToString();

                            if (DateTime.TryParse(attendanceDateString, out DateTime attendanceDate))
                            {
                                sortedDates.Add(attendanceDate);
                            }
                        }
                    }
                }

                // Sort the list of DateTime objects
                sortedDates.Sort();

                // Dynamically add columns to the DataGridView in sorted order
                foreach (DateTime sortedDate in sortedDates)
                {
                    string formattedDate = sortedDate.ToString("MMM dd, yy hh:mm tt");
                    dgvAttendanceReport.Columns.Add(formattedDate, formattedDate);
                }

                // Fetch all students
                string tblname = "tbl_" + classcode;
                string studentsQuery = $@"
            SELECT ID AS Student_ID,
                   UPPER(Lastname || ', ' || Firstname || ' ' || Middlename) AS Name
            FROM tbl_students_account s RIGHT JOIN {tblname} cl ON s.ID = cl.StudentID ORDER BY Name";

                Dictionary<string, int> studentRowIndexMap = new Dictionary<string, int>();

                using (SQLiteCommand studentsCommand = new SQLiteCommand(studentsQuery, con))
                {
                    using (SQLiteDataReader studentsReader = studentsCommand.ExecuteReader())
                    {
                        while (studentsReader.Read())
                        {
                            string studid = studentsReader["Student_ID"].ToString();
                            string studname = studentsReader["Name"].ToString();

                            // Add the student to the DataGridView
                            int rowIndex = dgvAttendanceReport.Rows.Add(false);
                            dgvAttendanceReport.Rows[rowIndex].Cells["attstudid"].Value = studid;
                            dgvAttendanceReport.Rows[rowIndex].Cells["attstudname"].Value = studname;

                            // Update the map with the new row index
                            studentRowIndexMap[studid] = rowIndex;
                        }
                    }
                }

                // Fetch student attendance details
                string attendanceQuery = @"SELECT sa.Student_ID,
                                            sa.Attendance_date,
                                            sa.Student_Status
                                            FROM tbl_subject_attendance sa
                                            WHERE sa.Class_Code = @Class_Code
                                            ORDER BY sa.Attendance_date, sa.Student_ID";

                using (SQLiteCommand attendanceCommand = new SQLiteCommand(attendanceQuery, con))
                {
                    attendanceCommand.Parameters.AddWithValue("@Class_Code", classcode);

                    using (SQLiteDataReader rd = attendanceCommand.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string studid = rd["Student_ID"].ToString();
                            string attendanceDate = rd["Attendance_date"].ToString();
                            string studentStatus = rd["Student_Status"].ToString();

                            int rowIndex = studentRowIndexMap[studid];

                            // Check if the column exists before trying to set its value
                            if (dgvAttendanceReport.Columns.Contains(attendanceDate))
                            {
                                // Find the dynamically added column index
                                int columnIndex = dgvAttendanceReport.Columns[attendanceDate].Index;

                                // Set the student status in the corresponding column
                                dgvAttendanceReport.Rows[rowIndex].Cells[columnIndex].Value = studentStatus;
                                dgvAttendanceReport.Rows[rowIndex].Cells[columnIndex].ReadOnly = true;

                                // Increment the corresponding counter based on student status
                                if (studentStatus == "P")
                                {
                                    int presentCount = Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["present"].Value ?? 0);
                                    dgvAttendanceReport.Rows[rowIndex].Cells["present"].Value = presentCount + 1;
                                }
                                else if (studentStatus == "A")
                                {
                                    int absentCount = Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["absent"].Value ?? 0);
                                    dgvAttendanceReport.Rows[rowIndex].Cells["absent"].Value = absentCount + 1;
                                }

                                // Increment the total classes counter
                                int totalClass = Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["total"].Value ?? 0);
                                dgvAttendanceReport.Rows[rowIndex].Cells["total"].Value = totalClass + 1;

                            }
                        }
                    }
                }
            }
        }
        private void dgvAttendanceReport_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // Check if the cell is in a numeric column and its value is null
            if (e.RowIndex >= 0 &&
                (e.ColumnIndex == dgvAttendanceReport.Columns["present"].Index ||
                 e.ColumnIndex == dgvAttendanceReport.Columns["absent"].Index ||
                 e.ColumnIndex == dgvAttendanceReport.Columns["total"].Index) &&
                dgvAttendanceReport.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
            {
                // Set the default value to 0
                e.Value = 0;
            }
        }
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Overview")
            {
                displayChart();
            }
            else if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Students")
            {
                displayStudents();
            }
            else if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Attendance Report")
            {
                displayStudentsAttendanceReport();
            }
            else if (tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "RFID Attendance Checker")
            {
                displayAttendanceChecker();
                if (tgbtnEnable_Att.Checked)
                {
                    tbRFID.Enabled = true;
                    tbSearchStudent.Enabled = false;
                    tbRFID.Focus();
                }
                else
                {
                    tbRFID.Enabled = false;
                    tbSearchStudent.Enabled = true;
                    tbSearchStudent.Focus();
                }
            }
            else if(tabControl1.SelectedTab != null && tabControl1.SelectedTab.Text == "Guidance Absenteeism Remarks")
            {
                academic();
                displayReport();
            }
        }

        private void btnNewAttendance_Click(object sender, EventArgs e)
        {
            // Disable the "New Attendance" button
            btnNewAttendance.Enabled = false;

            // Enable the "Save" button
            btnSave2.Enabled = true;

            // Make the "Cancel" button visible
            btnCancel2.Visible = true;

            // Get the selected date and time from the DateTimePicker
            DateTime dateTime = Dt.Value;

            // Format the date and time strings
            string formattedDate = dateTime.ToString("MMM dd, yy");
            string formattedTime = dateTime.ToString("hh:mm tt");

            // Create a new column name by combining formatted date and time
            string newColumnName = formattedDate + " " + formattedTime;

            // Add a new column to the DataGridView
            dgvAttendanceReport.Columns.Add(newColumnName, newColumnName);

            // Auto-size the newly added column to fit its content
            int columnIndex = dgvAttendanceReport.Columns.Count - 1;
            dgvAttendanceReport.AutoResizeColumn(columnIndex, DataGridViewAutoSizeColumnMode.AllCells);

            // Set the newly added column as the first displayed scrolling column
            dgvAttendanceReport.FirstDisplayedScrollingColumnIndex = columnIndex;

            // Subscribe to the CellDoubleClick event only once (to avoid multiple subscriptions)
            dgvAttendanceReport.CellDoubleClick += dgvAttendanceReport_CellDoubleClick;

            // Add the other event handlers as needed
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

            // Check if the length of the text is already 1
            if (((System.Windows.Forms.TextBox)sender).Text.Length >= 1)
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
            btnCancel2.Visible = false;
            btnSave2.Enabled = false;
            // Assuming your DataGridView is named dgvAttendanceReport
            if (dgvAttendanceReport.Columns.Count < 6)
            {
                MessageBox.Show("No attendance.");
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(conn.connectionString))
            {
                connection.Open();

                foreach (DataGridViewColumn column in dgvAttendanceReport.Columns)
                {
                    if (column.Index >= 5) // Start from the 3rd column
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
            displayStudentsAttendanceReport();
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
        public void displayAttendanceChecker()
        {
            for (int i = dgvAttendance.Columns.Count - 1; i >= 3; i--)
            {
                dgvAttendance.Columns.RemoveAt(i);
            }
            
            dgvAttendance.Rows.Clear();
            DateTime dateTime = Dt.Value;
            string formattedDate = dateTime.ToString("MMM dd, yy");
            string formattedTime = dateTime.ToString("hh:mm tt");
            string newColumnName = formattedDate + " " + formattedTime;

            // Add a new column to the DataGridView
            dgvAttendance.Columns.Add(newColumnName, newColumnName);

            // Auto-size the newly added column to fit its content
            int columnIndex = dgvAttendance.Columns.Count - 1;
            dgvAttendance.AutoResizeColumn(columnIndex, DataGridViewAutoSizeColumnMode.AllCells);

            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string tblname = "tbl_" + classcode;
                string query = $"SELECT RFID, StudentID, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM {tblname} cl LEFT JOIN tbl_students_account s ON cl.StudentID = s.ID ORDER BY Name";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string rfid = rd["RFID"].ToString();
                            string studid = rd["StudentID"].ToString();
                            string studname = rd["Name"].ToString();
                            int rowIndex = dgvAttendance.Rows.Add(false);
                            dgvAttendance.Rows[rowIndex].Cells["ID"].Value = studid;
                            dgvAttendance.Rows[rowIndex].Cells["Name_"].Value = studname;
                            dgvAttendance.Rows[rowIndex].Cells["rfid"].Value = rfid;
                        }
                    }
                }
            }
        }
        public async void checkRFID()
        {
            string rfid = tbRFID.Text;
            //MessageBox.Show(rfid);
            // Check if tbRFID is empty
            if (string.IsNullOrWhiteSpace(rfid))
            {
                return; // Exit the method if RFID is empty
            }
            displayStudInfo();
            // Assuming your DataGridView is named dgvAttendance
            foreach (DataGridViewRow row in dgvAttendance.Rows)
            {
                string studentID = row.Cells["ID"].Value.ToString();
                string studentName = row.Cells["Name_"].Value.ToString();
                string studentRFID = row.Cells["rfid"].Value.ToString();

                // Check if the RFID in the row matches the RFID you are looking for
                if (string.Equals(studentRFID, rfid, StringComparison.OrdinalIgnoreCase))
                {
                    // Find the index of the newly added column
                    int columnIndex = dgvAttendance.Columns.Count - 1;

                    // Mark the corresponding cell in the newly added column with a "P"
                    row.Cells[columnIndex].Value = "P";

                    // Optionally, break out of the loop if you only want to mark the first occurrence
                    break;
                }
            }

            await Task.Delay(1000);
            // Clear the tbRFID text only after processing the RFID check
            tbRFID.Text = String.Empty;
        }
        public void displayStudInfo()
        {
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();
                string tblname = "tbl_" + classcode;
                string query = $"SELECT RFID, StudentID, UPPER(s.Firstname || ' ' || s.Middlename || ' ' || s.Lastname) AS Name, Profile_pic FROM {tblname} cl LEFT JOIN tbl_students_account s ON cl.StudentID = s.ID WHERE RFID = @rfid";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@rfid", tbRFID.Text);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            object imageData = rd["Profile_pic"];
                            string name = rd["Name"].ToString();
                            string id = rd["StudentID"].ToString();

                            System.Drawing.Image image = conn.ConvertToImage(imageData);

                            ptbProfilePic.Image = image;
                            lblName.Text = name;
                            lblID.Text = id;

                        }
                    }
                }
            }
        }

        private void tbRFID_TextChanged(object sender, EventArgs e)
        {
            checkRFID();
        }

        private void dgvAttendanceReport_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgvAttendance_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            tbRFID.Focus();
        }

        private void tbSearchStudent_Leave(object sender, EventArgs e)
        {
            tbRFID.Focus();
        }

        private void dgvAttendance_Click(object sender, EventArgs e)
        {
            tbRFID.Focus();
        }

        private void tgbtnEnable_Att_CheckedChanged(object sender, EventArgs e)
        {
            if (tgbtnEnable_Att.Checked)
            {
                tbRFID.Enabled = true;
                tbSearchStudent.Enabled = false;
                tbRFID.Focus();
            }
            else
            {
                tbRFID.Enabled = false;
                tbSearchStudent.Enabled = true;
                tbSearchStudent.Focus();
            }
        }

        private void tbSearchStudent_Enter(object sender, EventArgs e)
        {
            tbSearchStudent.Text = String.Empty;
        }

        private void tbSearchStudent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string studID = tbSearchStudent.Text;

                // Check if tbRFID is empty
                if (string.IsNullOrWhiteSpace(studID))
                {
                    return; // Exit the method if RFID is empty
                }

                using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                {
                    cn.Open();
                    string tblname = "tbl_" + classcode;
                    string query = $"SELECT RFID, StudentID, UPPER(s.Firstname || ' ' || s.Middlename || ' ' || s.Lastname) AS Name, Profile_pic FROM {tblname} cl LEFT JOIN tbl_students_account s ON cl.StudentID = s.ID WHERE StudentID = @id";

                    using (SQLiteCommand command = new SQLiteCommand(query, cn))
                    {
                        command.Parameters.AddWithValue("@id", tbSearchStudent.Text);
                        using (SQLiteDataReader rd = command.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                object imageData = rd["Profile_pic"];
                                string name = rd["Name"].ToString();
                                string id = rd["StudentID"].ToString();

                                System.Drawing.Image image = conn.ConvertToImage(imageData);

                                ptbProfilePic.Image = image;
                                lblName.Text = name;
                                lblID.Text = id;

                            }
                        }
                    }
                }

                // Assuming your DataGridView is named dgvAttendance
                foreach (DataGridViewRow row in dgvAttendance.Rows)
                {
                    string studentID = row.Cells["ID"].Value.ToString();
                    string studentName = row.Cells["Name_"].Value.ToString();
                    string studentRFID = row.Cells["rfid"].Value.ToString();

                    // Check if the RFID in the row matches the RFID you are looking for
                    if (string.Equals(studentID, studID, StringComparison.OrdinalIgnoreCase))
                    {
                        // Find the index of the newly added column
                        int columnIndex = dgvAttendance.Columns.Count - 1;

                        // Mark the corresponding cell in the newly added column with a "P"
                        row.Cells[columnIndex].Value = "P";

                        // Clear the tbRFID text only after processing the RFID check
                        tbRFID.Text = String.Empty;

                        // Optionally, break out of the loop if you only want to mark the first occurrence
                        break;
                    }
                }
            }
        }

        private void btnRFIDSaveAtt_Click(object sender, EventArgs e)
        {
            if (dgvAttendance.Columns.Count < 4)
            {
                MessageBox.Show("No attendance.");
                return;
            }

            using (SQLiteConnection connection = new SQLiteConnection(conn.connectionString))
            {
                connection.Open();

                foreach (DataGridViewColumn column in dgvAttendance.Columns)
                {
                    if (column.Index >= 3) // Start from the 3rd column
                    {
                        string attendanceDate = column.HeaderText;

                        foreach (DataGridViewRow row in dgvAttendance.Rows)
                        {
                            string studentID = row.Cells["ID"].Value.ToString();

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
        private void dgvAttendanceReport_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Store the selected column index
                selectedColumnIndex = e.ColumnIndex;

                // Display the context menu at the current mouse position
                CMSOptions.Show(System.Windows.Forms.Cursor.Position);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            // If there is no selected column, cancel the opening of the context menu
            e.Cancel = selectedColumnIndex == -1;
        }

        private void delToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedColumnIndex != -1)
            {
                // Get the column header text of the selected column
                string columnHeader = dgvAttendanceReport.Columns[selectedColumnIndex].HeaderText;

                // Ask for confirmation before deleting the column
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the record on '{columnHeader}'?", "Delete Record",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (IsInternetConnected())
                    {
                        // Remove the selected column from the DataGridView
                        dgvAttendanceReport.Columns.RemoveAt(selectedColumnIndex);
                        try
                        {
                            using (SQLiteConnection connection = new SQLiteConnection(conn.connectionString))
                            {
                                connection.Open();

                                using (SQLiteCommand command = new SQLiteCommand(connection))
                                {
                                    string clearSql = @"DELETE FROM tbl_subject_attendance WHERE Attendance_date = @attdate;";
                                    command.CommandText = clearSql;
                                    command.Parameters.AddWithValue("@attdate", columnHeader);
                                    command.ExecuteNonQuery();
                                }
                                connection.Close();
                            }

                            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                            {
                                connection.Open();
                                string clearSql = @"DELETE FROM tbl_subject_attendance WHERE Attendance_date = @attdate;";
                                using (SqlCommand command = new SqlCommand(clearSql, connection))
                                {
                                    command.Parameters.AddWithValue("@attdate", columnHeader);
                                    command.ExecuteNonQuery();
                                }
                                connection.Close();
                            }

                            MessageBox.Show($"Record on '{columnHeader}' deleted.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Unstable Connection!! Can't connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Reset the selected column index after deletion or if the user canceled
                selectedColumnIndex = -1;
            }
        }

        private void btnExportStudentList_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDFStudentsList(dgvStudents, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }
        private void ExportToPDFStudentsList(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.LETTER);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            iTextSharp.text.Font headerFont2 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font headerFont3 = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("List of Students on " + lblSubjectName.Text, headerFont1);
            Paragraph titleParagraph2 = new Paragraph(lblSection.Text, headerFont2);
            Paragraph titleParagraph3 = new Paragraph(DateTime.Now.ToString("MMM dd, yyyy"), headerFont3);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            titleParagraph3.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);
            document.Add(titleParagraph3);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count - 1); // Subtract 1 to exclude the last column
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                if (column.Index != dataGridView.Columns.Count - 1) // Skip the last column
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                    cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                    pdfTable.AddCell(cell);
                }
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.ColumnIndex != dataGridView.Columns.Count - 1) // Skip the last column
                    {
                        PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString(), cellFont));
                        pdfTable.AddCell(pdfCell);
                    }
                }
            }

            document.Add(pdfTable);
            document.Close();
        }

        private void btnExportAttendance_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDFAttendanceReport(dgvAttendanceReport, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }
        private void ExportToPDFAttendanceReport(DataGridView dataGridView, string filePath)
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
            Paragraph titleParagraph2 = new Paragraph(lblSubjectName.Text , headerFont2);
            Paragraph titleParagraph4 = new Paragraph(lblSection.Text, headerFont2);
            Paragraph titleParagraph3 = new Paragraph(DateTime.Now.ToString("yyyy"), headerFont3);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            titleParagraph4.Alignment = Element.ALIGN_CENTER;
            titleParagraph3.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);
            document.Add(titleParagraph4);
            document.Add(titleParagraph3);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count); // Subtract 1 to exclude the last column
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
                    // Check if the cell value is not null before attempting to use it
                    string cellValue = cell.Value != null ? cell.Value.ToString() : "0";
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cellValue, cellFont));
                    pdfTable.AddCell(pdfCell);
                }
            }
            document.Add(pdfTable);
            document.Close();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            CMSSectionOption.Show(btnOption, new System.Drawing.Point(0, btnOption.Height));
        }

        private async void uploadtoolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to upload attendance record to cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptbLoading.Visible = true; // Show loading image before starting the synchronization
                await Task.Delay(3000);
                if (IsInternetConnected())
                {
                    try
                    {
                        using (SqlConnection cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            await cnn.OpenAsync();
                            System.Data.DataTable attendance_record = conn.GetAttendanceRecord(classcode);
                            System.Data.DataTable class_list = conn.GetClassList(classcode);

                            if (attendance_record.Rows.Count > 0 || class_list.Rows.Count >0)
                            {
                                foreach (DataRow row in attendance_record.Rows)
                                {
                                    string classcode = row["Class_Code"].ToString();
                                    string attendacedate = row["Attendance_date"].ToString();
                                    string studid = row["Student_ID"].ToString();
                                    string studstatus = row["Student_Status"].ToString();

                                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbl_subject_attendance WHERE Student_ID = @StudentID AND Attendance_date = @AttDate AND Class_Code = @ClassCode", cnn))
                                    {
                                        checkCmd.Parameters.AddWithValue("@StudentID", studid);
                                        checkCmd.Parameters.AddWithValue("@ClassCode", classcode);
                                        checkCmd.Parameters.AddWithValue("@AttDate", attendacedate);

                                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                                        if (recordCount == 0)
                                        {
                                            // If the record doesn't exist, insert it
                                            string insertQuery = "INSERT INTO tbl_subject_attendance (Class_Code, Attendance_date, Student_ID, Student_Status) VALUES (@ClassCode, @AttDate, @StudID, @StudStatus)";

                                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, cnn))
                                            {
                                                insertCmd.Parameters.AddWithValue("@ClassCode", classcode);
                                                insertCmd.Parameters.AddWithValue("@AttDate", attendacedate);
                                                insertCmd.Parameters.AddWithValue("@StudID", studid);
                                                insertCmd.Parameters.AddWithValue("@StudStatus", studstatus);

                                                insertCmd.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                                foreach (DataRow row in class_list.Rows)
                                {
                                    string classcode = row["Class_Code"].ToString();
                                    string secid = row["Section_ID"].ToString();
                                    string teachid = row["Teacher_ID"].ToString();
                                    string coursecode = row["Course_Code"].ToString();
                                    string schyear = row["School_Year"].ToString();
                                    string sem = row["Semester"].ToString();
                                    string acadlvl = row["Acad_Level"].ToString();

                                    using (SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM tbl_class_list WHERE Class_Code = @ClassCode AND Section_ID = @SecID AND Teacher_ID = @TeachID AND Course_Code = @Ccode AND School_Year = @SchYear AND Semester = @Sem AND Acad_Level = @AcadLvl", cnn))
                                    {
                                        checkCmd.Parameters.AddWithValue("@ClassCode", classcode);
                                        checkCmd.Parameters.AddWithValue("@SecID", secid);
                                        checkCmd.Parameters.AddWithValue("@TeachID", teachid);
                                        checkCmd.Parameters.AddWithValue("@Ccode", coursecode);
                                        checkCmd.Parameters.AddWithValue("@SchYear", schyear);
                                        checkCmd.Parameters.AddWithValue("@Sem", sem);
                                        checkCmd.Parameters.AddWithValue("@AcadLvl", acadlvl);

                                        int recordCount = Convert.ToInt32(checkCmd.ExecuteScalar());

                                        if (recordCount == 0)
                                        {
                                            // If the record doesn't exist, insert it
                                            string insertQuery = "INSERT INTO tbl_class_list (CLass_Code,Section_ID,Teacher_ID,Course_Code,School_Year,Semester,Acad_Level) VALUES (@ClassCode, @SecID, @TeachID, @Ccode, @SchYear, @Sem, @AcadLvl)";

                                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, cnn))
                                            {
                                                insertCmd.Parameters.AddWithValue("@ClassCode", classcode);
                                                insertCmd.Parameters.AddWithValue("@SecID", secid);
                                                insertCmd.Parameters.AddWithValue("@TeachID", teachid);
                                                insertCmd.Parameters.AddWithValue("@Ccode", coursecode);
                                                insertCmd.Parameters.AddWithValue("@SchYear", schyear);
                                                insertCmd.Parameters.AddWithValue("@Sem", sem);
                                                insertCmd.Parameters.AddWithValue("@AcadLvl", acadlvl);

                                                insertCmd.ExecuteNonQuery();
                                            }
                                        }
                                    }

                                    string tableName = "tbl_" + classcode;
                                    System.Data.DataTable class_stud_list = conn.GetStudListSingle(tableName, classcode);
                                    if (class_stud_list != null)
                                    {
                                        string createTableSql = $@"IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}')
                                    BEGIN
                                        CREATE TABLE {tableName} (
                                            StudentID INT PRIMARY KEY,
                                            Class_Code NVARCHAR(MAX)
                                        );
                                    END;";
                                        using (SqlCommand command = new SqlCommand(createTableSql, cnn))
                                        {
                                            command.ExecuteNonQuery();
                                        }

                                        string insertSql = $@"
                                    INSERT INTO {tableName} (StudentID, Class_Code)
                                    SELECT @StudID, @ClassCode
                                    WHERE NOT EXISTS (SELECT 1 FROM {tableName} WHERE StudentID = @StudID);";

                                        using (SqlCommand insertCommand = new SqlCommand(insertSql, cnn))
                                        {
                                            insertCommand.CommandText = insertSql;

                                            foreach (DataRow row1 in class_stud_list.Rows)
                                            {
                                                string studentID = row1["StudentID"].ToString();
                                                string classCodeValue = row1["Class_Code"].ToString(); ;

                                                insertCommand.Parameters.Clear();
                                                insertCommand.Parameters.AddWithValue("@StudID", studentID);
                                                insertCommand.Parameters.AddWithValue("@ClassCode", classCodeValue);
                                                insertCommand.ExecuteNonQuery();
                                            }
                                        }
                                    }
                                }
                                MessageBox.Show("Successfully Upload Data.", "Sync Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Unstable Connection!! Can't connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ptbLoading.Visible = false;
            }
        }
        private bool IsInternetConnected()
        {
            try
            {
                using (var mobileClient = new WebClient())
                using (var webConnection = mobileClient.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public void displayChart()
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            

            int totalStudents = 0;
            int totalClasses = 0;
            int totalAttendees = 0;

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
                        // Add a single series named "StudentRecord"
                        System.Windows.Forms.DataVisualization.Charting.Series series = chart1.Series.Add("StudentRecord");
                        series.ChartType = SeriesChartType.Column;

                        while (dateReader.Read())
                        {
                            string attendanceDate = dateReader["Attendance_date"].ToString();

                            // Add the date as a data point to the series
                            series.Points.AddXY(attendanceDate, 0);
                            totalClasses++;
                        }
                    }
                }
                string totalStud = $@"
                SELECT COUNT(*) AS totalStudent
                FROM tbl_{classcode}";

                using (SQLiteCommand cm = new SQLiteCommand(totalStud, con))
                {
                    using (SQLiteDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            totalStudents = Convert.ToInt32(dr["totalStudent"]);
                        }
                    }
                }


                // Fetch student attendance details
                string attendanceQuery = @"SELECT sa.Attendance_date,
                                  COUNT(sa.Student_ID) AS AttendeeCount
                           FROM tbl_subject_attendance sa
                           WHERE Student_Status = 'P' AND sa.Class_Code = @Class_Code
                           GROUP BY sa.Attendance_date
                           ORDER BY sa.Attendance_date";

                using (SQLiteCommand attendanceCommand = new SQLiteCommand(attendanceQuery, con))
                {
                    attendanceCommand.Parameters.AddWithValue("@Class_Code", classcode);

                    using (SQLiteDataReader rd = attendanceCommand.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string attendanceDate = rd["Attendance_date"].ToString();
                            int attendeeCount = Convert.ToInt32(rd["AttendeeCount"]);

                            // Find the single series in the existing Chart
                            System.Windows.Forms.DataVisualization.Charting.Series series = chart1.Series["StudentRecord"];

                            // Update the data point for each date with the total attendee count
                            foreach (DataPoint dataPoint in series.Points)
                            {
                                if (dataPoint.AxisLabel == attendanceDate)
                                {
                                    dataPoint.SetValueY(attendeeCount);
                                    totalAttendees += attendeeCount;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;
            chart1.Series["StudentRecord"].IsValueShownAsLabel = true;
            //// Calculate the average attendees
            //double averageAttendees = totalAttendees / (double)totalClasses;

            // Display the results in labels
            lblTotalStudents.Text = totalStudents.ToString();
            lblTotalClasses.Text = totalClasses.ToString();

            //// Calculate the average attendees as a percentage
            //double averageAttendeesPercentage = (averageAttendees / totalStudents) * 100;

            //// Display the result with two decimal places and a percentage symbol
            //lblAverageAttendees.Text = averageAttendeesPercentage.ToString("F2") + "%";

        }
        private void deltoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to delete section? You need Internet to delete section", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                if (IsInternetConnected())
                {
                    using (SQLiteConnection connection = new SQLiteConnection(conn.connectionString))
                    {
                        connection.Open();

                        using (SQLiteTransaction transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                // SQLite deletion and table drop
                                string clearSql = @"DELETE FROM tbl_class_list WHERE Class_Code = @classcode;";
                                using (SQLiteCommand command = new SQLiteCommand(clearSql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@classcode", classcode);
                                    command.ExecuteNonQuery();
                                }

                                // Delete attendance records
                                clearSql = $@"DELETE FROM tbl_subject_attendance WHERE Student_ID IN (SELECT StudentID FROM tbl_{classcode}) AND Class_Code = @classcode;";
                                using (SQLiteCommand command = new SQLiteCommand(clearSql, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@classcode", classcode);
                                    command.ExecuteNonQuery();
                                }

                                clearSql = $@"DROP TABLE IF EXISTS tbl_{classcode};";
                                using (SQLiteCommand command = new SQLiteCommand(clearSql, connection, transaction))
                                {
                                    command.ExecuteNonQuery();
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                            }
                        }
                    }

                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();

                        using (SqlTransaction transaction = cn.BeginTransaction())
                        {
                            try
                            {
                                // SQL Server deletion and table drop
                                string clearSql = @"DELETE FROM tbl_class_list WHERE Class_Code = @classcode;";
                                using (SqlCommand cm = new SqlCommand(clearSql, cn, transaction))
                                {
                                    cm.Parameters.AddWithValue("@classcode", classcode);
                                    cm.ExecuteNonQuery();
                                }

                                // Delete attendance records
                                clearSql = $@"DELETE FROM tbl_subject_attendance WHERE Student_ID IN (SELECT StudentID FROM tbl_{classcode}) AND Class_Code = @classcode;";
                                using (SqlCommand cm = new SqlCommand(clearSql, cn, transaction))
                                {
                                    cm.Parameters.AddWithValue("@classcode", classcode);
                                    cm.ExecuteNonQuery();
                                }

                                // Delete consultation records
                                // Delete consultation records with a join on tbl_class_list
                                clearSql = @"DELETE cr
                                FROM tbl_consultation_record cr
                                INNER JOIN tbl_class_list cl ON cr.Class_Code = cl.Class_Code
                                WHERE cl.Class_Code = @classcode;";
                                using (SqlCommand cm = new SqlCommand(clearSql, cn, transaction))
                                {
                                    cm.Parameters.AddWithValue("@classcode", classcode);
                                    cm.ExecuteNonQuery();
                                }

                                clearSql = $@"IF OBJECT_ID('tbl_{classcode}', 'U') IS NOT NULL DROP TABLE tbl_{classcode};";
                                using (SqlCommand cm = new SqlCommand(clearSql, cn, transaction))
                                {
                                    cm.ExecuteNonQuery();
                                }

                                transaction.Commit();

                                MessageBox.Show("Deletion successful.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                MessageBox.Show($"Deletion failed. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    this.Close();
                    form.OpenMainPage();
                    form.displaySectionOfSubject();
                }
                else
                {
                    MessageBox.Show("No internet connection available. Please check your network connection.");
                }
            }
        }
        private void btnExcel_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "Excel Files|*.xlsx";
                    saveFileDialog.Title = "Save As Excel File";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;

                        // Create a new Excel application
                        Excel.Application excelApp = new Excel.Application();
                        excelApp.Visible = false; // You can set this to true for debugging purposes

                        // Create a new Excel workbook and worksheet
                        Excel.Workbook workbook = excelApp.Workbooks.Add();
                        Excel.Worksheet worksheet = workbook.Worksheets[1];

                        // Set up the header rows formatting
                        Excel.Range headerRange1 = worksheet.Rows[1];
                        headerRange1.Font.Bold = true;

                        Excel.Range headerRange2 = worksheet.Rows[2];
                        headerRange2.Font.Bold = true;

                        Excel.Range headerRange3 = worksheet.Rows[3];
                        headerRange3.Font.Bold = true;

                        Excel.Range headerRange4 = worksheet.Rows[4];
                        headerRange4.Font.Bold = true;

                        // Add three-column headers
                        worksheet.Cells[4, 1] = "ID";
                        worksheet.Cells[4, 2] = "Name";

                        // Apply the extracted color to the header cells
                        Excel.Range headerCell1 = worksheet.Cells[4, 1];
                        headerCell1.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);
                        headerCell1.Font.Bold = true;

                        Excel.Range headerCell2 = worksheet.Cells[4, 2];
                        headerCell2.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
                        headerCell2.Font.Bold = true;

                        int excelColumnIndex = 3;
                        using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
                        {
                            con.Open();

                            // Fetch distinct attendance dates
                            string dateQuery = @"
                            SELECT DISTINCT Attendance_date
                            FROM tbl_subject_attendance
                            WHERE Class_Code = @Class_Code
                            ORDER BY Attendance_date";

                            List<DateTime> sortedDates = new List<DateTime>();

                            using (SQLiteCommand dateCommand = new SQLiteCommand(dateQuery, con))
                            {
                                dateCommand.Parameters.AddWithValue("@Class_Code", classcode);

                                using (SQLiteDataReader dateReader = dateCommand.ExecuteReader())
                                {
                                    while (dateReader.Read())
                                    {
                                        string attendanceDateString = dateReader["Attendance_date"].ToString();

                                        if (DateTime.TryParse(attendanceDateString, out DateTime attendanceDate))
                                        {
                                            sortedDates.Add(attendanceDate);
                                        }
                                    }
                                }
                            }
                            sortedDates.Sort();

                            string currentMonth = null;
                            int monthColumnStartIndex = 0;
                            int yearColumnIndex = 3; // New index for the year column

                            // Add the year to the first row
                            worksheet.Cells[1, yearColumnIndex] = sortedDates.Count > 0 ? sortedDates[0].ToString("yyyy") : string.Empty;

                            foreach (DateTime sortedDate in sortedDates)
                            {
                                string formattedDateMonth = sortedDate.ToString("MMM");

                                // Check if the month has changed
                                if (currentMonth != formattedDateMonth)
                                {
                                    // If a month change is detected, merge the cells from monthColumnStartIndex to excelColumnIndex - 1
                                    if (currentMonth != null)
                                    {
                                        Excel.Range mergedRange = worksheet.Range[worksheet.Cells[2, monthColumnStartIndex], worksheet.Cells[2, excelColumnIndex - 1]];
                                        mergedRange.Merge();
                                        mergedRange.Value = currentMonth; // Set the month name for the merged cells
                                        mergedRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                    }

                                    currentMonth = formattedDateMonth;
                                    monthColumnStartIndex = excelColumnIndex;
                                }

                                string formattedDateDay = sortedDate.ToString("dd");
                                string formattedDateTime = sortedDate.ToString("hh:ss tt");
                                worksheet.Cells[3, excelColumnIndex] = formattedDateDay;
                                worksheet.Cells[4, excelColumnIndex] = formattedDateTime;
                                worksheet.Cells[3, excelColumnIndex].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                worksheet.Cells[4, excelColumnIndex].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                excelColumnIndex++;
                            }

                            if (currentMonth != null)
                            {
                                Excel.Range mergedRange = worksheet.Range[worksheet.Cells[2, monthColumnStartIndex], worksheet.Cells[2, excelColumnIndex - 1]];
                                mergedRange.Merge();
                                mergedRange.Value = currentMonth;
                                mergedRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                            }

                            // Merge the year cell
                            worksheet.Range[worksheet.Cells[1, yearColumnIndex], worksheet.Cells[1, excelColumnIndex - 1]].Merge();
                            worksheet.Range[worksheet.Cells[1, yearColumnIndex], worksheet.Cells[1, excelColumnIndex - 1]].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                            // Add ID and Name to Excel instead of DataGridView
                            int currentRowIndex = 5; // Starting row index for ID and Name
                            string tblname = "tbl_" + classcode;
                            string studentsQuery = $@"
                            SELECT ID AS Student_ID,
                                   UPPER(Lastname || ', ' || Firstname || ' ' || Middlename) AS Name
                            FROM tbl_students_account s RIGHT JOIN {tblname} cl ON s.ID = cl.StudentID ORDER BY Name";
                            Dictionary<string, int> studentRowIndexMap = new Dictionary<string, int>();
                            using (SQLiteCommand studentsCommand = new SQLiteCommand(studentsQuery, con))
                            {
                                using (SQLiteDataReader studentsReader = studentsCommand.ExecuteReader())
                                {
                                    while (studentsReader.Read())
                                    {
                                        string studid = studentsReader["Student_ID"].ToString();
                                        string studname = studentsReader["Name"].ToString();

                                        // Add the student to Excel
                                        worksheet.Cells[currentRowIndex, 1] = studid;
                                        worksheet.Cells[currentRowIndex, 2] = studname;

                                        // Apply formatting to ID and Name columns
                                        Excel.Range idCell = worksheet.Cells[currentRowIndex, 1];
                                        idCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightBlue);

                                        Excel.Range nameCell = worksheet.Cells[currentRowIndex, 2];
                                        nameCell.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);

                                        studentRowIndexMap[studid] = currentRowIndex;
                                        currentRowIndex++;

                                    }
                                }
                            }

                            // Fetch student attendance details
                            string attendanceQuery = @"SELECT sa.Student_ID,
                                sa.Attendance_date,
                                sa.Student_Status
                                FROM tbl_subject_attendance sa
                                WHERE sa.Class_Code = @Class_Code
                                ORDER BY sa.Attendance_date, sa.Student_ID";

                            using (SQLiteCommand attendanceCommand = new SQLiteCommand(attendanceQuery, con))
                            {
                                attendanceCommand.Parameters.AddWithValue("@Class_Code", classcode);

                                using (SQLiteDataReader rd = attendanceCommand.ExecuteReader())
                                {
                                    while (rd.Read())
                                    {
                                        string studid = rd["Student_ID"].ToString();
                                        string attendanceDate = rd["Attendance_date"].ToString();
                                        string studentStatus = rd["Student_Status"].ToString();

                                        int rowIndex = studentRowIndexMap[studid];

                                        // Find the dynamically added column index
                                        int columnIndex = GetColumnIndex(attendanceDate, sortedDates);

                                        if (studentStatus.Equals("P"))
                                        {
                                            worksheet.Cells[rowIndex, columnIndex].Value = 1.5;
                                        }
                                        else
                                        {
                                            worksheet.Cells[rowIndex, columnIndex].Value = "";
                                        }

                                        // Set the student status in the corresponding column of the Excel sheet
                                       
                                        worksheet.Cells[rowIndex, columnIndex].HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                                    }
                                }
                            }
                        }

                        int lastRowIndex = worksheet.Cells[worksheet.Rows.Count, 1].End[Excel.XlDirection.xlUp].Row;
                        Excel.Range exportRange = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[lastRowIndex, excelColumnIndex - 1]];

                        exportRange.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                        exportRange.Borders.Weight = Excel.XlBorderWeight.xlThin;

                        // AutoResize the "Name" column (2nd column)
                        worksheet.Columns[2].AutoFit();

                        // Save the Excel file
                        workbook.SaveAs(filePath);

                        // Close Excel and release resources
                        workbook.Close();
                        excelApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(exportRange);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);

                        MessageBox.Show("Data exported to Excel successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public int GetColumnIndex(string attendanceDate, List<DateTime> sortedDates)
        {
            DateTime date = DateTime.Parse(attendanceDate);

            for (int i = 0; i < sortedDates.Count; i++)
            {
                if (date == sortedDates[i])
                {
                    return i + 3; // 3 is the offset for the ID and Name columns
                }
            }

            return -1; // Column not found
        }

        private void formSubjectOverview_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource?.Cancel();
        }

        private void dgvAbesnteismRep_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvAbesnteismRep.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvAbesnteismRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSDgvRemarkOption.Show(dgvAbesnteismRep, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
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
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];
                        formRemarks form = new formRemarks();
                        form.getForm(this, dgvAbesnteismRep.Rows[rowIndex].Cells[0].Value.ToString(), dgvAbesnteismRep.Rows[rowIndex].Cells[1].Value.ToString(), classcode, acadSchYeeardes, termdes, subacadlvl);

                        form.ShowDialog();
                        UseWaitCursor = false;
                    }
                }
            }
        }

        public void academic()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";
                query = "SELECT Acad_ID, (Academic_Year_Start +'-'+ Academic_Year_End) AS schyear FROM tbl_acad WHERE Status = 1";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    using (SqlDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            acadSchYeear = rd["schyear"].ToString();
                        }
                    }
                }

                query = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadShsSem = dr["Description"].ToString();
                        }
                    }
                }

                query = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadTerSem = dr["Description"].ToString();
                        }
                    }
                }
            }
        }

        private void tbSearch3_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch3.Text.Trim();
            ApplySearchFilterabs(searchKeyword);
        }

        private void ApplySearchFilterabs(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvAbesnteismRep.Rows)
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

        public async void displayReport()
        {
            try
            {
                if (IsInternetConnected())
                {
                    ptbLoadingG.Visible = true;
                    await Task.Delay(1000);
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string query = "";
                        if (subacadlvl.Equals("10001"))
                        {
                            if (acadSchYeear.Equals(acadSchYeeardes) && acadTerSem.Equals(termdes))
                            {
                                query = @"SELECT 
                                s.ID AS ID,
                                cr.Consultation_ID AS conid,
                                UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                                cr.Absences AS ConsecutiveAbsentDays
                            FROM 
                                tbl_consultation_record cr
                                LEFT JOIN tbl_student_accounts s ON cr.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                            LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                        	    LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                                LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
							    LEFT JOIN tbl_subject_attendance sa ON cr.Class_Code = sa.Class_Code
								LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID
								LEFT JOIN tbl_Semester sm ON cl.Semester = sm.Semester_ID
                            WHERE 
                                al.Academic_Level_ID = @acadlvl
                                AND cr.Class_Code = @clcode
                                AND s.Status = 1
                                AND (ad.Academic_Year_Start +'-'+ ad.Academic_Year_End) = @schyear
                                AND sm.Description = @sem
                                AND cr.Status = 'Done'
                            GROUP BY 
                                s.ID, s.Lastname, s.Firstname, s.Middlename, cr.Absences, cr.Consultation_ID
						    ORDER BY
							    Name";
                            }
                            else
                            {
                                query = @"SELECT 
                                s.ID AS ID,
                                cr.Consultation_ID AS conid,
                                UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                                cr.Absences AS ConsecutiveAbsentDays
                            FROM 
                                tbl_archived_consultation_record cr
                                LEFT JOIN tbl_student_accounts s ON cr.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                            LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                        	    LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                                LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
							    LEFT JOIN tbl_subject_attendance sa ON cr.Class_Code = sa.Class_Code
								LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID
								LEFT JOIN tbl_Semester sm ON cl.Semester = sm.Semester_ID
                            WHERE 
                                al.Academic_Level_ID = @acadlvl
                                AND cr.Class_Code = @clcode
                                AND s.Status = 1
                                AND (ad.Academic_Year_Start +'-'+ ad.Academic_Year_End) = @schyear
                                AND sm.Description = @sem
                                AND cr.Status = 'Done'
                            GROUP BY 
                                s.ID, s.Lastname, s.Firstname, s.Middlename, cr.Absences, cr.Consultation_ID
						    ORDER BY
							    Name";
                            }
                        }
                        else
                        {
                            if (acadSchYeear.Equals(acadSchYeeardes) && acadShsSem.Equals(termdes))
                            {
                                query = @"SELECT 
                                s.ID AS ID,
                                cr.Consultation_ID AS conid,
                                UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                                cr.Absences AS ConsecutiveAbsentDays
                            FROM 
                                tbl_consultation_record cr
                                LEFT JOIN tbl_student_accounts s ON cr.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                            LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                        	    LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                                LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
							    LEFT JOIN tbl_subject_attendance sa ON cr.Class_Code = sa.Class_Code
								LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID
								LEFT JOIN tbl_Quarter qr ON cl.Semester = qr.Quarter_ID
                            WHERE 
                                al.Academic_Level_ID = @acadlvl
                                AND cr.Class_Code = @clcode
                                AND s.Status = 1
                                AND (ad.Academic_Year_Start +'-'+ ad.Academic_Year_End) = @schyear
                                AND qr.Description = @sem
                                AND cr.Status = 'Done'
                            GROUP BY 
                                s.ID, s.Lastname, s.Firstname, s.Middlename, cr.Absences, cr.Consultation_ID
						    ORDER BY
							    Name";
                            }
                            else
                            {
                                query = @"SELECT 
                                s.ID AS ID,
                                cr.Consultation_ID AS conid,
                                UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                                cr.Absences AS ConsecutiveAbsentDays
                            FROM 
                                tbl_archived_consultation_record cr
                                LEFT JOIN tbl_student_accounts s ON cr.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                            LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                        	    LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                                LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
							    LEFT JOIN tbl_subject_attendance sa ON cr.Class_Code = sa.Class_Code
								LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID
								LEFT JOIN tbl_Quarter qr ON cl.Semester = qr.Quarter_ID
                            WHERE 
                                al.Academic_Level_ID = @acadlvl
                                AND cr.Class_Code = @clcode
                                AND s.Status = 1
                                AND (ad.Academic_Year_Start +'-'+ ad.Academic_Year_End) = @schyear
                                AND qr.Description = @sem
                                AND cr.Status = 'Done'
                            GROUP BY 
                                s.ID, s.Lastname, s.Firstname, s.Middlename, cr.Absences, cr.Consultation_ID
						    ORDER BY
							    Name";
                            }
                        }

                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            cmd.Parameters.AddWithValue("@acadlvl", subacadlvl);
                            cmd.Parameters.AddWithValue("@schyear", acadSchYeeardes);
                            cmd.Parameters.AddWithValue("@clcode", classcode);
                            cmd.Parameters.AddWithValue("@sem", termdes);

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                dgvAbesnteismRep.Rows.Clear();
                                int count = 1;
                                while (dr.Read())
                                {
                                    if (cancellationTokenSource.Token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    // Add a row and set the checkbox column value to false (unchecked)
                                    int rowIndex = dgvAbesnteismRep.Rows.Add(false);

                                    //Populate other columns, starting from index 1
                                    dgvAbesnteismRep.Rows[rowIndex].Cells["StudentID"].Value = dr["ID"] != DBNull.Value ? dr["ID"].ToString() : "";
                                    dgvAbesnteismRep.Rows[rowIndex].Cells["conid"].Value = dr["conid"] != DBNull.Value ? dr["conid"].ToString() : "";
                                    dgvAbesnteismRep.Rows[rowIndex].Cells["StudentName"].Value = dr["Name"] != DBNull.Value ? dr["Name"].ToString() : "";
                                    dgvAbesnteismRep.Rows[rowIndex].Cells["StudentAbsences"].Value = dr["ConsecutiveAbsentDays"] != DBNull.Value ? dr["ConsecutiveAbsentDays"].ToString() : "";

                                }
                            }
                        }
                    }
                    ptbLoadingG.Visible = false;
                }
                else
                {
                    MessageBox.Show("No Internet Connection!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
