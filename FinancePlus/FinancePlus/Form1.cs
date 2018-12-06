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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        
        private void Form1_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.treeView1.Nodes[0].ExpandAll();
            this.treeView1.Nodes[1].ExpandAll();
            this.treeView1.Nodes[2].ExpandAll();

            f = new Infoform();
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
                case "Account Opening":
                    f.Dispose();
                    f = new AccountOpening();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "Check Balance":
                    f.Dispose();
                    f = new CheckBalance();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "Deposit":
                   f.Dispose();
                    f = new Deposit();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "ID Management":
                    f.Dispose();
                    f = new IDManagement();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;


                /*case "Transfers":
               f.Dispose();
                f = new Transfers();
                f.TopLevel = false;
                this.panel1.Controls.Add(f);
                f.Dock = DockStyle.Fill;
                f.Show();
                break;
                */

                case "Withdrawal":
                    f.Dispose();
                    f = new Withdrawal();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "Same Bank Transfers":
                    f.Dispose();
                    f = new Transfers();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "Balance Sheets":
                    f.Dispose();
                    f = new BalanceSheet();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                case "Other Bank Transfers":
                    f.Dispose();
                    f = new OtherBankTransfers();
                    f.TopLevel = false;
                    this.panel1.Controls.Add(f);
                    f.Dock = DockStyle.Fill;
                    f.Show();
                    break;

                    /* case "Say Hello":
                         f.Dispose();
                         f = new Form3();
                         f.TopLevel = false;
                         this.panel1.Controls.Add(f);
                         f.Dock = DockStyle.Fill;
                         f.Show();
                         break;

                         */
            }
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Password ps = new Password();
            ps.Show();
        }
    }
}
