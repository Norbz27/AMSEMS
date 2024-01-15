namespace AMSEMS.SubForms_SAO
{
    partial class UserControlDays_Calendar
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
            this.components = new System.ComponentModel.Container();
            this.kryptonContextMenuHeading1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenuHeading();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addActtoolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.lblmonth = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.lblyear = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.lblDays = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonContextMenuHeading1
            // 
            this.kryptonContextMenuHeading1.ExtraText = "";
            // 
            // addEventToolStripMenuItem
            // 
            this.addEventToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addEventToolStripMenuItem.DoubleClickEnabled = true;
            this.addEventToolStripMenuItem.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addEventToolStripMenuItem.Name = "addEventToolStripMenuItem";
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(150, 26);
            this.addEventToolStripMenuItem.Text = "Add Event";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenuStrip1.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEventToolStripMenuItem,
            this.addActtoolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 56);
            // 
            // addActtoolStripMenuItem1
            // 
            this.addActtoolStripMenuItem1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.addActtoolStripMenuItem1.DoubleClickEnabled = true;
            this.addActtoolStripMenuItem1.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addActtoolStripMenuItem1.Name = "addActtoolStripMenuItem1";
            this.addActtoolStripMenuItem1.Size = new System.Drawing.Size(150, 26);
            this.addActtoolStripMenuItem1.Text = "Add Activity";
            this.addActtoolStripMenuItem1.Click += new System.EventHandler(this.addActtoolStripMenuItem1_Click);
            // 
            // lblmonth
            // 
            this.lblmonth.AutoSize = false;
            this.lblmonth.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblmonth.ForeColor = System.Drawing.Color.Black;
            this.lblmonth.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblmonth.Location = new System.Drawing.Point(28, 100);
            this.lblmonth.Name = "lblmonth";
            this.lblmonth.Size = new System.Drawing.Size(29, 10);
            this.lblmonth.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblmonth.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblmonth.Text = "00";
            this.lblmonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblmonth.Visible = false;
            // 
            // lblyear
            // 
            this.lblyear.AutoSize = false;
            this.lblyear.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblyear.ForeColor = System.Drawing.Color.Black;
            this.lblyear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblyear.Location = new System.Drawing.Point(63, 100);
            this.lblyear.Name = "lblyear";
            this.lblyear.Size = new System.Drawing.Size(29, 10);
            this.lblyear.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblyear.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblyear.Text = "00";
            this.lblyear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblyear.Visible = false;
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = false;
            this.lblDays.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblDays.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDays.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblDays.ForeColor = System.Drawing.Color.Black;
            this.lblDays.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDays.Location = new System.Drawing.Point(0, 0);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(103, 28);
            this.lblDays.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblDays.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblDays.Text = "00";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 28);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(103, 95);
            this.panel1.TabIndex = 4;
            // 
            // UserControlDays_Calendar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblyear);
            this.Controls.Add(this.lblmonth);
            this.Controls.Add(this.lblDays);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(103, 95);
            this.Name = "UserControlDays_Calendar";
            this.Size = new System.Drawing.Size(103, 123);
            this.Load += new System.EventHandler(this.UserControlDays_Calendar_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenuHeading kryptonContextMenuHeading1;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblmonth;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblyear;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblDays;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripMenuItem addActtoolStripMenuItem1;
    }
}
