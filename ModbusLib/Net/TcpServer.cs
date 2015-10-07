using System.Diagnostics;
using System.Net.Sockets;
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
    /// Concrete implementation of a TCP-listener
    /// </summary>
    internal class TcpServer
        : IpServer
    {
        public TcpServer(
            Socket port,
            IProtocol protocol)
            : base(port, protocol)
        {
        }



        private const int CacheSize = 300;

        internal int IdleTimeout = 60;

        /// <summary>
        /// Running thread handler
        /// </summary>
        protected override void Worker()
        {
            Debug.Print("start");
            //start the local timer, which gets the session dying
            int counter = IdleTimeout;

            using (var timer = new Timer(_ => counter--, null, 1000, 1000))
            {
                //create a writer for the incoming data
                ByteArrayWriter writer = null;
                var buffer = new byte[CacheSize];
                //loop, until the host closes, or the timer expires
                while (_closing == false && counter > 0)
                {
                    //look for incoming data
                    int length = Port.Available;
                    if (length > 0)
                    {
                        if (length > CacheSize)
                            length = CacheSize;
                        //read the data from the physical port
                        Port.Receive(buffer, length, SocketFlags.None);
                        Protocol.OnIncommingData(buffer);
                        //append the data to the writer
                        if (writer == null)
                            writer = new ByteArrayWriter();
                        writer.WriteBytes(buffer, 0, length);
                        //try to decode the incoming data
                        var data = new ServerCommData(Protocol);
                        data.IncomingData = writer.ToReader();
                        var result = Protocol.Codec.ServerDecode(data);
                        switch (result.Status)
                        {
                            case CommResponse.Ack:
                                {
                                    //the command is recognized, so call the host back
                                    OnServeCommand(data);
                                    //encode the host data
                                    Protocol.Codec.ServerEncode(data);
                                    //return the resulting data to the remote caller
                                    if (data.OutgoingData != null)
                                    {
                                        byte[] outgoing = data.OutgoingData.ToArray();
                                        Port.Send(outgoing);
                                        Protocol.OnOutgoingData(outgoing);                                        
                                    }

                                    //reset the timer
                                    counter = IdleTimeout;
                                    writer = null;
                                }
                                break;
                            case CommResponse.Ignore:
                                writer = null;
                                break;
                        }
                    }
                    Thread.Sleep(100);
                }
            }
            Port.Close();
            Debug.Print("close");
        }
    }

}
