using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
// C'est normal il est rien censé faire vers le serveur implémente le MVC depuis server.cs vers IHM les infos du coup
//using NetworkAndGenericCalculation.Nodes;
//using NetworkAndGenericCalculation.Worker;
//using NetworkAndGenericCalculation.Sockets;

namespace MainForms
{
    public partial class MainForm : Form
    {

        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // List of clientSocket for multiple connection from client
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        //private Node localnode;
        //private Server socketServer;


        //Accept the connection of multiple client
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
            AppendSrvStatus("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        // Receive the message from the client and do action following the input
        public void ReceiveCallback(IAsyncResult AR)
        {
            Invoke(new ThreadStart(() =>
            {
                Socket current = (Socket)AR.AsyncState;
                int received;
                try
                {
                    received = current.EndReceive(AR);
                }
                catch (SocketException)
                {
                    AppendSrvStatus("Client forcefully disconnected");
                    // Don't shutdown because the socket may be disposed and its disconnected anyway.
                    current.Close();
                    clientSockets.Remove(current);
                    return;
                }

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string text = Encoding.ASCII.GetString(recBuf);
                AppendSrvStatus("Received Text : " + text);

                if (text.ToLower() == "get time") // Client requested time
                {
                    AppendSrvStatus("Text is a get time request");
                    byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                    //Send data to the client
                    current.Send(data);
                    AppendSrvStatus("Time sent to client");
                }
                else if (text.ToLower() == "exit") // Client wants to exit gracefully
                {
                    // Always Shutdown before closing
                    current.Shutdown(SocketShutdown.Both);
                    current.Close();
                    clientSockets.Remove(current);
                    AppendSrvStatus("Client disconnected");
                    return;
                }
                else
                {
                    AppendSrvStatus("Text is an invalid request");
                    //Send data to the client
                    byte[] data = Encoding.ASCII.GetBytes("Invalid request");
                    current.Send(data);
                    AppendSrvStatus("Warning Sent");
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

        // Update the data grid, timer = 1s
        private void tmr_grid_data_update_Tick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grd_node_data.Rows)
            {
                //Local or Distant node
                //INode node = (INode)row.Cells[0].Value;
                //row.SetValues(node, node.ActualWorker + "/" + node.Workers.Count, Math.Round(node.ProcessorUsage, 2) + "%", node.MemoryUsage + "MB");
            }
        }

        // Server's button event handlers
        private void btn_start_srv_Click(object sender, EventArgs e)
        {
            //socketServer = new Server(this);
            //txt_status_srv.Text += socketServer.AppendSrvStatus("Setting up server...");
            // Enabling timer
            // () => == delegate
            Invoke(new ThreadStart(() => {
                tmr_grid_data_update.Enabled = true;
            }));

            formController.SetupServer(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text));
            //socketServer.SetupServer(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text));
            //txt_status_srv.Text += socketServer.AppendSrvStatus("Server setup complete");
            //AppendSrvStatus("Setting up local node...");
            //localnode = new Node(4, txt_host.Text);
            //ConnectLocalNode(localnode);
        }

        private void btn_stop_srv_Click(object sender, EventArgs e)
        {

        }
    }
}