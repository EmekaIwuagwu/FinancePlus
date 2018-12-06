using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FinancePlus
{
    public partial class Infoform : Form
    {
        public Infoform()
        {
            InitializeComponent();

            GetComputerInformation();
            GetUserStatistics();
        }

        private void GetUserStatistics()
        {
            try
            {
                using (banksqlEntities1 db = new banksqlEntities1())
                {
                    chart1.DataSource = db.userStats.ToList();
                    chart1.Series["UsrStats"].XValueMember = "Year";
                    chart1.Series["UsrStats"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
                    chart1.Series["UsrStats"].YValueMembers = "Total";
                    chart1.Series["UsrStats"].YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void GetComputerInformation()
        {
            string day = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
            string computername = System.Environment.MachineName;
            string LocalipIddress = getLocalIP();

            label5.Text = computername;
            label6.Text = day;
            label7.Text = LocalipIddress;
        }

        private string getLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }
    }
}
