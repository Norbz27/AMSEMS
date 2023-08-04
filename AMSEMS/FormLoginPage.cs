using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS
{
    public partial class FormLoginPage : KryptonForm
    {
        public FormLoginPage()
        {
            InitializeComponent();
        }

        private void tbID_Enter(object sender, EventArgs e)
        {
            if(tbID.Text == "School ID")
            {
                tbID.Text = string.Empty;
                tbID.StateCommon.Content.Color1 = Color.DimGray;
                tbID.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbID.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbID_Leave(object sender, EventArgs e)
        {
            if (tbID.Text == string.Empty)
            {
                tbID.Text = "School ID";
                tbID.StateCommon.Content.Color1 = Color.Silver;
                tbID.StateCommon.Border.Color1 = Color.Gray;
                tbID.StateCommon.Border.Color2 = Color.Gray;
            }
        }

        private void tbPass_Enter(object sender, EventArgs e)
        {
            if (tbPass.Text == "Password")
            {
                tbPass.Text = string.Empty;
                tbPass.StateCommon.Content.Color1 = Color.DimGray;
                tbPass.UseSystemPasswordChar = true;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 8F);
                tbPass.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbPass.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbPass_Leave(object sender, EventArgs e)
        {
            if (tbPass.Text == string.Empty)
            {
                tbPass.Text = "Password";
                tbPass.StateCommon.Content.Color1 = Color.Silver;
                tbPass.UseSystemPasswordChar = false;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 10F);
                tbPass.StateCommon.Border.Color1 = Color.Gray;
                tbPass.StateCommon.Border.Color2 = Color.Gray;
            }            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                login();
            }
        }

        public void login()
        {
            if (tbID.Text.Equals("admin"))
            {
                FormAdminNavigation formAdminNavigation = new FormAdminNavigation();
                formAdminNavigation.Show();
                this.Hide();
            }
            else if (tbID.Text.Equals("sao"))
            {
                FormSAONavigation formSAONavigation = new FormSAONavigation();
                formSAONavigation.Show();
                this.Hide();
            }
            else if (tbID.Text.Equals("dept"))
            {
                FormDeptHeadNavigation formDeptHeadNavigation = new FormDeptHeadNavigation();
                formDeptHeadNavigation.Show();
                this.Hide();
            }
            else if (tbID.Text.Equals("gui"))
            {
                FormGuidanceNavigation formGuidanceNavigation = new FormGuidanceNavigation();
                formGuidanceNavigation.Show();
                this.Hide();
            }
            else if (tbID.Text.Equals("tech"))
            {
                FormTeacherNavigation formTeacherNavigation = new FormTeacherNavigation();
                formTeacherNavigation.Show();
                this.Hide();
            }
        }
    }
}
