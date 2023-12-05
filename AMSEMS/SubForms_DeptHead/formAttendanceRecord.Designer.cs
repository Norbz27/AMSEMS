namespace AMSEMS.SubForms_DeptHead
{
    partial class formAttendanceRecord
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.kryptonGroupBox5 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.dgvRecord = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.event_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.penalty_am = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.am_login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.am_logout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.penalty_pm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pm_login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pm_logout = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checker = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.penalty_total = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblAmPenaltyFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblPmPenaltyFee = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lblTotalFees = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.Dt = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tbSearch = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.cbEvents = new System.Windows.Forms.ComboBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSection = new System.Windows.Forms.ComboBox();
            this.kryptonLabel12 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnRefresh = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnPenaltyFee = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnExport = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.lblAccountName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ptbLoading = new AMSEMS.RoundPictureBoxRect();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).BeginInit();
            this.kryptonGroupBox5.Panel.SuspendLayout();
            this.kryptonGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecord)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.kryptonGroupBox5);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(20, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(906, 738);
            this.panel1.TabIndex = 0;
            // 
            // kryptonGroupBox5
            // 
            this.kryptonGroupBox5.CaptionVisible = false;
            this.kryptonGroupBox5.CausesValidation = false;
            this.kryptonGroupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonGroupBox5.Location = new System.Drawing.Point(0, 185);
            this.kryptonGroupBox5.Name = "kryptonGroupBox5";
            // 
            // kryptonGroupBox5.Panel
            // 
            this.kryptonGroupBox5.Panel.Controls.Add(this.ptbLoading);
            this.kryptonGroupBox5.Panel.Controls.Add(this.dgvRecord);
            this.kryptonGroupBox5.Size = new System.Drawing.Size(906, 553);
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
            // dgvRecord
            // 
            this.dgvRecord.AllowUserToAddRows = false;
            this.dgvRecord.AllowUserToDeleteRows = false;
            this.dgvRecord.AllowUserToResizeColumns = false;
            this.dgvRecord.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvRecord.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecord.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRecord.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvRecord.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecord.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRecord.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvRecord.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.dgvRecord.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Poppins SemiBold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecord.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecord.ColumnHeadersHeight = 50;
            this.dgvRecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRecord.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.name,
            this.section,
            this.event_date,
            this.penalty_am,
            this.am_login,
            this.am_logout,
            this.penalty_pm,
            this.pm_login,
            this.pm_logout,
            this.checker,
            this.penalty_total});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecord.DefaultCellStyle = dataGridViewCellStyle10;
            this.dgvRecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecord.EnableHeadersVisualStyles = false;
            this.dgvRecord.GridColor = System.Drawing.SystemColors.ControlDarkDark;
            this.dgvRecord.Location = new System.Drawing.Point(0, 0);
            this.dgvRecord.MultiSelect = false;
            this.dgvRecord.Name = "dgvRecord";
            this.dgvRecord.ReadOnly = true;
            this.dgvRecord.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecord.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvRecord.RowHeadersVisible = false;
            this.dgvRecord.RowHeadersWidth = 30;
            this.dgvRecord.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecord.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvRecord.RowTemplate.Height = 30;
            this.dgvRecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecord.Size = new System.Drawing.Size(896, 543);
            this.dgvRecord.TabIndex = 1;
            // 
            // ID
            // 
            this.ID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 45;
            // 
            // name
            // 
            this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // section
            // 
            this.section.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.section.HeaderText = "Section";
            this.section.Name = "section";
            this.section.ReadOnly = true;
            this.section.Width = 78;
            // 
            // event_date
            // 
            this.event_date.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.event_date.HeaderText = "Date";
            this.event_date.Name = "event_date";
            this.event_date.ReadOnly = true;
            this.event_date.Width = 62;
            // 
            // penalty_am
            // 
            this.penalty_am.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.penalty_am.DefaultCellStyle = dataGridViewCellStyle3;
            this.penalty_am.HeaderText = "AM Penalty Fee";
            this.penalty_am.Name = "penalty_am";
            this.penalty_am.ReadOnly = true;
            this.penalty_am.Width = 113;
            // 
            // am_login
            // 
            this.am_login.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.am_login.DefaultCellStyle = dataGridViewCellStyle4;
            this.am_login.HeaderText = "AM IN";
            this.am_login.Name = "am_login";
            this.am_login.ReadOnly = true;
            this.am_login.Width = 62;
            // 
            // am_logout
            // 
            this.am_logout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.am_logout.DefaultCellStyle = dataGridViewCellStyle5;
            this.am_logout.HeaderText = "AM OUT";
            this.am_logout.Name = "am_logout";
            this.am_logout.ReadOnly = true;
            this.am_logout.Width = 73;
            // 
            // penalty_pm
            // 
            this.penalty_pm.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.penalty_pm.DefaultCellStyle = dataGridViewCellStyle6;
            this.penalty_pm.HeaderText = "PM Penalty Fee";
            this.penalty_pm.Name = "penalty_pm";
            this.penalty_pm.ReadOnly = true;
            this.penalty_pm.Width = 112;
            // 
            // pm_login
            // 
            this.pm_login.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.pm_login.DefaultCellStyle = dataGridViewCellStyle7;
            this.pm_login.HeaderText = "PM IN";
            this.pm_login.Name = "pm_login";
            this.pm_login.ReadOnly = true;
            this.pm_login.Width = 61;
            // 
            // pm_logout
            // 
            this.pm_logout.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.pm_logout.DefaultCellStyle = dataGridViewCellStyle8;
            this.pm_logout.HeaderText = "PM OUT";
            this.pm_logout.Name = "pm_logout";
            this.pm_logout.ReadOnly = true;
            this.pm_logout.Width = 72;
            // 
            // checker
            // 
            this.checker.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.checker.HeaderText = "Checker";
            this.checker.Name = "checker";
            this.checker.ReadOnly = true;
            this.checker.Width = 83;
            // 
            // penalty_total
            // 
            this.penalty_total.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Poppins", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.penalty_total.DefaultCellStyle = dataGridViewCellStyle9;
            this.penalty_total.HeaderText = "Total Penalty Fee";
            this.penalty_total.Name = "penalty_total";
            this.penalty_total.ReadOnly = true;
            this.penalty_total.Width = 105;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblAmPenaltyFee);
            this.panel3.Controls.Add(this.kryptonLabel5);
            this.panel3.Controls.Add(this.kryptonLabel1);
            this.panel3.Controls.Add(this.lblPmPenaltyFee);
            this.panel3.Controls.Add(this.kryptonLabel6);
            this.panel3.Controls.Add(this.lblTotalFees);
            this.panel3.Controls.Add(this.kryptonLabel3);
            this.panel3.Controls.Add(this.Dt);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.tbSearch);
            this.panel3.Controls.Add(this.cbEvents);
            this.panel3.Controls.Add(this.kryptonLabel2);
            this.panel3.Controls.Add(this.cbSection);
            this.panel3.Controls.Add(this.kryptonLabel12);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 33);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(906, 152);
            this.panel3.TabIndex = 10;
            // 
            // lblAmPenaltyFee
            // 
            this.lblAmPenaltyFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAmPenaltyFee.Location = new System.Drawing.Point(501, 52);
            this.lblAmPenaltyFee.Margin = new System.Windows.Forms.Padding(2);
            this.lblAmPenaltyFee.Name = "lblAmPenaltyFee";
            this.lblAmPenaltyFee.Size = new System.Drawing.Size(78, 31);
            this.lblAmPenaltyFee.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAmPenaltyFee.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAmPenaltyFee.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins Medium", 13F, System.Drawing.FontStyle.Bold);
            this.lblAmPenaltyFee.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.lblAmPenaltyFee.TabIndex = 148;
            this.lblAmPenaltyFee.Values.Text = "₱ 00.00";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonLabel5.Location = new System.Drawing.Point(474, 28);
            this.kryptonLabel5.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(113, 23);
            this.kryptonLabel5.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel5.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel5.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F);
            this.kryptonLabel5.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonLabel5.TabIndex = 147;
            this.kryptonLabel5.Values.Text = "AM Penalty Fee";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(-3, 86);
            this.kryptonLabel1.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(42, 22);
            this.kryptonLabel1.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel1.TabIndex = 146;
            this.kryptonLabel1.Values.Text = "Date";
            // 
            // lblPmPenaltyFee
            // 
            this.lblPmPenaltyFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPmPenaltyFee.Location = new System.Drawing.Point(653, 53);
            this.lblPmPenaltyFee.Margin = new System.Windows.Forms.Padding(2);
            this.lblPmPenaltyFee.Name = "lblPmPenaltyFee";
            this.lblPmPenaltyFee.Size = new System.Drawing.Size(78, 31);
            this.lblPmPenaltyFee.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblPmPenaltyFee.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblPmPenaltyFee.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins Medium", 13F, System.Drawing.FontStyle.Bold);
            this.lblPmPenaltyFee.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.lblPmPenaltyFee.TabIndex = 145;
            this.lblPmPenaltyFee.Values.Text = "₱ 00.00";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonLabel6.Location = new System.Drawing.Point(627, 29);
            this.kryptonLabel6.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(112, 23);
            this.kryptonLabel6.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel6.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel6.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F);
            this.kryptonLabel6.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonLabel6.TabIndex = 144;
            this.kryptonLabel6.Values.Text = "PM Penalty Fee";
            // 
            // lblTotalFees
            // 
            this.lblTotalFees.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalFees.Location = new System.Drawing.Point(807, 52);
            this.lblTotalFees.Margin = new System.Windows.Forms.Padding(2);
            this.lblTotalFees.Name = "lblTotalFees";
            this.lblTotalFees.Size = new System.Drawing.Size(78, 31);
            this.lblTotalFees.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblTotalFees.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblTotalFees.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins Medium", 13F, System.Drawing.FontStyle.Bold);
            this.lblTotalFees.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.lblTotalFees.TabIndex = 143;
            this.lblTotalFees.Values.Text = "₱ 00.00";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonLabel3.Location = new System.Drawing.Point(777, 29);
            this.kryptonLabel3.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(116, 23);
            this.kryptonLabel3.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel3.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel3.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 10F);
            this.kryptonLabel3.StateCommon.ShortText.TextH = ComponentFactory.Krypton.Toolkit.PaletteRelativeAlign.Far;
            this.kryptonLabel3.TabIndex = 142;
            this.kryptonLabel3.Values.Text = "Daily Total Fees";
            // 
            // Dt
            // 
            this.Dt.Location = new System.Drawing.Point(2, 110);
            this.Dt.MinDate = new System.DateTime(2000, 1, 1, 0, 0, 0, 0);
            this.Dt.Name = "Dt";
            this.Dt.PaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
            this.Dt.Size = new System.Drawing.Size(222, 29);
            this.Dt.StateActive.Border.Color1 = System.Drawing.Color.Gray;
            this.Dt.StateActive.Border.Color2 = System.Drawing.Color.Gray;
            this.Dt.StateActive.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.Dt.StateCommon.Back.Color1 = System.Drawing.Color.White;
            this.Dt.StateCommon.Border.Color1 = System.Drawing.Color.LightGray;
            this.Dt.StateCommon.Border.Color2 = System.Drawing.Color.LightGray;
            this.Dt.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.Dt.StateCommon.Border.Rounding = 2;
            this.Dt.StateCommon.Content.Color1 = System.Drawing.Color.Black;
            this.Dt.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 8.75F);
            this.Dt.TabIndex = 141;
            this.Dt.ValueChanged += new System.EventHandler(this.Dt_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBox1.Image = global::AMSEMS.Properties.Resources.search_16;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(690, 117);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 67;
            this.pictureBox1.TabStop = false;
            // 
            // tbSearch
            // 
            this.tbSearch.AcceptsReturn = true;
            this.tbSearch.AlwaysActive = false;
            this.tbSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbSearch.Location = new System.Drawing.Point(683, 110);
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
            this.tbSearch.TabIndex = 68;
            this.tbSearch.TextChanged += new System.EventHandler(this.tbSearch_TextChanged);
            // 
            // cbEvents
            // 
            this.cbEvents.Font = new System.Drawing.Font("Poppins", 8F);
            this.cbEvents.FormattingEnabled = true;
            this.cbEvents.Location = new System.Drawing.Point(2, 42);
            this.cbEvents.Name = "cbEvents";
            this.cbEvents.Size = new System.Drawing.Size(191, 27);
            this.cbEvents.TabIndex = 28;
            this.cbEvents.SelectedIndexChanged += new System.EventHandler(this.cbEvents_SelectedIndexChanged);
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(212, 18);
            this.kryptonLabel2.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(59, 22);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel2.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel2.TabIndex = 22;
            this.kryptonLabel2.Values.Text = "Section";
            // 
            // cbSection
            // 
            this.cbSection.Font = new System.Drawing.Font("Poppins", 8F);
            this.cbSection.FormattingEnabled = true;
            this.cbSection.Location = new System.Drawing.Point(217, 42);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(134, 27);
            this.cbSection.TabIndex = 21;
            this.cbSection.SelectedIndexChanged += new System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // kryptonLabel12
            // 
            this.kryptonLabel12.Location = new System.Drawing.Point(-3, 18);
            this.kryptonLabel12.Margin = new System.Windows.Forms.Padding(2);
            this.kryptonLabel12.Name = "kryptonLabel12";
            this.kryptonLabel12.Size = new System.Drawing.Size(46, 22);
            this.kryptonLabel12.StateCommon.ShortText.Color1 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Color2 = System.Drawing.Color.Black;
            this.kryptonLabel12.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 9.5F);
            this.kryptonLabel12.TabIndex = 18;
            this.kryptonLabel12.Values.Text = "Event";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Controls.Add(this.btnPenaltyFee);
            this.panel2.Controls.Add(this.btnExport);
            this.panel2.Controls.Add(this.lblAccountName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(906, 33);
            this.panel2.TabIndex = 9;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.Location = new System.Drawing.Point(783, 0);
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
            this.btnRefresh.Size = new System.Drawing.Size(37, 34);
            this.btnRefresh.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnRefresh.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnRefresh.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnRefresh.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnRefresh.StateCommon.Border.Rounding = 10;
            this.btnRefresh.StateCommon.Border.Width = 1;
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
            this.btnRefresh.TabIndex = 11;
            this.btnRefresh.Values.Image = global::AMSEMS.Properties.Resources.refresh_16;
            this.btnRefresh.Values.Text = "";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnPenaltyFee
            // 
            this.btnPenaltyFee.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPenaltyFee.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPenaltyFee.Location = new System.Drawing.Point(869, -1);
            this.btnPenaltyFee.Name = "btnPenaltyFee";
            this.btnPenaltyFee.OverrideDefault.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.OverrideDefault.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.OverrideDefault.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPenaltyFee.OverrideDefault.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            this.btnPenaltyFee.OverrideDefault.Border.Rounding = 10;
            this.btnPenaltyFee.OverrideDefault.Border.Width = 1;
            this.btnPenaltyFee.OverrideDefault.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnPenaltyFee.OverrideDefault.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPenaltyFee.OverrideDefault.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnPenaltyFee.OverrideFocus.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.OverrideFocus.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.OverrideFocus.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPenaltyFee.OverrideFocus.Border.Rounding = 10;
            this.btnPenaltyFee.Size = new System.Drawing.Size(37, 34);
            this.btnPenaltyFee.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnPenaltyFee.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnPenaltyFee.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPenaltyFee.StateCommon.Border.Rounding = 10;
            this.btnPenaltyFee.StateCommon.Border.Width = 1;
            this.btnPenaltyFee.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            this.btnPenaltyFee.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPenaltyFee.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnPenaltyFee.StateCommon.Content.ShortText.Font = new System.Drawing.Font("Poppins", 9F);
            this.btnPenaltyFee.StateCommon.Content.ShortText.ImageStyle = ComponentFactory.Krypton.Toolkit.PaletteImageStyle.CenterLeft;
            this.btnPenaltyFee.StatePressed.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.StatePressed.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnPenaltyFee.StatePressed.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPenaltyFee.StatePressed.Border.Rounding = 10;
            this.btnPenaltyFee.StatePressed.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnPenaltyFee.StatePressed.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPenaltyFee.StatePressed.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnPenaltyFee.StateTracking.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnPenaltyFee.StateTracking.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(79)))), ((int)(((byte)(161)))));
            this.btnPenaltyFee.StateTracking.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left) 
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            this.btnPenaltyFee.StateTracking.Border.Rounding = 10;
            this.btnPenaltyFee.StateTracking.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnPenaltyFee.StateTracking.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnPenaltyFee.StateTracking.Content.ShortText.Color2 = System.Drawing.Color.White;
            this.btnPenaltyFee.TabIndex = 10;
            this.btnPenaltyFee.Values.Image = global::AMSEMS.Properties.Resources.money;
            this.btnPenaltyFee.Values.Text = "";
            this.btnPenaltyFee.Click += new System.EventHandler(this.btnPenaltyFee_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.Location = new System.Drawing.Point(826, -1);
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
            // lblAccountName
            // 
            this.lblAccountName.Location = new System.Drawing.Point(-3, 3);
            this.lblAccountName.Name = "lblAccountName";
            this.lblAccountName.Size = new System.Drawing.Size(176, 27);
            this.lblAccountName.StateCommon.ShortText.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.lblAccountName.StateCommon.ShortText.Font = new System.Drawing.Font("Poppins", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccountName.TabIndex = 4;
            this.lblAccountName.Values.Text = "Attendance Record";
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
            this.ptbLoading.Size = new System.Drawing.Size(896, 543);
            this.ptbLoading.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ptbLoading.TabIndex = 3;
            this.ptbLoading.TabStop = false;
            this.ptbLoading.Visible = false;
            // 
            // formAttendanceRecord
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(946, 768);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "formAttendanceRecord";
            this.Padding = new System.Windows.Forms.Padding(20, 15, 20, 15);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.formAttendanceRecord_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.formAttendanceRecord_FormClosed);
            this.Load += new System.EventHandler(this.formAttendanceRecord_Load);
            this.Resize += new System.EventHandler(this.formAttendanceRecord_Resize);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).EndInit();
            this.kryptonGroupBox5.Panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).EndInit();
            this.kryptonGroupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecord)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAccountName;
        private System.Windows.Forms.Panel panel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private System.Windows.Forms.ComboBox cbSection;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel12;
        private ComponentFactory.Krypton.Toolkit.KryptonGroupBox kryptonGroupBox5;
        private System.Windows.Forms.ComboBox cbEvents;
        private System.Windows.Forms.DataGridView dgvRecord;
        private System.Windows.Forms.PictureBox pictureBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox tbSearch;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker Dt;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExport;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblTotalFees;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnPenaltyFee;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblPmPenaltyFee;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lblAmPenaltyFee;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnRefresh;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn section;
        private System.Windows.Forms.DataGridViewTextBoxColumn event_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn penalty_am;
        private System.Windows.Forms.DataGridViewTextBoxColumn am_login;
        private System.Windows.Forms.DataGridViewTextBoxColumn am_logout;
        private System.Windows.Forms.DataGridViewTextBoxColumn penalty_pm;
        private System.Windows.Forms.DataGridViewTextBoxColumn pm_login;
        private System.Windows.Forms.DataGridViewTextBoxColumn pm_logout;
        private System.Windows.Forms.DataGridViewTextBoxColumn checker;
        private System.Windows.Forms.DataGridViewTextBoxColumn penalty_total;
        private RoundPictureBoxRect ptbLoading;
    }
}