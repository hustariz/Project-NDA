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

namespace MainForms
{
    public partial class MainForm : Form
    {

        Socket ClientSocket;


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
           clientController.ConnectToServer(IPAddress.Parse(ipServer), Int32.Parse(txt_port.Text));
        }
        private void btn_exit_client_Click(object sender, EventArgs e)
        {
            clientController.Exit();
        }
        private void btn_send_client_Click(object sender, EventArgs e)
        {
            clientController.SendRequest(txt_message_client.Text);
            if (txt_message_client.Text.ToLower() != "exit")
            {
                clientController.ReceiveResponse();
            }
        }
    }
}
