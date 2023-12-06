using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public async void displayAnnouncements(string searchKeyword, DateTime filterDate)
        {
            panelAnnouncements.Controls.Clear();
            ptbLoading.Visible = true;
            await Task.Delay(1000);
            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Announcement_ID, Announcement_Title, Announcement_Description, Date_Time, Announce_By FROM tbl_Announcement WHERE 1 = 1";

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
                        string id = dr["Announcement_ID"].ToString();
                        string title = dr["Announcement_Title"].ToString();
                        string description = dr["Announcement_Description"].ToString();
                        string announceby = dr["Announce_By"].ToString();
                        DateTime datetime = DateTime.Parse(dr["Date_Time"].ToString());
                        string formattedDate = datetime.ToString("dddd, MMMM d, yyyy, h:mm tt");

                        announcementApperance(id, title, description, formattedDate, announceby);

                        labelCount++;
                    }
                }
            }
            ptbLoading.Visible = false;
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

        public void announcementApperance(string id, string title, string description, string dateTime, string announceby)
        {
            Panel panel12 = new Panel();
            KryptonGroupBox kryptonGroupBox4 = new KryptonGroupBox();
            KryptonLabel kryptonLabel9 = new KryptonLabel();
            KryptonLabel kryptonLabel10 = new KryptonLabel();
            //KryptonLabel kryptonLabel6 = new KryptonLabel();
            RichTextBox richTextBox1 = new RichTextBox();
            KryptonButton btnDelete = new KryptonButton();
            KryptonButton btnEdit = new KryptonButton();

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

            btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            btnDelete.Location = new System.Drawing.Point(705, 10);
            btnDelete.Name = "btnDelete";
            btnDelete.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.OverrideDefault.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnDelete.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            btnDelete.OverrideDefault.Border.Width = 1;
            btnDelete.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnDelete.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnDelete.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnDelete.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.Size = new System.Drawing.Size(36, 34);
            btnDelete.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnDelete.StateCommon.Border.Rounding = 10;
            btnDelete.StateCommon.Border.Width = 1;
            btnDelete.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            btnDelete.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnDelete.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnDelete.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            btnDelete.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            btnDelete.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnDelete.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnDelete.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnDelete.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnDelete.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btnDelete.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btnDelete.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnDelete.StateTracking.Border.Rounding = 10;
            btnDelete.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnDelete.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnDelete.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnDelete.TabIndex = 153;
            btnDelete.Values.Image = global::AMSEMS.Properties.Resources.delete_16;
            btnDelete.Values.Text = "";

            btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            btnEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            btnEdit.Location = new System.Drawing.Point(666, 10);
            btnEdit.Name = "btnEdit";
            btnEdit.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.OverrideDefault.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnEdit.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            btnEdit.OverrideDefault.Border.Width = 1;
            btnEdit.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnEdit.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnEdit.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnEdit.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.Size = new System.Drawing.Size(36, 34);
            btnEdit.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnEdit.StateCommon.Border.Rounding = 10;
            btnEdit.StateCommon.Border.Width = 1;
            btnEdit.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            btnEdit.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnEdit.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnEdit.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            btnEdit.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            btnEdit.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnEdit.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnEdit.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnEdit.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnEdit.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btnEdit.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            btnEdit.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            btnEdit.StateTracking.Border.Rounding = 10;
            btnEdit.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            btnEdit.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            btnEdit.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            btnEdit.TabIndex = 153;
            btnEdit.Values.Image = global::AMSEMS.Properties.Resources.edit1;
            btnEdit.Values.Text = "";

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

            panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            panel12.Dock = System.Windows.Forms.DockStyle.Top;
            panel12.Location = new System.Drawing.Point(0, 0);
            panel12.Name = "panel12";
            panel12.Cursor = System.Windows.Forms.Cursors.Hand;
            panel12.Size = new System.Drawing.Size(234, 10);
            panel12.TabIndex = 19;

            btnDelete.Click += (s, e) => btnDelete_Click(id);
            btnEdit.Click += (s, e) => btnEdit_Click(id);

            kryptonGroupBox4.Panel.Controls.Add(btnDelete);
            kryptonGroupBox4.Panel.Controls.Add(btnEdit);
            kryptonGroupBox4.Panel.Controls.Add(richTextBox1);
            kryptonGroupBox4.Panel.Controls.Add(kryptonLabel10);
            //kryptonGroupBox4.Panel.Controls.Add(kryptonLabel6);
            kryptonGroupBox4.Panel.Controls.Add(kryptonLabel9);


            panelAnnouncements.Controls.Add(kryptonGroupBox4);
            panelAnnouncements.Controls.Add(panel12);
        }
        private void btnDelete_Click(string id)
        {
            if (MessageBox.Show("Are you sure you want to delete this Announcement?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                    {
                        cn.Open();
                        string deleteQuery = "DELETE FROM tbl_Announcement WHERE Announcement_ID = @ID";

                        using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                        {
                            // Add parameter for the primary key value
                            command.Parameters.AddWithValue("@ID", id);
                            command.ExecuteNonQuery();

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
                    displayAnnouncements(searchKeyword, filterDate);
                }
            }
        }
        private void btnEdit_Click(string id)
        {
            formEditAnnouncement formEditAnnouncement = new formEditAnnouncement();
            formEditAnnouncement.getForm(this);
            formEditAnnouncement.dispayInfo(id);
            formEditAnnouncement.ShowDialog();
        }
    }
}
