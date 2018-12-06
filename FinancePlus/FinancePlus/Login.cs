using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class Login : Form
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnData"].ConnectionString;
        public Login()
        {
            Thread t = new Thread(new ThreadStart(Splash));
            t.Start();
            InitializeComponent();

            string str = string.Empty;
            for (int i = 0; i < 40000; i++)
            {
                str += i.ToString();
            }
            t.Abort();
        }

        private void Splash()
        {
            try
            {
                SplashScreen.SplashForm frm = new SplashScreen.SplashForm();
                frm.AppName = "Finance Plus™";
                Application.Run(frm);
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'banksqlDataSet5.branches' table. You can move, or remove it, as needed.
            //this.branchesTableAdapter.Fill(this.banksqlDataSet5.branches);
            // TODO: This line of code loads data into the 'banksqlDataSet4.branches' table. You can move, or remove it, as needed.
            //this.branchesTableAdapter.Fill(this.banksqlDataSet4.branches);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void LodgeReports()
        {
            DateTime localDate = DateTime.Now;
            string date = System.DateTime.Now.ToString("yyyy-MM-dd");
            string year = System.DateTime.Now.ToString("yyyy");
            string data = "Login Successful using username As : '" + userid.Text + "' by '" + localDate + "' Today: '" + date + "'";
            string sql = "insert into login_reports(data,date,year)values(@data,@date,@year)";
            using (SqlConnection cn = new SqlConnection(constring))
            {
                cn.Open();
                using (SqlCommand cd = new SqlCommand(sql, cn))
                {
                    cd.Parameters.AddWithValue("@data", data);
                    cd.Parameters.AddWithValue("@date", date);
                    cd.Parameters.AddWithValue("@year", year);
                    cd.ExecuteNonQuery();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            using (SqlConnection con = new SqlConnection(constring))
            {
                con.Open();
                string sql = "select * from userinfo where userid= @userid and pass =@pass";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@userid", userid.Text);
                    cmd.Parameters.AddWithValue("@pass", pass.Text);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        if (userid.Text == "admin")
                        {
                            AdminArea admin = new AdminArea();
                            LodgeReports();
                            admin.Show(); 
                            this.Hide();
                        }
                        else if (userid.Text != "admin")
                        {
                            Form1 frm = new Form1();
                            LodgeReports();
                            frm.Show();
                            this.Hide();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Username and Password", "Failed Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
