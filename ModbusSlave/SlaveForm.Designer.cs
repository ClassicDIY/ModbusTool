namespace ModbusSlave
{
    partial class SlaveForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlaveForm));
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.btnConnect = new System.Windows.Forms.Button();
            this.groupBoxFunctions = new System.Windows.Forms.GroupBox();
            this.radioButtonCoilStatus = new System.Windows.Forms.RadioButton();
            this.radioButtonInputStatus = new System.Windows.Forms.RadioButton();
            this.radioButtonInputRegisters = new System.Windows.Forms.RadioButton();
            this.radioButtonHoldingRegister = new System.Windows.Forms.RadioButton();
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
            "COM5"});
            // 
            // label8
            // 
            this.label8.Visible = false;
            // 
            // txtIP
            // 
            this.txtIP.Visible = false;
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
            "Space"});
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Location = new System.Drawing.Point(758, 46);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(86, 28);
            this.buttonDisconnect.TabIndex = 35;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.Click += new System.EventHandler(this.ButtonDisconnectClick);
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(758, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(86, 28);
            this.btnConnect.TabIndex = 34;
            this.btnConnect.Text = "Listen";
            this.btnConnect.Click += new System.EventHandler(this.BtnConnectClick);
            // 
            // groupBoxFunctions
            // 
            this.groupBoxFunctions.Controls.Add(this.radioButtonCoilStatus);
            this.groupBoxFunctions.Controls.Add(this.radioButtonInputStatus);
            this.groupBoxFunctions.Controls.Add(this.radioButtonInputRegisters);
            this.groupBoxFunctions.Controls.Add(this.radioButtonHoldingRegister);
            this.groupBoxFunctions.Location = new System.Drawing.Point(180, 144);
            this.groupBoxFunctions.Name = "groupBoxFunctions";
            this.groupBoxFunctions.Size = new System.Drawing.Size(196, 110);
            this.groupBoxFunctions.TabIndex = 36;
            this.groupBoxFunctions.TabStop = false;
            this.groupBoxFunctions.Text = "Function";
            // 
            // radioButtonCoilStatus
            // 
            this.radioButtonCoilStatus.Location = new System.Drawing.Point(6, 19);
            this.radioButtonCoilStatus.Name = "radioButtonCoilStatus";
            this.radioButtonCoilStatus.Size = new System.Drawing.Size(184, 21);
            this.radioButtonCoilStatus.TabIndex = 4;
            this.radioButtonCoilStatus.Tag = "CoilStatus";
            this.radioButtonCoilStatus.Text = "01 Coil Status (0x)";
            this.radioButtonCoilStatus.CheckedChanged += new System.EventHandler(this.RadioButtonFunctionCheckedChanged);
            // 
            // radioButtonInputStatus
            // 
            this.radioButtonInputStatus.Location = new System.Drawing.Point(6, 40);
            this.radioButtonInputStatus.Name = "radioButtonInputStatus";
            this.radioButtonInputStatus.Size = new System.Drawing.Size(184, 21);
            this.radioButtonInputStatus.TabIndex = 3;
            this.radioButtonInputStatus.Tag = "InputStatus";
            this.radioButtonInputStatus.Text = "02 Input Status (1x)";
            this.radioButtonInputStatus.CheckedChanged += new System.EventHandler(this.RadioButtonFunctionCheckedChanged);
            // 
            // radioButtonInputRegisters
            // 
            this.radioButtonInputRegisters.Location = new System.Drawing.Point(6, 81);
            this.radioButtonInputRegisters.Name = "radioButtonInputRegisters";
            this.radioButtonInputRegisters.Size = new System.Drawing.Size(184, 21);
            this.radioButtonInputRegisters.TabIndex = 6;
            this.radioButtonInputRegisters.Tag = "InputRegister";
            this.radioButtonInputRegisters.Text = "04 Input Register (3x)";
            this.radioButtonInputRegisters.CheckedChanged += new System.EventHandler(this.RadioButtonFunctionCheckedChanged);
            // 
            // radioButtonHoldingRegister
            // 
            this.radioButtonHoldingRegister.Checked = true;
            this.radioButtonHoldingRegister.Location = new System.Drawing.Point(6, 61);
            this.radioButtonHoldingRegister.Name = "radioButtonHoldingRegister";
            this.radioButtonHoldingRegister.Size = new System.Drawing.Size(184, 20);
            this.radioButtonHoldingRegister.TabIndex = 5;
            this.radioButtonHoldingRegister.TabStop = true;
            this.radioButtonHoldingRegister.Tag = "HoldingRegister";
            this.radioButtonHoldingRegister.Text = "03 Holding Register (4x)";
            this.radioButtonHoldingRegister.CheckedChanged += new System.EventHandler(this.RadioButtonFunctionCheckedChanged);
            // 
            // SlaveForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.ClientSize = new System.Drawing.Size(869, 887);
            this.Controls.Add(this.groupBoxFunctions);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.btnConnect);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SlaveForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SlaveFormClosing);
            this.Load += new System.EventHandler(this.SlaveFormLoading);
            this.Controls.SetChildIndex(this.grpExchange, 0);
            this.Controls.SetChildIndex(this.groupBox3, 0);
            this.Controls.SetChildIndex(this.grpStart, 0);
            this.Controls.SetChildIndex(this.groupBox4, 0);
            this.Controls.SetChildIndex(this.btnConnect, 0);
            this.Controls.SetChildIndex(this.buttonDisconnect, 0);
            this.Controls.SetChildIndex(this.groupBoxFunctions, 0);
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

        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.GroupBox groupBoxFunctions;
        private System.Windows.Forms.RadioButton radioButtonCoilStatus;
        private System.Windows.Forms.RadioButton radioButtonInputStatus;
        private System.Windows.Forms.RadioButton radioButtonInputRegisters;
        private System.Windows.Forms.RadioButton radioButtonHoldingRegister;
    }
}
