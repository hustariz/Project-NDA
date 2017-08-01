using System;
using System.Collections.Generic;
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
        bool gridCreated = false;
        // Append the Server Status Textbox with the argument
        public void AppendSrvStatus(params object[] message)
        {
            txt_status_srv.AppendText(string.Join(" ", message) + Environment.NewLine);
        }

        public void CreateNodeGrdStatus(string nodeAdress, int nodeActiveWThread, int nodeWThreadCount, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            Invoke(new ThreadStart(() =>
            {
                grd_node_data.Rows.Add(nodeAdress, nodeActiveWThread + "/" + nodeWThreadCount, nodeProcessorUsage + "%", nodeMemoryUsage + "MB");
                gridCreated = true;
            }));
        }


        public void AppendGrdStatus(string nodeAdress, int nodeActiveWThread, int nodeWThreadCount, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            Invoke(new ThreadStart(() =>
            {
                foreach (DataGridViewRow row in grd_node_data.Rows)
                {
                    //Local or Distant node
                    //INode node = (INode)row.Cells[0].Value;
                    row.SetValues(nodeAdress, nodeActiveWThread + "/" + nodeWThreadCount, Math.Round(nodeProcessorUsage, 2) + "%", nodeMemoryUsage + "MB");
                }
            }));
        }

        public void SLog(string message)
        {
            AppendSrvStatus(message);
        }
        public void Nlog(string nodeAdress, int nodeActiveWThread, int nodeWThreadCount, float nodeProcessorUsage, float nodeMemoryUsage)
        {
            if (!gridCreated) CreateNodeGrdStatus(nodeAdress, nodeActiveWThread, nodeWThreadCount, nodeProcessorUsage, nodeMemoryUsage);
            AppendGrdStatus(nodeAdress, nodeActiveWThread, nodeWThreadCount, nodeProcessorUsage, nodeMemoryUsage);
        }

        // Update the data grid, timer = 1s
        private void tmr_grid_data_update_Tick(object sender, EventArgs e)
        {
            servController.updateNodeGridData();            
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
            servController.ConnectNode(Convert.ToInt32(nmr_local_thread.Value), ipServer);

        }
    }
}