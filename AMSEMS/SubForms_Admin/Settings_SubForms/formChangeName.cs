﻿using ComponentFactory.Krypton.Toolkit;
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
    public partial class formChangeName : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        formAccountSetting form;
        public formChangeName(formAccountSetting form)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            this.form = form;

            cn.Open();
            cm = new SqlCommand("Select Firstname,Middlename,Lastname from tbl_admin_accounts where Unique_ID = '" + FormAdminNavigation.id + "'", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            tbFname.Text = dr["Firstname"].ToString();
            tbMname.Text = dr["Middlename"].ToString();
            tbLname.Text = dr["Lastname"].ToString();
            dr.Close();
            cn.Close();
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbFname.Text.Equals(String.Empty) || tbLname.Text.Equals(String.Empty) || tbMname.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbl_admin_accounts SET Firstname = @NewValue1, Middlename = @NewValue2, Lastname = @NewValue3 WHERE Unique_ID = @ConditionValue", cn);
                    cm.Parameters.AddWithValue("@NewValue1", tbFname.Text);
                    cm.Parameters.AddWithValue("@NewValue2", tbMname.Text);
                    cm.Parameters.AddWithValue("@NewValue3", tbLname.Text);
                    cm.Parameters.AddWithValue("@ConditionValue", FormAdminNavigation.id);
                    cm.ExecuteNonQuery();
                    MessageBox.Show("Name Changed!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    form.loadData();
                }
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
