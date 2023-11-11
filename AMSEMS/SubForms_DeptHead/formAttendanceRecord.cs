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
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        string event_id, date;

        public formAttendanceRecord()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }
        public void displayFilter()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cbEvents.Items.Clear();
                    cbSection.Items.Clear();
                    cbYlevel.Items.Clear();
                    cn.Open();
                    cm = new SqlCommand("Select Event_ID, Event_Name from tbl_events ORDER BY Start_Date", cn);
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

                    cm = new SqlCommand("Select Description from tbl_year_level", cn);
                    dr = cm.ExecuteReader();
                    while (dr.Read())
                    {
                        cbYlevel.Items.Add(dr["Description"].ToString());
                    }
                    dr.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void displayTable()
        {
            if (dgvRecord.InvokeRequired)
            {
                dgvRecord.Invoke(new Action(() => displayTable()));
                return;
            }
            try
            {
                dgvRecord.Rows.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    string query = @"SELECT stud.ID AS id, UPPER(stud.Firstname) AS fname,
                                    UPPER(stud.Middlename) AS mname,
                                    UPPER(stud.Lastname) AS lname,
                                    sec.Description AS secdes,
                                    FORMAT(att.Date_Time, 'yyyy-MM-dd') AS Date_Time,
                                    COALESCE(att.Penalty, 0) AS Penalty,
                                    ISNULL(FORMAT(att.AM_IN, 'hh:mm tt'), '-------') AS AM_IN,
                                    ISNULL(FORMAT(att.AM_OUT, 'hh:mm tt'), '-------') AS AM_OUT,
                                    ISNULL(FORMAT(att.PM_IN, 'hh:mm tt'), '-------') AS PM_IN,
                                    ISNULL(FORMAT(att.PM_OUT, 'hh:mm tt'), '-------') AS PM_OUT,
                                    UPPER(teach.Lastname) AS teachlname 
                                FROM 
                                    tbl_attendance AS att
                                LEFT JOIN 
                                    tbl_student_accounts AS stud ON att.Student_ID = stud.ID
                                LEFT JOIN 
                                    tbl_Section AS sec ON stud.Section = sec.Section_ID
                                LEFT JOIN 
                                    tbl_teacher_accounts AS teach ON att.Checker = teach.ID 
                                WHERE 
                                    Event_ID = @EventID AND Date_Time >= CAST(@Date AS DATE) AND stud.Department = @Dep;
                                ";

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        cmd.Parameters.AddWithValue("@EventID", event_id);
                        cmd.Parameters.AddWithValue("@Date", date);
                        cmd.Parameters.AddWithValue("@Dep", FormDeptHeadNavigation.dep);
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int rowIndex = dgvRecord.Rows.Add(false);

                                string name = dr["fname"].ToString() + " " + dr["mname"].ToString() + " " + dr["lname"].ToString();

                                dgvRecord.Rows[rowIndex].Cells["ID"].Value = dr["id"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["name"].Value = name;
                                dgvRecord.Rows[rowIndex].Cells["section"].Value = dr["secdes"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["event_date"].Value = dr["Date_Time"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["penalty"].Value = dr["Penalty"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["am_login"].Value = dr["AM_IN"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["am_logout"].Value = dr["AM_OUT"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["pm_login"].Value = dr["PM_IN"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["pm_logout"].Value = dr["PM_OUT"].ToString();
                                dgvRecord.Rows[rowIndex].Cells["checker"].Value = dr["teachlname"].ToString();

                                rowIndex++;
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
            displayFilter();
            displayTable();
        }

        private void Dt_ValueChanged(object sender, EventArgs e)
        {
            date = Dt.Value.ToString();
            displayTable();
        }

        private void cbEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    // Use parameterized query to avoid SQL injection
                    string query = "SELECT Event_ID, Event_Name, Start_Date FROM tbl_events WHERE Event_Name = @EventName";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@EventName", cbEvents.Text);
                        SqlDataReader dr = cm.ExecuteReader();

                        dr.Read();
                        event_id = dr["Event_ID"].ToString();
                        Dt.Value = (DateTime)dr["Start_Date"];
                        date = Dt.Value.ToString();
                        displayTable();
                        dr.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
