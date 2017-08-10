using NetworkAndGenericCalculation.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Sockets
{

    public class StateObject
    {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 2048;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class Client
    {
        private static Socket ClientSocket { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] Buffer { get; set; }
        private int Attempts { get; set; }
        private Action<string> Logger { get; set; }
        private int ServPort { get; set; }
        private IPAddress ServAddress { get; set; }
        private Node nodeClient { get; set; }
        private static String response = String.Empty;
        public static List<BackgroundWorker> backGroundworkerList { get; set; }


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
            BUFFER_SIZE = 2048;
            Buffer = new byte[BUFFER_SIZE];
            Attempts = 0;
        }

        //Open a Socket Connection with a server
        public void ConnectToServer(IPAddress host, int port)
        {
            nodeClient = new Node(4, host.ToString());
            ServAddress = host;
            ServPort = port;
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
            Log(nodeClient.NetworkAdress);
            Log("Connected");
            Receive(ClientSocket);

           
           // receiveDone.WaitOne();
        }

        private static void Receive(Socket client)
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



        public static void AcceptCallback(IAsyncResult ar)
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


        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                //Socket client = (Socket)ar.AsyncState;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);
            

                int finalresult = 0;


                backGroundworkerList = new List<BackgroundWorker>();

                

                for(int i = 0; i < 4; i++)
                {
                    //string track = StringFormat("bw{0}", i)

                    
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
                        finalresult = calculTest(2, 4);

                    };

                    bc.RunWorkerCompleted += (o, a) =>
                    {
                        Console.WriteLine(finalresult);
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
                    Send(ClientSocket, data);
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

                if (bytesRead > 0)
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
            }
        }

        // Receive and convert data into a string to print it
        public void ReceiveResponse()
        {

            Console.WriteLine("PAR LA");
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

        public static int calculTest(int nb1, int nb2) 
        {
            Console.WriteLine("PAR LA" + nb1 + nb2);
            int nb3 = nb1 + nb2;
            return nb3;
        }

        public static void monitoringBW()
        {

            foreach(BackgroundWorker bc in backGroundworkerList)
            {
                while (bc.IsBusy)
                {
                    //Console.WriteLine("workers are busy");
                    Thread.Sleep(1000);
                    monitoringBW();
                }
            }
        }

        private static void Send(Socket handler, byte[] chunkToUse)
        {

            // Begin sending the data to the remote device.
            handler.BeginSend(chunkToUse, 0, chunkToUse.Length, 0,
                new AsyncCallback(SendCallback), handler);
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                //handler.Shutdown(SocketShutdown.Both);
                //handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

    }
}
