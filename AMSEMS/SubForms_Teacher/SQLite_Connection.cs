using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AMSEMS
{
    public class SQLite_Connection
    {
        private string connectionString;

        public SQLite_Connection()
        {
            string databasePath = Path.Combine(Application.StartupPath, "db_AMSEMS_LocalDB.db");
            connectionString = $"Data Source={databasePath};Version=3;";
        }

        public void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Create tables and perform any initial setup
                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Create tables, add initial data, etc.
                    string createTableSql = @"CREATE TABLE IF NOT EXISTS tbl_teachers_account (
                                                    Unique_ID INTEGER PRIMARY KEY, 
                                                    ID TEXT,
                                                    Firstname TEXT,
                                                    Lastname TEXT,
                                                    Middlename TEXT,
                                                    Password TEXT,
                                                    Profile_pic BLOB,
                                                    Department INTEGER,
                                                    Role INTEGER,
                                                    Status INTEGER,
                                                    DateTime TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_subjects (
                                                    Course_code TEXT PRIMARY KEY, 
                                                    Course_Description TEXT,
                                                    Units INTEGER,
                                                    Image BLOB,
                                                    Status TEXT,
                                                    Assigned_Teacher TEXT,
                                                    Academic_Level INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_students_account (
                                                    Unique_ID INTEGER PRIMARY KEY, 
                                                    ID TEXT,
                                                    RFID TEXT,
                                                    Firstname TEXT,
                                                    Lastname TEXT, 
                                                    Middlename TEXT, 
                                                    Password TEXT,
                                                    Profile_pic BLOB,
                                                    Program INTEGER,
                                                    Section INTEGER,
                                                    Year_Level INTEGER,
                                                    Department INTEGER,
                                                    Role INTEGER,
                                                    Status INTEGER,
                                                    DateTime TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_departments (
                                                    Department_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_program (
                                                    Prgram_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_section (
                                                    Section_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_year_level (
                                                    Level_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_academic_level (
                                                    Academic_Level_ID INTEGER PRIMARY KEY, 
                                                    Academic_Level_Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_acad (
                                                    Acad_ID INTEGER PRIMARY KEY, 
                                                    Academic_Year_Start TEXT,
                                                    Academic_Year_End TEXT,
                                                    Ter_Academic_Sem INTEGER,
                                                    SHS_Academic_Sem INTEGER);";
                    command.CommandText = createTableSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void ClearData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string clearSql = @"DELETE FROM tbl_students_account;
                                        DELETE FROM tbl_teachers_account;
                                        DELETE FROM tbl_departments;
                                        DELETE FROM tbl_program;
                                        DELETE FROM tbl_section;
                                        DELETE FROM tbl_year_level;
                                        DELETE FROM tbl_academic_level;
                                        DELETE FROM tbl_acad;";
                    command.CommandText = clearSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void ClearSubjectsData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string clearSql = @"DELETE FROM tbl_subjects;";
                    command.CommandText = clearSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertTeacherData(int unique_id, string id, string fname, string lname, string mname, string pass, byte[] pic, string dep, string role, string status, string datetime)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    byte[] imageBytes = pic;
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_teachers_account (Unique_ID, ID, Firstname, Lastname, Middlename, Password, Profile_pic, Department, Role, Status, DateTime) VALUES (@UniqueID, @ID, @Fname, @Lname, @Mname, @Pass, @Pic, @Dep, @Role, @Status, @DateTime);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@UniqueID", unique_id);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Fname", fname);
                    command.Parameters.AddWithValue("@Lname", lname);
                    command.Parameters.AddWithValue("@Mname", mname);
                    command.Parameters.AddWithValue("@Pass", pass);
                    command.Parameters.Add("@Pic", DbType.Binary).Value = imageBytes;
                    command.Parameters.AddWithValue("@Dep", dep);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@DateTime", datetime);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertSubjectsData(string ccode, string cdes, string units, byte[] pic, string stat, string assTeach, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    byte[] imageBytes = pic;
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_subjects (Course_code, Course_Description, Units, Image, Status, Assigned_Teacher, Academic_Level) VALUES (@Ccode, @CDes, @Units, @Image, @Stat, @AssTeach, @AcadLvl);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@CDes", cdes);
                    command.Parameters.AddWithValue("@Units", units);
                    command.Parameters.Add("@Image", DbType.Binary).Value = imageBytes;
                    command.Parameters.AddWithValue("@Stat", stat);
                    command.Parameters.AddWithValue("@AssTeach", assTeach);
                    command.Parameters.AddWithValue("@AcadLvl", acadlvl);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertStudentData(int unique_id, string id, string rfid, string fname, string lname, string mname, string pass, byte[] pic, string prog, string sec, string yearlvl, string dep, string role, string status, string datetime)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    byte[] imageBytes = pic;
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_students_account (Unique_ID, ID, RFID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime) VALUES (@UniqueID, @ID, @RFID, @Fname, @Lname, @Mname, @Pass, @Pic, @Prog, @Sec, @YearLvl, @Dep, @Role, @Status, @DateTime);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@UniqueID", unique_id);
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@RFID", rfid);
                    command.Parameters.AddWithValue("@Fname", fname);
                    command.Parameters.AddWithValue("@Lname", lname);
                    command.Parameters.AddWithValue("@Mname", mname);
                    command.Parameters.AddWithValue("@Pass", pass);
                    command.Parameters.Add("@Pic", DbType.Binary).Value = imageBytes;
                    command.Parameters.AddWithValue("@Prog", prog);
                    command.Parameters.AddWithValue("@Sec", sec);
                    command.Parameters.AddWithValue("@YearLvl", yearlvl);
                    command.Parameters.AddWithValue("@Dep", dep);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@DateTime", datetime);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        
        public void InsertProgramData(string id, string desc)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_program (Prgram_ID, Description) VALUES (@ProgramID, @Desc);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@ProgramID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertSectionData(string id, string desc)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_section (Section_ID, Description) VALUES (@SectionID, @Desc);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@SectionID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertYearLevelData(string id, string desc)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_year_level (Level_ID, Description) VALUES (@LevelID, @Desc);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@LevelID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertDepartmentData(string id, string desc)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_departments (Department_ID, Description) VALUES (@DepartmentID, @Desc);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@DepartmentID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertAcadLvlData(string id, string desc)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_academic_level (Academic_Level_ID, Academic_Level_Description) VALUES (@acadlvlID, @Desc);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@acadlvlID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertAcadData(string id, string acadstart, string acadend, string teracadsem, string shsacadsem)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_acad (Acad_ID, Academic_Year_Start, Academic_Year_End, Ter_Academic_Sem, SHS_Academic_Sem) VALUES (@AcadId, @AcadStart, @AcadEnd, @TerAcadSem, @ShsAcadSem);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@AcadId", id);
                    command.Parameters.AddWithValue("@AcadStart", acadstart);
                    command.Parameters.AddWithValue("@AcadEnd", acadend);
                    command.Parameters.AddWithValue("@TerAcadSem", teracadsem);
                    command.Parameters.AddWithValue("@ShsAcadSem", shsacadsem);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public DataTable GetAssignedSubjects(string teachID)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Course_code, Course_Description, Image FROM tbl_subjects s LEFT JOIN tbl_teachers_account t ON s.Assigned_Teacher = t.ID WHERE Unique_ID = @TeachID AND s.Status = 1";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TeachID", teachID);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Create a new DataTable with the desired columns
            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add("Image", typeof(Image));
            newDataTable.Columns.Add("Course_code", typeof(string));
            newDataTable.Columns.Add("Course_Description", typeof(string));

            // Populate the new DataTable with data
            foreach (DataRow row in dataTable.Rows)
            {
                object imageData = row["Image"];
                Image image = ConvertToImage(imageData);

                newDataTable.Rows.Add(image, row["Course_code"], row["Course_Description"]);
            }

            return newDataTable;
        }

        private Image ConvertToImage(object imageData)
        {
            if (imageData != DBNull.Value) // Check if the column is not null
            {
                byte[] imageBytes = (byte[])imageData;
                if (imageBytes.Length > 0)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            return Image.FromStream(ms);
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log or handle the exception if necessary
                    }
                }
            }

            return null; // Return null for cases where the column is null or image conversion fails
        }
    }
}
