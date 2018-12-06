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
using System.Data.OleDb;



namespace FinancePlus
{
    public partial class AccountOpening : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        public AccountOpening()
        {
            InitializeComponent();
            //accountNo.Text = AccountNumber();
            // accountNo.Text = accountNum;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void AccountOpening_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            DisableControls();
        }

        private void DisableControls()
        {
            //On Default Disable Form Inputs
            fullname.Enabled = false;
            address.Enabled = false;
            city.Enabled = false;
            state.Enabled = false;
            email.Enabled = false;
            date_ofbirth.Enabled = false;
            tel.Enabled = false;
            id_type.Enabled = false;
            id_number.Enabled = false;

            bvn_search.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Camera cm = new Camera();
            cm.Show();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void starrtCameraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Camera cm = new Camera();
            cm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string sql = "insert into account_info (fullname,address,city,state,email,tel,id_type,id_number,bvn,date_ofbirth,accountNo,opening_amount,ccy,photo,signature,nok_fullname,nok_address,nok_city,nok_state,nok_email,date_accopen,account_type) values(@fullname,@address,@city,@state,@email,@tel,@id_type,@id_number,@bvn,@date_ofbirth,@accountNo,@opening_amount,@ccy,@photo,@signature,@nok_fullname,@nok_address,@nok_city,@nok_state,@nok_email,@date_accopen,@account_type)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    try
                    {

                        Random r = new Random();
                        string placeH = "200545";
                        string accountNum = placeH + r.Next(1, 9000);
                        accountNo.Text = accountNum;

                        MemoryStream ms = new MemoryStream();
                        pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                        byte[] photo = ms.ToArray();

                        MemoryStream sign_ms = new MemoryStream();
                        pictureBox3.Image.Save(sign_ms, pictureBox3.Image.RawFormat);
                        byte[] signature = sign_ms.ToArray();

                        string date = Convert.ToDateTime(date_ofbirth.Text).ToString("yyyy-MM-dd");
                        string accopenDate = System.DateTime.Now.ToString("yyyy - MM - dd");
                        cmd.Parameters.AddWithValue("@fullname", fullname.Text);
                        cmd.Parameters.AddWithValue("@address", address.Text);
                        cmd.Parameters.AddWithValue("@city", city.Text);
                        cmd.Parameters.AddWithValue("@state", state.Text);
                        cmd.Parameters.AddWithValue("@email", email.Text);
                        cmd.Parameters.AddWithValue("@tel", tel.Text);
                        cmd.Parameters.AddWithValue("@id_type", id_type.Text);
                        cmd.Parameters.AddWithValue("@id_number", id_number.Text);
                        cmd.Parameters.AddWithValue("@bvn", bvn.Text);
                        cmd.Parameters.AddWithValue("@date_ofbirth", date);
                        cmd.Parameters.AddWithValue("@accountNo", accountNo.Text);
                        cmd.Parameters.AddWithValue("@opening_amount", OleDbType.Currency).Value = opening_amount.Text;
                        cmd.Parameters.AddWithValue("@ccy", ccy.Text);
                        cmd.Parameters.AddWithValue("@photo", photo);
                        cmd.Parameters.AddWithValue("@signature", signature);
                        cmd.Parameters.AddWithValue("@nok_fullname", nok_fullname.Text);
                        cmd.Parameters.AddWithValue("@nok_address", nok_address.Text);
                        cmd.Parameters.AddWithValue("@nok_city", nok_city.Text);
                        cmd.Parameters.AddWithValue("@nok_state", nok_state.Text);
                        cmd.Parameters.AddWithValue("@nok_email", nok_email.Text);
                        cmd.Parameters.AddWithValue("@account_type", account_type.Text);
                        cmd.Parameters.AddWithValue("@date_accopen", accopenDate);

                        var msg = MessageBox.Show("Please Confirm all information"+Environment.NewLine+"You are about to Commit Information to Database, Are You sure you want to Continue?","Information",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                        if (msg == DialogResult.Yes)
                        {
                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Account Open"+Environment.NewLine+"Account Information"+Environment.NewLine+ "Account Number : '"+accountNo.Text+"'"+Environment.NewLine+"FullName :'"+fullname.Text+"'"+Environment.NewLine+ "Address :'"+address.Text+"'"+Environment.NewLine+"City: '"+city.Text+"'"+Environment.NewLine+"State: '"+state.Text+"'"+Environment.NewLine+ "Email: '"+email.Text+"'"+Environment.NewLine+"Telephone: '"+tel.Text+"'");
                            LodgeTransaction();
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

        private void LodgeTransaction()
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string q = "insert into transactions (depositor_name,accountNo1,telephone,transaction_desc,credit,balance,transaction_date) values(@depositor_name,@accountNo1,@telephone,@transaction_desc,@credit,@balance,@transaction_date)";
                using (SqlCommand cmd = new SqlCommand(q, cn))
                {
                    try
                    {
                        string today = System.DateTime.Now.ToString("dd-MM-yyyy");
                        string transaction_desc = "Balance BF A/C Open. Balance '" + opening_amount.Text + "'";
                        cmd.Parameters.AddWithValue("@depositor_name", fullname.Text);
                        cmd.Parameters.AddWithValue("@accountNo1", accountNo.Text);
                        cmd.Parameters.AddWithValue("@telephone", tel.Text);
                        cmd.Parameters.AddWithValue("@transaction_desc", transaction_desc);
                        cmd.Parameters.AddWithValue("@credit", opening_amount.Text);
                        cmd.Parameters.AddWithValue("@balance", opening_amount.Text);
                        cmd.Parameters.AddWithValue("@transaction_date", today);
                        cmd.ExecuteNonQuery();
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
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.Image = Image.FromFile(opf.FileName);
                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Choose Image(*.jpg; *.png; *.gif)|*.jpg; *.png; *.gif";
            if (opf.ShowDialog() == DialogResult.OK)
            {
                pictureBox3.Image = Image.FromFile(opf.FileName);
                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            /*  if (radioButton1.Checked == true)
              {
                  //MessageBox.Show("Ok Why Did You call Me?");
                  //radioButton1.Checked = false;

                  branchComboBox.Enabled = true;
                  fullname.Enabled = true;
                  address.Enabled = true;
                  city.Enabled = true;
                  state.Enabled = true;
                  email.Enabled = true;
                  date_ofbirth.Enabled = true;
                  tel.Enabled = true;
                  id_type.Enabled = true;
                  id_number.Enabled = true;
                  nok_fullname.Enabled = true;
                  nok_address.Enabled = true;
                  nok_city.Enabled = true;
                  nok_state.Enabled = true;
                  nok_email.Enabled = true;
                  nok_fullname.Enabled = true;
                  opening_amount.Enabled = true;
                  ccy.Enabled = true;
                  account_type.Enabled = true;
                  bvn.Enabled = true;
                  button2.Enabled = true;
                  button4.Enabled = true;
              }
              else
              {
                  branchComboBox.Enabled = false;
                  fullname.Enabled = false;
                  address.Enabled = false;
                  city.Enabled = false;
                  state.Enabled = false;
                  email.Enabled = false;
                  date_ofbirth.Enabled = false;
                  tel.Enabled = false;
                  id_type.Enabled = false;
                  id_number.Enabled = false;
                  nok_fullname.Enabled = false;
                  nok_address.Enabled = false;
                  nok_city.Enabled = false;
                  nok_state.Enabled = false;
                  nok_email.Enabled = false;
                  nok_fullname.Enabled = false;
                  opening_amount.Enabled = false;
                  ccy.Enabled = false;
                  account_type.Enabled = false;
                  bvn.Enabled = false;
                  button2.Enabled = false;
                  button4.Enabled = false;
              }
          }

      */

            if (radioButton1.Checked == true)
            {
                bvn_search.Enabled = true;

                branchComboBox.Enabled = true;
                fullname.Enabled = false;
                address.Enabled = false;
                city.Enabled = false;
                state.Enabled = false;
                email.Enabled = false;
                date_ofbirth.Enabled = false;
                tel.Enabled = false;
                id_type.Enabled = false;
                id_number.Enabled = false;
            }
            else if (radioButton2.Checked == true)
            {
                bvn_search.Enabled = false;

                branchComboBox.Enabled = true;
                fullname.Enabled = true;
                address.Enabled = true;
                city.Enabled = true;
                state.Enabled = true;
                email.Enabled = true;
                date_ofbirth.Enabled = true;
                tel.Enabled = true;
                id_type.Enabled = true;
                id_number.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            newAccountSearchByBVN.Customer cs;
            newAccountSearchByBVN.FetchInformationBVNService nacc = new newAccountSearchByBVN.FetchInformationBVNService();
            var a = nacc.GetCustomerNameWithBVN(bvn_search.Text);

            cs = nacc.GetCustomerNameWithBVN(bvn_search.Text); 
            if (cs != null)
            {
                fullname.Text = cs.fullname;
                address.Text = cs.address;
                city.Text = cs.city;
                state.Text = cs.state;
                id_type.Text = cs.id_type;
                id_number.Text = cs.id_number;

            }
        }
    }
}
