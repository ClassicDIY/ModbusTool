using System.IO.Ports;
using System.Threading;
using ModbusLib.Protocols;

/*
 * Copyright 2012 Mario Vernari (http://cetdevelop.codeplex.com/)
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace ModbusLib
{
    /// <summary>
    /// Concrete implementation of a serial port-listener
    /// </summary>
    internal class SerialPortServer
        : ICommServer
    {
        public SerialPortServer(
            SerialPort port,
            IProtocol protocol)
        {
            Port = port;
            Protocol = protocol;
        }



        public readonly SerialPort Port;
        public readonly IProtocol Protocol;

        private Thread _thread;
        protected bool _closing;



        /// <summary>
        /// Start the listener session
        /// </summary>
        public void Start()
        {
            _thread = new Thread(Worker);
            _thread.Start();
        }



        /// <summary>
        /// Running thread handler
        /// </summary>
        private void Worker()
        {
            //create a writer for the incoming data
            ByteArrayWriter writer = null;

            //loop, until the host closes
            while (_closing == false)
            {
                //look for incoming data
                int length = Port.BytesToRead;
                if (length > 0)
                {
                    var buffer = new byte[length];

                    //read the data from the physical port
                    Port.Read(
                        buffer,
                        0,
                        length);
                    Protocol.OnIncommingData(buffer);
                    //append the data to the writer
                    if (writer == null)
                        writer = new ByteArrayWriter();

                    writer.WriteBytes(
                        buffer,
                        0,
                        length);

                    //try to decode the incoming data
                    var data = new ServerCommData(Protocol);
                    data.IncomingData = writer.ToReader();

                    CommResponse result = Protocol
                        .Codec
                        .ServerDecode(data);

                    if (result.Status == CommResponse.Ack)
                    {
                        //the command is recognized, so call the host back
                        OnServeCommand(data);

                        //encode the host data
                        Protocol
                            .Codec
                            .ServerEncode(data);

                        //return the resulting data to the remote caller
                        byte[] outgoing = data
                            .OutgoingData
                            .ToArray();

                        Port.Write(
                            outgoing,
                            0,
                            outgoing.Length);
                        Protocol.OnOutgoingData(outgoing); 
                        writer = null;
                    }
                    else if (result.Status == CommResponse.Ignore)
                    {
                        writer = null;
                    }
                }

                Thread.Sleep(100);
            }
        }



        /// <summary>
        /// Close/abort the listener session
        /// </summary>
        public void Abort()
        {
            _closing = true;

            if (_thread != null &&
                _thread.IsAlive)
            {
                _thread.Join();
            }
        }



        #region EVT ServeCommand

        public event ServeCommandHandler ServeCommand;


        protected virtual void OnServeCommand(ServerCommData data)
        {
            var handler = ServeCommand;

            if (handler != null)
            {
                handler(
                    this,
                    new ServeCommandEventArgs(data));
            }
        }

        #endregion

    }
}
