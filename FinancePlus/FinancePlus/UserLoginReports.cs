using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class UserLoginReports : Form
    {
        public UserLoginReports()
        {
            InitializeComponent();
        }

        private void UserLoginReports_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'LoginReports.login_reports' table. You can move, or remove it, as needed.

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.login_reportsTableAdapter.Fill(this.LoginReports.login_reports, dateFrom.Text, dateTo.Text);
            this.reportViewer1.RefreshReport();
        }
    }
}
