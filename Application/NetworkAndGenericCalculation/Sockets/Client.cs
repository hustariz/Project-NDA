using NetworkAndGenericCalculation.Nodes;
using System;
using System.Collections.Generic;
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
        public const int BufferSize = 256;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    public class Client
    {
        Socket ClientSocket { get; set; }
        private int BUFFER_SIZE { get; set; }
        private static byte[] Buffer { get; set; }
        private int Attempts { get; set; }
        private Action<string> Logger { get; set; }
        private int ServPort { get; set; }
        private IPAddress ServAddress { get; set; }
        private Node nodeClient { get; set; }
        private static String response = String.Empty;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

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

        private static void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                Console.WriteLine("PAR LA");
                // Retrieve the state object and the client socket 
                // from the asynchronous state object.
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                

                if (bytesRead > 0)
                {
                    // There might be more data, so store the data received so far.
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

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
                    receiveDone.Set();
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
    }
}
