
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formEditAnnouncement : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        formAnnouncement form;
        string id;
        string announceBy;
        private string searchKeyword = string.Empty;
        private DateTime filterDate = DateTime.MinValue;
        public formEditAnnouncement()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
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
                        cm = new SqlCommand("UPDATE tbl_Announcement SET Announcement_Title = @Title, Announcement_Description = @Des WHERE Announcement_ID = @ID", cn);
                        cm.Parameters.AddWithValue("@ID", id);
                        cm.Parameters.AddWithValue("@Title", tbAnnounceTitle.Text);
                        cm.Parameters.AddWithValue("@Des", tbDescription.Text);
                        cm.ExecuteNonQuery();

                        MessageBox.Show("Announcement Updated!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        public void dispayInfo(string id)
        {
            this.id = id;
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                using (cm = new SqlCommand("SELECT * FROM tbl_Announcement WHERE Announcement_ID = " + id, cn))
                using (dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        tbAnnounceTitle.Text = dr["Announcement_Title"].ToString();
                        tbDescription.Text = dr["Announcement_Description"].ToString();
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
