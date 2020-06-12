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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MasterForm));
            this.groupBoxFunctions = new System.Windows.Forms.GroupBox();
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
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.grpStart.SuspendLayout();
            this.groupBoxRTU.SuspendLayout();
            this.groupBoxMode.SuspendLayout();
            this.groupBoxTCP.SuspendLayout();
            this.grpExchange.SuspendLayout();
            this.groupBoxFunctions.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxSerialPorts
            // 
            this.comboBoxSerialPorts.Items.AddRange(new object[] {
            "COM3",
            "COM4",
            "COM3",
            "COM4",
            "COM3",
            "COM4",
            "COM3",
            "COM4",
            "COM3",
            "COM4",
            "COM5",
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
            "Space",
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space",
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space",
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space",
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space",
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            // 
            // groupBoxFunctions
            // 
            this.groupBoxFunctions.Controls.Add(this.btnReadCoils);
            this.groupBoxFunctions.Controls.Add(this.btnReadDisInp);
            this.groupBoxFunctions.Controls.Add(this.btnWriteMultipleReg);
            this.groupBoxFunctions.Controls.Add(this.btnReadHoldReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteMultipleCoils);
            this.groupBoxFunctions.Controls.Add(this.btnReadInpReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteSingleReg);
            this.groupBoxFunctions.Controls.Add(this.btnWriteSingleCoil);
            this.groupBoxFunctions.Enabled = false;
            this.groupBoxFunctions.Location = new System.Drawing.Point(179, 144);
            this.groupBoxFunctions.Name = "groupBoxFunctions";
            this.groupBoxFunctions.Size = new System.Drawing.Size(339, 110);
            this.groupBoxFunctions.TabIndex = 35;
            this.groupBoxFunctions.TabStop = false;
            this.groupBoxFunctions.Text = "Functions";
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
            // MasterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(869, 887);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.groupBoxFunctions);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MasterForm";
            this.ShowDataLength = true;
            this.Text = "Modbus Master";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterFormClosing);
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
            this.grpExchange.ResumeLayout(false);
            this.grpExchange.PerformLayout();
            this.groupBoxFunctions.ResumeLayout(false);
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
    }
}
