using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAttendanceReport : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        public formAttendanceReport()
        {
            InitializeComponent();
        }

        public void displayReport()
        {
            dgvReport.Columns.Clear();
            dgvReport.Columns.Add("id", "ID");
            dgvReport.Columns.Add("name", "Name");
            dgvReport.Columns.Add("section", "Section");
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Event_Name FROM tbl_events e LEFT JOIN tbl_attendance a ON e.Event_ID = a.Event_ID";
                using (cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader eventsReader = cm.ExecuteReader())
                    {
                        while (eventsReader.Read())
                        {
                            string eventName = eventsReader["Event_Name"].ToString();
                            if (!dgvReport.Columns.Contains(eventName))
                            {
                                dgvReport.Columns.Add(eventName, eventName);
                            }
                        }
                    }
                }
            }
        }


        private void formAttendanceReport_Load(object sender, System.EventArgs e)
        {
            displayReport();
        }
    }
}
