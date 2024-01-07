﻿using ComponentFactory.Krypton.Toolkit;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formAddSchoolSetting2 : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();

        string header;
        string data;
        Boolean isUpdateTrue = false;

        formStudentForm formStudentForm;
        formTeacherForm formTeacherForm;
        formSubjectsForm formSubjectsForm;
        formSchoolDetails formSchoolDetails;
        public formAddSchoolSetting2(formStudentForm formStudentForm, formTeacherForm formTeacherForm, formSubjectsForm formSubjectsForm, formSchoolDetails formSchoolDetails)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);

            this.formStudentForm = formStudentForm;
            this.formTeacherForm = formTeacherForm;
            this.formSubjectsForm = formSubjectsForm;
            this.formSchoolDetails = formSchoolDetails;
        }

        public void setDisplayData(String header)
        {
            this.header = header;
            lblHeader1.Text = header;
            lblHeader2.Text = "Description:";
            lblHeader3.Text = "List of " + header + ":";
            displayData();
        }

        public void displayData()
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    using (cn = new SqlConnection(SQL_Connection.connection))
                    {
                        dataGridView.Rows.Clear();
                        cbAcadLevel.Items.Clear();
                        string selectQuery = "";
                        selectQuery = "Select * from tbl_academic_level";
                        cn.Open();
                        cm = new SqlCommand(selectQuery, cn);
                        dr = cm.ExecuteReader();
                        while (dr.Read())
                        {
                            cbAcadLevel.Items.Add(dr["Academic_Level_Description"].ToString());
                        }
                        dr.Close();

                        if (header.Equals("Departments"))
                        {
                            selectQuery = "Select * from tbl_Departments p left join tbl_academic_level l on p.AcadLevel_ID = l.Academic_Level_ID";

                            cm = new SqlCommand(selectQuery, cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                dataGridView.Rows.Add(dr["Department_ID"].ToString(), 
                                    dr["Description"].ToString(), 
                                    dr["Academic_Level_Description"].ToString());
                            }
                            dr.Close();
                        }
                        else if (header.Equals("Year Level"))
                        {
                            selectQuery = "Select * from tbl_year_level y left join tbl_academic_level l on y.AcadLevel_ID = l.Academic_Level_ID";

                            cm = new SqlCommand(selectQuery, cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                dataGridView.Rows.Add(dr["Level_ID"].ToString(),
                                   dr["Description"].ToString(),
                                    dr["Academic_Level_Description"].ToString());
                            }
                            dr.Close();
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    cn.Open();
                    ds.Clear();
                    string acadlevelid = "";
                    if (!tbDes.Text.Equals(String.Empty))
                    {
                        string selectQuery = "";
                        selectQuery = "Select * from tbl_academic_level WHERE Academic_Level_Description = @acaddes";

                        cm = new SqlCommand(selectQuery, cn);
                        cm.Parameters.AddWithValue("@acaddes", cbAcadLevel.Text);
                        dr = cm.ExecuteReader();
                        if (dr.Read())
                        {
                            acadlevelid = dr["Academic_Level_ID"].ToString();
                        }
                        dr.Close();

                        if (isUpdateTrue)
                        {
                            if (header.Equals("Departments"))
                            {
                                cm = new SqlCommand("Select * from tbl_Departments where Department_ID = '" + data + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i > 0)
                                {
                                    
                                    cm = new SqlCommand("UPDATE tbl_Departments SET Description = @NewValue, AcadLevel_ID = @NewValue2 WHERE Department_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@NewValue2", acadlevelid);
                                    cm.Parameters.AddWithValue("@ConditionValue", data);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
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

                                    cm = new SqlCommand("UPDATE tbl_year_level SET Description = @NewValue, AcadLevel_ID = @NewValue2 WHERE Level_ID = @ConditionValue", cn);
                                    cm.Parameters.AddWithValue("@NewValue", tbDes.Text);
                                    cm.Parameters.AddWithValue("@NewValue2", acadlevelid);
                                    cm.Parameters.AddWithValue("@ConditionValue", data);
                                    cm.ExecuteNonQuery();
                                    dr.Close();

                                }
                                else
                                {
                                    MessageBox.Show(tbDes.Text + " is Present!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        else
                        {
                            cbAcadLevel.SelectedIndex = 0;
                            if (header.Equals("Departments"))
                            {

                                cm = new SqlCommand("Select * from tbl_Departments where Description = '" + tbDes.Text + "'", cn);
                                ad = new SqlDataAdapter(cm);
                                ad.Fill(ds);
                                int i = ds.Tables[0].Rows.Count;
                                if (i == 0)
                                {
                            
                                    cm = new SqlCommand("Insert into tbl_Departments Values (@Des, @acadlevel)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.Parameters.AddWithValue("@acadlevel", acadlevelid);
                                    cm.ExecuteNonQuery();
                                    dr.Close();
                                
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
                                    cm = new SqlCommand("Insert into tbl_year_level Values (@Des, @acadlevel)", cn);
                                    cm.Parameters.AddWithValue("@Des", tbDes.Text);
                                    cm.Parameters.AddWithValue("@acadlevel", acadlevelid);
                                    cm.ExecuteNonQuery();
                                    dr.Close();

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
                    cbAcadLevel.Text = String.Empty;
                    data = String.Empty;
                    isUpdateTrue = false;
                    btnCancel.Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (formTeacherForm != null)
            {
                formTeacherForm.displayDept();
                this.Close();
            }
            else if (formStudentForm != null)
            {
                formStudentForm.displayPSY();
                this.Close();
            }
            else if(formSubjectsForm != null)
            {
                formSubjectsForm.displayCB();
                this.Close();
            }
            else if(formSchoolDetails != null)
            {
                formSchoolDetails.loadData();
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
                        DialogResult confirmationResult = MessageBox.Show("Are you sure you want to delete this data? There might be relevant data related to this.", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (confirmationResult == DialogResult.Yes)
                        {
                            using (cn = new SqlConnection(SQL_Connection.connection))
                            {
                                try
                                {
                                    cn.Open();
                                    // Assuming your primary key column is named "ID"
                                    int primaryKeyValue = Convert.ToInt32(rowToDelete.Cells["ID"].Value);
                                    String deleteQuery = "";

                                    if (header.Equals("Departments"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_Departments WHERE Department_ID = @ID";
                                    }
                                    else if (header.Equals("Year Level"))
                                    {
                                        // Create a DELETE query
                                        deleteQuery = "DELETE FROM tbl_year_level WHERE Level_ID = @ID";
                                    }

                                    using (SqlCommand command = new SqlCommand(deleteQuery, cn))
                                    {
                                        // Add parameter for the primary key value
                                        command.Parameters.AddWithValue("@ID", primaryKeyValue);
                                        
                                        command.ExecuteNonQuery();

                                        MessageBox.Show(header + " deleted successfully.");
                                        displayData();
                                  
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
                        data = dataGridView.Rows[rowIndex].Cells[0].Value.ToString();
                        cbAcadLevel.Text = dataGridView.Rows[rowIndex].Cells[2].Value.ToString();
                        tbDes.Text = dataGridView.Rows[rowIndex].Cells[1].Value.ToString();
                        isUpdateTrue = true;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            tbDes.Text = String.Empty;
            isUpdateTrue = false;
            btnCancel.Visible = false;
            cbAcadLevel.Text = String.Empty;
        }

        private void formAddSchoolSetting_FormClosed(object sender, FormClosedEventArgs e)
        {
            formStudentForm.displayPSY();
            formTeacherForm.displayDept();
        }

        private void btnCancelClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
