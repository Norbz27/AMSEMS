using ComponentFactory.Krypton.Toolkit;
using PusherServer;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAccountSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool fileChosen = false;

        public formAccountSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
        }

        private void formAccountSetting_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("Select ID,Firstname,Middlename,Lastname from tbl_admin_accounts where Unique_ID = @id", cn);
                    cm.Parameters.AddWithValue("@id", FormAdminNavigation.id);
                    dr = cm.ExecuteReader();

                    if (dr != null && dr.HasRows)
                    {
                        dr.Read();
                        lblFname.Text = (dr["Firstname"] != DBNull.Value) ? dr["Firstname"].ToString() : string.Empty;
                        lblMname.Text = (dr["Middlename"] != DBNull.Value) ? dr["Middlename"].ToString() : string.Empty;
                        lblLname.Text = (dr["Lastname"] != DBNull.Value) ? dr["Lastname"].ToString() : string.Empty;
                        lblSchoolID.Text = (dr["ID"] != DBNull.Value) ? dr["ID"].ToString() : string.Empty;
                        lblName.Text = lblFname.Text + " " + lblLname.Text;
                    }

                    dr.Close();
                    cn.Close();

                    cn.Open();
                    cm = new SqlCommand("Select Profile_pic from tbl_admin_accounts where Unique_ID = @id", cn);
                    cm.Parameters.AddWithValue("@id", FormAdminNavigation.id);
                    byte[] imageData = cm.ExecuteScalar() as byte[];

                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);

                            if (image != null)
                            {
                                ptbProfile.Image = image;
                            }
                        }
                    }

                    cn.Close();
                }
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        private void btnChamgePass_Click(object sender, EventArgs e)
        {
            formChangePass formChangePass = new formChangePass();
            formChangePass.ShowDialog();
        }

        private void btnEditID_Click(object sender, EventArgs e)
        {
            formChangeID formChangeID = new formChangeID(this);
            formChangeID.ShowDialog();
        }

        private void btnChangeProf_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            if (fileChosen)
            {
                formChangeImage formChangeImage = new formChangeImage(this, openFileDialog1.FileName);
                formChangeImage.ShowDialog();
                fileChosen = false; // Reset the flag
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            // Set the flag to indicate that a file has been chosen
            fileChosen = true;

            // Close the OpenFileDialog
            openFileDialog1.Dispose();

        }

        private void btnRemoveProf_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (MessageBox.Show("Are you sure you want to remove your Profile Picture?", "AMSEMS", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cn.Open();
                    cm = new SqlCommand("UPDATE tbl_admin_accounts SET Profile_pic = DEFAULT WHERE Unique_ID = @ConditionValue", cn);
                    cm.Parameters.AddWithValue("@ConditionValue", FormAdminNavigation.id);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadData();

                    var option = new PusherOptions
                    {
                        Cluster = "ap1",
                        Encrypted = true,
                    };
                    var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                    var result = pusher.TriggerAsync("amsems", FormAdminNavigation.id, new { message = "new notification" });
                }
            }
        }

        private void btnEditFname_Click(object sender, EventArgs e)
        {
            formChangeName formChangeName = new formChangeName(this);
            formChangeName.ShowDialog();
        }

        private void btnEditMname_Click(object sender, EventArgs e)
        {
            formChangeName formChangeName = new formChangeName(this);
            formChangeName.ShowDialog();
        }

        private void btEditLname_Click(object sender, EventArgs e)
        {
            formChangeName formChangeName = new formChangeName(this);
            formChangeName.ShowDialog();
        }
        private void FormAccountSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
