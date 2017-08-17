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
        //private static List<T> clientConnected { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] buffer { get; set; }
        private Action<string> ServLogger { get; set; }
        private Action<string, string, int, int, float, float> GridUpdater { get; set; }
        private int LocalPort { get; set; }
        private IPAddress LocalAddress { get; set; }
        private static String response = String.Empty;
        private static List<String> ipListe { get; set; }
        private List<Tuple<List<int>, Node>> Nodes;

        private Node localnode { get; set; }

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);


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
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback),serverSocket);
            //serverSocket.BeginAccept(SplitAndSend, serverSocket);
            SLog("Server setup complete");
            

        }

        private void Receive(Socket client)
        {
            try
            {
                // Create the state object.
                StateObject state = new StateObject();
                state.workSocket = client;

                // Begin receiving the data from the remote device.
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
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
            // Signal the main thread to continue.
            //allDone.Set();

            Socket listener = (Socket)ar.AsyncState;

           

            try
            {
                listener = serverSocket.EndAccept(ar);
                
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            
            IPEndPoint remoteIpEndPoint = listener.RemoteEndPoint as IPEndPoint;
            //String ipAddress = remoteIpEndPoint.Address;
            //ipListe.Add(ipAddress);

            clientSockets.Add(listener);
            StateObject state = new StateObject();
            state.workSocket = listener;
            Receive(listener);
            /*listener.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
           new AsyncCallback(ReceiveCallback), state);*/
            //listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, listener);
            //listener.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, SplitAndSend, listener);
            SLog("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }



        // Receive the message from the client and do action following the input
        /*public void ReceiveCallback(IAsyncResult ar)
        {

            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;
            int received = handler.EndReceive(ar);
            try
            {
                
                

                if (received > 0)
                {
                    // There might be more data, so store the data received so far.
                    Console.WriteLine("MONCUL");
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, received));
                    Console.WriteLine(Encoding.ASCII.GetString(state.buffer, 0, received));
                    
                   Receive(handler);
                    //receiveDone.Set();
                    // Get the rest of the data.
                    //handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    //    new AsyncCallback(ReceiveCallback), state);

                }
                else
                {
                    // All the data has arrived; put it in response.
                    if (state.sb.Length > 1)
                    {

                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received.
                    //Console.WriteLine("FIN");

                    receiveDone.Set();
                    //
                }
            }
            catch (SocketException)
            {
                SLog("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                handler.Close();
                clientSockets.Remove(handler);
                return;
            }
            


            /*
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
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
                clientSockets.Remove(handler);
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
                handler.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, handler);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }*/

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                byte[] coucou = new byte[bytesRead];

                //String lala = Encoding.ASCII.GetString(coucou);

                //Console.WriteLine(lala);
                state.data.Add(state.buffer);

                DataInput input = null;

                try
                {
                    byte[] data = state.data
                                     .SelectMany(a => a)
                                     .ToArray();
                     input = Format.Deserialize<DataInput>(data);
                     Console.WriteLine(input.Method);
                    // Receive après inputmabite

                    Receive(client);
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.ToString());
                    state.data.Add(state.buffer);
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                }

                ;

                /*DataInput dataI = new DataInput()
                {
                    TaskId = 1,
                    SubTaskId = 2,
                    Method = "loulou",
                    Data = "TA GUEULE",
                    NodeGUID = "192.168.31.26"
                };



                Send(ClientSocket, dataI);*/

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }



        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

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

        public void SplitAndSend(String method)
        {
            FileSplitter moncul = new FileSplitter();
            //String fileTosend = moncul.FileReader("C:/Users/loika/Desktop/projet-NDA/Project-NDA/Genomes/genome_kennethreitz.txt");
            String fileTosend = moncul.FileReader("E:/Dev/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");

            //ChunkSplit chunkToUse = new ChunkSplit();
            String chunkToUse = moncul.SplitIntoChunks(fileTosend, 4096, 0);

            DataInput dataI = new DataInput()
            {
                TaskId = 1,
                SubTaskId = 2,
                Method = method,
                Data = chunkToUse,
                NodeGUID = "192.168.31.26"
            };
            
            foreach(Socket clientSocket in clientSockets)
            {

                Send(clientSockets[0], dataI);
            }
            
            //sendDone.WaitOne();
            //clientSockets[0].BeginSend(chunkToUse.chunkBytes, 0, chunkToUse.chunkBytes.Length, SocketFlags.None,new AsyncCallback(SendCallback), clientSockets[0]);

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

        private static void Send(Socket client, DataInput obj)
        {

            byte[] data = Format.Serialize(obj);

            try
            {
                Console.WriteLine("Send data : " + obj + " to : " + client);
                client.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), client);
            }
            catch (SocketException ex)
            {
                /// Client Down ///
                if (!client.Connected)
                    Console.WriteLine("Client " + client.RemoteEndPoint.ToString() + " Disconnected");
                Console.WriteLine(ex.ToString());
            }
        }




    }
}
