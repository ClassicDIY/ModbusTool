
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
 * 09/Apr/2012
 *  Class name changed due improved abstraction (RTU support)
 **/
namespace ModbusLib.Protocols
{
    /// <summary>
    /// Modbus codec for commands: writing single discrete datum
    /// </summary>
    public class ModbusCodecWriteSingleDiscrete
        : ModbusCommandCodec
    {
        #region Client codec

        public override void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteUInt16BE((ushort)command.Offset);

            var value = command.Data[0] != 0
                ? 0xFF00
                : 0;

            body.WriteInt16BE((short)value);
        }


        public override void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            //not used
        }

        #endregion



        #region Server codec

        public override void ServerEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            ModbusCodecBase.PushRequestHeader(
                command,
                body);
        }


        public override void ServerDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
            command.Offset = body.ReadUInt16BE();
            command.Count = 1;
            command.QueryTotalLength += 2;

            command.Data = new ushort[1];
            command.Data[0] = body.ReadUInt16BE();
        }

        #endregion

    }
}
