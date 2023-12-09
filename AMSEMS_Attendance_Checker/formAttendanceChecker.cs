using AMSEMS;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceChecker : KryptonForm
    {
        SQLite_Connection sQLite_Connection;

        string attendance_stat;
        string event_code;
        string teach_id;
        private string scannedRFIDData = "";
        public bool isCollapsed;
        public formAttendanceChecker()
        {
            InitializeComponent();
            this.splitContainer1.Panel2Collapsed = true;
            isCollapsed = true;

            sQLite_Connection = new SQLite_Connection();

            this.KeyPreview = true; // Ensure form captures keyboard input
            this.KeyPress += FormAttendanceChecker_KeyPress;
        }
        public void getTeachID(string id)
        {
            teach_id = id;
        }
        public void getAttendanceSettings(string att, string code)
        {
            attendance_stat = att;
            event_code = code;
        }
        public void SetAttendanceEnabled(bool enabled)
        {
            tbAttendance.Enabled = enabled;
            dgvStudents.Enabled = enabled;
        }
        private void FormAttendanceChecker_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the RFID reader input is received
            if (e.KeyChar == '\r') // Assuming the RFID input is terminated with Enter key
            {
                scannedRFIDData = tbAttendance.Text; // Store the RFID input in the variable
                e.Handled = true; // Prevent the Enter key from being added to tbSearch

                if (tbAttendance.Enabled == true)
                {
                    // Process the RFID input as needed (you can call a separate function here)
                    ProcessScannedData(scannedRFIDData);
                    displayAttendanceRecord();
                    displayStudentInfo();
                }
            }
        }
        public void clearStudInfo()
        {
            lblID.Text = "00000000";
            lblName.Text = "Name";
            lblDepartment.Text = "Department";
            lblSection.Text = "Section";
            lblAttDate.Text = "Date";
            lblAttTime.Text = "Time";
            ptbCheck.Image = Properties.Resources.check_64;
            ptbProfilePic.Image = Properties.Resources.Pulse_1s_200px;
        }
        public void displayStudentInfo()
        {
            DataTable studentInfo = sQLite_Connection.GetStudentByRFID(scannedRFIDData, event_code);

            if (studentInfo.Rows.Count > 0)
            {
                DataRow row = studentInfo.Rows[0]; // Assuming you want to display information for the first student

                string studentID = row["ID"].ToString();
                string studentName = row["Name"].ToString();
                string department = row["depdes"].ToString();
                string section = row["secdes"].ToString();

                if (row["pic"] is Image image)
                {
                    ptbProfilePic.Image = image; // Assuming ptbProfilePic is a PictureBox
                }

                DateTime dateTime = DateTime.Now;
                string time = dateTime.ToString("h:mm tt");
                string date = dateTime.ToString("dddd, MMMM d, yyyy");

                // Assuming you have labels named lblStudentID, lblStudentName, lblDepartment, lblSection, lblDate, and lblTime
                lblID.Text = studentID;
                lblName.Text = studentName;
                lblDepartment.Text = department;
                lblSection.Text = section;
                lblAttDate.Text = date;
                lblAttTime.Text = time;
                ptbCheck.Image = Properties.Resources.check_64;
            }
            else
            {
                lblID.Text = "";
                lblName.Text = "No Record Found";
                lblDepartment.Text = "";
                lblSection.Text = "";
                lblAttDate.Text = "";
                lblAttTime.Text = "";
                ptbCheck.Image = Properties.Resources.ex;
                ptbProfilePic.Image = Properties.Resources.empty_folder_9841555;
            }
        }

        public void displayAttendanceRecord()
        {
            dgvAttendance.Rows.Clear();
            tableLayoutPanel1.Controls.Clear();
            DateTime dateTimeNow = DateTime.Now;
            string formattedDate = dateTimeNow.ToString("M/d/yyyy");
            string period = dateTimeNow.Hour < 12 ? "AM" : "PM";
            try
            {
            DataTable attendance = sQLite_Connection.GetAttendanceRecord(event_code, period, attendance_stat, formattedDate);

            DataTable attendancePics = sQLite_Connection.GetAttendanceRecordDone(event_code, period, attendance_stat, formattedDate);

                if (attendancePics.Rows.Count != 0)
                {
                    int displayedCount = 0;
                    for (int i = attendancePics.Rows.Count - 1; i >= 1; i--)
                    {
                        DataRow row = attendancePics.Rows[i];
                        Image pic = null;
                        string studentName = row["Name"].ToString();

                        if (row["pic"] is Image image)
                        {
                            pic = image;
                        }

                        doneAttendanceApperance(studentName, pic);
                        displayedCount++;
                    }
                }

                foreach (DataRow row in attendance.Rows)
                {
                    // Create a new DataGridViewRow
                    DataGridViewRow newRow = new DataGridViewRow();

                    // Add cells to the row, including the "Profile_pic" cell
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["ID"] });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["Name"] });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["secdes"] });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["depdes"] });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["Date"] });
                    newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["Time"] });

                    // Add the row to the DataGridView
                    dgvAttendance.Rows.Add(newRow);
                }
            }
            catch (Exception ex)
            {
                // Log or print the exception message
                Console.WriteLine("Error fetching attendance records: " + ex.Message);
            }
        }
        private void ProcessScannedData(string data)
        {
            DateTime dateTimeNow = DateTime.Now;
            string period = dateTimeNow.Hour < 12 ? "AM" : "PM";
            if (period.Equals("AM"))
            {
                if (attendance_stat.Equals("IN"))
                    sQLite_Connection.GetStudentForAttendance(data, event_code, dateTimeNow.ToString(), dateTimeNow.ToString(), null, null, null, teach_id);
                else if (attendance_stat.Equals("OUT"))
                    sQLite_Connection.GetStudentForAttendance(data, event_code, dateTimeNow.ToString(), null, dateTimeNow.ToString(), null, null, teach_id);
            }
            else if (period.Equals("PM"))
            {
                if (attendance_stat.Equals("IN"))
                    sQLite_Connection.GetStudentForAttendance(data, event_code, dateTimeNow.ToString(), null, null, dateTimeNow.ToString(), null, teach_id);
                else if (attendance_stat.Equals("OUT"))
                    sQLite_Connection.GetStudentForAttendance(data, event_code, dateTimeNow.ToString(), null, null, null, dateTimeNow.ToString(), teach_id);
            }
            tbAttendance.Text = String.Empty;
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            CollapseForm();
        }

        public void CollapseForm()
        {
            if (isCollapsed)
            {
                this.splitContainer1.Panel2Collapsed = false;
                timer1.Stop();
                isCollapsed = false;
                tbSearch.Focus();
                ActiveControl = tbSearch;
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                timer1.Stop();
                isCollapsed = true;
                tbAttendance.Focus();
            }
        }
        public void displayStudents()
        {
            tbAttendance.Focus();
            dgvStudents.Rows.Clear();
            DataTable allStudents = sQLite_Connection.GetAllStudents(event_code);

            // Define the desired margin
            int cellMargin = 5; // Adjust the margin size as needed

            foreach (DataRow row in allStudents.Rows)
            {
                // Create a new DataGridViewRow
                DataGridViewRow newRow = new DataGridViewRow();

                if (row["Profile_pic"] is Image image)
                {
                    DataGridViewImageCell imageCell = new DataGridViewImageCell();
                    imageCell.ImageLayout = DataGridViewImageCellLayout.Stretch; // Stretch the image
                    imageCell.Value = image;
                    imageCell.Style.Padding = new Padding(cellMargin);
                    newRow.Cells.Add(imageCell);
                }

                // Add cells to the row, including the "Profile_pic" cell
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["ID"], Style = { Padding = new Padding(cellMargin) } });
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["Name"], Style = { Padding = new Padding(cellMargin) } });
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["depdes"], Style = { Padding = new Padding(cellMargin) } });
                newRow.Cells.Add(new DataGridViewTextBoxCell { Value = row["RFID"], Style = { Padding = new Padding(cellMargin) } });

                // Add the row to the DataGridView
                dgvStudents.Rows.Add(newRow);
                dgvStudents.Rows[dgvStudents.Rows.Count - 1].Height = 80;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            formAttendanceCheckerSettings2 formAttendanceCheckerSettings = new formAttendanceCheckerSettings2();
            formAttendanceCheckerSettings.getSetting(event_code, attendance_stat);
            formAttendanceCheckerSettings.getAttendanceStatus(tbAttendance.Enabled);
            formAttendanceCheckerSettings.getForm(this);
            formAttendanceCheckerSettings.ShowDialog();
            tbAttendance.Focus();
        }

        private void formAttendanceChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void formAttendanceChecker_Load(object sender, EventArgs e)
        {
            displayStudents();
            setDate();
            displayAttendanceRecord();
        }
        public void setEvent(string eventname)
        {
            lblEventName.Text = eventname;
            lblAttStatus.Text = attendance_stat;
        }
        public void setDate()
        {
            DateTime date = DateTime.Now;
            string formattedDate = date.ToString("dddd, MMMM d, yyyy");
            lblDate.Text = formattedDate;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = tbSearch.Text.Trim();
            ApplySearchFilter(searchKeyword);
            if (!string.IsNullOrEmpty(tbSearch.Text))
            {
                btnCancel.Visible = true;
            }
            else
            {
                btnCancel.Visible = false;
            }
        }
        private void ApplySearchFilter(string searchKeyword)
        {
            // Loop through each row in the DataGridView
            foreach (DataGridViewRow row in dgvStudents.Rows)
            {
                bool rowVisible = false;

                // Loop through each cell in the row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (cell.Value != null && cell.Value.ToString().IndexOf(searchKeyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        rowVisible = true;
                        break; // No need to check other cells in the row
                    }
                }

                // Show or hide the row based on search result
                row.Visible = rowVisible;
            }
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            tbAttendance.Focus();
        }

        public void doneAttendanceApperance(string name, Image image)
        {
            Panel pnAttDone = new Panel();
            Label label3 = new Label();
            RoundPictureBoxRect pictureBox4 = new RoundPictureBoxRect();

            pnAttDone.Controls.Add(label3);
            pnAttDone.Controls.Add(pictureBox4);
            pnAttDone.Dock = System.Windows.Forms.DockStyle.Fill;
            pnAttDone.Location = new System.Drawing.Point(0, 0);
            pnAttDone.Name = "pnAttDone";
            pnAttDone.Size = new System.Drawing.Size(300, 312);
            pnAttDone.TabIndex = 0;

            label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            label3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            label3.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label3.ForeColor = System.Drawing.Color.White;
            label3.Location = new System.Drawing.Point(0, 280);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(351, 32);
            label3.TabIndex = 11;
            label3.Text = name;
            label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureBox4.Image = image;
            pictureBox4.Location = new System.Drawing.Point(0, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new System.Drawing.Size(351, 312);
            pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 10;
            pictureBox4.TabStop = false;

            tableLayoutPanel1.Controls.Add(pnAttDone);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbSearch.Text = String.Empty;
        }
        public void Logout()
        {
            this.Dispose();
            FormLoginPage formLoginPage = new FormLoginPage();
            formLoginPage.Show();
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvStudents.Columns[e.ColumnIndex].Name;
            if (col == "check" && e.RowIndex >= 0)
            {
                // Get the value in the "IDColumn" for the clicked row
                object idValue = dgvStudents["stud_id", e.RowIndex].Value;

                // Check if the value is not null
                if (idValue != null)
                {
                    // Convert the value to the appropriate data type (e.g., int)
                    string studentID = idValue.ToString();

                    DateTime dateTimeNow = DateTime.Now;
                    string period = dateTimeNow.Hour < 12 ? "AM" : "PM";
                    if (period.Equals("AM"))
                    {
                        if (attendance_stat.Equals("IN"))
                            sQLite_Connection.GetManualStudentForAttendance(studentID, event_code, dateTimeNow.ToString(), dateTimeNow.ToString(), null, null, null, teach_id);
                        else if (attendance_stat.Equals("OUT"))
                            sQLite_Connection.GetManualStudentForAttendance(studentID, event_code, dateTimeNow.ToString(), null, dateTimeNow.ToString(), null, null, teach_id);
                    }
                    else if (period.Equals("PM"))
                    {
                        if (attendance_stat.Equals("IN"))
                            sQLite_Connection.GetManualStudentForAttendance(studentID, event_code, dateTimeNow.ToString(), null, null, dateTimeNow.ToString(), null, teach_id);
                        else if (attendance_stat.Equals("OUT"))
                            sQLite_Connection.GetManualStudentForAttendance(studentID, event_code, dateTimeNow.ToString(), null, null, null, dateTimeNow.ToString(), teach_id);
                    }

                    DataTable studentInfo = sQLite_Connection.GetStudentByManual(studentID, event_code);

                    if (studentInfo.Rows.Count > 0)
                    {
                        DataRow row = studentInfo.Rows[0]; // Assuming you want to display information for the first student

                        string studID = row["ID"].ToString();
                        string studentName = row["Name"].ToString();
                        string department = row["depdes"].ToString();
                        string section = row["secdes"].ToString();

                        if (row["pic"] is Image image)
                        {
                            ptbProfilePic.Image = image; // Assuming ptbProfilePic is a PictureBox
                        }

                        DateTime dateTime = DateTime.Now;
                        string time = dateTime.ToString("h:mm tt");
                        string date = dateTime.ToString("dddd, MMMM d, yyyy");

                        // Assuming you have labels named lblStudentID, lblStudentName, lblDepartment, lblSection, lblDate, and lblTime
                        lblID.Text = studID;
                        lblName.Text = studentName;
                        lblDepartment.Text = department;
                        lblSection.Text = section;
                        lblAttDate.Text = date;
                        lblAttTime.Text = time;
                    }
                    displayAttendanceRecord();

                    tbAttendance.Text = String.Empty;
                }
            }
        }
    }
}
