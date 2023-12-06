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
        static string subjectAcadlvl;
        static string acadlvl;
        public formSubjectInformation()
        {
            InitializeComponent();
            formAddSectionToSubject = new formAddSectionToSubject(this);
            conn = new SQLite_Connection();
        }
        public static void setForm(FormTeacherNavigation form1, string ccode1, string acadlvl1)
        {
            form = form1;
            ccode = ccode1;
            acadlvl = acadlvl1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
            displaySectionOfSubject();
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
                string query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@teachID", FormTeacherNavigation.id);
                    command.Parameters.AddWithValue("@Ccode", ccode);
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
                formSubjectOverview.setCode(ccode, classcode, subjectAcadlvl, this);
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
            OpenChildForm(new formSubjectMainPage());
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
            }
        }
    }
}
