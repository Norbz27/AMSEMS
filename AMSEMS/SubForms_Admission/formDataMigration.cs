using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formDataMigration : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        String Pass;

        int roleID;
        String choice;
        bool istrue = false;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        formAcctounts_Guidance form;
        formAccounts_SAO form2;
        formDashboard form3;
        formAcctounts_AcadHead form4;
        public formDataMigration()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayCB();
        }
        public void displayCB()
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Description FROM tbl_year_level ORDER BY Description";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using(SqlDataReader dr = cm.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            cbYearlvlTo.Items.Add(dr["Description"].ToString());
                        }
                    }
                }

                query = "SELECT Description FROM tbl_section ORDER BY Description";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSectionTo.Items.Add(dr["Description"].ToString());
                        }
                    }
                }
                
                query = "SELECT Description FROM tbl_Section sc LEFT JOIN tbl_student_accounts st ON sc.Section_ID = st.Section GROUP BY Description ORDER BY Description";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbSectionFrom.Items.Add(dr["Description"].ToString());
                        }
                    }
                }

                query = "SELECT Description FROM tbl_year_level sc LEFT JOIN tbl_student_accounts st ON sc.Level_ID = st.Year_Level GROUP BY Description ORDER BY Description";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            cbYearlvlFrom.Items.Add(dr["Description"].ToString());
                        }
                    }
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string LvlFrom = "", LvlTo = "";
                string SecFrom = "", SecTo = "";

                if (cbSectionFrom.Text.Equals(String.Empty) && cbYearlvlFrom.Text.Equals(String.Empty) && cbSectionTo.Text.Equals(String.Empty) && cbYearlvlTo.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Missing information, Fill in all the form.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    string query = "SELECT Section_ID FROM tbl_section WHERE Description = @sec";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@sec", cbSectionFrom.Text);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                SecFrom = dr["Section_ID"].ToString();
                            }
                        }
                    }

                    query = "SELECT Section_ID FROM tbl_section WHERE Description = @sec";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@sec", cbSectionTo.Text);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                SecTo = dr["Section_ID"].ToString();
                            }
                        }
                    }

                    query = "SELECT Level_ID FROM tbl_year_level WHERE Description = @lvl";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@lvl", cbYearlvlFrom.Text);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                LvlFrom = dr["Level_ID"].ToString();
                            }
                        }
                    }

                    query = "SELECT Level_ID FROM tbl_year_level WHERE Description = @lvl";
                    using (SqlCommand cm = new SqlCommand(query, cn))
                    {
                        cm.Parameters.AddWithValue("@lvl", cbYearlvlTo.Text);
                        using (SqlDataReader dr = cm.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                LvlTo = dr["Level_ID"].ToString();
                            }
                        }
                    }

                    if (LvlFrom != String.Empty && SecFrom != String.Empty || LvlFrom != "" && SecFrom != "")
                    {
                        query = "UPDATE tbl_student_accounts SET Year_Level = @Ylvlto, Section = @Secto WHERE Year_Level = @YlvlFrom AND Section = @SecFrom";
                        using (SqlCommand cm = new SqlCommand(query, cn))
                        {
                            cm.Parameters.AddWithValue("@Ylvlto", LvlTo);
                            cm.Parameters.AddWithValue("@Secto", SecTo);
                            cm.Parameters.AddWithValue("@YlvlFrom", LvlFrom);
                            cm.Parameters.AddWithValue("@SecFrom", SecFrom);
                            cm.ExecuteNonQuery();

                            MessageBox.Show("Seccessfully migrated students infotmation", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
        }
    }
}
