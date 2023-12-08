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

        private List<DataGridViewRow> rowsToDelete = new List<DataGridViewRow>();
        public formSubjectOverview()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
            form2 = new formAddStudentToSubject(this);
            dgvStudents.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAttendanceReport.DefaultCellStyle.Font = new System.Drawing.Font("Poppins", 9F);
            dgvAttendance.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ptbLoading.Style = ProgressBarStyle.Marquee;
        }
        public static void setCode(string ccode1, string classcode1, string subacadlvl1, formSubjectInformation form1)
        {
            ccode = ccode1;
            classcode = classcode1;
            subacadlvl = subacadlvl1;
            form = form1;
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
            dgvAttendanceReport.Columns["present"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAttendanceReport.Columns.Add("absent", "Classes Absent");
            dgvAttendanceReport.Columns["absent"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAttendanceReport.Columns["absent"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvAttendanceReport.Columns.Add("total", "Total Classes");
            dgvAttendanceReport.Columns["total"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvAttendanceReport.Columns["total"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

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
                                    dgvAttendanceReport.Rows[rowIndex].Cells["present"].Value =
                                        Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["present"].Value) + 1;
                                }
                                else if (studentStatus == "A")
                                {
                                    dgvAttendanceReport.Rows[rowIndex].Cells["absent"].Value =
                                        Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["absent"].Value) + 1;
                                }

                                // Increment the total classes counter
                                dgvAttendanceReport.Rows[rowIndex].Cells["total"].Value =
                                    Convert.ToInt32(dgvAttendanceReport.Rows[rowIndex].Cells["total"].Value) + 1;
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
            btnSave2.Enabled = true;
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
                DialogResult result = MessageBox.Show($"Are you sure you want to delete the record on '{columnHeader}'?", "Delete Column",
                                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Remove the selected column from the DataGridView
                    dgvAttendanceReport.Columns.RemoveAt(selectedColumnIndex);

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

                    MessageBox.Show($"Record on '{columnHeader}' deleted.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString(), cellFont));
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

                                    // Insert data into the table only if the StudentID doesn't exist
                                    System.Data.DataTable class_stud_list = conn.GetStudListSingle(tableName, classcode);
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
                    MessageBox.Show("No internet connection available. Please check your network connection.");
                }
                ptbLoading.Visible = false;
            }
        }
        private bool IsInternetConnected()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("www.google.com"); // You can use a reliable external host for testing connectivity.

                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true; // Internet is reachable.
                }
                return false; // No internet connection.
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

                            // Dynamically add series to the existing Chart for each attendance date
                            System.Windows.Forms.DataVisualization.Charting.Series series = new System.Windows.Forms.DataVisualization.Charting.Series(attendanceDate);
                            series.ChartType = SeriesChartType.Column;
                            chart1.Series.Add(series);
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

                            // Find the corresponding series in the existing Chart
                            System.Windows.Forms.DataVisualization.Charting.Series series = chart1.Series[attendanceDate];

                            // Add a data point to the series for each student
                            series.Points.AddXY(studid, (studentStatus == "P") ? 1 : 0);

                            // Set the data point label
                            DataPoint dataPoint = series.Points.Last();
                            dataPoint.Label = studentStatus;

                            // Increment the total classes counter for each student
                            DataPoint totalDataPoint = null;
                            foreach (DataPoint point in chart1.Series["Total Classes"].Points)
                            {
                                if (point.AxisLabel == studid)
                                {
                                    totalDataPoint = point;
                                    break;
                                }
                            }
                            if (totalDataPoint == null)
                            {
                                totalDataPoint = new DataPoint();
                                totalDataPoint.AxisLabel = studid;
                                totalDataPoint.SetValueY(1);
                                chart1.Series["Total Classes"].Points.Add(totalDataPoint);
                            }
                            else
                            {
                                totalDataPoint.SetValueY(totalDataPoint.YValues[0] + 1);
                            }
                        }
                    }
                }
            }
        }

        private void deltoolStripMenuItem1_Click(object sender, EventArgs e)
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

                            clearSql = $@"IF OBJECT_ID('tbl_{classcode}', 'U') IS NOT NULL DROP TABLE tbl_{classcode};";
                            using (SqlCommand cm = new SqlCommand(clearSql, cn, transaction))
                            {
                                cm.ExecuteNonQuery();
                            }

                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                        }
                    }
                }
                this.Close();
                form.OpenMainPage();
                form.displaySectionOfSubject();
            }
        }
    }
}
