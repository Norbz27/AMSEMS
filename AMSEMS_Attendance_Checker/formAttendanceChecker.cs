using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class formAttendanceChecker : KryptonForm
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;

        SQLite_Connection sQLite_Connection;

        string attendance_stat;
        string event_code;

        public bool isCollapsed;
        public formAttendanceChecker()
        {
            InitializeComponent();
            this.splitContainer1.Panel2Collapsed = true;
            isCollapsed = true;

            sQLite_Connection = new SQLite_Connection("db_AMSEMS_CHECKER.db");

        }
        public void getAttendanceSettings(string att, string code)
        {
            attendance_stat = att;
            event_code = code;
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
                
            }
            else
            {
                this.splitContainer1.Panel2Collapsed = true;
                timer1.Stop();
                isCollapsed = true;
            }
        }
        public void displayStudents()
        {
            dgvStudents.Rows.Clear();
            DataTable allStudents = sQLite_Connection.GetAllStudents();

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

                // Add the row to the DataGridView
                dgvStudents.Rows.Add(newRow);
                dgvStudents.Rows[dgvStudents.Rows.Count - 1].Height = 80;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            formAttendanceCheckerSettings2 formAttendanceCheckerSettings = new formAttendanceCheckerSettings2();
            formAttendanceCheckerSettings.getSetting(event_code, attendance_stat);
            formAttendanceCheckerSettings.getForm(this);
            formAttendanceCheckerSettings.ShowDialog();
        }

        private void formAttendanceChecker_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void formAttendanceChecker_Load(object sender, EventArgs e)
        {
            displayStudents();
            setDate();
        }
        public void setEvent(string eventname)
        {
            lblEventName.Text = eventname;
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
    }
}
