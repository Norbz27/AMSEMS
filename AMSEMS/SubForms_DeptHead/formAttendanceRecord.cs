using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceRecord : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        formConfigFee formConfigFee;

        string event_id, Attendancedate, sec;
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        private CancellationTokenSource cancellationTokenSource;

        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public formAttendanceRecord()
        {
            InitializeComponent();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
            dgvRecord.Columns["penalty_total"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;

            cn = new SqlConnection(SQL_Connection.connection);
            formConfigFee = new formConfigFee();
        }
        private void CenterPictureBoxInDataGridView()
        {
            int x = (dgvRecord.Width - pictureBox1.Width) / 2;
            int y = (dgvRecord.Height - pictureBox1.Height) / 2;

            // Set the location of the PictureBox
            pictureBox1.Location = new Point(x, y);
        }

        private async void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            await displayTable();
            Thread.Sleep(2000);
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
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
            }
        }
        public void displayFilter()
        {
            if (cbEvents.InvokeRequired)
            {
                cbEvents.Invoke(new Action(() => displayFilter()));
                return;
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbEvents.Items.Clear();
                    cbSection.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand(@"SELECT Event_ID, Event_Name FROM tbl_events WHERE Attendance = 'True' AND (Exclusive = 'All Students' OR Exclusive = @Department OR Exclusive = 'Specific Students' OR CHARINDEX(@Department, Selected_Departments) > 0) ORDER BY Start_Date;", cn);
                    cm.Parameters.AddWithValue("@Department", FormDeptHeadNavigation.depdes);
                    dr = cm.ExecuteReader();

                    while (dr.Read())
                    {
                        cbEvents.Items.Add(dr["Event_Name"].ToString());
                    }

                    dr.Close();

                    if (cbEvents.Items.Count > 0)
                    {
                        cbEvents.SelectedIndex = 0;
                    }

                    cm = new SqlCommand("Select Description from tbl_section", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbSection.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();

                    if (cbSection.Items.Count > 0)
                    {
                        cbSection.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void displayFees()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                string eventid = null;
                cn.Open();
                cm = new SqlCommand("Select Event_ID from tbl_events where Event_Name = @Eventname", cn);
                cm.Parameters.AddWithValue("@Eventname", cbEvents.Text);
                using (SqlDataReader reader = cm.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        eventid = reader["Event_ID"].ToString();
                    }
                }

                cm = new SqlCommand("Select Penalty_AM, Penalty_PM from tbl_attendance where Event_ID = @eventid AND FORMAT(Date_Time, 'yyyy-MM-dd') = @date", cn);
                cm.Parameters.AddWithValue("@eventid", eventid);
                cm.Parameters.AddWithValue("@date", Dt.Value.ToString("yyyy-MM-dd"));
                using (dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        // Check for DBNull.Value and display "00.00" in that case
                        string penaltyAM = dr["Penalty_AM"] != DBNull.Value ? Convert.ToDecimal(dr["Penalty_AM"]).ToString("0.00") : "00.00";
                        string penaltyPM = dr["Penalty_PM"] != DBNull.Value ? Convert.ToDecimal(dr["Penalty_PM"]).ToString("0.00") : "00.00";

                        lblAmPenaltyFee.Text = "₱ " + penaltyAM;
                        lblPmPenaltyFee.Text = "₱ " + penaltyPM;
                    }
                    else
                    {
                        lblAmPenaltyFee.Text = "₱ 00.00";
                        lblPmPenaltyFee.Text = "₱ 00.00";
                    }
                }
            }
        }
        public async Task displayTable()
        {
            cancellationTokenSource = new CancellationTokenSource();
            if (dgvRecord.InvokeRequired)
            {
                dgvRecord.Invoke(new Action(() => displayTable()));
                return;
            }
            if (cbSection.InvokeRequired)
            {
                cbSection.Invoke(new Action(() => displayTable()));
                return;
            }
            displayFees();
            dgvRecord.Rows.Clear();
            ptbLoading.Visible = true;
            await Task.Delay(1000);
            try
            {
                await Task.Run(() =>
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string query = "";
                        double total = 0;

                        // Check if the event is for Specific Students
                        if (IsEventForSpecificStudents(event_id))
                        {
                            query = @"SELECT
                                    e.Event_ID,
                                    s.ID AS id,
                                    UPPER(s.Firstname) AS fname,
                                    UPPER(s.Middlename) AS mname,
                                    UPPER(s.Lastname) AS lname,
                                    sec.Description AS secdes,
                                    ISNULL(FORMAT(att.Date_Time, 'yyyy-MM-dd'), '-------') AS Date_Time,
                                    COALESCE(att.Penalty_AM, 0) AS Penalty_AM,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), @AM_Pen) AS AM_IN_Penalty,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), @AM_Pen) AS AM_OUT_Penalty,
                                    COALESCE(att.Penalty_PM, 0) AS Penalty_PM,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), @PM_Pen) AS PM_IN_Penalty,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), @PM_Pen) AS PM_OUT_Penalty,
                                    ISNULL(UPPER(teach.Lastname), '-------') AS teachlname
                                FROM
                                    tbl_events e
                                LEFT JOIN
                                    tbl_student_accounts s ON CHARINDEX(s.FirstName + ' ' + s.LastName, e.Specific_Students) > 0 OR CHARINDEX(s.LastName + ' ' + s.FirstName, e.Specific_Students) > 0
                                LEFT JOIN
                                    tbl_attendance AS att ON s.ID = att.Student_ID AND e.Event_ID = att.Event_ID AND FORMAT(att.Date_Time, 'yyyy-MM-dd') = @Date
                                LEFT JOIN
                                    tbl_departments d ON s.Department = d.Department_ID
                                LEFT JOIN
                                    tbl_Section AS sec ON s.Section = sec.Section_ID
                                LEFT JOIN
                                    tbl_teacher_accounts AS teach ON att.Checker = teach.ID
                                WHERE
                                    e.Event_ID = @EventID
                                    AND e.Exclusive = 'Specific Students'
                                    AND s.ID IS NOT NULL
                                    AND s.Department = @Dep
                                    AND sec.Description = @sec
                                    AND s.Status = 1
                                ORDER BY
                                    s.Lastname, att.Date_Time;";
                        }
                        else if (IsEventForSelectedDep(event_id))
                        {
                            query = @"SELECT
                                    e.Event_ID,
                                    s.ID AS id,
                                    UPPER(s.Firstname) AS fname,
                                    UPPER(s.Middlename) AS mname,
                                    UPPER(s.Lastname) AS lname,
                                    sec.Description AS secdes,
                                    ISNULL(FORMAT(att.Date_Time, 'yyyy-MM-dd'), '-------') AS Date_Time,
                                    COALESCE(att.Penalty_AM, 0) AS Penalty_AM,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), @AM_Pen) AS AM_IN_Penalty,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), @AM_Pen) AS AM_OUT_Penalty,
                                    COALESCE(att.Penalty_PM, 0) AS Penalty_PM,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), @PM_Pen) AS PM_IN_Penalty,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), @PM_Pen) AS PM_OUT_Penalty,
                                    ISNULL(UPPER(teach.Lastname), '-------') AS teachlname
                                FROM
                                    tbl_events e
                                LEFT JOIN
                                    tbl_student_accounts s ON s.ID IS NOT NULL
                                LEFT JOIN
                                    tbl_departments dep ON s.Department = dep.Department_ID
                                LEFT JOIN
                                    tbl_attendance AS att ON s.ID = att.Student_ID AND e.Event_ID = att.Event_ID AND FORMAT(att.Date_Time, 'yyyy-MM-dd') = @Date
                                LEFT JOIN
                                    tbl_Section sec ON s.Section = sec.Section_ID
                                LEFT JOIN
                                    tbl_teacher_accounts teach ON att.Checker = teach.ID
                                WHERE
                                    e.Event_ID = @EventID
                                    AND e.Exclusive = 'Selected Departments'
                                    AND CHARINDEX(CONVERT(VARCHAR, dep.Description), e.Selected_Departments) > 0
	                                AND s.Department = @Dep
                                    AND sec.Description = @sec
                                    AND s.Status = 1
                                ORDER BY
                                    s.Lastname, att.Date_Time;";
                        }
                        else
                        {
                            query = @"SELECT stud.ID AS id, 
                                    UPPER(stud.Firstname) AS fname,
                                    UPPER(stud.Middlename) AS mname,
                                    UPPER(stud.Lastname) AS lname,
                                    sec.Description AS secdes,
                                    FORMAT(att.Date_Time, 'yyyy-MM-dd') AS Date_Time,
                                    COALESCE(att.Penalty_AM, 0) AS Penalty_AM,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), @AM_Pen) AS AM_IN_Penalty,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), @AM_Pen) AS AM_OUT_Penalty,
                                    COALESCE(att.Penalty_PM, 0) AS Penalty_PM,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), @PM_Pen) AS PM_IN_Penalty,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), @PM_Pen) AS PM_OUT_Penalty,
                                    ISNULL(UPPER(teach.Lastname), '-------') AS teachlname
                                    FROM tbl_student_accounts AS stud
                                    LEFT JOIN tbl_attendance AS att ON stud.ID = att.Student_ID AND att.Event_ID = @EventID AND FORMAT(att.Date_Time, 'yyyy-MM-dd') = @Date
                                    LEFT JOIN tbl_Section AS sec ON stud.Section = sec.Section_ID
                                    LEFT JOIN tbl_teacher_accounts AS teach ON att.Checker = teach.ID 
                                    WHERE stud.Department = @Dep AND sec.Description = @sec AND stud.Status = 1 ORDER BY stud.Lastname";
                        }

                        using (SqlCommand cmd = new SqlCommand(query, cn))
                        {
                            string modifiedStringAM = lblAmPenaltyFee.Text.Replace("₱ ", "");
                            string modifiedStringPM = lblPmPenaltyFee.Text.Replace("₱ ", "");
                            cmd.Parameters.AddWithValue("@EventID", event_id);
                            cmd.Parameters.AddWithValue("@Date", Attendancedate);
                            cmd.Parameters.AddWithValue("@AM_Pen", modifiedStringAM);
                            cmd.Parameters.AddWithValue("@PM_Pen", modifiedStringPM);
                            cmd.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                            cmd.Parameters.AddWithValue("@sec", sec);

                            using (SqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    if (cancellationTokenSource.Token.IsCancellationRequested)
                                    {
                                        return;
                                    }
                                    if (dgvRecord.InvokeRequired)
                                    {
                                        dgvRecord.Invoke(new Action(() =>
                                        {
                                            int rowIndex = dgvRecord.Rows.Add(false);

                                            string name = dr["fname"].ToString() + " " + dr["mname"].ToString() + " " + dr["lname"].ToString();

                                            double totalfee = 0;

                                            if (double.TryParse(dr["AM_IN_Penalty"].ToString(), out double amInPenalty) && !string.IsNullOrEmpty(dr["AM_IN"].ToString()))
                                                totalfee += amInPenalty;

                                            if (double.TryParse(dr["AM_OUT_Penalty"].ToString(), out double amOutPenalty) && !string.IsNullOrEmpty(dr["AM_OUT"].ToString()))
                                                totalfee += amOutPenalty;

                                            if (double.TryParse(dr["PM_IN_Penalty"].ToString(), out double pmInPenalty) && !string.IsNullOrEmpty(dr["PM_IN"].ToString()))
                                                totalfee += pmInPenalty;

                                            if (double.TryParse(dr["PM_OUT_Penalty"].ToString(), out double pmOutPenalty) && !string.IsNullOrEmpty(dr["PM_OUT"].ToString()))
                                                totalfee += pmOutPenalty;

                                            string formattedTotalFee = totalfee.ToString("F2");

                                            string formattedDate = Dt.Value.ToString("MM-dd-yyyy");

                                            dgvRecord.Rows[rowIndex].Cells["ID"].Value = dr["id"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["name"].Value = name;
                                            dgvRecord.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["event_date"].Value = formattedDate;
                                            dgvRecord.Rows[rowIndex].Cells["penalty_am"].Value = lblAmPenaltyFee.Text;
                                            dgvRecord.Rows[rowIndex].Cells["am_login"].Value = dr["AM_IN"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["am_logout"].Value = dr["AM_OUT"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["penalty_pm"].Value = lblPmPenaltyFee.Text;
                                            dgvRecord.Rows[rowIndex].Cells["pm_login"].Value = dr["PM_IN"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["pm_logout"].Value = dr["PM_OUT"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["checker"].Value = dr["teachlname"].ToString();
                                            dgvRecord.Rows[rowIndex].Cells["penalty_total"].Value = "₱ " + formattedTotalFee;

                                            rowIndex++;
                                            total += totalfee;
                                        }));
                                    }
                                    else
                                    {
                                        int rowIndex = dgvRecord.Rows.Add(false);

                                        string name = dr["fname"].ToString() + " " + dr["mname"].ToString() + " " + dr["lname"].ToString();

                                        double totalfee = 0;

                                        if (double.TryParse(dr["AM_IN_Penalty"].ToString(), out double amInPenalty) && !string.IsNullOrEmpty(dr["AM_IN"].ToString()))
                                            totalfee += amInPenalty;

                                        if (double.TryParse(dr["AM_OUT_Penalty"].ToString(), out double amOutPenalty) && !string.IsNullOrEmpty(dr["AM_OUT"].ToString()))
                                            totalfee += amOutPenalty;

                                        if (double.TryParse(dr["PM_IN_Penalty"].ToString(), out double pmInPenalty) && !string.IsNullOrEmpty(dr["PM_IN"].ToString()))
                                            totalfee += pmInPenalty;

                                        if (double.TryParse(dr["PM_OUT_Penalty"].ToString(), out double pmOutPenalty) && !string.IsNullOrEmpty(dr["PM_OUT"].ToString()))
                                            totalfee += pmOutPenalty;

                                        string formattedTotalFee = totalfee.ToString("F2");

                                        string formattedDate = Dt.Value.ToString("MM-dd-yyyy");

                                        dgvRecord.Rows[rowIndex].Cells["ID"].Value = dr["id"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["name"].Value = name;
                                        dgvRecord.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["event_date"].Value = formattedDate;
                                        dgvRecord.Rows[rowIndex].Cells["penalty_am"].Value = lblAmPenaltyFee.Text;
                                        dgvRecord.Rows[rowIndex].Cells["am_login"].Value = dr["AM_IN"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["am_logout"].Value = dr["AM_OUT"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["penalty_pm"].Value = lblPmPenaltyFee.Text;
                                        dgvRecord.Rows[rowIndex].Cells["pm_login"].Value = dr["PM_IN"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["pm_logout"].Value = dr["PM_OUT"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["checker"].Value = dr["teachlname"].ToString();
                                        dgvRecord.Rows[rowIndex].Cells["penalty_total"].Value = "₱ " + formattedTotalFee;

                                        rowIndex++;
                                        total += totalfee;
                                    }
                                    
                                }
                            }
                        }
                        lblTotalFees.Text = "₱ " + total.ToString("F2");
                    }
                }, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                // Handle cancellation if needed
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                saveStudentBalance();
                ptbLoading.Visible = false;
            }
        }

        private bool IsEventForSpecificStudents(string eventId)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Exclusive FROM tbl_events WHERE Event_ID = @EventID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    object result = cmd.ExecuteScalar();
                    return result != null && result.ToString() == "Specific Students";
                }
            }
        }
        private bool IsEventForSelectedDep(string eventId)
        {
            // Implement your logic to check if the event is for Specific Students
            // For example, check if the Exclusive column is 'Specific Students'
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Exclusive FROM tbl_events WHERE Event_ID = @EventID";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    object result = cmd.ExecuteScalar();
                    return result != null && result.ToString() == "Selected Departments";
                }
            }
        }

        private void formAttendanceRecord_Load(object sender, EventArgs e)
        {
            CenterPictureBoxInDataGridView();
            displayFilter();
        }

        private async void Dt_ValueChanged(object sender, EventArgs e)
        {
            Attendancedate = Dt.Value.ToString();
            dgvRecord.Rows.Clear();
            await DisplayTableWithCheck();
        }

        private async void cbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRecord.Rows.Clear();
            if (cbSection.InvokeRequired)
            {
                cbSection.Invoke(new Action(() => cbSection_SelectedIndexChanged(sender, e)));
            }
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT Event_ID, Event_Name, Start_Date, End_Date FROM tbl_events WHERE Event_Name = @EventName";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@EventName", cbEvents.Text);
                        SqlDataReader dr = cm.ExecuteReader();

                        if (dr.Read())
                        {
                            event_id = dr["Event_ID"].ToString();

                            // Reset MinDate and MaxDate to their default values
                            Dt.MinDate = DateTimePicker.MinimumDateTime;
                            Dt.MaxDate = DateTimePicker.MaximumDateTime;

                            // Ensure that the values are DateTime before assigning to MinDate and MaxDate
                            DateTime startDate = (DateTime)dr["Start_Date"];
                            DateTime endDate = (DateTime)dr["End_Date"];

                            // Optionally, check if the dates are valid before setting
                            if (startDate != DateTime.MinValue && endDate != DateTime.MinValue && startDate <= endDate)
                            {
                                Dt.Value = startDate;
                                Dt.MinDate = startDate;
                                Dt.MaxDate = endDate;
                            }

                            Attendancedate = Dt.Value.ToString();
                        }

                        dr.Close();
                    }
                }
                await DisplayTableWithCheck();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgvRecord.Rows.Clear();
            sec = cbSection.Text;
            await DisplayTableWithCheck();
        }
        private async Task DisplayTableWithCheck()
        {
            // Use SemaphoreSlim to ensure that only one displayTable operation is in progress at a time
            await semaphoreSlim.WaitAsync();
            try
            {
                // Call the method
                await displayTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                // Release the semaphore when the method is done
                semaphoreSlim.Release();
            }
        }
        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplyCBFilter(searchKeyword);
        }

        private void btnPenaltyFee_Click(object sender, EventArgs e)
        {
            formConfigFee.displayCBData(cbEvents.Text);
            formConfigFee.getForm(this);
            formConfigFee.ShowDialog();
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            displayFilter();
            await displayTable();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                ExportToPDF(dgvRecord, saveFileDialog.FileName);
                MessageBox.Show("Data exported to PDF successfully.", "Export to PDF", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Process.Start(saveFileDialog.FileName);
            }
        }
        private void ExportToPDF(DataGridView dataGridView, string filePath)
        {
            Document document = new Document(PageSize.LETTER.Rotate());
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            // Customizing the font and size
            iTextSharp.text.Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font headerFont1 = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13);
            iTextSharp.text.Font headerFont2 = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            iTextSharp.text.Font headerFont3 = FontFactory.GetFont(FontFactory.HELVETICA, 9);
            iTextSharp.text.Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9);

            // Add title "List of Students:"
            Paragraph titleParagraph = new Paragraph("Attendance Record", headerFont1);
            Paragraph titleParagraph2 = new Paragraph(cbEvents.Text, headerFont2);
            Paragraph titleParagraph3 = new Paragraph(Dt.Value.ToString(), headerFont3);
            titleParagraph.Alignment = Element.ALIGN_CENTER;
            titleParagraph2.Alignment = Element.ALIGN_CENTER;
            titleParagraph3.Alignment = Element.ALIGN_CENTER;
            document.Add(titleParagraph);
            document.Add(titleParagraph2);
            document.Add(titleParagraph3);

            // Customizing the table appearance
            PdfPTable pdfTable = new PdfPTable(dataGridView.Columns.Count);
            pdfTable.WidthPercentage = 100; // Table width as a percentage of page width
            pdfTable.SpacingBefore = 10f; // Add space before the table
            pdfTable.DefaultCell.Padding = 3; // Cell padding

            float[] columnWidths = new float[dataGridView.Columns.Count];
            columnWidths[0] = 50; 
            columnWidths[1] = 90;
            columnWidths[2] = 40;
            columnWidths[3] = 40; 
            columnWidths[4] = 50; 
            columnWidths[5] = 40; 
            columnWidths[6] = 40; 
            columnWidths[7] = 50;
            columnWidths[8] = 40;
            columnWidths[9] = 40;
            columnWidths[10] = 55;
            pdfTable.SetWidths(columnWidths);

            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, headerFont));
                cell.BackgroundColor = new BaseColor(240, 240, 240); // Cell background color
                pdfTable.AddCell(cell);
            }

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    PdfPCell pdfCell = new PdfPCell(new Phrase(cell.Value.ToString(), cellFont));
                    pdfTable.AddCell(pdfCell);
                }
            }

            document.Add(pdfTable);
            document.Close();
        }


        private void ApplyCBFilter(string selectedIndex)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvRecord.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(selectedIndex, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break; // No need to check other cells in the row
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }

        private void formAttendanceRecord_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
            cancellationTokenSource?.Cancel();
        }

        private void formAttendanceRecord_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private void formAttendanceRecord_Resize(object sender, EventArgs e)
        {
            CenterPictureBoxInDataGridView();
        }
        private void ExecuteStoredProcedure()
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetTotalPenaltyFee", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@dep",FormDeptHeadNavigation.dep);
                        command.ExecuteNonQuery();

                        //MessageBox.Show("Stored procedure executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void saveStudentBalance()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    foreach (DataGridViewRow row in dgvRecord.Rows)
                    {
                        string studID = row.Cells["ID"].Value.ToString();
                        string balFee = row.Cells["penalty_total"].Value.ToString().Replace("₱ ", "");

                        double bal_fee = 0;

                        // Check if the modified value can be converted to a decimal
                        if (double.TryParse(balFee, out double cellValue))
                        {
                            bal_fee = cellValue;
                        }

                        // Check if bal_fee is greater than 0 before proceeding with insertion or update

                        cm = new SqlCommand("SELECT COUNT(*) FROM tbl_balance_fees WHERE Student_ID = @StudID AND Event_ID = @EventID AND FORMAT(Date, 'yyyy-MM-dd') = @Date", cn);
                        cm.Parameters.AddWithValue("@StudID", studID);
                        cm.Parameters.AddWithValue("@EventID", event_id);
                        cm.Parameters.AddWithValue("@Date", Dt.Value.ToString("yyyy-MM-dd"));

                        int existingRecords = (int)cm.ExecuteScalar();

                        if (existingRecords == 0)
                        {
                            if (bal_fee > 0)
                            {
                                cm = new SqlCommand("INSERT INTO tbl_balance_fees VALUES (@StudID, @EventID, @Date, @BalFee)", cn);
                                cm.Parameters.AddWithValue("@StudID", studID);
                                cm.Parameters.AddWithValue("@EventID", event_id);
                                cm.Parameters.AddWithValue("@Date", Dt.Value.ToString("yyyy-MM-dd"));
                                cm.Parameters.AddWithValue("@BalFee", bal_fee);
                                cm.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Update the existing record
                            cm = new SqlCommand("UPDATE tbl_balance_fees SET Balance_Fee = @BalFee WHERE Student_ID = @StudID AND Event_ID = @EventID AND FORMAT(Date, 'yyyy-MM-dd') = @Date", cn);
                            cm.Parameters.AddWithValue("@StudID", studID);
                            cm.Parameters.AddWithValue("@EventID", event_id);
                            cm.Parameters.AddWithValue("@Date", Dt.Value.ToString("yyyy-MM-dd"));
                            cm.Parameters.AddWithValue("@BalFee", bal_fee);
                            cm.ExecuteNonQuery();
                        }
                        
                    }
                    ExecuteStoredProcedure();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
