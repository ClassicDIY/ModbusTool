namespace ModbusMaster
{
    partial class MasterForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            this.groupBoxFunctions = new System.Windows.Forms.GroupBox();
            this.txtPollDelay = new System.Windows.Forms.TextBox();
            this.cbPoll = new System.Windows.Forms.CheckBox();
            this.btnReadCoils = new System.Windows.Forms.Button();
            this.btnReadDisInp = new System.Windows.Forms.Button();
            this.btnWriteMultipleReg = new System.Windows.Forms.Button();
            this.btnReadHoldReg = new System.Windows.Forms.Button();
            this.btnWriteMultipleCoils = new System.Windows.Forms.Button();
            this.btnReadInpReg = new System.Windows.Forms.Button();
            this.btnWriteSingleReg = new System.Windows.Forms.Button();
            this.btnWriteSingleCoil = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pollTimer = new System.Windows.Forms.Timer(this.components);
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpStart.SuspendLayout();
            this.groupBoxRTU.SuspendLayout();
            this.groupBoxMode.SuspendLayout();
            this.groupBoxTCP.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.grpExchange.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.groupBoxFunctions.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Location = new System.Drawing.Point(7, 718);
            // 
            // groupBox3
            // 
            this.groupBox3.Size = new System.Drawing.Size(189, 110);
            // 
            // radioButtonInteger
            // 
            this.radioButtonInteger.Location = new System.Drawing.Point(86, 20);
            this.radioButtonInteger.Size = new System.Drawing.Size(64, 21);
            // 
            // comboBoxSerialPorts
            // 
            this.comboBoxSerialPorts.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5"});
            // 
            // label1
            // 
            this.label1.Visible = false;
            // 
            // textBoxSlaveDelay
            // 
            this.textBoxSlaveDelay.Visible = false;
            // 
            // comboBoxParity
            // 
            this.comboBoxParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            // 
            // radioButtonReverseFloat
            // 
            this.radioButtonReverseFloat.Location = new System.Drawing.Point(86, 40);
            // 
            // groupBoxFunctions
            // 
            this.groupBoxFunctions.Controls.Add(this.txtPollDelay);
            this.groupBoxFunctions.Controls.Add(this.cbPoll);
            this.groupBoxFunctions.Controls.Add(this.btnReadCoils);
            this.groupBoxFunctions.Controls.Add(this.btnReadDisInp);
            this.groupBoxFunctions.Controls.Add(this.btnWriteMultipleReg);
            this.groupBoxFunctions.Controls.Add(this.btnReadHoldReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteMultipleCoils);
            this.groupBoxFunctions.Controls.Add(this.btnReadInpReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteSingleReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteSingleCoil);
            this.groupBoxFunctions.Enabled = false;
            this.groupBoxFunctions.Location = new System.Drawing.Point(225, 144);
            this.groupBoxFunctions.Name = "groupBoxFunctions";
            this.groupBoxFunctions.Size = new System.Drawing.Size(340, 128);
            this.groupBoxFunctions.TabIndex = 35;
            this.groupBoxFunctions.TabStop = false;
            this.groupBoxFunctions.Text = "Functions";
            // 
            // txtPollDelay
            // 
            this.txtPollDelay.Location = new System.Drawing.Point(56, 100);
            this.txtPollDelay.Name = "txtPollDelay";
            this.txtPollDelay.Size = new System.Drawing.Size(52, 20);
            this.txtPollDelay.TabIndex = 25;
            this.txtPollDelay.Text = "2000";
            this.txtPollDelay.Leave += new System.EventHandler(this.txtPollDelay_Leave);
            // 
            // cbPoll
            // 
            this.cbPoll.AutoSize = true;
            this.cbPoll.Location = new System.Drawing.Point(7, 102);
            this.cbPoll.Name = "cbPoll";
            this.cbPoll.Size = new System.Drawing.Size(43, 17);
            this.cbPoll.TabIndex = 24;
            this.cbPoll.Text = "Poll";
            this.cbPoll.UseVisualStyleBackColor = true;
            this.cbPoll.CheckStateChanged += new System.EventHandler(this.cbPoll_CheckStateChanged);
            // 
            // btnReadCoils
            // 
            this.btnReadCoils.Location = new System.Drawing.Point(6, 19);
            this.btnReadCoils.Name = "btnReadCoils";
            this.btnReadCoils.Size = new System.Drawing.Size(78, 35);
            this.btnReadCoils.TabIndex = 11;
            this.btnReadCoils.Text = "Read coils";
            this.btnReadCoils.Click += new System.EventHandler(this.BtnReadCoilsClick);
            // 
            // btnReadDisInp
            // 
            this.btnReadDisInp.Location = new System.Drawing.Point(6, 60);
            this.btnReadDisInp.Name = "btnReadDisInp";
            this.btnReadDisInp.Size = new System.Drawing.Size(78, 35);
            this.btnReadDisInp.TabIndex = 16;
            this.btnReadDisInp.Text = "Read discrete inputs";
            this.btnReadDisInp.Click += new System.EventHandler(this.BtnReadDisInpClick);
            // 
            // btnWriteMultipleReg
            // 
            this.btnWriteMultipleReg.Location = new System.Drawing.Point(249, 60);
            this.btnWriteMultipleReg.Name = "btnWriteMultipleReg";
            this.btnWriteMultipleReg.Size = new System.Drawing.Size(78, 35);
            this.btnWriteMultipleReg.TabIndex = 23;
            this.btnWriteMultipleReg.Text = "Write multiple register";
            this.btnWriteMultipleReg.Click += new System.EventHandler(this.BtnWriteMultipleRegClick);
            // 
            // btnReadHoldReg
            // 
            this.btnReadHoldReg.Location = new System.Drawing.Point(87, 19);
            this.btnReadHoldReg.Name = "btnReadHoldReg";
            this.btnReadHoldReg.Size = new System.Drawing.Size(78, 35);
            this.btnReadHoldReg.TabIndex = 17;
            this.btnReadHoldReg.Text = "Read holding register";
            this.btnReadHoldReg.Click += new System.EventHandler(this.BtnReadHoldRegClick);
            // 
            // btnWriteMultipleCoils
            // 
            this.btnWriteMultipleCoils.Location = new System.Drawing.Point(249, 19);
            this.btnWriteMultipleCoils.Name = "btnWriteMultipleCoils";
            this.btnWriteMultipleCoils.Size = new System.Drawing.Size(78, 35);
            this.btnWriteMultipleCoils.TabIndex = 22;
            this.btnWriteMultipleCoils.Text = "Write multiple coils";
            this.btnWriteMultipleCoils.Click += new System.EventHandler(this.BtnWriteMultipleCoilsClick);
            // 
            // btnReadInpReg
            // 
            this.btnReadInpReg.Location = new System.Drawing.Point(87, 60);
            this.btnReadInpReg.Name = "btnReadInpReg";
            this.btnReadInpReg.Size = new System.Drawing.Size(78, 35);
            this.btnReadInpReg.TabIndex = 18;
            this.btnReadInpReg.Text = "Read input register";
            this.btnReadInpReg.Click += new System.EventHandler(this.BtnReadInpRegClick);
            // 
            // btnWriteSingleReg
            // 
            this.btnWriteSingleReg.Location = new System.Drawing.Point(168, 60);
            this.btnWriteSingleReg.Name = "btnWriteSingleReg";
            this.btnWriteSingleReg.Size = new System.Drawing.Size(78, 35);
            this.btnWriteSingleReg.TabIndex = 21;
            this.btnWriteSingleReg.Text = "Write single register";
            this.btnWriteSingleReg.Click += new System.EventHandler(this.BtnWriteSingleRegClick);
            // 
            // btnWriteSingleCoil
            // 
            this.btnWriteSingleCoil.Location = new System.Drawing.Point(168, 19);
            this.btnWriteSingleCoil.Name = "btnWriteSingleCoil";
            this.btnWriteSingleCoil.Size = new System.Drawing.Size(78, 35);
            this.btnWriteSingleCoil.TabIndex = 19;
            this.btnWriteSingleCoil.Text = "Write single coil";
            this.btnWriteSingleCoil.Click += new System.EventHandler(this.BtnWriteSingleCoilClick);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(759, 47);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(86, 28);
            this.buttonDisconnect.TabIndex = 37;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.Click += new System.EventHandler(this.ButtonDisconnectClick);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(759, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(86, 28);
            this.btnConnect.TabIndex = 36;
            this.btnConnect.Text = "Connect";
            this.btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
            // 
            // pollTimer
            // 
            this.pollTimer.Interval = 2000;
            this.pollTimer.Tick += new System.EventHandler(this.pollTimer_Tick);
            // 
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(869, 917);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBoxFunctions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MasterForm";
            this.ShowDataLength = true;
            this.Text = "Modbus Master";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterFormClosing);
            this.Controls.SetChildIndex(this.tabControl1, 0);
            this.Controls.SetChildIndex(this.grpExchange, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.Controls.SetChildIndex(this.grpStart, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.groupBoxFunctions, 0);
            this.Controls.SetChildIndex(this.btnConnect, 0);
            this.Controls.SetChildIndex(this.buttonDisconnect, 0);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.grpStart.ResumeLayout(false);
            this.groupBoxRTU.ResumeLayout(false);
            this.groupBoxRTU.PerformLayout();
            this.groupBoxMode.ResumeLayout(false);
            this.groupBoxMode.PerformLayout();
            this.groupBoxTCP.ResumeLayout(false);
            this.groupBoxTCP.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.groupBoxFunctions.ResumeLayout(false);
            this.groupBoxFunctions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxFunctions;
        private System.Windows.Forms.Button btnReadCoils;
        private System.Windows.Forms.Button btnReadDisInp;
        private System.Windows.Forms.Button btnWriteMultipleReg;
        private System.Windows.Forms.Button btnReadHoldReg;
        private System.Windows.Forms.Button btnWriteMultipleCoils;
        private System.Windows.Forms.Button btnReadInpReg;
        private System.Windows.Forms.Button btnWriteSingleReg;
        private System.Windows.Forms.Button btnWriteSingleCoil;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPollDelay;
        private System.Windows.Forms.CheckBox cbPoll;
        private System.Windows.Forms.Timer pollTimer;
    }
}
