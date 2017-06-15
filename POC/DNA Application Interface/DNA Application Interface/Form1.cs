using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace DNA_Application_Interface
{
    public partial class Form1 : Form
    {

        private TcpClient client;
        public StreamReader STR;
        public StreamWriter STW;
        public String receive;
        public String text_to_send;


        public Form1()
        {
            InitializeComponent();
            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());  // get my own IP
            foreach(IPAddress address in localIP)
            {
                if(address.AddressFamily == AddressFamily.InterNetwork)
                {
                    textBox3.Text = address.ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {
                text_to_send = textBox1.Text;
                backgroundWorker2.RunWorkerAsync();

            }
            textBox1.Text = ""; 
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) // Start Server
        {


            ServUtils servUtils = new ServUtils();
            MessageBox.Show("Mabite : " + servUtils.divideGenomeByWorker(2));


            TcpListener listener = new TcpListener(IPAddress.Any, int.Parse(textBox4.Text));
            listener.Start();
            client = listener.AcceptTcpClient();
            STR = new StreamReader(client.GetStream());
            STW = new StreamWriter(client.GetStream());
            STW.AutoFlush = true;

            backgroundWorker1.RunWorkerAsync(); // Start receiving Data in background.
            backgroundWorker2.WorkerSupportsCancellation = true; // Ability to cancel this tread.
            
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
   
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) // receive data
        {
            while (client.Connected)
            {
                try
                {
                  
                    receive = STR.ReadLine();
                    this.textBox2.Invoke(new MethodInvoker(delegate () { textBox2.AppendText("you : " + receive + "\n"); }));
                    receive = " ";
                }
                catch(Exception x)
                {
                    MessageBox.Show(x.Message.ToString());
                }
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e) // send data
        {
            if (client.Connected)
            {
                STW.WriteLine(text_to_send);
                this.textBox2.Invoke(new MethodInvoker(delegate () { textBox2.AppendText("me : " + text_to_send + "\n"); }));
            }
              else
            {
                MessageBox.Show("Send failed !");
            }
            backgroundWorker2.CancelAsync();
        }

        private void button3_Click(object sender, EventArgs e) // Connect to Server
        {
            client = new TcpClient();
            IPEndPoint IP_End = new IPEndPoint(IPAddress.Parse(textBox6.Text), int.Parse(textBox5.Text));
            try
            {
                client.Connect(IP_End);
                if (client.Connected)
                {
                    textBox2.AppendText("connected to server" + "\n");
                    STW = new StreamWriter(client.GetStream());
                    STR = new StreamReader(client.GetStream());
                    STW.AutoFlush = true;

                    backgroundWorker1.RunWorkerAsync();
                    backgroundWorker2.WorkerSupportsCancellation = true;

                    SplitFile split = new SplitFile();

                    

                    //MessageBox.Show(split.splitForBasisPairs(1));
                }
            }catch(Exception x)
            {
                MessageBox.Show(x.Message.ToString());
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
