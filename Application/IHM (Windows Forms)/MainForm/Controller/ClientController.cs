
using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MainForms
{ 

    public class ClientController
    {
        private MainForm view;
        private Client clientSocket;



        public ClientController(MainForm view, Client clientSocket)
        {
            this.view = view;
            this.clientSocket = clientSocket;
            view.SetClientController(this);

        }

        public void ConnectToServer(IPAddress host, int port)
        {
            clientSocket.ConnectToServer(host, port);
        }

        internal void Exit()
        {
            clientSocket.Exit();
        }

        internal void SendRequest(String request)
        {
            clientSocket.SendRequest(request);
        }

        internal void ReceiveResponse()
        {
            clientSocket.ReceiveResponse();
        }
    }
}
