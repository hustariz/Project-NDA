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


    }
}
