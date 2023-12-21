using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS.SubForms_Teacher
{
    public partial class formDataSync : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        private SqlConnection cn;
        private SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;
        private bool showMessageOnToggle = false;
        private bool previousToggleState; // Store the previous toggle state
        private BackgroundWorker backgroundWorker = new BackgroundWorker();

        public formDataSync()
        {
            InitializeComponent();
            cn = new SqlConnection(SQL_Connection.connection);
            sQLite_Connection = new SQLite_Connection();
            backgroundWorker.DoWork += backgroundWorker_DoWork;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            backgroundWorker.WorkerSupportsCancellation = true;
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // This method runs in a background thread
            // Perform time-consuming operations here
            //loadAcad();

            // Simulate a time-consuming operation
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // Handle any errors that occurred during the background work
                MessageBox.Show("An error occurred: " + e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Handle the case where the background work was canceled
            }
            else
            {
                // Data has been loaded, update the UI
                // Stop the wait cursor (optional)
                this.Cursor = Cursors.Default;
            }
        }

        private void formAcademicYearSetting_Load(object sender, EventArgs e)
        {
            backgroundWorker.RunWorkerAsync();
        }

        private void formAcademicYearSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                backgroundWorker.CancelAsync();
            }
        }

        private async void btnSync_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sync data from the cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptLoading.Style = ProgressBarStyle.Marquee;
                ptLoading.Visible = true;
                await Task.Delay(3000);
                if (CheckForInternetConnection())
                {
                    try
                    {
                        using (cn = new SqlConnection(SQL_Connection.connection))
                        {
                            await cn.OpenAsync();
                            sQLite_Connection.ClearData();
                            sQLite_Connection.ClearSubjectsData();

                            cm = new SqlCommand("SELECT Unique_ID, ID, Firstname, Lastname, Middlename, Password, Profile_pic, Department, Role, Status, DateTime from tbl_teacher_accounts", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertTeacherData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Course_code, Course_Description, Units, Image, Status, Assigned_Teacher, Academic_Level from tbl_subjects", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertSubjectsData(dr["Course_code"].ToString(), dr["Course_Description"].ToString(), dr["Units"].ToString(), (byte[])dr["Image"], dr["Status"].ToString(), dr["Assigned_Teacher"].ToString(), dr["Academic_Level"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Unique_ID, ID, RFID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime from tbl_student_accounts", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertStudentData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Program"].ToString(), dr["Section"].ToString(), dr["Year_Level"].ToString(), dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Program_ID, Description, AcadLevel_ID from tbl_program", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertProgramData(dr["Program_ID"].ToString(), dr["Description"].ToString(), dr["AcadLevel_ID"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Section_ID, Description, AcadLevel_ID from tbl_section", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertSectionData(dr["Section_ID"].ToString(), dr["Description"].ToString(), dr["AcadLevel_ID"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Level_ID, Description, AcadLevel_ID from tbl_year_level", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertYearLevelData(dr["Level_ID"].ToString(), dr["Description"].ToString(), dr["AcadLevel_ID"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Department_ID, Description, AcadLevel_ID from tbl_departments", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertDepartmentData(dr["Department_ID"].ToString(), dr["Description"].ToString(), dr["AcadLevel_ID"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Academic_Level_ID, Academic_Level_Description from tbl_academic_level", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertAcadLvlData(dr["Academic_Level_ID"].ToString(), dr["Academic_Level_Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Acad_ID, Academic_Year_Start, Academic_Year_End, Ter_Academic_Sem, SHS_Academic_Sem from tbl_acad", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertAcadData(dr["Acad_ID"].ToString(), dr["Academic_Year_Start"].ToString(), dr["Academic_Year_End"].ToString(), dr["Ter_Academic_Sem"].ToString(), dr["SHS_Academic_Sem"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT CLass_Code, Section_ID, Teacher_ID, Course_Code, School_Year, Semester, Acad_Level FROM tbl_class_list", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertClassListData(dr["CLass_Code"].ToString(), dr["Section_ID"].ToString(), dr["Teacher_ID"].ToString(), dr["Course_Code"].ToString(), dr["School_Year"].ToString(), dr["Semester"].ToString(), dr["Acad_Level"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Class_Code, Attendance_date, Student_ID, Student_Status FROM tbl_subject_attendance;", cn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertAttendancetData(dr["CLass_Code"].ToString(), dr["Attendance_date"].ToString(), dr["Student_ID"].ToString(), dr["Student_Status"].ToString());
                            }
                            dr.Close();

                            MessageBox.Show("Successfully Sync Data.", "Sync Confirmation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Unstable Connection!! Can't connect to server!!", "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                ptLoading.Visible = false;
            }
        }
        public static DataTable GetStudList(string tblname)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                connection.Open();

                // Check if the table exists
                string checkTableQuery = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tblname}';";

                using (SqlCommand checkTableCommand = new SqlCommand(checkTableQuery, connection))
                {
                    int tableCount = Convert.ToInt32(checkTableCommand.ExecuteScalar());

                    // If the table does not exist, return null
                    if (tableCount == 0)
                    {
                        return null;
                    }
                }

                // Table exists, proceed with the original query
                string query = $"SELECT * FROM {tblname}";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var mobileClient = new WebClient())
                using (var webConnection = mobileClient.OpenRead("http://www.google.com"))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
