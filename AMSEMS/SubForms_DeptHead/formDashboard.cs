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
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public String id;
        public static String id2 { get; set; }
        static FormDeptHeadNavigation form;
        public formDashboard(String id1)
        {
            InitializeComponent();
            id = id1;
        }
        public static void setForm(FormDeptHeadNavigation form1)
        {
            form = form1;
        }

        private void formDashboard_Load(object sender, EventArgs e)
        {
            if (id.Equals(String.Empty))
                loadData(id2);
            else
                loadData(id);

            form.loadData();
        }
        public void loadData(String id)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("select Firstname, Lastname from tbl_deptHead_accounts where Unique_ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
            }
        }

    }
}
