using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formAcademicYearSetting : KryptonForm
    {
        private SqlConnection cn;
        private SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool showMessageOnToggle = false;
        private bool previousToggleState; // Store the previous toggle state

        public formAcademicYearSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            loadAcad();
        }

        public void loadAcad()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Academic_Year_Start,Academic_Year_End,Academic_Sem from tbl_acad where Acad_ID = 1", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblAcadYear.Text = dr["Academic_Year_Start"].ToString() + "-" + dr["Academic_Year_End"].ToString();
                lblAcadSem.Text = dr["Academic_Sem"].ToString();
                dr.Close();
                cn.Close();
            }
        }
    }
}
