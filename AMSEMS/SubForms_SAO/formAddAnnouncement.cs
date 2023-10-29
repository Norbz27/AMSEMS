
using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace AMSEMS.SubForms_SAO
{
    public partial class formAddAnnouncement : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        formAnnouncement form;
        string selectedColor;
        string announceBy;
        public formAddAnnouncement()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            announceBy = "Student Association Office(SAO)";


        }
        public void getForm(formAnnouncement form)
        {
            this.form = form;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbAnnounceTitle.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        DateTime dateTime = DateTime.Now;
                        cn.Open();
                        cm = new SqlCommand("Insert Into tbl_Announcement Values (@Title,@Des,@DateTime,@AnnounceBy)", cn);
                        cm.Parameters.AddWithValue("@Title", tbAnnounceTitle.Text);
                        cm.Parameters.AddWithValue("@Des", tbDescription.Text);
                        cm.Parameters.AddWithValue("@DateTime", dateTime);
                        cm.Parameters.AddWithValue("@AnnounceBy", announceBy);
                        cm.ExecuteNonQuery();

                        MessageBox.Show("Announcement Forwarded!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        form.displayAnnouncements();
                        this.Dispose();
                    }
                }
            }

        }
        private void formAddEvent_Load(object sender, EventArgs e)
        {
            //if (IsValidDate(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day))
            //{
            //    DateTime selectedDate = new DateTime(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day);
            //    DtStart.Value = selectedDate;
            //    DtEnd.Value = selectedDate;
            //}
            //else
            //{
     
            //}
        }
        private void SetButtonAppearance(KryptonButton button)
        {
            button.StateCommon.Back.Color1 = System.Drawing.Color.Silver;
            button.StateCommon.Back.Color2 = System.Drawing.Color.Silver;
            button.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.OverrideDefault.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            button.StateCommon.Border.GraphicsHint = ComponentFactory.Krypton.Toolkit.PaletteGraphicsHint.AntiAlias;
            button.StateCommon.Border.Width = 1;
            button.StateCommon.Content.Image.Effect = ComponentFactory.Krypton.Toolkit.PaletteImageEffect.Normal;
            button.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            button.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }

        private void SetButtonAppearance2(KryptonButton button)
        {
            button.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Back.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            button.StateCommon.Border.DrawBorders = ((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders)((((ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Top | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Bottom)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Left)
            | ComponentFactory.Krypton.Toolkit.PaletteDrawBorders.Right)));
            button.StateCommon.Border.Rounding = 10;
            button.StateCommon.Border.Width = 1;
            button.StateCommon.Content.Padding = new System.Windows.Forms.Padding(2, -1, -1, -1);
            button.StateCommon.Content.ShortText.Color1 = System.Drawing.Color.White;
            button.StateCommon.Content.ShortText.Color2 = System.Drawing.Color.White;
        }

        private void btnColorMarron_Click(object sender, EventArgs e)
        {
            //selectedColor = "#800000";
            //SetButtonAppearance(btnColorMarron);
            //SetButtonAppearance2(btnColorGreen);
            //SetButtonAppearance2(btnColorBlue);
            //SetButtonAppearance2(btnColorPurple);
            //SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorGreen_Click(object sender, EventArgs e)
        {
            //selectedColor = "#009600";
            //SetButtonAppearance2(btnColorMarron);
            //SetButtonAppearance(btnColorGreen);
            //SetButtonAppearance2(btnColorBlue);
            //SetButtonAppearance2(btnColorPurple);
            //SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorBlue_Click(object sender, EventArgs e)
        {
            //selectedColor = "#07006E";
            //SetButtonAppearance2(btnColorMarron);
            //SetButtonAppearance2(btnColorGreen);
            //SetButtonAppearance(btnColorBlue);
            //SetButtonAppearance2(btnColorPurple);
            //SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorPurple_Click(object sender, EventArgs e)
        {
            //selectedColor = "#6E002B";
            //SetButtonAppearance2(btnColorMarron);
            //SetButtonAppearance2(btnColorGreen);
            //SetButtonAppearance2(btnColorBlue);
            //SetButtonAppearance(btnColorPurple);
            //SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorOrange_Click(object sender, EventArgs e)
        {
            //selectedColor = "#A63C00";
            //SetButtonAppearance2(btnColorMarron);
            //SetButtonAppearance2(btnColorGreen);
            //SetButtonAppearance2(btnColorBlue);
            //SetButtonAppearance2(btnColorPurple);
            //SetButtonAppearance(btnColorOrange);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
