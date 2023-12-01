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
    public partial class formSubjectOverview : Form
    {
        SQLite_Connection conn;

        static FormTeacherNavigation form;
        static string ccode;
        static string classcode;
        public formSubjectOverview()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
        }
        public static void setCode(string ccode1, string classcode1)
        {
            ccode = ccode1;
            classcode = classcode1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            displaysubjectinfo();
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
                            dgvStudents.Rows[rowIndex].Cells["role"].Value = "Student";
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
        }
    }
}
