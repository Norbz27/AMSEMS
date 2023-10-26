
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
    public partial class formAddEvent : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        UserControlDays_Calendar form;
        string selectedColor;
        public formAddEvent()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            selectedColor = "#800000";
            SetButtonAppearance(btnColorMarron);

        }
        public void getForm(UserControlDays_Calendar form)
        {
            this.form = form;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbEventName.Text.Equals(String.Empty) && tbDescription.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    try
                    {
                        byte[] picData = null;
                        DateTime startDate = DtStart.Value;
                        // Now, check if an image file is selected using OpenFileDialog
                        if (openFileDialog1.FileName != String.Empty)
                        {
                            picData = System.IO.File.ReadAllBytes(openFileDialog1.FileName);
                        }

                        cn.Open();
                        cm = new SqlCommand();
                        cm.Connection = cn;
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.CommandText = "sp_AddEvent";
                        cm.Parameters.AddWithValue("@Event_Name", tbEventName.Text);
                        cm.Parameters.AddWithValue("@Start_Date", DtStart.Value);
                        cm.Parameters.AddWithValue("@End_Date", DtEnd.Value);
                        cm.Parameters.AddWithValue("@Image", picData);
                        cm.Parameters.AddWithValue("@Description", tbDescription.Text);
                        cm.Parameters.AddWithValue("@Color", selectedColor);
                        cm.ExecuteNonQuery();
                        cn.Close();
                        MessageBox.Show("Event Saved!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {

                        form.refresh();
                        this.Dispose();
                    }
                }
            }

        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            lblUpload.Visible = true;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            lblUpload.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
        }

        private void lblUpload_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }
        private bool IsValidDate(int year, int month, int day)
        {
            try
            {
                DateTime date = new DateTime(year, month, day);
                return true; // The date is valid.
            }
            catch (ArgumentOutOfRangeException)
            {
                return false; // The date is not valid.
            }
        }
        private void formAddEvent_Load(object sender, EventArgs e)
        {
            if (IsValidDate(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day))
            {
                DateTime selectedDate = new DateTime(UserControlDays_Calendar.static_year, UserControlDays_Calendar.static_month, UserControlDays_Calendar.static_day);
                DtStart.Value = selectedDate;
                DtEnd.Value = selectedDate;
            }
            else
            {
     
            }
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
            selectedColor = "#800000";
            SetButtonAppearance(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorGreen_Click(object sender, EventArgs e)
        {
            selectedColor = "#009600";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorBlue_Click(object sender, EventArgs e)
        {
            selectedColor = "#07006E";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorPurple_Click(object sender, EventArgs e)
        {
            selectedColor = "#6E002B";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance(btnColorPurple);
            SetButtonAppearance2(btnColorOrange);
        }

        private void btnColorOrange_Click(object sender, EventArgs e)
        {
            selectedColor = "#A63C00";
            SetButtonAppearance2(btnColorMarron);
            SetButtonAppearance2(btnColorGreen);
            SetButtonAppearance2(btnColorBlue);
            SetButtonAppearance2(btnColorPurple);
            SetButtonAppearance(btnColorOrange);
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
