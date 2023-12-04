namespace AMSEMS.SubForm_Guidance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblRepHeader = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.kryptonGroupBox5 = new ComponentFactory.Krypton.Toolkit.KryptonGroupBox();
            this.dgvAbesnteismRep = new System.Windows.Forms.DataGridView();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.cbSection = new System.Windows.Forms.ComboBox();
            this.studid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.studname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.section = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.absences = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.option = new System.Windows.Forms.DataGridViewImageColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonGroupBox5.Panel)).BeginInit();
            this.kryptonGroupBox5.Panel.SuspendLayout();
            this.kryptonGroupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAbesnteismRep)).BeginInit();
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
            this.panel1.Controls.Add(this.lblRepHeader);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(20, 15);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(788, 36);
            this.panel1.TabIndex = 7;
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
            this.kryptonGroupBox5.Panel.Controls.Add(this.dgvAbesnteismRep);
            this.kryptonGroupBox5.Size = new System.Drawing.Size(787, 416);
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
            this.studname,
            this.section,
            this.absences,
            this.option});
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
            this.dgvAbesnteismRep.Size = new System.Drawing.Size(777, 406);
            this.dgvAbesnteismRep.TabIndex = 1;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(19, 60);
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
            this.cbSection.Location = new System.Drawing.Point(24, 84);
            this.cbSection.Name = "cbSection";
            this.cbSection.Size = new System.Drawing.Size(108, 27);
            this.cbSection.TabIndex = 38;
            this.cbSection.SelectedIndexChanged += new System.EventHandler(this.cbSection_SelectedIndexChanged);
            // 
            // studid
            // 
            this.studid.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.studid.HeaderText = "ID";
            this.studid.Name = "studid";
            this.studid.ReadOnly = true;
            this.studid.Width = 45;
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
            // absences
            // 
            this.absences.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.absences.HeaderText = "Absences";
            this.absences.Name = "absences";
            this.absences.ReadOnly = true;
            this.absences.Width = 90;
            // 
            // option
            // 
            this.option.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.option.HeaderText = "";
            this.option.Image = global::AMSEMS.Properties.Resources.option;
            this.option.Name = "option";
            this.option.ReadOnly = true;
            this.option.Width = 5;
            // 
            // formAbsReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(828, 549);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn studid;
        private System.Windows.Forms.DataGridViewTextBoxColumn studname;
        private System.Windows.Forms.DataGridViewTextBoxColumn section;
        private System.Windows.Forms.DataGridViewTextBoxColumn absences;
        private System.Windows.Forms.DataGridViewImageColumn option;
    }
}