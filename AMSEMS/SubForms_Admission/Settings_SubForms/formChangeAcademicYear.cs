using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using static System.Windows.Forms.AxHost;

namespace AMSEMS.SubForms_Admin
{
    public partial class formChangeAcademicYear : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        DataSet ds = new DataSet();
        bool option;
        formNewAcadYear form;
        String id;
        public formChangeAcademicYear(formNewAcadYear form, bool option)
        {
            InitializeComponent();

            cn = new SqlConnection(SQL_Connection.connection);
            this.form = form;

            this.option = option;
            if (!option)
            {
                lblName.Text = "Update Academic Year";
            }
        }
        public void getInfoEdit(String id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
                {
                    connection.Open();

                    // Create the SQL command
                    string selectQuery = "SELECT Acad_ID, Academic_Year_Start, Academic_Year_End, Status FROM tbl_acad WHERE Acad_ID = @acadid";
                    using (SqlCommand command = new SqlCommand(selectQuery, connection))
                    {
                        command.Parameters.AddWithValue("acadid", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            // Iterate through the data reader
                            if (reader.Read())
                            {
                                // Access the columns by their index (0-based) or by name
                                int acadID = reader.GetInt32(reader.GetOrdinal("Acad_ID"));
                                string academicYearStart = reader.GetString(reader.GetOrdinal("Academic_Year_Start"));
                                string academicYearEnd = reader.GetString(reader.GetOrdinal("Academic_Year_End"));
                                int status = reader.GetInt32(reader.GetOrdinal("Status"));

                                tbStartY.Text = academicYearStart;
                                tbEndY.Text = academicYearEnd;
                                this.id = id;
                            }
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
            
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            using (cn = new SqlConnection(SQL_Connection.connection))
            {
                if (tbStartY.Text.Equals(String.Empty) || tbEndY.Text.Equals(String.Empty))
                {
                    MessageBox.Show("Empty Fields Detected!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    cn.Open();
                    if (option)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to save new academic year?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string updateQuery = "UPDATE tbl_acad SET Status = 2";
                            using (SqlCommand updateCommand = new SqlCommand(updateQuery, cn))
                            {
                                updateCommand.ExecuteNonQuery();
                            }

                            string query = "INSERT INTO tbl_acad (Academic_Year_Start, Academic_Year_End, Status) " +
                                           "VALUES (@AcademicYearStart, @AcademicYearEnd, @Status)";

                            using (SqlCommand command = new SqlCommand(query, cn))
                            {
                                // Replace txtStart, txtEnd, and txtStatus with your actual TextBox or other controls
                                command.Parameters.AddWithValue("@AcademicYearStart", tbStartY.Text);
                                command.Parameters.AddWithValue("@AcademicYearEnd", tbEndY.Text);
                                command.Parameters.AddWithValue("@Status", "1");

                                // Execute the command
                                command.ExecuteNonQuery();

                                MessageBox.Show("New academic year set successfully!");
                            }
                        }
                    }
                    else
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to save changes?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            string updateQuery = "UPDATE tbl_acad SET Academic_Year_Start = @AcademicYearStart, " +
                                         "Academic_Year_End = @AcademicYearEnd " +
                                         "WHERE Acad_ID = @AcadID";

                            using (SqlCommand command = new SqlCommand(updateQuery, cn))
                            {
                                command.Parameters.AddWithValue("@AcademicYearStart", tbStartY.Text);
                                command.Parameters.AddWithValue("@AcademicYearEnd", tbEndY.Text);
                                command.Parameters.AddWithValue("@AcadID", id);

                                // Execute the command
                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Record updated successfully!");
                                }

                            }
                        }
                    }
                }
            }
            form.displayData();
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
