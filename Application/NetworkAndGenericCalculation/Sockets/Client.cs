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


    public class Client : IMapper, IReducer
    {
        public Socket ClientSocket { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] Buffer { get; set; }
        private int Attempts { get; set; }
        private Action<string> Logger { get; set; }
        private string NodePort { get; set; }
        private string nodeAdress { get; set; }
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
        private string nodeName { get; set; }
        

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

        public Client(Action<string> logger)
        {
           
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Logger = logger;
            //BUFFER_SIZE = 4096;
            //Buffer = new byte[BUFFER_SIZE];
            //Attempts = 0;
            processorCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            //Ajouter le NodeId
            
        }

        public Client(String adress, String port, String name)
        {
            nodeAdress = adress;
            NodePort = port;
            nodeName = name;

        }


        //Open a Socket Connection with a server
        public void ConnectToServer(IPAddress host, int port)
        {
            //nodeClient = new Node(4, host.ToString());
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
            //Log(nodeClient.NetworkAdress);
            Log("Connected");
            //this.isAvailable = true;

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
            
           
           // receiveDone.WaitOne();
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

        // Close socket and write a message in the status box.
        public void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            Log("Client disconnected");
        }

        // Send a Request to the server
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

        // Sends a string to the server with ASCII encoding.
        public void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }



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

                Console.WriteLine("bt :" + bytesRead);

                //Console.WriteLine(lala);
                /*if(bytesRead < StateObject.BufferSize)
                {
                    Console.WriteLine("PASSAGE");
                    byte[] msg = new byte[bytesRead];
                    Array.Copy(state.buffer, msg, bytesRead);
                    state.data.Add(msg); 
                }else
                {
                    state.data.Add(state.buffer);
                }*/

                 //input = null;
                   /* try
                    {
                        byte[] data = state.data
                                         .SelectMany(a => a)
                                         .ToArray();
                    DataInput input = Format.Deserialize<DataInput>(data);

                    // Receive après inputmabite
                    Console.WriteLine("DATA RECEIVED MON CUL : " + input.Data);
                    ProcessInput(input);


                    /*
                    DataInput dataI = new DataInput()
                    {
                        TaskId = 1,
                        SubTaskId = 2,
                        Method = "loulou",
                        Data = "TA GUEULE",
                        NodeGUID = "192.168.31.26"
                    };*/

                    //Receive(client);

                    try
                    {
                        // Read data from the remote device.
                        //int nbByteRead = node.NodeSocket.EndReceive(ar);
                        // Gety data from buffer
                        byte[] dataToConcat = new byte[bytesRead];
                        Array.Copy(state.buffer, 0, dataToConcat, 0, bytesRead);
                        state.data.Add(dataToConcat);
                    if (IsEndOfMessage(state.buffer, bytesRead))
                    {
                        byte[] data = ConcatByteArray(state.data);
                        DataInput input = Format.Deserialize<DataInput>(data);
                        Receive(client);
                        Console.WriteLine("DATA INPUT : " + input.Data);
                        ProcessInput(input);
                    }
                    else
                    {
                        client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
                    }


                    }
                    catch(Exception e)
                    {
                    Console.WriteLine("erreur sérialisation ");
                    //Console.WriteLine("Error : " + e);
                    //Console.WriteLine(e.ToString());
                    //state.data.Add(state.buffer);
                    //Receive(client);
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReceiveCallback), state);
                }

                //Console.WriteLine("LLLLLLLLLLLLAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
                //Console.WriteLine(input.Method);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            
            //try
            //{
            /*// Retrieve the state object and the client socket 
            // from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.workSocket;
            //processInput(state);
            //Socket client = (Socket)ar.AsyncState;

            // Read data from the remote device.
            int bytesRead = client.EndReceive(ar);

            //processInput();

            int finalresult = 0;

            byte[] coucou = new byte[bytesRead];

            ProcessInput(coucou);

            backGroundworkerList = new List<BackgroundWorker>();

            for(int i = 0; i < 4; i++)
            {

                BackgroundWorker bw2 = new BackgroundWorker()
                {
                    WorkerSupportsCancellation = true,
                    WorkerReportsProgress = true
                };
                backGroundworkerList.Add(bw2);

            }


            foreach(BackgroundWorker bc in backGroundworkerList)
            {
                bc.DoWork += (o, a) =>
                {
                    //Console.WriteLine("MABITE");
                    Thread.Sleep(1000);
                    //finalresult = calculTest(2, 4);
                    a.Result = calculTest(2, 4);
                    //Console.WriteLine();
                };

                bc.RunWorkerCompleted += (o, a) =>
                {
                    int moncul =  (int)a.Result;
                    Console.WriteLine(moncul);
                };

                bc.RunWorkerAsync();
            }

            BackgroundWorker bcChecker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            bcChecker.DoWork += (o, a) =>
            {
                //Console.WriteLine("MABITE");
                monitoringBW();

            };

            bcChecker.RunWorkerCompleted += (o, a) =>
            {
                var data = new byte[bytesRead];
                
                Console.WriteLine("Tout le monde a fini");
            };


            bcChecker.RunWorkerAsync();

            /*
            BackgroundWorker bw = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true,
                WorkerReportsProgress = true
            };

            bw.DoWork += (o, a) =>
            {
                Console.WriteLine("MABITE");
                finalresult = calculTest(2, 4);
                //finalresult = (int)a.Result;
                //Console.WriteLine(finalresult);
            };


            bw.RunWorkerCompleted += (o, a) =>
            {

                Console.WriteLine(finalresult);
            };

            //bw.RunWorkerCompleted += worker_RunWorkerCompleted;

            bw.RunWorkerAsync();

            */

            /*if (bytesRead > 0)
             {
                 // There might be more data, so store the data received so far.
                 state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                 Console.WriteLine(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                 // Get the rest of the data.
                 client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                     new AsyncCallback(ReceiveCallback), state);
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
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }*/
        }

        public virtual Object ProcessInput(DataInput dataI) {

            switch (dataI.Method)
            {
                case "IdentNode":
                    NodeID = (String)dataI.Data;
                    Console.WriteLine(NodeID);
                    break;
               
                
            }

            return null;

        }

        public List<String> nodeMethods() {

            List<string> listest = new List<string>();
            listest.Add("COUCOU");

            return listest;
        }

        //public abstract object ProcessInput(DataInput dataI);

        // Receive and convert data into a string to print it
        public void ReceiveResponse()
        {
            int received = ClientSocket.Receive(Buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(Buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            Log("Server Response : " + text);
        }

        internal void Log(string msg)
        {
            Logger?.Invoke(msg);
        }

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

        public static void Send(Socket handler, DataInput obj)
        {

            byte[] data = Format.Serialize(obj);

            try
            {
                //Console.WriteLine("Send data : " + obj + " to : " + handler);
                handler.BeginSend(data, 0, data.Length, 0,
                    new AsyncCallback(SendCallback), handler);
            }
            catch (SocketException ex)
            {
                /// Client Down ///
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


        public float ProcessorUsage => processorCounter.NextValue();
        public float MemoryUsage => memoryCounter.NextValue();

        /*
        public int Length => throw new NotImplementedException();

        public int ChunkDefaultLength => throw new NotImplementedException();

        public int ChunkCount => throw new NotImplementedException();

        public bool IsActive => throw new NotImplementedException();

        public int ChunkRemainsLength => throw new NotImplementedException();
        */
        /// <summary>
        /// Fonction générant l'ID d'un Node
        /// </summary>
        protected void genGUID()
        {
            NodeID = "NODE" + ":" + serveurAdress + ":" + serverNodePort;
        }

        public virtual object map(string Method, string[] text, int chunkSize, int offsets)
        {
            return null;
        }

        public virtual object reduce()
        {
            throw new NotImplementedException();
        }

        private bool IsEndOfMessage(byte[] buffer, int byteRead)
        {
            byte[] endSequence = Encoding.ASCII.GetBytes("PIPICACA");
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
