
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class formEventDetails : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        string selectedColor;
        bool isTrue;
        string eventid;

        public static bool attendance;
        public static bool penalty;
        public static HashSet<string> selected = new HashSet<string> { };
        public static string exclusive;
        public static bool change = false;
        FormAdmissionNavigation form;
        public formEventDetails()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            isTrue = true;
            DtEnd.MinDate = DtStart.Value;

        }
        public void getForm(FormAdmissionNavigation form)
        {
            this.form = form;
        }
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            
        }
        private void formAddEvent_Load(object sender, EventArgs e)
        {
            
        }
        public void displayDetails(string eventid)
        {
            this.eventid = eventid;
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
        private void DtStart_ValueChanged(object sender, EventArgs e)
        {
            DtEnd.MinDate = DtStart.Value;
        }
    }
}
