using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.FileTreatment;
using NetworkAndGenericCalculation.MapReduce;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace NetworkAndGenericCalculation.Sockets
{

    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 4096;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        public List<byte[]> data = new List<byte[]>();
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }


    public class Node : IMapper, IReducer
    {
        public Socket ClientSocket { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] Buffer { get; set; }
        private int Attempts { get; set; }
        private Action<string> Logger { get; set; }
        public string NodePort { get; set; }
        public string nodeAdress { get; set; }
        private IPAddress serveurAdress { get; set; }
        private int serverNodePort { get; set; }
        //private Node nodeClient { get; set; }
        private static String response = String.Empty;
        public static List<BackgroundWorker> backGroundworkerList { get; set; }
        private static List<Byte[]> toto { get; set; }
        public int i = 0;
        private PerformanceCounter processorCounter;
        private PerformanceCounter memoryCounter;
        public bool isAvailable { get; set; }
        public String NodeID;
        public string nodeName { get; set; }
        

        // Tout doux
        //public listMethod()


        //public static StateObject state { get; set; }

        private int nbBGW;
        public int NbBGW
        {
            [MethodImpl(MethodImplOptions.Synchronized)]
            get { return nbBGW; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set { nbBGW = value; }
        }


        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        /// <summary>
        /// Initialisation du Node
        /// </summary>
        /// <param name="logger"></param>
        public Node(Action<string> logger)
        {         
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Logger = logger;
            processorCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");            
        }

        /// <summary>
        /// Création d'un Node à la volée
        /// </summary>
        /// <param name="adress"></param>
        /// <param name="port"></param>
        /// <param name="name"></param>
        public Node(String adress, String port, String name)
        {   
            nodeAdress = adress;
            Console.WriteLine("From node constructor : " + nodeAdress);
            NodePort = port;
            nodeName = name;
            processorCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        }

        /// <summary>
        /// Ouvre la connexion avec le serveur
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void ConnectToServer(IPAddress host, int port)
        {
            serveurAdress = host;
            serverNodePort = port;
            while (!ClientSocket.Connected)
            {
                try
                {
                    Attempts++;
                    Log("Connection attempt :" + Attempts);
                    ClientSocket.Connect(host, port);                  
                }
                catch (SocketException)
                {
                    Log("SocketException");
                }
            }
            Log("Connected");

            //Send methods to serveur
            List<string> methodList = nodeMethods();
            DataInput dataI = new DataInput()
            {
                Method = "MethodLIST",
                Data = methodList,
                NodeGUID = "Récupérer le nodeGUID"
            };
            Send(ClientSocket, dataI);
            Receive(ClientSocket);
        }

        /// <summary>
        /// Initialise le ReceiveCallback en Asynchrone
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

        /// <summary>
        /// Ferme la connexion
        /// </summary>
        public void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Log("Client disconnected");
        }

        /// <summary>
        /// Envoi une requête au serveur
        /// </summary>
        /// <param name="message"></param>
        public void SendRequest(String message)
        {
            string request = message;
            SendString(request);
            Log("request sent : " + request);
            if (request.ToLower() == "exit")
            {
                Exit();
            }
        }

        /// <summary>
        /// Envoi un string au serveur
        /// </summary>
        /// <param name="text"></param>
        public void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        /// <summary>
        /// Accepte la communication en Asynchrone
        /// </summary>
        /// <param name="ar"></param>
        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// Reçoit la communication en Asynchrone
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

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);       

                try
                {
                    // Read data from the remote device.
                    // Gety data from buffer
                    byte[] dataToConcat = new byte[bytesRead];
                    Array.Copy(state.buffer, 0, dataToConcat, 0, bytesRead);
                    state.data.Add(dataToConcat);
                    if (IsEndOfMessage(state.buffer, bytesRead))
                    {
                        byte[] data = ConcatByteArray(state.data);
                        DataInput input = Format.Deserialize<DataInput>(data);
                        Receive(client);
                        ProcessInput(input);
                    }
                    else
                    {
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("erreur sérialisation "+e);
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
        /// Orchestre les méthodes reçues
        /// </summary>
        /// <param name="dataI"></param>
        /// <returns></returns>
        public virtual Object ProcessInput(DataInput dataI) {

            switch (dataI.Method)
            {
                case "IdentNode":
                    NodeID = (String)dataI.Data;
                    break;          
            }
            return null;
        }

        /// <summary>
        /// TODO : Récupère la liste des méthodes
        /// </summary>
        /// <returns></returns>
        public List<String> nodeMethods() {
            List<string> listest = new List<string>();
            listest.Add("COUCOU");
            return listest;
        }

        // Receive and convert data into a string to print it
        /// <summary>
        /// Reçoit la réponse et converti les données en string pour les afficher
        /// </summary>
        public void ReceiveResponse()
        {
            int received = ClientSocket.Receive(Buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(Buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Log("Server Response : " + text);
        }

        /// <summary>
        /// Log
        /// </summary>
        /// <param name="msg"></param>
        internal void Log(string msg)
        {
            Logger?.Invoke(msg);
        }

        /// <summary>
        /// Monitore les BackgroundWorkers
        /// </summary>
        public static void monitoringBW()
        {
            foreach(BackgroundWorker bc in backGroundworkerList)
            {
                while (bc.IsBusy)
                {
                    Thread.Sleep(1000);
                    monitoringBW();
                }
            }
        }

        /// <summary>
        /// Envoi des données sérialisées en Asynchrone
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="obj"></param>
        public static void Send(Socket handler, DataInput obj)
        {
            byte[] data = Format.Serialize(obj);
            try
            {
                handler.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (SocketException ex)
            {
                if (!handler.Connected)
                    Console.WriteLine("Client " + handler.RemoteEndPoint.ToString() + " Disconnected");
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        /// Fonction permettant d'envoyer en asynchrone les données vers le serveur
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
               // Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        /// <summary>
        /// Usage du processeur
        /// </summary>
        public float ProcessorUsage => processorCounter.NextValue();
        /// <summary>
        /// Usage de la mémoire
        /// </summary>
        public float MemoryUsage => memoryCounter.NextValue();


        /// <summary>
        /// Fonction générant l'ID d'un Node
        /// </summary>
        protected void genGUID()
        {
            NodeID = "NODE" + ":" + serveurAdress + ":" + serverNodePort;
        }

        /// <summary>
        /// Mapping générique
        /// </summary>
        /// <param name="Method"></param>
        /// <param name="text"></param>
        /// <param name="chunkSize"></param>
        /// <param name="offsets"></param>
        /// <returns></returns>
        public virtual object map(string Method, string[] text, int chunkSize, int offsets)
        {
            return null;
        }

        /// <summary>
        /// Reduce générique
        /// </summary>
        /// <returns></returns>
        public virtual object reduce()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Défini si il s'agit de la fin d'un message et y ajoute une fin de séquence
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="byteRead"></param>
        /// <returns></returns>
        private bool IsEndOfMessage(byte[] buffer, int byteRead)
        {
            byte[] endSequence = Encoding.ASCII.GetBytes("PIPICACA");
            byte[] endOfBuffer = new byte[8];
            Array.Copy(buffer, byteRead - endSequence.Length, endOfBuffer, 0, endSequence.Length);
            return endSequence.SequenceEqual(endOfBuffer);
        }

        /// <summary>
        /// Concatène tous les buffers présents dans la liste dans un buffer final
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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
