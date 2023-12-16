using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Admin
{
    public partial class formChangeAcademicYear : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        private string originalStartY;
        private string originalEndY;
        private string originalTerSem;
        private string originalSHSSem;
        formAcademicYearSetting form;
        public formChangeAcademicYear(formAcademicYearSetting form)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            this.form = form;

            cn.Open();
            cm = new SqlCommand("Select TOP 1 Academic_Year_Start,Academic_Year_End,Ter_Academic_Sem,SHS_Academic_Sem from tbl_acad ORDER BY 1 DESC;", cn);
            dr = cm.ExecuteReader();
            dr.Read();
            tbStartY.Text = dr["Academic_Year_Start"].ToString();
            tbEndY.Text = dr["Academic_Year_End"].ToString();
            tbTerSem.Text = dr["Ter_Academic_Sem"].ToString();
            tbSHSSem.Text = dr["SHS_Academic_Sem"].ToString();
            dr.Close();
            cn.Close();

            originalStartY = tbStartY.Text;
            originalEndY = tbEndY.Text;
            originalTerSem = tbTerSem.Text;
            originalSHSSem = tbSHSSem.Text;
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbStartY.Text.Equals(String.Empty) || tbTerSem.Text.Equals(String.Empty) || tbEndY.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();

                    // Retrieve the original values before updating
                    GetOriginalValues();

                    // Check if any values have changed
                    bool startYChanged = originalStartY != tbStartY.Text;
                    bool endYChanged = originalEndY != tbEndY.Text;
                    bool terSemChanged = originalTerSem != tbTerSem.Text;
                    bool shsSemChanged = originalSHSSem != tbSHSSem.Text;

                    // Update only the changed columns
                    if (startYChanged || endYChanged || terSemChanged || shsSemChanged)
                    {
                        // Build the update command based on the changed values
                        SqlCommand updateCommand = new SqlCommand("UPDATE tbl_acad SET ", cn);

                        if (startYChanged)
                        {
                            updateCommand.CommandText += "Academic_Year_Start = @NewValue1, ";
                            updateCommand.Parameters.AddWithValue("@NewValue1", tbStartY.Text);
                        }

                        if (endYChanged)
                        {
                            updateCommand.CommandText += "Academic_Year_End = @NewValue2, ";
                            updateCommand.Parameters.AddWithValue("@NewValue2", tbEndY.Text);
                        }

                        if (terSemChanged)
                        {
                            updateCommand.CommandText += "Ter_Academic_Sem = @NewValue3, ";
                            updateCommand.Parameters.AddWithValue("@NewValue3", tbTerSem.Text);
                        }

                        if (shsSemChanged)
                        {
                            updateCommand.CommandText += "SHS_Academic_Sem = @NewValue4, ";
                            updateCommand.Parameters.AddWithValue("@NewValue4", tbSHSSem.Text);
                        }

                        // Remove the trailing comma and space
                        updateCommand.CommandText = updateCommand.CommandText.TrimEnd(',', ' ');

                        // Add the WHERE clause
                        updateCommand.CommandText += " WHERE Acad_ID = 1";

                        // Execute the update command
                        updateCommand.ExecuteNonQuery();

                        MessageBox.Show("Academic Status Changed!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                        form.loadAcad();
                    }
                }
            }
        }

        private void GetOriginalValues()
        {
            // Fetch the original values before the update
            SqlCommand selectCommand = new SqlCommand("SELECT Academic_Year_Start, Academic_Year_End, Ter_Academic_Sem, SHS_Academic_Sem FROM tbl_acad WHERE Acad_ID = 1", cn);

            using (SqlDataReader reader = selectCommand.ExecuteReader())
            {
                if (reader.Read())
                {
                    originalStartY = reader["Academic_Year_Start"].ToString();
                    originalEndY = reader["Academic_Year_End"].ToString();
                    originalTerSem = reader["Ter_Academic_Sem"].ToString();
                    originalSHSSem = reader["SHS_Academic_Sem"].ToString();
                }
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //public void archive()
        //{
        //    ArchiveClassList();
        //    ArchiveConsultationRecords();
        //    ArchiveSubjectAttendance();
        //    List<string> classCodes = GetClassCodes();

        //    foreach (string classCode in classCodes)
        //    {
        //        string oldTable = "tbl_" + classCode;
        //        string newTable = "tbl_archived" + classCode;
        //        RenameTable(oldTable, newTable);
        //    }
        //}
        //public static void RenameTable(string oldTableName, string newTableName)
        //{
        //    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
        //    {
        //        connection.Open();

        //        // Rename the table using sp_rename stored procedure
        //        using (SqlCommand command = new SqlCommand($"EXEC sp_rename '{oldTableName}', '{newTableName}'", connection))
        //        {
        //            command.ExecuteNonQuery();
        //        }

        //        connection.Close();
        //    }
        //}
        //public static void ArchiveConsultationRecords()
        //{
        //    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
        //    {
        //        connection.Open();

        //        // Step 1: Archive data to tbl_archived_consultation_record
        //        using (SqlCommand archiveCommand = new SqlCommand("INSERT INTO tbl_archived_consultation_record SELECT * FROM tbl_consultation_record", connection))
        //        {
        //            archiveCommand.ExecuteNonQuery();
        //        }

        //        // Step 2: Set Archived_Date to the current date in tbl_archived_consultation_record
        //        using (SqlCommand updateArchivedDateCommand = new SqlCommand("UPDATE tbl_archived_consultation_record SET Archived_Date = @ArchivedDate", connection))
        //        {
        //            updateArchivedDateCommand.Parameters.AddWithValue("@ArchivedDate", DateTime.Now);
        //            updateArchivedDateCommand.ExecuteNonQuery();
        //        }

        //        // Step 3: Delete data from tbl_consultation_record
        //        using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM tbl_consultation_record", connection))
        //        {
        //            deleteCommand.ExecuteNonQuery();
        //        }

        //        connection.Close();
        //    }
        //}
        //public static void ArchiveSubjectAttendance()
        //{
        //    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
        //    {
        //        connection.Open();

        //        // Step 1: Archive data to tbl_archived_subject_attendance
        //        using (SqlCommand archiveCommand = new SqlCommand("INSERT INTO tbl_archived_subject_attendance SELECT * FROM tbl_subject_attendance", connection))
        //        {
        //            archiveCommand.ExecuteNonQuery();
        //        }

        //        // Step 2: Set Archived_Date to the current date in tbl_archived_subject_attendance
        //        using (SqlCommand updateArchivedDateCommand = new SqlCommand("UPDATE tbl_archived_subject_attendance SET Archived_Date = @ArchivedDate", connection))
        //        {
        //            updateArchivedDateCommand.Parameters.AddWithValue("@ArchivedDate", DateTime.Now);
        //            updateArchivedDateCommand.ExecuteNonQuery();
        //        }

        //        // Step 3: Delete data from tbl_subject_attendance
        //        using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM tbl_subject_attendance", connection))
        //        {
        //            deleteCommand.ExecuteNonQuery();
        //        }

        //        connection.Close();
        //    }
        //}

        //public static void ArchiveClassList()
        //{
        //    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
        //    {
        //        connection.Open();

        //        // Step 1: Archive data to tbl_archived_class_list
        //        using (SqlCommand archiveCommand = new SqlCommand("INSERT INTO tbl_archived_class_list SELECT * FROM tbl_class_list", connection))
        //        {
        //            archiveCommand.ExecuteNonQuery();
        //        }

        //        // Step 2: Set Archived_Date to the current date in tbl_archived_class_list
        //        using (SqlCommand updateArchivedDateCommand = new SqlCommand("UPDATE tbl_archived_class_list SET Archived_Date = @ArchivedDate", connection))
        //        {
        //            updateArchivedDateCommand.Parameters.AddWithValue("@ArchivedDate", DateTime.Now);
        //            updateArchivedDateCommand.ExecuteNonQuery();
        //        }

        //        // Step 3: Delete data from tbl_class_list
        //        using (SqlCommand deleteCommand = new SqlCommand("DELETE FROM tbl_class_list", connection))
        //        {
        //            deleteCommand.ExecuteNonQuery();
        //        }

        //        connection.Close();
        //    }
        //}
        //public static List<string> GetClassCodes()
        //{
        //    List<string> classCodes = new List<string>();

        //    using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
        //    {
        //        connection.Open();

        //        // Select Class_Code from tbl_class_list
        //        using (SqlCommand command = new SqlCommand("SELECT Class_Code FROM tbl_class_list", connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    string classCode = reader["Class_Code"].ToString();
        //                    classCodes.Add(classCode);
        //                }
        //            }
        //        }

        //        connection.Close();
        //    }

        //    return classCodes;
        //}
    }
}
