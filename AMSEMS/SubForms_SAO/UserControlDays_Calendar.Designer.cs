﻿namespace AMSEMS.SubForms_SAO
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
            this.lblDays = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.kryptonContextMenuHeading1 = new ComponentFactory.Krypton.Toolkit.KryptonContextMenuHeading();
            this.addEventToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lblmonth = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.lblyear = new ComponentFactory.Krypton.Toolkit.KryptonWrapLabel();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblDays
            // 
            this.lblDays.AutoSize = false;
            this.lblDays.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDays.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblDays.ForeColor = System.Drawing.Color.Black;
            this.lblDays.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblDays.Location = new System.Drawing.Point(0, 0);
            this.lblDays.Name = "lblDays";
            this.lblDays.Size = new System.Drawing.Size(106, 28);
            this.lblDays.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblDays.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblDays.Text = "00";
            this.lblDays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.addEventToolStripMenuItem.Size = new System.Drawing.Size(139, 26);
            this.addEventToolStripMenuItem.Text = "Add Event";
            this.addEventToolStripMenuItem.Click += new System.EventHandler(this.addEventToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.contextMenuStrip1.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addEventToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.contextMenuStrip1.Size = new System.Drawing.Size(140, 30);
            // 
            // lblmonth
            // 
            this.lblmonth.AutoSize = false;
            this.lblmonth.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblmonth.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblmonth.ForeColor = System.Drawing.Color.Black;
            this.lblmonth.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblmonth.Location = new System.Drawing.Point(0, 28);
            this.lblmonth.Name = "lblmonth";
            this.lblmonth.Size = new System.Drawing.Size(106, 28);
            this.lblmonth.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblmonth.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblmonth.Text = "00";
            this.lblmonth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblmonth.Visible = false;
            // 
            // lblyear
            // 
            this.lblyear.AutoSize = false;
            this.lblyear.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblyear.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblyear.ForeColor = System.Drawing.Color.Black;
            this.lblyear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblyear.Location = new System.Drawing.Point(0, 56);
            this.lblyear.Name = "lblyear";
            this.lblyear.Size = new System.Drawing.Size(106, 28);
            this.lblyear.StateCommon.Font = new System.Drawing.Font("Poppins", 8.25F);
            this.lblyear.StateCommon.TextColor = System.Drawing.Color.Black;
            this.lblyear.Text = "00";
            this.lblyear.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblyear.Visible = false;
            // 
            // UserControlDays_Calendar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.lblyear);
            this.Controls.Add(this.lblmonth);
            this.Controls.Add(this.lblDays);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "UserControlDays_Calendar";
            this.Size = new System.Drawing.Size(106, 87);
            this.Load += new System.EventHandler(this.UserControlDays_Calendar_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblDays;
        private ComponentFactory.Krypton.Toolkit.KryptonContextMenuHeading kryptonContextMenuHeading1;
        private System.Windows.Forms.ToolStripMenuItem addEventToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblmonth;
        private ComponentFactory.Krypton.Toolkit.KryptonWrapLabel lblyear;
    }
}
