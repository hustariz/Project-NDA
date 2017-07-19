namespace MultiServeurSocket
{
    partial class ServerForm
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
            this.btn_start_srv = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_stop_srv = new System.Windows.Forms.Button();
            this.txt_srv_status = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btn_start_srv
            // 
            this.btn_start_srv.Location = new System.Drawing.Point(242, 26);
            this.btn_start_srv.Name = "btn_start_srv";
            this.btn_start_srv.Size = new System.Drawing.Size(75, 23);
            this.btn_start_srv.TabIndex = 0;
            this.btn_start_srv.Text = "Start";
            this.btn_start_srv.UseVisualStyleBackColor = true;
            this.btn_start_srv.Click += new System.EventHandler(this.btn_start_srv_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Host :";
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(64, 28);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(100, 20);
            this.txt_host.TabIndex = 2;
            this.txt_host.Text = "127.0.0.1";
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(202, 28);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(34, 20);
            this.txt_port.TabIndex = 4;
            this.txt_port.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(170, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port :";
            // 
            // btn_stop_srv
            // 
            this.btn_stop_srv.Location = new System.Drawing.Point(323, 26);
            this.btn_stop_srv.Name = "btn_stop_srv";
            this.btn_stop_srv.Size = new System.Drawing.Size(75, 23);
            this.btn_stop_srv.TabIndex = 5;
            this.btn_stop_srv.Text = "Stop";
            this.btn_stop_srv.UseVisualStyleBackColor = true;
            this.btn_stop_srv.Click += new System.EventHandler(this.btn_stop_srv_Click);
            // 
            // txt_srv_status
            // 
            this.txt_srv_status.Location = new System.Drawing.Point(64, 55);
            this.txt_srv_status.Multiline = true;
            this.txt_srv_status.Name = "txt_srv_status";
            this.txt_srv_status.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_srv_status.Size = new System.Drawing.Size(334, 195);
            this.txt_srv_status.TabIndex = 6;
            // 
            // ServerForm
            // 
            this.ClientSize = new System.Drawing.Size(424, 262);
            this.Controls.Add(this.txt_srv_status);
            this.Controls.Add(this.btn_stop_srv);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_host);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_start_srv);
            this.Name = "ServerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serveur";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start_srv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_stop_srv;
        private System.Windows.Forms.TextBox txt_srv_status;
    }
}

