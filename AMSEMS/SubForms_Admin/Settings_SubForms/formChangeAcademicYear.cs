using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formChangeAcademicYear : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        formAcademicYearSetting form;
        public formChangeAcademicYear(formAcademicYearSetting form)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            this.form = form;

            cn.Open();
            cm = new SqlCommand("Select TOP 1 Academic_Year_Start,Academic_Year_End,Ter_Academic_Sem,SHS_Academic_Sem from tbl_acad ORDER BY 1 DESC;", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            tbStartY.Text = dr["Academic_Year_Start"].ToString();
            tbEndY.Text = dr["Academic_Year_End"].ToString();
            tbTerSem.Text = dr["Ter_Academic_Sem"].ToString();
            tbSHSSem.Text = dr["SHS_Academic_Sem"].ToString();
            dr.Close();
            cn.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbStartY.Text.Equals(String.Empty) || tbTerSem.Text.Equals(String.Empty) || tbEndY.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbl_acad SET Academic_Year_Start = @NewValue1, Academic_Year_End = @NewValue2, Ter_Academic_Sem = @NewValue3, SHS_Academic_Sem = @NewValue4 WHERE Acad_ID = 1", cn);
                    cm.Parameters.AddWithValue("@NewValue1", tbStartY.Text);
                    cm.Parameters.AddWithValue("@NewValue2", tbEndY.Text);
                    cm.Parameters.AddWithValue("@NewValue3", tbTerSem.Text);
                    cm.Parameters.AddWithValue("@NewValue4", tbSHSSem.Text);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Academic Status Changed!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    form.loadAcad();
                }
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
