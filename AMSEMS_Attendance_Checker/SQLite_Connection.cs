using AMSEMS;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public class SQLite_Connection
    {
        private string connectionString;

        public SQLite_Connection()
        {
            string databasePath = Path.Combine(Application.StartupPath, "db_ATTENDANCE_CHECKER.db");
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
                                            CREATE TABLE IF NOT EXISTS tbl_events (
                                                    Event_ID INTEGER PRIMARY KEY, 
                                                    Event_Name TEXT, 
                                                    Start_Date TEXT,
                                                    End_Date TEXT, 
                                                    Description TEXT, 
                                                    Color TEXT,
                                                    Image BLOB,
                                                    Attendance TEXT,
                                                    Penalty TEXT,
                                                    Exclusive TEXT,
                                                    Specific_Students TEXT);
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
                                            CREATE TABLE IF NOT EXISTS tbl_attendance (
                                                    Attendance_ID INTEGER PRIMARY KEY, 
                                                    Student_ID TEXT,
                                                    Event_ID TEXT,
                                                    Date_Time TEXT,
                                                    AM_IN TEXT,
                                                    AM_OUT TEXT,
                                                    PM_IN TEXT,
                                                    PM_OUT TEXT,
                                                    Checker TEXT);";
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
                                        DELETE FROM tbl_departments;
                                        DELETE FROM tbl_program;
                                        DELETE FROM tbl_section;
                                        DELETE FROM tbl_year_level;";
                    command.CommandText = clearSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void ClearAttendaceData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string clearSql = @"DELETE FROM tbl_attendance;";
                    command.CommandText = clearSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void ClearTeachersData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string clearSql = @"DELETE FROM tbl_teachers_account;";
                    command.CommandText = clearSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
        public void ClearEventData()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string clearSql = @"DELETE FROM tbl_events;";
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
        public void InsertEventsData(string id, string eventname, string startdate, string enddate, string desc, string color, string image, string attendance, string exclusive, string specific)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_events (Event_ID, Event_Name, Start_Date, End_Date, Description, Color, Image, Attendance, Exclusive, Specific_Students) VALUES (@EventID, @EventName, @StartDate, @EndDate, @Desc, @Color, @Image, @Attendance, @Exclusive, @Specific_Students);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@EventID", id);
                    command.Parameters.AddWithValue("@EventName", eventname);
                    command.Parameters.AddWithValue("@StartDate", startdate);
                    command.Parameters.AddWithValue("@EndDate", enddate);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Color", color);
                    command.Parameters.AddWithValue("@Image", image);
                    command.Parameters.AddWithValue("@Attendance", attendance);
                    command.Parameters.AddWithValue("@Exclusive", exclusive);
                    command.Parameters.AddWithValue("@Specific_Students", specific);
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
        public async Task<DataTable> LoginTeacherDataAsync(string id, string pass)
        {
            DataTable dataTable = new DataTable();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                await connection.OpenAsync(); // Open the connection asynchronously

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM tbl_teachers_account WHERE ID = @ID AND Password = @Pass AND Status = 1;", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@Pass", pass);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        await Task.Run(() => adapter.Fill(dataTable)); // Use Task.Run to perform the blocking operation asynchronously
                    }
                }

                connection.Close();
            }
            return dataTable;
        }
        private bool IsEventForSpecificStudents(string eventId)
        {
            using (SQLiteConnection cn = new SQLiteConnection(connectionString))
            {
                cn.Open();
                string query = "SELECT Exclusive FROM tbl_events WHERE Event_ID = @EventID";
                using (SQLiteCommand cmd = new SQLiteCommand(query, cn))
                {
                    cmd.Parameters.AddWithValue("@EventID", eventId);
                    object result = cmd.ExecuteScalar();
                    return result != null && result.ToString() == "Specific Students";
                }
            }
        }
        public DataTable GetAllStudents(string event_id)
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query;
                if (IsEventForSpecificStudents(event_id))
                {
                    query = @"SELECT Profile_pic, s.ID, RFID, UPPER(s.Firstname) AS Firstname, UPPER(s.Lastname) AS Lastname, UPPER(s.Middlename) AS Middlename, dep.Description AS depdes 
                        FROM tbl_events e
                        LEFT JOIN tbl_students_account s ON instr(e.Specific_Students, s.FirstName || ' ' || s.LastName) > 0 
                        LEFT JOIN tbl_departments AS dep ON s.Department = dep.Department_ID 
                        WHERE Status = 1 AND Event_ID = '"+event_id+"' ORDER BY depdes;";
                }
                else
                {

                    query = "SELECT Profile_pic, ID, RFID, UPPER(Firstname) AS Firstname, UPPER(Lastname) AS Lastname, UPPER(Middlename) AS Middlename, dep.Description AS depdes FROM tbl_students_account as stud LEFT JOIN tbl_departments AS dep ON stud.Department = dep.Department_ID WHERE Status = 1 ORDER BY depdes";
                }

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            // Create a new DataTable with the desired columns
            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add("Profile_pic", typeof(Image));
            newDataTable.Columns.Add("ID", typeof(string));
            newDataTable.Columns.Add("Name", typeof(string));
            newDataTable.Columns.Add("depdes", typeof(string));
            newDataTable.Columns.Add("RFID", typeof(string));

            // Populate the new DataTable with data
            foreach (DataRow row in dataTable.Rows)
            {
                string name = $"{row["Firstname"]} {row["Middlename"]} {row["Lastname"]}";

                object imageData = row["Profile_pic"];
                if (imageData != DBNull.Value) // Check if the column is not null
                {
                    byte[] imageBytes = (byte[])imageData;
                    if (imageBytes.Length > 0)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                Image image = Image.FromStream(ms);
                                newDataTable.Rows.Add(image, row["ID"], name, row["depdes"], row["RFID"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception (e.g., log it, display an error message)
                            // Optionally, you can add a placeholder image or null for the problematic data.
                            newDataTable.Rows.Add(null, row["ID"], name, row["depdes"], row["RFID"]);
                        }
                    }
                }
                else
                {
                    // Handle the case where the column is null
                    newDataTable.Rows.Add(null, row["ID"], name, row["depdes"], row["RFID"]);
                }
            }

            return newDataTable;
        }
        public DataTable GetAttendanceRecord(string eventID, string period, string status, string datetime)
        {
            DataTable dataTable = new DataTable();
            string query = null;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                if (period.Equals("AM"))
                {
                    if (status.Equals("IN"))
                    {
                        query = @"SELECT 
                            stud.Profile_Pic as pic,
                            ID,
                            AM_IN AS record,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE 
                            Event_ID = @eventID
                            AND stud.Status = 1 AND AM_IN IS NOT NULL AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date
                        ORDER BY 
                            record DESC";
                    }
                    else if (status.Equals("OUT"))
                    {
                        query = @"SELECT 
                            stud.Profile_Pic as pic,
                            ID,
                            AM_OUT AS record,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE 
                            Event_ID = @eventID
                            AND stud.Status = 1 AND AM_OUT IS NOT NULL AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date
                        ORDER BY 
                            record DESC";
                    }
                }
                else if (period.Equals("PM"))
                {
                    if (status.Equals("IN"))
                    {
                        query = @"SELECT 
                            stud.Profile_Pic as pic,
                            ID,
                            PM_IN AS record,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE 
                            Event_ID = @eventID
                            AND stud.Status = 1 AND PM_IN IS NOT NULL AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date
                        ORDER BY 
                            record DESC";
                    }
                    else if (status.Equals("OUT"))
                    {
                        query = @"SELECT 
                            stud.Profile_Pic as pic,
                            ID,
                            PM_OUT AS record,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE 
                            Event_ID = @eventID
                            AND stud.Status = 1 AND PM_OUT IS NOT NULL AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date
                        ORDER BY 
                            record DESC";
                    }
                }


                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@eventID", eventID);
                    command.Parameters.AddWithValue("@date", datetime);
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            // Create a new DataTable with the desired columns
            DataTable newDataTable = new DataTable();
            newDataTable.Columns.Add("pic", typeof(Image));
            newDataTable.Columns.Add("ID", typeof(string));
            newDataTable.Columns.Add("Name", typeof(string));
            newDataTable.Columns.Add("secdes", typeof(string));
            newDataTable.Columns.Add("depdes", typeof(string));
            newDataTable.Columns.Add("Date", typeof(string));
            newDataTable.Columns.Add("Time", typeof(string));

            foreach (DataRow row in dataTable.Rows)
            {
                string dateTimeString = $"{row["record"]}";
                DateTime dateTime = DateTime.Parse(dateTimeString);
                string time = dateTime.ToString("h:mm tt");
                string date = dateTime.ToString("M-d-yyyy");

                object imageData = row["pic"];
                byte[] imageBytes = (byte[])imageData;
                if (imageBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image image = Image.FromStream(ms);
                        newDataTable.Rows.Add(image, row["ID"], row["Name"], row["secdes"], row["depdes"], date, time);
                    }
                }
                else
                {
                    newDataTable.Rows.Add(null, row["ID"], row["Name"], row["secdes"], row["depdes"], date, time);
                }
            }

            return newDataTable;
        }


        public string GetEvent(string id)
        {
            string event_name = null; // Initialize the variable

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Event_Name FROM tbl_events WHERE Event_ID = @id AND Attendance = 'True'";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id); // Use parameterized query to prevent SQL injection
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            event_name = reader["Event_Name"].ToString();
                        }
                    }
                }
            }
            return event_name;
        }
        public void GetStudentForAttendance(string rfid, string eventID, string date, string amIn, string amOut, string pmIn, string pmOut, string checker)
        {
            string stud_id = null; // Initialize the variable
            string attendance_stud_id = null;
            DateTime dateTimeNow = DateTime.ParseExact(date, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            string formattedDate = dateTimeNow.ToString("M/d/yyyy");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Retrieve student information
                string query;
                if (IsEventForSpecificStudents(eventID))
                {
                    query = @"SELECT s.ID FROM tbl_events e
                            LEFT JOIN tbl_students_account s ON instr(e.Specific_Students, s.FirstName || ' ' || s.LastName) > 0 
                            LEFT JOIN tbl_departments AS dep ON s.Department = dep.Department_ID 
                            WHERE s.Status = 1 AND e.Event_ID = '" + eventID + "' AND s.RFID = @rfid ";
                }
                else
                {
                    query = "SELECT ID FROM tbl_students_account WHERE RFID = @rfid";
                }

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rfid", rfid); // Use parameterized query to prevent SQL injection
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stud_id = reader["ID"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(stud_id))
                {

                    query = "SELECT Student_ID FROM tbl_attendance WHERE Student_ID = @id AND Event_ID = @event AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", stud_id);
                        command.Parameters.AddWithValue("@event", eventID);
                        command.Parameters.AddWithValue("@date", formattedDate);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                attendance_stud_id = reader["Student_ID"].ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(attendance_stud_id))
                    {
                        // Create SQL UPDATE statements to update the corresponding columns
                        if (!string.IsNullOrEmpty(amIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "UPDATE tbl_attendance SET AM_IN = @amIn WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(amOut))
                        {
                            // Existing record found, UPDATE the AM-OUT column
                            query = "UPDATE tbl_attendance SET AM_OUT = @amOut WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(pmIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "UPDATE tbl_attendance SET PM_IN = @pmIn WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(pmOut))
                        {
                            // Existing record found, UPDATE the PM-OUT column
                            query = "UPDATE tbl_attendance SET PM_OUT = @pmOut WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(amIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, AM_IN, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @amIn, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(amOut))
                        {
                            // Existing record found, UPDATE the AM-OUT column
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, AM_OUT, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @amOut, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(pmIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, PM_IN, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @pmIn, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(pmOut))
                        {
                            // Existing record found, UPDATE the PM-OUT column
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, PM_OUT, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @pmOut, @checker)";
                        }
                    }

                    using (SQLiteCommand updateCommand = new SQLiteCommand(query, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@studentID", stud_id);
                        updateCommand.Parameters.AddWithValue("@eventID", eventID);
                        updateCommand.Parameters.AddWithValue("@date", date); // Include Date_Time in the UPDATE queries
                        updateCommand.Parameters.AddWithValue("@amIn", amIn);
                        updateCommand.Parameters.AddWithValue("@amOut", amOut);
                        updateCommand.Parameters.AddWithValue("@pmIn", pmIn);
                        updateCommand.Parameters.AddWithValue("@pmOut", pmOut);
                        updateCommand.Parameters.AddWithValue("@checker", checker);

                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Update or insertion successful
                        }
                        else
                        {
                            // Update or insertion failed
                        }
                    }
                }
                else
                {
                    MessageBox.Show("RFID is Not Registered or Not Valid!!", "RFID Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
        public DataTable GetStudentByRFID(string rfid, string event_id)
        {
            DataTable studentInfoTable = new DataTable();
            studentInfoTable.Rows.Clear();
            string query;
            if (IsEventForSpecificStudents(event_id))
            {
                query = @"SELECT stud.Profile_Pic AS pic,
                               ID,
                               sec.Description AS secdes, 
                               dep.Description AS depdes, 
                               UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM tbl_attendance AS att
                        LEFT JOIN tbl_events AS e ON att.Event_ID = e.Event_ID
                        LEFT JOIN tbl_students_account AS stud ON instr(e.Specific_Students, stud.FirstName || ' ' || stud.LastName) > 0
                        LEFT JOIN tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE RFID = @rfid AND Status = 1 AND e.Event_ID = '" + event_id+"'";
            }
            else
            {
                query = @"SELECT
                            stud.Profile_Pic as pic,
                            ID,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE RFID = @rfid AND Status = 1";
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@rfid", rfid);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(studentInfoTable);
                    }
                }
            }


            DataTable newDataTable = new DataTable();
            newDataTable.Rows.Clear();
            newDataTable.Columns.Add("ID", typeof(string));
            newDataTable.Columns.Add("Name", typeof(string));
            newDataTable.Columns.Add("secdes", typeof(string));
            newDataTable.Columns.Add("depdes", typeof(string));
            newDataTable.Columns.Add("pic", typeof(Image));

            foreach (DataRow row in studentInfoTable.Rows)
            {
                object imageData = row["pic"];
                byte[] imageBytes = (byte[])imageData;
                if (imageBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image image = Image.FromStream(ms);
                        newDataTable.Rows.Add(row["ID"], row["Name"], row["secdes"], row["depdes"], image);
                    }
                }

            }

            return newDataTable;
        }
        public DataTable GetStudentByManual(string studid, string event_id)
        {
            DataTable studentInfoTable = new DataTable();
            studentInfoTable.Rows.Clear();

            string query;
            if (IsEventForSpecificStudents(event_id))
            {
                query = @"SELECT stud.Profile_Pic AS pic,
                               ID,
                               sec.Description AS secdes, 
                               dep.Description AS depdes, 
                               UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM tbl_attendance AS att
                        LEFT JOIN tbl_events AS e ON att.Event_ID = e.Event_ID
                        LEFT JOIN tbl_students_account AS stud ON instr(e.Specific_Students, stud.FirstName || ' ' || stud.LastName) > 0
                        LEFT JOIN tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE ID = @id AND Status = 1 AND e.Event_ID = '" + event_id + "'";
            }
            else
            {
                query = @"SELECT
                            stud.Profile_Pic as pic,
                            ID,
                            sec.Description AS secdes, 
                            dep.Description AS depdes, 
                            UPPER(stud.Firstname) || ' ' || UPPER(stud.Middlename) || ' ' || UPPER(stud.Lastname) AS Name
                        FROM 
                            tbl_attendance AS att
                        LEFT JOIN 
                            tbl_students_account AS stud ON att.Student_ID = stud.ID
                        LEFT JOIN 
                            tbl_departments AS dep ON stud.Department = dep.Department_ID
                        LEFT JOIN 
                            tbl_section AS sec ON stud.Section = sec.Section_ID
                        WHERE ID = @id";
            }

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", studid);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(studentInfoTable);
                    }
                }
            }


            DataTable newDataTable = new DataTable();
            newDataTable.Rows.Clear();
            newDataTable.Columns.Add("ID", typeof(string));
            newDataTable.Columns.Add("Name", typeof(string));
            newDataTable.Columns.Add("secdes", typeof(string));
            newDataTable.Columns.Add("depdes", typeof(string));
            newDataTable.Columns.Add("pic", typeof(Image));

            foreach (DataRow row in studentInfoTable.Rows)
            {
                object imageData = row["pic"];
                byte[] imageBytes = (byte[])imageData;
                if (imageBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        Image image = Image.FromStream(ms);
                        newDataTable.Rows.Add(row["ID"], row["Name"], row["secdes"], row["depdes"], image);
                    }
                }

            }

            return newDataTable;
        }
        public void GetManualStudentForAttendance(string stud_id, string eventID, string date, string amIn, string amOut, string pmIn, string pmOut, string checker)
        {
            string attendance_stud_id = null;
            DateTime dateTimeNow = DateTime.ParseExact(date, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
            string formattedDate = dateTimeNow.ToString("M/d/yyyy");

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                // Retrieve student information
                string query;
                if (IsEventForSpecificStudents(eventID))
                {
                    query = @"SELECT s.ID FROM tbl_events e
                            LEFT JOIN tbl_students_account s ON instr(e.Specific_Students, s.FirstName || ' ' || s.LastName) > 0 
                            LEFT JOIN tbl_departments AS dep ON s.Department = dep.Department_ID 
                            WHERE s.Status = 1 AND e.Event_ID = '"+eventID+"' AND s.ID = @id ";
                }
                else
                {
                    query = "SELECT ID FROM tbl_students_account WHERE ID = @id";
                }

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", stud_id); // Use parameterized query to prevent SQL injection
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            stud_id = reader["ID"].ToString();
                        }
                    }
                }

                if (!string.IsNullOrEmpty(stud_id))
                {

                    query = "SELECT Student_ID FROM tbl_attendance WHERE Student_ID = @id AND Event_ID = @event AND SUBSTR(Date_Time, 1, INSTR(Date_Time, ' ') - 1) = @date";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", stud_id);
                        command.Parameters.AddWithValue("@event", eventID);
                        command.Parameters.AddWithValue("@date", formattedDate);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                attendance_stud_id = reader["Student_ID"].ToString();
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(attendance_stud_id))
                    {
                        // Create SQL UPDATE statements to update the corresponding columns
                        if (!string.IsNullOrEmpty(amIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "UPDATE tbl_attendance SET AM_IN = @amIn WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(amOut))
                        {
                            // Existing record found, UPDATE the AM-OUT column
                            query = "UPDATE tbl_attendance SET AM_OUT = @amOut WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(pmIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "UPDATE tbl_attendance SET PM_IN = @pmIn WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                        else if (!string.IsNullOrEmpty(pmOut))
                        {
                            // Existing record found, UPDATE the PM-OUT column
                            query = "UPDATE tbl_attendance SET PM_OUT = @pmOut WHERE Student_ID = @studentID AND Event_ID = @eventID";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(amIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, AM_IN, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @amIn, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(amOut))
                        {
                            // Existing record found, UPDATE the AM-OUT column
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, AM_OUT, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @amOut, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(pmIn))
                        {
                            // No existing record found, INSERT a new record
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, PM_IN, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @pmIn, @checker)";
                        }
                        else if (!string.IsNullOrEmpty(pmOut))
                        {
                            // Existing record found, UPDATE the PM-OUT column
                            query = "INSERT INTO tbl_attendance (Student_ID, Event_ID, Date_Time, PM_OUT, Checker) " +
                                    "VALUES (@studentID, @eventID, @date, @pmOut, @checker)";
                        }
                    }

                    using (SQLiteCommand updateCommand = new SQLiteCommand(query, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@studentID", stud_id);
                        updateCommand.Parameters.AddWithValue("@eventID", eventID);
                        updateCommand.Parameters.AddWithValue("@date", date); // Include Date_Time in the UPDATE queries
                        updateCommand.Parameters.AddWithValue("@amIn", amIn);
                        updateCommand.Parameters.AddWithValue("@amOut", amOut);
                        updateCommand.Parameters.AddWithValue("@pmIn", pmIn);
                        updateCommand.Parameters.AddWithValue("@pmOut", pmOut);
                        updateCommand.Parameters.AddWithValue("@checker", checker);

                        updateCommand.ExecuteNonQuery();
                    }
                }
                else
                {
                    MessageBox.Show("Student is Not Registered or Not Valid!!", "Attendance Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        public DataTable GetAttendanceRecord()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM tbl_attendance";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                {
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }
    }
}
