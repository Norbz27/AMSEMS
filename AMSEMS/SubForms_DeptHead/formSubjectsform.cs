using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formSubjectsForm : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        String Pass;

        String choice;
        bool istrue = false;
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random RandomGenerator = new Random();

        formSubjects form;

        public formSubjectsForm()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

        }
        public void setData(String choice, formSubjects form)
        {
            this.form = form;
            this.choice = choice;
        }

        private void formStudentForm_Load(object sender, EventArgs e)
        {
            cbTeacher.Items.Clear();
            if (choice.Equals("Update"))
            {
                tbCcode.Enabled = false;

            }
            else
            {
                tbCcode.Enabled = true;
                tbStatus.Text = "Active";
            }

            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Lastname from tbl_teacher_accounts", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbTeacher.Items.Add(dr["Lastname"].ToString());
                }
                dr.Close();
                cn.Close();

            }

            btnSubmit.Text = choice;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbCourseDes.Text == "" && tbCcode.Text == "" && tbUnits.Text == "" && cbAcadLevel.Text == "" && cbTeacher.Text == "")
                {
                    MessageBox.Show("Empty Fields!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    if (btnSubmit.Text.Equals("Update"))
                    {
                        cm = new SqlCommand("SELECT Image FROM tbl_subjects WHERE Course_code = @Course_Code", cn);
                        cm.Parameters.AddWithValue("@Course_Code", tbCcode.Text);

                        ad = new SqlDataAdapter(cm);
                        DataSet ds = new DataSet();
                        ad.Fill(ds);

                        byte[] picData = null;

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            // Read the image data from the retrieved row
                            picData = (byte[])ds.Tables[0].Rows[0]["Image"];
                        }

                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }

                        cn.Open();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "sp_UpdateSubjects";
                        cm.Parameters.AddWithValue("@Course_Code", tbCcode.Text);
                        cm.Parameters.AddWithValue("@Course_Description", tbCourseDes.Text);
                        cm.Parameters.AddWithValue("@Image", picData);
                        cm.Parameters.AddWithValue("@Teach", cbTeacher.Text);
                        cm.Parameters.AddWithValue("@AcadLevel", cbAcadLevel.Text);
                        cm.Parameters.AddWithValue("@Units", tbUnits.Text);
                        cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Subject Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ds.Tables[0].Rows.Clear();
                    }
                    else
                    {
                        byte[] picData;
                        if (openFileDialog1.FileName == String.Empty)
                        {
                            picData = null;
                        }
                        else
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }
                        cm = new SqlCommand("Select Course_Code from tbl_subjects where Course_Code = '" + tbCcode.Text + "'", cn);
                        ad = new SqlDataAdapter(cm);
                        ad.Fill(ds);
                        int i = ds.Tables[0].Rows.Count;

                        if (i != 0)
                        {
                            MessageBox.Show("A Subject is already Present!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ds.Tables[0].Rows.Clear();
                        }
                        else
                        {
                            cn.Open();
                            cm = new SqlCommand();
                            cm.Connection = cn;
                            cm.CommandType = CommandType.StoredProcedure;
                            cm.CommandText = "sp_AddSubjects";
                            cm.Parameters.AddWithValue("@Course_Code", tbCcode.Text);
                            cm.Parameters.AddWithValue("@Course_Description", tbCourseDes.Text);
                            cm.Parameters.AddWithValue("@Image", picData);
                            cm.Parameters.AddWithValue("@Teach", cbTeacher.Text);
                            cm.Parameters.AddWithValue("@AcadLevel", cbAcadLevel.Text);
                            cm.Parameters.AddWithValue("@Units", tbUnits.Text);
                            cm.Parameters.AddWithValue("@Status", tbStatus.Text);
                            cm.ExecuteNonQuery();
                            cn.Close();
                            MessageBox.Show("Subject Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ds.Tables[0].Rows.Clear();
                        }
                    }
                    form.displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1 AND al.Academic_Level_Description = @acadlevel");
                }
            }
        }
        public void clearTexts()
        {
            tbCcode.Text = "";
            tbCourseDes.Text = "";
            tbUnits.Text = "";
            tbStatus.Text = "";
            cbTeacher.Text = "";
            cbAcadLevel.Text = "";
            ptbImage.Image = global::AMSEMS.Properties.Resources.book1;
        }

        public void getStudID(String Course_code)
        {
            using (cn)
            {
                try
                {
                    cn.Open();
                    cm = new SqlCommand("Select Image from tbl_subjects where Course_code = '" + Course_code + "'", cn);

                    byte[] imageData = (byte[])cm.ExecuteScalar();

                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);
                            ptbImage.Image = image;
                        }
                    }
                    cn.Close();
                    cn.Open();
                    cm = new SqlCommand("Select Course_code,Course_Description,Units,t.Lastname as teach,st.Description as stDes, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID where s.Course_code = '" + Course_code + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    tbCcode.Text = dr["Course_code"].ToString();
                    tbCourseDes.Text = dr["Course_Description"].ToString();
                    tbUnits.Text = dr["Units"].ToString();
                    cbTeacher.Text = dr["teach"].ToString();
                    cbAcadLevel.Text = dr["Acad"].ToString();
                    tbStatus.Text = dr["stDes"].ToString();

                    dr.Close();
                    cn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            ptbImage.Image = Image.FromFile(openFileDialog1.FileName);
        }

        private void cbTeacher_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void formSubjectsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.displayFilter();
            form.loadCMSControls();
        }
    }
}
