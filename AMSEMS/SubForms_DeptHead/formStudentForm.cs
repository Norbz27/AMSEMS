using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formStudentForm : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        string Pass;

        int roleID1;
        string choice1;
        bool istrue = false;
        private static readonly Random RandomGenerator = new Random();

        formAccounts_Students form1;
        formDashboard form2;
        public formStudentForm()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

        }
        public void setData(int roleID, String choice, formAccounts_Students form)
        {
            form1 = form;
            roleID1 = roleID;
            choice1 = choice;
        }
        public void setData2(int roleID, String choice, formDashboard form, bool istrue)
        {
            form2 = form;
            roleID1 = roleID;
            choice1 = choice;
            this.istrue = istrue;
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            displayPSY();
        }

        public void displayPSY()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_Role where Role_ID = " + roleID1 + "", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    tbRole.Text = dr["Description"].ToString();
                }
                dr.Close();
                cn.Close();
            }
        }

        public void getStudID(String ID)
        {
            using (cn)
            {
                try
                {
                    cn.Open();
                    cm = new SqlCommand("Select Profile_pic from tbl_student_accounts where ID = " + ID + "", cn);

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

                    cn.Open();
                    cm = new SqlCommand("Select ID,RFID,Firstname,Lastname,Middlename,Password,p.Description as pDes,se.Description as sDes,yl.Description as yDes, d.Description as depDes,st.Description as stDes from tbl_student_accounts as sa " +
                        "left join tbl_program as p on sa.Program = p.Program_ID left join tbl_Section as se on sa.Section = se.Section_ID " +
                        "left join tbl_year_level as yl on sa.Year_level = yl.Level_ID " +
                        "left join tbl_Departments as d on sa.Department = d.Department_ID " +
                        "left join tbl_status as st on sa.Status = st.Status_ID where ID = " + ID + "", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    tbID.Text = dr["ID"].ToString();
                    tbRFID.Text = dr["RFID"].ToString();
                    tbFname.Text = dr["Firstname"].ToString().ToUpper();
                    tbLname.Text = dr["Lastname"].ToString().ToUpper();
                    tbMname.Text = dr["Middlename"].ToString().ToUpper();
                    Pass = dr["Password"].ToString();
                    tbProgram.Text = dr["pDes"].ToString();
                    tbSec.Text = dr["sDes"].ToString();
                    tbYlevel.Text = dr["yDes"].ToString();
                    tbDep.Text = dr["depDes"].ToString();
                    tbStatus.Text = dr["stDes"].ToString();
                    dr.Close();
                    cn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void formStudentForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!istrue)
            {
                form1.displayFilter();
            }
        }
    }
}
