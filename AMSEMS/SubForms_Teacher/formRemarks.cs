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
        string schyear, term;

        public bool isCollapsed;
        private List<string> suggestions = new List<string>{};
        private ListBox listBoxSuggestions;

        formSubjectOverview form;
        private string acadSchYeear;
        private string acadShsSem;
        private string acadTerSem;
        private string subacadlvl;

        public formRemarks()
        {
            InitializeComponent();
        }
        public void getForm(formSubjectOverview form, string studid, string conid, string classcode, string schyear, string term, string subacadlvl)
        {
            this.form = form;
            stud_id = studid;
            con_id = conid;
            class_code = classcode;
            this.schyear = schyear;
            this.term = term;
            this.subacadlvl = subacadlvl;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void formEventConfig_Load(object sender, EventArgs e)
        {
            academic();
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
       
        }
        public void academic()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";
                query = "SELECT Acad_ID, (Academic_Year_Start +'-'+ Academic_Year_End) AS schyear FROM tbl_acad WHERE Status = 1";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    using (SqlDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            acadSchYeear = rd["schyear"].ToString();
                        }
                    }
                }

                query = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {

                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadShsSem = dr["Description"].ToString();
                        }
                    }
                }

                query = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            acadTerSem = dr["Description"].ToString();
                        }
                    }
                }
            }
        }
        public void displayRemarks()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";
                if (subacadlvl.Equals("10001"))
                {
                    if (acadSchYeear.Equals(schyear) && acadTerSem.Equals(term))
                    {
                         query = "SELECT Reason, Remarks, Date FROM tbl_consultation_record WHERE Consultation_ID = @conid";
                    }
                    else
                    {
                         query = "SELECT Reason, Remarks, Date FROM tbl_archived_consultation_record WHERE Consultation_ID = @conid";
                    }
                }
                else
                {
                    if (acadSchYeear.Equals(schyear) && acadShsSem.Equals(term))
                    {
                         query = "SELECT Reason, Remarks, Date FROM tbl_consultation_record WHERE Consultation_ID = @conid";
                    }
                    else
                    {
                         query = "SELECT Reason, Remarks, Date FROM tbl_archived_consultation_record WHERE Cnsultation_ID = @conid";
                    }
                }
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    cm.Parameters.AddWithValue("@conid", con_id);
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            string res = dr["Reason"].ToString();
                            string rem = dr["Remarks"].ToString();
                            DateTime date = Convert.ToDateTime(dr["Date"].ToString());

                            tbReason.Text = res;
                            tbRemarks.Text = rem;

                            string formatteddate = date.ToString("MMMM dd, yyyy");
                            string formattedtime = date.ToString("hh:mm tt");
                            lblDate.Text = formatteddate;
                            lblTime.Text = formattedtime;
                        }
                    }
                }
            }
        }
    }
}
