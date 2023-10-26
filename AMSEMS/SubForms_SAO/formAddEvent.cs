
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace AMSEMS.SubForms_SAO
{
    public partial class formAddEvent : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        UserControlDays_Calendar form;

        public formAddEvent()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

        }
        public void getForm(UserControlDays_Calendar form)
        {
            this.form = form;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbEventName.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        byte[] picData = null;
                        DateTime startDate = DtStart.Value;
                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }

                        cn.Open();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "sp_AddEvent";
                        cm.Parameters.AddWithValue("@Event_Name", tbEventName.Text);
                        cm.Parameters.AddWithValue("@Start_Date", DtStart.Value);
                        cm.Parameters.AddWithValue("@End_Date", DtEnd.Value);
                        cm.Parameters.AddWithValue("@Image", picData);
                        cm.Parameters.AddWithValue("@Description", tbDescription.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Event Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {

                        form.refresh();
                        this.Dispose();
                    }
                }
            }

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            lblUpload.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            lblUpload.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
        }

        private void lblUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void formAddEvent_Load(object sender, EventArgs e)
        {
            DateTime selectedDate = new DateTime(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day);
            DtStart.Value = selectedDate;
            DtEnd.Value = selectedDate;
        }

        private void btnColorMarron_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
