using ComponentFactory.Krypton.Toolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace AMSEMS.SubForms_DeptHead
{
    public partial class formMakePayment : KryptonForm
    {
        SqlConnection cn;
        SqlDataAdapter ad;
        SqlCommand cm;
        SqlDataReader dr;


        string stud_id, date;

        public bool paystatus = false;

        formStudentBalanceFee formStudentBalanceFee;
        private string schYear;
        private string Tersem;
        private string Shssem;

        public formMakePayment()
        {
            InitializeComponent();
            btnPay.Enabled = false;

            ToolTip toolTip = new ToolTip();
            toolTip.InitialDelay = 500;
            toolTip.AutoPopDelay = int.MaxValue;

            toolTip.SetToolTip(btnSearch, "Search Student");

            using (SqlConnection cn = new SqlConnection(SQL_Connection.connection))
            {
                cn.Open();
                string query = "SELECT Academic_Year_Start + '-' + Academic_Year_End AS SchYear, Ter_Academic_Sem, SHS_Academic_Sem FROM tbl_acad";
                using (SqlCommand command = new SqlCommand(query, cn))
                {
                    command.CommandText = query;
                    using (SqlDataReader rd = command.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            schYear = rd["SchYear"].ToString();
                            Tersem = rd["Ter_Academic_Sem"].ToString();
                            Shssem = rd["SHS_Academic_Sem"].ToString();
                        }
                    }
                }
            }
        }
        public void getForm(formStudentBalanceFee formStudentBalanceFee)
        {
            this.formStudentBalanceFee = formStudentBalanceFee;
        } 
        private void formEventConfig_Load(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            string formatDate = dateTime.ToString("MM-dd-yyyy");
            lblDate.Text = formatDate;
        }
        public void searchStudent(string stud_id)
        {
            tbStudID.Text = stud_id;
            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand(@"SELECT
                                        COALESCE(bf.Student_ID, t.Student_ID) AS Student_ID,
                                        s.Lastname AS lname,
                                        s.Firstname AS fname,
                                        s.Middlename AS mname,
                                        COALESCE(SUM(bf.Balance_Fee), 0) AS Total_Balance_Fee,
                                        COALESCE(SUM(t.Payment_Amount), 0) AS Total_Payment_Amount,
                                        CASE
                                            WHEN COALESCE(SUM(bf.Balance_Fee), 0) < COALESCE(SUM(t.Payment_Amount), 0)
                                                THEN 0
                                            ELSE COALESCE(SUM(bf.Balance_Fee), 0) - COALESCE(SUM(t.Payment_Amount), 0)
                                        END AS Remaining_Balance
                                    FROM (
                                        SELECT
                                            Student_ID,
                                            SUM(Balance_Fee) AS Balance_Fee
                                        FROM
                                            dbo.tbl_balance_fees
                                        GROUP BY
                                            Student_ID
                                    ) bf
                                    FULL JOIN (
                                        SELECT
                                            Student_ID,
                                            SUM(Payment_Amount) AS Payment_Amount
                                        FROM
                                            dbo.tbl_transaction
                                        GROUP BY
                                            Student_ID
                                    ) t ON bf.Student_ID = t.Student_ID
                                    JOIN dbo.tbl_student_accounts s ON COALESCE(bf.Student_ID, t.Student_ID) = s.ID
                                    LEFT JOIN tbl_total_penalty_fee tp ON s.ID = tp.Student_ID
                                    WHERE
                                        s.Status = 1
                                        AND s.Department = @dep
                                        AND s.ID = @id
                                    GROUP BY
                                        COALESCE(bf.Student_ID, t.Student_ID),
                                        s.Lastname,
                                        s.Firstname,
                                        s.Middlename;", cn);
                    cm.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                    cm.Parameters.AddWithValue("@id", stud_id);
                    using (dr = cm.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            this.stud_id = dr["Student_ID"].ToString();
                            string name = dr["lname"].ToString() + ", " + dr["fname"].ToString() + " " + dr["mname"].ToString();
                            double balance = (dr["Remaining_Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Remaining_Balance"]);
                            double amount_paid = (dr["Total_Payment_Amount"] == DBNull.Value) ? 0 : Convert.ToDouble(dr["Total_Payment_Amount"]);

                            lblName.Text = name.ToUpper();
                            lblBalanceFee.Text = "₱ " + balance.ToString("F2");
                            lblAmountPaid.Text = "₱ " + amount_paid.ToString("F2");
                            btnPay.Enabled = true;
                        }
                        else
                        {
                            lblName.Text = "No data found";
                            btnPay.Enabled = false;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public void clearText()
        {
            tbStudID.Text = "";
            tbPayment.Text = "00";
            lblName.Text = "Name";
            lblAmountPaid.Text = "₱ 0.00";
            lblAmountPaid.Text = "₱ 0.00";
            lblBalanceFee.Text = "₱ 0.00";
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            clearText();
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            stud_id = tbStudID.Text;
            searchStudent(stud_id);
        }

        private void btnPay_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to make this payment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Ensure the payment amount is a valid number
                if (double.TryParse(tbPayment.Text, out double paymentAmount))
                {
                    // Ensure payment amount is not greater than the balance fee
                    double balanceFee = (lblBalanceFee.Text == "₱ 0.00") ? 0 : Convert.ToDouble(lblBalanceFee.Text.Substring(2));

                    if (paymentAmount <= balanceFee)
                    {
                        payAmount();
                        ExecuteStoredProcedure();
                        searchStudent(stud_id);
                        MessageBox.Show("Payment Done.", "Payment", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        formStudentBalanceFee.displayBalanceFees();
                        formStudentBalanceFee.displayOverallSummary();
                    }
                    else
                    {
                        MessageBox.Show("Payment amount cannot be greater than the balance fee.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid payment amount. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }   
        public void payAmount()
        {
            try
            {
                using (cn = new SqlConnection(SQL_Connection.connection))
                {
                    cn.Open();
                    cm = new SqlCommand("INSERT INTO tbl_transaction VALUES (@StudID, @amount, @Date, @SchYear)", cn);
                    cm.Parameters.AddWithValue("@StudID", stud_id);
                    cm.Parameters.AddWithValue("@amount", tbPayment.Text);
                    cm.Parameters.AddWithValue("@Date", lblDate.Text);
                    cm.Parameters.AddWithValue("@SchYear", schYear);
                    cm.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void ExecuteStoredProcedure()
        {
            using (SqlConnection connection = new SqlConnection(SQL_Connection.connection))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("GetTotalPenaltyFee", connection))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@dep", FormDeptHeadNavigation.dep);
                        command.ExecuteNonQuery();

                        //MessageBox.Show("Stored procedure executed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tbStudID_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                stud_id = tbStudID.Text;
                searchStudent(stud_id);
            }
        }

        private void tbPayment_Enter(object sender, EventArgs e)
        {
            tbPayment.Text = String.Empty;
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like Backspace)
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
            {
                // If the pressed key is not a digit or Backspace, suppress it
                e.Handled = true;
            }
        }
    }
}
