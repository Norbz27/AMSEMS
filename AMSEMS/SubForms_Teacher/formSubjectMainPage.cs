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
using System.Windows.Forms.DataVisualization.Charting;
using Series = System.Windows.Forms.DataVisualization.Charting.Series;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectMainPage : Form
    {
        SQLite_Connection conn;
        private Form activeForm;
        static formSubjectInformation form;
        static string ccode;
        static string subjectAcadlvl;
        string schYear = String.Empty;
        string Tersem = String.Empty;
        string Shssem = String.Empty;
        public formSubjectMainPage()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
        }
        public static void setForm(formSubjectInformation form1, string ccode1)
        {
            form = form1;
            ccode = ccode1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
            getAcad();
            PopulateStudentsChart();
            PopulateAttendanceTrendChart();
        }
        public void getAcad()
        {
            using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
            {
                cn.Open();
                using (SQLiteCommand command = new SQLiteCommand(cn))
                {
                    string query1 = "SELECT Acad_ID FROM tbl_acad WHERE Status = 1";
                    command.CommandText = query1;
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
                    string query2 = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";
                    cm.CommandText = query2;
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
                    string query3 = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";
                    cm.CommandText = query3;
                    using (SQLiteDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            Tersem = dr["Semester_ID"].ToString();
                        }
                    }
                }
            }
        }
        private void PopulateStudentsChart()
        {
            try
            {
                studperSec.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
                studperSec.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

                studperSec.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
                studperSec.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;
                using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                {
                    cn.Open();
                    string query = "";
                    if (formSubjectInformation.subjectAcadlvl == "10001")
                    {
                        query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND School_Year = @schyear AND Semester = @sem";
                    }
                    else
                    {
                        query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND School_Year = @schyear AND Semester = @quar";
                    }

                    int totalSections = 0;
                    int totalStudents = 0;
                    using (SQLiteCommand classListCommand = new SQLiteCommand(query, cn))
                    {
                        classListCommand.Parameters.AddWithValue("@teachID", FormTeacherNavigation.id);
                        classListCommand.Parameters.AddWithValue("@Ccode", ccode);
                        classListCommand.Parameters.AddWithValue("@schyear", schYear);
                        classListCommand.Parameters.AddWithValue("@sem", Tersem);
                        classListCommand.Parameters.AddWithValue("@quar", Shssem);
                        using (SQLiteDataReader classListReader = classListCommand.ExecuteReader())
                        {
                            while (classListReader.Read())
                            {
                                // For each class, get the class code and dynamically generate the table name.
                                string classCode = classListReader["Class_Code"].ToString();
                                string secDes = classListReader["secdes"].ToString();
                                string tableName = "tbl_" + classCode;

                                // Query to check if the table exists.
                                string tableExistsQuery = $"SELECT 1 FROM sqlite_master WHERE type='table' AND name='{tableName}'";

                                using (SQLiteCommand tableExistsCommand = new SQLiteCommand(tableExistsQuery, cn))
                                {
                                    object result = tableExistsCommand.ExecuteScalar();

                                    // Check if the table exists.
                                    if (result != null && result != DBNull.Value)
                                    {
                                        totalSections++;
                                        // Table exists, query the student count.
                                        string studentCountQuery = $"SELECT COUNT(StudentID) AS StudentCount FROM {tableName}";

                                        using (SQLiteCommand studentCountCommand = new SQLiteCommand(studentCountQuery, cn))
                                        {
                                            using (SQLiteDataReader studentCountReader = studentCountCommand.ExecuteReader())
                                            {
                                                if (studentCountReader.Read())
                                                {
                                                    // Add data points to the series.
                                                    int studentCount = Convert.ToInt32(studentCountReader["StudentCount"]);
                                                    studperSec.Series["studpersection"].Points.AddXY(secDes, studentCount);
                                                    totalStudents += studentCount;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        // Table doesn't exist, add 0 as the value.
                                        studperSec.Series["studpersection"].Points.AddXY(secDes, 0);
                                    }
                                }
                            }
                        }
                    }

                    // Set chart properties as needed.
                    studperSec.ChartAreas[0].AxisX.Title = "Sections";
                    studperSec.ChartAreas[0].AxisY.Title = "Number of Students";

                    //// Set chart properties for displaying total students on each class.
                    studperSec.Series["studpersection"].IsValueShownAsLabel = true;
                    studperSec.Series["studpersection"].LabelFormat = "N0"; // Display labels as whole numbers.

                    // Display total sections and total students.
                    lblTotalSection.Text = totalSections.ToString();
                    lblTotalStudents.Text = totalStudents.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PopulateAttendanceTrendChart()
        {
            try
            {
                attendanceTrendChart.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
                attendanceTrendChart.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

                attendanceTrendChart.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
                attendanceTrendChart.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;

                using (SQLiteConnection cn = new SQLiteConnection(conn.connectionString))
                {
                    cn.Open();
                    string query = "";
                    if (formSubjectInformation.subjectAcadlvl == "10001")
                    {
                        query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND School_Year = @schyear AND Semester = @sem";
                    }
                    else
                    {
                        query = "SELECT sec.Description AS secdes, Class_Code FROM tbl_class_list cl LEFT JOIN tbl_section sec ON cl.Section_ID = sec.Section_ID WHERE cl.Teacher_ID = @teachID AND cl.Course_Code = @Ccode AND School_Year = @schyear AND Semester = @quar";
                    }

                    using (SQLiteCommand classListCommand = new SQLiteCommand(query, cn))
                    {
                        classListCommand.Parameters.AddWithValue("@teachID", FormTeacherNavigation.id);
                        classListCommand.Parameters.AddWithValue("@Ccode", ccode);
                        classListCommand.Parameters.AddWithValue("@schyear", schYear);
                        classListCommand.Parameters.AddWithValue("@sem", Tersem);
                        classListCommand.Parameters.AddWithValue("@quar", Shssem);
                        using (SQLiteDataReader classListReader = classListCommand.ExecuteReader())
                        {
                            while (classListReader.Read())
                            {
                                // For each class, get the class code and dynamically generate the table name.
                                string classCode = classListReader["Class_Code"].ToString();
                                string secDes = classListReader["secdes"].ToString();

                                // Query to get the attendance data for the current class.
                                string attendanceQuery = $"SELECT Attendance_date, COUNT(Student_ID) AS AttendanceCount FROM tbl_subject_attendance WHERE Class_Code = @ClassCode GROUP BY Attendance_date";

                                using (SQLiteCommand attendanceCommand = new SQLiteCommand(attendanceQuery, cn))
                                {
                                    attendanceCommand.Parameters.AddWithValue("@ClassCode", classCode);
                                    using (SQLiteDataReader attendanceReader = attendanceCommand.ExecuteReader())
                                    {
                                        Series series = new Series(secDes);
                                        series.ChartType = SeriesChartType.Line;

                                        while (attendanceReader.Read())
                                        {
                                            // Add data points to the series.
                                            string attendanceDate = attendanceReader["Attendance_date"].ToString();
                                            int attendanceCount = Convert.ToInt32(attendanceReader["AttendanceCount"]);
                                            series.Points.AddXY(attendanceDate, attendanceCount);
                                        }

                                        // Add the series to the chart.
                                        attendanceTrendChart.Series.Add(series);
                                    }
                                }
                            }
                        }
                    }

                    // Set chart properties as needed.
                    attendanceTrendChart.ChartAreas[0].AxisX.Title = "Attendance Date";
                    attendanceTrendChart.ChartAreas[0].AxisY.Title = "Attendance Count";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
