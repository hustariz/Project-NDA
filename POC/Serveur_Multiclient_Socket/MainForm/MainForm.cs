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



        // Code to put in Client.cs


        private void btn_connection_client_Click(object sender, EventArgs e)
        {
            ConnectToServer(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text));
        }
        private void btn_exit_client_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btn_send_client_Click(object sender, EventArgs e)
        {
            SendRequest();
            if (txt_message_client.Text.ToLower() != "exit")
            {
                ReceiveResponse();
            }

        }



     


        // Server

        private void btn_start_srv_Click(object sender, EventArgs e)
        {
            SetupServer(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text));
        }

        private void btn_stop_srv_Click(object sender, EventArgs e)
        {

        }


      

    }
}
