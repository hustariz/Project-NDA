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
            this.txt_port_client = new System.Windows.Forms.TextBox();
            this.lbl_port_client = new System.Windows.Forms.Label();
            this.txt_host_client = new System.Windows.Forms.TextBox();
            this.lbl_host_client = new System.Windows.Forms.Label();
            this.btn_connection_client = new System.Windows.Forms.Button();
            this.txt_message_client = new System.Windows.Forms.TextBox();
            this.btn_send_client = new System.Windows.Forms.Button();
            this.txt_status_client = new System.Windows.Forms.TextBox();
            this.btn_exit_client = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_port_client
            // 
            this.txt_port_client.Location = new System.Drawing.Point(191, 17);
            this.txt_port_client.Name = "txt_port_client";
            this.txt_port_client.Size = new System.Drawing.Size(34, 20);
            this.txt_port_client.TabIndex = 9;
            this.txt_port_client.Text = "100";
            // 
            // lbl_port_client
            // 
            this.lbl_port_client.AutoSize = true;
            this.lbl_port_client.Location = new System.Drawing.Point(159, 20);
            this.lbl_port_client.Name = "lbl_port_client";
            this.lbl_port_client.Size = new System.Drawing.Size(32, 13);
            this.lbl_port_client.TabIndex = 8;
            this.lbl_port_client.Text = "Port :";
            // 
            // txt_host_client
            // 
            this.txt_host_client.Location = new System.Drawing.Point(53, 17);
            this.txt_host_client.Name = "txt_host_client";
            this.txt_host_client.Size = new System.Drawing.Size(100, 20);
            this.txt_host_client.TabIndex = 7;
            this.txt_host_client.Text = "127.0.0.1";
            // 
            // lbl_host_client
            // 
            this.lbl_host_client.AutoSize = true;
            this.lbl_host_client.Location = new System.Drawing.Point(12, 20);
            this.lbl_host_client.Name = "lbl_host_client";
            this.lbl_host_client.Size = new System.Drawing.Size(35, 13);
            this.lbl_host_client.TabIndex = 6;
            this.lbl_host_client.Text = "Host :";
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
            // txt_message_client
            // 
            this.txt_message_client.Location = new System.Drawing.Point(53, 42);
            this.txt_message_client.Multiline = true;
            this.txt_message_client.Name = "txt_message_client";
            this.txt_message_client.Size = new System.Drawing.Size(333, 44);
            this.txt_message_client.TabIndex = 10;
            // 
            // btn_send_client
            // 
            this.btn_send_client.Location = new System.Drawing.Point(311, 92);
            this.btn_send_client.Name = "btn_send_client";
            this.btn_send_client.Size = new System.Drawing.Size(75, 23);
            this.btn_send_client.TabIndex = 11;
            this.btn_send_client.Text = "Send";
            this.btn_send_client.UseVisualStyleBackColor = true;
            this.btn_send_client.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // txt_status_client
            // 
            this.txt_status_client.Location = new System.Drawing.Point(53, 121);
            this.txt_status_client.Multiline = true;
            this.txt_status_client.Name = "txt_status_client";
            this.txt_status_client.Size = new System.Drawing.Size(333, 148);
            this.txt_status_client.TabIndex = 12;
            // 
            // btn_exit_client
            // 
            this.btn_exit_client.Location = new System.Drawing.Point(311, 15);
            this.btn_exit_client.Name = "btn_exit_client";
            this.btn_exit_client.Size = new System.Drawing.Size(75, 23);
            this.btn_exit_client.TabIndex = 13;
            this.btn_exit_client.Text = "Exit";
            this.btn_exit_client.UseVisualStyleBackColor = true;
            this.btn_exit_client.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // Client
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 292);
            this.Controls.Add(this.btn_exit_client);
            this.Controls.Add(this.txt_status_client);
            this.Controls.Add(this.btn_send_client);
            this.Controls.Add(this.txt_message_client);
            this.Controls.Add(this.txt_port_client);
            this.Controls.Add(this.lbl_port_client);
            this.Controls.Add(this.txt_host_client);
            this.Controls.Add(this.lbl_host_client);
            this.Controls.Add(this.btn_connection_client);
            this.Name = "Client";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_port_client;
        private System.Windows.Forms.Label lbl_port_client;
        private System.Windows.Forms.TextBox txt_host_client;
        private System.Windows.Forms.Label lbl_host_client;
        private System.Windows.Forms.Button btn_connection_client;
        private System.Windows.Forms.TextBox txt_message_client;
        private System.Windows.Forms.Button btn_send_client;
        private System.Windows.Forms.TextBox txt_status_client;
        private System.Windows.Forms.Button btn_exit_client;
    }
}

