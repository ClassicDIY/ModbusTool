using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Modbus.Common;
using ModbusLib;
using ModbusLib.Protocols;

namespace ModbusSlave
{
    public partial class SlaveForm : BaseForm
    {
                    
        private Function _function = Function.HoldingRegister;
        private ICommServer _listener;
        private SerialPort _uart;
        private Thread _thread;

        #region Form
        
        public SlaveForm()
        {
            base.ShowDataLength = false;
            InitializeComponent();
        }

        private void SlaveFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveUserData();
            DoDisconnect();
        }

        private void SlaveFormLoading(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            FunctionCode = Modbus.Common.Properties.Settings.Default.Function;
        }

        private void SaveUserData()
        {
            Modbus.Common.Properties.Settings.Default.Function = FunctionCode;
            Modbus.Common.Properties.Settings.Default.Save();
        }

        #endregion

        #region Connect/disconnect

        private void BtnConnectClick(object sender, EventArgs e)
        {
            try
            {
                switch (CommunicationMode)
                {
                    case CommunicationMode.RTU:
                        _uart = new SerialPort(PortName, Baud, Parity, DataBits, StopBits);
                        _uart.Open();
                        var rtuServer = new ModbusServer(new ModbusRtuCodec()) { Address = SlaveId };
                        rtuServer.OutgoingData += DriverOutgoingData;
                        rtuServer.IncommingData += DriverIncommingData;
                        _listener = _uart.GetListener(rtuServer);
                        _listener.ServeCommand += listener_ServeCommand;
                        _listener.Start();
                        AppendLog(String.Format("Connected using RTU to {0}", PortName));
                        break;

                    case CommunicationMode.UDP:
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        _socket.Bind(new IPEndPoint(IPAddress.Any, TCPPort));
                        //create a server driver
                        var udpServer = new ModbusServer(new ModbusTcpCodec()) { Address = SlaveId };
                        udpServer.OutgoingData += DriverOutgoingData;
                        udpServer.IncommingData += DriverIncommingData;
                        //listen for an incoming request
                        _listener = _socket.GetUdpListener(udpServer);
                        _listener.ServeCommand += listener_ServeCommand;
                        _listener.Start();
                        AppendLog(String.Format("Listening to UDP port {0}", TCPPort));
                        break;

                    case CommunicationMode.TCP:
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _socket.Bind(new IPEndPoint(IPAddress.Any, TCPPort));
                        _socket.Listen(10);
                        //create a server driver
                        _thread = new Thread(Worker);
                        _thread.Start();
                        AppendLog(String.Format("Listening to TCP port {0}", TCPPort));
                        break;
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
                return;
            }
            btnConnect.Enabled = false;
            buttonDisconnect.Enabled = true;
            grpExchange.Enabled = true;
            groupBoxFunctions.Enabled = false;
            groupBoxTCP.Enabled = false;
            groupBoxRTU.Enabled = false;
            groupBoxMode.Enabled = false;
        }

        /// <summary>
        /// Running thread handler
        /// </summary>
        protected void Worker()
        {
            var server = new ModbusServer(new ModbusTcpCodec()) { Address = SlaveId };
            server.IncommingData += DriverIncommingData;
            server.OutgoingData += DriverOutgoingData;
            try
            {
                while (_thread.ThreadState == ThreadState.Running)
                {
                    //wait for an incoming connection
                    _listener = _socket.GetTcpListener(server);
                    _listener.ServeCommand += listener_ServeCommand;
                    _listener.Start();
                    AppendLog(String.Format("Accepted connection."));
                    Thread.Sleep(1);
                }
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
            }

        }

        private void ButtonDisconnectClick(object sender, EventArgs e)
        {
            DoDisconnect();
            btnConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
            grpExchange.Enabled = true;
            groupBoxMode.Enabled = true;
            groupBoxFunctions.Enabled = true;
            SetMode();
            AppendLog("Disconnected");
        }

        private void DoDisconnect()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(DoDisconnect));
                return;
            }
            if (_listener != null)
            {
                _listener.Abort();
                _listener = null;
            }
            if (_uart != null)
            {
                _uart.Close();
                _uart.Dispose();
                _uart = null;
            }
            if (_thread != null && _thread.IsAlive)
            {
                if (_thread.Join(2000) == false)
                {
                    _thread.Abort();
                    _thread = null;
                }
            }
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }

        #endregion

        #region Listen functions

        void listener_ServeCommand(object sender, ServeCommandEventArgs e)
        {
            var command = (ModbusCommand)e.Data.UserData;

            Thread.Sleep(SlaveDelay);

            //take the proper function command handler
            switch (command.FunctionCode)
            {
                case ModbusCommand.FuncReadCoils:
                    if (_function == Function.CoilStatus)
                        DoRead(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncReadInputDiscretes:
                    if (_function == Function.InputStatus)
                        DoRead(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncReadInputRegisters:
                    if (_function == Function.InputRegister)
                        DoRead(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncReadMultipleRegisters:
                    if (_function == Function.HoldingRegister)
                        DoRead(command);
                    else
                        IllegalFunction(command);
                    break;

                case ModbusCommand.FuncWriteCoil:
                    if (_function == Function.CoilStatus)
                        DoWrite(command);
                    else
                        IllegalFunction(command);
                    break;

                case ModbusCommand.FuncForceMultipleCoils:
                    if (_function == Function.CoilStatus)
                        DoWrite(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncWriteMultipleRegisters:
                    if (_function == Function.HoldingRegister)
                        DoWrite(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncWriteSingleRegister:
                    if (_function == Function.HoldingRegister)
                        DoWrite(command);
                    else
                        IllegalFunction(command);
                    break;
                case ModbusCommand.FuncReadExceptionStatus:
                    //TODO
                    break;

                case ModbusCommand.FuncReadCustom:
                    if (_function == Function.HoldingRegister)
                    {
                        for (int i = 0; i < command.Count; i++)
                            command.Data[i] = _registerData[command.Offset + i];
                    }
                    else
                        IllegalFunction(command);
                    break;

                default:
                    //return an exception
                    command.ExceptionCode = ModbusCommand.ErrorIllegalFunction;
                    break;
            }
        }

        private void IllegalFunction(ModbusCommand command)
        {
            AppendLog(String.Format("Illegal Function, expecting function code {0}.", FunctionCode));
            command.ExceptionCode = ModbusCommand.ErrorIllegalFunction;
        }

        private byte FunctionCode
        {
            get
            {
                byte rVal = ModbusCommand.ErrorIllegalFunction;
                switch (_function)
                {
                    case Function.CoilStatus:
                        rVal = ModbusCommand.FuncReadCoils;
                        break;
                    case Function.HoldingRegister:
                        rVal = ModbusCommand.FuncReadMultipleRegisters;
                        break;
                    case Function.InputRegister:
                        rVal = ModbusCommand.FuncReadInputRegisters;
                        break;
                    case Function.InputStatus:
                        rVal = ModbusCommand.FuncReadInputDiscretes;
                        break;

                }
                return rVal;
            }
            set
            {
                switch (value)
                {
                    case ModbusCommand.FuncReadCoils:
                        radioButtonCoilStatus.Checked = true;
                        _function = Function.CoilStatus;
                        break;
                    case ModbusCommand.FuncReadInputDiscretes:
                        radioButtonInputStatus.Checked = true;
                        _function = Function.InputStatus;
                        break;
                    case ModbusCommand.FuncReadInputRegisters:
                        radioButtonInputRegisters.Checked = true;
                        _function = Function.InputRegister;
                        break;
                    case ModbusCommand.FuncReadMultipleRegisters:
                        radioButtonHoldingRegister.Checked = true;
                        _function = Function.HoldingRegister;
                        break;
                }
            }
        }


        private void DoRead(ModbusCommand command)
        {
            for (int i = 0; i < command.Count; i++)
                command.Data[i] = _registerData[command.Offset + i];
            AppendLog(String.Format("Sent data: Function code:{0}.", command.FunctionCode));

        }

        private void DoWrite(ModbusCommand command)
        {
            var dataAddress = command.Offset;
            if (dataAddress < StartAddress || dataAddress > StartAddress + DataLength)
            {
                AppendLog(String.Format("Received address is not within viewable range, Received address:{0}.", dataAddress));
                return;
            }
            if ((command.Count + dataAddress) > _registerData.Length)
            {
                AppendLog(String.Format("Received address is not within viewable range, Received address:{0}.", dataAddress));
                return;
            }
            command.Data.CopyTo(_registerData, dataAddress);
            UpdateDataTable();
            AppendLog(String.Format("Received data: Function code:{0}.", command.FunctionCode));
        }

        #endregion

        #region Radion button check handlers

        private void RadioButtonFunctionCheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton)
            {
                var rb = (RadioButton)sender;
                if (rb.Checked)
                {
                    Function.TryParse(rb.Tag.ToString(), true, out _function);
                    ClearRegisterData();
                }
            }
        }

        #endregion

    }
}
