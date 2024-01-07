using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formDashboard : Form
    {
        SqlCommand cm;
        SqlDataReader dr;
        static FormGuidanceNavigation form;
        //string acadSchYeear, acadTerSem, acadShsSem;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        private CancellationTokenSource cancellationTokenSource;
        string acadSchYeear, acadTerSem, acadShsSem;
        public formDashboard()
        {
            InitializeComponent();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            cancellationTokenSource = new CancellationTokenSource();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";
                query = "SELECT Quarter_ID AS ID, Description FROM tbl_Quarter WHERE Status = 1";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadShsSem = dr["ID"].ToString();
                        }
                    }
                }

                query = "SELECT Semester_ID AS ID, Description FROM tbl_Semester WHERE Status = 1";

                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadTerSem = dr["ID"].ToString();
                        }
                    }
                }

                query = "SELECT Acad_ID FROM tbl_acad WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            acadSchYeear = dr["Acad_ID"].ToString();
                        }
                    }
                }
            }
        }
        public static void getForm(FormGuidanceNavigation form1)
        {
            form = form1;
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            loadData();
            displayPendingCount();
            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This method runs on the UI thread
            // Update the UI or perform other tasks after the background work completes
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {

            }
            else
            {
                this.Cursor = Cursors.Default;
            }
        }
        public void loadData()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_guidance_accounts where Unique_ID = '" + FormGuidanceNavigation.id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
            }
        }
        public async void displayPendingCount()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();
                string query = "";
                query = @"SELECT 
						COUNT(*) AS countStudTer
                        FROM 
                            tbl_consultation_record cr
                        	LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                            LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
                        WHERE 
                            Acad_Level = '10001'
                            AND cl.School_Year = @schyear
                            AND cl.Semester = @sem
                            AND cr.Status = 'Pending'";
                using (SqlCommand cm =  new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@schyear", acadSchYeear);
                    cm.Parameters.AddWithValue("@sem", acadTerSem);
                    using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                    {
                        if (dr.Read())
                        {
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            string tercount = dr["countStudTer"].ToString();
                            lblTerPending.Text = tercount + " Pending";
                        }
                    }
                }

                query = @"SELECT 
						COUNT(*) AS countStudSHS
                        FROM 
                            tbl_consultation_record cr
                        	LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                            LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
                        WHERE 
                            Acad_Level = '10002'
                            AND cl.School_Year = @schyear
                            AND cl.Semester = @sem
                            AND cr.Status = 'Pending'";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@schyear", acadSchYeear);
                    cm.Parameters.AddWithValue("@sem", acadShsSem);
                    using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                    {
                        if (dr.Read())
                        {
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            string shscount = dr["countStudSHS"].ToString();

                            lblShsPending.Text = shscount + " Pending";
                        }
                    }
                }
            }
        }
        private async void DisplayConsultedRecordsChart()
        {

            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    await cn.OpenAsync();
                    string query = "";
                    query = @"
                SELECT
                    MONTH(Date) AS Month,
                    COUNT(CASE WHEN Status = 'Done' THEN 1 ELSE NULL END) AS recordCount
                FROM tbl_consultation_record cr
				LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code 
                WHERE Status = 'Done' AND School_Year = @schyear AND Semester = @sem
                GROUP BY MONTH(Date)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@schyear", acadSchYeear);
                        cmd.Parameters.AddWithValue("@sem", acadTerSem);
                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                int month = Convert.ToInt32(dr["Month"]);
                                int recordCount = Convert.ToInt32(dr["recordCount"]);

                                // Add data points to the series
                                chart1.Series["monthlyconsulted"].Points.AddXY(GetMonthName(month), recordCount);
                            }


                            chart1.ChartAreas[0].AxisX.Title = "Month";
                            chart1.ChartAreas[0].AxisY.Title = "Record Count";
                        }
                    }
                    query = @"
                SELECT
                    MONTH(Date) AS Month,
                    COUNT(CASE WHEN Status = 'Done' THEN 1 ELSE NULL END) AS recordCount
                FROM tbl_consultation_record cr
				LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code 
                WHERE Status = 'Done' AND School_Year = @schyear AND Semester = @sem
                GROUP BY MONTH(Date)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@schyear", acadSchYeear);
                        cmd.Parameters.AddWithValue("@sem", acadShsSem);
                        using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                        {
                            while (dr.Read())
                            {
                                if (cancellationTokenSource.Token.IsCancellationRequested)
                                {
                                    return;
                                }
                                int month = Convert.ToInt32(dr["Month"]);
                                int recordCount = Convert.ToInt32(dr["recordCount"]);

                                // Add data points to the series
                                chart1.Series["monthlyconsulted"].Points.AddXY(GetMonthName(month), recordCount);
                            }


                            chart1.ChartAreas[0].AxisX.Title = "Month";
                            chart1.ChartAreas[0].AxisY.Title = "Record Count";
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private string GetMonthName(int month)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
        }
        public async void displayLatestList()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                await cn.OpenAsync();
                string query = @"SELECT TOP 10
                                cr.Consultation_ID,
                                UPPER(s.Firstname + ' ' + s.Lastname) AS Name,
                                sub.Course_Description AS subdes
                            FROM 
                                tbl_consultation_record cr
                                LEFT JOIN tbl_student_accounts s ON cr.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
                                LEFT JOIN tbl_class_list cl ON cr.Class_Code = cl.CLass_Code
                                LEFT JOIN tbl_subjects sub ON cl.Course_Code = sub.Course_code
	                            LEFT JOIN tbl_subject_attendance sa ON cr.Class_Code = sa.Class_Code
                            WHERE 
                                s.Status = 1
                                AND cr.Status = 'Pending'
                            GROUP BY 
                                s.ID, s.Lastname, s.Firstname, s.Middlename, sec.Description, sub.Course_Description,cr.Status, cr.Absences, cr.Consultation_ID, cr.Class_Code
                            ORDER BY
	                            cr.Consultation_ID DESC";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = await cmd.ExecuteReaderAsync())
                    {
                        while (dr.Read())
                        {
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            dgvRecord.Rows.Add(
                                dr["Name"].ToString(),
                                dr["subdes"].ToString()
                            );
                        }
                        dr.Close();
                    }
                }
                cn.Close();
            }
        }
        private void formDashboard_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
            DisplayConsultedRecordsChart();
            displayLatestList();
        }

        private void formDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }

        private void btnViewShs_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForm_Guidance.formAbsReport("SHS"));
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }
        public void controls()
        {
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

            form.btnRep.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnRep.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnRep.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            form.btnRep.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            form.btnRep.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }

        private void btnViewTer_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForm_Guidance.formAbsReport("Tertiary"));
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
            controls();
        }

        private void kryptonLabel2_Click(object sender, EventArgs e)
        {
            controls2();
        }
        public void controls2()
        {
            form.OpenChildForm(new SubForm_Guidance.formAbsReport("SHS"));
            form.btnRep.Focus();
            form.isCollapsed = true;
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
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

            form.btnRep.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnRep.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnRep.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            form.btnRep.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            form.btnRep.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }
    }
}
