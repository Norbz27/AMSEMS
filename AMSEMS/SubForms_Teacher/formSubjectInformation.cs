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

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectInformation : Form
    {
        formAddSectionToSubject formAddSectionToSubject;
        SQLite_Connection conn;

        static FormTeacherNavigation form;
        static string ccode;
        static string subjectAcadlvl;
        public formSubjectInformation()
        {
            InitializeComponent();
            formAddSectionToSubject = new formAddSectionToSubject();
            conn = new SQLite_Connection();
        }
        public static void setForm(FormTeacherNavigation form1, string ccode1)
        {
            form = form1;
            ccode = ccode1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
            displaySectionOfSubject();
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
            using (SQLiteConnection con = new SQLiteConnection(conn.connectionString))
            {
                con.Open();
                string query = "SELECT sec.Description AS secdes FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode";

                using (SQLiteCommand command = new SQLiteCommand(query, con))
                {
                    command.Parameters.AddWithValue("@teachID", FormTeacherNavigation.id);
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while(rd.Read())
                        {
                            string SectionDes = rd["secdes"].ToString();
                            apperanceSectionOfSebject(SectionDes);
                        }
                    }
                }
            }
        }
        public void apperanceSectionOfSebject(String sectionName)
        {
            System.Windows.Forms.Button btnSection = new System.Windows.Forms.Button();
            btnSection.Dock = System.Windows.Forms.DockStyle.Top;
            btnSection.FlatAppearance.BorderSize = 0;
            btnSection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnSection.Location = new System.Drawing.Point(0, 0);
            btnSection.Size = new System.Drawing.Size(169, 28);
            btnSection.TabIndex = 0;
            btnSection.Text = sectionName;
            btnSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnSection.UseVisualStyleBackColor = true;

            panel4.Controls.Add(btnSection);
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
    }
}
