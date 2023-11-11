using System;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formAbsReport : Form
    {
        public formAbsReport(String accountName)
        {
            InitializeComponent();
            lblAccountName.Text = accountName;
        }
    }
}
