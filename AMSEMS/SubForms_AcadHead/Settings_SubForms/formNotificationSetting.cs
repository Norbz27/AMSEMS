using ComponentFactory.Krypton.Toolkit;
using System.Data.SqlClient;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formNotificationSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string id;
        private bool fileChosen = false;

        public formNotificationSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormAdmissionNavigation.id;

        }

    }
}
