using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
