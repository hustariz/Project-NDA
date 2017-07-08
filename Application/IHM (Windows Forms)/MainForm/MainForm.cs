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
using MainForm;
using System.Threading;

namespace MainForm
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //Get your own IP
            foreach (IPAddress adress in localIP)
            {
                if (adress.AddressFamily == AddressFamily.InterNetwork)
                {
                    txt_host_client.Text = adress.ToString();
                    txt_host.Text = adress.ToString();

                }
            }
        }

        // Append the Server Status Textbox with the argument
        public void AppendSrvStatus(params object[] message)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new ThreadStart(delegate { AppendSrvStatus(message); }));
                else
                    txt_status_srv.AppendText(string.Join(" ", message) + Environment.NewLine);
            }
            catch
            {
                return;
            }
        }

        // Append the Client Status Textbox with the argument
        public void AppendClientStatus(params object[] message)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new ThreadStart(delegate { AppendSrvStatus(message); }));
                else
                    txt_status_client.AppendText(string.Join(" ", message) + Environment.NewLine);
            }
            catch
            {
                return;
            }
        }

    }
}
