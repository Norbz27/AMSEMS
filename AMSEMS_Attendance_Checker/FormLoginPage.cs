using AMSEMS;
using ComponentFactory.Krypton.Toolkit;
using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AMSEMS_Attendance_Checker
{
    public partial class FormLoginPage : KryptonForm
    {
        SQLite_Connection sQLite_Connection;
        SqlConnection cnn;
        SqlCommand cm;
        SqlDataReader dr;
        private BackgroundWorker backgroundWorker;
        private bool isLoggingIn = false;
        public FormLoginPage()
        {
            InitializeComponent();
            try
            {
                sQLite_Connection = new SQLite_Connection();
                //sQLite_Connection.InitializeDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(BackgroundWorker_DoWork);
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(BackgroundWorker_RunWorkerCompleted);
        }
        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                ptLoading.Visible = true;
            });
        }

        // BackgroundWorker RunWorkerCompleted event handler
        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ptLoading.Visible = false;
        }

        private void tbID_Enter(object sender, EventArgs e)
        {
            if (tbID.Text == "School ID")
            {
                tbID.Text = string.Empty;
                tbID.StateCommon.Content.Color1 = Color.DimGray;
                tbID.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbID.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbID_Leave(object sender, EventArgs e)
        {
            if (tbID.Text == string.Empty)
            {
                tbID.Text = "School ID";
                tbID.StateCommon.Content.Color1 = Color.Silver;
                tbID.StateCommon.Border.Color1 = Color.Gray;
                tbID.StateCommon.Border.Color2 = Color.Gray;
            }
        }

        private void tbPass_Enter(object sender, EventArgs e)
        {
            if (tbPass.Text == "Password")
            {
                tbPass.Text = string.Empty;
                tbPass.StateCommon.Content.Color1 = Color.DimGray;
                tbPass.UseSystemPasswordChar = true;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 8F);
                tbPass.StateCommon.Border.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
                tbPass.StateCommon.Border.Color2 = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(52)))), ((int)(((byte)(132)))));
            }
        }

        private void tbPass_Leave(object sender, EventArgs e)
        {
            if (tbPass.Text == string.Empty)
            {
                tbPass.Text = "Password";
                tbPass.StateCommon.Content.Color1 = Color.Silver;
                tbPass.UseSystemPasswordChar = false;
                tbPass.StateCommon.Content.Font = new System.Drawing.Font("Poppins", 10F);
                tbPass.StateCommon.Border.Color1 = Color.Gray;
                tbPass.StateCommon.Border.Color2 = Color.Gray;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (isLoggingIn) return; // If already logging in, exit

            isLoggingIn = true;
            UseWaitCursor = true;

            try
            {
                await LoginAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
                isLoggingIn = false;
            }
        }

        private async void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (isLoggingIn) return; // If already logging in, exit

                isLoggingIn = true;
                UseWaitCursor = true;

                try
                {
                    await LoginAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "AMSEMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    UseWaitCursor = false;
                    isLoggingIn = false;
                }
            }
        }

        private async Task LoginAsync()
        {
            string id = tbID.Text; // Get the ID from your form
            string pass = tbPass.Text; // Get the password from your form

            try
            {
                DataTable result = await sQLite_Connection.LoginTeacherDataAsync(id, pass);

                if (result.Rows.Count > 0)
                {
                    // Successfully logged in, you can proceed with your logic here
                    formAttendanceCheckerSettings formAttendanceCheckerSettings = new formAttendanceCheckerSettings();
                    formAttendanceCheckerSettings.getTeachID(id);
                    formAttendanceCheckerSettings.ShowDialog();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid credentials. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while logging in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnSync_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sync data from the cloud?", "Cloud Sync Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ptLoading.Visible = true; // Show loading image before starting the synchronization
                await Task.Delay(3000);
                if (IsInternetConnected())
                {
                    try
                    {
                        using (cnn = new SqlConnection(SQL_Connection.connection))
                        {
                            await cnn.OpenAsync();
                            sQLite_Connection.ClearData();
                            sQLite_Connection.ClearTeachersData();
                            sQLite_Connection.ClearAttendaceData();
                            sQLite_Connection.ClearEventData();
                            cm = new SqlCommand("SELECT Unique_ID, ID, Firstname, Lastname, Middlename, Password, Profile_pic, Department, Role, Status, DateTime from tbl_teacher_accounts", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertTeacherData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Unique_ID, ID, RFID, Firstname, Lastname, Middlename, Password, Profile_pic, Program, Section, Year_Level, Department, Role, Status, DateTime from tbl_student_accounts", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertStudentData(Convert.ToInt32(dr["Unique_ID"]), dr["ID"].ToString(), dr["RFID"].ToString(), dr["Firstname"].ToString(), dr["Lastname"].ToString(), dr["Middlename"].ToString(), dr["Password"].ToString(), (byte[])dr["Profile_pic"], dr["Program"].ToString(), dr["Section"].ToString(), dr["Year_Level"].ToString(), dr["Department"].ToString(), dr["Role"].ToString(), dr["Status"].ToString(), dr["DateTime"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Event_ID, Event_Name, Start_Date, End_Date, Description, Color, Image, Attendance, Exclusive, Specific_Students, Selected_Departments from tbl_events", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertEventsData(dr["Event_ID"].ToString(), dr["Event_Name"].ToString(), dr["Start_Date"].ToString(), dr["End_Date"].ToString(), dr["Description"].ToString(), dr["Color"].ToString(), dr["Image"].ToString(), dr["Attendance"].ToString(), dr["Exclusive"].ToString(), dr["Specific_Students"].ToString(), dr["Selected_Departments"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Program_ID, Description from tbl_program", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertProgramData(dr["Program_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Section_ID, Description from tbl_section", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertSectionData(dr["Section_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Level_ID, Description from tbl_year_level", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertYearLevelData(dr["Level_ID"].ToString(), dr["Description"].ToString());
                            }
                            dr.Close();

                            cm = new SqlCommand("SELECT Department_ID, Description from tbl_departments", cnn);
                            dr = cm.ExecuteReader();
                            while (dr.Read())
                            {
                                sQLite_Connection.InsertDepartmentData(dr["Department_ID"].ToString(), dr["Description"].ToString());
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
                    MessageBox.Show("No internet connection available. Please check your network connection.");
                }
                ptLoading.Visible = false;
            }
        }
        private bool IsInternetConnected()
        {
            try
            {
                Ping ping = new Ping();
                PingReply reply = ping.Send("www.google.com"); // You can use a reliable external host for testing connectivity.

                if (reply != null && reply.Status == IPStatus.Success)
                {
                    return true; // Internet is reachable.
                }
                return false; // No internet connection.
            }
            catch
            {
                return false;
            }
        }
    }
}
