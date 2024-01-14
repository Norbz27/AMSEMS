using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Threading;
using System.Windows.Forms;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formDashboard : Form
    {
        SqlCommand cm;
        SqlDataReader dr;

        static FormAcadHeadNavigatio form;
        private BackgroundWorker dataLoader;
        private CancellationTokenSource cancellationTokenSource;
        public formDashboard()
        {

            InitializeComponent();
            cancellationTokenSource = new CancellationTokenSource();
            dataLoader = new BackgroundWorker();
            dataLoader.DoWork += DataLoader_DoWork;
            dataLoader.RunWorkerCompleted += DataLoader_RunWorkerCompleted;
            dataLoader.WorkerSupportsCancellation = true;
        }
        public static void setForm(FormAcadHeadNavigatio form1)
        {
            form = form1;
        }
        private void DataLoader_DoWork(object sender, DoWorkEventArgs e)
        {
            // This is where you put your time-consuming data loading code

            loadData(FormAcadHeadNavigatio.id);

            DisplayData();
            displayChart();
            displayAccounts();

            System.Threading.Thread.Sleep(2000);

            // Any other work you want to do in the background
        }

        private void DataLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
            }
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            dataLoader.RunWorkerAsync();
        }

        // Declare a class-level list to store data
        private List<Tuple<string, int>> chartData = new List<Tuple<string, int>>();

        // Update data in a separate method
        private void UpdateChartData(string program, int studentCount)
        {
            chartData.Add(Tuple.Create(program, studentCount));
        }

        // Update chart in the UI thread
        private void UpdateChartUI()
        {
            chart1.Series[0].Points.Clear();

            foreach (var dataPoint in chartData)
            {
                chart1.Series[0].Points.AddXY(dataPoint.Item1, dataPoint.Item2);
            }

            chartData.Clear();
        }

        // Modify displayChart to call the update methods
        public void displayChart()
        {
            if (this.InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { displayChart(); });
                return;
            }

            //chart1.Invalidate();
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                chart1.Series["s1"].IsValueShownAsLabel = true;
                string query = @"SELECT d.Description as Dep, COUNT(*) + COALESCE((SELECT COUNT(*) FROM tbl_subjects WHERE Department_ID IS NULL), 0) AS StudentCount FROM tbl_subjects as sa inner join tbl_Departments as d on sa.Department_ID = d.Department_ID GROUP BY d.Description";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    // Execute the query and retrieve data
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (cancellationTokenSource.Token.IsCancellationRequested)
                            {
                                return;
                            }
                            string program = reader["Dep"].ToString();
                            int studentCount = Convert.ToInt32(reader["StudentCount"]);

                            // Update data in a separate method
                            UpdateChartData(program, studentCount);
                        }
                    }
                }
            }

            // Update the chart in the UI thread
            UpdateChartUI();
        }

        public void loadData(String id)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("select Firstname, Lastname from tbl_acadHead_accounts where Unique_ID = '" + id + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                    dr.Close();
                    cn.Close();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void displayAccounts()
        {
            // Invoke the UI thread to update the DataGridView
            this.Invoke((MethodInvoker)delegate
            {
                dgvAccounts.Rows.Clear();
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("SELECT TOP 10 Course_code, Course_Description FROM tbl_subjects ORDER BY 1 DESC", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        if (cancellationTokenSource.Token.IsCancellationRequested)
                        {
                            return;
                        }
                        dgvAccounts.Rows.Add(
                            dr["Course_Code"].ToString(),
                            dr["Course_Description"].ToString()
                        );
                    }
                    dr.Close();
                    cn.Close();
                }
            });
        }
        public void DisplayData()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                //Ter
                cn.Open();
                cm = new SqlCommand("select count(*) as countActive from tbl_subjects where Status = 1 and Academic_Level = 10001", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActTerSub.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_subjects where Status = 2 and Academic_Level = 10001", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacTerSub.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();

                //SHS
                cm = new SqlCommand("select count(*) as countActive from tbl_subjects where Status = 1 and Academic_Level = 10002", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblActShsSub.Text = dr["countActive"].ToString() + " Active";
                dr.Close();

                cm = new SqlCommand("select count(*) as countActive from tbl_subjects where Status = 2 and Academic_Level = 10002", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblInacShsSub.Text = dr["countActive"].ToString() + " Inactive";
                dr.Close();
                cn.Close();
            }
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

            form.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            form.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            form.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            form.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            form.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            form.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            form.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            form.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnViewStud_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForms_AcadHead.formSubjects());
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            controls();
        }
        private void btnView_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForms_AcadHead.formSubjects());
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            controls();
        }

        private void btnViewTeachers_Click(object sender, EventArgs e)
        {
            form.OpenChildForm(new SubForms_AcadHead.formSubjects());
            form.kryptonSplitContainer1.Panel2Collapsed = true;
            controls();
        }

        private void formDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Check if the BackgroundWorker is running and stop it if needed.
            if (dataLoader.IsBusy)
            {
                dataLoader.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }
    }
}
