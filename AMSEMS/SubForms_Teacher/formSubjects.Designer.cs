namespace AMSEMS.SubForms_Teacher
{
    partial class formSubjects
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblAccountName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.kryptonGroupBox2 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.lblAcadLvl = new System.Windows.Forms.Label();
            this.lblCcode = new System.Windows.Forms.Label();
            this.lblSubjectName = new System.Windows.Forms.Label();
            this.ptbSubjectPic = new AMSEMS.RoundPictureBoxRect();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).BeginInit();
            this.kryptonGroupBox2.Panel.SuspendLayout();
            this.kryptonGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSubjectPic)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblAccountName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 46);
            this.panel1.TabIndex = 8;
            // 
            // lblAccountName
            // 
            this.lblAccountName.Location = new System.Drawing.Point(3, 3);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(85, 27);
            this.lblAccountName.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountName.TabIndex = 4;
            this.lblAccountName.Values.Text = "Subjects";
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.tableLayoutPanel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(20, 61);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 534);
            this.panel2.TabIndex = 9;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tableLayoutPanel1.Controls.Add(this.kryptonGroupBox2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 230F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1016, 534);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // kryptonGroupBox2
            // 
            this.kryptonGroupBox2.CaptionVisible = false;
            this.kryptonGroupBox2.CausesValidation = false;
            this.kryptonGroupBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.kryptonGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox2.Location = new System.Drawing.Point(0, 3);
            this.kryptonGroupBox2.Margin = new System.Windows.Forms.Padding(0, 3, 6, 3);
            this.kryptonGroupBox2.Name = "kryptonGroupBox2";
            // 
            // kryptonGroupBox2.Panel
            // 
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblAcadLvl);
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblCcode);
            this.kryptonGroupBox2.Panel.Controls.Add(this.ptbSubjectPic);
            this.kryptonGroupBox2.Panel.Controls.Add(this.lblSubjectName);
            this.kryptonGroupBox2.Panel.Padding = new System.Windows.Forms.Padding(15, 10, 15, 10);
            this.kryptonGroupBox2.Size = new System.Drawing.Size(248, 224);
            this.kryptonGroupBox2.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox2.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox2.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox2.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox2.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroupBox2.StateCommon.Border.Rounding = 10;
            this.kryptonGroupBox2.StateCommon.Border.Width = 2;
            this.kryptonGroupBox2.TabIndex = 15;
            this.kryptonGroupBox2.Click += new System.EventHandler(this.ShowSubjectInfo_Click);
            // 
            // lblAcadLvl
            // 
            this.lblAcadLvl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAcadLvl.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcadLvl.Location = new System.Drawing.Point(146, 6);
            this.lblAcadLvl.Name = "lblAcadLvl";
            this.lblAcadLvl.Size = new System.Drawing.Size(71, 23);
            this.lblAcadLvl.TabIndex = 3;
            this.lblAcadLvl.Text = "Tertiary";
            this.lblAcadLvl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCcode
            // 
            this.lblCcode.AutoSize = true;
            this.lblCcode.Font = new System.Drawing.Font("Poppins SemiBold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCcode.Location = new System.Drawing.Point(7, 7);
            this.lblCcode.Name = "lblCcode";
            this.lblCcode.Size = new System.Drawing.Size(58, 23);
            this.lblCcode.TabIndex = 2;
            this.lblCcode.Text = "WAF124";
            // 
            // lblSubjectName
            // 
            this.lblSubjectName.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSubjectName.Font = new System.Drawing.Font("Poppins SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblSubjectName.Location = new System.Drawing.Point(15, 154);
            this.lblSubjectName.Name = "lblSubjectName";
            this.lblSubjectName.Size = new System.Drawing.Size(208, 50);
            this.lblSubjectName.TabIndex = 0;
            this.lblSubjectName.Text = "Subject Name";
            this.lblSubjectName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // ptbSubjectPic
            // 
            this.ptbSubjectPic.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ptbSubjectPic.BorderWidth = 2;
            this.ptbSubjectPic.CornerRadius = 10;
            this.ptbSubjectPic.Image = global::AMSEMS.Properties.Resources.book1;
            this.ptbSubjectPic.Location = new System.Drawing.Point(58, 43);
            this.ptbSubjectPic.Name = "ptbSubjectPic";
            this.ptbSubjectPic.Size = new System.Drawing.Size(118, 108);
            this.ptbSubjectPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptbSubjectPic.TabIndex = 1;
            this.ptbSubjectPic.TabStop = false;
            // 
            // formSubjects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1056, 610);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formSubjects";
            this.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.Text = "formSubjects";
            this.Load += new System.EventHandler(this.formSubjects_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2.Panel)).EndInit();
            this.kryptonGroupBox2.Panel.ResumeLayout(false);
            this.kryptonGroupBox2.Panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox2)).EndInit();
            this.kryptonGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbSubjectPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccountName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox2;
        private System.Windows.Forms.Label lblAcadLvl;
        private System.Windows.Forms.Label lblCcode;
        private RoundPictureBoxRect ptbSubjectPic;
        private System.Windows.Forms.Label lblSubjectName;
    }
}