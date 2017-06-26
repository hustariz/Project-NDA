namespace MainForm
{
    partial class MainForm
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.Server = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txt_srv_status = new System.Windows.Forms.TextBox();
            this.btn_stop_srv = new System.Windows.Forms.Button();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_start_srv = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_exit_client = new System.Windows.Forms.Button();
            this.txt_status_client = new System.Windows.Forms.TextBox();
            this.btn_send_client = new System.Windows.Forms.Button();
            this.txt_message_client = new System.Windows.Forms.TextBox();
            this.txt_port_client = new System.Windows.Forms.TextBox();
            this.lbl_port_client = new System.Windows.Forms.Label();
            this.txt_host_client = new System.Windows.Forms.TextBox();
            this.lbl_host_client = new System.Windows.Forms.Label();
            this.btn_connection_client = new System.Windows.Forms.Button();
            this.Server.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Server
            // 
            this.Server.Controls.Add(this.tabPage1);
            this.Server.Controls.Add(this.tabPage2);
            this.Server.Location = new System.Drawing.Point(12, 12);
            this.Server.Name = "Server";
            this.Server.SelectedIndex = 0;
            this.Server.Size = new System.Drawing.Size(405, 317);
            this.Server.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txt_srv_status);
            this.tabPage1.Controls.Add(this.btn_stop_srv);
            this.tabPage1.Controls.Add(this.txt_port);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txt_host);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btn_start_srv);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(397, 291);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txt_srv_status
            // 
            this.txt_srv_status.Location = new System.Drawing.Point(47, 41);
            this.txt_srv_status.Multiline = true;
            this.txt_srv_status.Name = "txt_srv_status";
            this.txt_srv_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_srv_status.Size = new System.Drawing.Size(334, 195);
            this.txt_srv_status.TabIndex = 13;
            // 
            // btn_stop_srv
            // 
            this.btn_stop_srv.Location = new System.Drawing.Point(306, 12);
            this.btn_stop_srv.Name = "btn_stop_srv";
            this.btn_stop_srv.Size = new System.Drawing.Size(75, 23);
            this.btn_stop_srv.TabIndex = 12;
            this.btn_stop_srv.Text = "Stop";
            this.btn_stop_srv.UseVisualStyleBackColor = true;
            this.btn_stop_srv.Click += new System.EventHandler(this.btn_stop_srv_Click);
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(185, 14);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(34, 20);
            this.txt_port.TabIndex = 11;
            this.txt_port.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(153, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Port :";
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(47, 14);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(100, 20);
            this.txt_host.TabIndex = 9;
            this.txt_host.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Host :";
            // 
            // btn_start_srv
            // 
            this.btn_start_srv.Location = new System.Drawing.Point(225, 12);
            this.btn_start_srv.Name = "btn_start_srv";
            this.btn_start_srv.Size = new System.Drawing.Size(75, 23);
            this.btn_start_srv.TabIndex = 7;
            this.btn_start_srv.Text = "Start";
            this.btn_start_srv.UseVisualStyleBackColor = true;
            this.btn_start_srv.Click += new System.EventHandler(this.btn_start_srv_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_exit_client);
            this.tabPage2.Controls.Add(this.txt_status_client);
            this.tabPage2.Controls.Add(this.btn_send_client);
            this.tabPage2.Controls.Add(this.txt_message_client);
            this.tabPage2.Controls.Add(this.txt_port_client);
            this.tabPage2.Controls.Add(this.lbl_port_client);
            this.tabPage2.Controls.Add(this.txt_host_client);
            this.tabPage2.Controls.Add(this.lbl_host_client);
            this.tabPage2.Controls.Add(this.btn_connection_client);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(397, 291);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Client";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_exit_client
            // 
            this.btn_exit_client.Location = new System.Drawing.Point(305, 16);
            this.btn_exit_client.Name = "btn_exit_client";
            this.btn_exit_client.Size = new System.Drawing.Size(75, 23);
            this.btn_exit_client.TabIndex = 22;
            this.btn_exit_client.Text = "Exit";
            this.btn_exit_client.UseVisualStyleBackColor = true;
            this.btn_exit_client.Click += new System.EventHandler(this.btn_exit_client_Click);
            // 
            // txt_status_client
            // 
            this.txt_status_client.Location = new System.Drawing.Point(47, 122);
            this.txt_status_client.Multiline = true;
            this.txt_status_client.Name = "txt_status_client";
            this.txt_status_client.Size = new System.Drawing.Size(333, 148);
            this.txt_status_client.TabIndex = 21;
            // 
            // btn_send_client
            // 
            this.btn_send_client.Location = new System.Drawing.Point(305, 93);
            this.btn_send_client.Name = "btn_send_client";
            this.btn_send_client.Size = new System.Drawing.Size(75, 23);
            this.btn_send_client.TabIndex = 20;
            this.btn_send_client.Text = "Send";
            this.btn_send_client.UseVisualStyleBackColor = true;
            this.btn_send_client.Click += new System.EventHandler(this.btn_send_client_Click);
            // 
            // txt_message_client
            // 
            this.txt_message_client.Location = new System.Drawing.Point(47, 43);
            this.txt_message_client.Multiline = true;
            this.txt_message_client.Name = "txt_message_client";
            this.txt_message_client.Size = new System.Drawing.Size(333, 44);
            this.txt_message_client.TabIndex = 19;
            // 
            // txt_port_client
            // 
            this.txt_port_client.Location = new System.Drawing.Point(185, 18);
            this.txt_port_client.Name = "txt_port_client";
            this.txt_port_client.Size = new System.Drawing.Size(34, 20);
            this.txt_port_client.TabIndex = 18;
            this.txt_port_client.Text = "100";
            // 
            // lbl_port_client
            // 
            this.lbl_port_client.AutoSize = true;
            this.lbl_port_client.Location = new System.Drawing.Point(153, 21);
            this.lbl_port_client.Name = "lbl_port_client";
            this.lbl_port_client.Size = new System.Drawing.Size(32, 13);
            this.lbl_port_client.TabIndex = 17;
            this.lbl_port_client.Text = "Port :";
            // 
            // txt_host_client
            // 
            this.txt_host_client.Location = new System.Drawing.Point(47, 18);
            this.txt_host_client.Name = "txt_host_client";
            this.txt_host_client.Size = new System.Drawing.Size(100, 20);
            this.txt_host_client.TabIndex = 16;
            this.txt_host_client.Text = "127.0.0.1";
            // 
            // lbl_host_client
            // 
            this.lbl_host_client.AutoSize = true;
            this.lbl_host_client.Location = new System.Drawing.Point(6, 21);
            this.lbl_host_client.Name = "lbl_host_client";
            this.lbl_host_client.Size = new System.Drawing.Size(35, 13);
            this.lbl_host_client.TabIndex = 15;
            this.lbl_host_client.Text = "Host :";
            // 
            // btn_connection_client
            // 
            this.btn_connection_client.Location = new System.Drawing.Point(225, 16);
            this.btn_connection_client.Name = "btn_connection_client";
            this.btn_connection_client.Size = new System.Drawing.Size(75, 23);
            this.btn_connection_client.TabIndex = 14;
            this.btn_connection_client.Text = "Connect";
            this.btn_connection_client.UseVisualStyleBackColor = true;
            this.btn_connection_client.Click += new System.EventHandler(this.btn_connection_client_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 382);
            this.Controls.Add(this.Server);
            this.Name = "MainForm";
            this.Text = "Project NDA";
            this.Server.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl Server;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btn_exit_client;
        private System.Windows.Forms.TextBox txt_status_client;
        private System.Windows.Forms.Button btn_send_client;
        private System.Windows.Forms.TextBox txt_message_client;
        private System.Windows.Forms.TextBox txt_port_client;
        private System.Windows.Forms.Label lbl_port_client;
        private System.Windows.Forms.TextBox txt_host_client;
        private System.Windows.Forms.Label lbl_host_client;
        private System.Windows.Forms.Button btn_connection_client;
        private System.Windows.Forms.TextBox txt_srv_status;
        private System.Windows.Forms.Button btn_stop_srv;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_start_srv;
    }
}

