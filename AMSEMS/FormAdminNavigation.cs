﻿using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS
{
    public partial class FormAdminNavigation : KryptonForm
    {
        SqlCommand cm;
        SqlDataReader dr;

        public bool isCollapsed;
        private Form activeForm;
        public static String id;

        public FormAdminNavigation(String id1)
        {
            InitializeComponent();
            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            setCalendar();
            id = id1;
            SubForms_Admin.formDashboard.setForm(this);
            OpenChildForm(new SubForms_Admin.formDashboard());
            this.kryptonSplitContainer1.Panel2Collapsed = false;
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
                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("select Firstname, Lastname from tbl_admin_accounts where Unique_ID = '" + id + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    lblName.Text = dr["Firstname"].ToString() + " " + dr["Lastname"].ToString();
                    dr.Close();

                    cm = new SqlCommand("Select Profile_pic from tbl_admin_accounts where Unique_ID = " + id + "", cn);

                    byte[] imageData = (byte[])cm.ExecuteScalar();

                    if (imageData != null && imageData.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(imageData))
                        {
                            Image image = Image.FromStream(ms);
                            ptbProfile.Image = image;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately
                MessageBox.Show("Error loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDashboard_Click(object sender, EventArgs e)
        {
            isCollapsed = false;
            timer1.Start();
            loadData();
            this.kryptonSplitContainer1.Panel2Collapsed = false;
            OpenChildForm(new SubForms_Admin.formDashboard());
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

            this.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnAccounts_Click(object sender, EventArgs e)
        {
            isCollapsed = true;
            timer1.Start();
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

            this.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnSubjects_Click(object sender, EventArgs e)
        {
            isCollapsed = false;
            timer1.Start();
            OpenChildForm(new SubForms_Admin.formSubjects());
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            this.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnDashboard.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnDashboard.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnDashboard.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnDashboard.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnSettings.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSettings.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSettings.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSettings.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            isCollapsed = false;
            timer1.Start();
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formSettings(this));
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

            this.btnAccounts.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccounts.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccounts.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccounts.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnSubjects.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnSubjects.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnSubjects.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnSubjects.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            CollapseForm();
        }

        public void CollapseForm()
        {
            if (isCollapsed)
            {
                panelCollapsed.Height += 10;
                if (panelCollapsed.Size == panelCollapsed.MaximumSize)
                {
                    timer1.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                panelCollapsed.Height -= 10;
                if (panelCollapsed.Size == panelCollapsed.MinimumSize)
                {
                    timer1.Stop();
                    isCollapsed = true;
                }
            }
        }

        private void btnTeachers_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Teachers.setAccountName("Teachers Accounts");
            SubForms_Admin.formAccounts_Teachers.setRole(6);
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formAccounts_Teachers());

        }

        private void btnDeptHead_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_DeptHead.setAccountName("Department Head Accounts");
            SubForms_Admin.formAcctounts_DeptHead.setRole(2);
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formAcctounts_DeptHead());
        }

        private void btnGuidance_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAcctounts_Guidance.setAccountName("Guidance Associate Accounts");
            SubForms_Admin.formAcctounts_Guidance.setRole(3);
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formAcctounts_Guidance());
        }

        private void btnSAO_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_SAO.setAccountName("Student Affairs Officer Accounts");
            SubForms_Admin.formAccounts_SAO.setRole(4);
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formAccounts_SAO());
        }

        private void btnStudents_Click(object sender, EventArgs e)
        {
            SubForms_Admin.formAccounts_Students.setAccountName("Students Account");
            SubForms_Admin.formAccounts_Students.setRole(5);
            this.kryptonSplitContainer1.Panel2Collapsed = true;
            OpenChildForm(new SubForms_Admin.formAccounts_Students());
        }

        private void FormAdminNavigation_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormAdminNavigation_Load(object sender, EventArgs e)
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

        private void FormAdminNavigation_FormClosing(object sender, FormClosingEventArgs e)
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
                    using (SqlCommand cm = new SqlCommand("SELECT Event_ID,Event_Name, Color, Start_Date FROM tbl_events WHERE Start_Date >= CAST(GETDATE() AS DATE) ORDER BY Start_Date DESC", cn))
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
            kryptonLabel11.Name = "kryptonLabel11";
            kryptonLabel11.Cursor = System.Windows.Forms.Cursors.Hand;
            kryptonLabel11.Size = new System.Drawing.Size(96, 23);
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
