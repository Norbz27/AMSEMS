namespace AMSEMS.SubForms_DeptHead
{
    partial class formAttendanceReport
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.kryptonGroupBox5 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.panel5 = new System.Windows.Forms.Panel();
            this.btnRefresh = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbSearch = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblMonthlyTotalFees = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSection = new System.Windows.Forms.ComboBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbYear = new System.Windows.Forms.ComboBox();
            this.kryptonLabel12 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExport = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblAccountName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ptbLoading = new AMSEMS.RoundPictureBoxRect();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).BeginInit();
            this.kryptonGroupBox5.Panel.SuspendLayout();
            this.kryptonGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(20, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(906, 738);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.kryptonGroupBox5);
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.panel3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(906, 738);
            this.panel4.TabIndex = 1;
            // 
            // kryptonGroupBox5
            // 
            this.kryptonGroupBox5.CaptionVisible = false;
            this.kryptonGroupBox5.CausesValidation = false;
            this.kryptonGroupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox5.Location = new System.Drawing.Point(0, 176);
            this.kryptonGroupBox5.Name = "kryptonGroupBox5";
            // 
            // kryptonGroupBox5.Panel
            // 
            this.kryptonGroupBox5.Panel.Controls.Add(this.ptbLoading);
            this.kryptonGroupBox5.Panel.Controls.Add(this.dgvReport);
            this.kryptonGroupBox5.Size = new System.Drawing.Size(906, 562);
            this.kryptonGroupBox5.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox5.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.kryptonGroupBox5.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox5.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.kryptonGroupBox5.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.kryptonGroupBox5.StateCommon.Border.Rounding = 10;
            this.kryptonGroupBox5.StateCommon.Border.Width = 2;
            this.kryptonGroupBox5.TabIndex = 28;
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.AllowUserToResizeColumns = false;
            this.dgvReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvReport.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Poppins", 8.75F, System.Drawing.FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvReport.ColumnHeadersHeight = 50;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvReport.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.EnableHeadersVisualStyles = false;
            this.dgvReport.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvReport.Location = new System.Drawing.Point(0, 0);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvReport.RowHeadersVisible = false;
            this.dgvReport.RowHeadersWidth = 30;
            this.dgvReport.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvReport.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvReport.RowTemplate.Height = 30;
            this.dgvReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReport.Size = new System.Drawing.Size(896, 552);
            this.dgvReport.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnRefresh);
            this.panel5.Controls.Add(this.pictureBox1);
            this.panel5.Controls.Add(this.tbSearch);
            this.panel5.Controls.Add(this.kryptonLabel2);
            this.panel5.Controls.Add(this.lblMonthlyTotalFees);
            this.panel5.Controls.Add(this.kryptonLabel4);
            this.panel5.Controls.Add(this.cbSection);
            this.panel5.Controls.Add(this.kryptonLabel1);
            this.panel5.Controls.Add(this.cbYear);
            this.panel5.Controls.Add(this.kryptonLabel12);
            this.panel5.Controls.Add(this.cbMonth);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 33);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(906, 143);
            this.panel5.TabIndex = 10;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Location = new System.Drawing.Point(0, 100);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnRefresh.OverrideDefault.Border.Rounding = 10;
            this.btnRefresh.OverrideDefault.Border.Width = 1;
            this.btnRefresh.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnRefresh.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRefresh.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnRefresh.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.OverrideFocus.Border.Rounding = 10;
            this.btnRefresh.Size = new System.Drawing.Size(160, 33);
            this.btnRefresh.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnRefresh.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnRefresh.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.StateCommon.Border.Rounding = 10;
            this.btnRefresh.StateCommon.Border.Width = 1;
            this.btnRefresh.StateCommon.Content.AdjacentGap = 5;
            this.btnRefresh.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            this.btnRefresh.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRefresh.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnRefresh.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            this.btnRefresh.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnRefresh.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.StatePressed.Border.Rounding = 10;
            this.btnRefresh.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnRefresh.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRefresh.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnRefresh.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnRefresh.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnRefresh.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.StateTracking.Border.Rounding = 10;
            this.btnRefresh.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnRefresh.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnRefresh.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnRefresh.TabIndex = 12;
            this.btnRefresh.Values.Image = global::AMSEMS.Properties.Resources.refresh_16;
            this.btnRefresh.Values.Text = "Genarate Report";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = global::AMSEMS.Properties.Resources.search_16;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(689, 109);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 150;
            this.pictureBox1.TabStop = false;
            // 
            // tbSearch
            // 
            this.tbSearch.AcceptsReturn = true;
            this.tbSearch.AlwaysActive = false;
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(682, 102);
            this.tbSearch.Name = "tbSearch";
            this.tbSearch.Size = new System.Drawing.Size(221, 30);
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
            this.tbSearch.TabIndex = 151;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(313, 19);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(59, 22);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel2.TabIndex = 22;
            this.kryptonLabel2.Values.Text = "Section";
            // 
            // lblMonthlyTotalFees
            // 
            this.lblMonthlyTotalFees.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMonthlyTotalFees.Location = new System.Drawing.Point(810, 43);
            this.lblMonthlyTotalFees.Margin = new System.Windows.Forms.Padding(2);
            this.lblMonthlyTotalFees.Name = "lblMonthlyTotalFees";
            this.lblMonthlyTotalFees.Size = new System.Drawing.Size(78, 31);
            this.lblMonthlyTotalFees.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblMonthlyTotalFees.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblMonthlyTotalFees.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins Medium", 13F, System.Drawing.FontStyle.Bold);
            this.lblMonthlyTotalFees.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.lblMonthlyTotalFees.TabIndex = 147;
            this.lblMonthlyTotalFees.Values.Text = "₱ 00.00";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonLabel4.Location = new System.Drawing.Point(680, 20);
            this.kryptonLabel4.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(216, 23);
            this.kryptonLabel4.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel4.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel4.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F);
            this.kryptonLabel4.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonLabel4.TabIndex = 146;
            this.kryptonLabel4.Values.Text = "Monthly Total Fees Per Section";
            // 
            // cbSection
            // 
            this.cbSection.DropDownHeight = 100;
            this.cbSection.Font = new System.Drawing.Font("Poppins", 8F);
            this.cbSection.FormattingEnabled = true;
            this.cbSection.IntegralHeight = false;
            this.cbSection.Location = new System.Drawing.Point(318, 43);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(127, 27);
            this.cbSection.TabIndex = 21;
            this.cbSection.SelectedIndexChanged += new System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(146, 19);
            this.kryptonLabel1.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(40, 22);
            this.kryptonLabel1.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel1.TabIndex = 20;
            this.kryptonLabel1.Values.Text = "Year";
            // 
            // cbYear
            // 
            this.cbYear.DropDownHeight = 100;
            this.cbYear.Font = new System.Drawing.Font("Poppins", 8F);
            this.cbYear.FormattingEnabled = true;
            this.cbYear.IntegralHeight = false;
            this.cbYear.Location = new System.Drawing.Point(151, 43);
            this.cbYear.Name = "cbYear";
            this.cbYear.Size = new System.Drawing.Size(144, 27);
            this.cbYear.TabIndex = 19;
            this.cbYear.SelectedIndexChanged += new System.EventHandler(this.cbYear_SelectedIndexChanged);
            // 
            // kryptonLabel12
            // 
            this.kryptonLabel12.Location = new System.Drawing.Point(-3, 19);
            this.kryptonLabel12.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel12.Name = "kryptonLabel12";
            this.kryptonLabel12.Size = new System.Drawing.Size(52, 22);
            this.kryptonLabel12.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel12.TabIndex = 18;
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
            this.cbMonth.Location = new System.Drawing.Point(2, 43);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(127, 27);
            this.cbMonth.TabIndex = 0;
            this.cbMonth.SelectedIndexChanged += new System.EventHandler(this.cbMonth_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnExport);
            this.panel3.Controls.Add(this.lblAccountName);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(906, 33);
            this.panel3.TabIndex = 9;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Location = new System.Drawing.Point(868, 0);
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
            this.btnExport.TabIndex = 10;
            this.btnExport.Values.Image = global::AMSEMS.Properties.Resources.export_16;
            this.btnExport.Values.Text = "";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblAccountName
            // 
            this.lblAccountName.Location = new System.Drawing.Point(-3, 3);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(172, 27);
            this.lblAccountName.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountName.TabIndex = 4;
            this.lblAccountName.Values.Text = "Attendance Report";
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
            this.ptbLoading.Size = new System.Drawing.Size(896, 552);
            this.ptbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ptbLoading.TabIndex = 4;
            this.ptbLoading.TabStop = false;
            this.ptbLoading.Visible = false;
            // 
            // formAttendanceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(946, 768);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formAttendanceReport";
            this.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.Text = "formDashboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formAttendanceReport_FormClosing);
            this.Load += new System.EventHandler(this.formAttendanceReport_Load);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).EndInit();
            this.kryptonGroupBox5.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).EndInit();
            this.kryptonGroupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblMonthlyTotalFees;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox5;
        private System.Windows.Forms.DataGridView dgvReport;
        private System.Windows.Forms.Panel panel5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRefresh;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.ComboBox cbSection;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private System.Windows.Forms.ComboBox cbYear;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel12;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.Panel panel3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExport;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccountName;
        private RoundPictureBoxRect ptbLoading;
    }
}