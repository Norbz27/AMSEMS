using ComponentFactory.Krypton.Toolkit;
using System.Windows.Forms;

namespace AMSEMS.SubForm_Guidance
{
    public partial class formSettings : KryptonForm
    {
        FormGuidanceNavigation FormGuidanceNavigation;
        private Form activeForm;
        public formSettings(FormGuidanceNavigation formGuidanceNavigation)
        {
            InitializeComponent();
            this.FormGuidanceNavigation = formGuidanceNavigation;
        }

        private void btnAccountProf_Click(object sender, System.EventArgs e)
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

            this.btnAcadPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcadPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcadPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcadPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcadPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }

        private void btnNotif_Click(object sender, System.EventArgs e)
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

            this.btnAcadPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcadPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAcadPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAcadPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAcadPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
        }
        private void btnAcadPer_Click(object sender, System.EventArgs e)
        {
            OpenChildForm(new formAcademicYearSetting());
            this.btnAcadPer.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAcadPer.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAcadPer.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAcadPer.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAcadPer.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;

            this.btnNotif.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnNotif.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnNotif.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnNotif.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(247)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.DarkDark;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.DarkGray;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
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
        private void btnLogout_Click(object sender, System.EventArgs e)
        {
            FormGuidanceNavigation.Logout();
        }
    }
}
