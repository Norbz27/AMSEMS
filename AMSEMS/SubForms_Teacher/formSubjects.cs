using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formSubjects : Form
    {
        SQLite_Connection sQLite_Connection;
  
        public formSubjects()
        {
            InitializeComponent();
            sQLite_Connection = new SQLite_Connection();
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
                    string subjectname = row["Course_Description"].ToString();
                    if (row["Image"] is Image image)
                    {
                        img = image;
                    }

                    subjectsApperance(subjectname, img);
                }
            }
        }
        public void subjectsApperance(string subjectname, Image image)
        {
            Label lblSubjectName = new Label();
            KryptonGroupBox kryptonGroupBox2 = new KryptonGroupBox();
            RoundPictureBoxRect ptbSubjectPic = new RoundPictureBoxRect();
            KryptonButton btnOption = new KryptonButton();

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

            ptbSubjectPic.BorderWidth = 2;
            ptbSubjectPic.CornerRadius = 10;
            ptbSubjectPic.Anchor = AnchorStyles.None;
            ptbSubjectPic.Image = image;
            ptbSubjectPic.Location = new System.Drawing.Point(50, 24);
            ptbSubjectPic.Name = "ptbSubjectPic";
            ptbSubjectPic.Size = new System.Drawing.Size(118, 108);
            ptbSubjectPic.SizeMode = PictureBoxSizeMode.StretchImage;
            ptbSubjectPic.TabIndex = 1;
            ptbSubjectPic.TabStop = false;
            ptbSubjectPic.Cursor = Cursors.Hand;

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

            btnOption.Anchor = ((AnchorStyles)((AnchorStyles.Top | AnchorStyles.Right)));
            btnOption.Location = new Point(185, 7);
            btnOption.Name = "btnOption";
            btnOption.PaletteMode = PaletteMode.ProfessionalSystem;
            btnOption.Size = new Size(29, 24);
            btnOption.Cursor = Cursors.Hand;
            btnOption.StateCommon.Back.Color1 = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnOption.StateCommon.Back.Color2 = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnOption.StateCommon.Back.ImageStyle = PaletteImageStyle.Inherit;
            btnOption.StateCommon.Border.Color1 = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnOption.StateCommon.Border.Color2 = Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            btnOption.StateCommon.Border.DrawBorders = ((PaletteDrawBorders)((((PaletteDrawBorders.Top | PaletteDrawBorders.Bottom)
            | PaletteDrawBorders.Left)
            | PaletteDrawBorders.Right)));
            btnOption.StateCommon.Border.ImageStyle = PaletteImageStyle.Inherit;
            btnOption.TabIndex = 2;
            btnOption.Values.Image = Properties.Resources.option;
            btnOption.Values.Text = ""; 
            btnOption.Click += (sender, e) =>
            {
                ContextMenuStrip CMSOptions = new ContextMenuStrip();
                ToolStripMenuItem toolStripMenuItem2 = new ToolStripMenuItem();

                toolStripMenuItem2.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                toolStripMenuItem2.Name = "toolStripMenuItem2";
                toolStripMenuItem2.Size = new System.Drawing.Size(148, 26);
                toolStripMenuItem2.Text = "Manage Subject";

                CMSOptions.Font = new System.Drawing.Font("Segoe UI", 9F);
                CMSOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            toolStripMenuItem2});
                CMSOptions.Name = "contextMenuStrip2";
                CMSOptions.ShowImageMargin = false;
                CMSOptions.ShowItemToolTips = false;
                CMSOptions.Size = new System.Drawing.Size(149, 30);

                CMSOptions.Show(btnOption, new Point(0, btnOption.Height));
            };

            

            kryptonGroupBox2.Panel.Controls.Add(ptbSubjectPic);
            kryptonGroupBox2.Panel.Controls.Add(lblSubjectName);
            kryptonGroupBox2.Panel.Controls.Add(btnOption);

            tableLayoutPanel1.Controls.Add(kryptonGroupBox2, 0, 0);
        }
    }
}
