using AMSEMS.SubForms_Teacher;
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
        public string connectionString;

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
                                                    Description TEXT,
                                                    AcadLevel_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_program (
                                                    Prgram_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    AcadLevel_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_section (
                                                    Section_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    AcadLevel_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_year_level (
                                                    Level_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    AcadLevel_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_academic_level (
                                                    Academic_Level_ID INTEGER PRIMARY KEY, 
                                                    Academic_Level_Description TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_acad (
                                                    Acad_ID INTEGER PRIMARY KEY, 
                                                    Academic_Year_Start TEXT,
                                                    Academic_Year_End TEXT,
                                                    Ter_Academic_Sem INTEGER,
                                                    SHS_Academic_Sem INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_class_list (
                                                    CLass_Code TEXT PRIMARY KEY,
                                                    Section_ID INTEGER,
                                                    Teacher_ID TEXT,
                                                    Course_Code TEXT,
                                                    School_Year TEXT,
                                                    Semester TEXT,
                                                    Acad_Level TEXT);
                                            CREATE TABLE IF NOT EXISTS tbl_subject_attendance (
                                                    Attendance_id   INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    Class_Code      TEXT,
                                                    Attendance_date TEXT,
                                                    Student_ID      TEXT,
                                                    Student_Status  TEXT    DEFAULT A);";
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
        public void InsertClassListData(string clCode, string secID, string teachID, string cCode, string schYear, string sem, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand checkCommand = new SQLiteCommand(connection))
                {
                    // Check if the record already exists
                    string checkSql = "SELECT COUNT(*) FROM tbl_class_list WHERE CLass_Code = @CLass_Code AND Section_ID = @Section_ID AND Teacher_ID = @Teacher_ID AND Course_Code = @Course_Code AND School_Year = @School_Year AND Semester = @Semester AND Acad_Level = @Acad_Level;";

                    checkCommand.CommandText = checkSql;
                    checkCommand.Parameters.AddWithValue("@CLass_Code", clCode);
                    checkCommand.Parameters.AddWithValue("@Section_ID", secID);
                    checkCommand.Parameters.AddWithValue("@Teacher_ID", teachID);
                    checkCommand.Parameters.AddWithValue("@Course_Code", cCode);
                    checkCommand.Parameters.AddWithValue("@School_Year", schYear);
                    checkCommand.Parameters.AddWithValue("@Semester", sem);
                    checkCommand.Parameters.AddWithValue("@Acad_Level", acadlvl);

                    int recordCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (recordCount == 0)
                    {
                        // If the record doesn't exist, insert it
                        using (SQLiteCommand insertCommand = new SQLiteCommand(connection))
                        {
                            string insertSql = "INSERT INTO tbl_class_list (CLass_Code, Section_ID, Teacher_ID, Course_Code, School_Year, Semester, Acad_Level) VALUES (@CLass_Code, @Section_ID, @Teacher_ID, @Course_Code, @School_Year, @Semester, @Acad_Level);";

                            insertCommand.CommandText = insertSql;
                            insertCommand.Parameters.AddWithValue("@CLass_Code", clCode);
                            insertCommand.Parameters.AddWithValue("@Section_ID", secID);
                            insertCommand.Parameters.AddWithValue("@Teacher_ID", teachID);
                            insertCommand.Parameters.AddWithValue("@Course_Code", cCode);
                            insertCommand.Parameters.AddWithValue("@School_Year", schYear);
                            insertCommand.Parameters.AddWithValue("@Semester", sem);
                            insertCommand.Parameters.AddWithValue("@Acad_Level", acadlvl);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                string tableName = "tbl_" + clCode;
                DataTable class_stud_list = formDataSync.GetStudList(tableName);

                if (class_stud_list != null && class_stud_list.Rows.Count > 0)
                {
                    string createTableSql = $@"CREATE TABLE IF NOT EXISTS {tableName} (
                            StudentID INTEGER PRIMARY KEY, 
                            Class_Code TEXT);";

                    using (SQLiteCommand command = new SQLiteCommand(createTableSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    string insertSql2 = $@"INSERT INTO {tableName} (StudentID, Class_Code)
                        SELECT @StudID, @ClassCode
                        WHERE NOT EXISTS (SELECT 1 FROM {tableName} WHERE StudentID = @StudID);";

                    using (SQLiteCommand insertCommand = new SQLiteCommand(insertSql2, connection))
                    {
                        foreach (DataRow row1 in class_stud_list.Rows)
                        {
                            string studentID = row1["StudentID"].ToString();
                            string classCodeValue = row1["Class_Code"].ToString(); ;

                            insertCommand.Parameters.Clear();
                            insertCommand.Parameters.AddWithValue("@StudID", studentID);
                            insertCommand.Parameters.AddWithValue("@ClassCode", classCodeValue);
                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }

                connection.Close();
            }
        }
        public void InsertAttendancetData(string clCode, string attdate, string studID, string studStat)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand checkCommand = new SQLiteCommand(connection))
                {
                    // Check if the record already exists
                    string checkSql = "SELECT COUNT(*) FROM tbl_subject_attendance WHERE Class_Code = @ClCode AND Attendance_date = @AttDate AND Student_ID = @StudID AND Student_Status = @StudStat;";

                    checkCommand.CommandText = checkSql;
                    checkCommand.Parameters.AddWithValue("@ClCode", clCode);
                    checkCommand.Parameters.AddWithValue("@AttDate", attdate);
                    checkCommand.Parameters.AddWithValue("@StudID", studID);
                    checkCommand.Parameters.AddWithValue("@StudStat", studStat);

                    int recordCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                    if (recordCount == 0)
                    {
                        // If the record doesn't exist, insert it
                        using (SQLiteCommand insertCommand = new SQLiteCommand(connection))
                        {
                            string insertSql = "INSERT INTO tbl_subject_attendance (Class_Code, Attendance_date, Student_ID, Student_Status) VALUES (@ClCode, @AttDate, @StudID, @StudStat);";

                            insertCommand.CommandText = insertSql;
                            insertCommand.Parameters.AddWithValue("@ClCode", clCode);
                            insertCommand.Parameters.AddWithValue("@AttDate", attdate);
                            insertCommand.Parameters.AddWithValue("@StudID", studID);
                            insertCommand.Parameters.AddWithValue("@StudStat", studStat);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
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
        
        public void InsertProgramData(string id, string desc, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_program (Prgram_ID, Description, AcadLevel_ID) VALUES (@ProgramID, @Desc, @Acadlvl);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@ProgramID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertSectionData(string id, string desc, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_section (Section_ID, Description, AcadLevel_ID) VALUES (@SectionID, @Desc, @Acadlvl);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@SectionID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertYearLevelData(string id, string desc, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_year_level (Level_ID, Description, AcadLevel_ID) VALUES (@LevelID, @Desc, @Acadlvl);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@LevelID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertDepartmentData(string id, string desc, string acadlvl)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_departments (Department_ID, Description, AcadLevel_ID) VALUES (@DepartmentID, @Desc, @Acadlvl);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@DepartmentID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
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
                string query = "SELECT Course_code, Course_Description, Image, Academic_Level FROM tbl_subjects s LEFT JOIN tbl_teachers_account t ON s.Assigned_Teacher = t.ID WHERE Unique_ID = @TeachID AND s.Status = 1";

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
            newDataTable.Columns.Add("Academic_Level", typeof(string));

            // Populate the new DataTable with data
            foreach (DataRow row in dataTable.Rows)
            {
                object imageData = row["Image"];
                Image image = ConvertToImage(imageData);

                newDataTable.Rows.Add(image, row["Course_code"], row["Course_Description"], row["Academic_Level"]);
            }

            return newDataTable;
        }

        public Image ConvertToImage(object imageData)
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
        public DataTable GetAttendanceRecord()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_subject_attendance";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }
        public DataTable GetClassList()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_class_list";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public DataTable GetAttendanceRecord(string classcode)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_subject_attendance WHERE Class_Code = @ClassCode";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassCode", classcode);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public DataTable GetClassList(string classcode)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_class_list WHERE Class_Code = @ClassCode";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassCode", classcode);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public DataTable GetStudList(string tblname)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {tblname}";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
        public DataTable GetStudListSingle(string tblname, string classcode)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = $"SELECT * FROM {tblname} WHERE  Class_Code = @ClassCode";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClassCode", classcode);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }
    }
}
