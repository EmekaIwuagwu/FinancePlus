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
    public partial class AdminArea : Form
    {
        public AdminArea()
        {
            InitializeComponent();
        }

        private void AdminArea_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.treeView1.Nodes[0].ExpandAll();

            f = new AdminDashboard();
            f.TopLevel = false;

            this.panel1.Controls.Add(f);
            f.Dock = DockStyle.Fill;
            f.Show();
        }

        private Form f;

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = treeView1.SelectedNode;
            switch (node.Text)
            {
                case "Manage Users":
                    f.Dispose();
                    f = new ManageUsers();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "View Login Reports":
                    f.Dispose();
                    f = new UserLoginReports();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

            }
        }
    }
}
