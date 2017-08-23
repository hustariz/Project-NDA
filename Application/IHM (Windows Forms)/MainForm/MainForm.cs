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
using GenomicTreatment;
using System.IO;

namespace MainForms
{
    public partial class MainForm : Form
    {
        private ServController servController;
        private ClientController clientController;
        private Server server;
        private Client client;
        private String ipServer, ipClient;
        private List <String> modules;


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
                    ipClient = txt_host_client.Text;
                    txt_host.Text = adress.ToString();
                    ipServer = txt_host.Text;

                }
            }
            //Initialise with the log for the IHM to access return from server
            server = new GenomicServeur(IPAddress.Parse(ipServer), Int32.Parse(txt_port.Text), this.SLog, this.Nlog);
            servController = new ServController(this, server);
            client = new GenomicNode(this.CLog);
            //IPEndPoint remoteIpEndPoint = client.ClientSocket.RemoteEndPoint as IPEndPoint;
            //Console.WriteLine(remoteIpEndPoint.Address);
            clientController = new ClientController(this, client);

            //Initialisation of the combobox
            modules = new List<string>();
            modules.Add("1. Quantitative analysis");
            modules.Add("2. Genomic Sequence Search");
            modules.Add("3. Gene Search");

            foreach (String module in modules)
            {
                cbb_module_to_process.Items.Add(module);
            }

            //Fill the grid with the determined column's name
            grd_node_data.Columns.Add("nodeAddress&Name", "Node");
            grd_node_data.Columns.Add("nodeState", "State");
            grd_node_data.Columns.Add("nodeCpuUsage", "CPU");
            grd_node_data.Columns.Add("nodeMemoryUsage", "Memory");
            grd_node_data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Adjust Size of the cells to fill the grid spaces
            grd_node_data.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;


            int worker = 0;
            int io = 0;
            ThreadPool.GetAvailableThreads(out worker, out io);

            Console.WriteLine("Thread pool threads available at startup: ");
            Console.WriteLine("   Worker threads: {0:N0}", worker);
            Console.WriteLine("   Asynchronous I/O threads: {0:N0}", io);

            Console.WriteLine("The number of processors " +
            "on this computer is {0}.",
            Environment.ProcessorCount);

            nmr_local_thread.Maximum = Environment.ProcessorCount;

        }

        public void SetServController(ServController controller)
        {
            servController = controller;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            server.SplitAndSend("method1");

        }

        public void SetClientController(ClientController controller)
        {
            clientController = controller;
        }

    }
}
