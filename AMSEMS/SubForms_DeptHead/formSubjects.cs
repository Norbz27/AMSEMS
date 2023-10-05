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

namespace AMSEMS.SubForm_DeptHead
{
    public partial class formSubjects : Form
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool headerCheckboxAdded = false; // Add this flag

        private CheckBox headerCheckbox = new CheckBox();
        public formSubjects()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);

            dgvSubjects.RowsDefaultCellStyle.BackColor = Color.White; // Default row color
            dgvSubjects.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            // Initialize the header checkbox in the constructor
            headerCheckbox.Size = new Size(15, 15);
            headerCheckbox.CheckedChanged += HeaderCheckbox_CheckedChanged;

            // Add the header checkbox to the DataGridView controls
            dgvSubjects.Controls.Add(headerCheckbox);
        }

        private void formSubjects_Load(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;
            toolTip.SetToolTip(btnExport, "Export");
            toolTip.SetToolTip(btnMultiDel, "Delete");

            displayTable("Select Course_code,Course_Description,Units,t.Lastname as teach, al.Academic_Level_Description as Acad from tbl_subjects as s left join tbl_status as st on s.Status = st.Status_ID left join tbl_teacher_accounts as t on s.Assigned_Teacher = t.ID left join tbl_Academic_Level as al on s.Academic_Level = al.Academic_Level_ID Where s.Status = 1");
        }

        private void HeaderCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox headerCheckbox = (CheckBox)sender;

            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    checkBoxCell.Value = headerCheckbox.Checked;
                }
            }

            // Force a refresh of the DataGridView to update the highlighting
            dgvSubjects.Refresh();

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }
        private void UpdatePanelVisibility()
        {
            bool anyChecked = false;

            // Iterate through the DataGridView rows to check if any checkboxes are checked
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell &&
                    (bool)checkBoxCell.Value)
                {
                    anyChecked = true;
                    break; // Exit the loop if at least one checkbox is checked
                }
            }

            // Show or hide the panel based on the state of the checkboxes
            pnControl.Visible = anyChecked;
        }
        public void displayTable(string query)
        {
            try
            {
                dgvSubjects.Rows.Clear();

                using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand(query, cn))
                    {
                        using (SqlDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                // Add a row and set the checkbox column value to false (unchecked)
                                int rowIndex = dgvSubjects.Rows.Add(false);

                                // Populate other columns, starting from index 1
                                dgvSubjects.Rows[rowIndex].Cells["ID"].Value = dr["Course_code"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["Des"].Value = dr["Course_Description"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["units"].Value = dr["Units"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["teach"].Value = dr["teach"].ToString();
                                dgvSubjects.Rows[rowIndex].Cells["acad"].Value = dr["Acad"].ToString();

                                // Populate your control column here (change "ControlColumn" to your actual column name)
                                dgvSubjects.Rows[rowIndex].Cells["option"].Value = option.Image;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvSubjects_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dgvSubjects.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                System.Drawing.Rectangle cellBounds = dgvSubjects.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                CMSOptions.Show(dgvSubjects, cellBounds.Left, cellBounds.Bottom);
            }
            if (e.ColumnIndex == dgvSubjects.Columns["Select"].Index)
            {
                // Checkbox column clicked
                if (dgvSubjects.Rows[e.RowIndex].Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell)
                {
                    // Toggle the checkbox value
                    checkBoxCell.Value = !(bool)checkBoxCell.Value;

                    // Check the state of checkboxes and show/hide the panel accordingly
                    UpdatePanelVisibility();
                }
            }
        }

        private void dgvTeachers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell belongs to the "Select" checkbox column
            if (dgvSubjects.Columns[e.ColumnIndex].Name == "Select")
            {
                return; // Skip formatting for the checkbox column
            }

            // Determine the background color based on the checkbox in the same row
            if (Convert.ToBoolean(dgvSubjects.Rows[e.RowIndex].Cells["Select"].Value) == true)
            {
                // Checkbox is checked, highlight the row
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;
            }
            else
            {
                // Checkbox is unchecked, remove the highlight
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Empty;
            }

            if (dgvSubjects.Rows[e.RowIndex].Selected)
            {
                // Highlight selected rows
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = SystemColors.GradientInactiveCaption;
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Black; // Optional: Change text color
            }
            else
            {
                // Reset the default appearance for unselected rows
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = Color.Empty;
                dgvSubjects.Rows[e.RowIndex].DefaultCellStyle.SelectionForeColor = Color.Empty; // Optional: Reset text color
            }

            // Update the panel visibility based on checkbox states
            UpdatePanelVisibility();
        }

        private void dgvTeachers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == dgvSubjects.Columns["Select"].Index)
            {
                e.PaintBackground(e.CellBounds, true);
                e.PaintContent(e.CellBounds);

                if (!headerCheckboxAdded) // Check if the checkbox has already been added
                {


                    // Center the checkbox within the header cell
                    int x = e.CellBounds.X + (e.CellBounds.Width - headerCheckbox.Width) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - headerCheckbox.Height) / 2;

                    headerCheckbox.Location = new Point(x, y);
                    headerCheckbox.Checked = AreAllCheckboxesChecked();


                    dgvSubjects.Controls.Add(headerCheckbox);

                    headerCheckboxAdded = true; // Set the flag to true
                }
            }
        }
        private bool AreAllCheckboxesChecked()
        {
            foreach (DataGridViewRow row in dgvSubjects.Rows)
            {
                if (row.Cells["Select"] is DataGridViewCheckBoxCell checkBoxCell &&
                    !(bool)checkBoxCell.Value)
                {
                    return false; // At least one checkbox is not checked
                }
            }

            return true; // All checkboxes are checked

        }
    }
}
