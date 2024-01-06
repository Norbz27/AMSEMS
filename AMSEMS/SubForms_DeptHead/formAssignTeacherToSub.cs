using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAssignTeacherToSub : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        private List<string> suggestions = new List<string> { };
        private List<string> suggestionsstudents = new List<string> { };
        private ListBox listBoxSuggestions;
        String choice;
        bool istrue = false;

        formSubjects form;
        private string schoolYear, sem;

        public formAssignTeacherToSub()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            InitializeListBox();
            if(FormDeptHeadNavigation.acadlevel == "SHS")
            {
                lblSemOrQuar.Text = "Quarter";
            }
            else
            {
                lblSemOrQuar.Text = "Semester";
            }
        }
        public void setData(String choice, formSubjects form)
        {
            this.form = form;
            this.choice = choice;
        }
        public void displaySubjects()
        {
            string query = "Select Course_code,Course_Description,Units,d.Description as ddes,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID where (@AcadLevelDescription IS NULL OR al.Academic_Level_Description = @AcadLevelDescription) AND (d.Description IS NULL OR d.Description = @DepDescription) AND s.Status = 1 ORDER BY 1 DESC";
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@AcadLevelDescription", FormDeptHeadNavigation.acadlevel);
                    cmd.Parameters.AddWithValue("@DepDescription", FormDeptHeadNavigation.depdes);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            String Course = dr["Course_Description"].ToString();
                            cbSubject.Items.Add(Course);
                        }
                    }
                }
            }
        }
        public void displayAcademicYear()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";
                
                query = "SELECT Acad_ID, Academic_Year_Start +'-'+ Academic_Year_End AS acad_year FROM tbl_acad WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tbSchoolYear.Text = dr["acad_year"].ToString();
                            schoolYear = dr["Acad_ID"].ToString();
                        }
                    }
                }
                if (FormDeptHeadNavigation.acadlevel == "SHS")
                {
                    query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter WHERE Status = 1";
                }
                else
                {
                    query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester WHERE Status = 1";
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            tbSem.Text = dr["Description"].ToString();
                            sem = dr["ID"].ToString();
                        }
                    }
                }
            }
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

        }
        private void listBoxSuggestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();

                        string query = "SELECT UPPER(Lastname +', '+ Firstname +' '+ Middlename) AS Name, d.Description FROM tbl_teacher_accounts ta LEFT JOIN tbl_Departments d ON ta.Department = d.Department_ID WHERE UPPER(Lastname +', '+ Firstname +' '+ Middlename) = @name";
                        using (SqlCommand command = new SqlCommand(query, cn))
                        {
                            command.Parameters.AddWithValue("@name", selectedSuggestion);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    lblName.Text = reader["Name"].ToString();
                                    lblDepartment.Text = reader["Description"].ToString();
                                }
                            }
                        }
                    }
                    tbSearch.Text = string.Empty;
                }
            }
        }
        private void listBoxSuggestions_DoubleClick(object sender, EventArgs e)
        {
            string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSuggestion))
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    string query = "SELECT UPPER(Lastname +', '+ Firstname +' '+ Middlename) AS Name, d.Description FROM tbl_teacher_accounts ta LEFT JOIN tbl_Departments d ON ta.Department = d.Department_ID WHERE UPPER(Lastname +', '+ Firstname +' '+ Middlename) = @name";
                    using (SqlCommand command = new SqlCommand(query, cn))
                    {
                        command.Parameters.AddWithValue("@name", selectedSuggestion);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblName.Text = reader["Name"].ToString();
                                lblDepartment.Text = reader["Description"].ToString();
                            }
                        }
                    }
                }
                tbSearch.Text = string.Empty;
            }
        }
        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displaySubjects();
            displayAcademicYear();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblName.Text.Equals(Name) || cbSubject.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string TeachID, CourseCode;
                        string query = "";
                        query = "SELECT ta.ID FROM tbl_teacher_accounts ta LEFT JOIN tbl_Departments d ON ta.Department = d.Department_ID WHERE UPPER(Lastname +', '+ Firstname +' '+ Middlename) = @name";
                        using (SqlCommand cm = new SqlCommand(query, cn))
                        {
                            cm.Parameters.AddWithValue("@name", lblName.Text);
                            using (SqlDataReader dr = cm.ExecuteReader())
                            {
                                dr.Read();
                                TeachID = dr["ID"].ToString();
                            }
                        }
                        query = "Select Course_code from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_Departments d on s.Department_ID = d.Department_ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID where Course_Description = @cdes";
                        using (SqlCommand cm = new SqlCommand(query, cn))
                        {
                            cm.Parameters.AddWithValue("@cdes", cbSubject.Text);
                            using (SqlDataReader dr = cm.ExecuteReader())
                            {
                                dr.Read();
                                CourseCode = dr["Course_code"].ToString();
                            }
                        }

                        string checkQuery = "";
                        if (FormDeptHeadNavigation.acadlevel == "SHS")
                        {
                            checkQuery = "SELECT COUNT(*) FROM tbl_shs_assigned_teacher_to_sub " +
                                         "WHERE Teacher_ID = @TeacherID AND Course_Code = @CourseCode " +
                                         "AND Quarter = @Sem AND School_Year = @SchoolYear AND Department_ID = @Dep";
                        }
                        else
                        {
                            checkQuery = "SELECT COUNT(*) FROM tbl_ter_assigned_teacher_to_sub " +
                                         "WHERE Teacher_ID = @TeacherID AND Course_Code = @CourseCode " +
                                         "AND Semester = @Sem AND School_Year = @SchoolYear AND Department_ID = @Dep";
                        }

                        using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                        {
                            checkCommand.Parameters.AddWithValue("@TeacherID", TeachID);
                            checkCommand.Parameters.AddWithValue("@CourseCode", CourseCode);
                            checkCommand.Parameters.AddWithValue("@Sem", sem);
                            checkCommand.Parameters.AddWithValue("@SchoolYear", schoolYear);
                            checkCommand.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);

                            int existingRecords = (int)checkCommand.ExecuteScalar();

                            if (existingRecords > 0)
                            {
                                // Assignment already exists, show a message
                                MessageBox.Show("This assignment already exists.", "Duplicate Assignment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return; 
                            }
                        }

                        string insertquery = "";
                        if (FormDeptHeadNavigation.acadlevel == "SHS")
                        {
                            insertquery = "INSERT INTO tbl_shs_assigned_teacher_to_sub " +
                                        "(Teacher_ID, Course_Code, Quarter, School_Year, Department_ID) " +
                                        "VALUES (@TeacherID, @CourseCode, @Sem, @SchoolYear, @Dep)";
                        }
                        else
                        {
                            insertquery = "INSERT INTO tbl_ter_assigned_teacher_to_sub " +
                                        "(Teacher_ID, Course_Code, Semester, School_Year, Department_ID) " +
                                        "VALUES (@TeacherID, @CourseCode, @Sem, @SchoolYear, @Dep)";
                        }

                        using (SqlCommand command = new SqlCommand(insertquery, cn))
                        {
                            // Set parameters
                            command.Parameters.AddWithValue("@TeacherID", TeachID);
                            command.Parameters.AddWithValue("@CourseCode", CourseCode);
                            command.Parameters.AddWithValue("@Sem", sem);
                            command.Parameters.AddWithValue("@SchoolYear", schoolYear);
                            command.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);

                            DialogResult result = MessageBox.Show("Do you want to assign this teacher to the subject?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                // Execute the query
                                command.ExecuteNonQuery();

                                // Display confirmation message
                                MessageBox.Show("Teacher assigned to subject successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error, show an error message)
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void formSubjectsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
           
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateSuggestions(tbSearch.Text);
        }
        private void UpdateSuggestions(string searchText)
        {
            suggestions.Clear();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                string query = "SELECT UPPER(Lastname +', '+ Firstname +' '+ Middlename) AS Name FROM tbl_teacher_accounts";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestions.Add(reader["Name"].ToString());
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

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();

                        string query = "SELECT UPPER(Lastname +', '+ Firstname +' '+ Middlename) AS Name, d.Description FROM tbl_teacher_accounts ta LEFT JOIN tbl_Departments d ON ta.Department = d.Department_ID WHERE UPPER(Lastname +', '+ Firstname +' '+ Middlename) = @name";
                        using (SqlCommand command = new SqlCommand(query, cn))
                        {
                            command.Parameters.AddWithValue("@name", selectedSuggestion);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    lblName.Text = reader["Name"].ToString();
                                    lblDepartment.Text = reader["Description"].ToString();
                                }
                            }
                        }
                    }
                    tbSearch.Text = string.Empty;
                }
            }
        }
    }
}
