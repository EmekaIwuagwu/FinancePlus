using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class Transfers : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        double newBal;
        string telephone;
        string ccy;
        public Transfers()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Transfers_Load(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();

                string q = "select * from account_info where accountNo = @accountNo";
                using (SqlCommand cmd = new SqlCommand(q, cn))
                {
                    cmd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                telephone = (rdr["tel"].ToString());
                                accountNo_1.Text = (rdr["accountNo"].ToString());
                                opening_amount.Text = (rdr["opening_amount"].ToString());
                                ccy = (rdr["ccy"].ToString());
                                fullname.Text = (rdr["fullname"].ToString());
                                bvn.Text = (rdr["bvn"].ToString());
                                id_type.Text = (rdr["id_type"].ToString());
                                id_number.Text = (rdr["id_number"].ToString());
                                id.Text = (rdr["id"].ToString());

                                currency.Text = ccy;

                                string balance = string.Format("{0:n}", double.Parse(opening_amount.Text));
                                opening_amount.Text = ccy + " " + balance;
                            }
                        }

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string sql = "select * from account_info where accountNo = @accountNo";
                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    try
                    {
                        cmd.Parameters.AddWithValue("@accountNo", account_dest.Text);
                        using (SqlDataReader rd = cmd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                id_dest.Text = (rd["id"].ToString());
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string q = "select * from account_info where accountNo =@accountNo";
                using (SqlCommand cd = new SqlCommand(q, cn))
                {
                    cd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                    try
                    {
                        using (SqlDataReader rd = cd.ExecuteReader())
                        {
                            if (rd.Read())
                            {
                                string aAmt = (rd["opening_amount"].ToString());
                                newBal = Convert.ToDouble(aAmt) - Convert.ToDouble(_amt.Text);
                            }
                        }

                        string qOrigin = "update account_info set opening_amount = '" + newBal + "' where id='" + id.Text + "'";
                        using (SqlCommand cmd2 = new SqlCommand(qOrigin, cn))
                        {
                            cmd2.ExecuteNonQuery();
                            LodgeDebitTransactionInformation();
                        }

                        //--------------------------------------------------

                        //Destination Account

                        string esql = "select * from account_info where accountNo = @accountNo";
                        using (SqlCommand cmd3 = new SqlCommand(esql, cn))
                        {
                            cmd3.Parameters.AddWithValue("@accountNo", account_dest.Text);
                            using (SqlDataReader rdr2 = cmd3.ExecuteReader())
                            {
                                try
                                {
                                    if (rdr2.Read())
                                    {
                                        string bAmt = (rdr2["opening_amount"].ToString());
                                        newBal = Convert.ToDouble(bAmt) + Convert.ToDouble(_amt.Text);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }

                        string sql3 = "update account_info set opening_amount = '" + newBal + "' where id='" + id_dest.Text + "'";
                        using (SqlCommand cmd4 = new SqlCommand(sql3, cn))
                        {
                            cmd4.ExecuteNonQuery();
                            //MessageBox.Show("OK");
                            string finalBalance = Convert.ToString(newBal);

                            currency.Text = ccy;

                            string Msgbalance = string.Format("{0:n}", double.Parse(finalBalance));
                            string MsgboxBalance = ccy + " " + Msgbalance;

                            string DepositMoney = string.Format("{0:n}", double.Parse(_amt.Text));
                            string FinalWDMoney = ccy + " " + DepositMoney;

                            string confirmationString = "Action Complete " + Environment.NewLine + " Transaction Confirmation are as Follows : " + Environment.NewLine + " Origin Account: " + accountNo_1.Text + " " + Environment.NewLine + " Withdrawal Amount : " + FinalWDMoney + " Destination Account Balance: " + MsgboxBalance + " " + Environment.NewLine + " Account Holder Name : " + fullname.Text + "" + Environment.NewLine + " Transaction Date: " + transaction_date.Text + " Transaction Desc: " + transaction_desc.Text + "";
                            MessageBox.Show(confirmationString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LodgeCreditTransactionInformation();
                            Close();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void LodgeCreditTransactionInformation()
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();

                string q = "insert into transactions(depositor_name,accountNo1,telephone,transaction_desc,credit,balance,transaction_date) values (@depositor_name,@accountNo1,@telephone,@transaction_desc,@credit,@balance,@transaction_date)";
                using (SqlCommand cmd = new SqlCommand(q, cn))
                {
                    try
                    {
                        string date = Convert.ToDateTime(transaction_date.Text).ToString("dd-MM-yyyy");
                        cmd.Parameters.AddWithValue("@depositor_name", fullname.Text);
                        cmd.Parameters.AddWithValue("@accountNo1", account_dest.Text);
                        cmd.Parameters.AddWithValue("@telephone", telephone);
                        cmd.Parameters.AddWithValue("@transaction_desc", transaction_desc.Text);
                        cmd.Parameters.AddWithValue("@credit", _amt.Text);
                        cmd.Parameters.AddWithValue("@balance", newBal);
                        cmd.Parameters.AddWithValue("@transaction_date", date);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void LodgeDebitTransactionInformation()
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();

                string q = "insert into transactions(depositor_name,accountNo1,telephone,transaction_desc,debit,balance,transaction_date) values (@depositor_name,@accountNo1,@telephone,@transaction_desc,@debit,@balance,@transaction_date)";
                using (SqlCommand cmd = new SqlCommand(q, cn))
                {
                    try
                    {
                        string date = Convert.ToDateTime(transaction_date.Text).ToString("dd-MM-yyyy");
                        cmd.Parameters.AddWithValue("@depositor_name", fullname.Text);
                        cmd.Parameters.AddWithValue("@accountNo1", accountNo_1.Text);
                        cmd.Parameters.AddWithValue("@telephone", telephone);
                        cmd.Parameters.AddWithValue("@transaction_desc", transaction_desc.Text);
                        cmd.Parameters.AddWithValue("@debit", _amt.Text);
                        cmd.Parameters.AddWithValue("@balance", newBal);
                        cmd.Parameters.AddWithValue("@transaction_date", date);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
