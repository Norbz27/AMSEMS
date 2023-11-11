
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formAddAnnouncement : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        formAnnouncement form;
        string announceBy;
        private string searchKeyword = string.Empty;
        private DateTime filterDate = DateTime.MinValue;
        public formAddAnnouncement()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            announceBy = "Student Association Office(SAO)";
        }
        public void getForm(formAnnouncement form)
        {
            this.form = form;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbAnnounceTitle.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        DateTime dateTime = DateTime.Now;
                        cn.Open();
                        cm = new SqlCommand("Insert Into tbl_Announcement Values (@Title,@Des,@DateTime,@AnnounceBy)", cn);
                        cm.Parameters.AddWithValue("@Title", tbAnnounceTitle.Text);
                        cm.Parameters.AddWithValue("@Des", tbDescription.Text);
                        cm.Parameters.AddWithValue("@DateTime", dateTime);
                        cm.Parameters.AddWithValue("@AnnounceBy", announceBy);
                        cm.ExecuteNonQuery();

                        MessageBox.Show("Announcement Forwarded!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        form.displayAnnouncements(searchKeyword, filterDate);
                        this.Dispose();
                    }
                }
            }

        }

        private void formAddEvent_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
