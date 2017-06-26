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

namespace ClientSocket
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //Get your own IP
            foreach (IPAddress adress in localIP)
            {
                if (adress.AddressFamily == AddressFamily.InterNetwork)
                {
                    txt_host_client.Text = adress.ToString();
                }
            }
        }
        private static readonly Socket ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);


        private void btn_connection_client_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            SendRequest();
            ReceiveResponse();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void ConnectToServer()
        {
            int attempts = 0;

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    txt_status_client.Text += ("Connection attempt " + attempts + "\r\n");
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.

                    ClientSocket.Connect(IPAddress.Parse(txt_host_client.Text), Int32.Parse(txt_port_client.Text));
                }
                catch (SocketException)
                {
                    txt_status_client.Text = "SocketException";
                }
            }

            txt_status_client.Text = "";
            txt_status_client.Text += ("Connected \r\n");
        }

        /// <summary>
        /// Close socket and exit program.
        /// </summary>
        private void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Environment.Exit(0);
        }

        private void SendRequest()
        {

            string request = txt_message_client.Text;
            txt_status_client.Text += ("request sent : " + request + "\r\n");
            SendString(request);

            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        /// <summary>
        /// Sends a string to the server with ASCII encoding.
        /// </summary>
        private void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);

            txt_status_client.Text += ("Response : " + text + "\r\n");
        }


    }
}
