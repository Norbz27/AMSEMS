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

namespace AMSEMS.SubForms_Admin
{
    public partial class formAddSchoolSetting : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        String header, data;
        Boolean isUpdateTrue = false;

        formStudentForm formStudentForm;
        formTeacherForm formTeacherForm;
        public formAddSchoolSetting()
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            lblHeader1.Text = header;
            lblHeader2.Text = "Description:";
            lblHeader3.Text = "List of " + header + ":";

            displayData();
        }

        public void setData(String header)
        {
            this.header = header;
        }

        public void displayData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    dataGridView.Rows.Clear();

                    string selectQuery = "";
                    if (header.Equals("Program"))
                    {
                        selectQuery = "Select * from tbl_program";
                        cn.Open();
                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dataGridView.Rows.Add(dr["Program_ID"].ToString(), dr["Description"].ToString());
                        }
                        dr.Close();
                    }
                    else if (header.Equals("Year Level"))
                    {
                        selectQuery = "Select * from tbl_year_level";
                        cn.Open();
                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dataGridView.Rows.Add(dr["Level_ID"].ToString(), dr["Description"].ToString());
                        }
                        dr.Close();
                    }
                    else if (header.Equals("Section"))
                    {
                        selectQuery = "Select * from tbl_Section";
                        cn.Open();
                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dataGridView.Rows.Add(dr["Section_ID"].ToString(), dr["Description"].ToString());
                        }
                        dr.Close();
                    }
                    else if (header.Equals("Departments"))
                    {
                        selectQuery = "Select * from tbl_Departments";
                        cn.Open();
                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            dataGridView.Rows.Add(dr["Department_ID"].ToString(), dr["Description"].ToString());
                        }
                        dr.Close();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    cn.Close();
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    if (!tbDes.Text.Equals(String.Empty))
                    {
                        if (isUpdateTrue)
                        {
                            if (header.Equals("Program"))
                            {
                                cm = new SqlCommand("Select * from tbl_program where Program_ID = '" + data + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i > 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("UPDATE tbl_program SET Description = @NewValue WHERE Program_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@ConditionValue", ds.Tables[0].Rows[0]["Program_ID"]);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (header.Equals("Year Level"))
                            {
                                cm = new SqlCommand("Select * from tbl_year_level where Level_ID = '" + data + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i > 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("UPDATE tbl_year_level SET Description = @NewValue WHERE Level_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@ConditionValue", ds.Tables[0].Rows[0]["Level_ID"]);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (header.Equals("Section"))
                            {
                                cm = new SqlCommand("Select * from tbl_Section where Section_ID = '" + data + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i > 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("UPDATE tbl_Section SET Description = @NewValue WHERE Section_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@ConditionValue", ds.Tables[0].Rows[0]["Section_ID"]);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                            }
                            else if (header.Equals("Departments"))
                            {
                                cm = new SqlCommand("Select * from tbl_Departments where Department_ID = '" + data + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i > 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("UPDATE tbl_Departments SET Description = @NewValue WHERE Department_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@ConditionValue", ds.Tables[0].Rows[0]["Department_ID"]);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                            }
                        }
                        else
                        {
                            if (header.Equals("Program"))
                            {

                                cm = new SqlCommand("Select * from tbl_program where Description = '" + tbDes.Text + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i == 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("Insert into tbl_program Values (@Des)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                            else if (header.Equals("Year Level"))
                            {
                                cm = new SqlCommand("Select * from tbl_year_level where Description = '" + tbDes.Text + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i == 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("Insert into tbl_year_level Values (@Des)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (header.Equals("Section"))
                            {
                                cm = new SqlCommand("Select * from tbl_Section where Description = '" + tbDes.Text + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i == 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("Insert into tbl_Section Values (@Des)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else if (header.Equals("Departments"))
                            {
                                cm = new SqlCommand("Select * from tbl_Departments where Description = '" + tbDes.Text + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i == 0)
                                {
                                    cn.Open();
                                    cm = new SqlCommand("Insert into tbl_Departments Values (@Des)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                    cn.Close();
                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(header + " is empty!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    displayData();
                    tbDes.Text = String.Empty;
                    isUpdateTrue = false;
                    btnCancel.Visible = false;
                    this.btnAdd.Values.Image = global::AMSEMS.Properties.Resources.plus_16;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (header.Equals("Departments"))
            {
                formTeacherForm.displayDept();
                this.Close();
            }
            else
            {
                formStudentForm.displayPSY();
                this.Close();
            }
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string col = dataGridView.Columns[e.ColumnIndex].Name;
            if (col == "option")
            {
                // Get the bounds of the cell
                Rectangle cellBounds = dataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);

                // Show the context menu just below the cell
                contextMenuStrip2.Show(dataGridView, cellBounds.Left, cellBounds.Bottom);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                // Get the ContextMenuStrip associated with the clicked item
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                if (menu != null)
                {
                    // Get the DataGridView that the context menu is associated with
                    DataGridView dataGridView = menu.SourceControl as DataGridView;

                    if (dataGridView != null)
                    {
                        int rowIndex = dataGridView.CurrentCell.RowIndex;
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];

                        // Ask for confirmation before deleting
                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to delete this data?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            using (cn = new SqlConnection(SQL_Connection.connection))
                            {
                                try
                                {

                                    // Assuming your primary key column is named "ID"
                                    int primaryKeyValue = Convert.ToInt32(rowToDelete.Cells["ID"].Value);
                                    String deleteQuery = "";

                                    if (header.Equals("Program"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_program WHERE Program_ID = @ID";
                                    }
                                    else if (header.Equals("Year Level"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_year_level WHERE Level_ID = @ID";
                                    }
                                    else if (header.Equals("Section"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_Section WHERE Section_ID = @ID";
                                    }
                                    else if (header.Equals("Departments"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_Departments WHERE Department_ID = @ID";
                                    }

                                    using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                    {
                                        // Add parameter for the primary key value
                                        command.Parameters.AddWithValue("@ID", primaryKeyValue);
                                        cn.Open();
                                        command.ExecuteNonQuery();

                                        MessageBox.Show(header + " deleted successfully.");
                                        displayData();
                                        cn.Close();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Error deleting record: " + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = true;
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem != null)
            {
                // Get the ContextMenuStrip associated with the clicked item
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;

                if (menu != null)
                {
                    // Get the DataGridView that the context menu is associated with
                    DataGridView dataGridView = menu.SourceControl as DataGridView;

                    if (dataGridView != null)
                    {
                        int rowIndex = dataGridView.CurrentCell.RowIndex;
                        DataGridViewRow rowToDelete = dataGridView.Rows[rowIndex];
                        tbDes.Text = dataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                        data = dataGridView.Rows[rowIndex].Cells[0].Value.ToString(); ;
                        isUpdateTrue = true;
                        this.btnAdd.Values.Image = global::AMSEMS.Properties.Resources.refresh_16;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbDes.Text = String.Empty;
            isUpdateTrue = false;
            btnCancel.Visible = false;
            this.btnAdd.Values.Image = global::AMSEMS.Properties.Resources.plus_16;
        }
    }
}
