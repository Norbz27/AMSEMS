using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
