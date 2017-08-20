using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.FileTreatment;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Nodes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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

    /// <summary>
    /// Orchestrateur
    /// </summary>
    public class Server : IMapper, IReducer
    {

        private static Socket serverSocket { get; set; }
        // List of clientSocket for multiple connection from client
        private static List<Socket> clientSockets { get; set; }
        public List<Client> nodesConnected { get; set; }
        //private static List<Client> clientSockets { get; set; }
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
        private int nbConnectedNode { get; set; }
        private int fileState { get; set; }
        

        public int Length => throw new NotImplementedException();

        public int ChunkDefaultLength => throw new NotImplementedException();

        public int ChunkCount => throw new NotImplementedException();

        public bool IsActive => throw new NotImplementedException();

        public int ChunkRemainsLength => throw new NotImplementedException();

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
            nodesConnected = new List<Client>();
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
            SLog("Server setup complete");
            

        }
        /// <summary>
        /// Fonction permettant de recevoir les données du Client Socket passé en paramètre
        /// </summary>
        /// <param name="client"></param>
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

            //Client clientConnected = new GenomicNode(listener);

            //clientConnected.isAvailable = true;

            try
            {
                listener = serverSocket.EndAccept(ar);
                
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            
            IPEndPoint remoteIpEndPoint = listener.RemoteEndPoint as IPEndPoint;
            String ipAddress = remoteIpEndPoint.Address.ToString();
            String port = remoteIpEndPoint.Port.ToString();
            String name = "Node "+nbConnectedNode;

            // TODO
            // Création d'un node 
            // Ajout du node à la liste
            // Envoi du port pour MAJ nodeID


            Client nodeConnected = new Client(ipAddress,port,name);
            nodeConnected.NodeID = createNodeId(ipAddress, port, name);
            nodeConnected.isAvailable = true;
            nodeConnected.ClientSocket = listener;
            nodesConnected.Add(nodeConnected);

            
            //Création d'un nouveau DataInput afin de le renvoyer dès que le serveur à reçu l'information
            DataInput dataI = new DataInput()
              {
                  TaskId = 1,
                  SubTaskId = 2,
                  Method = "IdentNode",
                  Data = nodeConnected.NodeID,
                  NodeGUID = "192.168.31.26"
              };

            Receive(listener);
            Send(listener, dataI);
            SLog("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                //On ajoute le buffer récupéré à la liste
                state.data.Add(state.buffer);

                DataInput input = null;

                try
                {
                    //On désérialise la data
                    byte[] data = state.data
                                     .SelectMany(a => a)
                                     .ToArray();
                     input = Format.Deserialize<DataInput>(data);

                    //TODO : remove
                     Console.WriteLine(input.Method);

                    Receive(client);

                    //On récupère la méthod liste pour remplir la combobox du serveur
                    if(input.Method == "MethodLIST")
                    {
                        //A mettre dans la combobox
                        List<String> methodReceive = (List<string>)input.Data;
                        foreach(String method in methodReceive)
                        {
                            Console.WriteLine(method);
                        }
                    }
                }
                catch (Exception e)
                {
                    // TODO : Nécessaire ?
                    state.data.Add(state.buffer);
                    //Le ReceiveCallback est rappelé si rien n'a été récupéré plus tôt
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }


        /// <summary>
        /// Fonction Async permettant d'envoyer des données aux Nodes connectés
        /// </summary>
        /// <param name="ar"></param>
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
            FileSplitter fileSplitted = new FileSplitter();
            //String fileTosend = fileSplitted.FileReader("C:/Users/loika/Desktop/projet-NDA/Project-NDA/Genomes/genome_kennethreitz.txt");
            // TODO : remplacer par le choix fait dans la combobox du Serveur
            String fileTosend = fileSplitted.FileReader("E:/Dev/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");
            string[] file = File.ReadAllLines("E:/Dev/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");
            //ChunkSplit chunkToUse = new ChunkSplit();

            int FileLength = file.Length;

            foreach (Client clientSocket in nodesConnected)
            {
                if(FileLength == fileState)
                {
                    break;
                }

                Tuple<int, string[]> chunkToUse = (Tuple<int,string[]>)map("Method1", file, 10, fileState);
                

                fileState = chunkToUse.Item1;

                //Console.WriteLine(chunkToUse.Item1);
                //Création d'un nouveau DataInput à envoyer aux Nodes
                DataInput dataI = new DataInput()
                {
                    TaskId = 1,
                    SubTaskId = 2,
                    Method = method,
                    Data = chunkToUse.Item2,
                    NodeGUID = "192.168.31.26"
                };
                //voir pour mettre à jour la liste automatiquement

                if (clientSocket.isAvailable)
                {
                    Send(clientSocket.ClientSocket, dataI);
                    clientSocket.isAvailable = false;
                }
                
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

        public String createNodeId(string adress, string port, string name)
        {
            return adress + ":" + port + ":" + name + ":";
        }

        public virtual Object map(string MethodMap, string[] text, int chunkSize, int offsets)
        {
            Console.WriteLine("OK SERVEUR BRO");
            return null;
        }

        public object reduce()
        {

            throw new NotImplementedException();
        }
    }
}
