﻿
using ComponentFactory.Krypton.Toolkit;
using PusherServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS.SubForms_SAO
{
    public partial class formEventDetails : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        UserControlDays_Calendar form;
        formEvents form1;
        string selectedColor;
        bool isTrue;
        string eventid;
        formEventEditConfig formEventEditConfig;

        public static bool attendance;
        public static bool penalty;
        public static HashSet<string> selected = new HashSet<string> { };
        public static string exclusive;
        public static bool change = false;
        public formEventDetails()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            isTrue = true;
            formEventEditConfig = new formEventEditConfig(this);
            DtEnd.MinDate = DtStart.Value;

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnConfig, "Event Configuration");
            toolTip.SetToolTip(btnDelete, "Delete Event");
        }
        public void getForm(UserControlDays_Calendar form)
        {
            this.form = form;
        }
        public void getForm2(formEvents form1)
        {
            this.form1 = form1;
        }
        public void change2()
        {
            btnCancel.Visible = true;
            btnDone.Visible = true;
        }
        private void btnDone_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbEventName.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    using (cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        byte[] picData = null;
                        DateTime startDate = DtStart.Value;

                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }
                        string selectedStudents = GetSelectedAsString();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "sp_UpdateEvent";
                        cm.Parameters.AddWithValue("@Event_ID", eventid);
                        cm.Parameters.AddWithValue("@Event_Name", tbEventName.Text);
                        cm.Parameters.AddWithValue("@Start_Date", DtStart.Value);
                        cm.Parameters.AddWithValue("@End_Date", DtEnd.Value);
                        cm.Parameters.AddWithValue("@Image", picData);
                        cm.Parameters.AddWithValue("@Description", tbDescription.Text);
                        cm.Parameters.AddWithValue("@Color", selectedColor);
                        cm.Parameters.AddWithValue("@Attendance", attendance);
                        cm.Parameters.AddWithValue("@Penalty", penalty);

                        if (exclusive.Equals("Specific Students"))
                        {
                            cm.Parameters.AddWithValue("@Exclusive", exclusive);
                            cm.Parameters.AddWithValue("@Specific_Students", selectedStudents);
                        }
                        else if (exclusive.Equals("Selected Departments"))
                        {
                            cm.Parameters.AddWithValue("@Exclusive", exclusive);
                            cm.Parameters.AddWithValue("@Selected_Departments", selectedStudents);
                        }
                        else
                        {
                            cm.Parameters.AddWithValue("@Exclusive", exclusive);
                            cm.Parameters.AddWithValue("@Specific_Students", null);
                            cm.Parameters.AddWithValue("@Selected_Departments", null);
                        }

                        cm.ExecuteNonQuery();

                        MessageBox.Show("Event Information Updated!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cn.Close();
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
        //private bool IsValidDate(int year, int month, int day)
        //{
        //    try
        //    {
        //        DateTime date = new DateTime(year, month, day);
        //        return true; // The date is valid.
        //    }
        //    catch (ArgumentOutOfRangeException)
        //    {
        //        return false; // The date is not valid.
        //    }
        //}
        private void formAddEvent_Load(object sender, EventArgs e)
        {
            
        }
        public void displayDetails(string eventid)
        {
            this.eventid = eventid;
            formEventEditConfig.getEventID(eventid);
            formEventEditConfig.displayConfig();
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                using (cm = new SqlCommand("SELECT * FROM tbl_events WHERE Event_ID = " + eventid, cn))
                using (dr = cm.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        tbEventName.Text = dr["Event_Name"].ToString();
                        tbDescription.Text = dr["Description"].ToString();
                        DtStart.Value = (DateTime)dr["Start_Date"];
                        DtEnd.Value = (DateTime)dr["End_Date"];
                        lblCode.Text = dr["Event_ID"].ToString();
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


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this event?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string deleteQuery = "DELETE FROM tbl_events WHERE Event_ID = @ID";

                        using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                        {
                            // Add parameter for the primary key value
                            command.Parameters.AddWithValue("@ID", eventid);
                            command.ExecuteNonQuery();

                            var option = new PusherOptions
                            {
                                Cluster = "ap1",
                                Encrypted = true,
                            };
                            var pusher = new Pusher("1732969", "6cc843a774ea227a754f", "de6683c35f58d7bc943f", option);

                            var result = pusher.TriggerAsync("amsems", "events", new { message = "new notification" });
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
        private string GetSelectedAsString()
        {
            if (selected != null)
            {
                HashSet<string> selectedStudentIds = selected;

                return string.Join(",", selectedStudentIds);
            }
            else
            {
                return null;
            }
        }
        private void btnConfig_Click(object sender, EventArgs e)
        {
            formEventEditConfig.ShowDialog();
        }

        private void DtStart_ValueChanged(object sender, EventArgs e)
        {
            DtEnd.MinDate = DtStart.Value;
        }
    }
}
