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
                    dgvStudents.Rows.RemoveAt(rowIndex);
                    btnSave1.Visible = true;
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
                            int rowIndex = dgvAttendanceReport.Rows.Add(false);
                            dgvAttendanceReport.Rows[rowIndex].Cells["attstudid"].Value = studid;
                            dgvAttendanceReport.Rows[rowIndex].Cells["attstudname"].Value = studname;
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
            DateTime dateTime = Dt.Value;
            string formattedDate = dateTime.ToString("MMM dd, yy");
            string formattedTime = dateTime.ToString("hh:mm tt");
            string newColumnName = formattedDate + " " + formattedTime;

            // Add a new column to the DataGridView
            dgvAttendanceReport.Columns.Add(newColumnName, newColumnName);

            // Auto-size the newly added column to fit its content
            int columnIndex = dgvAttendanceReport.Columns.Count - 1;
            //dgvAttendanceReport.AutoResizeColumn(columnIndex, DataGridViewAutoSizeColumnMode.AllCells);

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
            if (dgvStudents.Rows.Count > 0)
            {
                using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                {
                    cn.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cn))
                    {
                        string tableName = "tbl_" + classcode;
                        string insertSql = $"INSERT INTO {tableName} (StudentID, Class_Code) VALUES (@StudID, @ClassCode)";

                        foreach (DataGridViewRow row in dgvStudents.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string studentID = row.Cells["id"].Value.ToString();
                                string classCodeValue = classcode;

                                // Check if the student is not already in the table
                                if (!IsStudentInTable(cn, tableName, studentID))
                                {
                                    command.Parameters.Clear();
                                    command.CommandText = insertSql;
                                    command.Parameters.AddWithValue("@StudID", studentID);
                                    command.Parameters.AddWithValue("@ClassCode", classCodeValue);
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    cn.Close();
                    btnSave1.Visible = false;
                    this.Close();
                }
            }
        }

        private bool IsStudentInTable(SQLiteConnection connection, string tableName, string studentID)
        {
            using (SQLiteCommand command = new SQLiteCommand(connection))
            {
                command.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE StudentID = @StudID";
                command.Parameters.AddWithValue("@StudID", studentID);

                int count = Convert.ToInt32(command.ExecuteScalar());
                return count > 0;
            }
        }

    }
}
