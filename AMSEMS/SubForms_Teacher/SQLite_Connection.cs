﻿using AMSEMS.SubForms_Teacher;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
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
            try
            {
                string databasePath = Path.Combine(Application.StartupPath, "db_AMSEMS_LocalDB.db");
                connectionString = $"Data Source={databasePath};Version=3;";

                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(databasePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create the database file if it doesn't exist
                if (!File.Exists(databasePath))
                {
                    SQLiteConnection.CreateFile(databasePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing SQLite connection: {ex.Message}", "SQLite Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                                                    Academic_Level INTEGER,
                                                    Department_ID INTEGER);
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
                                                    AcadLevel_ID INTEGER,
                                                    Department_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_section (
                                                    Section_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    AcadLevel_ID INTEGER,
                                                    Department_ID INTEGER);
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
                                                    Status INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_Semester (
                                                    Semester_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    Status INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_Quarter (
                                                    Quarter_ID INTEGER PRIMARY KEY, 
                                                    Description TEXT,
                                                    Status INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_ter_assigned_teacher_to_sub (
                                                    Assigned_ID   INTEGER,
                                                    Teacher_ID      TEXT,
                                                    Course_Code TEXT,
                                                    Semester      int,
                                                    School_Year  int,
                                                    Department_ID INTEGER);
                                            CREATE TABLE IF NOT EXISTS tbl_shs_assigned_teacher_to_sub (
                                                    Assigned_ID   INTEGER,
                                                    Teacher_ID      TEXT,
                                                    Course_Code TEXT,
                                                    Quarter      int,
                                                    School_Year  int,
                                                    Department_ID INTEGER);
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
                                        DELETE FROM tbl_ter_assigned_teacher_to_sub;
                                        DELETE FROM tbl_shs_assigned_teacher_to_sub;
                                        DELETE FROM tbl_Semester;
                                        DELETE FROM tbl_Quarter;
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
                    string clearSql = @"DELETE FROM tbl_subjects;                                                               DELETE FROM tbl_class_list;";
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
        public void InsertSubjectsData(string ccode, string cdes, string units, byte[] pic, string stat, string acadlvl, string depid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    byte[] imageBytes = pic;
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_subjects (Course_code, Course_Description, Units, Image, Status, Academic_Level, Department_ID) VALUES (@Ccode, @CDes, @Units, @Image, @Stat, @AcadLvl, @DepID);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@CDes", cdes);
                    command.Parameters.AddWithValue("@Units", units);
                    command.Parameters.Add("@Image", DbType.Binary).Value = imageBytes;
                    command.Parameters.AddWithValue("@Stat", stat);
                    command.Parameters.AddWithValue("@AcadLvl", acadlvl);
                    command.Parameters.AddWithValue("@DepID", depid);
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
        
        public void InsertProgramData(string id, string desc, string acadlvl, string depid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_program (Prgram_ID, Description, AcadLevel_ID, Department_ID) VALUES (@ProgramID, @Desc, @Acadlvl, @DepID);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@ProgramID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
                    command.Parameters.AddWithValue("@DepID", depid);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertSectionData(string id, string desc, string acadlvl, string depid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_section (Section_ID, Description, AcadLevel_ID, Department_ID) VALUES (@SectionID, @Desc, @Acadlvl, @DepID);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@SectionID", id);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Acadlvl", acadlvl);
                    command.Parameters.AddWithValue("@DepID", depid);
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
        public void InsertAcadData(string id, string acadstart, string acadend, string status)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_acad (Acad_ID, Academic_Year_Start, Academic_Year_End, Status) VALUES (@AcadId, @AcadStart, @AcadEnd, @Stat);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@AcadId", id);
                    command.Parameters.AddWithValue("@AcadStart", acadstart);
                    command.Parameters.AddWithValue("@AcadEnd", acadend);
                    command.Parameters.AddWithValue("@Stat", status);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertSemData(string id, string des, string status)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_Semester (Semester_ID, Description, Status) VALUES (@SemID, @Des, @Stat);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@SemID", id);
                    command.Parameters.AddWithValue("@Des", des);
                    command.Parameters.AddWithValue("@Stat", status);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertQuarData(string id, string des, string status)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_Quarter (Quarter_ID, Description, Status) VALUES (@SemID, @Des, @Stat);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@SemID", id);
                    command.Parameters.AddWithValue("@Des", des);
                    command.Parameters.AddWithValue("@Stat", status);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertTerAssignedData(string id, string teach, string ccode, string sem, string schyear, string depid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_ter_assigned_teacher_to_sub (Assigned_ID, Teacher_ID, Course_Code, Semester, School_Year, Department_ID) VALUES (@AssID, @TeachID, @Ccode, @Sem, @SchYear, @DepID);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@AssID", id);
                    command.Parameters.AddWithValue("@TeachID", teach);
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@Sem", sem);
                    command.Parameters.AddWithValue("@SchYear", schyear);
                    command.Parameters.AddWithValue("@DepID", depid);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void InsertShsAssignedData(string id, string teach, string ccode, string sem, string schyear, string depid)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_shs_assigned_teacher_to_sub (Assigned_ID, Teacher_ID, Course_Code, Quarter, School_Year, Department_ID) VALUES (@AssID, @TeachID, @Ccode, @Sem, @SchYear, @DepID);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@AssID", id);
                    command.Parameters.AddWithValue("@TeachID", teach);
                    command.Parameters.AddWithValue("@Ccode", ccode);
                    command.Parameters.AddWithValue("@Sem", sem);
                    command.Parameters.AddWithValue("@SchYear", schyear);
                    command.Parameters.AddWithValue("@DepID", depid);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public DataTable GetAssignedSubjects(string teachID, string acadlvl, string schyear, string sem)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                //string schyear = null, sem = null, quar = null;
                string query = "";

                //// Get Academic Year
                //query = "SELECT Acad_ID, Academic_Year_Start, Academic_Year_End FROM tbl_acad WHERE Status = 1";

                //using (SQLiteCommand command = new SQLiteCommand(query, connection))
                //{
                //    using (SQLiteDataReader adapter = command.ExecuteReader())
                //    {
                //        if (adapter.Read()) // Move to the first row
                //        {
                //            schyear = adapter["Acad_ID"].ToString();
                //        }
                //    }
                //}

                //// Get Quarter
                //query = "SELECT Quarter_ID, Description FROM tbl_Quarter WHERE Status = 1";

                //using (SQLiteCommand cm = new SQLiteCommand(query, connection))
                //{
                //    using (SQLiteDataReader dr = cm.ExecuteReader())
                //    {
                //        if (dr.Read())
                //        {
                //            quar = dr["Quarter_ID"].ToString();
                //        }
                //    }
                //}

                //// Get Semester
                //query = "SELECT Semester_ID, Description FROM tbl_Semester WHERE Status = 1";

                //using (SQLiteCommand cm = new SQLiteCommand(query, connection))
                //{
                //    using (SQLiteDataReader dr = cm.ExecuteReader())
                //    {
                //        if (dr.Read())
                //        {
                //            sem = dr["Semester_ID"].ToString();
                //        }
                //    }
                //}
                if (acadlvl == "Tertiary")
                {
                    query = "SELECT s.Course_code, Course_Description, Image, Academic_Level, Academic_Level_Description " +
                    "FROM tbl_subjects AS s " +
                    "LEFT JOIN tbl_Departments d ON s.Department_ID = d.Department_ID " +
                    "LEFT JOIN tbl_Academic_Level AS al ON s.Academic_Level = al.Academic_Level_ID " +
                    "LEFT JOIN tbl_ter_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code " +
                    "LEFT JOIN tbl_teachers_account ta ON ter.Teacher_ID = ta.ID " +
                    "LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID " +
                    "LEFT JOIN tbl_Semester sm ON ter.Semester = sm.Semester_ID " +
                    "WHERE (Academic_Year_Start ||'-'|| Academic_Year_End) = @schyear AND sm.Description = @sem AND Unique_ID = @TeachID " +
                    "ORDER BY 1 DESC";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@schyear", schyear);
                        command.Parameters.AddWithValue("@sem", sem);
                        command.Parameters.AddWithValue("@TeachID", teachID);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
                else
                {
                    query = @"SELECT s.Course_code, Course_Description, Image, Academic_Level, Academic_Level_Description 
                    FROM tbl_subjects AS s
                    LEFT JOIN tbl_Departments d ON s.Department_ID = d.Department_ID
                    LEFT JOIN tbl_Academic_Level AS al ON s.Academic_Level = al.Academic_Level_ID
                    LEFT JOIN tbl_shs_assigned_teacher_to_sub ter ON s.Course_code = ter.Course_code
                    LEFT JOIN tbl_teachers_account ta ON ter.Teacher_ID = ta.ID
                    LEFT JOIN tbl_acad ad ON ter.School_Year = ad.Acad_ID
                    LEFT JOIN tbl_Quarter qu ON ter.Quarter = qu.Quarter_ID
                    WHERE (Academic_Year_Start ||'-'|| Academic_Year_End) = @schyear AND qu.Description = @sem AND Unique_ID = @TeachID
                    ORDER BY 1 DESC";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@schyear", schyear);
                        command.Parameters.AddWithValue("@sem", sem);
                        command.Parameters.AddWithValue("@TeachID", teachID);

                        using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }

            // Create a new DataTable with the desired columns
            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add("Image", typeof(Image));
            newDataTable.Columns.Add("Course_code", typeof(string));
            newDataTable.Columns.Add("Course_Description", typeof(string));
            newDataTable.Columns.Add("Academic_Level", typeof(string));
            newDataTable.Columns.Add("Academic_Level_Description", typeof(string));

            // Populate the new DataTable with data
            foreach (DataRow row in dataTable.Rows)
            {
                object imageData = row["Image"];
                Image image = ConvertToImage(imageData);

                newDataTable.Rows.Add(image, row["Course_code"], row["Course_Description"], row["Academic_Level"], row["Academic_Level_Description"]);
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

            try
            {
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
            }
            catch (SQLiteException ex)
            {
                return null;
            }

            return dataTable;
        }

        public DataTable GetStudListSingle(string tblname, string classcode)
        {
            DataTable dataTable = new DataTable();
            try
            {
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
            }
            catch (SQLiteException ex)
            {
                return null;
            }
            return dataTable;
        }
    }
}
