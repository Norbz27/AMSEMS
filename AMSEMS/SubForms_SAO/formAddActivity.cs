
using ComponentFactory.Krypton.Toolkit;
using Org.BouncyCastle.Ocsp;
using PusherServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AMSEMS.SubForms_SAO
{
    public partial class formAddActivity : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        UserControlDays_Calendar form;
        formEvents form1;
        string selectedColor;
        public static bool attendance = false;
        public static bool penalty = false;
        public static List<string> selected = new List<string> { };
        public static string exclusive = "All Students";

        formEventAddConfig formEventConfig;
        private string schYear;
        bool istrue;
        string act_id;
        public formAddActivity()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            selectedColor = "#800000";
            SetButtonAppearance(btnColorMarron);
            formEventConfig = new formEventAddConfig();

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "";

                query = "SELECT Acad_ID FROM tbl_acad WHERE Status = 1";
                using (SqlCommand cm = new SqlCommand(query, cn))
                {
                    using (SqlDataReader dr = cm.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            schYear = dr["Acad_ID"].ToString();
                        }
                    }
                }
            }
        }
        
        public void getForm(UserControlDays_Calendar form, bool istrue)
        {
            this.form = form;
            this.istrue = istrue;
        }
        public void getForm2(formEvents form1, bool istrue)
        {
            this.form1 = form1;
            this.istrue = istrue;
        }
        public void displayInformation(string act_id)
        {
            lblHeader.Text = "Activity Information";
            lblHeader2.Text = "";
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                using (cm = new SqlCommand("SELECT * FROM tbl_activities WHERE Activity_ID = " + act_id, cn))
                using (dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        this.act_id = act_id;
                        tbActivityName.Text = dr["Activity_Name"].ToString();
                        tbDescription.Text = dr["Description"].ToString();
                        DtStart.Value = (DateTime)dr["Date"];
                        DtTime.Value = DateTime.Parse(dr["Time"].ToString());
                        selectedColor = dr["Color"].ToString();

                        object imageData = dr["Image"];
                        if (imageData != DBNull.Value) // Check if the column is not null
                        {
                            byte[] imageBytes = (byte[])imageData;
                            if (imageBytes.Length > 0)
                            {
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    Image image = Image.FromStream(ms);
                                    pictureBox1.Image = image;
                                }
                            }
                        }


                        if (dr["Color"].ToString().Equals("#800000"))
                        {
                            SetButtonAppearance(btnColorMarron);
                            SetButtonAppearance2(btnColorGreen);
                            SetButtonAppearance2(btnColorBlue);
                            SetButtonAppearance2(btnColorPurple);
                            SetButtonAppearance2(btnColorOrange);
                        }
                        else if (dr["Color"].ToString().Equals("#009600"))
                        {
                            SetButtonAppearance2(btnColorMarron);
                            SetButtonAppearance(btnColorGreen);
                            SetButtonAppearance2(btnColorBlue);
                            SetButtonAppearance2(btnColorPurple);
                            SetButtonAppearance2(btnColorOrange);
                        }
                        else if (dr["Color"].ToString().Equals("#07006E"))
                        {
                            SetButtonAppearance2(btnColorMarron);
                            SetButtonAppearance2(btnColorGreen);
                            SetButtonAppearance(btnColorBlue);
                            SetButtonAppearance2(btnColorPurple);
                            SetButtonAppearance2(btnColorOrange);
                        }
                        else if (dr["Color"].ToString().Equals("#6E002B"))
                        {
                            SetButtonAppearance2(btnColorMarron);
                            SetButtonAppearance2(btnColorGreen);
                            SetButtonAppearance2(btnColorBlue);
                            SetButtonAppearance(btnColorPurple);
                            SetButtonAppearance2(btnColorOrange);
                        }
                        else if (dr["Color"].ToString().Equals("#A63C00"))
                        {
                            SetButtonAppearance2(btnColorMarron);
                            SetButtonAppearance2(btnColorGreen);
                            SetButtonAppearance2(btnColorBlue);
                            SetButtonAppearance2(btnColorPurple);
                            SetButtonAppearance(btnColorOrange);
                        }
                    }
                }
            }
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbActivityName.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        byte[] picData = null;
                        DateTime startDate = DtStart.Value;
                        DateTime dateTime = DateTime.Now;
                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }
                        string selected = GetSelectedAsString();
                        cn.Open();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        if (istrue == true)
                        {
                            cm.CommandText = "sp_AddActivity";
                        }
                        else
                        {
                            cm.CommandText = "sp_UpdateActivity";
                            cm.Parameters.AddWithValue("@Activity_ID", act_id);
                        }
                      
                        cm.Parameters.AddWithValue("@Activity_Name", tbActivityName.Text);
                        cm.Parameters.AddWithValue("@Date", DtStart.Value);
                        cm.Parameters.AddWithValue("@Time", DtTime.Value);
                        cm.Parameters.AddWithValue("@Image", picData);
                        cm.Parameters.AddWithValue("@Description", tbDescription.Text);
                        cm.Parameters.AddWithValue("@Color", selectedColor);
                        cm.Parameters.AddWithValue("@DateTime", dateTime);
                        cm.Parameters.AddWithValue("@SchYear", schYear);
                                      
                        cm.ExecuteNonQuery();
                        cn.Close();
                        var option = new PusherOptions
                        {
                            Cluster = "ap1",
                            Encrypted = true,
                        };
                        var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                        var result = pusher.TriggerAsync("amsems", "activity", new { message = "new notification" });
                        if (istrue == true)
                        {
                            MessageBox.Show("Activity Created!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Activity Updated!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                         MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                    if (form != null)
                    {
                        form.refresh();
                    }
                    else
                    {
                        form1.RefreshCalendar();
                    }
                    this.Dispose();
                }
            }
            }

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            ptbUpload.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            ptbUpload.Visible = false;
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
        private bool IsValidDate(int year, int month, int day)
        {
            try
            {
                DateTime date = new DateTime(year, month, day);
                return true; // The date is valid.
            }
            catch (ArgumentOutOfRangeException)
            {
                return false; // The date is not valid.
            }
        }
        private void formAddEvent_Load(object sender, EventArgs e)
        {
            if (IsValidDate(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day))
            {
                DateTime selectedDate = new DateTime(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day);
                DtStart.Value = selectedDate;
            }
            else
            {

            }
        }
        private void SetButtonAppearance(KryptonButton button)
        {
            button.StateCommon.Back.Color1 = System.Drawing.Color.Silver;
            button.StateCommon.Back.Color2 = System.Drawing.Color.Silver;
            button.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            button.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            button.StateCommon.Border.Width = 1;
            button.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            button.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            button.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }

        private void SetButtonAppearance2(KryptonButton button)
        {
            button.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            button.StateCommon.Border.Rounding = 10;
            button.StateCommon.Border.Width = 1;
            button.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            button.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            button.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }

        private void btnColorMarron_Click(object sender, EventArgs e)
        {
            selectedColor = "#800000";
            SetButtonAppearance(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorGreen_Click(object sender, EventArgs e)
        {
            selectedColor = "#009600";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorBlue_Click(object sender, EventArgs e)
        {
            selectedColor = "#07006E";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorPurple_Click(object sender, EventArgs e)
        {
            selectedColor = "#6E002B";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorOrange_Click(object sender, EventArgs e)
        {
            selectedColor = "#A63C00";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance(btnColorOrange);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            formEventConfig.ShowDialog();
        }

        private string GetSelectedAsString()
        {
            if(selected != null)
            {
                List<string> selectedNamesAndDep = selected;

                return string.Join(",", selectedNamesAndDep);
            }
            else
            {
                return null;
            }
        }

        private void DtStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Activity?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string deleteQuery = "DELETE FROM tbl_activities WHERE Activity_ID = @ID";

                        using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                        {
                            // Add parameter for the primary key value
                            command.Parameters.AddWithValue("@ID", act_id);
                            command.ExecuteNonQuery();

                            var option = new PusherOptions
                            {
                                Cluster = "ap1",
                                Encrypted = true,
                            };
                            var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                            var result = pusher.TriggerAsync("amsems", "activity", new { message = "new notification" });
                            MessageBox.Show("Deleted successfully.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (form != null)
                    {
                        form.refresh();
                    }
                    else
                    {
                        form1.RefreshCalendar();
                    }
                    this.Dispose();
                }
            }
        }
    }
}
