using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
// C'est normal il est rien censé faire vers le serveur implémente le MVC depuis server.cs vers IHM les infos du coup
//using NetworkAndGenericCalculation.Nodes;
//using NetworkAndGenericCalculation.Worker;
//using NetworkAndGenericCalculation.Sockets;

namespace MainForms
{
    public partial class MainForm : Form
    {

        //if true doesn't create another row in Nlog
        int compteurNode = -1;
        bool clientConnected = false;
        // Append the Server Status Textbox with the argument
        public void AppendSrvStatus(params object[] message)
        {
            BeginInvoke((Action)(() =>
            {
                txt_status_srv.AppendText(string.Join(" ", message) + Environment.NewLine);
            }), null);
        }

        //Create Row each time a node connect to the server.
        public void CreateNodeGrdStatus(string nodeAdress, string nodeStatus, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            Invoke(new ThreadStart(() =>
            {
                grd_node_data.Rows.Add(nodeAdress, nodeStatus, nodeProcessorUsage + "%", nodeMemoryUsage + "MB");         
            }));
        }

        //Refresh row with NodeId related to the actual node's Row.
        public void AppendGrdStatus(int nodeId, string nodeAdress, string nodeStatus, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            Invoke(new ThreadStart(() =>
            {
                for (int i = 0; i< grd_node_data.RowCount; i++)
                {
                    grd_node_data.Rows[nodeId].SetValues(nodeAdress, nodeStatus, Math.Round(nodeProcessorUsage, 2) + "%", nodeMemoryUsage + "MB");
                }
            }));
        }

        //Receive data from server throught log system to refresh the server status box.
        public void SLog(string message)
        {
            AppendSrvStatus(message);
            if (message == "Client connected, waiting for request...")
            {
                clientConnected = true;
                grp_box_data_process.Enabled = true;
            }
            else if (message == "Client disconnected")
            {
                grp_box_data_process.Enabled = false;
            }
        }
        //Receive data from server throught log system to create and refresh NodeDataGrid.
        public void Nlog(int nodeID, string nodeAdress, string nodeStatus, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            Console.WriteLine(nodeAdress + nodeStatus + nodeProcessorUsage + nodeMemoryUsage);
            for (int i = 0; i < servController.getNodeCount(); i++)
            {
                //Create Row
                if (compteurNode < i && clientConnected)
                {
                    Console.WriteLine("Test : " + i);
                    CreateNodeGrdStatus(nodeAdress, nodeStatus, nodeProcessorUsage, nodeMemoryUsage);
                    compteurNode += 1;
                }
                //Refresh all Row
                AppendGrdStatus(nodeID, nodeAdress, nodeStatus, nodeProcessorUsage, nodeMemoryUsage);
            }
        }

        // Update the data grid, timer = 1s
        private void tmr_grid_data_update_Tick(object sender, EventArgs e)
        {
            if (clientConnected) servController.updateNodeGridData();
        }

        private void btn_load_genome_Click(object sender, EventArgs e)
        {
            int size = -1;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog1.FileName;
                txt_file_path.Text = file;
                try
                {
                    string text = File.ReadAllText(file);
                    size = text.Length;
                }
                catch (IOException)
                {
                }
            }
        }


        // Server's button event handlers
        private void btn_start_srv_Click(object sender, EventArgs e)
        {
            // Enabling timer
            // () => == delegate
            Invoke(new ThreadStart(() => {
                tmr_grid_data_update.Enabled = true;
            }));

            servController.SetupServer();

        }
    }
}