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

namespace AMSEMS.SubForm_Guidance
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

        formStudAttRecord form;
        public formRemarks()
        {
            InitializeComponent();
        }
        public void getForm(formStudAttRecord form, string studid, string conid, string classcode)
        {
            this.form = form;
            stud_id = studid;
            con_id = conid;
            class_code = classcode;
        } 
        private void formEventConfig_Load(object sender, EventArgs e)
        {
            displayInfo();
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
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                if (tbReason.Text.Length > 0 && tbRemarks.Text.Length > 0)
                {
                    // Check if the status is already 'Done'
                    string checkQuery = "SELECT Status FROM tbl_consultation_record WHERE Student_ID = @StudID AND Class_Code = @ccode AND Consultation_ID = @conID";
                    using (SqlCommand checkCommand = new SqlCommand(checkQuery, cn))
                    {
                        checkCommand.Parameters.AddWithValue("@StudID", stud_id);
                        checkCommand.Parameters.AddWithValue("@conID", con_id);
                        checkCommand.Parameters.AddWithValue("@ccode", class_code);

                        object statusResult = checkCommand.ExecuteScalar();

                        if (statusResult != null && statusResult.ToString() == "Done")
                        {
                            MessageBox.Show("Record is already marked as 'Done'.", "Consultation Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        else
                        {
                            // Update the status to 'Done'
                            string updateQuery = "UPDATE tbl_consultation_record SET Status = 'Done', Date = @DateNow, Reason = @Reas, Remarks = @Rem WHERE Student_ID = @StudID AND Class_Code = @ccode AND Consultation_ID = @conID";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, cn))
                            {
                                updateCommand.Parameters.AddWithValue("@StudID", stud_id);
                                updateCommand.Parameters.AddWithValue("@conID", con_id);
                                updateCommand.Parameters.AddWithValue("@ccode", class_code);
                                updateCommand.Parameters.AddWithValue("@DateNow", DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"));
                                updateCommand.Parameters.AddWithValue("@Reas", tbReason.Text);
                                updateCommand.Parameters.AddWithValue("@Rem", tbRemarks.Text);
                                updateCommand.ExecuteNonQuery();
                            }
                            form.displayStatus();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Complete the Remarks!", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                cn.Close();
                this.Close();
            }
        }
    }
}
