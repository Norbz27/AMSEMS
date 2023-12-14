using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public String id;
        public static String id2 { get; set; }
        static FormDeptHeadNavigation form;
        public formDashboard(String id1)
        {
            InitializeComponent();
            id = id1;

            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray; // Set the color of major grid lines on the X-axis
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray; // Set the color of major grid lines on the Y-axis

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray; // Set the color of minor grid lines on the X-axis
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray; // Set the color of minor grid lines on the Y-axis
        }
        public static void setForm(FormDeptHeadNavigation form1)
        {
            form = form1;
        }

        private async void formDashboard_Load(object sender, EventArgs e)
        {
            if (id.Equals(String.Empty))
                loadData(id2);
            else
                loadData(id);
            await displayChartAsync();
            form.loadData();
        }
        public void loadData(String id)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_deptHead_accounts where Unique_ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
            }
        }
        public async Task displayChartAsync()
        {
            int currentYear = DateTime.Now.Year;
            //Chart1
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync(); // Use OpenAsync to open the connection asynchronously
                using (SqlCommand command = new SqlCommand(@"SELECT e.Event_ID, e.Event_Name, COUNT(a.Student_ID) AS AttendanceCount FROM tbl_events e LEFT JOIN tbl_attendance a ON e.Event_ID = a.Event_ID LEFT JOIN tbl_student_accounts s ON a.Student_ID = s.ID  WHERE e.Attendance = 'true' AND YEAR(e.Start_Date) = @Year AND s.Department = @dep GROUP BY e.Event_ID, e.Event_Name", cn))
                {
                    command.Parameters.AddWithValue("@Year", currentYear);
                    command.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    await Task.Run(() => adapter.Fill(dt)); // Use Task.Run to execute Fill asynchronously

                    chart1.DataSource = dt;
                    chart1.Series["Attendees"].XValueMember = "Event_Name";
                    chart1.Series["Attendees"].YValueMembers = "AttendanceCount";
                }
            }

            // Step 5: Customize Chart
            chart1.ChartAreas[0].AxisX.Title = "Events";
            chart1.ChartAreas[0].AxisY.Title = "Students Attended";

            //Chart2
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"WITH RecentAttendance AS 
                (
                    SELECT 
                        a.[Event_ID], 
                        a.[Student_ID], 
                        MAX(a.[Date_Time]) AS [RecentDate] 
                    FROM 
                        [db_Amsems].[dbo].[tbl_attendance] a 
                    GROUP BY 
                        a.[Event_ID], 
                        a.[Student_ID]
                )

                SELECT  
                    e.[Event_ID], 
                    e.[Event_Name],
                    COUNT(DISTINCT ra.[Student_ID]) AS [RecentAttendees], 
                    COUNT(DISTINCT s.[ID]) AS [TotalStudentsInDepartment], 
                    e.End_Date,
                    CAST(COUNT(DISTINCT ra.[Student_ID]) AS FLOAT) / NULLIF(COUNT(DISTINCT s.[ID]), 0) * 100 AS [PercentageRecentAttendees] 
                FROM 
                    [db_Amsems].[dbo].[tbl_events] e 
                LEFT JOIN 
                    RecentAttendance ra ON e.[Event_ID] = ra.[Event_ID] 
                LEFT JOIN 
                    [db_Amsems].[dbo].[tbl_student_accounts] s ON s.[Department] = @dep
                WHERE 
                    e.Attendance = 'true'
                GROUP BY 
                    e.[Event_ID], 
                    e.[Event_Name], 
                    e.End_Date
                ORDER BY 
                    MAX(ra.[RecentDate]) DESC; ", connection))
                {
                    command.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Display only the event with the highest recent attendees
                    DataRow highestRecentAttendeesRow = dt.Rows.Count > 0 ? dt.Rows[0] : null;

                    // Step 4: Bind Data to Chart
                    if (highestRecentAttendeesRow != null)
                    {
                        string eventName = highestRecentAttendeesRow["Event_Name"] != DBNull.Value ? highestRecentAttendeesRow["Event_Name"].ToString() : "";
                        string attendees = highestRecentAttendeesRow["RecentAttendees"] != DBNull.Value ? highestRecentAttendeesRow["RecentAttendees"].ToString() : "0";
                        string totalStud = highestRecentAttendeesRow["TotalStudentsInDepartment"] != DBNull.Value ? highestRecentAttendeesRow["TotalStudentsInDepartment"].ToString() : "0";
                        double percentageRecentAttendees = Convert.ToDouble(highestRecentAttendeesRow["PercentageRecentAttendees"] != DBNull.Value ? highestRecentAttendeesRow["PercentageRecentAttendees"].ToString() : "0");

                        // Calculate the remaining percentage
                        double remainingPercentage = 100 - percentageRecentAttendees;
                        lbltotalStud.Text = totalStud;
                        lblAttendees.Text = attendees;
                        // Add data points to the chart
                        chart2.Series["AttendanceRateSeries"].Points.AddXY(Convert.ToInt32(percentageRecentAttendees) + "%", percentageRecentAttendees);
                        chart2.Series["AttendanceRateSeries"].Points.AddXY("", remainingPercentage);
                    }

                    // Step 5: Customize Chart
                    chart2.ChartAreas[0].AxisX.Title = "Event";
                    chart2.ChartAreas[0].AxisY.Title = "Percentage";

                    // Customize font
                    chart2.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);
                    chart2.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);
                    chart2.Legends[0].Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);

                    // You can customize the appearance of the series if needed
                    chart2.Series["AttendanceRateSeries"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                }
            }
            //Chart3
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(@"SELECT
                COUNT(*) AS Total_Students,
                SUM(CASE WHEN Remaining_Balance <= 0 THEN 1 ELSE 0 END) AS Paid_Students,
                AVG(CASE WHEN Remaining_Balance <= 0 THEN 100 ELSE (Total_Payment_Amount / Total_Balance_Fee) * 100 END) AS Average_PercentagePaid
            FROM (
                SELECT
                    COALESCE(bf.Student_ID, t.Student_ID) AS Student_ID,
                    COALESCE(SUM(bf.Balance_Fee), 0) AS Total_Balance_Fee,
                    COALESCE(SUM(t.Payment_Amount), 0) AS Total_Payment_Amount,
                    CASE
                        WHEN COALESCE(SUM(bf.Balance_Fee), 0) < COALESCE(SUM(t.Payment_Amount), 0) THEN 0
                        ELSE COALESCE(SUM(bf.Balance_Fee), 0) - COALESCE(SUM(t.Payment_Amount), 0)
                    END AS Remaining_Balance
                FROM (
                    SELECT
                        Student_ID,
                        SUM(Balance_Fee) AS Balance_Fee
                    FROM
                        dbo.tbl_balance_fees
                    GROUP BY
                        Student_ID
                ) bf
                FULL JOIN (
                    SELECT
                        Student_ID,
                        SUM(Payment_Amount) AS Payment_Amount
                    FROM
                        dbo.tbl_transaction
                    GROUP BY
                        Student_ID
                ) t ON bf.Student_ID = t.Student_ID
                JOIN dbo.tbl_student_accounts s ON COALESCE(bf.Student_ID, t.Student_ID) = s.ID
                WHERE
                    s.Status = 1
                    AND s.Department = @dep
                GROUP BY
                    COALESCE(bf.Student_ID, t.Student_ID)) AS subquery;", connection))
                {
                    command.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Display the data on chart3
                    if (dt.Rows.Count > 0)
                    {
                        double totalStudents = Convert.ToDouble(dt.Rows[0]["Total_Students"] != DBNull.Value ? dt.Rows[0]["Total_Students"].ToString() : "0");
                        double averagePercentagePaid = Convert.ToDouble(dt.Rows[0]["Average_PercentagePaid"] != DBNull.Value ? dt.Rows[0]["Average_PercentagePaid"].ToString() : "0");
                        double paidstud = Convert.ToDouble(dt.Rows[0]["Paid_Students"] != DBNull.Value ? dt.Rows[0]["Paid_Students"].ToString() : "0");

                        lblPaidStud.Text = paidstud.ToString();
                        // Add data points to the chart
                        chart3.Series["Penalty"].Points.AddXY(averagePercentagePaid + "%", averagePercentagePaid);
                        chart3.Series["Penalty"].Points.AddXY("", totalStudents);
                    }

                    // Customize Chart
                    chart3.ChartAreas[0].AxisX.Title = "Metrics";
                    chart3.ChartAreas[0].AxisY.Title = "Values";

                    // Customize font
                    chart3.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);
                    chart3.ChartAreas[0].AxisY.LabelStyle.Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);
                    chart3.Legends[0].Font = new System.Drawing.Font("Poppins", 10, System.Drawing.FontStyle.Bold);

                    // You can customize the appearance of the series if needed
                    chart3.Series["Penalty"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
                }
            }

        }
    }
}
