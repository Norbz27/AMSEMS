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

            kryptonGroupBox2.CaptionStyle = LabelStyle.GroupBoxCaption;
            kryptonGroupBox2.CaptionVisible = false;
            kryptonGroupBox2.CausesValidation = false;
            kryptonGroupBox2.Dock = DockStyle.Fill;
            kryptonGroupBox2.GroupBackStyle = PaletteBackStyle.ControlGroupBox;
            kryptonGroupBox2.GroupBorderStyle = PaletteBorderStyle.ControlGroupBox;
            kryptonGroupBox2.Location = new System.Drawing.Point(0, 3);
            kryptonGroupBox2.Margin = new Padding(0, 3, 6, 3);
            kryptonGroupBox2.Name = "kryptonGroupBox2";

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

            lblSubjectName.Dock = DockStyle.Bottom;
            lblSubjectName.Font = new System.Drawing.Font("Poppins SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblSubjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            lblSubjectName.Location = new System.Drawing.Point(15, 141);
            lblSubjectName.Name = "lblSubjectName";
            lblSubjectName.Size = new System.Drawing.Size(191, 50);
            lblSubjectName.TabIndex = 0;
            lblSubjectName.Text = subjectname;
            lblSubjectName.TextAlign = System.Drawing.ContentAlignment.TopCenter;

            kryptonGroupBox2.Panel.Controls.Add(ptbSubjectPic);
            kryptonGroupBox2.Panel.Controls.Add(lblSubjectName);

            tableLayoutPanel1.Controls.Add(kryptonGroupBox2, 0, 0);
        }
    }
}
