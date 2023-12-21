﻿using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjects : Form
    {
        SQLite_Connection sQLite_Connection;
        static FormTeacherNavigation form;
        public formSubjects()
        {
            InitializeComponent();
            sQLite_Connection = new SQLite_Connection();
        }
        public static void setForm(FormTeacherNavigation form1)
        {
            form = form1;
        }
        private void formSubjects_Load(object sender, EventArgs e)
        {
            displaysubjects();
        }
        public void displaysubjects()
        {
            tableLayoutPanel1.Controls.Clear();
            DataTable subjects = sQLite_Connection.GetAssignedSubjects(FormTeacherNavigation.id);

            if (subjects.Rows.Count > 0)
            {
                foreach (DataRow row in subjects.Rows)
                {
                    Image img = null;
                    string subjectcode = row["Course_Code"].ToString();
                    string subjectname = row["Course_Description"].ToString();
                    string subjectnacadlvl = row["Academic_Level"].ToString();
                    string subjectnacadlvldes = row["Academic_Level_Description"].ToString();
                    if (row["Image"] is Image image)
                    {
                        img = image;
                    }

                    subjectsApperance(subjectcode, subjectname, img, subjectnacadlvl, subjectnacadlvldes);
                }
            }
        }
        public void subjectsApperance(string ccode, string subjectname, Image image, string subjectnacadlvl, string subjectnacadlvldes)
        {
            Label lblSubjectName = new Label();
            KryptonGroupBox kryptonGroupBox2 = new KryptonGroupBox();
            RoundPictureBoxRect ptbSubjectPic = new RoundPictureBoxRect();
            Label lblAcadLvl = new Label();
            Label lblCcode = new Label();

            kryptonGroupBox2.CaptionStyle = LabelStyle.GroupBoxCaption;
            kryptonGroupBox2.CaptionVisible = false;
            kryptonGroupBox2.CausesValidation = false;
            kryptonGroupBox2.Dock = DockStyle.Fill;
            kryptonGroupBox2.GroupBackStyle = PaletteBackStyle.ControlGroupBox;
            kryptonGroupBox2.GroupBorderStyle = PaletteBorderStyle.ControlGroupBox;
            kryptonGroupBox2.Location = new System.Drawing.Point(0, 3);
            kryptonGroupBox2.Margin = new Padding(0, 3, 6, 3);
            kryptonGroupBox2.Name = "kryptonGroupBox2";
            kryptonGroupBox2.Cursor = Cursors.Hand;
            kryptonGroupBox2.Panel.Click += (senderbtn, ebtn) =>
            {
                form.otherformclick1(ccode, subjectnacadlvl);
            };

            kryptonGroupBox2.Panel.Padding = new Padding(15, 10, 15, 10);
            kryptonGroupBox2.Size = new System.Drawing.Size(231, 211);
            kryptonGroupBox2.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox2.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            kryptonGroupBox2.StateCommon.Back.ImageStyle = PaletteImageStyle.Inherit;
            kryptonGroupBox2.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox2.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            kryptonGroupBox2.StateCommon.Border.DrawBorders = ((PaletteDrawBorders)((((PaletteDrawBorders.Top | PaletteDrawBorders.Bottom)
            | PaletteDrawBorders.Left)
            | PaletteDrawBorders.Right)));
            kryptonGroupBox2.StateCommon.Border.ImageStyle = PaletteImageStyle.Inherit;
            kryptonGroupBox2.StateCommon.Border.Rounding = 10;
            kryptonGroupBox2.StateCommon.Border.Width = 2;
            kryptonGroupBox2.TabIndex = 15;
            kryptonGroupBox2.Click += (senderbtn, ebtn) =>
            {
                form.otherformclick1(ccode, subjectnacadlvl);
            };

            ptbSubjectPic.BorderWidth = 2;
            ptbSubjectPic.CornerRadius = 10;
            ptbSubjectPic.Anchor = AnchorStyles.None;
            ptbSubjectPic.Image = image;
            ptbSubjectPic.Location = new System.Drawing.Point(61, 37);
            ptbSubjectPic.Name = "ptbSubjectPic";
            ptbSubjectPic.Size = new System.Drawing.Size(118, 108);
            ptbSubjectPic.SizeMode = PictureBoxSizeMode.StretchImage;
            ptbSubjectPic.TabIndex = 1;
            ptbSubjectPic.TabStop = false;
            ptbSubjectPic.Cursor = Cursors.Hand;
            ptbSubjectPic.Click += (senderbtn, ebtn) =>
            {
                form.otherformclick1(ccode, subjectnacadlvl);
            };

            lblSubjectName.Dock = DockStyle.Bottom;
            lblSubjectName.Font = new Font("Poppins SemiBold", 12F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            lblSubjectName.ForeColor = Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            lblSubjectName.Location = new Point(15, 141);
            lblSubjectName.Name = "lblSubjectName";
            lblSubjectName.Size = new Size(191, 50);
            lblSubjectName.TabIndex = 0;
            lblSubjectName.Text = subjectname;
            lblSubjectName.TextAlign = ContentAlignment.TopCenter;
            lblSubjectName.Cursor = Cursors.Hand;
            lblSubjectName.Click += (senderbtn, ebtn) =>
            {
                form.otherformclick1(ccode, subjectnacadlvl);
            };

            lblAcadLvl.AutoSize = false;
            lblAcadLvl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            lblAcadLvl.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblAcadLvl.Location = new System.Drawing.Point(146, 6);
            lblAcadLvl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            lblAcadLvl.Name = "lblAcadLvl";
            lblAcadLvl.Size = new System.Drawing.Size(71, 23);
            lblAcadLvl.TabIndex = 3;
            lblAcadLvl.Text = subjectnacadlvldes;

            lblCcode.AutoSize = true;
            lblCcode.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblCcode.Location = new System.Drawing.Point(7, 7);
            lblCcode.Name = "lblCcode";
            lblCcode.Size = new System.Drawing.Size(58, 23);
            lblCcode.TabIndex = 2;
            lblCcode.Text = ccode;

            kryptonGroupBox2.Panel.Controls.Add(lblAcadLvl);
            kryptonGroupBox2.Panel.Controls.Add(lblCcode);
            kryptonGroupBox2.Panel.Controls.Add(ptbSubjectPic);
            kryptonGroupBox2.Panel.Controls.Add(lblSubjectName);

            tableLayoutPanel1.Controls.Add(kryptonGroupBox2, 0, 0);
        }

        private void ShowSubjectInfo_Click(object sender, EventArgs e)
        {
            //form.OpenChildForm(new formSubjectInformation());
            //form.isCollapsed = true;
            //form.kryptonSplitContainer1.Panel2Collapsed = true;
            //form.panelCollapsed.Size = form.panelCollapsed.MaximumSize;
        }
    }
}
