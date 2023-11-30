namespace AMSEMS.SubForms_Teacher
{
    partial class formSubjectInformation
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSubjectName = new System.Windows.Forms.Label();
            this.btnback = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ptbSubjectPic = new AMSEMS.RoundPictureBoxRect();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSubjectPic)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(203, 0);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panel2.Size = new System.Drawing.Size(637, 550);
            this.panel2.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.lblSubjectName);
            this.panel1.Controls.Add(this.btnback);
            this.panel1.Controls.Add(this.ptbSubjectPic);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.panel1.Size = new System.Drawing.Size(203, 550);
            this.panel1.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Location = new System.Drawing.Point(21, 233);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(159, 299);
            this.panel3.TabIndex = 5;
            // 
            // lblSubjectName
            // 
            this.lblSubjectName.Font = new System.Drawing.Font("Poppins SemiBold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubjectName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblSubjectName.Location = new System.Drawing.Point(20, 171);
            this.lblSubjectName.Name = "lblSubjectName";
            this.lblSubjectName.Size = new System.Drawing.Size(160, 59);
            this.lblSubjectName.TabIndex = 4;
            this.lblSubjectName.Text = "Subject Name";
            // 
            // btnback
            // 
            this.btnback.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnback.Location = new System.Drawing.Point(21, 18);
            this.btnback.Name = "btnback";
            this.btnback.Size = new System.Drawing.Size(109, 23);
            this.btnback.StateCommon.AdjacentGap = 5;
            this.btnback.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnback.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnback.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F);
            this.btnback.TabIndex = 3;
            this.btnback.Values.Image = global::AMSEMS.Properties.Resources.prev;
            this.btnback.Values.Text = "All Subjects";
            this.btnback.Click += new System.EventHandler(this.btnback_Click);
            // 
            // ptbSubjectPic
            // 
            this.ptbSubjectPic.BackColor = System.Drawing.Color.White;
            this.ptbSubjectPic.BorderWidth = 2;
            this.ptbSubjectPic.CornerRadius = 10;
            this.ptbSubjectPic.Image = global::AMSEMS.Properties.Resources.book1;
            this.ptbSubjectPic.Location = new System.Drawing.Point(21, 67);
            this.ptbSubjectPic.Name = "ptbSubjectPic";
            this.ptbSubjectPic.Size = new System.Drawing.Size(92, 86);
            this.ptbSubjectPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ptbSubjectPic.TabIndex = 2;
            this.ptbSubjectPic.TabStop = false;
            // 
            // formSubjectInformation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(840, 550);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "formSubjectInformation";
            this.Text = "formSubjectInformation";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSubjectPic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel btnback;
        private RoundPictureBoxRect ptbSubjectPic;
        private System.Windows.Forms.Label lblSubjectName;
        private System.Windows.Forms.Panel panel3;
    }
}