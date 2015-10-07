using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ModbusLib.Protocols;

namespace ModbusSlave
{
    public class UDPBroadcaster
    {
        private Thread _thread;
        public bool _running = true;
        private int selectedPort;
        public void SendDatagrams(int p)
        {
            this.selectedPort = p;
            _thread = new Thread(Worker);
            _thread.Start();
        }
        public void Stop()
        {
            _running = false;

        }

        protected void Worker()
        {
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            try
            {
                UdpClient udpSocket = new UdpClient(4626);
                IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 4626);
                udpSocket.EnableBroadcast = true;
                udpSocket.DontFragment = true;
                udpSocket.MulticastLoopback = false;
                var add = Dns.GetHostAddresses(Dns.GetHostName()).First(a => a.AddressFamily == AddressFamily.InterNetwork);
                var bytes = add.GetAddressBytes();
                byte[] port = new byte[2];
                port[0] = (byte)(selectedPort & 0x00ff);
                port[1] = (byte)((selectedPort >> 8) & 0x00ff);
                byte[] buf = bytes.Concat(port).ToArray();
                while (_running)
                {
                    udpSocket.Send(buf, buf.Length, ip);
                    
                    Thread.Sleep(3000);
                }
                udpSocket.Close();
            }
            catch (Exception ex)
            {
                String msg = ex.Message;
            }

        }
    }
}
