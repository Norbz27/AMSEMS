using iTextSharp.xmp.options;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formAbsReport : Form
    {
        string rep;
        public formAbsReport(String rep)
        {
            InitializeComponent();
            this.rep = rep;
            lblRepHeader.Text = rep + " Absenteeism Report";
            
        }
        public void displayFilter()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT sec.Description AS secdes FROM tbl_Section sec LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID WHERE al.Academic_Level_Description = @acadlvl";
                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@acadlvl", rep);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        cbSection.Items.Clear();
                        int count = 1;
                        cbSection.Items.Add("All");
                        while (dr.Read())
                        {
                            cbSection.Items.Add(dr["secdes"].ToString());
                        }
                    }
                }
                cbSection.SelectedIndex = 0;
            }
        }
        public void displayReport()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = @"SELECT 
                                ID,
                                UPPER(s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename) AS Name,
                                sec.Description AS secdes,
                                COUNT(DISTINCT CASE WHEN sat.Student_Status = 'A' THEN sat.Attendance_date END) AS ConsecutiveAbsentDays
                            FROM 
                                tbl_subject_attendance sat
                                LEFT JOIN tbl_student_accounts s ON sat.Student_ID = s.ID 
                                LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID 
	                            LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID
                            WHERE 
                                al.Academic_Level_Description = @acadlvl 
                                AND (@SectionDescription = 'All' OR sec.Description = @SectionDescription) 
                                AND s.Status = 1
                            GROUP BY 
                                ID, s.Lastname, s.Firstname, s.Middlename, sec.Description
                            HAVING 
                                COUNT(DISTINCT CASE WHEN sat.Student_Status = 'A' THEN sat.Attendance_date END) > 2
                            ORDER BY
	                            Name;";

                using (SqlCommand cmd = new SqlCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@acadlvl", rep);
                    cmd.Parameters.AddWithValue("@SectionDescription", string.IsNullOrEmpty(cbSection.Text) ? DBNull.Value : (object)cbSection.Text);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        dgvAbesnteismRep.Rows.Clear();
                        int count = 1;
                        while (dr.Read())
                        {
                            // Add a row and set the checkbox column value to false (unchecked)
                            int rowIndex = dgvAbesnteismRep.Rows.Add(false);

                            // Populate other columns, starting from index 1
                            dgvAbesnteismRep.Rows[rowIndex].Cells["studid"].Value = dr["ID"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["studname"].Value = dr["Name"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                            dgvAbesnteismRep.Rows[rowIndex].Cells["absences"].Value = dr["ConsecutiveAbsentDays"].ToString();
                        }
                    }
                }
            }
        }

        private void formAbsReport_Load(object sender, EventArgs e)
        {
            displayFilter();
        }

        private void cbSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            displayReport();
        }

        private void dgvAbesnteismRep_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvAbesnteismRep.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvAbesnteismRep.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvAbesnteismRep, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UseWaitCursor = true;
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                // Get the ContextMenuStrip associated with the clicked item
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                if (menu != null)
                {
                    // Get the DataGridView that the context menu is associated with
                    DataGridView dataGridView = menu.SourceControl as DataGridView;

                    if (dataGridView != null)
                    {
                        int rowIndex = dataGridView.CurrentCell.RowIndex;
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];
                        formStudAttRecord form = new formStudAttRecord();
                        form.getForm(this, dgvAbesnteismRep.Rows[rowIndex].Cells[0].Value.ToString());
                        form.ShowDialog();
                        UseWaitCursor = false;
                    }
                }
            }
        }
    }
}
