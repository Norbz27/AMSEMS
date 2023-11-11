using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceRecord : Form
    {
        public formAttendanceRecord()
        {
            InitializeComponent();
        }

        public void displayTable(string query)
        {
            if (dgvRecord.InvokeRequired)
            {
                dgvRecord.Invoke(new Action(() => displayTable(query)));
                return;
            }
            try
            {
                dgvRecord.Rows.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvRecord.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvRecord.Rows[rowIndex].Cells["ID"].Value = dr["Course_code"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                //dgvRecord.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void formAttendanceRecord_Load(object sender, EventArgs e)
        {
            displayTable("Select Student_,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1");
        }
    }
}
