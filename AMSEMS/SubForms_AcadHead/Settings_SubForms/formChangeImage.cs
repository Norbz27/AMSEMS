using ComponentFactory.Krypton.Toolkit;
using PusherServer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

namespace AMSEMS.SubForms_AcadHead
{
    public partial class formChangeImage : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        formAccountSetting form;
        String file;
        public formChangeImage(formAccountSetting form, String file)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.form = form;
            this.file = file;
            ptbProfile.Image = Image.FromFile(file);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                byte[] picData = System.IO.File.ReadAllBytes(file);

                cn.Open();
                cm = new SqlCommand("UPDATE tbl_acadHead_accounts SET Profile_pic = @NewValue WHERE Unique_ID = @ConditionValue", cn);
                cm.Parameters.AddWithValue("@NewValue", picData);
                cm.Parameters.AddWithValue("@ConditionValue", FormAcadHeadNavigatio.id);
                cm.ExecuteNonQuery();
                cn.Close();
                form.loadData();
                this.Close();

                var option = new PusherOptions
                {
                    Cluster = "ap1",
                    Encrypted = true,
                };
                var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                var result = pusher.TriggerAsync("amsems", FormAcadHeadNavigatio.id, new { message = "new notification" });
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
