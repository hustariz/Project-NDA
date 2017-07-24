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
using MainForm;
using System.Threading;
using System.Diagnostics;
using c_projet_adn.Worker;
using System.IO;
using NetworkAndGenericCalculation.Worker;
using NetworkAndGenericCalculation.Node;
using NetworkAndGenericCalculation.FileTreatment;

namespace MainForm
{
    public partial class MainForm : Form
    {
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

            grd_node_data.Columns.Add("nodeAddress&Name", "Node");
            grd_node_data.Columns.Add("nodeWorkersNumber", "Worker");
            grd_node_data.Columns.Add("nodeCpuUsage", "CPU");
            grd_node_data.Columns.Add("nodeMemoryUsage", "Memory");


            grd_node_data.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            // Adjust Size of the cells to fill the grid spaces
            grd_node_data.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;



        }



        // Append the Client Status Textbox with the argument
        public void AppendClientStatus(params object[] message)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new ThreadStart(delegate { AppendClientStatus(message); }));
                else
                    txt_status_client.AppendText(string.Join(" ", message) + Environment.NewLine);
            }
            catch
            {
                return;
            }
        }

        // Append the Server Status Textbox with the argument
        public void AppendSrvStatus(params object[] message)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(new ThreadStart(delegate { AppendSrvStatus(message); }));
                else
                    txt_status_srv.AppendText(string.Join(" ", message) + Environment.NewLine);
            }
            catch
            {
                return;
            }
        }


        private void sendFileButton_Click(object sender, EventArgs e)
        {

        }



        private void label3_Click(object sender, EventArgs e)
        {

        }

        public void RunMapReduce(String[] data, int i)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            MapReduce<int, string, string, int, string, int> job = new MapReduce<int, string, string, int, string, int>(Map, Reduce);
            var items = data;
            sw.Stop();
            Console.WriteLine("Process file time : " + sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            var pairs = items.Select((item, k) => new KVPair<int, string>() { Key = k, Value = item });
            sw.Stop();
            Console.WriteLine("Select time : " + sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            job.Map(pairs);
            sw.Stop();
            Console.WriteLine("Map time : " + sw.ElapsedMilliseconds);
            sw.Reset();
            sw.Start();
            var result = job.Reduce();
            sw.Stop();
            Console.WriteLine("Reduce time : " + sw.ElapsedMilliseconds);
            Console.WriteLine("end");
        }

        private static void Map(int key, string value, Map<string, int> context)
        {
            context.AddPair(value, 1);
        }

        private static void Reduce(string key, IList<int> values, Reduce<string, int> context)
        {
            int total = values.Sum();
            context.AddPair(key, total);
        }

        private string[] DnaProcess(string sourceFile,int nbBytes)
        {

            byte[] toBytes = Encoding.ASCII.GetBytes(sourceFile);
            string[] pairs;
            List<string> pairsList = new List<string>();
            List<char[]> charList = new List<char[]>();
            foreach (string line in File.ReadAllLines(sourceFile))
            {
                if (!line.StartsWith("#"))
                {
                    foreach (char c in line.Split('\t')[3].ToCharArray())
                    {
                        pairsList.Add(c.ToString());
                    }
                }
            }

            pairs = pairsList.ToArray<string>();
            return pairs;
        }

        private void file_Click(object sender, EventArgs e)
        {
            Node nd1 = new Node(1, "22");
            WThread ws = new WThread(nd1, 1);

            //String[] data = DnaProcess("C:/Users/loika/Desktop/projet-NDA/Project-NDA/Genomes/genome_greshake.txt",10000);

            FileSplitter fs = new FileSplitter();

            String readedFile = fs.FileReader("C:/Users/loika/Desktop/projet-NDA/Project-NDA/Genomes/genome_greshake.txt");

            List<String> moncul = fs.Moncul(readedFile, 10);

            
            //string coucou = "hello";
            //int coucou2 = 3;

            //ws.ExecuteFunction<String[], int>(data,2, RunMapReduce);
            //int coucou3 = (int)ws.ExecuteTask<string, int> (coucou, coucou2);

           
        }

        public void fonctionTest(string s1,int s2)
        {
            MessageBox.Show(s1);
        }
    }
}
