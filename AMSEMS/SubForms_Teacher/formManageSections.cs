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
    public partial class formManageSections : KryptonForm
    {
        private List<string> suggestionsstudents = new List<string> { };
        private ListBox listBoxSuggestionsStudents;
        SQLite_Connection conn;
        private const string AllowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();
        string ccode;
        string subacadlvl;
        string classcode;
        formSubjectOverview form;
        public formManageSections(formSubjectOverview formSubjectOverview)
        {
            InitializeComponent();
            InitializeListBox();
            conn = new SQLite_Connection();
            form = formSubjectOverview;
        }
        public void setSubject(string ccode, string subacadlvl, string classcode)
        {
            this.ccode = ccode;
            this.subacadlvl = subacadlvl;
            this.classcode = classcode;
        }
        private void InitializeListBox()
        {
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

            panel1.Controls.Add(listBoxSuggestionsStudents);

            listBoxSuggestionsStudents.BringToFront();

        }
       
        private void UpdateSuggestionsStudents(string searchText)
        {
            suggestionsstudents.Clear();
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();

                string query = "SELECT UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM tbl_students_account s LEFT JOIN tbl_section sec ON s.Section = sec.Section_ID WHERE sec.AcadLevel_ID = @acadlvl";
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

                string query = "SELECT s.ID AS id, UPPER(s.Lastname || ', ' || s.Firstname || ' ' || s.Middlename) AS Name FROM tbl_students_account s LEFT JOIN tbl_section sec ON s.Section = sec.Section_ID WHERE Name = @name AND sec.AcadLevel_ID = @acadlvl ORDER BY Name";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@name", studname);
                    command.Parameters.AddWithValue("@acadlvl", subacadlvl);

                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            int rowIndex = dgvSections.Rows.Add(false);
                            dgvSections.Rows[rowIndex].Cells["id"].Value = rd["id"].ToString();
                            dgvSections.Rows[rowIndex].Cells["name"].Value = rd["Name"].ToString().ToUpper();
                        }
                    }
                }
            }
        }

        private bool IsStudentAlreadyAdded(string studname)
        {
            foreach (DataGridViewRow row in dgvSections.Rows)
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
            if(dgvSections.Rows.Count > 0)
            {
                btnDone.Enabled = true;
            }
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
            string col = dgvSections.Columns[e.ColumnIndex].Name;

            if (col == "del")
            {
                // Get the index of the clicked row
                int rowIndex = e.RowIndex;

                // Check if the clicked cell is in a valid row
                if (rowIndex >= 0 && rowIndex < dgvSections.Rows.Count)
                {
                    dgvSections.Rows.RemoveAt(rowIndex);
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (dgvSections.Rows.Count > 0)
            {
                using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                {
                    cn.Open();
                    using (SQLiteCommand command = new SQLiteCommand(cn))
                    {
                        string tableName = "tbl_" + classcode;
                        string insertSql = $"INSERT INTO {tableName} (StudentID, Class_Code) VALUES (@StudID, @ClassCode);";

                        command.CommandText = insertSql;

                        foreach (DataGridViewRow row in dgvSections.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                string studentID = row.Cells["id"].Value.ToString();
                                string classCodeValue = classcode;

                                command.Parameters.Clear();
                                command.Parameters.AddWithValue("@StudID", studentID);
                                command.Parameters.AddWithValue("@ClassCode", classCodeValue);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    cn.Close();
                    btnDone.Enabled = false;
                    form.displayStudents();
                    this.Close();
                }
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
