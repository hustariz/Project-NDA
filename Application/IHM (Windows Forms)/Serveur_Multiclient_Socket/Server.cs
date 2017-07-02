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

namespace MultiServeurSocket
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); //Get your own IP
            foreach(IPAddress adress in localIP)
            {
                if(adress.AddressFamily == AddressFamily.InterNetwork)
                {
                    txt_host.Text = adress.ToString();
                }
            }
        }
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];




        private void btn_start_srv_Click(object sender, EventArgs e)
        {
            SetupServer();
        }

        private void btn_stop_srv_Click(object sender, EventArgs e)
        {

        }

        private void SetupServer()
        {
            txt_srv_status.Text += ("Setting up server...\r\n");
            serverSocket.Bind(new IPEndPoint(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text)));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(AcceptCallback, null);
            txt_srv_status.Text += ("Server setup complete\r\n");
        }
        private void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            txt_srv_status.Text +=  ("Client connected, waiting for request...\r\n");
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                txt_srv_status.Text += ("Client forcefully disconnected\r\n");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            txt_srv_status.Text +=  ("Received Text : " + text + "\r\n");

            if (text.ToLower() == "get time") // Client requested time
            {
                txt_srv_status.Text += ("Text is a get time request\r\n");
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                current.Send(data);
                txt_srv_status.Text += ("Time sent to client\r\n");
            }
            else if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                txt_srv_status.Text += ("Client disconnected\r\n");
                return;
            }
            else
            {
                txt_srv_status.Text += ("Text is an invalid request\r\n");
                byte[] data = Encoding.ASCII.GetBytes("Invalid request\r\n");
                current.Send(data);
                txt_srv_status.Text += ("Warning Sent\r\n");
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }

    }
}
