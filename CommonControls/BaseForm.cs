using System;
using System.Configuration;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace Modbus.Common
{
    public partial class BaseForm : Form
    {
        private DisplayFormat _displayFormat = DisplayFormat.Integer;
        private CommunicationMode _communicationMode = CommunicationMode.TCP;
        protected Socket _socket;
        protected readonly ushort[] _registerData;
        private bool _logPaused = false;

        #region Form 

        public BaseForm()
        {
            InitializeComponent();
            _registerData = new ushort[65600];
        }

        private void BaseFormLoading(object sender, EventArgs e)
        {
            comboBoxBaudRate.SelectedIndex = 4;
            FillRTUDropDownLists();
            CurrentTab.RegisterData = _registerData;
            if (_registerData == null)
            {
                throw new ApplicationException("Failed to allocate 128k block");
            }
            LoadUserData();
            CurrentTab.DisplayFormat = DisplayFormat;
            RefreshData();
        }

        private void BaseFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserData();
        }

        private void FillRTUDropDownLists()
        {
            comboBoxSerialPorts.Items.Clear();
            foreach (var port in SerialPort.GetPortNames())
            {
                comboBoxSerialPorts.Items.Add(port);
            }
            if (comboBoxSerialPorts.Items.Count > 0)
                comboBoxSerialPorts.SelectedIndex = 0;
            comboBoxParity.Items.Clear();
            comboBoxParity.Items.Add(Parity.None.ToString());
            comboBoxParity.Items.Add(Parity.Odd.ToString());
            comboBoxParity.Items.Add(Parity.Even.ToString());
            comboBoxParity.Items.Add(Parity.Mark.ToString());
            comboBoxParity.Items.Add(Parity.Space.ToString());
        }

        private void LoadUserData()
        {
            CommunicationMode mode;
            if (Enum.TryParse(Properties.Settings.Default.CommunicationMode, out mode))
                CommunicationMode = mode;
            DisplayFormat format;
            if (Enum.TryParse(Properties.Settings.Default.DisplayFormat, out format))
                DisplayFormat = format;
            IPAddress ipAddress;
            if (IPAddress.TryParse(Properties.Settings.Default.IPAddress, out ipAddress))
                IPAddress = ipAddress;
            TCPPort = Properties.Settings.Default.TCPPort;
            PortName = Properties.Settings.Default.PortName;
            Baud = Properties.Settings.Default.Baud;
            Parity = Properties.Settings.Default.Parity;
            StartAddress = Properties.Settings.Default.StartAddress;
            DataLength = Properties.Settings.Default.DataLength;
            SlaveId = Properties.Settings.Default.SlaveId;
            SlaveDelay = Properties.Settings.Default.SlaveDelay;
            DataBits = Properties.Settings.Default.DataBits;
            StopBits = Properties.Settings.Default.StopBits;
        }

        private void SaveUserData()
        {
            Properties.Settings.Default.CommunicationMode = CommunicationMode.ToString();
            Properties.Settings.Default.IPAddress = IPAddress.ToString();
            Properties.Settings.Default.DisplayFormat = DisplayFormat.ToString();
            Properties.Settings.Default.TCPPort = TCPPort;
            Properties.Settings.Default.PortName = PortName;
            Properties.Settings.Default.Baud = Baud;
            Properties.Settings.Default.Parity = Parity;
            Properties.Settings.Default.StartAddress = StartAddress;
            Properties.Settings.Default.DataLength = DataLength;
            Properties.Settings.Default.SlaveId = SlaveId;
            Properties.Settings.Default.SlaveDelay = SlaveDelay;
            Properties.Settings.Default.DataBits = DataBits;
            Properties.Settings.Default.StopBits = StopBits;
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Import - Export

        private void ButtonImportClick(object sender, EventArgs e)
        {
            openFileDialog.AddExtension = true;
            openFileDialog.DefaultExt = ".csv";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var s = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var r = new StreamReader(s))
                    {
                        var rec = r.ReadToEnd();
                        var sets = rec.Split(',');
                        var first = true;
                        foreach (var s1 in sets)
                        {
                            DisplayFormat fmt;
                            var v = s1.Split(':');
                            var address = int.Parse(v[0]);
                            ushort data;
                            if (v[1].StartsWith("0x", StringComparison.CurrentCultureIgnoreCase))
                            {
                                fmt = DisplayFormat.Hex;
                                var sub = v[1].Substring(2);
                                ushort.TryParse(sub, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out data);
                            }
                            else if (v[1].Length > 6) //must be binary
                            {
                                fmt = DisplayFormat.Binary;
                                data = Convert.ToUInt16(v[1], 2);
                            }
                            else
                            {
                                fmt = DisplayFormat.Integer;
                                data = Convert.ToUInt16(v[1], 10);
                            }
                            if (address < _registerData.Length)
                            {
                                _registerData[address] = data;
                            }
                            if (first)
                            {
                                SetFunction(fmt);
                                first = false;
                                StartAddress = UInt16.Parse(v[0]);
                            }
                        }
                        r.Close();
                        DataLength = Convert.ToUInt16(sets.Length);
                        // display data
                    }
                    s.Close();
                }
                RefreshData();
            }
        }

        public delegate void SetFunctionDelegate(DisplayFormat log);

        protected void SetFunction(DisplayFormat fmt)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new SetFunctionDelegate(SetFunction), new object[] { fmt });
                return;
            }
            DisplayFormat = fmt;
            switch (fmt)
            {
                case DisplayFormat.Integer:
                    radioButtonInteger.Checked = true;
                    break;
                case DisplayFormat.Binary:
                    radioButtonBinary.Checked = true;
                    break;
                case DisplayFormat.Hex:
                    radioButtonHex.Checked = true;
                    break;
                case DisplayFormat.LED:
                    radioButtonLED.Checked = true;
                    break;
                case DisplayFormat.FloatReverse:
                    radioButtonReverseFloat.Checked = true;
                    break;
            }
        }

        private void ButtonExportClick(object sender, EventArgs e)
        {
            var startAddress = StartAddress;
            var length = DataLength;
            string suffix = "-";
            switch (DisplayFormat)
            {
                case DisplayFormat.Integer:
                    suffix = "_Decimal_";
                    break;
                case DisplayFormat.Hex:
                    suffix = "_HEX_";
                    break;
                case DisplayFormat.Binary:
                    suffix = "_Binary_";
                    break;
                case DisplayFormat.LED:
                    suffix = "_LED_";
                    break;
            }
            var filename = "ModbusExport_" + startAddress + suffix + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv";
            saveFileDialog.AddExtension = true;
            saveFileDialog.DefaultExt = ".csv";
            saveFileDialog.FileName = filename;
            saveFileDialog.OverwritePrompt = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var s = saveFileDialog.OpenFile())
                {
                    using (var w = new StreamWriter(s))
                    {
                        for (var x = 0; x < length; x++)
                        {
                            w.Write(startAddress++);
                            w.Write(':');
                            var data = _registerData[StartAddress + x];
                            switch (DisplayFormat)
                            {
                                case DisplayFormat.Integer:
                                    w.Write(string.Format("{0}", data));
                                    break;
                                case DisplayFormat.Hex:
                                    w.Write(string.Format("0x{0:x4}", data));
                                    break;
                                case DisplayFormat.Binary:
                                case DisplayFormat.LED:
                                    w.Write(Convert.ToString(data, 2).PadLeft(16, '0'));
                                    break;
                            }
                            if (x < length - 1)
                                w.Write(',');
                        }
                        w.Flush();
                        w.Close();
                    }
                    s.Close();
                }
            }
        }

        #endregion

        #region Radion button check handlers

        private void RadioButtonModeChanged(object sender, EventArgs e)
        {
            SetMode();
        }

        protected void SetMode()
        {
            if (radioButtonTCP.Checked)
            {
                _communicationMode = CommunicationMode.TCP;
                groupBoxTCP.Enabled = true;
                groupBoxRTU.Enabled = false;
            }
            if (radioButtonRTU.Checked)
            {
                _communicationMode = CommunicationMode.RTU;
                groupBoxTCP.Enabled = false;
                groupBoxRTU.Enabled = true;
            }
            if (radioButtonUDP.Checked)
            {
                _communicationMode = CommunicationMode.UDP;
                groupBoxTCP.Enabled = true;
                groupBoxRTU.Enabled = false;
            }
        }

        private void RadioButtonDisplayFormatCheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton)
            {
                var rb = (RadioButton)sender;
                if (rb.Checked)
                {
                    DisplayFormat.TryParse(rb.Tag.ToString(), true, out _displayFormat);
                    CurrentTab.DisplayFormat = DisplayFormat;
                    RefreshData();
                }
            }
        }

        #endregion

        #region properties

        private ushort _startAddress;
        protected ushort StartAddress
        {
            get
            {
                return _startAddress;
            }
            set
            {
                CurrentTab.StartAddress = value;
                var tab = tabControl1.SelectedTab;
                tab.Text = value.ToString();
                _startAddress = value;
            }
        }

        private ushort _dataLength;
        private bool showDataLength;

        protected ushort DataLength
        {
            get
            {
                return _dataLength;
            }
            set
            {
                _dataLength = value;
                CurrentTab.DataLength = value;
            }
        }

        public bool ShowDataLength
        {
            get => showDataLength;
            set
            {
                showDataLength = value;
                foreach (DataTab tab in tabPage1.Controls)
                {
                    tab.ShowDataLength = value;
                }
                foreach (DataTab tab in tabPage2.Controls)
                {
                    tab.ShowDataLength = value;
                }
            }
        }
        protected IPAddress IPAddress
        {
            get
            {
                return IPAddress.Parse(txtIP.Text);
            }
            set
            {
                txtIP.Text = value.ToString();
            }
        }

        protected int TCPPort
        {
            get
            {
                return Int32.Parse(textBoxPort.Text);
            }
            set
            {
                textBoxPort.Text = Convert.ToString(value);

            }
        }

        protected byte SlaveId
        {
            get
            {
                return Byte.Parse(textBoxSlaveID.Text);
            }
            set
            {
                textBoxSlaveID.Text = Convert.ToString(value);
            }
        }

        protected int SlaveDelay
        {
            get
            {
                return int.Parse(textBoxSlaveDelay.Text);
            }
            set
            {
                textBoxSlaveDelay.Text = Convert.ToString(value);
            }
        }

        protected string PortName
        {
            get
            {
                return comboBoxSerialPorts.Text;
            }
            set
            {
                comboBoxSerialPorts.Text = value;
            }
        }

        protected int Baud
        {
            get
            {
                return Int32.Parse(comboBoxBaudRate.Text);
            }
            set
            {
                comboBoxBaudRate.SelectedItem = Convert.ToString(value);
            }
        }

        protected Parity Parity
        {
            get
            {
                var parity = Parity.None;
                if (comboBoxParity.SelectedItem.Equals(Parity.None.ToString()))
                {
                    parity = Parity.None;
                }
                else if (comboBoxParity.SelectedItem.Equals(Parity.Odd.ToString()))
                {
                    parity = Parity.Odd;
                }
                else if (comboBoxParity.SelectedItem.Equals(Parity.Even.ToString()))
                {
                    parity = Parity.Even;
                }
                else if (comboBoxParity.SelectedItem.Equals(Parity.Mark.ToString()))
                {
                    parity = Parity.Mark;
                }
                else if (comboBoxParity.SelectedItem.Equals(Parity.Space.ToString()))
                {
                    parity = Parity.Space;
                }
                return parity;
            }
            set
            {
                comboBoxParity.SelectedItem = Convert.ToString(value);
            }
        }

        protected int DataBits
        {
            get
            {
                int bits = 0;
                switch (comboBoxDataBits.SelectedIndex)
                {
                    case 0:
                        bits = 7;
                        break;
                    case 1:
                        bits = 8;
                        break;
                }
                return bits;
            }
            set
            {
                switch (value)
                {
                    case 7:
                        comboBoxDataBits.SelectedIndex = 0;
                        break;
                    case 8:
                        comboBoxDataBits.SelectedIndex = 1;
                        break;
                }
            }
        }

        protected StopBits StopBits
        {
            get
            {
                StopBits bits = StopBits.None;
                switch (comboBoxStopBits.SelectedIndex)
                {
                    case 0:
                        bits = StopBits.None;
                        break;
                    case 1:
                        bits = StopBits.One;
                        break;
                    case 2:
                        bits = StopBits.OnePointFive;
                        break;
                    case 3:
                        bits = StopBits.Two;
                        break;
                }
                return bits;
            }
            set
            {
                switch (value)
                {
                    case StopBits.None:
                        comboBoxStopBits.SelectedIndex = 0;
                        break;
                    case StopBits.One:
                        comboBoxStopBits.SelectedIndex = 1;
                        break;
                    case StopBits.OnePointFive:
                        comboBoxStopBits.SelectedIndex = 2;
                        break;
                    case StopBits.Two:
                        comboBoxStopBits.SelectedIndex = 3;
                        break;
                }
            }
        }

        protected DisplayFormat DisplayFormat
        {
            get { return _displayFormat; }
            set
            {
                switch (value)
                {
                    case DisplayFormat.LED:
                        radioButtonLED.Checked = true;
                        break;
                    case DisplayFormat.Binary:
                        radioButtonBinary.Checked = true;
                        break;
                    case DisplayFormat.Hex:
                        radioButtonHex.Checked = true;
                        break;
                    case DisplayFormat.Integer:
                        radioButtonInteger.Checked = true;
                        break;
                    case DisplayFormat.FloatReverse:
                        radioButtonReverseFloat.Checked = true;
                        break;
                }
                _displayFormat = value;
                CurrentTab.DisplayFormat = DisplayFormat;
                RefreshData();
            }
        }

        protected CommunicationMode CommunicationMode
        {
            get { return _communicationMode; }
            set
            {
                switch (value)
                {
                    case CommunicationMode.TCP:
                        radioButtonTCP.Checked = true;
                        break;
                    case CommunicationMode.UDP:
                        radioButtonUDP.Checked = true;
                        break;
                    case CommunicationMode.RTU:
                        radioButtonRTU.Checked = true;
                        break;
                }
                _communicationMode = value;
            }
        }

        #endregion

        #region Logging

        public delegate void AppendLogDelegate(String log);

        protected void ButtonClearLogClick(object sender, EventArgs e)
        {
            listBoxCommLog.Items.Clear();
        }

        private void buttonPauseLog_Click(object sender, EventArgs e)
        {
            _logPaused = !_logPaused;
            buttonPauseLog.Text = _logPaused ? "Resume" : "Pause";
        }

        private async void buttonSaveLog_ClickAsync(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "ModbusLog";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName, append: true))
                {
                    foreach (string line in listBoxCommLog.Items)
                    {
                        await writer.WriteLineAsync(line);
                    }
                }
            }
        }

        protected void DriverIncommingData(byte[] data, int len)
        {
            if (_logPaused)
                return;
            var hex = new StringBuilder(len);
            for(int i = 0; i < len; i++)
            {
                hex.AppendFormat("{0:x2} ", data[i]);
            }
            AppendLog(String.Format("RX: {0}", hex));
        }

        protected void DriverOutgoingData(byte[] data)
        {
            if (_logPaused)
                return;
            var hex = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
                hex.AppendFormat("{0:x2} ", b);
            AppendLog(String.Format("TX: {0}", hex));
        }

        protected void AppendLog(String log)
        {
            if (_logPaused)
                return;
            if (InvokeRequired)
            {
                BeginInvoke(new AppendLogDelegate(AppendLog), new object[] { log });
                return;
            }
            var now = DateTime.Now;
            var tmpStr = ">" + now.ToLongTimeString() + ": " + log;
            listBoxCommLog.Items.Add(tmpStr);
            listBoxCommLog.SelectedIndex = listBoxCommLog.Items.Count - 1;
            //listBoxCommLog.SelectedIndex = -1;
        }

        #endregion

        #region Data Table

        protected void ButtonDataClearClick(object sender, EventArgs e)
        {
            ClearRegisterData();
        }

        protected void ClearRegisterData()
        {
            for (int i = 0; i < _registerData.Length; i++)
            {
                _registerData[i] = 0;
            }
            RefreshData();
        }

        protected DataTab CurrentTab
        {
            get
            {
                var tab = tabControl1.SelectedTab;
                return (DataTab)tab.Controls[0];
            }
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RefreshData));
                return;
            }
            CurrentTab.RefreshData();

            //  Reset event handler
            CurrentTab.OnApply -= dataTab_OnApply;
            CurrentTab.OnApply += dataTab_OnApply;
        }

        public void UpdateDataTable()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateDataTable));
                return;
            }
            CurrentTab.UpdateDataTable();
        }

        #endregion

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            CurrentTab.RegisterData = _registerData;
            CurrentTab.DisplayFormat = DisplayFormat;
            var tab = tabControl1.SelectedTab;
            if (tab.Text.Equals("...") && tabControl1.TabPages.Count < 20)
            {
                DataTab dataTab = new DataTab();
                dataTab.DataLength = 256;
                dataTab.DisplayFormat = DisplayFormat.Integer;
                dataTab.Location = new Point(3, 3);
                dataTab.Name = "dataTab" + (tabControl1.TabPages.Count + 1);
                dataTab.RegisterData = _registerData;
                dataTab.ShowDataLength = ShowDataLength;
                dataTab.Size = new Size(839, 406);
                dataTab.StartAddress = 0;
                dataTab.TabIndex = 0;
                dataTab.OnApply += dataTab_OnApply;
                TabPage tabPage = new TabPage();
                tabPage.Controls.Add(dataTab);
                tabPage.Location = new Point(4, 22);
                tabPage.Name = "tabPage" + (tabControl1.TabPages.Count + 1);
                tabPage.Padding = new Padding(3);
                tabPage.Size = new Size(851, 411);
                tabPage.TabIndex = tabControl1.TabPages.Count;
                tabPage.Text = "...";
                tabPage.UseVisualStyleBackColor = true;
                tabControl1.Controls.Add(tabPage);
            }
            var address = CurrentTab.StartAddress;
            tab.Text = address.ToString();
            _startAddress = address;
            _dataLength = CurrentTab.DataLength;
        }

        void dataTab_OnApply(object sender, EventArgs e)
        {
            var tab = tabControl1.SelectedTab;
            var address = CurrentTab.StartAddress;
            tab.Text = address.ToString();
            _startAddress = address;
            _dataLength = CurrentTab.DataLength;
        }

        private void donate_Click(object sender, EventArgs e)
        {
            string url = "https://www.buymeacoffee.com/r4K2HIB";
            System.Diagnostics.Process.Start(url);
        }

    }
}
