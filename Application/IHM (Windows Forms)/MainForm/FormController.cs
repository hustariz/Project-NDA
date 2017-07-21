using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MainForms
{
    public class FormController
    {
        private MainForm view;
        private Server serverSocket;
        private Client clientSocket;

        

        public FormController(MainForm view, Server serverSocket, Client clientSocket)
        {
            this.view = view;
            this.serverSocket = serverSocket;
            this.clientSocket = clientSocket;
            view.SetController(this);

        }

        public void SetupServer(IPAddress host, int port)
        {
            AppendSrvLog("Setting up server...");
            serverSocket.SetupServer(host, port);
            AppendSrvLog("Server setup complete");
        }

        public void AppendSrvLog(params object[] message)
        {
            view.AppendSrvStatus(message);
        }

        public void AppendClientLog(params object[] message)
        {
            view.AppendClientStatus(message);
        }
    }
}
