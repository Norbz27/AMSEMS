using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComponentFactory.Krypton.Toolkit;

namespace AMSEMS.SubForms_Admin
{
    public partial class formSettings : KryptonForm
    {
        private Form activeForm;
        FormAdminNavigation form;
        public formSettings(FormAdminNavigation form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void formSettings_Load(object sender, EventArgs e)
        {
            OpenChildForm(new formAccountSetting());

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }
        public void OpenChildForm(Form childForm)
        {
            if (activeForm != null)
            {
                activeForm.Close();
            }
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.Dock = DockStyle.Fill;
            childForm.FormBorderStyle = FormBorderStyle.None;
            this.panelSetting.Controls.Add(childForm);
            this.panelSetting.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        private void btnAccountProf_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formAccountSetting());
            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnArch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnArch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnArch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAcPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnBackup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnBackup.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnBackup.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnNotif_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formNotificationSetting());
            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnArch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnArch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnArch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAcPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            
            this.btnBackup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnBackup.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnBackup.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnArch_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formArchiveSetting());
            this.btnArch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnArch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnArch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnArch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnArch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAcPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnBackup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnBackup.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnBackup.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnAcPer_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formAcademicYearSetting());
            this.btnAcPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAcPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAcPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAcPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAcPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnArch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnArch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnArch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnBackup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnBackup.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnBackup.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnBackup.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            OpenChildForm(new formBackupSetting());
            this.btnBackup.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnBackup.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnBackup.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnBackup.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnBackup.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnArch.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnArch.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnArch.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnArch.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAcPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            form.Logout();
        }
    }
}
