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
    public partial class ManageUsers : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        public ManageUsers()
        {
            InitializeComponent();
        }

        /* private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string q = "insert into userinfo (userid,pass,pass2,email,tel,remarks,photo) values (@userid,@pass,@pass2,@email,@tel,@remarks,@photo) ";
                if (pass.Text != pass2.Text)
                {
                    MessageBox.Show("Passwords Dont Match!");
                }
                
                else
                {
                    using (SqlCommand cmd = new SqlCommand(q, cn))
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream();
                            pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                            byte[] photo = ms.ToArray();

                            cmd.Parameters.AddWithValue("@userid", userid.Text);
                            cmd.Parameters.AddWithValue("@pass", pass.Text);
                            cmd.Parameters.AddWithValue("@pass2", pass2.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@tel", tel.Text);
                            cmd.Parameters.AddWithValue("@remarks", remarks.Text);
                            cmd.Parameters.AddWithValue("@photo", photo);
                            cmd.ExecuteNonQuery();
                            string details = "User Created "+Environment.NewLine+"User ID : "+userid.Text+""+Environment.NewLine+ "Password : "+pass.Text+"";
                            MessageBox.Show(details, "User Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.ToString());
                        }
                    }
                }
            }
        }

    */

        private void button1_Click_1(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                string q = "insert into userinfo (userid,pass,pass2,email,tel,remarks,photo) values (@userid,@pass,@pass2,@email,@tel,@remarks,@photo) ";
                if (pass.Text != pass2.Text)
                {
                    MessageBox.Show("Passwords Dont Match!");
                }

                else
                {
                    using (SqlCommand cmd = new SqlCommand(q, cn))
                    {
                        try
                        {
                            MemoryStream ms = new MemoryStream();
                            pictureBox2.Image.Save(ms, pictureBox2.Image.RawFormat);
                            byte[] photo = ms.ToArray();

                            cmd.Parameters.AddWithValue("@userid", userid.Text);
                            cmd.Parameters.AddWithValue("@pass", pass.Text);
                            cmd.Parameters.AddWithValue("@pass2", pass2.Text);
                            cmd.Parameters.AddWithValue("@email", email.Text);
                            cmd.Parameters.AddWithValue("@tel", tel.Text);
                            cmd.Parameters.AddWithValue("@remarks", remarks.Text);
                            cmd.Parameters.AddWithValue("@photo", photo);
                            cmd.ExecuteNonQuery();
                            string details = "User Created " + Environment.NewLine + "User ID : " + userid.Text + "" + Environment.NewLine + "Password : " + pass.Text + "";
                            MessageBox.Show(details, "User Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
}
