using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SocketServerForApp.Server;
using SocketServerForApp.Client;

namespace SocketServerForApp
{
    public partial class Form1 : Form
    {
        public static Form serverForm;
        public static Form clientForm; 
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_server_Click(object sender, EventArgs e)
        {
            if (serverForm == null || serverForm.IsDisposed)
            {
                serverForm = new Server.Server();
                serverForm.Show();
            }
            else
            {
                serverForm.BringToFront();
            }
        }

        private void btn_client_Click(object sender, EventArgs e)
        {

            if (clientForm == null || clientForm.IsDisposed)
            {
                clientForm = new Client.Client();
                clientForm.Show();
            }
            else
            {
                clientForm.BringToFront();
            }
        }
    }
}
