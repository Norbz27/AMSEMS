using AMSEMS.Properties;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace AMSEMS.SubForms_SAO
{
    public partial class formAnnouncement : Form
    {
        private string searchKeyword = string.Empty;
        private DateTime filterDate = DateTime.MinValue;
        private KryptonGroupBox[] announcements;
        public formAnnouncement()
        {
            InitializeComponent();
        }

        private void btnAnnounce_Click(object sender, EventArgs e)
        {
            formAddAnnouncement formAddAnnouncement = new formAddAnnouncement();
            formAddAnnouncement.getForm(this);
            formAddAnnouncement.ShowDialog();
        }

        public void displayAnnouncements(string searchKeyword, DateTime filterDate)
        {
            panelAnnouncements.Controls.Clear();

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Announcement_Title, Announcement_Description, Date_Time, Announce_By FROM tbl_Announcement WHERE 1 = 1";

                if (!string.IsNullOrEmpty(searchKeyword))
                {
                    query += $" AND (Announcement_Title LIKE '%{searchKeyword}%' OR Announcement_Description LIKE '%{searchKeyword}%')";
                }

                if (filterDate != DateTime.MinValue)
                {
                    query += $" AND Date_Time >= '{filterDate.ToString("yyyy-MM-dd HH:mm:ss")}'";
                }

                using (SqlCommand cm = new SqlCommand(query, cn))
                using (SqlDataReader dr = cm.ExecuteReader())
                {
                    announcements = new KryptonGroupBox[0];

                    int labelCount = 0;
                    while (dr.Read())
                    {
                        string title = dr["Announcement_Title"].ToString();
                        string description = dr["Announcement_Description"].ToString();
                        string announceby = dr["Announce_By"].ToString();
                        DateTime datetime = DateTime.Parse(dr["Date_Time"].ToString());
                        string formattedDate = datetime.ToString("dddd, MMMM d, yyyy, h:mm tt");

                        announcementApperance(title, description, formattedDate, announceby);

                        labelCount++;
                    }
                }
            }
        }

        private void formAnnouncement_Load(object sender, EventArgs e)
        {
            displayAnnouncements(searchKeyword, filterDate);
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            searchKeyword = tbSearch.Text;
            displayAnnouncements(searchKeyword, filterDate);
        }

        private void DtEnd_ValueChanged(object sender, EventArgs e)
        {
            filterDate = Dt.Value;
            displayAnnouncements(searchKeyword, filterDate);
        }

        public void announcementApperance(string title, string description, string dateTime, string announceby)
        {
            Panel panel12 = new Panel();
            KryptonGroupBox kryptonGroupBox4 = new KryptonGroupBox();
            KryptonLabel kryptonLabel9 = new KryptonLabel();
            KryptonLabel kryptonLabel10 = new KryptonLabel();
            //KryptonLabel kryptonLabel6 = new KryptonLabel();
            RichTextBox richTextBox1 = new RichTextBox();

            kryptonGroupBox4.Dock = System.Windows.Forms.DockStyle.Top; // Use Dock property to fill the entire width
            kryptonGroupBox4.CaptionStyle = ComponentFactory.Krypton.Toolkit.LabelStyle.GroupBoxCaption;
            kryptonGroupBox4.CaptionVisible = false;
            kryptonGroupBox4.CausesValidation = false;
            kryptonGroupBox4.Name = "kryptonGroupBox4";
            kryptonGroupBox4.AutoSize = true;
            // 
            // kryptonGroupBox4.Panel
            // 
            kryptonGroupBox4.Panel.AutoScroll = true;
            kryptonGroupBox4.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            kryptonGroupBox4.Size = new System.Drawing.Size(760, 194);
            kryptonGroupBox4.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox4.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox4.StateCommon.Back.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            kryptonGroupBox4.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox4.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox4.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            kryptonGroupBox4.StateCommon.Border.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            kryptonGroupBox4.StateCommon.Border.Rounding = 10;
            kryptonGroupBox4.StateCommon.Border.Width = 2;
            kryptonGroupBox4.TabIndex = 24;

            kryptonLabel9.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonLabel9.Location = new System.Drawing.Point(15, 10);
            kryptonLabel9.Name = "kryptonLabel9";
            kryptonLabel9.Size = new System.Drawing.Size(720, 25);
            kryptonLabel9.StateCommon.Padding = new System.Windows.Forms.Padding(-2, -1, -1, -1);
            kryptonLabel9.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            kryptonLabel9.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            kryptonLabel9.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 11F, System.Drawing.FontStyle.Bold);
            kryptonLabel9.StateCommon.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            kryptonLabel9.StateCommon.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            kryptonLabel9.TabIndex = 4;
            kryptonLabel9.Values.Text = title;

            //kryptonLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            //kryptonLabel6.Cursor = System.Windows.Forms.Cursors.Hand;
            //kryptonLabel6.Location = new System.Drawing.Point(634, 13);
            //kryptonLabel6.Name = "kryptonLabel6";
            //kryptonLabel6.Size = new System.Drawing.Size(102, 21);
            //kryptonLabel6.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            //kryptonLabel6.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            //kryptonLabel6.StateCommon.ShortText.Color1 = System.Drawing.Color.DarkGray;
            //kryptonLabel6.StateCommon.ShortText.Color2 = System.Drawing.Color.DarkGray;
            //kryptonLabel6.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            //kryptonLabel6.StateCommon.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            //kryptonLabel6.StateCommon.ShortText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            //kryptonLabel6.TabIndex = 5;
            //kryptonLabel6.Values.Image = global::AMSEMS.Properties.Resources.right_arrow__1_;
            //kryptonLabel6.Values.Text = "View Details";

            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Default;
            kryptonLabel10.Dock = System.Windows.Forms.DockStyle.Top;
            kryptonLabel10.Location = new System.Drawing.Point(15, 35);
            kryptonLabel10.Name = "kryptonLabel10";
            kryptonLabel10.Size = new System.Drawing.Size(720, 19);
            kryptonLabel10.StateCommon.Padding = new System.Windows.Forms.Padding(-2, -1, -1, -1);
            kryptonLabel10.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            kryptonLabel10.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            kryptonLabel10.StateCommon.LongText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            kryptonLabel10.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.LongText.Trim = ComponentFactory.Krypton.Toolkit.PaletteTextTrim.Inherit;
            kryptonLabel10.StateCommon.ShortText.Color1 = System.Drawing.Color.Gray;
            kryptonLabel10.StateCommon.ShortText.Color2 = System.Drawing.Color.Gray;
            kryptonLabel10.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            kryptonLabel10.StateCommon.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.Inherit;
            kryptonLabel10.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.TabIndex = 8;
            kryptonLabel10.Values.Text = dateTime + " by " + announceby;

            richTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            richTextBox1.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            richTextBox1.Location = new System.Drawing.Point(19, 57);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            //richTextBox1.Size = new System.Drawing.Size(720, 117);
            richTextBox1.Width = 720;
            richTextBox1.TabIndex = 25;
            richTextBox1.Text = description;
            richTextBox1.WordWrap = true;

            richTextBox1.ContentsResized += (sender, e) =>
            {
                // Adjust the height of the RichTextBox based on its content
                richTextBox1.Height = e.NewRectangle.Height;
            };

            panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            panel12.Dock = System.Windows.Forms.DockStyle.Top;
            panel12.Location = new System.Drawing.Point(0, 0);
            panel12.Name = "panel12";
            panel12.Cursor = System.Windows.Forms.Cursors.Hand;
            panel12.Size = new System.Drawing.Size(234, 10);
            panel12.TabIndex = 19;

            kryptonGroupBox4.Panel.Controls.Add(richTextBox1);
            kryptonGroupBox4.Panel.Controls.Add(kryptonLabel10);
            //kryptonGroupBox4.Panel.Controls.Add(kryptonLabel6);
            kryptonGroupBox4.Panel.Controls.Add(kryptonLabel9);

            panelAnnouncements.Controls.Add(kryptonGroupBox4);
            panelAnnouncements.Controls.Add(panel12);
        }

        
    }
}
