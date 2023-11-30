using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectInformation : Form
    {
        static FormTeacherNavigation form;
        public formSubjectInformation()
        {
            InitializeComponent();
        }
        public static void setForm(FormTeacherNavigation form1)
        {
            form = form1;
        }
        private void btnback_Click(object sender, EventArgs e)
        {
            form.otherformclick();
        }
    }
}
