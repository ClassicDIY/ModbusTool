
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
 * 
 * 09/Apr/2012:
 *  Fixed the wrong accessor for the CommandCodecs array,
 *  which has to be "public" instead of "private"
 * 
 * 20/Apr/2012:
 *  Added the "force multiple coils" codec handler
 **/

using System.Collections.Generic;
using System.Data.Odbc;

namespace ModbusLib.Protocols
{
    public class ModbusCodecBase
    {
        static ModbusCodecBase()
        {
            //fill the local array with the curretly supported commands
            CommandCodecs[ModbusCommand.FuncReadMultipleRegisters] = new ModbusCodecReadMultipleRegisters();
            CommandCodecs[ModbusCommand.FuncWriteMultipleRegisters] = new ModbusCodecWriteMultipleRegisters();
            CommandCodecs[ModbusCommand.FuncReadCoils] = new ModbusCodecReadMultipleDiscretes();
            CommandCodecs[ModbusCommand.FuncReadInputDiscretes] = new ModbusCodecReadMultipleDiscretes();
            CommandCodecs[ModbusCommand.FuncReadInputRegisters] = new ModbusCodecReadMultipleRegisters();
            CommandCodecs[ModbusCommand.FuncWriteCoil] = new ModbusCodecWriteSingleDiscrete();
            CommandCodecs[ModbusCommand.FuncWriteSingleRegister] = new ModbusCodecWriteSingleRegister();
            CommandCodecs[ModbusCommand.FuncForceMultipleCoils] = new ModbusCodecForceMultipleCoils();

            CommandCodecs[ModbusCommand.FuncReadCustom] = new ModbusCodecReadCustom();
        }


        public static readonly Dictionary<byte, ModbusCommandCodec> CommandCodecs = new Dictionary<byte, ModbusCommandCodec>(9);
        //public static readonly ModbusCommandCodec[] CommandCodecs = new ModbusCommandCodec[36];



        /// <summary>
        /// Append the typical header for a request command (master-side)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PushRequestHeader(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            body.WriteUInt16BE((ushort)command.Offset);
            if (command.FunctionCode == 05 || command.FunctionCode == 06)
            {
                body.WriteInt16BE((short)command.Data[0]);
            }
            else
            {
                body.WriteInt16BE((short)command.Count);
            }
        }



        /// <summary>
        /// Extract the typical header for a request command (server-side)
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PopRequestHeader(
            ModbusCommand command,
            ByteArrayReader body)
        {
            command.Offset = body.ReadUInt16BE();
            command.Count = body.ReadInt16BE();
        }



        /// <summary>
        /// Helper for packing the discrete data outgoing as a bit-array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PushDiscretes(
            ModbusCommand command,
            ByteArrayWriter body)
        {
            var count = ((byte)((command.Count + 7) / 8));
            var wholeWords = command.Count / 16;
            var remainingBits = command.Count % 16;
            body.WriteByte(count);
            int k;
            for (k = 0; k < wholeWords; k++)
            {
                var hb = (byte)(command.Data[k] >> 8);
                var lb = (byte)(command.Data[k] & 0x00FF);
                body.WriteByte(hb);
                body.WriteByte(lb);
            }
            if (remainingBits > 0)
            {
                byte bitMask = 1;
                byte cell = 0;
                byte currentByte = (byte)(command.Data[k] >> 8);
                for (int j = 0; j < remainingBits; j++)
                {
                    if (j == 8)
                    {
                        body.WriteByte(cell);
                        currentByte = (byte)(command.Data[k] & 0x00FF);
                        bitMask = 1;
                        cell = 0;
                    }
                    cell |= (byte)(currentByte & bitMask);
                    bitMask = (byte)(bitMask << 1);
                }
                body.WriteByte(cell);
            }
        }

        /// <summary>
        /// Helper for unpacking discrete data incoming as a bit-array
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        internal static void PopDiscretes(
            ModbusCommand command,
            ByteArrayReader body)
        {
            var byteCount = body.ReadByte();

            var count = command.Count;
            command.Data = new ushort[count];
            command.QueryTotalLength += (byteCount + 1);

            int k = 0;
            while (body.EndOfBuffer == false)
            {
                if (command.Count <= k)
                    break;
                byte hb = body.CanRead(1) ? body.ReadByte() : (byte)0;
                byte lb = body.CanRead(1) ? body.ReadByte() : (byte)0;
                command.Data[k++] = (ushort)((hb << 8) | lb);
                //int n = count <= 8 ? count : 8;
                //count -= n;
                //for (int i = 0; i < n; i++)
                //    command.Data[k++] = (ushort)(cell & (1 << i));
            }
        }

    }
}
