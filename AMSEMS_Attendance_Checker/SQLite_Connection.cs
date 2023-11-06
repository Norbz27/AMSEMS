using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Drawing;

namespace AMSEMS_Attendance_Checker
{
    public class SQLite_Connection
    {
        private string connectionString;

        public SQLite_Connection(string databasePath)
        {
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
                                                    Image BLOB);
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
                                                    Description TEXT);";
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
                    string clearSql = @"DELETE FROM tbl_teachers_account;
                                        DELETE FROM tbl_students_account;
                                        DELETE FROM tbl_events;
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
        public void InsertStudentData(int unique_id, string id, string fname, string lname, string mname, string pass, byte[] pic, string prog, string sec, string yearlvl, string dep, string role, string status, string datetime)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    byte[] imageBytes = pic;
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_students_account (Unique_ID, ID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime) VALUES (@UniqueID, @ID, @Fname, @Lname, @Mname, @Pass, @Pic, @Prog, @Sec, @YearLvl, @Dep, @Role, @Status, @DateTime);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@UniqueID", unique_id);
                    command.Parameters.AddWithValue("@ID", id);
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
        public void InsertEventsData(string id, string eventname, string startdate, string enddate, string desc, string color, string image)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    // Insert new data
                    string insertSql = "INSERT INTO tbl_events (Event_ID, Event_Name, Start_Date, End_Date, Description, Color, Image) VALUES (@EventID, @EventName, @StartDate, @EndDate, @Desc, @Color, @Image);";

                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@EventID", id);
                    command.Parameters.AddWithValue("@EventName", eventname);
                    command.Parameters.AddWithValue("@StartDate", startdate);
                    command.Parameters.AddWithValue("@EndDate", enddate);
                    command.Parameters.AddWithValue("@Desc", desc);
                    command.Parameters.AddWithValue("@Color", color);
                    command.Parameters.AddWithValue("@Image", image);
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
        public DataTable GetAllStudents()
        {
            DataTable dataTable = new DataTable();

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT Profile_pic, ID, Firstname, Lastname, Middlename, dep.Description AS depdes FROM tbl_students_account as stud LEFT JOIN tbl_departments AS dep ON stud.Department = dep.Department_ID ORDER BY depdes";

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
                                newDataTable.Rows.Add(image, row["ID"], name, row["depdes"]);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception (e.g., log it, display an error message)
                            // Optionally, you can add a placeholder image or null for the problematic data.
                            newDataTable.Rows.Add(null, row["ID"], name, row["depdes"]);
                        }
                    }
                }
                else
                {
                    // Handle the case where the column is null
                    newDataTable.Rows.Add(null, row["ID"], name, row["depdes"]);
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

                string query = "SELECT Event_Name FROM tbl_events WHERE Event_ID = @id";

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



        public void UpdateData(int id, string name, int age)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string updateSql = "UPDATE MyTable SET Name = @Name, Age = @Age WHERE ID = @ID;";
                    command.CommandText = updateSql;
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public void DeleteData(int id)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string deleteSql = "DELETE FROM MyTable WHERE ID = @ID;";
                    command.CommandText = deleteSql;
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
