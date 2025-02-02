﻿using System;
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
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectInformation : Form
    {
        formAddSectionToSubject formAddSectionToSubject;
        SQLite_Connection conn;
        private Form activeForm;
        static FormTeacherNavigation form;
        static string ccode;
        public static string subjectAcadlvl;
        static string acadlvl;
        static string schYear;
        static string term;
        //string Tersem = String.Empty;
        //string Shssem = String.Empty;
        public formSubjectInformation()
        {
            InitializeComponent();
            formAddSectionToSubject = new formAddSectionToSubject(this);
            conn = new SQLite_Connection();
        }
        public static void setForm(FormTeacherNavigation form1, string ccode1, string acadlvl1, string schyear1, string term1)
        {
            form = form1;
            ccode = ccode1;
            acadlvl = acadlvl1;
            schYear = schyear1;
            term = term1;
        }
        public void getAcad()
        {
            //using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            //{
            //    cn.Open();
            //    using (SQLiteCommand command = new SQLiteCommand(cn))
            //    {
            //        string query1 = "SELECT Acad_ID FROM tbl_acad WHERE Status = 1";
            //        command.CommandText = query1;
            //        using (SQLiteDataReader rd = command.ExecuteReader())
            //        {
            //            if (rd.Read())
            //            {
            //                schYear = rd["Acad_ID"].ToString();
            //            }
            //        }
            //    }

            //    using (SQLiteCommand cm = new SQLiteCommand(cn))
            //    {
            //        string query2 = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";
            //        cm.CommandText = query2;
            //        using (SQLiteDataReader dr = cm.ExecuteReader())
            //        {
            //            if (dr.Read())
            //            {
            //                Shssem = dr["Quarter_ID"].ToString();
            //            }
            //        }
            //    }

            //    using (SQLiteCommand cm = new SQLiteCommand(cn))
            //    {
            //        string query3 = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";
            //        cm.CommandText = query3;
            //        using (SQLiteDataReader dr = cm.ExecuteReader())
            //        {
            //            if (dr.Read())
            //            {
            //                Tersem = dr["Semester_ID"].ToString();
            //            }
            //        }
            //    }
            //}
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            //getAcad();
            displaysubjectinfo();
            displaySectionOfSubject();
            formSubjectMainPage.setForm(this, ccode, subjectAcadlvl, schYear, term);
            OpenChildForm(new formSubjectMainPage());
        }
        public void displaysubjectinfo()
        {
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string query = "SELECT Course_Code, Course_Description, Image, Academic_Level_ID FROM tbl_subjects s LEFT JOIN tbl_academic_level al ON s.Academic_Level = al.Academic_Level_ID WHERE Course_Code = @Ccode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            string CourseCode = rd["Course_Code"].ToString();
                            string CourseDes = rd["Course_Description"].ToString();
                            string CourseAcadlvl = rd["Academic_Level_ID"].ToString();
                            object image = rd["Image"];
                            Image img = conn.ConvertToImage(image);

                            subjectAcadlvl = CourseAcadlvl;
                            lblSubjectName.Text = CourseDes;
                            ptbSubjectPic.Image = img;
                        }
                    }
                }
            }
        }
        public void displaySectionOfSubject()
        {
            panel4.Controls.Clear();
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string query = "";
                if (subjectAcadlvl == "10001")
                {
                    query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID LEFT JOIN tbl_Semester sm ON cl.Semester = sm.Semester_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND (ad.Academic_Year_Start ||'-'|| ad.Academic_Year_End) = @schyear AND sm.Description = @sem";
                }
                else
                {
                    query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID LEFT JOIN tbl_acad ad ON cl.School_Year = ad.Acad_ID LEFT JOIN tbl_Quarter qr ON cl.Semester = qr.Quarter_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND (ad.Academic_Year_Start ||'-'|| ad.Academic_Year_End) = @schyear AND qr.Description = @sem";
                }

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@teachID", FormTeacherNavigation.id);
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@schyear", schYear);
                    command.Parameters.AddWithValue("@sem", term);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while(rd.Read())
                        {
                            string SectionDes = rd["secdes"].ToString();
                            string classcode = rd["Class_Code"].ToString();
                            apperanceSectionOfSebject(SectionDes, classcode, subjectAcadlvl);
                        }
                    }
                }
            }
        }
        public void apperanceSectionOfSebject(string sectionName, string classcode, string subacadlvl)
        {
            System.Windows.Forms.Button btnSection = new System.Windows.Forms.Button();
            btnSection.Dock = System.Windows.Forms.DockStyle.Top;
            btnSection.FlatAppearance.BorderSize = 0;
            btnSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSection.Location = new System.Drawing.Point(0, 0);
            btnSection.Size = new System.Drawing.Size(169, 28);
            btnSection.TabIndex = 0;
            btnSection.Text = sectionName;
            btnSection.Cursor = Cursors.Hand;
            btnSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnSection.UseVisualStyleBackColor = true;
            btnSection.Click += (senderbtn, ebtn) =>
            {
                formSubjectOverview.setCode(ccode, classcode, subjectAcadlvl, this, schYear, term);
                OpenChildForm(new formSubjectOverview());
            };

            panel4.Controls.Add(btnSection);
        }
        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            childForm.FormBorderStyle = FormBorderStyle.None;
            this.pnView.Controls.Add(childForm);
            this.pnView.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }
        public void OpenMainPage()
        {
            formSubjectMainPage.setForm(this, ccode, subjectAcadlvl, schYear, term);
            OpenChildForm(new formSubjectMainPage());
        }
        private void btnback_Click(object sender, EventArgs e)
        {
            form.otherformclick();
        }

        private void btnOption_Click(object sender, EventArgs e)
        {
            CMSOptions.Show(btnOption, new System.Drawing.Point(0,btnOption.Height));
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formAddSectionToSubject.setSubject(ccode, subjectAcadlvl);
            formAddSectionToSubject.ShowDialog();
        }

        private void btnMainPage_Click(object sender, EventArgs e)
        {
            formSubjectMainPage.setForm(this, ccode, subjectAcadlvl, schYear, term);
            OpenChildForm(new formSubjectMainPage());
        }
        private bool IsInternetConnected()
        {
            try
            {
                using (var client = new Ping())
                {
                    var result = client.Send("8.8.8.8", 1000);
                    return result.Status == IPStatus.Success;
                }
            }
            catch
            {
                return false;
            }
        }
        private async void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to upload attendance record to cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                await Task.Delay(3000);
                if (IsInternetConnected())
                {
                    try
                    {
                        using (SqlConnection cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            await cnn.OpenAsync();
                            System.Data.DataTable attendance_record = conn.GetAttendanceRecord();
                            System.Data.DataTable class_list = conn.GetClassList();
                            
                            if (attendance_record.Rows.Count > 0 || class_list.Rows.Count > 0)
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
                                    System.Data.DataTable class_stud_list = conn.GetStudList(tableName);
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
            }
        }
    }
}
