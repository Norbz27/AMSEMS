using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        static FormGuidanceNavigation form;
        //string acadSchYeear, acadTerSem, acadShsSem;
        private BackgroundWorker backgroundWorker = new BackgroundWorker();
        public formDashboard()
        {
            InitializeComponent();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;

            //using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            //{
            //    string query = "SELECT * FROM tbl_acad";
            //    using (SqlCommand cmd = new SqlCommand(query, cn))
            //    {
            //        using (SqlDataReader dr = cmd.ExecuteReader())
            //        {
            //            if (dr.Read())
            //            {
            //                acadSchYeear = dr["Academic_Year_Start"].ToString() + "-" + dr["Academic_Year_End"].ToString();
            //                acadShsSem = dr["Ter_Academic_Sem"].ToString();
            //                acadTerSem = dr["SHS_Academic_Sem"].ToString();
            //            }
            //        }
            //    }
            //}
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
            using (cn = new SqlConnection(SQL_Connection.connection))
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
        public void displayPendingCount()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = @"SELECT
                COUNT(CASE WHEN sec.AcadLevel_ID = '10001' THEN 1 END) AS countStudTer,
                COUNT(CASE WHEN sec.AcadLevel_ID = '10002' THEN 1 END) AS countStudSHS
            FROM tbl_consultation_record cr
            LEFT JOIN tbl_student_accounts sa ON cr.Student_ID = sa.ID
            LEFT JOIN tbl_Section sec ON sa.Section = sec.Section_ID
            WHERE cr.Status = 'Pending';";
                using (SqlCommand cm =  new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string tercount = dr["countStudTer"].ToString();
                            string shscount = dr["countStudSHS"].ToString();

                            lblTerPending.Text = tercount + " Pending";
                            lblShsPending.Text = shscount + " Pending";
                        }
                    }
                }
            }
        }
        private void DisplayConsultedRecordsChart()
        {

            chart1.ChartAreas["ChartArea1"].AxisX.MajorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MajorGrid.LineColor = Color.LightGray;

            chart1.ChartAreas["ChartArea1"].AxisX.MinorGrid.LineColor = Color.LightGray;
            chart1.ChartAreas["ChartArea1"].AxisY.MinorGrid.LineColor = Color.LightGray;

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();

                    string query = @"
                SELECT
                    MONTH(Date) AS Month,
                    COUNT(CASE WHEN Status = 'Done' THEN 1 ELSE NULL END) AS recordCount
                FROM tbl_consultation_record
                WHERE Status = 'Done'
                GROUP BY MONTH(Date)";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
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
        public void displayLatestList()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
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
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
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
    }
}
