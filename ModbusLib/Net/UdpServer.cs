using System.Net;
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
    /// Concrete implementation of a UDP-listener
    /// </summary>
    internal class UdpServer
        : IpServer
    {
        public UdpServer(
            Socket port,
            IProtocol protocol)
            : base(port, protocol)
        {
        }



        /// <summary>
        /// Running thread handler
        /// </summary>
        protected override void Worker()
        {
            //loop, until the host closes
            while (_closing == false)
            {
                //look for incoming data
                int length = Port.Available;

                if (length > 0)
                {
                    var buffer = new byte[length];
                    EndPoint remote = new IPEndPoint(IPAddress.Any, 0);

                    //read the data from the physical port
                    Port.ReceiveFrom(
                        buffer,
                        ref remote);
                    Protocol.OnIncommingData(buffer);
                    //try to decode the incoming data
                    var data = new ServerCommData(Protocol) {IncomingData = new ByteArrayReader(buffer)};

                    var result = Protocol
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

                        Port.SendTo(
                            outgoing,
                            remote);
                        Protocol.OnOutgoingData(outgoing); 
                    }
                }

                Thread.Sleep(100);
            }
        }
    }

}
