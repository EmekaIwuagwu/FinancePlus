﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class Deposit : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        double newBal;
        string ccy;
        public Deposit()
        {
            InitializeComponent();
        }

        private void Deposit_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string q = "select * from account_info where accountNo = @accountNo";
                using (SqlCommand cd = new SqlCommand(q, cn))
                {
                    cd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                    using (SqlDataReader rd = cd.ExecuteReader())
                    {
                        try
                        {
                            if (rd.Read())
                            {
                                fullname.Text = (rd["fullname"].ToString());
                                accountNo1.Text = (rd["accountNo"].ToString());
                                opening_amount.Text = (rd["opening_amount"].ToString());
                                account_type.Text = (rd["account_type"].ToString());
                                id.Text = (rd["id"].ToString());
                                ccy = (rd["ccy"].ToString());

                                currency.Text = ccy;

                                string balance = string.Format("{0:n}", double.Parse(opening_amount.Text));
                                opening_amount.Text = ccy + " " + balance;

                            }
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                var msg = MessageBox.Show("Please Confirm if you want to continue", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (msg == DialogResult.Yes)
                {
                    string q = "select * from account_info where accountNo = @accountNo";
                    using (SqlCommand cmd = new SqlCommand(q, cn))
                    {
                        cmd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            try
                            {
                                if (rdr.Read())
                                {
                                    string fullname = (rdr["fullname"].ToString());
                                    string account_number = (rdr["accountNo"].ToString());
                                    string aAmt = (rdr["opening_amount"].ToString());
                                    string account_type = (rdr["account_type"].ToString());

                                    newBal = Convert.ToDouble(aAmt) + Convert.ToDouble(amt.Text);
                                }
                            }

                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
                            }
                        }

                        string sql2 = "update account_info set opening_amount = '" + newBal + "' where id='" + id.Text + "'";
                        using (SqlCommand cmd2 = new SqlCommand(sql2, cn))
                        {
                            cmd2.ExecuteNonQuery();
                            string finalBalance = Convert.ToString(newBal);

                            currency.Text = ccy;

                            string Msgbalance = string.Format("{0:n}", double.Parse(finalBalance));
                            string MsgboxBalance = ccy + " " + Msgbalance;

                            string DepositMoney = string.Format("{0:n}", double.Parse(amt.Text));
                            string FinalDepositMoney = ccy + " " + DepositMoney;

                            string confirmationString = "Action Complete "+Environment.NewLine+" Transaction Confirmation are as Follows : "+Environment.NewLine+" Origin Account: "+ accountNo1.Text + " "+Environment.NewLine+" Deposit Amount : "+ FinalDepositMoney + " Balance: "+ MsgboxBalance + " "+Environment.NewLine+" Depositor Name : "+depositor_name.Text+""+Environment.NewLine+" Transaction Date: "+transaction_date.Text+" Transaction Desc: "+transaction_desc.Text+"";
                            MessageBox.Show(confirmationString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LodgeAccountTransaction();
                            SendSMSTransactionNotification();
                            Close();
                        }
                    }
                }
            }
        }

        private void SendSMSTransactionNotification()
        {
            string finalBalance = Convert.ToString(newBal);
            currency.Text = ccy;
            string Msgbalance = string.Format("{0:n}", double.Parse(finalBalance));
            string MsgboxBalance = ccy + " " + Msgbalance;

            string DepositMoney = string.Format("{0:n}", double.Parse(amt.Text));
            string FinalDepositMoney = ccy + " " + DepositMoney;
            string depositNotificationString = "Account Number :" + accountNo1.Text + " \nDesposit Amount :" + FinalDepositMoney + "\n Balance: " + MsgboxBalance + "";

            string UserName = "malaysianburger@meprice.co";
            string password = "crlr1";
            string sender = "LAPO@LapoNet";
            string smsMsg = depositNotificationString;
            /*    string url = "http://smsc.vianett.no/v3/send.ashx?" +
                "src=" + sender +
                "&dst=" + telephone.Text +
                "&msg=" + System.Web.HttpUtility.UrlEncode(depositNotificationString, System.Text.Encoding.GetEncoding("ISO-8859-1")) +
                "&username=" + System.Web.HttpUtility.UrlEncode(UserName) +
                "&password=" + password;

                */

            string url = "http://smsc.vianett.no/v3/send.ashx?src="+sender+"&dst="+telephone.Text+"&msg="+smsMsg+"&username="+UserName+"&password="+password+" ";

                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Stream resStream = response.GetResponseStream();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        private void LodgeAccountTransaction()
        {
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string sql = "insert into transactions (depositor_name,accountNo1,telephone,transaction_desc,credit,balance,transaction_date) values(@depositor_name,@accountNo1,@telephone,@transaction_desc,@credit,@balance,@transaction_date) ";
                using (SqlCommand cd = new SqlCommand(sql, con))
                {
                    try
                    {
                        string date = Convert.ToDateTime(transaction_date.Text).ToString("dd-MM-yyyy");
                        cd.Parameters.AddWithValue("@depositor_name", depositor_name.Text);
                        cd.Parameters.AddWithValue("@accountNo1", accountNo1.Text);
                        cd.Parameters.AddWithValue("@telephone", telephone.Text);
                        cd.Parameters.AddWithValue("@transaction_desc", transaction_desc.Text);
                        cd.Parameters.AddWithValue("@credit", amt.Text);
                        cd.Parameters.AddWithValue("@balance", newBal);
                        cd.Parameters.AddWithValue("@transaction_date", date);
                        cd.ExecuteNonQuery();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                }

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                try
                {
                    using (SqlConnection cn = new SqlConnection(constring))
                    {
                        cn.Open();
                        string sql = "select * from account_info where accountNo = @accountNo";
                        using (SqlCommand cmd = new SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                            using (SqlDataReader rd = cmd.ExecuteReader())
                            {
                                if (rd.Read())
                                {
                                    depositor_name.Text = (rd["fullname"].ToString());
                                    telephone.Text = (rd["tel"].ToString());

                                    depositor_name.Enabled = false;
                                    telephone.Enabled = false;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else
            {
                if (checkBox1.Checked == false)
                {
                    depositor_name.Clear();
                    telephone.Clear();

                    depositor_name.Enabled = true;
                    telephone.Enabled = true;
                }
            }
        }
    }
}
