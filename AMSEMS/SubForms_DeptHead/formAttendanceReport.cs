using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceReport : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        public formAttendanceReport()
        {
            InitializeComponent();
        }
        public void displayFilter()
        {
            cbMonth.Items.Clear();

            for(int i = 1; i <= 12; i++)
            {
                string monthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(i);
                cbMonth.Items.Add(monthName);
            }
            cbMonth.SelectedItem = DateTime.Now.ToString("MMMM");

            int currentYear = DateTime.Now.Year;
            for (int year = currentYear - 10; year <= currentYear; year++)
            {
                cbYear.Items.Add(year.ToString());
            }

            cbYear.SelectedItem = currentYear.ToString();

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_section", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbSection.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
            }
        }

        public void displayReport()
        {
            dgvReport.Columns.Clear();
            dgvReport.Rows.Clear();
            dgvReport.Columns.Add("id", "ID");
            dgvReport.Columns.Add("name", "Name");
            dgvReport.Columns.Add("section", "Section");

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Event_Name, Date_Time FROM tbl_attendance a LEFT JOIN tbl_events e ON e.Event_ID = a.Event_ID ORDER BY Event_Name, Date_Time";

                using (cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader eventsReader = cm.ExecuteReader())
                    {
                        List<string> columnNames = new List<string>();

                        while (eventsReader.Read())
                        {
                            string eventName = eventsReader["Event_Name"].ToString();
                            DateTime date = (DateTime)eventsReader["Date_Time"];
                            string eventDate = date.ToString("MM-dd-yy");
                            string columnName = $"{eventName}  ({eventDate})";

                            if (!columnNames.Contains(columnName))
                            {
                                dgvReport.Columns.Add(columnName, columnName);
                                dgvReport.Columns[columnName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                                columnNames.Add(columnName);
                            }
                        }
                    }
                }

                // Retrieve and display data rows
                query = @"SELECT DISTINCT
                        s.ID AS ID,
                        UPPER(CONCAT(s.Firstname, ' ', s.Middlename, ' ', UPPER(s.Lastname))) AS Name,
                        s.Section AS Section,
                        e.Event_Name,
                        a.Date_Time
                    FROM 
                        tbl_attendance a
                    LEFT JOIN 
                        tbl_events e ON e.Event_ID = a.Event_ID
                    LEFT JOIN 
                        tbl_student_accounts s ON a.Student_ID = s.ID
                    WHERE
                        S.Department = @Dep
                    ORDER BY 
                        Name, 
                        Event_Name, 
                        Date_Time";

                using (cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                    using (SqlDataReader dataReader = cm.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            int id = Convert.ToInt32(dataReader["ID"]);
                            string name = dataReader["Name"].ToString();
                            string section = dataReader["Section"].ToString();

                            dgvReport.Rows.Add(id, name, section);

                            //foreach (string columnName in columnNames)
                            //{
                            //    string eventName = columnName.Split('(')[0].Trim();
                            //    DateTime date = DateTime.ParseExact(columnName.Split('(')[1].Split(')')[0].Trim(), "MM-dd-yy", null);

                            //    // Add the data for each dynamic column
                            //    dgvReport.Rows[dgvReport.Rows.Count - 1].Cells[columnName].Value = GetDataForCell(id, eventName, date); // You need to implement this method to get the data for the corresponding cell
                            //}
                        }
                    }
                }
            }

            dgvReport.Columns.Add("total", "Total Penalty Fee");
        }
        private void formAttendanceReport_Load(object sender, EventArgs e)
        {
            displayFilter();
            displayReport();
        }
    }
}
