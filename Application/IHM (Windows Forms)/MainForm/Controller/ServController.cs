using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetworkAndGenericCalculation.Sockets;

namespace MainForms
{
    public class ServController
    {
        private MainForm view;
        private Server serverSocket;
        //private string nodeName, nodeAdress;
        //private bool isAvailable;
        //private float processorUsage, memoryUsage;
        private List<Node> ClientsConnected;




        public ServController(MainForm view, Server serverSocket)
        {
            this.view = view;
            this.serverSocket = serverSocket;
            view.SetServController(this);

        }

        public void SetupServer()
        {
            serverSocket.SetupServer();
        }

        //public void ConnectNode(int threadCount, String IP)
        //{
        //    serverSocket.ConnectNode(threadCount, IP);
        //}

        public void updateNodeGridData()
        {
                //nodeName = serverSocket.ClientsConnected[i].NodeID;
                //nodeAdress = serverSocket.ClientsConnected[i].nodeAdress;
                //isAvailable = serverSocket.ClientsConnected[i].isAvailable;
                //processorUsage = serverSocket.ClientsConnected[i].ProcessorUsage;
                //memoryUsage = serverSocket.ClientsConnected[i].MemoryUsage;
                ClientsConnected = serverSocket.nodesConnected;
                serverSocket.updateNodeGridData(ClientsConnected);                  
        }

        public int getNodeCount()
        {
            int count = 0;
            for (int i = 0; i < serverSocket.nodesConnected.Count; i++)
            {
                count += 1;
            }

            return count;
        }

        public void sendFile()
        {
            
        }

    }
}
