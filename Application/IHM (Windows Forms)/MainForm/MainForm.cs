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
        private FormController formController;
        private Server server;
        private Client client;



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
            server = new Server();
            client = new Client();

            formController = new FormController(this, server, client);


            grd_node_data.Columns.Add("nodeAddress&Name", "Node");
            grd_node_data.Columns.Add("nodeWorkersNumber", "Worker(s)");
            grd_node_data.Columns.Add("nodeCpuUsage", "CPU");
            grd_node_data.Columns.Add("nodeMemoryUsage", "Memory");
  
            // Adjust Size of the cells to fill the grid spaces
            grd_node_data.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;       
        }

        public void SetController(FormController controller)
        {
            formController = controller;
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
    }
}
