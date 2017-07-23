using NetworkAndGenericCalculation.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace NetworkAndGenericCalculation.Sockets
{
    public class Server
    {

        private static Socket serverSocket { get; set; }
        // List of clientSocket for multiple connection from client
        private static List<Socket> clientSockets { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] buffer { get; set; }
        private Action<string> Logger { get; set; }
        private int LocalPort { get; set; }
        private IPAddress LocalAddress { get; set; }



        public Server(IPAddress host, int portNumber, Action<string> logger)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSockets = new List<Socket>();
            LocalPort = portNumber;
            LocalAddress = host;
            BUFFER_SIZE = 2048;
            buffer = new byte[BUFFER_SIZE];
            Logger = logger;
        }


        public void SetupServer()
        {
            Log("Setting up server...");
            serverSocket.Bind(new IPEndPoint(LocalAddress, LocalPort));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
            Log("Server setup complete");
            //AppendSrvStatus("Setting up local node...");
            //localnode = new Node(4, txt_host.Text);
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
            Socket list = (Socket)AR.AsyncState;
            try
            {
                list = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            clientSockets.Add(list);
            list.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, list);
            Log("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        // Receive the message from the client and do action following the input
        public void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Log("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }
            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            Log("Received Text : " + text);

            if (text.ToLower() == "get time") // Client requested time
            {
                Log("Text is a get time request");
                byte[] data = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                //Require an invoke if monoinstance
                //Send data to the client
                current.Send(data);
                //String data = DateTime.Now.ToLongTimeString();
                //OnMessageReceived?.Invoke(current, data);
                Log("Time sent to client");
            }
            else if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                Log("Client disconnected");
                return;
            }
            else
            {
                Log("Text is an invalid request");
                //Send data to the client
                byte[] data = Encoding.ASCII.GetBytes("Invalid request : please send 'get time'");
                //String data = "Invalid request";
                //    OnMessageReceived?.Invoke(current, data);
                current.Send(data);
                Log("Warning Sent");
            }
            try
            {
                current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        internal void Log(string msg)
        {
            Logger?.Invoke(msg);
        }

    }
}
