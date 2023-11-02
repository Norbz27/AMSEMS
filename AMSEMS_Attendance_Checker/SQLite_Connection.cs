using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

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
                                                    Image BLOB);";
                    command.CommandText = createTableSql;
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void InsertData(string name, int age)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string insertSql = "INSERT INTO MyTable (Name, Age) VALUES (@Name, @Age);";
                    command.CommandText = insertSql;
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Age", age);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        public DataTable SelectData()
        {
            DataTable dataTable = new DataTable();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand("SELECT * FROM MyTable;", connection))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }

                connection.Close();
            }
            return dataTable;
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
