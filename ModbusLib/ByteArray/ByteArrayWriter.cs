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

/**
 * 09/Apr/2012:
 *  Added the ToArray method.
 **/
namespace ModbusLib
{
    /// <summary>
    /// Wrapper around an ordinary byte array,
    /// which realizes a forward-only writer
    /// </summary>
    public class ByteArrayWriter
        : IEnumerable, IByteArray
    {
        private const int CHUNK_SIZE = 0x100;

        /// <summary>
        /// Crea an empty instance
        /// </summary>
        public ByteArrayWriter()
        {
            _buffer = new byte[CHUNK_SIZE];
            _proxy = new byte[8];
        }



        /// <summary>
        /// Create an instance using the specified content,
        /// and sealing it as immutable
        /// </summary>
        /// <param name="initial"></param>
        public ByteArrayWriter(byte[] initial)
        {
            _length = initial.Length;
            _buffer = new byte[_length];
            Array.Copy(
                initial,
                _buffer,
                _length);
        }



        private byte[] _buffer;
        private int _length;
        private readonly byte[] _proxy;



        /// <summary>
        /// Indicate the current length of the written data
        /// </summary>
        public int Length
        {
            get { return _length; }
        }



        /// <summary>
        /// Provide a way to reset the writer
        /// </summary>
        /// <remarks>
        /// This functionaly is not allowed when the writer has been sealed
        /// </remarks>
        public void Reset()
        {
            CheckImmutable();
            _length = 0;
        }



        /// <summary>
        /// Create a <see cref="ByteArrayReader"/> using the current writer as source
        /// </summary>
        /// <returns></returns>
        public ByteArrayReader ToReader()
        {
            return new ByteArrayReader(((IByteArray)this).Data);
        }



        /// <summary>
        /// Create a byte-array using the current writer as source
        /// </summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            return ((IByteArray)this).Data;
        }



        /// <summary>
        /// Add a byte to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteByte(byte value)
        {
            Allocate(1);
            _buffer[_length++] = value;
        }


        /// <summary>
        /// Add a series of bytes to the writer, from a byte array
        /// by specifying the starting index and the number involved
        /// </summary>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteBytes(
            byte[] values,
            int offset,
            int count)
        {
            Allocate(count);

            Array.Copy(
                values,
                offset,
                _buffer,
                _length,
                count);

            _length += count;
        }



        /// <summary>
        /// Add a series of bytes to the writer
        /// </summary>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteBytes(byte[] values)
        {
            WriteBytes(
                values,
                0,
                values.Length);
        }



        /// <summary>
        /// Add the content of the given reader to the writer
        /// </summary>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteBytes(ByteArrayReader reader)
        {
            WriteBytes(((IByteArray)reader).Data);
        }



        /// <summary>
        /// Add the content of another writer to the writer
        /// </summary>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteBytes(ByteArrayWriter writer)
        {
            WriteBytes(((IByteArray)writer).Data);
        }



        /// <summary>
        /// Add an <see cref="System.Int16"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteInt16LE(Int16 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteInt16LE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                2);
        }



        /// <summary>
        /// Add an <see cref="System.Int16"/> (Big-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteInt16BE(Int16 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteInt16BE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                2);
        }



        #region NON CLS-Compliant members

#pragma warning disable 3001, 3002, 0618

        /// <summary>
        /// Add an <see cref="System.UInt16"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteUInt16LE(UInt16 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteUInt16LE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                2);
        }



        /// <summary>
        /// Add an <see cref="System.UInt16"/> (Big-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteUInt16BE(UInt16 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteUInt16BE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                2);
        }

#pragma warning restore 3001, 3002, 0618

        #endregion



        /// <summary>
        /// Add an <see cref="System.Int32"/> (Little-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteInt32LE(Int32 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteInt32LE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                4);
        }



        /// <summary>
        /// Add an <see cref="System.Int32"/> (Big-endian) to the writer
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// This functionality is not allowed when the writer has been sealed
        /// </remarks>
        public void WriteInt32BE(Int32 value)
        {
            CheckImmutable();
            ByteArrayHelpers.WriteInt32BE(
                _proxy,
                0,
                value);

            WriteBytes(
                _proxy,
                0,
                4);
        }



        /// <summary>
        /// Allocate the specified number of bytes in the underlying
        /// buffer. Get the buffer longer when needed
        /// </summary>
        /// <param name="count"></param>
        private void Allocate(int count)
        {
            CheckImmutable();

            var size = _buffer.Length;
            var newLen = _length + count;
            if (newLen < size)
                return;

            do
            {
                size += CHUNK_SIZE;
            } while (size < newLen);

            var temp = new byte[size];

            Array.Copy(
                _buffer,
                temp,
                _buffer.Length);

            _buffer = temp;
        }



        private void CheckImmutable()
        {
            //if the instance has been created as immutable,
            //can't allow any modification
            if (_proxy == null)
                throw new Exception();
        }



        #region IEnumerable members

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0; i < _length; i++)
                yield return _buffer[i];
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
            get 
            {
                var temp = new byte[_length];
                Array.Copy(
                    _buffer,
                    temp,
                    _length);

                return temp; 
            }
        }
    }
}
