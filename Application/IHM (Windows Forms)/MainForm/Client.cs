using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NetworkAndGenericCalculation.Sockets;
using GenomicTreatment;

namespace MainForms
{
    public partial class MainForm : Form
    {


        // Append the Client Status Textbox with the argument
        //Params object to be able to display object
        public void AppendClientStatus(params object[] message)
        {
            txt_status_client.AppendText(string.Join(" ", message) + Environment.NewLine);
        }


        public void CLog(string message)
        {
            AppendClientStatus(message);
        }

        // Client's button event handler
        private void btn_connection_client_Click(object sender, EventArgs e)
        {
            ipNode = txt_host_client.Text;

            //Initialise with the log for the IHM to access return from server

            Node = new GenomicNode(this.CLog);
            NodeController = new NodeController(this, Node);
            NodeController.ConnectToServer(IPAddress.Parse(ipNode), Int32.Parse(txt_port.Text));
        }
        private void btn_exit_client_Click(object sender, EventArgs e)
        {
            NodeController.Exit();
        }
        private void btn_send_client_Click(object sender, EventArgs e)
        {
            NodeController.SendRequest(txt_message_client.Text);
            if (txt_message_client.Text.ToLower() != "exit")
            {
                NodeController.ReceiveResponse();
            }
        }
    }
}
