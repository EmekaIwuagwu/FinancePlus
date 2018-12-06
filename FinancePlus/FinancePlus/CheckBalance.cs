using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Globalization;

namespace FinancePlus
{
    public partial class CheckBalance : Form
    {
        
        public CheckBalance()
        {
            InitializeComponent();
           // label7.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }

        private void CheckBalance_Load(object sender, EventArgs e)
        {
            //opening_amount.Text = string.Format("{0:n0}", double.Parse(opening_amount.Text));
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string sql = "select * from account_info where accountNo = @accountNo";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                    try
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                fullname.Text = (rdr["fullname"].ToString());
                                textBox3.Text = (rdr["accountNo"].ToString());
                                opening_amount.Text = (rdr["opening_amount"].ToString());
                                account_type.Text = (rdr["account_type"].ToString());
                                address.Text = (rdr["address"].ToString());
                                string ccy = (rdr["ccy"].ToString());
                                id.Text = (rdr["id"].ToString());
                                id_type.Text = (rdr["id_type"].ToString());
                                tel.Text = (rdr["tel"].ToString());
                                id_number.Text = (rdr["id_number"].ToString());
                                bvn.Text = (rdr["bvn"].ToString());

                                byte[] pic = (byte[])rdr["photo"];
                                MemoryStream ms = new MemoryStream(pic);
                                pictureBox2.Image = Image.FromStream(ms);
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                                this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

                                byte[] pic2 = (byte[])rdr["signature"];
                                MemoryStream ms2 = new MemoryStream(pic2);
                                pictureBox3.Image = Image.FromStream(ms2);
                                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                                this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;

                                currency.Text = ccy;

                                //  string balance = ccy + " " + amt;
                                //label10.Text = balance;

                                // double amount = Convert.ToDouble(opening_amount.Text);
                                //opening_amount.Text = ccy +" "+amount;
                                string balance = string.Format("{0:n}", double.Parse(opening_amount.Text));
                                opening_amount.Text = ccy + " " + balance;
                                
                                
                                //double amt = 0.0d;
                                /*if (Double.TryParse(opening_amount.Text, NumberStyles.Currency, null, out amount))
                                {
                                    opening_amount.Text = amount.ToString("C");
                                }*/

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

        private void label13_Click(object sender, EventArgs e)
        {

        }
    }
}
