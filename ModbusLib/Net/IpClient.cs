using System;
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
    /// Implementation of a socket client (either TCP, or UDP)
    /// </summary>
    public class IpClient
        : ICommClient
    {
        public IpClient(Socket port)
        {
            Port = port;
        }


        public readonly Socket Port;

        public int Latency { get; set; }
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public delegate void ResponseData(CommResponse response);
        /// <summary>Response data event. This event is called when new data arrives</summary>
        public event ResponseData OnResponseData;
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public delegate void ExceptionData(ushort id, byte function, byte exception);
        /// <summary>Exception data event. This event is called when the data is incorrect</summary>
        public event ExceptionData OnException;


        /// <summary>
        /// Entry-point for submitting a query to the remote device
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public CommResponse Query(ClientCommData data)
        {
            lock (Port)
            {
                //convert the request data as an ordinary byte array
                byte[] outgoing = data.OutgoingData.ToArray();

                //create a writer for accumulate the incoming data
                var incoming = new ByteArrayWriter();

                const int tempSize = 256;
                var temp = new byte[tempSize];

                //retries loop
                for (int attempt = 0, retries = data.Retries; attempt < retries; attempt++)
                {
                    //phyiscal writing
                    Port.Send(outgoing);
                    incoming.Reset();

                    //start the local timer
                    bool timeoutExpired;
                    int totalTimeout = Latency + data.Timeout;

                    using (new Timer(_ => timeoutExpired = true, null, totalTimeout, Timeout.Infinite))
                    {
                        //reception loop, until a valid response or timeout
                        timeoutExpired = false;
                        while (timeoutExpired == false)
                        {
                            var length = Port.Available;

                            if (length > 0)
                            {
                                if (length > tempSize)
                                    length = tempSize;

                                //read the incoming data from the physical port
                                Port.Receive(temp, length, SocketFlags.None);

                                //append data to the writer
                                incoming.WriteBytes(temp, 0, length);

                                //try to decode the stream
                                data.IncomingData = incoming.ToReader();

                                CommResponse result = data.OwnerProtocol.Codec.ClientDecode(data);

                                //exit whether any concrete result: either good or bad
                                if (result.Status == CommResponse.Ack)
                                {
                                    return result;
                                }
                                if (result.Status == CommResponse.Critical)
                                {
                                    return result;
                                }
                                if (result.Status != CommResponse.Unknown)
                                {
                                    break;
                                }
                            }

                            Thread.Sleep(0);

                            //stop immediately if the host asked to abort

                            //TODO
                        }
                    }   //using (timer)
                }       //for

                //no attempt was successful
                return new CommResponse(
                    data,
                    CommResponse.Critical);
            }   //lock
        }

        private byte[] tcpAsyClBuffer = new byte[2048];

        public void QueryAsync(ClientCommData data)
        {
            lock (Port)
            {
                //convert the request data as an ordinary byte array
                byte[] outgoing = data.OutgoingData.ToArray();

                //phyiscal writing
                Port.BeginSend(outgoing, 0, outgoing.Length, SocketFlags.None, OnSend, null);
                //read the incoming data from the physical port
                Port.BeginReceive(tcpAsyClBuffer, 0, tcpAsyClBuffer.Length, SocketFlags.None, OnReceive, data);
            }   //lock
        }

        private void OnReceive(IAsyncResult result)
        {
            if (result.IsCompleted == false) CallException(0xFF, 0xFF, ModbusCommand.ExceptionConnectionLost);

            //create a writer for accumulate the incoming data
            var incoming = new ByteArrayWriter();
            //append data to the writer
            incoming.WriteBytes(tcpAsyClBuffer, 0, tcpAsyClBuffer[8]+9);
            var data = (ClientCommData)result.AsyncState;
            //try to decode the stream
            data.IncomingData = incoming.ToReader();
            var resp = data.OwnerProtocol.Codec.ClientDecode(data);
            if (OnResponseData != null) OnResponseData(resp);
        }

        private void OnSend(IAsyncResult result)
        {
            if (result.IsCompleted == false) CallException(0xFFFF, 0xFF, ModbusCommand.SendFail);
        }

        internal void CallException(ushort id, byte function, byte exception)
        {
            if (OnException != null) OnException(id, function, exception);
        }
    }
}
