
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
namespace ModbusLib.Protocols
{
    /// <summary>
    /// Provide the abstraction for any Modbus command codec
    /// </summary>
    public class ModbusCommandCodec
    {
        #region Client codec

        /// <summary>
        /// Encode the client-side command toward the remote slave device
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        public virtual void ClientEncode(
            ModbusCommand command,
            ByteArrayWriter body)
        {
        }

        /// <summary>
        /// Decode the incoming data from the remote slave device 
        /// to a client-side command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        public virtual void ClientDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
        }

        #endregion



        #region Server codec

        /// <summary>
        /// Encode the server-side command toward the master remote device
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        public virtual void ServerEncode(
        ModbusCommand command,
        ByteArrayWriter body)
        {
        }

        /// <summary>
        /// Decode the incoming data from the remote master device 
        /// to a server-side command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="body"></param>
        public virtual void ServerDecode(
            ModbusCommand command,
            ByteArrayReader body)
        {
        }

        #endregion
    }
}
