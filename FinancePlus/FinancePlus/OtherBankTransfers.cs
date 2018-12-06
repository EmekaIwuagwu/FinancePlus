using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class OtherBankTransfers : Form
    {
        string telephone;
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        double newBal;
        string ccy;

        public OtherBankTransfers()
        {
            InitializeComponent();
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
                                string ccy = (rdr["ccy"].ToString());
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
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (bank_name.Text)
            {
                case "ABZ BANK":
                    try
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
                                    using (SqlDataReader rd = cmd.ExecuteReader())
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
                                    }

                                    string date = Convert.ToDateTime(transaction_date.Text).ToString("dd-MM-yyyy");
                                    string url = "http://localhost:8080/api/api.php?account_number=" + account_dest.Text + "&transaction_desc=" + transaction_desc.Text + "&credit=" + _amt.Text + "&transaction_date=" + date + "";
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    Stream resStream = response.GetResponseStream();
                                    LodgeTransactionInformation();
                                    //MessageBox.Show("OK");

                                    string finalBalance = Convert.ToString(newBal);

                                    currency.Text = ccy;

                                    string Msgbalance = string.Format("{0:n}", double.Parse(finalBalance));
                                    string MsgboxBalance = ccy + " " + Msgbalance;

                                    string DepositMoney = string.Format("{0:n}", double.Parse(_amt.Text));
                                    string FinalWDMoney = ccy + " " + DepositMoney;

                                    string confirmationString = "Action Complete " + Environment.NewLine + " Transaction Confirmation are as Follows : " + Environment.NewLine + " Origin Account: " + accountNo_1.Text + " " + Environment.NewLine + " Withdrawal Amount : " + FinalWDMoney + " " + Environment.NewLine + " Account Holder Name : " + fullname.Text + "" + Environment.NewLine + " Transaction Date: " + transaction_date.Text + " Transaction Desc: " + transaction_desc.Text + "";
                                    MessageBox.Show(confirmationString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    break;

                case "BANK BNZ":
                    try
                    {
                        using (SqlConnection cn = new SqlConnection(constring))
                        {
                            cn.Open();
                            string q = "select * from account_info where accountNo = @accountNo";
                            using (SqlCommand cd = new SqlCommand(q, cn))
                            {
                                cd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                                try
                                {
                                    using (SqlDataReader rd2 = cd.ExecuteReader())
                                    {
                                        if (rd2.Read())
                                        {
                                            string aAmt = (rd2["opening_amount"].ToString());
                                            newBal = Convert.ToDouble(aAmt) - Convert.ToDouble(_amt.Text);
                                        }
                                    }

                                    string qOrigin = "update account_info set opening_amount = '" + newBal + "' where id='" + id.Text + "'";
                                    using (SqlCommand cmd2 = new SqlCommand(qOrigin, cn))
                                    {
                                        cmd2.ExecuteNonQuery();
                                    }

                                    string date = Convert.ToDateTime(transaction_date.Text).ToString("dd-MM-yyyy");
                                    string url = "http://localhost:8080/api/api.php?account_number=" + account_dest.Text + "&transaction_desc=" + transaction_desc.Text + "&credit=" + _amt.Text + "&transaction_date=" + date + "";
                                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                                    Stream resStream = response.GetResponseStream();
                                    LodgeTransactionInformation();
                                    MessageBox.Show("OK");
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    break;
               
            }
        }

        private void LodgeTransactionInformation()
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
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }
            }
        }
    }
}
