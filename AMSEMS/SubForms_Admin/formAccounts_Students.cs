using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccounts_Students : Form
    {
        public formAccounts_Students(String accountName, int role)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
        }

        private void btnAddStudent_Click(object sender, EventArgs e)
        {
            formStudentForm formStudentForm = new formStudentForm(5);
            formStudentForm.ShowDialog();
        }
    }
}
