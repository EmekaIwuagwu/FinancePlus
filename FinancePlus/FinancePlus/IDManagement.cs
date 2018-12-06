using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class IDManagement : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        public IDManagement()
        {
            InitializeComponent();
        }

        private void IDManagement_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void nok_fullname_TextChanged(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void nok_address_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void nok_city_TextChanged(object sender, EventArgs e)
        {

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
                    try
                    {
                        using (SqlDataReader rd = cd.ExecuteReader())
                        {
                            while (rd.Read())
                            {
                                id.Text = (rd["id"].ToString());
                                fullname.Text = (rd["fullname"].ToString());
                                address.Text = (rd["address"].ToString());
                                city.Text = (rd["city"].ToString());
                                state.Text = (rd["state"].ToString());
                                email.Text = (rd["email"].ToString());
                                date_ofbirth.Text = (rd["date_ofbirth"].ToString());
                                tel.Text = (rd["tel"].ToString());
                                id_type.Text = (rd["id_type"].ToString());
                                id_number.Text = (rd["id_number"].ToString());
                                accountNo_1.Text = (rd["accountNo"].ToString());
                                opening_amount.Text = (rd["opening_amount"].ToString());
                                string ccy = (rd["ccy"].ToString());
                                account_type.Text = (rd["account_type"].ToString());
                                bvn.Text = (rd["bvn"].ToString());

                                byte[] pic = (byte[])rd["photo"];
                                MemoryStream ms = new MemoryStream(pic);
                                pictureBox2.Image = Image.FromStream(ms);
                                pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
                                this.pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;

                                byte[] pic2 = (byte[])rd["signature"];
                                MemoryStream ms2 = new MemoryStream(pic2);
                                pictureBox3.Image = Image.FromStream(ms2);
                                pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
                                this.pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;

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

        private void button4_Click(object sender, EventArgs e)
        {
            fullname.Enabled = true;
            address.Enabled = true;
            city.Enabled = true;
            state.Enabled = true;
            email.Enabled = true;
            date_ofbirth.Enabled = true;
            tel.Enabled = true;
            id_type.Enabled = true;
            id_number.Enabled = true;
            account_type.Enabled = true;
            bvn.Enabled = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();

                string q = "update account_info set fullname = '" + fullname.Text + "',address = '" + address.Text + "',city = '" + city.Text + "',state = '" + state.Text + "',email = '" + email.Text + "',date_ofbirth = '" + date_ofbirth.Text + "', tel = '" + tel.Text + "',id_type = '" + id_type.Text + "', id_number= '" + id_number.Text + "',account_type = '" + account_type.Text + "',bvn = '" + bvn.Text + "' where id='" + id.Text + "'";
                using (SqlCommand cmd = new SqlCommand(q, cn))
                {
                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Information Update Successful", "Account Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        fullname.Enabled = false;
                        address.Enabled = false;
                        city.Enabled = false;
                        state.Enabled = false;
                        email.Enabled = false;
                        date_ofbirth.Enabled = false;
                        tel.Enabled = false;
                        id_type.Enabled = false;
                        id_number.Enabled = false;
                        account_type.Enabled = false;
                        bvn.Enabled = false;
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

        private void button3_Click(object sender, EventArgs e)
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
    }
}
