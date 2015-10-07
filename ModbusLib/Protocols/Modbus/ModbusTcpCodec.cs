
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

/**
 * 09/Apr/2012:
 *  The original class ModbusTcpCodec has been splitted in two parts,
 *  a base and its derivation. In this way, the base ModbusCodecBase
 *  can be shared for both TCP and RTU mode
 **/

using System;

namespace ModbusLib.Protocols
{
    public class ModbusTcpCodec
        : ModbusCodecBase, IProtocolCodec
    {
        #region IProtocolCodec members

        void IProtocolCodec.ClientEncode(CommDataBase data)
        {
            var client = (ModbusClient)data.OwnerProtocol;
            var command = (ModbusCommand)data.UserData;
            var fncode = command.FunctionCode;

            //encode the command body, if applies
            var body = new ByteArrayWriter();
            var codec = CommandCodecs[fncode];
            if (codec != null)
                codec.ClientEncode(command, body);

            //calculate length field
            var length = 2 + body.Length;

            //create a writer for the outgoing data
            var writer = new ByteArrayWriter();

            //transaction-id 
            writer.WriteUInt16BE((ushort)command.TransId);

            //protocol-identifier (always zero)
            writer.WriteInt16BE(0);

            //message length
            writer.WriteInt16BE((short)length);

            //unit identifier (address)
            writer.WriteByte(client.Address);

            //function code
            writer.WriteByte(fncode);

            //body data
            writer.WriteBytes(body);

            data.OutgoingData = writer.ToReader();
        }



        CommResponse IProtocolCodec.ClientDecode(CommDataBase data)
        {
            var client = (ModbusClient)data.OwnerProtocol;
            var command = (ModbusCommand)data.UserData;
            var incoming = data.IncomingData;

            //validate header first
            if (incoming.Length >= 6 &&
                incoming.ReadUInt16BE() == (ushort)command.TransId &&
                incoming.ReadInt16BE() == 0     //protocol-identifier
                )
            {
                //message length
                var length = incoming.ReadInt16BE();

                //validate address
                if (incoming.Length >= (length + 6) &&
                    incoming.ReadByte() == client.Address
                    )
                {
                    //validate function code
                    var fncode = incoming.ReadByte();

                    if ((fncode & 0x7F) == command.FunctionCode)
                    {
                        if (fncode <= 0x7F)
                        {
                            //
                            var body = new ByteArrayReader(incoming.ReadToEnd());

                            //encode the command body, if applies
                            var codec = CommandCodecs[fncode];
                            if (codec != null)
                                codec.ClientDecode(command, body);

                            return new CommResponse(
                                data,
                                CommResponse.Ack);
                        }
                        else
                        {
                            //exception
                            command.ExceptionCode = incoming.ReadByte();

                            return new CommResponse(
                                data,
                                CommResponse.Critical);
                        }
                    }
                }
            }

            return new CommResponse(
                data,
                CommResponse.Unknown);
        }



        void IProtocolCodec.ServerEncode(CommDataBase data)
        {
            var server = (ModbusServer)data.OwnerProtocol;
            var command = (ModbusCommand)data.UserData;
            var fncode = command.FunctionCode;

            //encode the command body, if applies
            var body = new ByteArrayWriter();
            var codec = CommandCodecs[fncode];
            if (codec != null)
                codec.ServerEncode(command, body);

            //calculate length field
            var length = (command.ExceptionCode == 0)
                ? 2 + body.Length
                : 3;

            //create a writer for the outgoing data
            var writer = new ByteArrayWriter();

            //transaction-id
            writer.WriteUInt16BE((ushort)command.TransId);

            //protocol-identifier
            writer.WriteInt16BE(0);

            //message length
            writer.WriteInt16BE((short)length);

            //unit identifier (address)
            writer.WriteByte(server.Address);

            if (command.ExceptionCode == 0)
            {
                //function code
                writer.WriteByte(command.FunctionCode);

                //body data
                writer.WriteBytes(body);
            }
            else
            {
                //function code
                writer.WriteByte((byte)(command.FunctionCode | 0x80));

                //exception code
                writer.WriteByte(command.ExceptionCode);
            }

            data.OutgoingData = writer.ToReader();
        }



        CommResponse IProtocolCodec.ServerDecode(CommDataBase data)
        {
            var server = (ModbusServer)data.OwnerProtocol;
            var incoming = data.IncomingData;

            //validate header first
            if (incoming.Length < 6)
                goto LabelUnknown;

            //transaction-id
            var transId = incoming.ReadUInt16BE();

            //protocol-identifier
            var protId = incoming.ReadInt16BE();
            if (protId != 0)
                goto LabelIgnore;

            //message length
            var length = incoming.ReadInt16BE();

            if (incoming.Length < (length + 6))
                goto LabelUnknown;

            //address
            var address = incoming.ReadByte();

            if (address == server.Address)
            {
                //function code
                var fncode = incoming.ReadByte();
                if (!CommandCodecs.ContainsKey(fncode))
                {
                    return new CommResponse(null, CommResponse.Unknown);
                }

                //create a new command
                var command = new ModbusCommand(fncode);
                data.UserData = command;
                command.TransId = transId;

                //
                var body = new ByteArrayReader(incoming.ReadToEnd());

                //encode the command body, if applies
                var codec = CommandCodecs[fncode];
                if (codec != null)
                    codec.ServerDecode(command, body);

                return new CommResponse(
                    data,
                    CommResponse.Ack);
            }

            //exception
        LabelIgnore:
            return new CommResponse(
                data,
                CommResponse.Ignore);

        LabelUnknown:
            return new CommResponse(
                data,
                CommResponse.Unknown);
        }

        #endregion

    }
}
