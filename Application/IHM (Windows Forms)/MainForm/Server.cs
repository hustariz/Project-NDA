using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForm
{
    public partial class MainForm : Form
    {

        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];



        public void SetupServer(IPAddress host, int port)
        {
                AppendSrvStatus("Setting up server...");
                serverSocket.Bind(new IPEndPoint(host, port));
                serverSocket.Listen(1);
                serverSocket.BeginAccept(AcceptCallback, null);
                AppendSrvStatus("Server setup complete");
        }

        public void AcceptCallback(IAsyncResult AR)
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
            //txt_srv_status.Invoke((MethodInvoker)delegate
            //{
            AppendSrvStatus("Client connected, waiting for request...");
            //});
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            Invoke(new ThreadStart(delegate
            {
                Socket current = (Socket)AR.AsyncState;
                int received;

                try
                {
                    received = current.EndReceive(AR);
                }
                catch (SocketException)
                {
                    AppendSrvStatus("Client forcefully disconnected);
                    // Don't shutdown because the socket may be disposed and its disconnected anyway.
                    current.Close();
                    clientSockets.Remove(current);
                    return;
                }

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string text = Encoding.ASCII.GetString(recBuf);

                //txt_srv_status.Invoke((MethodInvoker)delegate
                //{
                AppendSrvStatus("Received Text : " + text);
                //});

                if (text.ToLower() == "get time") // Client requested time
                {
                    //txt_srv_status.Invoke((MethodInvoker)delegate
                    //{
                    AppendSrvStatus("Text is a get time request");
                    byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                    current.Send(data);
                    AppendSrvStatus("Time sent to client");
                    //});

                }
                else if (text.ToLower() == "exit") // Client wants to exit gracefully
                {
                    // Always Shutdown before closing
                    current.Shutdown(SocketShutdown.Both);
                    current.Close();
                    clientSockets.Remove(current);
                    //txt_srv_status.Invoke((MethodInvoker)delegate
                    //{
                    AppendSrvStatus("Client disconnected");
                    return;
                    //});
                }
                else
                {
                    //txt_srv_status.Invoke((MethodInvoker)delegate
                    //{
                    AppendSrvStatus("Text is an invalid request");
                    byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                    current.Send(data);
                    AppendSrvStatus("Warning Sent");
                    //});
                }
                try
                {
                    current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }));
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
