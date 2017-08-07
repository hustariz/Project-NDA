using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.FileTreatment;
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
        private static List<Node> nodeConnected { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] buffer { get; set; }
        private Action<string> ServLogger { get; set; }
        private Action<string, string, int, int, float, float> GridUpdater { get; set; }
        private int LocalPort { get; set; }
        private IPAddress LocalAddress { get; set; }
        private Node localnode { get; set; }



        public Server(IPAddress host, int portNumber, Action<string> servLogger, Action<string,string, int, int, float, float> gridupdater)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSockets = new List<Socket>();
            LocalPort = portNumber;
            LocalAddress = host;
            BUFFER_SIZE = 2048;
            buffer = new byte[BUFFER_SIZE];
            ServLogger = servLogger;
            GridUpdater = gridupdater;
            nodeConnected = new List<Node>();
        }


        public void SetupServer()
        {
            SLog("Setting up server...");
            serverSocket.Bind(new IPEndPoint(LocalAddress, LocalPort));
            serverSocket.Listen(1);
            serverSocket.BeginAccept(AcceptCallback, serverSocket);
            //serverSocket.BeginAccept(SplitAndSend, serverSocket);
            SLog("Server setup complete");

        }

        public void ConnectNode(int threadCount, String IP)
        {
            SLog("Setting up local node...");
            localnode = new Node(threadCount, IP);
            SLog("Connected : " + localnode.ToString());
            nodeConnected.Add(localnode);

        }

        public void updateNodeGridData()
        {
            string nodeState = "Active";
            if (localnode.isAvailable) nodeState = "Inactive";
            NLog(localnode.ToString(), nodeState, localnode.ActualWorker, localnode.Workers.Count, localnode.ProcessorUsage, localnode.MemoryUsage);
        }

        //Accept the connection of multiple client
        public void AcceptCallback(IAsyncResult ar)
        {

            Socket listener = (Socket)ar.AsyncState;
            try
            {
                listener = serverSocket.EndAccept(ar);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            clientSockets.Add(listener);
            listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, listener);
            //listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, SplitAndSend, listener);
            SLog("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        // Receive the message from the client and do action following the input
        public void ReceiveCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int received;
            try
            {
                received = client.EndReceive(ar);
            }
            catch (SocketException)
            {
                SLog("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                client.Close();
                clientSockets.Remove(client);
                return;
            }
            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            SLog("Received Text : " + text);

            if (text.ToLower() == "get time") // Client requested time
            {
                SLog("Text is a get time request");
                byte[] callback = Encoding.ASCII.GetBytes(DateTime.Now.ToLongTimeString());
                //SendCallback(ar, callback);
                SLog("Time sent to client");
            }
            else if (text.ToLower() == "exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                client.Shutdown(SocketShutdown.Both);
                client.Close();
                clientSockets.Remove(client);
                SLog("Client disconnected");
                return;
            }
            else
            {
                SLog("Text is an invalid request");

                byte[] callback = Encoding.ASCII.GetBytes("Invalid request : please send 'get time'");
                //Send callback to the client
                //SendCallback(ar, callback);
                SLog("Warning Sent");
            }
            try
            {
                client.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, client);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;

                //Send data to the client
                //client.Send(callback);
                // Complete sending the data to the remote device
                
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        internal void NLog(string nodeAdress, string nodeState, int nodeActiveWThread, int nodeWorkerCount, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            GridUpdater?.Invoke(nodeAdress, nodeState, nodeActiveWThread, nodeWorkerCount, nodeProcessorUsage, nodeMemoryUsage);
        }


        internal void SLog(string msg)
        {
            ServLogger?.Invoke(msg);
        }

        public void SplitAndSend()
        {
            FileSplitter moncul = new FileSplitter();
            String fileTosend = moncul.FileReader("E:/Dev/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");

            //ChunkSplit chunkToUse = new ChunkSplit();
            ChunkSplit chunkToUse = moncul.SplitIntoChunks(fileTosend, 15000, 15000);
            


            clientSockets[0].BeginSend(chunkToUse.chunkBytes, 0, chunkToUse.chunkBytes.Length, SocketFlags.None,new AsyncCallback(SendCallback), clientSockets[0]);

           // clientSockets[0].BeginSend(chunkToUse.chunkBytes ,0, AcceptCallback, clientSockets[0]);




            // Serveur envoyer la méthode / Texte
            // Thread
            // Si node available
            // Envoyer un tableau chunk
            // Si pas worker 
            // Thread.sleep
            // jusqu'à un événement worker dispo

            //chunkToUse.chunkBytes

        }

    }
}
