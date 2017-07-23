using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MainForms;
using System.Threading;
using NetworkAndGenericCalculation.Sockets;

namespace MainForms
{
    public partial class MainForm : Form
    {
        private ServController servController;
        private ClientController clientController;
        private Server server;
        private Client client;
        public Action<string> Logger { get; private set; }

        public MainForm()
        {

            InitializeComponent();
            // Center window's position on the actual screen
            CenterToScreen();
            //Get your own IP
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName()); 
            foreach (IPAddress adress in localIP)
            {
                if (adress.AddressFamily == AddressFamily.InterNetwork)
                {
                    txt_host_client.Text = adress.ToString();
                    txt_host.Text = adress.ToString();
                }
            }

            server = new Server(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text), this.SLog);
            servController = new ServController(this, server);
            client = new Client(this.CLog);
            clientController = new ClientController(this, client);


            grd_node_data.Columns.Add("nodeAddress&Name", "Node");
            grd_node_data.Columns.Add("nodeWorkersNumber", "Worker(s)");
            grd_node_data.Columns.Add("nodeCpuUsage", "CPU");
            grd_node_data.Columns.Add("nodeMemoryUsage", "Memory");
  
            // Adjust Size of the cells to fill the grid spaces
            grd_node_data.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;       
        }

        public void SetServController(ServController controller)
        {
            servController = controller;
        }

        public void SetClientController(ClientController controller)
        {
            clientController = controller;
        }

        // Append the Client Status Textbox with the argument
        //Params object to be able to display object
        public void AppendClientStatus(params object[] message)
        {
            if (InvokeRequired)
                Invoke(new ThreadStart(() => { AppendClientStatus(message); }));
            else
                txt_status_client.AppendText(string.Join(" ", message) + Environment.NewLine);
        }

        // Append the Server Status Textbox with the argument
        public void AppendSrvStatus(params object[] message)
        {
            if (InvokeRequired)
                Invoke(new ThreadStart(() => { AppendSrvStatus(message); }));
            else
                txt_status_srv.AppendText(string.Join(" ", message) + Environment.NewLine);
        }

        public void SLog(string message)
        {
            AppendSrvStatus(message);
        }
        public void CLog(string message)
        {
            AppendClientStatus(message);
        }



    }
}
