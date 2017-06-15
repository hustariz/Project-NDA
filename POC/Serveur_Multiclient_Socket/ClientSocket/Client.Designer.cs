namespace ClientSocket
{
    partial class Client
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
            this.txt_port = new System.Windows.Forms.TextBox();
            this.lbl_port = new System.Windows.Forms.Label();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.lbl_host = new System.Windows.Forms.Label();
            this.btn_connection_client = new System.Windows.Forms.Button();
            this.txt_message = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.txt_status = new System.Windows.Forms.TextBox();
            this.btn_exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(191, 17);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(34, 20);
            this.txt_port.TabIndex = 9;
            this.txt_port.Text = "100";
            // 
            // lbl_port
            // 
            this.lbl_port.AutoSize = true;
            this.lbl_port.Location = new System.Drawing.Point(159, 20);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(32, 13);
            this.lbl_port.TabIndex = 8;
            this.lbl_port.Text = "Port :";
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(53, 17);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(100, 20);
            this.txt_host.TabIndex = 7;
            this.txt_host.Text = "127.0.0.1";
            // 
            // lbl_host
            // 
            this.lbl_host.AutoSize = true;
            this.lbl_host.Location = new System.Drawing.Point(12, 20);
            this.lbl_host.Name = "lbl_host";
            this.lbl_host.Size = new System.Drawing.Size(35, 13);
            this.lbl_host.TabIndex = 6;
            this.lbl_host.Text = "Host :";
            // 
            // btn_connection_client
            // 
            this.btn_connection_client.Location = new System.Drawing.Point(231, 15);
            this.btn_connection_client.Name = "btn_connection_client";
            this.btn_connection_client.Size = new System.Drawing.Size(75, 23);
            this.btn_connection_client.TabIndex = 5;
            this.btn_connection_client.Text = "Connect";
            this.btn_connection_client.UseVisualStyleBackColor = true;
            this.btn_connection_client.Click += new System.EventHandler(this.btn_connection_client_Click);
            // 
            // txt_message
            // 
            this.txt_message.Location = new System.Drawing.Point(53, 42);
            this.txt_message.Multiline = true;
            this.txt_message.Name = "txt_message";
            this.txt_message.Size = new System.Drawing.Size(333, 44);
            this.txt_message.TabIndex = 10;
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(311, 92);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(75, 23);
            this.btn_send.TabIndex = 11;
            this.btn_send.Text = "Send";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txt_status
            // 
            this.txt_status.Location = new System.Drawing.Point(53, 121);
            this.txt_status.Multiline = true;
            this.txt_status.Name = "txt_status";
            this.txt_status.Size = new System.Drawing.Size(333, 148);
            this.txt_status.TabIndex = 12;
            // 
            // btn_exit
            // 
            this.btn_exit.Location = new System.Drawing.Point(311, 15);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(75, 23);
            this.btn_exit.TabIndex = 13;
            this.btn_exit.Text = "Exit";
            this.btn_exit.UseVisualStyleBackColor = true;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 292);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.txt_status);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.txt_message);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.txt_host);
            this.Controls.Add(this.lbl_host);
            this.Controls.Add(this.btn_connection_client);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.Label lbl_host;
        private System.Windows.Forms.Button btn_connection_client;
        private System.Windows.Forms.TextBox txt_message;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox txt_status;
        private System.Windows.Forms.Button btn_exit;
    }
}

