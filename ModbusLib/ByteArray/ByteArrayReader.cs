using System;
using System.Collections;

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
    /// Wrapper around a byte array for a forward-only reading access
    /// </summary>
    public class ByteArrayReader
        : IEnumerable, IByteArray
    {
        /// <summary>
        /// Create the instance from a byte array source
        /// </summary>
        /// <param name="source"></param>
        public ByteArrayReader(byte[] source)
        {
            _buffer = source.ToArray();
            Length = _buffer.Length;
            Reset();
        }



        /// <summary>
        /// Create the instance from the specified part of the byte array source
        /// </summary>
        /// <param name="source"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public ByteArrayReader(
            byte[] source,
            int offset,
            int count)
        {
            _buffer = source
                .ToArray(offset, count);
            Length = count;
            Reset();
        }



        private readonly byte[] _buffer;

        public int Position { get; private set; }
        public int Length { get; private set; }



        /// <summary>
        /// Allow to reset the reader pointer
        /// </summary>
        public void Reset()
        {
            Position = -1;
        }



        /// <summary>
        /// Convert the reader content to a normal byte array
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return _buffer
                .ToArray();
        }



        /// <summary>
        /// Indicate whether the pointer has reached the end of the internal buffer
        /// </summary>
        public bool EndOfBuffer
        {
            get { return Position >= Length - 1; }
        }



        /// <summary>
        /// Tell whether the specified amount of bytes could be read from the reader
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool CanRead(int count)
        {
            int len = Length - Position - 1;
            return (count <= len);
        }



        /// <summary>
        /// Read the current pointed byte, but without moving the pointer
        /// </summary>
        /// <returns></returns>
        public byte Peek()
        {
            return _buffer[Position];
        }



        /// <summary>
        /// Read the current byte and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public byte ReadByte()
        {
            return _buffer[++Position];
        }



        /// <summary>
        /// Try to read a byte, if available, and return true if succeeded
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryReadByte(out byte value)
        {
            if (CanRead(1))
            {
                value = ReadByte();
                return true;
            }
            else
            {
                value = 0;
                return false;
            }
        }



        /// <summary>
        /// Read the specified amount of bytes, 
        /// and move the pointer accordingly
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public byte[] ReadBytes(int count)
        {
            var collection = new byte[count];

            for (int i = 0; i < count; i++)
            {
                collection[i] = _buffer[++Position];
            }

            return collection;
        }



        /// <summary>
        /// Read the remaining bytes up to the end, 
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public byte[] ReadToEnd()
        {
            return ReadBytes(Length - (Position + 1));
        }



        /// <summary>
        /// Read an <see cref="System.Int16"/> (Little-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public Int16 ReadInt16LE()
        {
            int ptr = Position + 1;
            Position += 2;
            return ByteArrayHelpers.ReadInt16LE(
                _buffer,
                ptr);
        }



        /// <summary>
        /// Read an <see cref="System.Int16"/> (Big-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public Int16 ReadInt16BE()
        {
            int ptr = Position + 1;
            Position += 2;
            return ByteArrayHelpers.ReadInt16BE(
                _buffer,
                ptr);
        }



        #region NON CLS-Compliant members

#pragma warning disable 3001, 3002

        /// <summary>
        /// Read an <see cref="System.UInt16"/> (Little-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public UInt16 ReadUInt16LE()
        {
            int ptr = Position + 1;
            Position += 2;
            return ByteArrayHelpers.ReadUInt16LE(
                _buffer,
                ptr);
        }



        /// <summary>
        /// Read an <see cref="System.UInt16"/> (Big-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public UInt16 ReadUInt16BE()
        {
            int ptr = Position + 1;
            Position += 2;
            return ByteArrayHelpers.ReadUInt16BE(
                _buffer,
                ptr);
        }

#pragma warning restore 3001, 3002

        #endregion



        /// <summary>
        /// Read an <see cref="System.Int32"/> (Little-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public Int32 ReadInt32LE()
        {
            int ptr = Position + 1;
            Position += 4;
            return ByteArrayHelpers.ReadInt32LE(
                _buffer,
                ptr);
        }



        /// <summary>
        /// Read an <see cref="System.Int32"/> (Big-endian),
        /// and move the pointer accordingly
        /// </summary>
        /// <returns></returns>
        public Int32 ReadInt32BE()
        {
            int ptr = Position + 1;
            Position += 4;
            return ByteArrayHelpers.ReadInt32BE(
                _buffer,
                ptr);
        }



        #region IEnumerable members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _buffer
                .GetEnumerator();
        }

        #endregion



        /// <summary>
        /// Facility for data exchange
        /// </summary>
        /// <remarks>
        /// Avoid to use this member unless strictly necessary
        /// </remarks>
        byte[] IByteArray.Data
        {
            get { return _buffer; }
        }

    }
}
