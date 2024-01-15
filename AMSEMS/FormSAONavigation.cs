using ComponentFactory.Krypton.Toolkit;
using PusherClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class FormSAONavigation : KryptonForm
    {
        SqlConnection cnn;
        SqlCommand cm;
        SqlDataReader dr;

        public bool isCollapsed;
        private Form activeForm;
        public static String id;
        private Pusher pusher;
        private Channel channel;
        public FormSAONavigation(String id1)
        {
            InitializeComponent();

            cnn = new SqlConnection(SQL_Connection.connection);

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            setCalendar();
            SubForms_SAO.formDashboard.setForm(this);
            this.kryptonSplitContainer1.Panel2Collapsed = false;
            OpenChildForm(new SubForms_SAO.formDashboard(id1));

            id = id1;

            notification();
        }
        public async void notification()
        {
            pusher = new Pusher("6cc843a774ea227a754f", new PusherOptions()
            {
                Cluster = "ap1",
                Encrypted = true
            });

            pusher.Error += OnPusherOnError;
            pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            pusher.Connected += PusherOnConnected;
            channel = await pusher.SubscribeAsync("amsems");
            await pusher.ConnectAsync();

            void PusherOnConnectionStateChanged(object sender, ConnectionState state)
            {
                Console.Write("Connection state changed");
            }

            void OnPusherOnError(object s, PusherException e)
            {
                Console.Write("Errored");
            }

            void OnChannelOnSubscribed(object s)
            {
                Console.Write("Subscribed");
            }
            void PusherOnConnected(object sender)
            {
                Console.Write("Connected");
                channel.Bind("notification", (dynamic data) =>
                {
                    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                    {
                        connection.Open();
                        string query = "SELECT TOP 1 Announcement_Title, Date_Time FROM tbl_Announcement ORDER BY Date_Time DESC";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                notifyIcon1.BalloonTipTitle = "Announcement";
                                notifyIcon1.BalloonTipText = reader["Announcement_Title"].ToString();
                                notifyIcon1.ShowBalloonTip(3000);
                            }
                        }
                    }
                });
                channel.Bind("events", (dynamic data) =>
                {
                    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                    {
                        connection.Open();
                        string query = "SELECT TOP 1 Event_Name, Date_Time FROM tbl_events ORDER BY Date_Time DESC";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                reader.Read();
                                notifyIcon1.BalloonTipTitle = "Event";
                                notifyIcon1.BalloonTipText = reader["Event_Name"].ToString();
                                notifyIcon1.ShowBalloonTip(3000);
                                setCalendar();
                                DisplayEventsDetails();
                            }
                        }
                    }
                });
                channel.Bind(FormSAONavigation.id, (dynamic data) =>
                {
                    loadData();
                });
            }
        }
        public void setCalendar()
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                connection.Open();
                string query = "SELECT Start_Date FROM tbl_events";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<DateTime> boldedDates = new List<DateTime>();

                        while (reader.Read())
                        {
                            Object eventDate = reader["Start_Date"];
                            DateTime eventDate2 = (DateTime)eventDate;

                            // Add the date to the list of bolded dates
                            boldedDates.Add(eventDate2);
                        }

                        // Set the BoldedDates property of the KryptonMonthCalendar
                        kryptonMonthCalendar1.BoldedDates = boldedDates.ToArray();
                    }
                }
            }
        }
        public void loadData()
        {
            try
            {
                using (cnn = new SqlConnection(SQL_Connection.connection))
                {
                    cnn.Open(); // Open the connection here

                    cm = new SqlCommand("select Firstname, Lastname, Profile_pic from tbl_sao_accounts where Unique_ID = '" + id + "'", cnn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();

                    object imageData = dr["Profile_pic"];
                    if (imageData != DBNull.Value) // Check if the column is not null
                    {
                        byte[] imageBytes = (byte[])imageData;
                        if (imageBytes.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                ptbProfile.Image = image;
                            }
                        }
                    }
                    dr.Close();
                } // The connection is automatically closed when exiting the using block

            }
            catch (Exception e)
            {

            }
        }



        private void btnDashboard_Click(object sender, EventArgs e)
        {
            this.kryptonSplitContainer1.Panel2Collapsed = false;
            OpenChildForm(new SubForms_SAO.formDashboard(id));
            this.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnAnnouncement.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnEvents.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnEvents.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnEvents.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnAnnouncement_Click(object sender, EventArgs e)
        {
            this.kryptonSplitContainer1.Panel2Collapsed = false;
            OpenChildForm(new SubForms_SAO.formAnnouncement());
            loadData();
            this.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAnnouncement.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAnnouncement.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAnnouncement.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnEvents.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnEvents.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnEvents.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnEvents_Click(object sender, EventArgs e)
        {
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_SAO.formEvents());

            this.btnEvents.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnEvents.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnEvents.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnEvents.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnEvents.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAnnouncement.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_SAO.formSettings(this));

            this.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAnnouncement.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAnnouncement.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAnnouncement.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnEvents.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnEvents.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnEvents.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnEvents.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            childForm.FormBorderStyle = FormBorderStyle.None;
            this.kryptonSplitContainer1.Panel1.Controls.Add(childForm);
            this.kryptonSplitContainer1.Panel1.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void FormSAONavigation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormSAONavigation_Load(object sender, EventArgs e)
        {
            loadData();
            DisplayEventsDetails();
        }
        public void Logout()
        {
            this.Dispose();
            FormLoginPage formLoginPage = new FormLoginPage();
            formLoginPage.Show();
        }

        private void FormSAONavigation_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
        public async void DisplayEventsDetails()
        {
            if (panel7.InvokeRequired)
            {
                panel7.Invoke((MethodInvoker)delegate
                {
                    // Call the same method on the main threadz
                    DisplayEventsDetails();
                });
            }
            else
            {
                panel7.Controls.Clear();
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    using (SqlCommand cm = new SqlCommand("SELECT Event_ID,Event_Name, Color, Start_Date FROM tbl_events WHERE Start_Date > CAST(GETDATE() AS DATE) ORDER BY Start_Date DESC", cn))
                    using (SqlDataReader dr = await cm.ExecuteReaderAsync())
                    {
                        int labelCount = 0;
                        while (dr.Read())
                        {
                            string eventid = dr["Event_ID"].ToString();
                            string eventname = dr["Event_Name"].ToString();
                            string color = dr["Color"].ToString();
                            DateTime date = DateTime.Parse(dr["Start_Date"].ToString());
                            string formattedDate = date.ToString("dddd, MMMM d, yyyy");
                            string day = date.Day.ToString();

                            EventDetailsApperance(eventid, eventname, color, formattedDate, day);

                            labelCount++;
                        }
                    }
                }
            }
        }

        public void EventDetailsApperance(string eventid, string eventname, string color, string date, string day)
        {
            Color Backcolor = ColorTranslator.FromHtml(color);
            KryptonGroupBox kryptonGroupBox6 = new KryptonGroupBox();
            Panel panel12 = new Panel();
            //KryptonLabel kryptonLabel4 = new KryptonLabel();
            KryptonLabel kryptonLabel2 = new KryptonLabel();
            KryptonLabel kryptonLabel11 = new KryptonLabel();
            KryptonLabel kryptonLabel10 = new KryptonLabel();

            this.panel7.Controls.Add(kryptonGroupBox6);
            this.panel7.Controls.Add(panel12);
            kryptonGroupBox6.CaptionVisible = false;
            kryptonGroupBox6.CausesValidation = false;

            //kryptonGroupBox6.Panel.Controls.Add(kryptonLabel4);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel2);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel11);
            kryptonGroupBox6.Panel.Controls.Add(kryptonLabel10);
            kryptonGroupBox6.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonGroupBox6.Dock = DockStyle.Top;
            kryptonGroupBox6.Margin = new System.Windows.Forms.Padding(5);
            kryptonGroupBox6.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            kryptonGroupBox6.Size = new System.Drawing.Size(208, 92);
            kryptonGroupBox6.StateCommon.Back.Color1 = Backcolor;
            kryptonGroupBox6.StateCommon.Back.Color2 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.Color1 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.Color2 = Backcolor;
            kryptonGroupBox6.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            kryptonGroupBox6.StateCommon.Border.Rounding = 10;
            kryptonGroupBox6.StateCommon.Border.Width = 1;

            kryptonLabel2.Location = new System.Drawing.Point(15, 8);
            kryptonLabel2.Name = "kryptonLabel2";
            kryptonLabel2.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel2.Size = new System.Drawing.Size(44, 39);
            kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 18F, System.Drawing.FontStyle.Bold);
            kryptonLabel2.Values.Text = day;

            kryptonLabel11.Location = new System.Drawing.Point(57, 16);
            kryptonLabel11.AutoSize = false;
            kryptonLabel11.Name = "kryptonLabel11";
            kryptonLabel11.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel11.Size = new System.Drawing.Size(170, 23);
            kryptonLabel11.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel11.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel11.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F, System.Drawing.FontStyle.Bold);
            kryptonLabel11.Values.Text = eventname;

            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Default;
            kryptonLabel10.Location = new System.Drawing.Point(15, 53);
            kryptonLabel10.Name = "kryptonLabel10";
            kryptonLabel10.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel10.Size = new System.Drawing.Size(117, 19);
            kryptonLabel10.StateCommon.Draw = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            kryptonLabel10.StateCommon.Image.ImageH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            kryptonLabel10.StateCommon.LongText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.LongText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.LongText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel10.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonLabel10.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 8F);
            kryptonLabel10.StateCommon.ShortText.MultiLine = ComponentFactory.Krypton.Toolkit.InheritBool.True;
            kryptonLabel10.StateCommon.ShortText.MultiLineH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.StateCommon.ShortText.TextV = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Near;
            kryptonLabel10.Values.Text = date;

            panel12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            panel12.Dock = System.Windows.Forms.DockStyle.Top;
            panel12.Location = new System.Drawing.Point(0, 0);
            panel12.Name = "panel12";
            panel12.Cursor = System.Windows.Forms.Cursors.Hand;
            panel12.Size = new System.Drawing.Size(234, 10);
            panel12.TabIndex = 19;

            kryptonGroupBox6.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonGroupBox6.Panel.Click += (s, e) => KryptonGroupBox_Click(eventid);

            panel12.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel10.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel11.Click += (s, e) => KryptonGroupBox_Click(eventid);
            kryptonLabel2.Click += (s, e) => KryptonGroupBox_Click(eventid);

            this.panel7.Invoke((MethodInvoker)delegate
            {
                // Inside this block, you can update UI controls safely
                this.panel7.Controls.Add(kryptonGroupBox6);
                this.panel7.Controls.Add(panel12);
            });
        }
        private void KryptonGroupBox_Click(string eventid)
        {
            formEventDetails formEventDetails = new formEventDetails();
            formEventDetails.displayDetails(eventid);
            formEventDetails.ShowDialog();
        }
    }
}
