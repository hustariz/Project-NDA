﻿using System;

namespace MainForms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.Server = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grp_box_data_process = new System.Windows.Forms.GroupBox();
            this.btn_start_process = new System.Windows.Forms.Button();
            this.cbb_module_to_process = new System.Windows.Forms.ComboBox();
            this.lbl_module_to_execute = new System.Windows.Forms.Label();
            this.txt_file_path = new System.Windows.Forms.TextBox();
            this.btn_load_genome = new System.Windows.Forms.Button();
            this.nmr_local_thread = new System.Windows.Forms.NumericUpDown();
            this.lbl_local_thread = new System.Windows.Forms.Label();
            this.grd_node_data = new System.Windows.Forms.DataGridView();
            this.lbl_server_logs = new System.Windows.Forms.Label();
            this.txt_status_srv = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_host = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_start_srv = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lbl_client_logs = new System.Windows.Forms.Label();
            this.btn_exit_client = new System.Windows.Forms.Button();
            this.txt_status_client = new System.Windows.Forms.TextBox();
            this.btn_send_client = new System.Windows.Forms.Button();
            this.txt_message_client = new System.Windows.Forms.TextBox();
            this.txt_port_client = new System.Windows.Forms.TextBox();
            this.lbl_port_client = new System.Windows.Forms.Label();
            this.txt_host_client = new System.Windows.Forms.TextBox();
            this.lbl_host_client = new System.Windows.Forms.Label();
            this.btn_connection_client = new System.Windows.Forms.Button();
            this.application_title = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tmr_grid_data_update = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Server.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.grp_box_data_process.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_local_thread)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_node_data)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Server
            // 
            this.Server.Controls.Add(this.tabPage1);
            this.Server.Controls.Add(this.tabPage2);
            this.Server.Location = new System.Drawing.Point(165, 47);
            this.Server.Margin = new System.Windows.Forms.Padding(4);
            this.Server.Name = "Server";
            this.Server.SelectedIndex = 0;
            this.Server.Size = new System.Drawing.Size(645, 630);
            this.Server.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grp_box_data_process);
            this.tabPage1.Controls.Add(this.nmr_local_thread);
            this.tabPage1.Controls.Add(this.lbl_local_thread);
            this.tabPage1.Controls.Add(this.grd_node_data);
            this.tabPage1.Controls.Add(this.lbl_server_logs);
            this.tabPage1.Controls.Add(this.txt_status_srv);
            this.tabPage1.Controls.Add(this.txt_port);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txt_host);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.btn_start_srv);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(637, 601);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grp_box_data_process
            // 
            this.grp_box_data_process.Controls.Add(this.btn_start_process);
            this.grp_box_data_process.Controls.Add(this.cbb_module_to_process);
            this.grp_box_data_process.Controls.Add(this.lbl_module_to_execute);
            this.grp_box_data_process.Controls.Add(this.txt_file_path);
            this.grp_box_data_process.Controls.Add(this.btn_load_genome);
            this.grp_box_data_process.Enabled = false;
            this.grp_box_data_process.Location = new System.Drawing.Point(12, 495);
            this.grp_box_data_process.Margin = new System.Windows.Forms.Padding(4);
            this.grp_box_data_process.Name = "grp_box_data_process";
            this.grp_box_data_process.Padding = new System.Windows.Forms.Padding(4);
            this.grp_box_data_process.Size = new System.Drawing.Size(615, 96);
            this.grp_box_data_process.TabIndex = 19;
            this.grp_box_data_process.TabStop = false;
            this.grp_box_data_process.Text = "Input Processing";
            // 
            // btn_start_process
            // 
            this.btn_start_process.Location = new System.Drawing.Point(456, 23);
            this.btn_start_process.Margin = new System.Windows.Forms.Padding(4);
            this.btn_start_process.Name = "btn_start_process";
            this.btn_start_process.Size = new System.Drawing.Size(159, 59);
            this.btn_start_process.TabIndex = 28;
            this.btn_start_process.Text = "Start Processing";
            this.btn_start_process.UseVisualStyleBackColor = true;
            this.btn_start_process.Click += new System.EventHandler(this.btn_start_process_Click);
            // 
            // cbb_module_to_process
            // 
            this.cbb_module_to_process.FormattingEnabled = true;
            this.cbb_module_to_process.Location = new System.Drawing.Point(147, 57);
            this.cbb_module_to_process.Margin = new System.Windows.Forms.Padding(4);
            this.cbb_module_to_process.Name = "cbb_module_to_process";
            this.cbb_module_to_process.Size = new System.Drawing.Size(300, 24);
            this.cbb_module_to_process.TabIndex = 27;
            // 
            // lbl_module_to_execute
            // 
            this.lbl_module_to_execute.AutoSize = true;
            this.lbl_module_to_execute.Location = new System.Drawing.Point(8, 60);
            this.lbl_module_to_execute.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_module_to_execute.Name = "lbl_module_to_execute";
            this.lbl_module_to_execute.Size = new System.Drawing.Size(131, 17);
            this.lbl_module_to_execute.TabIndex = 26;
            this.lbl_module_to_execute.Text = "Module to perform :";
            // 
            // txt_file_path
            // 
            this.txt_file_path.Location = new System.Drawing.Point(147, 22);
            this.txt_file_path.Margin = new System.Windows.Forms.Padding(4);
            this.txt_file_path.Name = "txt_file_path";
            this.txt_file_path.Size = new System.Drawing.Size(300, 22);
            this.txt_file_path.TabIndex = 25;
            // 
            // btn_load_genome
            // 
            this.btn_load_genome.Location = new System.Drawing.Point(8, 20);
            this.btn_load_genome.Margin = new System.Windows.Forms.Padding(4);
            this.btn_load_genome.Name = "btn_load_genome";
            this.btn_load_genome.Size = new System.Drawing.Size(131, 28);
            this.btn_load_genome.TabIndex = 24;
            this.btn_load_genome.Text = "Load Genome";
            this.btn_load_genome.UseVisualStyleBackColor = true;
            this.btn_load_genome.Click += new System.EventHandler(this.btn_load_genome_Click);
            // 
            // nmr_local_thread
            // 
            this.nmr_local_thread.Enabled = false;
            this.nmr_local_thread.Location = new System.Drawing.Point(444, 16);
            this.nmr_local_thread.Margin = new System.Windows.Forms.Padding(4);
            this.nmr_local_thread.Name = "nmr_local_thread";
            this.nmr_local_thread.Size = new System.Drawing.Size(48, 22);
            this.nmr_local_thread.TabIndex = 18;
            this.nmr_local_thread.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lbl_local_thread
            // 
            this.lbl_local_thread.AutoSize = true;
            this.lbl_local_thread.Enabled = false;
            this.lbl_local_thread.Location = new System.Drawing.Point(312, 20);
            this.lbl_local_thread.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_local_thread.Name = "lbl_local_thread";
            this.lbl_local_thread.Size = new System.Drawing.Size(123, 17);
            this.lbl_local_thread.TabIndex = 17;
            this.lbl_local_thread.Text = "Available Thread :";
            // 
            // grd_node_data
            // 
            this.grd_node_data.AllowUserToAddRows = false;
            this.grd_node_data.AllowUserToDeleteRows = false;
            this.grd_node_data.AllowUserToOrderColumns = true;
            this.grd_node_data.BackgroundColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.grd_node_data.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grd_node_data.Location = new System.Drawing.Point(9, 49);
            this.grd_node_data.Margin = new System.Windows.Forms.Padding(4);
            this.grd_node_data.Name = "grd_node_data";
            this.grd_node_data.ReadOnly = true;
            this.grd_node_data.RowHeadersVisible = false;
            this.grd_node_data.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grd_node_data.Size = new System.Drawing.Size(616, 176);
            this.grd_node_data.TabIndex = 16;
            // 
            // lbl_server_logs
            // 
            this.lbl_server_logs.AutoSize = true;
            this.lbl_server_logs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbl_server_logs.Location = new System.Drawing.Point(8, 233);
            this.lbl_server_logs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_server_logs.Name = "lbl_server_logs";
            this.lbl_server_logs.Size = new System.Drawing.Size(88, 18);
            this.lbl_server_logs.TabIndex = 15;
            this.lbl_server_logs.Text = "Server Logs";
            // 
            // txt_status_srv
            // 
            this.txt_status_srv.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txt_status_srv.Location = new System.Drawing.Point(12, 255);
            this.txt_status_srv.Margin = new System.Windows.Forms.Padding(4);
            this.txt_status_srv.Multiline = true;
            this.txt_status_srv.Name = "txt_status_srv";
            this.txt_status_srv.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_status_srv.Size = new System.Drawing.Size(612, 232);
            this.txt_status_srv.TabIndex = 13;
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(259, 16);
            this.txt_port.Margin = new System.Windows.Forms.Padding(4);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(44, 22);
            this.txt_port.TabIndex = 11;
            this.txt_port.Text = "100";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(208, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 10;
            this.label2.Text = "Port :";
            // 
            // txt_host
            // 
            this.txt_host.Location = new System.Drawing.Point(63, 16);
            this.txt_host.Margin = new System.Windows.Forms.Padding(4);
            this.txt_host.Name = "txt_host";
            this.txt_host.Size = new System.Drawing.Size(132, 22);
            this.txt_host.TabIndex = 9;
            this.txt_host.Text = "127.0.0.1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 17);
            this.label1.TabIndex = 8;
            this.label1.Text = "Host :";
            // 
            // btn_start_srv
            // 
            this.btn_start_srv.Location = new System.Drawing.Point(525, 14);
            this.btn_start_srv.Margin = new System.Windows.Forms.Padding(4);
            this.btn_start_srv.Name = "btn_start_srv";
            this.btn_start_srv.Size = new System.Drawing.Size(100, 28);
            this.btn_start_srv.TabIndex = 7;
            this.btn_start_srv.Text = "Start";
            this.btn_start_srv.UseVisualStyleBackColor = true;
            this.btn_start_srv.Click += new System.EventHandler(this.btn_start_srv_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lbl_client_logs);
            this.tabPage2.Controls.Add(this.btn_exit_client);
            this.tabPage2.Controls.Add(this.txt_status_client);
            this.tabPage2.Controls.Add(this.btn_send_client);
            this.tabPage2.Controls.Add(this.txt_message_client);
            this.tabPage2.Controls.Add(this.txt_port_client);
            this.tabPage2.Controls.Add(this.lbl_port_client);
            this.tabPage2.Controls.Add(this.txt_host_client);
            this.tabPage2.Controls.Add(this.lbl_host_client);
            this.tabPage2.Controls.Add(this.btn_connection_client);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(637, 601);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Client";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lbl_client_logs
            // 
            this.lbl_client_logs.AutoSize = true;
            this.lbl_client_logs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.lbl_client_logs.Location = new System.Drawing.Point(12, 170);
            this.lbl_client_logs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_client_logs.Name = "lbl_client_logs";
            this.lbl_client_logs.Size = new System.Drawing.Size(82, 18);
            this.lbl_client_logs.TabIndex = 23;
            this.lbl_client_logs.Text = "Client Logs";
            // 
            // btn_exit_client
            // 
            this.btn_exit_client.Location = new System.Drawing.Point(527, 20);
            this.btn_exit_client.Margin = new System.Windows.Forms.Padding(4);
            this.btn_exit_client.Name = "btn_exit_client";
            this.btn_exit_client.Size = new System.Drawing.Size(100, 28);
            this.btn_exit_client.TabIndex = 22;
            this.btn_exit_client.Text = "Exit";
            this.btn_exit_client.UseVisualStyleBackColor = true;
            this.btn_exit_client.Click += new System.EventHandler(this.btn_exit_client_Click);
            // 
            // txt_status_client
            // 
            this.txt_status_client.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txt_status_client.Location = new System.Drawing.Point(8, 193);
            this.txt_status_client.Margin = new System.Windows.Forms.Padding(4);
            this.txt_status_client.Multiline = true;
            this.txt_status_client.Name = "txt_status_client";
            this.txt_status_client.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_status_client.Size = new System.Drawing.Size(617, 397);
            this.txt_status_client.TabIndex = 21;
            // 
            // btn_send_client
            // 
            this.btn_send_client.Location = new System.Drawing.Point(527, 139);
            this.btn_send_client.Margin = new System.Windows.Forms.Padding(4);
            this.btn_send_client.Name = "btn_send_client";
            this.btn_send_client.Size = new System.Drawing.Size(100, 28);
            this.btn_send_client.TabIndex = 20;
            this.btn_send_client.Text = "Send";
            this.btn_send_client.UseVisualStyleBackColor = true;
            this.btn_send_client.Click += new System.EventHandler(this.btn_send_client_Click);
            // 
            // txt_message_client
            // 
            this.txt_message_client.Location = new System.Drawing.Point(8, 53);
            this.txt_message_client.Margin = new System.Windows.Forms.Padding(4);
            this.txt_message_client.Multiline = true;
            this.txt_message_client.Name = "txt_message_client";
            this.txt_message_client.Size = new System.Drawing.Size(617, 78);
            this.txt_message_client.TabIndex = 19;
            // 
            // txt_port_client
            // 
            this.txt_port_client.Location = new System.Drawing.Point(255, 23);
            this.txt_port_client.Margin = new System.Windows.Forms.Padding(4);
            this.txt_port_client.Name = "txt_port_client";
            this.txt_port_client.Size = new System.Drawing.Size(44, 22);
            this.txt_port_client.TabIndex = 18;
            this.txt_port_client.Text = "100";
            // 
            // lbl_port_client
            // 
            this.lbl_port_client.AutoSize = true;
            this.lbl_port_client.Location = new System.Drawing.Point(204, 26);
            this.lbl_port_client.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_port_client.Name = "lbl_port_client";
            this.lbl_port_client.Size = new System.Drawing.Size(42, 17);
            this.lbl_port_client.TabIndex = 17;
            this.lbl_port_client.Text = "Port :";
            // 
            // txt_host_client
            // 
            this.txt_host_client.Location = new System.Drawing.Point(63, 22);
            this.txt_host_client.Margin = new System.Windows.Forms.Padding(4);
            this.txt_host_client.Name = "txt_host_client";
            this.txt_host_client.Size = new System.Drawing.Size(132, 22);
            this.txt_host_client.TabIndex = 16;
            this.txt_host_client.Text = "127.0.0.1";
            // 
            // lbl_host_client
            // 
            this.lbl_host_client.AutoSize = true;
            this.lbl_host_client.Location = new System.Drawing.Point(8, 26);
            this.lbl_host_client.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_host_client.Name = "lbl_host_client";
            this.lbl_host_client.Size = new System.Drawing.Size(45, 17);
            this.lbl_host_client.TabIndex = 15;
            this.lbl_host_client.Text = "Host :";
            // 
            // btn_connection_client
            // 
            this.btn_connection_client.Location = new System.Drawing.Point(419, 20);
            this.btn_connection_client.Margin = new System.Windows.Forms.Padding(4);
            this.btn_connection_client.Name = "btn_connection_client";
            this.btn_connection_client.Size = new System.Drawing.Size(100, 28);
            this.btn_connection_client.TabIndex = 14;
            this.btn_connection_client.Text = "Connect";
            this.btn_connection_client.UseVisualStyleBackColor = true;
            this.btn_connection_client.Click += new System.EventHandler(this.btn_connection_client_Click);
            // 
            // application_title
            // 
            this.application_title.AutoSize = true;
            this.application_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold);
            this.application_title.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.application_title.Location = new System.Drawing.Point(348, 5);
            this.application_title.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.application_title.Name = "application_title";
            this.application_title.Size = new System.Drawing.Size(270, 39);
            this.application_title.TabIndex = 1;
            this.application_title.Text = "PROJECT NDA";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(819, 74);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(211, 598);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.ErrorImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.ErrorImage")));
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.InitialImage")));
            this.pictureBox1.Location = new System.Drawing.Point(21, 74);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(136, 603);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // tmr_grid_data_update
            // 
            this.tmr_grid_data_update.Interval = 1000;
            this.tmr_grid_data_update.Tick += new System.EventHandler(this.tmr_grid_data_update_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1045, 692);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.application_title);
            this.Controls.Add(this.Server);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Project NDA";
            this.Server.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.grp_box_data_process.ResumeLayout(false);
            this.grp_box_data_process.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmr_local_thread)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grd_node_data)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.TextBox txt_status_srv;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_host;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_start_srv;
        private System.Windows.Forms.Label application_title;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label lbl_server_logs;
        private System.Windows.Forms.Label lbl_client_logs;
        private System.Windows.Forms.DataGridView grd_node_data;
        private System.Windows.Forms.Timer tmr_grid_data_update;
        private System.Windows.Forms.NumericUpDown nmr_local_thread;
        private System.Windows.Forms.Label lbl_local_thread;
        private System.Windows.Forms.GroupBox grp_box_data_process;
        private System.Windows.Forms.Button btn_start_process;
        private System.Windows.Forms.ComboBox cbb_module_to_process;
        private System.Windows.Forms.Label lbl_module_to_execute;
        private System.Windows.Forms.TextBox txt_file_path;
        private System.Windows.Forms.Button btn_load_genome;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

