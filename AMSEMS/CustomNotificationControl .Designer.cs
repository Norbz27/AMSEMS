namespace AMSEMS
{
    partial class CustomNotificationControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblHtittle = new System.Windows.Forms.Label();
            this.lbltitle = new System.Windows.Forms.Label();
            this.lblSpan = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblHtittle
            // 
            this.lblHtittle.AutoSize = true;
            this.lblHtittle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHtittle.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHtittle.Location = new System.Drawing.Point(5, 5);
            this.lblHtittle.Name = "lblHtittle";
            this.lblHtittle.Size = new System.Drawing.Size(105, 22);
            this.lblHtittle.TabIndex = 0;
            this.lblHtittle.Text = "Announcement";
            // 
            // lbltitle
            // 
            this.lbltitle.AutoEllipsis = true;
            this.lbltitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lbltitle.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltitle.Location = new System.Drawing.Point(5, 27);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Size = new System.Drawing.Size(245, 22);
            this.lbltitle.TabIndex = 1;
            this.lbltitle.Text = "Announcementdaw  awdwa aw aw a dawdwad wad aw wad aw awa dwaw daw  a a daw";
            // 
            // lblSpan
            // 
            this.lblSpan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpan.AutoSize = true;
            this.lblSpan.Font = new System.Drawing.Font("Poppins", 7.5F);
            this.lblSpan.Location = new System.Drawing.Point(185, 8);
            this.lblSpan.Name = "lblSpan";
            this.lblSpan.Size = new System.Drawing.Size(62, 17);
            this.lblSpan.TabIndex = 2;
            this.lblSpan.Text = "20 min ago";
            // 
            // CustomNotificationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.Controls.Add(this.lblSpan);
            this.Controls.Add(this.lbltitle);
            this.Controls.Add(this.lblHtittle);
            this.Name = "CustomNotificationControl";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Size = new System.Drawing.Size(255, 54);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblHtittle;
        private System.Windows.Forms.Label lbltitle;
        private System.Windows.Forms.Label lblSpan;
    }
}
