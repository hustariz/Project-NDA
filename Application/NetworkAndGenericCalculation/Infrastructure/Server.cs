using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.FileTreatment;
using NetworkAndGenericCalculation.MapReduce;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public List<Node> nodesConnected { get; set; }
        //private static List<Client> clientSockets { get; set; }
        private static List<Node> nodeConnected { get; set; }
        //private static List<T> clientConnected { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] buffer { get; set; }
        private Action<string> ServLogger { get; set; }
        private Action<int, string, string, float, float> GridUpdater { get; set; }
        public int LocalPort { get; set; }
        public IPAddress LocalAddress { get; set; }
        private static String response = String.Empty;
        private static List<String> ipListe { get; set; }
        //private List<Tuple<List<int>, Node>> Nodes;
        private Node localnode { get; set; }
        private int nbConnectedNode { get; set; }
        private int fileState { get; set; }
        public List<Tuple<List<Tuple<string,int>>,string,int,bool>> taskList { get; set; }
        public ConcurrentDictionary<int, Tuple<bool, Dictionary<string, int>>> dicoFinal { get; set; }
        public Stopwatch stopWatch = new Stopwatch();
        private string nodeState { get; set; }
        private bool clientConnected { get; set; }
        private float nodeProcessorUsage;
        private float nodeMemoryUsage;
        private string nodeName;

        public int subTaskCount { get; set; }

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        /// <summary>
        /// Initialisation du serveur
        /// </summary>
        /// <param name="host"></param>
        /// <param name="portNumber"></param>
        /// <param name="servLogger"></param>
        /// <param name="gridupdater"></param>
        public Server(IPAddress host, int portNumber, Action<string> servLogger, Action<int, string,string, float, float> gridupdater)
        {
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSockets = new List<Socket>();
            nodesConnected = new List<Node>();
            LocalPort = portNumber;
            LocalAddress = host;
            BUFFER_SIZE = 4096;
            buffer = new byte[BUFFER_SIZE];
            ServLogger = servLogger;
            GridUpdater = gridupdater;
            nodeConnected = new List<Node>();
            //tasksInProcess = new List<Tuple<string, int, string, int>>();
            taskList = new List<Tuple<List<Tuple<string, int>>, string, int, bool>>();
            dicoFinal = new ConcurrentDictionary<int, Tuple<bool, Dictionary<string, int>>>();

        }

        /// <summary>
        /// Démarrage de l'écoute
        /// </summary>
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

        //public void ConnectNode(int threadCount, String IP)
        //{
        //    SLog("Setting up local node...");
        //    localnode = new Node(threadCount, IP);
        //    SLog("Connected : " + localnode.ToString());
        //    nodeConnected.Add(localnode);

        //}

        /// <summary>
        /// Met à jour la grille depuis le système de log
        /// </summary>
        /// <param name="ClientsConnected"></param>
        public void updateNodeGridData(List<Node> ClientsConnected)
        {
            if (clientConnected)
            {
                nodeState = "Active";

                for (int i = 0; i < ClientsConnected.Count; i++) {

                    if (ClientsConnected[i].isAvailable) nodeState = "Inactive";
                    nodeName = "Node : " + i + " [" + (ClientsConnected[i].nodeAdress + ":" + ClientsConnected[i].NodePort + "]");
                    nodeProcessorUsage = ClientsConnected[i].ProcessorUsage;
                    nodeMemoryUsage = ClientsConnected[i].MemoryUsage;
                    //i is the ID of the node, necessary for the AppendGrdStatus method in the IHM to update the right row of the node.             
                    NLog(i, nodeName, nodeState, nodeProcessorUsage, nodeMemoryUsage);

                }
              
            }

        }

        /// <summary>
        /// Accepte la connexion des nodes
        /// </summary>
        /// <param name="ar"></param>
        public void AcceptCallback(IAsyncResult ar)
        {
            Socket listener = (Socket)ar.AsyncState;
            try
            {
                listener = serverSocket.EndAccept(ar);
                
            }
            catch (ObjectDisposedException)
            {
                return;
            }
            
            IPEndPoint remoteIpEndPoint = listener.RemoteEndPoint as IPEndPoint;
            String ipAddress = remoteIpEndPoint.Address.ToString();
            String port = remoteIpEndPoint.Port.ToString();
            String name = "Node "+nbConnectedNode++;


            Node nodeConnected = new Node(ipAddress,port,name);
            nodeConnected.NodeID = createNodeId(LocalAddress.ToString(), LocalPort.ToString(), name);
            nodeConnected.isAvailable = true;
            nodeConnected.ClientSocket = listener;
            nodesConnected.Add(nodeConnected);
            clientConnected = true;

            //call the method the first time
            updateNodeGridData(nodesConnected);

            Receive(listener);
            //Send(listener, dataI);
            SLog("Client connected, waiting for request...");
            //In case another client wants to connect
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        /// <summary>
        /// Reçoit les informations de la part des nodes
        /// </summary>
        /// <param name="ar"></param>
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
                int bytesRead = client.EndReceive(ar);
                try
                {
                    // Read data from the remote device.
                    // Gety data from buffer
                    byte[] dataToConcat = new byte[bytesRead];
                    Array.Copy(state.buffer, 0, dataToConcat, 0, bytesRead);
                    state.data.Add(dataToConcat);
                    if (MessageFlag(state.buffer, bytesRead))
                    {
                        byte[] data = ConcatByteArray(state.data);
                        Chunk.Chunk input = Format.Deserialize<Chunk.Chunk>(data);
                        Receive(client);
                        ProcessInput(input);
                    }
                    else
                    {
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception : "+e);
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //Transite data for the IHM throught a log system to the nodedatagrid
        internal void NLog(int nodeID, string nodeAdress, string nodeState, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            GridUpdater?.Invoke(nodeID, nodeAdress, nodeState, nodeProcessorUsage, nodeMemoryUsage);
        }


        //Transite data for the IHM throught a log system to the server status box
        public void SLog(string msg)
        {
            ServLogger?.Invoke(msg);
        }

        /// <summary>
        /// Execution du split et de l'envoi des fichiers
        /// </summary>
        public void SplitAndSend(string methodName, string fileLocation)
        {
            stopWatch.Start();
            SLog("Début du traitement");

            String method = methodName;
            subTaskCount = 0;
            fileState = 0;
            Console.WriteLine("REINIT FILESTATE : " + fileState);


            FileSplitter fileSplitted = new FileSplitter();

            //string[] file = File.ReadAllLines("E:/Dev/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");
            //string[] file = File.ReadAllLines("D:/ProjectC#/ProjectC#/Project-NDA/Genomes/genome_kennethreitz.txt");
            string[] file = File.ReadAllLines(fileLocation);
            int FileLength = file.Length;
            //int nbOfLine = FileLength / nodesConnected.Count;

            int nbOfLine = Math.DivRem(FileLength, nodesConnected.Count, out int remainder);


            Tuple<int, string[]> chunkToUse = null;
            bool isSuccess = true;


            while (FileLength != fileState)
            {             
                foreach (Node clientSocket in nodesConnected)
                {


                    if (isSuccess)
                    {
                        subTaskCount++;
                        chunkToUse = (Tuple<int, string[]>)map("Method1", file, nbOfLine+ remainder, fileState);
                        remainder = 0;
                        fileState = chunkToUse.Item1;
                        Dictionary<string, int> ProccessDico = new Dictionary<string, int>();
                        Tuple<bool, Dictionary<string, int>> ProcessTuple = new Tuple<bool, Dictionary<string, int>>(false, ProccessDico);
                        dicoFinal.TryAdd(subTaskCount, ProcessTuple);
                    }

                    //Création d'un nouveau DataInput à envoyer aux Nodes
                    Chunk.Chunk dataI = new Chunk.Chunk()
                    {
                        TaskId = 1,
                        SubTaskId = subTaskCount,
                        Method = method,
                        Data = chunkToUse.Item2,
                        NodeGUID = clientSocket.NodeID
                    };
                    //voir pour mettre à jour la liste automatiquement
                    if (clientSocket.isAvailable)
                    {
                        Send(clientSocket.ClientSocket, dataI);
                        clientSocket.isAvailable = false;
                        isSuccess = true;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        isSuccess = false;
                    }
                }
            }
        }

        /// <summary>
        /// Envoi en Asynchrone les données
        /// </summary>
        /// <param name="client"></param>
        /// <param name="obj"></param>
        private static void Send(Socket client, Chunk.Chunk obj)
        {
            //Sérialise les données à envoyer
            byte[] data = Format.Serialize(obj);
            try
            {
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

        /// <summary>
        /// Créer l'identifiant du Node
        /// </summary>
        /// <param name="adress"></param>
        /// <param name="port"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public String createNodeId(string adress, string port, string name)
        {
            return adress + ":" + port + ":" + name + ":";
        }

        /// <summary>
        /// Mapping Générique
        /// </summary>
        /// <param name="MethodMap"></param>
        /// <param name="text"></param>
        /// <param name="chunkSize"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public virtual Object map(string MethodMap, string[] text, int chunkSize, int offsets)
        {
            return null;
        }

        /// <summary>
        /// Réduce générique
        /// </summary>
        /// <returns></returns>
        public object reduce()
        {
            throw new NotImplementedException();
        }

        public virtual List<Tuple<char, int>> ReduceMethod1(List<Tuple<char, int>> listGlobale, List<Tuple<char, int>> listMapped)
        {
            return null;
        }

        public virtual void ProcessInput(Chunk.Chunk input)
        {
            if (input.Method == "MethodLIST")
            {
                //A mettre dans la combobox
                List<String> methodReceive = (List<string>)input.Data;
            }
        }

        /// <summary>
        /// Défini si il s'agit de la fin d'un message et y ajoute une fin de séquence
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="byteRead"></param>
        /// <returns></returns>
        private bool MessageFlag(byte[] buffer, int byteRead)
        {
            byte[] endSequence = Encoding.ASCII.GetBytes("GAMEOVER");
            byte[] endOfBuffer = new byte[8];
            Array.Copy(buffer, byteRead - endSequence.Length, endOfBuffer, 0, endSequence.Length);
            return endSequence.SequenceEqual(endOfBuffer);
        }

        private byte[] ConcatByteArray(List<byte[]> data)
        {
            List<byte> byteStorage = new List<byte>();
            foreach (byte[] bytes in data)
            {
                foreach (byte bit in bytes)
                {
                    byteStorage.Add(bit);
                }
            }
            return byteStorage.ToArray();
        }
    }
            
}
