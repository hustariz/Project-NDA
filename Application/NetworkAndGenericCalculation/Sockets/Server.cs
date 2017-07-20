using NetworkAndGenericCalculation.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainForms;

namespace NetworkAndGenericCalculation.Sockets
{
    public class Server
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // List of clientSocket for multiple connection from client
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];
        private Node localnode;
        private Server socketServer;
        private MainForm view;

        public Server(MainForm view)
        {
            this.view = view;
        }



        public void SetupServer(IPAddress host, int port)
        {
            view.AppendSrvStatus("Setting up server...");
            AppendSrvStatus("Setting up server...");
            serverSocket.Bind(new IPEndPoint(host, port));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(AcceptCallback, null);
            AppendSrvStatus("Server setup complete");
            AppendSrvStatus("Setting up local node...");
            //localnode = new Node(4, txt_host.Text);
        }

        // Append the Server Status Textbox with the argument
        public string AppendSrvStatus(params object[] message)
        {
            return string.Join(" ", message) + Environment.NewLine;
        }

        private void ConnectNode(INode node)
        {
            //AppendSrvStatus("Connected : ", node);
            //Invoke(new ThreadStart(() => {
            //    grd_node_data.Rows.Add(node, "0/" + node.Workers.Count, node.ProcessorUsage + "%", node.MemoryUsage + "MB");
            //}));
        }

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

        private void Invoke(ThreadStart threadStart)
        {
            throw new NotImplementedException();
        }
    }
}
