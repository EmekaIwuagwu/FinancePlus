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
    public partial class AdminDashboard : Form
    {
        public AdminDashboard()
        {
            InitializeComponent();
            GetComputerInformation();
        }

        private void GetComputerInformation()
        {
            string day = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
            string computername = System.Environment.MachineName;
            string LocalipIddress = getLocalIP();

            label2.Text = computername;
            label3.Text = day;
            label5.Text = getLocalIP();
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
