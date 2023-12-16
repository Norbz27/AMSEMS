﻿using System.Data.SqlClient;
using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data.SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using AMSEMS.SubForms_SAO;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        SQLite_Connection con;
        static FormTeacherNavigation form;
        public formDashboard()
        {
            InitializeComponent();

            con = new SQLite_Connection();

            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;
        }
        public static void setForm(FormTeacherNavigation form1)
        {
            form = form1;
        }
        public async void loadData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();
                cm = new SqlCommand("select Firstname, Lastname from tbl_teacher_accounts where Unique_ID = '" + FormTeacherNavigation.id + "'", cn);
                dr = await cm.ExecuteReaderAsync();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
            }
        }
        public async void displayChart()
        {
            await DisplayStudPerSubChartAsync();
            await DisplaySubjectsSummaryAsync();
        }
        public async Task DisplayStudPerSubChartAsync()
        {
            using (SQLiteConnection cn = new SQLiteConnection(con.connectionString))
            {
                await cn.OpenAsync();

                // Dictionary to store the total student count for each course code
                Dictionary<string, int> courseStudentCount = new Dictionary<string, int>();

                // Retrieve subjects assigned to the teacher
                string query = "SELECT s.Course_code, s.Course_Description, s.Image, s.Academic_Level, cl.Class_Code " +
                                "FROM tbl_subjects s " +
                                "LEFT JOIN tbl_class_list cl ON s.Course_code = cl.Course_Code " +
                                "LEFT JOIN tbl_teachers_account t ON s.Assigned_Teacher = t.ID " +
                                "WHERE t.Unique_ID = @TeachID AND s.Status = 1 AND cl.Class_Code IS NOT NULL";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@TeachID", FormTeacherNavigation.id);

                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string courseDes = rd["Course_Description"].ToString();
                            string classCode = rd["Class_Code"].ToString();
                            // Construct the dynamic table name
                            string dynamicTableName = $"tbl_{classCode}";

                            // Check if the table exists
                            string tableExistQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@TableName";

                            using (SQLiteCommand tableExistCommand = new SQLiteCommand(tableExistQuery, cn))
                            {
                                tableExistCommand.Parameters.AddWithValue("@TableName", dynamicTableName);

                                int tableCount = Convert.ToInt32(tableExistCommand.ExecuteScalar());

                                if (tableCount > 0)
                                {
                                    // The table exists, proceed with your original logic

                                    // Query the dynamic table to get the count of students
                                    string studentQuery = $"SELECT COUNT(StudentID) AS StudentCount FROM {dynamicTableName} WHERE Class_Code = @ClassCode";

                                    using (SQLiteCommand studentCommand = new SQLiteCommand(studentQuery, cn))
                                    {
                                        studentCommand.Parameters.AddWithValue("@ClassCode", classCode);

                                        // Retrieve the student count
                                        int studentCount = Convert.ToInt32(studentCommand.ExecuteScalar());

                                        // Accumulate the student count for the course code
                                        if (courseStudentCount.ContainsKey(courseDes))
                                        {
                                            courseStudentCount[courseDes] += studentCount;
                                        }
                                        else
                                        {
                                            courseStudentCount[courseDes] = studentCount;
                                        }
                                    }
                                }
                            }
                        }
                        await Task.Run(() =>
                        {
                            foreach (var kvp in courseStudentCount)
                            {
                                string key = kvp.Key;
                                int value = kvp.Value;

                                chart1.Invoke((MethodInvoker)delegate
                                {
                                    chart1.Series["StudPerSub"].Points.AddXY(key, value);
                                });
                            }

                            chart1.Invoke((MethodInvoker)delegate
                            {
                                chart1.ChartAreas[0].AxisX.Title = "Subjects";
                                chart1.ChartAreas[0].AxisY.Title = "Number of Students";
                            });
                        });
                    }
                }
            }
        }
        public async Task DisplaySubjectsSummaryAsync()
        {
            using (SQLiteConnection cn = new SQLiteConnection(con.connectionString))
            {
                await cn.OpenAsync();

                // Dictionary to store the total student count for each course code
                Dictionary<string, int> courseStudentCount = new Dictionary<string, int>();
                int totalStudentCount = 0;
                int totalSubCount = 0;

                // Retrieve subjects assigned to the teacher
                string query = "SELECT s.Course_code, s.Course_Description, s.Image, s.Academic_Level, cl.Class_Code " +
                                "FROM tbl_subjects s " +
                                "LEFT JOIN tbl_class_list cl ON s.Course_code = cl.Course_Code " +
                                "LEFT JOIN tbl_teachers_account t ON s.Assigned_Teacher = t.ID " +
                                "WHERE t.Unique_ID = @TeachID AND s.Status = 1 AND cl.Class_Code IS NOT NULL";

                using (SQLiteCommand command = new SQLiteCommand(query, cn))
                {
                    command.Parameters.AddWithValue("@TeachID", FormTeacherNavigation.id);

                    using (SQLiteDataReader rd = command.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            string courseDes = rd["Course_Description"].ToString();
                            string classCode = rd["Class_Code"].ToString();

                            // Construct the dynamic table name
                            string dynamicTableName = $"tbl_{classCode}";

                            // Check if the table exists
                            string tableExistQuery = "SELECT COUNT(*) FROM sqlite_master WHERE type='table' AND name=@TableName";

                            using (SQLiteCommand tableExistCommand = new SQLiteCommand(tableExistQuery, cn))
                            {
                                tableExistCommand.Parameters.AddWithValue("@TableName", dynamicTableName);

                                int tableCount = Convert.ToInt32(tableExistCommand.ExecuteScalar());

                                if (tableCount > 0)
                                {
                                    // The table exists, proceed with your original logic

                                    // Query the dynamic table to get the count of students
                                    string studentQuery = $"SELECT COUNT(StudentID) AS StudentCount FROM {dynamicTableName} WHERE Class_Code = @ClassCode";

                                    using (SQLiteCommand studentCommand = new SQLiteCommand(studentQuery, cn))
                                    {
                                        studentCommand.Parameters.AddWithValue("@ClassCode", classCode);

                                        // Retrieve the student count
                                        int studentCount = Convert.ToInt32(studentCommand.ExecuteScalar());

                                        // Accumulate the student count for the course code
                                        if (courseStudentCount.ContainsKey(courseDes))
                                        {
                                            courseStudentCount[courseDes] += studentCount;
                                        }
                                        else
                                        {
                                            courseStudentCount[courseDes] = studentCount;
                                        }

                                        // Accumulate the total student count
                                        totalStudentCount += studentCount;
                                    }

                                    totalSubCount++;
                                }
                            }

                        }
                        await Task.Run(() =>
                        {
                            foreach (var kvp in courseStudentCount)
                            {
                                string subject = kvp.Key;
                                int studentCount = kvp.Value;

                                // Calculate the percentage
                                double percentage = (double)studentCount / totalStudentCount * 100;

                                chart2.Invoke((MethodInvoker)delegate
                                {
                                    // Add the data point to the donut chart
                                    chart2.Series["SubjectStudents"].Points.AddY(percentage);
                                    chart2.Series["SubjectStudents"].Points.Last().AxisLabel = $"{subject} \r\n {percentage:0.00}%";
                                });
                            }

                            chart2.Invoke((MethodInvoker)delegate
                            {
                                // Update labels and chart titles
                                lblStud.Text = totalStudentCount.ToString();
                                lbltotalSub.Text = totalSubCount.ToString();
                                chart2.ChartAreas[0].AxisX.Title = "Subjects";
                                chart2.ChartAreas[0].AxisY.Title = "Percentage of Students";
                            });
                        });
                    }
                }
            }
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            loadData();
            displayChart();
        }

        private void formDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {
            controls();
            
        }
        public void controls()
        {
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            SubForms_Teacher.formSubjects.setForm(form);
            form.OpenChildForm(new SubForms_Teacher.formSubjects());
            form.btnSubjects.Focus();
            form.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            form.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            form.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            form.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            form.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }
    }
}
