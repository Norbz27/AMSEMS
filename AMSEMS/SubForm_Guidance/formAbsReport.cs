using System;
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
                string query = "SELECT ID , s.Lastname + ', ' + s.Firstname + ' ' + s.Middlename AS Name, sec.Description AS secdes,  COUNT(CASE WHEN sat.Student_Status = 'A' THEN 1 END) AS AbsentCount FROM tbl_subject_attendance sat LEFT JOIN tbl_student_accounts s ON sat.Student_ID = s.ID LEFT JOIN tbl_Section sec ON s.Section = sec.Section_ID LEFT JOIN tbl_academic_level al ON sec.AcadLevel_ID = al.Academic_Level_ID WHERE al.Academic_Level_Description = @acadlvl AND (@SectionDescription = 'All' OR sec.Description = @SectionDescription) GROUP BY ID,s.Lastname, s.Firstname, s.Middlename, sec.Description HAVING COUNT(CASE WHEN sat.Student_Status = 'A' THEN 1 END) > 2;";

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
                            dgvAbesnteismRep.Rows[rowIndex].Cells["absences"].Value = dr["AbsentCount"].ToString();
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
    }
}
