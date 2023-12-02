using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.Office.Interop.Excel;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjectMainPage : Form
    {
        SQLite_Connection conn;
        private Form activeForm;
        static FormTeacherNavigation form;
        static string ccode;
        static string subjectAcadlvl;
        public formSubjectMainPage()
        {
            InitializeComponent();
            conn = new SQLite_Connection();
        }
        public static void setForm(FormTeacherNavigation form1, string ccode1)
        {
            form = form1;
            ccode = ccode1;
        }
        private void formSubjectInformation_Load(object sender, EventArgs e)
        {
           
        }
    }
}
