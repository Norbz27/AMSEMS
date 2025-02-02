﻿using ComponentFactory.Krypton.Toolkit;
using PusherServer;
using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formAccountSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string id;
        private bool fileChosen = false;

        public formAccountSetting()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            id = FormGuidanceNavigation.id;
        }

        private void formAccountSetting_Load(object sender, EventArgs e)
        {
            loadData();
        }
        public void loadData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select ID,Firstname,Middlename,Lastname from tbl_guidance_accounts where Unique_ID = '" + id + "'", cn);
                dr = cm.ExecuteReader();
                dr.Read();
                lblFname.Text = dr["Firstname"].ToString();
                lblMname.Text = dr["Middlename"].ToString();
                lblLname.Text = dr["Lastname"].ToString();
                lblSchoolID.Text = dr["ID"].ToString();
                lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                dr.Close();
                cn.Close();

                cn.Open();
                cm = new SqlCommand("Select Profile_pic from tbl_guidance_accounts where Unique_ID = " + id + "", cn);

                byte[] imageData = (byte[])cm.ExecuteScalar();

                if (imageData != null && imageData.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        Image image = Image.FromStream(ms);
                        ptbProfile.Image = image;
                    }
                }
                cn.Close();
            }
        }
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            loadData(); // Load data

            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This method runs on the UI thread
            // Update the UI or perform other tasks after the background work completes
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
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
                    cm = new SqlCommand("UPDATE tbl_guidance_accounts SET Profile_pic = DEFAULT WHERE Unique_ID = @ConditionValue", cn);
                    cm.Parameters.AddWithValue("@ConditionValue", FormGuidanceNavigation.id);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    loadData();

                    var option = new PusherOptions
                    {
                        Cluster = "ap1",
                        Encrypted = true,
                    };
                    var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                    var result = pusher.TriggerAsync("amsems", FormGuidanceNavigation.id, new { message = "new notification" });
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
