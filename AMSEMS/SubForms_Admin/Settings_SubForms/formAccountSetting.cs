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
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccountSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public formAccountSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void ptbProfile_MouseHover(object sender, EventArgs e)
        {

        }

        private void formAccountSetting_Load(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Firstname,Middlename,Lastname from tbl_admin_accounts where ID = '"+ FormAdminNavigation.id +"'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblFname.Text =  dr["Firstname"].ToString();
                lblMname.Text =  dr["Middlename"].ToString();
                lblLname.Text =  dr["Lastname"].ToString();
                dr.Close();
                cn.Close();
            }
        }
    }
}
