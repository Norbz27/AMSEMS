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
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AMSEMS.SubForms_SAO
{
    public partial class formEventAddConfig : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;

        public bool isCollapsed;
        private List<string> suggestions = new List<string>{};
        private ListBox listBoxSuggestions;
        public formEventAddConfig()
        {
            InitializeComponent();
            InitializeListBox();
            formAddEvent.students.Clear();

            displayCBData();
            if (cbExclusive.Items.Count > 0)
            {
                cbExclusive.SelectedIndex = 0;
            }
        }
        public void displayCBData()
        {
            cbExclusive.Items.Clear();
            cbExclusive.Items.Add("All Students");
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                cm = new SqlCommand("Select Description from tbl_Departments", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    cbExclusive.Items.Add(dr["Description"].ToString());
                }
                dr.Close();
            }
            cbExclusive.Items.Add("Specific Students");
        }

        public void CollapseForm()
        {
            if (isCollapsed)
            {
                pnExclusive.Height += 10;
                if (pnExclusive.Size == pnExclusive.MaximumSize)
                {
                    timer1.Stop();
                    isCollapsed = false;
                }
            }
            else
            {
                pnExclusive.Height -= 10;
                if (pnExclusive.Size == pnExclusive.MinimumSize)
                {
                    timer1.Stop();
                    isCollapsed = true;
                }
            }
        }
        private void InitializeListBox()
        {
            listBoxSuggestions = new ListBox
            {
                Visible = false,
                Width = tbSearch.Width,
                Height = 50,
                Location = new Point(tbSearch.Left, tbSearch.Bottom + 5),
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
            };

            listBoxSuggestions.DoubleClick += listBoxSuggestions_DoubleClick;
            listBoxSuggestions.KeyDown += listBoxSuggestions_KeyDown;

            pnExclusive.Controls.Add(listBoxSuggestions);

            listBoxSuggestions.BringToFront();

        }
        private void listBoxSuggestions_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    tbSearch.Text = selectedSuggestion;

                    if (!IsSuggestionInFlowLayoutPanel(selectedSuggestion))
                    {
                        AddSuggestionToFlowLayoutPanel(selectedSuggestion);
                    }
                    tbSearch.Text = string.Empty;
                }
            }
        }
        private void listBoxSuggestions_DoubleClick(object sender, EventArgs e)
        {
            string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedSuggestion))
            {
                tbSearch.Text = selectedSuggestion;

                if (!IsSuggestionInFlowLayoutPanel(selectedSuggestion))
                {
                    AddSuggestionToFlowLayoutPanel(selectedSuggestion);
                }
                tbSearch.Text = string.Empty;
            }
        }
        private void AddSuggestionToFlowLayoutPanel(string suggestion)
        {
            // Create a label for the suggestion
            Label label = new Label
            {
                AutoSize = true,
                Text = suggestion,
                Margin = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle
            };

            // Add the label to the existing FlowLayoutPanel (flowLayoutPanel1)
            flowLayoutPanel1.Controls.Add(label);
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            CollapseForm();
        }

        private void cbExclusive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbExclusive.Text.Equals("Specific Students"))
            {
                isCollapsed = true;
                timer1.Start();
            }
            else
            {
                isCollapsed = false;
                timer1.Start();
            }

            formAddEvent.exclusive = cbExclusive.Text;
        }

        private void tbSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateSuggestions(tbSearch.Text);
        }
        private void UpdateSuggestions(string searchText)
        {
            suggestions.Clear();

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();

                string query = "SELECT CONCAT(Firstname, ' ', Lastname) AS FullName FROM tbl_student_accounts WHERE Status = 1";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            suggestions.Add(reader["FullName"].ToString());
                        }
                    }
                }
            }

            var filteredSuggestions = suggestions
                .Where(s => s.ToLower().Contains(searchText.ToLower()))
                .ToArray();

            listBoxSuggestions.DataSource = filteredSuggestions;

            // Update the visibility of the ListBox based on whether the search text is empty
            listBoxSuggestions.Visible = !string.IsNullOrWhiteSpace(searchText) && filteredSuggestions.Length > 0;
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string selectedSuggestion = listBoxSuggestions.SelectedItem?.ToString();

                if (!string.IsNullOrEmpty(selectedSuggestion))
                {
                    tbSearch.Text = selectedSuggestion;

                    if (!IsSuggestionInFlowLayoutPanel(selectedSuggestion))
                    {
                        AddSuggestionToFlowLayoutPanel(selectedSuggestion);
                    }
                    tbSearch.Text = string.Empty;
                }
            }
        }
        private bool IsSuggestionInFlowLayoutPanel(string suggestion)
        {
            return flowLayoutPanel1.Controls.OfType<Label>().Any(label => label.Text.Equals(suggestion));
        }

        private void tgbtnAtt_CheckedChanged(object sender, EventArgs e)
        {
            if(tgbtnAtt.Checked == true)
            {
                formAddEvent.attendance = true;
                tgbtnPenalty.Enabled = true;
            }
            else
            {
                formAddEvent.attendance = false;
                tgbtnPenalty.Enabled = false;
                tgbtnPenalty.Checked = false;
            }
        }
        private void tgbtnPenalty_CheckedChanged(object sender, EventArgs e)
        {
            if (tgbtnPenalty.Checked == true)
            {
                formAddEvent.penalty = true;
            }
            else
            {
                formAddEvent.penalty = false;
            }
        }
        private void formEventConfig_Load(object sender, EventArgs e)
        {
            if (tgbtnAtt.Checked == true)
            {
                formAddEvent.attendance = true;
            }
            else
            {
                formAddEvent.attendance = false;
            }

            if (tgbtnPenalty.Checked == true)
            {
                formAddEvent.penalty = true;
            }
            else
            {
                formAddEvent.penalty = false;
            }
        }

        private void cbExclusive_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
    }
}
