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

        // Update the data grid, timer = 1s
        private void tmr_grid_data_update_Tick(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in grd_node_data.Rows)
            {
                //Local or Distant node
                //INode node = (INode)row.Cells[0].Value;
                //row.SetValues(node, node.ActualWorker + "/" + node.Workers.Count, Math.Round(node.ProcessorUsage, 2) + "%", node.MemoryUsage + "MB");
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

            //AppendSrvStatus("Setting up local node...");
            //localnode = new Node(4, txt_host.Text);
            //ConnectLocalNode(localnode);
        }

        private void btn_stop_srv_Click(object sender, EventArgs e)
        {

        }
    }
}