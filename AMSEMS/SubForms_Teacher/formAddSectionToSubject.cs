using ComponentFactory.Krypton.Toolkit;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using Microsoft.Office.Interop.Excel;
using ListBox = System.Windows.Forms.ListBox;
using static System.Windows.Forms.AxHost;
using System.Security.Cryptography;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formAddSectionToSubject : KryptonForm
    {
        private List<string> suggestions = new List<string> { };
        private List<string> suggestionsstudents = new List<string> { };
        private ListBox listBoxSuggestions;
        private ListBox listBoxSuggestionsStudents;
        SQLite_Connection conn;
        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();
        string ccode;
        string subacadlvl;
        formSubjectInformation form;
        public formAddSectionToSubject(formSubjectInformation formSubjectInformation)
        {
            InitializeComponent();
            InitializeListBox();
            conn = new SQLite_Connection();
            form = formSubjectInformation;
        }
        public void setSubject(string ccode, string subacadlvl)
        {
            this.ccode = ccode;
            this.subacadlvl = subacadlvl;
        }
        private void InitializeListBox()
        {
            listBoxSuggestions = new ListBox
            {
                Visible = false,
                Width = tbSearch.Width,
                Height = 100,
                Location = new System.Drawing.Point(tbSearch.Left, tbSearch.Bottom + 5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            };

            listBoxSuggestions.DoubleClick += listBoxSuggestions_DoubleClick;
            listBoxSuggestions.KeyDown += listBoxSuggestions_KeyDown;

            panel1.Controls.Add(listBoxSuggestions);

            listBoxSuggestions.BringToFront();

            listBoxSuggestionsStudents = new ListBox
            {
                Visible = false,
                Width = tbSearchStudent.Width,
                Height = 100,
                Location = new System.Drawing.Point(tbSearchStudent.Left, tbSearchStudent.Bottom + 5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            };

            listBoxSuggestionsStudents.DoubleClick += listBoxSuggestionsStudents_DoubleClick;
            listBoxSuggestionsStudents.KeyDown += listBoxSuggestionsStudents_KeyDown;

            panel4.Controls.Add(listBoxSuggestionsStudents);

            listBoxSuggestionsStudents.BringToFront();

        }
        private void UpdateSuggestions(string searchText)
        {
            suggestions.Clear();
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();

                string query = "SELECT Description FROM tbl_Section WHERE AcadLevel_ID = @acadlvl";
                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@acadlvl", subacadlvl);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestions.Add(reader["Description"].ToString());
                        }
                    }
                }
            }
            

            var filteredSuggestions = suggestions
                .Where(s => s.ToLower().Contains(searchText.ToLower()))
                .ToArray();

            listBoxSuggestions.DataSource = filteredSuggestions;

            // Update the visibility of the ListBox based on whether the search text is empty
            listBoxSuggestions.Visible = !string.IsNullOrWhiteSpace(searchText) && filteredSuggestions.Length > 0;
        }
        private void UpdateSuggestionsStudents(string searchText)
        {
            suggestionsstudents.Clear();
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();

                string query = "SELECT UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM tbl_students_account s LEFT JOIN tbl_section sec ON s.Section = sec.Section_ID WHERE sec.AcadLevel_ID = @acadlvl AND s.Status = 1";
                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@acadlvl", subacadlvl);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestionsstudents.Add(reader["Name"].ToString());
                        }
                    }
                }
            }


            var filteredSuggestions = suggestionsstudents
                .Where(s => s.ToLower().Contains(searchText.ToLower()))
                .ToArray();

            listBoxSuggestionsStudents.DataSource = filteredSuggestions;

            // Update the visibility of the ListBox based on whether the search text is empty
            listBoxSuggestionsStudents.Visible = !string.IsNullOrWhiteSpace(searchText) && filteredSuggestions.Length > 0;
        }
        public void displaysubjectinfo()
        {
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string query = "SELECT Course_Description FROM tbl_subjects WHERE Course_Code = @Ccode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            string CourseDes = rd["Course_Description"].ToString();
                            lblHeaderSubject.Text = "Add Section to " + CourseDes;
                        }
                    }
                }
            }
        }
        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    lblSection.Text = selectedSuggestion;
                    lblsecstud.Text = "Students on Section " + selectedSuggestion;
                    displaySectionStudents(selectedSuggestion);
                    tbSearch.Text = string.Empty;
                    btnDone.Enabled = true;
                }
            }
        }
        private void listBoxSuggestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    lblSection.Text = selectedSuggestion;
                    lblsecstud.Text = "Students on Section " + selectedSuggestion;
                    displaySectionStudents(selectedSuggestion);
                    tbSearch.Text = string.Empty;
                    btnDone.Enabled = true;
                }
            }
        }
        private void listBoxSuggestions_DoubleClick(object sender, EventArgs e)
        {
            string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSuggestion))
            {
                lblSection.Text = selectedSuggestion;
                lblsecstud.Text = "Students on Section " + selectedSuggestion;
                displaySectionStudents(selectedSuggestion);
                tbSearch.Text = string.Empty;
                btnDone.Enabled = true;
            }
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateSuggestions(tbSearch.Text);
        }
        private void listBoxSuggestionsStudents_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestionsStudents.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    displaySelectedStudents(selectedSuggestion);
                    tbSearchStudent.Text = string.Empty;
                }
            }
        }
        private void listBoxSuggestionsStudents_DoubleClick(object sender, EventArgs e)
        {
            string selectedSuggestion = listBoxSuggestionsStudents.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSuggestion))
            {
                displaySelectedStudents(selectedSuggestion);
                tbSearchStudent.Text = string.Empty;
            }
        }
        public void displaySelectedStudents(string studname)
        {
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();

                // Check if the student is already in the DataGridView
                if (IsStudentAlreadyAdded(studname))
                {
                    MessageBox.Show("Student already added to the list.");
                    return;
                }

                string query = "SELECT s.ID AS id, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM tbl_students_account s LEFT JOIN tbl_section sec ON s.Section = sec.Section_ID WHERE Name = @name AND sec.AcadLevel_ID = @acadlvl AND s.Status = 1 ORDER BY Name";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@name", studname);
                    command.Parameters.AddWithValue("@acadlvl", subacadlvl);

                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            int rowIndex = dgvSectionStudents.Rows.Add(false);
                            dgvSectionStudents.Rows[rowIndex].Cells["id"].Value = rd["id"].ToString();
                            dgvSectionStudents.Rows[rowIndex].Cells["name"].Value = rd["Name"].ToString().ToUpper();
                        }
                    }
                }
            }
        }

        private bool IsStudentAlreadyAdded(string studname)
        {
            foreach (DataGridViewRow row in dgvSectionStudents.Rows)
            {
                string nameInGrid = row.Cells["name"].Value?.ToString();

                if (nameInGrid != null && nameInGrid.Equals(studname, StringComparison.OrdinalIgnoreCase))
                {
                    return true; 
                }
            }

            return false;
        }

        private void tbSearchStudent_TextChanged(object sender, EventArgs e)
        {
            UpdateSuggestionsStudents(tbSearchStudent.Text);
        }
        private void tbSearchStudent_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestionsStudents.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    displaySelectedStudents(selectedSuggestion);
                    tbSearchStudent.Text = string.Empty;
                }
            }
        }
        public void displaySectionStudents(string sectionname)
        {
            dgvSectionStudents.Rows.Clear();
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();
                string query = "SELECT s.ID AS id, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM tbl_students_account s LEFT JOIN tbl_section sec ON s.Section = sec.Section_ID WHERE sec.Description = @Secdes AND sec.AcadLevel_ID = @acadlvl AND s.Status = 1 ORDER BY Name";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@Secdes", sectionname);
                    command.Parameters.AddWithValue("@acadlvl", subacadlvl);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            int rowIndex = dgvSectionStudents.Rows.Add(false);
                            dgvSectionStudents.Rows[rowIndex].Cells["id"].Value = rd["id"].ToString();
                            dgvSectionStudents.Rows[rowIndex].Cells["name"].Value = rd["Name"].ToString().ToUpper();
                        }
                    }
                }
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formAddSectionToSubject_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
        }

        private void dgvSectionStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvSectionStudents.Columns[e.ColumnIndex].Name;

            if (col == "del")
            {
                // Get the index of the clicked row
                int rowIndex = e.RowIndex;

                // Check if the clicked cell is in a valid row
                if (rowIndex >= 0 && rowIndex < dgvSectionStudents.Rows.Count)
                {
                    dgvSectionStudents.Rows.RemoveAt(rowIndex);
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();
                string secID = String.Empty;
                string teachUID = FormTeacherNavigation.id;
                string Ccode = ccode;
                string schYear = String.Empty;
                string Tersem = String.Empty;
                string Shssem = String.Empty;
                string classCode = GeneratePassword(4) + ccode;

                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    string query = "SELECT Section_ID FROM tbl_section WHERE Description = @secdes";
                    command.CommandText = query;
                    command.Parameters.AddWithValue("@secdes", lblSection.Text);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            secID = rd["Section_ID"].ToString();
                        }
                    }
                }
                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    string query = "SELECT Acad_ID FROM tbl_acad WHERE Status = 1";
                    command.CommandText = query;
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            schYear = rd["Acad_ID"].ToString();
                        }
                    }
                }

                using (SQLiteCommand cm = new SQLiteCommand(cn))
                {
                    string query = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";
                    cm.CommandText = query;
                    using (SQLiteDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Shssem = dr["Quarter_ID"].ToString();
                        }
                    }
                }

                using (SQLiteCommand cm = new SQLiteCommand(cn))
                {
                    string query = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";
                    cm.CommandText = query;
                    using (SQLiteDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Tersem = dr["Semester_ID"].ToString();
                        }
                    }
                }

                using (SQLiteCommand checkCommand = new SQLiteCommand(cn))
                {
                    string checkQuery = "";
                    if (subacadlvl.Equals("10001"))
                    {
                        // Check if the section is already present for the same subject or course
                        checkQuery = "SELECT COUNT(*) FROM tbl_class_list WHERE Section_ID = @SecID AND Course_Code = @Ccode AND School_Year = @schyear AND Semester = @sem";
                    }
                    else
                    {
                        checkQuery = "SELECT COUNT(*) FROM tbl_class_list WHERE Section_ID = @SecID AND Course_Code = @Ccode AND School_Year = @schyear AND Semester = @quar";
                    }
                    checkCommand.CommandText = checkQuery;
                    checkCommand.Parameters.AddWithValue("@SecID", secID);
                    checkCommand.Parameters.AddWithValue("@Ccode", ccode);
                    checkCommand.Parameters.AddWithValue("@schyear", schYear);
                    checkCommand.Parameters.AddWithValue("@sem", Tersem);
                    checkCommand.Parameters.AddWithValue("@quar", Shssem);

                    int existingCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (existingCount > 0)
                    {
                        MessageBox.Show("Section already exists for the same subject or course.");
                        return;
                    }
                }

                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_class_list (Class_Code, Section_ID, Teacher_ID, Course_Code, School_Year, Semester, Acad_Level) VALUES (@ClassCode, @SecID, @TeachID, @Ccode, @SchoolYear, @Sem, @Acadlvl);";

                    
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@ClassCode", classCode);
                    command.Parameters.AddWithValue("@SecID", secID);
                    command.Parameters.AddWithValue("@TeachID", teachUID);
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@SchoolYear", schYear);
                    if (subacadlvl.Equals("10001"))
                    {
                        command.Parameters.AddWithValue("@Sem", Tersem);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Sem", Shssem);
                    }
                    command.Parameters.AddWithValue("@Acadlvl", subacadlvl);
                    command.ExecuteNonQuery();
                }

                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    string tableName = "tbl_" + classCode;
                    string createTableSql = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                                    StudentID INTEGER PRIMARY KEY, 
                                    Class_Code TEXT);";
                    command.CommandText = createTableSql;
                    command.ExecuteNonQuery();
                }


                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    string tableName = "tbl_" + classCode;
                    string insertSql = $@"INSERT INTO {tableName} (StudentID, Class_Code)
                    SELECT @StudID, @ClassCode
                    WHERE NOT EXISTS (SELECT 1 FROM {tableName} WHERE StudentID = @StudID);";

                    command.CommandText = insertSql;

                    foreach (DataGridViewRow row in dgvSectionStudents.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string studentID = row.Cells["id"].Value.ToString();
                            string classCodeValue = classCode;

                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@StudID", studentID);
                            command.Parameters.AddWithValue("@ClassCode", classCodeValue);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                cn.Close();
                btnDone.Enabled = false;
                form.displaySectionOfSubject();
                this.Close();
            }
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
