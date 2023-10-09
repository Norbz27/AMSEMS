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

namespace AMSEMS.SubForms_SAO
{
    public partial class formSettings : KryptonForm
    {
        private Form activeForm;
        FormSAONavigation form;
        public formSettings(FormSAONavigation form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            form.Logout();
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

        private void formSettings_Load(object sender, EventArgs e)
        {
            OpenChildForm(new formAccountSetting());

            this.btnAccountProf.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            this.btnAccountProf.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            this.btnAccountProf.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            this.btnAccountProf.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }
    }
}
