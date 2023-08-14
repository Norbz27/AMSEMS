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

namespace AMSEMS.SubForms_Admin
{
    public partial class formDashboard : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        String id;
        public formDashboard(String id)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            cn.Open();
            cm = new SqlCommand("select Firstname, Lastname from tbl_admin_accounts where ID = '" + id + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
            dr.Close();
            cn.Close();
            this.id = id;
        }
    }
}
