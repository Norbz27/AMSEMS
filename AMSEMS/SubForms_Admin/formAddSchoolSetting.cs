using ComponentFactory.Krypton.Toolkit;
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
    public partial class formAddSchoolSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        String header;
        public formAddSchoolSetting(String header)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.header = header;

            lblHeader1.Text = "Add " + header;
            lblHeader2.Text = header + ":";
            lblHeader3.Text = "List of " + header + ":";

            displayData();
        }

        public void displayData()
        {
            dataGridView.Rows.Clear();

            if (header.Equals("Program"))
            {
                cn.Open();
                cm = new SqlCommand("Select * from tbl_program", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView.Rows.Add(dr["Program_ID"].ToString(), dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();
            }
            else if (header.Equals("Year Level"))
            {
                cn.Open();
                cm = new SqlCommand("Select * from tbl_year_level", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView.Rows.Add(dr["Level_ID"].ToString(), dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();
            }
            else if (header.Equals("Section"))
            {
                cn.Open();
                cm = new SqlCommand("Select * from tbl_Section", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    dataGridView.Rows.Add(dr["Section_ID"].ToString(), dr["Description"].ToString());
                }
                dr.Close();
                cn.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (header.Equals("Program"))
            {

                cm = new SqlCommand("Select * from tbl_program where Description = '"+ tbDes.Text +"'", cn);
                ad = new SqlDataAdapter(cm);
                ad.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i == 0)
                {
                    cn.Open();
                    cm = new SqlCommand("Insert into tbl_program Values (@Des)", cn);
                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else if (header.Equals("Year Level"))
            {
                cm = new SqlCommand("Select * from tbl_year_level where Description = '"+ tbDes.Text +"'", cn);
                ad = new SqlDataAdapter(cm);
                ad.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i == 0)
                {
                    cn.Open();
                    cm = new SqlCommand("Insert into tbl_year_level Values (@Des)", cn);
                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (header.Equals("Section"))
            {
                cm = new SqlCommand("Select * from tbl_Section where Description = '"+ tbDes.Text +"'", cn);
                ad = new SqlDataAdapter(cm);
                ad.Fill(ds);
                int i = ds.Tables[0].Rows.Count;
                if (i == 0)
                {
                    cn.Open();
                    cm = new SqlCommand("Insert into tbl_Section Values (@Des)", cn);
                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                    cm.ExecuteNonQuery();
                    dr.Close();
                    cn.Close();
                }
                else
                {
                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            displayData();
        }
    }
}
