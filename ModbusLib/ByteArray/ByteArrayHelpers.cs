using System;

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
    /// Helpers for the byte arrays
    /// </summary>
    public static class ByteArrayHelpers
    {
        /// <summary>
        /// Define the look-up table for calculating the CRC16 algorithm
        /// </summary>
        private static readonly ushort[] CrcTable = new ushort[256]
        {
            0X0000, 0XC0C1, 0XC181, 0X0140, 0XC301, 0X03C0, 0X0280, 0XC241,
            0XC601, 0X06C0, 0X0780, 0XC741, 0X0500, 0XC5C1, 0XC481, 0X0440,
            0XCC01, 0X0CC0, 0X0D80, 0XCD41, 0X0F00, 0XCFC1, 0XCE81, 0X0E40,
            0X0A00, 0XCAC1, 0XCB81, 0X0B40, 0XC901, 0X09C0, 0X0880, 0XC841,
            0XD801, 0X18C0, 0X1980, 0XD941, 0X1B00, 0XDBC1, 0XDA81, 0X1A40,
            0X1E00, 0XDEC1, 0XDF81, 0X1F40, 0XDD01, 0X1DC0, 0X1C80, 0XDC41,
            0X1400, 0XD4C1, 0XD581, 0X1540, 0XD701, 0X17C0, 0X1680, 0XD641,
            0XD201, 0X12C0, 0X1380, 0XD341, 0X1100, 0XD1C1, 0XD081, 0X1040,
            0XF001, 0X30C0, 0X3180, 0XF141, 0X3300, 0XF3C1, 0XF281, 0X3240,
            0X3600, 0XF6C1, 0XF781, 0X3740, 0XF501, 0X35C0, 0X3480, 0XF441,
            0X3C00, 0XFCC1, 0XFD81, 0X3D40, 0XFF01, 0X3FC0, 0X3E80, 0XFE41,
            0XFA01, 0X3AC0, 0X3B80, 0XFB41, 0X3900, 0XF9C1, 0XF881, 0X3840,
            0X2800, 0XE8C1, 0XE981, 0X2940, 0XEB01, 0X2BC0, 0X2A80, 0XEA41,
            0XEE01, 0X2EC0, 0X2F80, 0XEF41, 0X2D00, 0XEDC1, 0XEC81, 0X2C40,
            0XE401, 0X24C0, 0X2580, 0XE541, 0X2700, 0XE7C1, 0XE681, 0X2640,
            0X2200, 0XE2C1, 0XE381, 0X2340, 0XE101, 0X21C0, 0X2080, 0XE041,
            0XA001, 0X60C0, 0X6180, 0XA141, 0X6300, 0XA3C1, 0XA281, 0X6240,
            0X6600, 0XA6C1, 0XA781, 0X6740, 0XA501, 0X65C0, 0X6480, 0XA441,
            0X6C00, 0XACC1, 0XAD81, 0X6D40, 0XAF01, 0X6FC0, 0X6E80, 0XAE41,
            0XAA01, 0X6AC0, 0X6B80, 0XAB41, 0X6900, 0XA9C1, 0XA881, 0X6840,
            0X7800, 0XB8C1, 0XB981, 0X7940, 0XBB01, 0X7BC0, 0X7A80, 0XBA41,
            0XBE01, 0X7EC0, 0X7F80, 0XBF41, 0X7D00, 0XBDC1, 0XBC81, 0X7C40,
            0XB401, 0X74C0, 0X7580, 0XB541, 0X7700, 0XB7C1, 0XB681, 0X7640,
            0X7200, 0XB2C1, 0XB381, 0X7340, 0XB101, 0X71C0, 0X7080, 0XB041,
            0X5000, 0X90C1, 0X9181, 0X5140, 0X9301, 0X53C0, 0X5280, 0X9241,
            0X9601, 0X56C0, 0X5780, 0X9741, 0X5500, 0X95C1, 0X9481, 0X5440,
            0X9C01, 0X5CC0, 0X5D80, 0X9D41, 0X5F00, 0X9FC1, 0X9E81, 0X5E40,
            0X5A00, 0X9AC1, 0X9B81, 0X5B40, 0X9901, 0X59C0, 0X5880, 0X9841,
            0X8801, 0X48C0, 0X4980, 0X8941, 0X4B00, 0X8BC1, 0X8A81, 0X4A40,
            0X4E00, 0X8EC1, 0X8F81, 0X4F40, 0X8D01, 0X4DC0, 0X4C80, 0X8C41,
            0X4400, 0X84C1, 0X8581, 0X4540, 0X8701, 0X47C0, 0X4680, 0X8641,
            0X8201, 0X42C0, 0X4380, 0X8341, 0X4100, 0X81C1, 0X8081, 0X4040 
        };



        /// <summary>
        /// Calculate the CRC16 over a byte-array segment
        /// </summary>
        /// <param name="buffer">The source byte-array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <param name="count">The number of the bytes to be considered</param>
        /// <returns>The resulting CRC16 value</returns>
        /// <remarks>
        /// Ported from plain-C source found here: http://www.modbustools.com/modbus_crc16.htm
        /// </remarks>
        public static ushort CalcCRC16(
            byte[] buffer,
            int offset,
            int count)
        {
            ushort result = 0xFFFF;

            while (count-- > 0)
            {
                if (buffer.Length <= offset)
                    break;
                var temp = (byte)(buffer[offset++] ^ result);
                result >>= 8;
                result ^= CrcTable[temp];
            }

            return result;
        }



        /// <summary>
        /// Create a copy of the given byte array
        /// </summary>
        /// <param name="source">The source byte-array</param>
        /// <returns>The resulting copy</returns>
        public static byte[] ToArray(this byte[] source)
        {
            return source.ToArray(
                0,
                source.Length);
        }



        /// <summary>
        /// Create a new byte array starting from the defined
        /// segment of the given one
        /// </summary>
        /// <param name="source">The source byte-array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <param name="count">The number of the bytes to be considered</param>
        /// <returns>The resulting byte-array</returns>
        public static byte[] ToArray(
            this byte[] source,
            int offset,
            int count)
        {
            var target = new byte[count];
            Array.Copy(
                source,
                offset,
                target,
                0,
                count);

            return target;
        }



        /// <summary>
        /// Calculate the LRC of the given byte array
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <param name="count">The number of the bytes to be considered</param>
        /// <returns>The resulting value</returns>
        public static byte CalcLRC(
            byte[] buffer,
            int offset,
            int count)
        {
            int lrc = 0;

            for (int i = offset, upper = offset + count; i < upper; i++)
            {
                lrc ^= buffer[i];
            }

            return (byte)lrc;
        }



        /// <summary>
        /// Read an <see cref="System.Int16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int16 ReadInt16LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset+2)
                return 0;

            return (Int16)(
                buffer[offset] |
                (buffer[offset + 1] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.Int16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt16LE(
            byte[] buffer,
            int offset,
            Int16 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }



        /// <summary>
        /// Read an <see cref="System.Int16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int16 ReadInt16BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset+2)
                return 0;

            return (Int16)(
                buffer[offset + 1] |
                (buffer[offset] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.Int16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt16BE(
            byte[] buffer,
            int offset,
            Int16 value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }



        #region NON CLS-Compliant members

#pragma warning disable 3001, 3002

        /// <summary>
        /// Read an <see cref="System.UInt16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt16 ReadUInt16LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 2)
                return 0;
            return (UInt16)(
                buffer[offset] |
                (buffer[offset + 1] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.UInt16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt16LE(
            byte[] buffer,
            int offset,
            UInt16 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
        }



        /// <summary>
        /// Read an <see cref="System.UInt16"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static UInt16 ReadUInt16BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 2)
                return 0;
            return (UInt16)(
                buffer[offset + 1] |
                (buffer[offset] << 8));
        }



        /// <summary>
        /// Write an <see cref="System.UInt16"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteUInt16BE(
            byte[] buffer,
            int offset,
            UInt16 value)
        {
            buffer[offset] = (byte)(value >> 8);
            buffer[offset + 1] = (byte)value;
        }

#pragma warning restore 3001, 3002

        #endregion



        /// <summary>
        /// Read an <see cref="System.Int32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int32 ReadInt32LE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;
            return (Int32)(
                buffer[offset] |
                (buffer[offset + 1] << 8) |
                (buffer[offset + 2] << 16) |
                (buffer[offset + 3] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.Int32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Little-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt32LE(
            byte[] buffer,
            int offset,
            Int32 value)
        {
            buffer[offset] = (byte)value;
            buffer[offset + 1] = (byte)(value >> 8);
            buffer[offset + 2] = (byte)(value >> 16);
            buffer[offset + 3] = (byte)(value >> 24);
        }



        /// <summary>
        /// Read an <see cref="System.Int32"/> from the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static Int32 ReadInt32BE(
            byte[] buffer,
            int offset)
        {
            if (buffer.Length < offset + 4)
                return 0;

            return (Int32)(
                buffer[offset + 3] |
                (buffer[offset + 2] << 8) |
                (buffer[offset + 1] << 16) |
                (buffer[offset] << 24));
        }



        /// <summary>
        /// Write an <see cref="System.Int32"/> to the given byte array
        /// starting from the specified offset, 
        /// and composed as the Big-endian format
        /// </summary>
        /// <param name="buffer">The source byte array</param>
        /// <param name="offset">The index of the first byte to be considered</param>
        /// <returns>The resulting value</returns>
        public static void WriteInt32BE(
            byte[] buffer,
            int offset,
            Int32 value)
        {
            buffer[offset] = (byte)(value >> 24);
            buffer[offset + 1] = (byte)(value >> 16);
            buffer[offset + 2] = (byte)(value >> 8);
            buffer[offset + 3] = (byte)value;
        }

    }
}
