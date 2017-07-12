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


    }
}
