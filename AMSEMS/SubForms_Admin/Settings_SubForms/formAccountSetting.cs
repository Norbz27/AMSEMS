using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;
using Microsoft.VisualBasic.ApplicationServices;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccountSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string id;

        public formAccountSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormAdminNavigation.id;

        }

        private void ptbProfile_MouseHover(object sender, EventArgs e)
        {

        }

        private void formAccountSetting_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select ID,Firstname,Middlename,Lastname from tbl_admin_accounts where Unique_ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblFname.Text = dr["Firstname"].ToString();
                lblMname.Text = dr["Middlename"].ToString();
                lblLname.Text = dr["Lastname"].ToString();
                lblSchoolID.Text = dr["ID"].ToString();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Profile_pic from tbl_admin_accounts where ID = " + id + "", cn);

                byte[] imageData = (byte[])cm.ExecuteScalar();

                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);
                        ptbProfile.Image = image;
                    }
                }
                cn.Close();
            }
        }

        private void btnChamgePass_Click(object sender, EventArgs e)
        {
            formChangePass formChangePass = new formChangePass();
            formChangePass.ShowDialog();
        }

        private void btnEditID_Click(object sender, EventArgs e)
        {
            formChangeID formChangeID = new formChangeID(this);
            formChangeID.ShowDialog();
        }

        private void btnChangeProf_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            formChangeImage formChangeImage = new formChangeImage(this, openFileDialog1.FileName);

            formChangeImage.ShowDialog();
        }
    }
}
