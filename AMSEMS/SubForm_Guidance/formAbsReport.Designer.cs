﻿namespace AMSEMS.SubForm_Guidance
{
    partial class formAbsReport
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblRepHeader = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReload = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnExport = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonGroupBox5 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.dgvAbesnteismRep = new System.Windows.Forms.DataGridView();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSection = new System.Windows.Forms.ComboBox();
            this.CMSOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.CMSExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnExpPDF = new System.Windows.Forms.ToolStripMenuItem();
            this.kryptonLabel12 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbSearch = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbStatus = new System.Windows.Forms.ComboBox();
            this.ptbLoading = new AMSEMS.RoundPictureBoxRect();
            this.studid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.consultid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.studname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.classcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sub = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.absences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.option = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).BeginInit();
            this.kryptonGroupBox5.Panel.SuspendLayout();
            this.kryptonGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbesnteismRep)).BeginInit();
            this.CMSOptions.SuspendLayout();
            this.CMSExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // lblRepHeader
            // 
            this.lblRepHeader.Location = new System.Drawing.Point(-5, 3);
            this.lblRepHeader.Name = "lblRepHeader";
            this.lblRepHeader.Size = new System.Drawing.Size(68, 27);
            this.lblRepHeader.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblRepHeader.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblRepHeader.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRepHeader.TabIndex = 4;
            this.lblRepHeader.Values.Text = "Report";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnReload);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.lblRepHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(843, 36);
            this.panel1.TabIndex = 7;
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReload.Location = new System.Drawing.Point(763, 0);
            this.btnReload.Name = "btnReload";
            this.btnReload.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnReload.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnReload.OverrideDefault.Border.Rounding = 10;
            this.btnReload.OverrideDefault.Border.Width = 1;
            this.btnReload.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnReload.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnReload.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnReload.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnReload.OverrideFocus.Border.Rounding = 10;
            this.btnReload.Size = new System.Drawing.Size(37, 34);
            this.btnReload.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnReload.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnReload.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnReload.StateCommon.Border.Rounding = 10;
            this.btnReload.StateCommon.Border.Width = 1;
            this.btnReload.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnReload.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnReload.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            this.btnReload.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnReload.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnReload.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnReload.StatePressed.Border.Rounding = 10;
            this.btnReload.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnReload.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnReload.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnReload.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnReload.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnReload.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnReload.StateTracking.Border.Rounding = 10;
            this.btnReload.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnReload.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnReload.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnReload.TabIndex = 10;
            this.btnReload.Values.Image = global::AMSEMS.Properties.Resources.refresh_16;
            this.btnReload.Values.Text = "";
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Location = new System.Drawing.Point(806, 0);
            this.btnExport.Name = "btnExport";
            this.btnExport.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnExport.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnExport.OverrideDefault.Border.Rounding = 10;
            this.btnExport.OverrideDefault.Border.Width = 1;
            this.btnExport.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnExport.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnExport.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnExport.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnExport.OverrideFocus.Border.Rounding = 10;
            this.btnExport.Size = new System.Drawing.Size(37, 34);
            this.btnExport.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnExport.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnExport.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnExport.StateCommon.Border.Rounding = 10;
            this.btnExport.StateCommon.Border.Width = 1;
            this.btnExport.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            this.btnExport.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnExport.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnExport.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            this.btnExport.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnExport.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnExport.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnExport.StatePressed.Border.Rounding = 10;
            this.btnExport.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnExport.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnExport.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnExport.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnExport.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnExport.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnExport.StateTracking.Border.Rounding = 10;
            this.btnExport.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnExport.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnExport.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnExport.TabIndex = 9;
            this.btnExport.Values.Image = global::AMSEMS.Properties.Resources.export_16;
            this.btnExport.Values.Text = "";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // kryptonGroupBox5
            // 
            this.kryptonGroupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonGroupBox5.CaptionVisible = false;
            this.kryptonGroupBox5.CausesValidation = false;
            this.kryptonGroupBox5.Location = new System.Drawing.Point(21, 120);
            this.kryptonGroupBox5.Name = "kryptonGroupBox5";
            // 
            // kryptonGroupBox5.Panel
            // 
            this.kryptonGroupBox5.Panel.Controls.Add(this.ptbLoading);
            this.kryptonGroupBox5.Panel.Controls.Add(this.dgvAbesnteismRep);
            this.kryptonGroupBox5.Size = new System.Drawing.Size(842, 416);
            this.kryptonGroupBox5.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox5.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox5.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox5.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox5.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroupBox5.StateCommon.Border.Rounding = 10;
            this.kryptonGroupBox5.StateCommon.Border.Width = 2;
            this.kryptonGroupBox5.TabIndex = 32;
            // 
            // dgvAbesnteismRep
            // 
            this.dgvAbesnteismRep.AllowUserToAddRows = false;
            this.dgvAbesnteismRep.AllowUserToDeleteRows = false;
            this.dgvAbesnteismRep.AllowUserToResizeColumns = false;
            this.dgvAbesnteismRep.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvAbesnteismRep.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAbesnteismRep.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAbesnteismRep.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAbesnteismRep.BackgroundColor = System.Drawing.Color.White;
            this.dgvAbesnteismRep.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvAbesnteismRep.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvAbesnteismRep.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvAbesnteismRep.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAbesnteismRep.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAbesnteismRep.ColumnHeadersHeight = 50;
            this.dgvAbesnteismRep.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvAbesnteismRep.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.studid,
            this.consultid,
            this.studname,
            this.section,
            this.classcode,
            this.sub,
            this.absences,
            this.status,
            this.option});
            this.dgvAbesnteismRep.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Poppins", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAbesnteismRep.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAbesnteismRep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAbesnteismRep.EnableHeadersVisualStyles = false;
            this.dgvAbesnteismRep.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dgvAbesnteismRep.Location = new System.Drawing.Point(0, 0);
            this.dgvAbesnteismRep.MultiSelect = false;
            this.dgvAbesnteismRep.Name = "dgvAbesnteismRep";
            this.dgvAbesnteismRep.ReadOnly = true;
            this.dgvAbesnteismRep.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAbesnteismRep.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAbesnteismRep.RowHeadersVisible = false;
            this.dgvAbesnteismRep.RowHeadersWidth = 30;
            this.dgvAbesnteismRep.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAbesnteismRep.RowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAbesnteismRep.RowTemplate.Height = 30;
            this.dgvAbesnteismRep.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAbesnteismRep.Size = new System.Drawing.Size(832, 406);
            this.dgvAbesnteismRep.TabIndex = 1;
            this.dgvAbesnteismRep.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAbesnteismRep_CellContentClick);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(151, 60);
            this.kryptonLabel1.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(59, 22);
            this.kryptonLabel1.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel1.TabIndex = 39;
            this.kryptonLabel1.Values.Text = "Section";
            // 
            // cbSection
            // 
            this.cbSection.Font = new System.Drawing.Font("Poppins", 8F);
            this.cbSection.FormattingEnabled = true;
            this.cbSection.Location = new System.Drawing.Point(156, 84);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(124, 27);
            this.cbSection.TabIndex = 38;
            this.cbSection.SelectedIndexChanged += new System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // CMSOptions
            // 
            this.CMSOptions.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CMSOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem});
            this.CMSOptions.Name = "contextMenuStrip2";
            this.CMSOptions.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.CMSOptions.ShowImageMargin = false;
            this.CMSOptions.Size = new System.Drawing.Size(126, 30);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(125, 26);
            this.viewToolStripMenuItem.Text = "View Record";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::AMSEMS.Properties.Resources.option_24;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            // 
            // CMSExport
            // 
            this.CMSExport.AutoSize = false;
            this.CMSExport.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.CMSExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToToolStripMenuItem,
            this.btnExpPDF});
            this.CMSExport.Name = "contextMenuStrip2";
            this.CMSExport.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.CMSExport.ShowImageMargin = false;
            this.CMSExport.ShowItemToolTips = false;
            this.CMSExport.Size = new System.Drawing.Size(146, 70);
            // 
            // exportToToolStripMenuItem
            // 
            this.exportToToolStripMenuItem.AutoSize = false;
            this.exportToToolStripMenuItem.Enabled = false;
            this.exportToToolStripMenuItem.Font = new System.Drawing.Font("Poppins", 7.55F);
            this.exportToToolStripMenuItem.Name = "exportToToolStripMenuItem";
            this.exportToToolStripMenuItem.Size = new System.Drawing.Size(145, 26);
            this.exportToToolStripMenuItem.Text = "Export to";
            // 
            // btnExpPDF
            // 
            this.btnExpPDF.AutoSize = false;
            this.btnExpPDF.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExpPDF.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpPDF.Margin = new System.Windows.Forms.Padding(7, 0, 0, 0);
            this.btnExpPDF.Name = "btnExpPDF";
            this.btnExpPDF.Size = new System.Drawing.Size(135, 26);
            this.btnExpPDF.Text = "PDF File";
            this.btnExpPDF.Click += new System.EventHandler(this.btnExpPDF_Click);
            // 
            // kryptonLabel12
            // 
            this.kryptonLabel12.Location = new System.Drawing.Point(284, 60);
            this.kryptonLabel12.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel12.Name = "kryptonLabel12";
            this.kryptonLabel12.Size = new System.Drawing.Size(52, 22);
            this.kryptonLabel12.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel12.TabIndex = 41;
            this.kryptonLabel12.Values.Text = "Month";
            // 
            // cbMonth
            // 
            this.cbMonth.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"});
            this.cbMonth.Location = new System.Drawing.Point(289, 84);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(127, 27);
            this.cbMonth.TabIndex = 40;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = global::AMSEMS.Properties.Resources.search_16;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(673, 88);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 42;
            this.pictureBox1.TabStop = false;
            // 
            // tbSearch
            // 
            this.tbSearch.AcceptsReturn = true;
            this.tbSearch.AlwaysActive = false;
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(667, 81);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(196, 30);
            this.tbSearch.StateActive.Border.Color1 = System.Drawing.Color.Gray;
            this.tbSearch.StateActive.Border.Color2 = System.Drawing.Color.Gray;
            this.tbSearch.StateActive.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.tbSearch.StateCommon.Border.Color1 = System.Drawing.Color.LightGray;
            this.tbSearch.StateCommon.Border.Color2 = System.Drawing.Color.LightGray;
            this.tbSearch.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.tbSearch.StateCommon.Border.Rounding = 2;
            this.tbSearch.StateCommon.Border.Width = 1;
            this.tbSearch.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 8.75F);
            this.tbSearch.StateCommon.Content.Padding = new System.Windows.Forms.Padding(25, 4, 0, 4);
            this.tbSearch.TabIndex = 43;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(15, 60);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(52, 22);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel2.TabIndex = 45;
            this.kryptonLabel2.Values.Text = "Status";
            // 
            // cbStatus
            // 
            this.cbStatus.Font = new System.Drawing.Font("Poppins", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbStatus.FormattingEnabled = true;
            this.cbStatus.Items.AddRange(new object[] {
            "Pending",
            "Done"});
            this.cbStatus.Location = new System.Drawing.Point(20, 84);
            this.cbStatus.Name = "cbStatus";
            this.cbStatus.Size = new System.Drawing.Size(127, 27);
            this.cbStatus.TabIndex = 44;
            this.cbStatus.SelectedIndexChanged += new System.EventHandler(this.cbStatus_SelectedIndexChanged);
            // 
            // ptbLoading
            // 
            this.ptbLoading.BackColor = System.Drawing.Color.White;
            this.ptbLoading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ptbLoading.BorderWidth = 0;
            this.ptbLoading.CornerRadius = 10;
            this.ptbLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ptbLoading.Image = global::AMSEMS.Properties.Resources.loading;
            this.ptbLoading.ImageLocation = "";
            this.ptbLoading.Location = new System.Drawing.Point(0, 0);
            this.ptbLoading.Name = "ptbLoading";
            this.ptbLoading.Size = new System.Drawing.Size(832, 406);
            this.ptbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ptbLoading.TabIndex = 13;
            this.ptbLoading.TabStop = false;
            this.ptbLoading.Visible = false;
            // 
            // studid
            // 
            this.studid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.studid.HeaderText = "ID";
            this.studid.Name = "studid";
            this.studid.ReadOnly = true;
            this.studid.Width = 45;
            // 
            // consultid
            // 
            this.consultid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.consultid.HeaderText = "Consultation ID";
            this.consultid.Name = "consultid";
            this.consultid.ReadOnly = true;
            this.consultid.Visible = false;
            this.consultid.Width = 116;
            // 
            // studname
            // 
            this.studname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.studname.HeaderText = "Name";
            this.studname.Name = "studname";
            this.studname.ReadOnly = true;
            // 
            // section
            // 
            this.section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.section.HeaderText = "Section";
            this.section.Name = "section";
            this.section.ReadOnly = true;
            this.section.Width = 78;
            // 
            // classcode
            // 
            this.classcode.HeaderText = "Class Code";
            this.classcode.Name = "classcode";
            this.classcode.ReadOnly = true;
            this.classcode.Visible = false;
            // 
            // sub
            // 
            this.sub.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.sub.HeaderText = "Subject";
            this.sub.Name = "sub";
            this.sub.ReadOnly = true;
            this.sub.Width = 78;
            // 
            // absences
            // 
            this.absences.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.absences.HeaderText = "Consecutive Absent";
            this.absences.Name = "absences";
            this.absences.ReadOnly = true;
            this.absences.Width = 140;
            // 
            // status
            // 
            this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.status.HeaderText = "Consultation Status";
            this.status.Name = "status";
            this.status.ReadOnly = true;
            this.status.Width = 140;
            // 
            // option
            // 
            this.option.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.option.HeaderText = "";
            this.option.Image = global::AMSEMS.Properties.Resources.option_24;
            this.option.Name = "option";
            this.option.ReadOnly = true;
            this.option.Width = 5;
            // 
            // formAbsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(883, 549);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.cbStatus);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.tbSearch);
            this.Controls.Add(this.kryptonLabel12);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.cbSection);
            this.Controls.Add(this.kryptonGroupBox5);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formAbsReport";
            this.Padding = new System.Windows.Forms.Padding(20, 15, 20, 10);
            this.Text = " ";
            this.Load += new System.EventHandler(this.formAbsReport_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).EndInit();
            this.kryptonGroupBox5.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).EndInit();
            this.kryptonGroupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbesnteismRep)).EndInit();
            this.CMSOptions.ResumeLayout(false);
            this.CMSExport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblRepHeader;
        private System.Windows.Forms.Panel panel1;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox5;
        private System.Windows.Forms.DataGridView dgvAbesnteismRep;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.ComboBox cbSection;
        private System.Windows.Forms.ContextMenuStrip CMSOptions;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExport;
        private System.Windows.Forms.ContextMenuStrip CMSExport;
        private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnExpPDF;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel12;
        private System.Windows.Forms.ComboBox cbMonth;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReload;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.ComboBox cbStatus;
        private RoundPictureBoxRect ptbLoading;
        private System.Windows.Forms.DataGridViewTextBoxColumn studid;
        private System.Windows.Forms.DataGridViewTextBoxColumn consultid;
        private System.Windows.Forms.DataGridViewTextBoxColumn studname;
        private System.Windows.Forms.DataGridViewTextBoxColumn section;
        private System.Windows.Forms.DataGridViewTextBoxColumn classcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn sub;
        private System.Windows.Forms.DataGridViewTextBoxColumn absences;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
        private System.Windows.Forms.DataGridViewImageColumn option;
    }
}