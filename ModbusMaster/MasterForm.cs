using System;
using System.IO.Ports;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Modbus.Common;
using ModbusLib;
using ModbusLib.Protocols;

namespace ModbusMaster
{
    public partial class MasterForm : BaseForm
    {
        private int _transactionId;
        private ModbusClient _driver;
        private ICommClient _portClient;
        private SerialPort _uart;

        #region Form

        public MasterForm()
        {
            InitializeComponent();
        }

        private void MasterFormClosing(object sender, FormClosingEventArgs e)
        {
            DoDisconnect();
        }

        #endregion

        #region Connect/disconnect

        private void DoDisconnect()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket.Dispose();
                _socket = null;
            }
            if (_uart != null)
            {
                _uart.Close();
                _uart.Dispose();
                _uart = null;
            }
            _portClient = null;
            _driver = null;
        }

        private void BtnConnectClick(object sender, EventArgs e)
        {
            try
            {
                switch (CommunicationMode)
                {
                    case CommunicationMode.RTU:
                        _uart = new SerialPort(PortName, Baud, Parity, DataBits, StopBits);
                        _uart.Open();
                        _portClient = _uart.GetClient();
                        _driver = new ModbusClient(new ModbusRtuCodec()) { Address = SlaveId };
                        _driver.OutgoingData += DriverOutgoingData;
                        _driver.IncommingData += DriverIncommingData;
                        AppendLog(String.Format("Connected using RTU to {0}", PortName));
                        break;

                    case CommunicationMode.UDP:
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                        _socket.Connect(new IPEndPoint(IPAddress, TCPPort));
                        _portClient = _socket.GetClient();
                        _driver = new ModbusClient(new ModbusTcpCodec()) { Address = SlaveId };
                        _driver.OutgoingData += DriverOutgoingData;
                        _driver.IncommingData += DriverIncommingData;
                        AppendLog(String.Format("Connected using UDP to {0}", _socket.RemoteEndPoint));
                        break;

                    case CommunicationMode.TCP:
                        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                        _socket.SendTimeout = 2000;
                        _socket.ReceiveTimeout = 2000;
                        _socket.Connect(new IPEndPoint(IPAddress, TCPPort));
                        _portClient = _socket.GetClient();
                        _driver = new ModbusClient(new ModbusTcpCodec()) { Address = SlaveId };
                        _driver.OutgoingData += DriverOutgoingData;
                        _driver.IncommingData += DriverIncommingData;
                        AppendLog(String.Format("Connected using TCP to {0}", _socket.RemoteEndPoint));
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
            groupBoxFunctions.Enabled = true;
            groupBoxTCP.Enabled = false;
            groupBoxRTU.Enabled = false;
            groupBoxMode.Enabled = false;
            grpExchange.Enabled = false;
        }

        private void ButtonDisconnectClick(object sender, EventArgs e)
        {
            DoDisconnect();
            btnConnect.Enabled = true;
            buttonDisconnect.Enabled = false;
            groupBoxFunctions.Enabled = false;
            groupBoxMode.Enabled = true;
            grpExchange.Enabled = true;
            SetMode();
            AppendLog("Disconnected");
        }

        #endregion

        #region Functions buttons

        private void BtnReadCoilsClick(object sender, EventArgs e)
        {
            ExecuteReadCommand(ModbusCommand.FuncReadCoils);
        }

        private void BtnReadDisInpClick(object sender, EventArgs e)
        {
            ExecuteReadCommand(ModbusCommand.FuncReadInputDiscretes);
        }

        private void BtnReadHoldRegClick(object sender, EventArgs e)
        {
            ExecuteReadCommand(ModbusCommand.FuncReadMultipleRegisters);
        }

        private void BtnReadInpRegClick(object sender, EventArgs e)
        {
            ExecuteReadCommand(ModbusCommand.FuncReadInputRegisters);
        }

        private void ExecuteReadCommand(byte function)
        {
            try
            {
                var command = new ModbusCommand(function) {Offset = StartAddress, Count = DataLength, TransId = _transactionId++};
                var result = _driver.ExecuteGeneric(_portClient, command);
                if (result.Status == CommResponse.Ack)
                {
                    command.Data.CopyTo(_registerData, StartAddress);
                    UpdateDataTable();
                    AppendLog(String.Format("Read succeeded: Function code:{0}.", function));
                }
                else
                {
                    AppendLog(String.Format("Failed to execute Read: Error code:{0}", result.Status));
                }
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        private void ExecuteWriteCommand(byte function)
        {
            try
            {
                var command = new ModbusCommand(function)
                                  {
                                      Offset = StartAddress,
                                      Count = DataLength,
                                      TransId = _transactionId++,
                                      Data = new ushort[DataLength]
                                  };
                for (int i = 0; i < DataLength; i++)
                {
                    var index = StartAddress + i;
                    if (index > _registerData.Length)
                    {
                        break;
                    }
                    command.Data[i] = _registerData[index];
                }
                var result = _driver.ExecuteGeneric(_portClient, command);
                AppendLog(result.Status == CommResponse.Ack
                              ? String.Format("Write succeeded: Function code:{0}", function)
                              : String.Format("Failed to execute Write: Error code:{0}", result.Status));
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }


        private void BtnWriteSingleCoilClick(object sender, EventArgs e)
        {
            try
            {
                var command = new ModbusCommand(ModbusCommand.FuncWriteCoil)
                {
                    Offset = StartAddress,
                    Count = 1,
                    TransId = _transactionId++,
                    Data = new ushort[1]
                };
                command.Data[0] = (ushort)(_registerData[StartAddress] & 0x0100);
                var result = _driver.ExecuteGeneric(_portClient, command);
                AppendLog(result.Status == CommResponse.Ack
                              ? String.Format("Write succeeded: Function code:{0}", ModbusCommand.FuncWriteCoil)
                              : String.Format("Failed to execute Write: Error code:{0}", result.Status));
            }
            catch (Exception ex)
            {
                AppendLog(ex.Message);
            }
        }

        private void BtnWriteSingleRegClick(object sender, EventArgs e)
        {
            ExecuteWriteCommand(ModbusCommand.FuncWriteSingleRegister);
        }

        private void BtnWriteMultipleCoilsClick(object sender, EventArgs e)
        {
            ExecuteWriteCommand(ModbusCommand.FuncForceMultipleCoils);
        }

        private void BtnWriteMultipleRegClick(object sender, EventArgs e)
        {
            ExecuteWriteCommand(ModbusCommand.FuncWriteMultipleRegisters);
        }

        private void ButtonReadExceptionStatusClick(object sender, EventArgs e)
        {

        }

        #endregion

    }
}
