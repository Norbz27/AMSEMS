using ComponentFactory.Krypton.Toolkit;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formRemarks : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        string stud_id, con_id, class_code;
        string event_id, date;

        public bool isCollapsed;
        private List<string> suggestions = new List<string>{};
        private ListBox listBoxSuggestions;

        formSubjectOverview form;
        public formRemarks()
        {
            InitializeComponent();
        }
        public void getForm(formSubjectOverview form, string studid, string conid, string classcode)
        {
            this.form = form;
            stud_id = studid;
            con_id = conid;
            class_code = classcode;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formEventConfig_Load(object sender, EventArgs e)
        {
            displayInfo();
            displayRemarks();
        }
        public void displayInfo()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT UPPER(Firstname + ' ' + Middlename + ' ' + Lastname) AS Name FROM tbl_student_accounts WHERE ID = @studID";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@studID", stud_id);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string name = dr["Name"].ToString();
                            lblStudName.Text = name;
                        }
                    }
                }
            }
            DateTime now = DateTime.Now;
            string formatteddate = now.ToString("MMMM dd, yyyy");
            string formattedtime = now.ToString("hh:mm tt");
            lblDate.Text = formatteddate;
            lblTime.Text = formattedtime;
        }
        public void displayRemarks()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Reason, Remarks FROM tbl_consultation_record WHERE Consultation_ID = @conid";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@conid", con_id);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string res = dr["Reason"].ToString();
                            string rem = dr["Remarks"].ToString();
                            tbReason.Text = res;
                            tbRemarks.Text = rem;
                        }
                    }
                }
            }
        }
    }
}
