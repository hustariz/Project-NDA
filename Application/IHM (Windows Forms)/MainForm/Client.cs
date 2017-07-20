using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace MainForm
{
    public partial class MainForm : Form
    {

        Socket ClientSocket;

        //Open a Socket Connection with a server
        private void ConnectToServer(IPAddress host, int port)
        {
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            int attempts = 0;
            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    AppendClientStatus("Connection attempt :" + attempts);
                    ClientSocket.Connect(host, port);
                }
                catch (SocketException)
                {
                    AppendClientStatus("SocketException");
                }
            }
            AppendClientStatus("Connected");
        }

        // Close socket and write a message in the status box.
        private void Exit()
        {
            SendString("exit"); // Tell the server we are exiting
            ClientSocket.Shutdown(SocketShutdown.Both);
            ClientSocket.Close();
            AppendClientStatus("Client disconnected");
        }

        // Send a Request to the server
        private void SendRequest()
        {
            string request = txt_message_client.Text;
            SendString(request);
            AppendClientStatus("request sent : " + request);
                if (request.ToLower() == "exit")
                {
                    Exit();
                }

        }


        // Sends a string to the server with ASCII encoding.
        private void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        // Receive and convert data into a string to print it
        // Do not remove the delegate == () =>
        private void ReceiveResponse()
        {
            Invoke(new ThreadStart(() =>
            {
                var buffer = new byte[2048];
                int received = ClientSocket.Receive(buffer, SocketFlags.None);
                if (received == 0) return;
                var data = new byte[received];
                Array.Copy(buffer, data, received);
                string text = Encoding.ASCII.GetString(data);
                AppendClientStatus("Response : " + text);
            }));
        }

        // Client's button event handler
        private void btn_connection_client_Click(object sender, EventArgs e)
        {
            ConnectToServer(IPAddress.Parse(txt_host.Text), Int32.Parse(txt_port.Text));
        }
        private void btn_exit_client_Click(object sender, EventArgs e)
        {
            Exit();
        }
        private void btn_send_client_Click(object sender, EventArgs e)
        {
            SendRequest();
            if (txt_message_client.Text.ToLower() != "exit")
            {
                ReceiveResponse();
            }
        }
    }
}
