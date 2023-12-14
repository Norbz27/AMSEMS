using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public String id;
        public static String id2 { get; set; }
        static FormSAONavigation form;
        public formDashboard(String id)
        {
            InitializeComponent();

            this.id = id;
            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray; // Set the color of major grid lines on the X-axis
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray; // Set the color of major grid lines on the Y-axis

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray; // Set the color of minor grid lines on the X-axis
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray; // Set the color of minor grid lines on the Y-axis
        }

        public static void setForm(FormSAONavigation form1)
        {
            form = form1;
        }
        public void loadData(String id)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_sao_accounts where Unique_ID = '" + id + "'", cn);
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

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();

                // Modify the SQL query to include a condition for the current year
                using (SqlCommand command = new SqlCommand(@"SELECT e.Event_ID, e.Event_Name, COUNT(a.Student_ID) AS AttendanceCount
                                                            FROM tbl_events e 
                                                            LEFT JOIN tbl_attendance a ON e.Event_ID = a.Event_ID 
                                                            WHERE YEAR(e.Start_Date) = @Year
                                                            GROUP BY e.Event_ID, e.Event_Name", cn))
                {
                    command.Parameters.AddWithValue("@Year", currentYear);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    await Task.Run(() => adapter.Fill(dt));

                    chart1.DataSource = dt;
                    chart1.Series["Attendees"].XValueMember = "Event_Name";
                    chart1.Series["Attendees"].YValueMembers = "AttendanceCount";
                }
            }

            // Step 5: Customize Chart
            chart1.ChartAreas[0].AxisX.Title = "Events";
            chart1.ChartAreas[0].AxisY.Title = "Attendance Count";
        }

        public async Task displayAnnouncementAsync()
        {
            pnAnnouncements.Controls.Clear();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT TOP 5 Announcement_ID, Announcement_Title, Announcement_Description, Date_Time, Announce_By FROM tbl_Announcement ORDER BY Date_Time DESC";

                using (SqlCommand cm = new SqlCommand(query, cn))
                using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                {
                    int labelCount = 0;
                    while (await dr.ReadAsync())
                    {
                        string id = dr["Announcement_ID"].ToString();
                        string title = dr["Announcement_Title"].ToString();
                        string description = dr["Announcement_Description"].ToString();
                        string announceby = dr["Announce_By"].ToString();
                        DateTime datetime = DateTime.Parse(dr["Date_Time"].ToString());
                        string formattedDate = datetime.ToString("dddd, MMMM d, yyyy, h:mm tt");

                        announceApperance(id, title, description, formattedDate, announceby);

                        labelCount++;
                    }
                }
            }
        }
        public void announceApperance(string id, string title, string description, string dateTime, string announceby)
        {
            System.Windows.Forms.Panel panel12 = new System.Windows.Forms.Panel();
            KryptonGroupBox kryptonGroupBox6 = new KryptonGroupBox();
            KryptonLabel kryptonLabel3 = new KryptonLabel();
            KryptonLabel kryptonLabel11 = new KryptonLabel();
            KryptonLabel kryptonLabel10 = new KryptonLabel();

            kryptonGroupBox6.CaptionVisible = false;
            kryptonGroupBox6.CausesValidation = false;
            kryptonGroupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonGroupBox6.Location = new System.Drawing.Point(0, 0);
            kryptonGroupBox6.Name = "kryptonGroupBox6";

            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel3);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel11);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel10);
            kryptonGroupBox6.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            kryptonGroupBox6.Size = new System.Drawing.Size(711, 99);
            kryptonGroupBox6.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox6.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox6.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox6.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            kryptonGroupBox6.StateCommon.Border.Rounding = 10;
            kryptonGroupBox6.StateCommon.Border.Width = 1;
            kryptonGroupBox6.TabIndex = 4;

            kryptonLabel3.AutoSize = false;
            kryptonLabel3.Cursor = System.Windows.Forms.Cursors.Default;
            kryptonLabel3.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonLabel3.Location = new System.Drawing.Point(15, 50);
            kryptonLabel3.Name = "kryptonLabel3";
            kryptonLabel3.Size = new System.Drawing.Size(673, 35);
            kryptonLabel3.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel3.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            kryptonLabel3.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            kryptonLabel3.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel3.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel3.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel3.StateCommon.ShortText.Color1 = System.Drawing.Color.Gray;
            kryptonLabel3.StateCommon.ShortText.Color2 = System.Drawing.Color.Gray;
            kryptonLabel3.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            kryptonLabel3.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel3.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel3.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel3.TabIndex = 8;
            kryptonLabel3.Values.Text = description;

            kryptonLabel11.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonLabel11.Location = new System.Drawing.Point(15, 29);
            kryptonLabel11.Name = "kryptonLabel11";
            kryptonLabel11.Size = new System.Drawing.Size(673, 21);
            kryptonLabel11.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            kryptonLabel11.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            kryptonLabel11.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Bold);
            kryptonLabel11.TabIndex = 7;
            kryptonLabel11.Values.Text = title;

            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Default;
            kryptonLabel10.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonLabel10.Location = new System.Drawing.Point(15, 10);
            kryptonLabel10.Name = "kryptonLabel10";
            kryptonLabel10.Size = new System.Drawing.Size(673, 19);
            kryptonLabel10.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            kryptonLabel10.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            kryptonLabel10.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.Color1 = System.Drawing.Color.Gray;
            kryptonLabel10.StateCommon.ShortText.Color2 = System.Drawing.Color.Gray;
            kryptonLabel10.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            kryptonLabel10.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.TabIndex = 6;
            kryptonLabel10.Values.Text = dateTime + " by " + announceby;

            panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            panel12.Dock = System.Windows.Forms.DockStyle.Top;
            panel12.Location = new System.Drawing.Point(0, 0);
            panel12.Name = "panel12";
            panel12.Cursor = System.Windows.Forms.Cursors.Hand;
            panel12.Size = new System.Drawing.Size(234, 10);
            panel12.TabIndex = 19;

            pnAnnouncements.Controls.Add(kryptonGroupBox6);
            pnAnnouncements.Controls.Add(panel12);
        }

        private async void formDashboard_Load(object sender, EventArgs e)
        {
            if (id.Equals(String.Empty))
                loadData(id2);
            else
                loadData(id);

            await displayAnnouncementAsync();
            await displayChartAsync();
            form.loadData();
        }

        private void formDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
