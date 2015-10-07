
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
    /// Define the interface for any protocol codec
    /// </summary>
    public interface IProtocolCodec
    {
        /// <summary>
        /// Encode the client-side outgoing data
        /// </summary>
        /// <param name="data"></param>
        void ClientEncode(CommDataBase data);

        /// <summary>
        /// Attempt to decode the client-side incoming data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CommResponse ClientDecode(CommDataBase data);


        /// <summary>
        /// Encode the server-side outgoing data
        /// </summary>
        /// <param name="data"></param>
        void ServerEncode(CommDataBase data);

        /// <summary>
        /// Attempt to decode the server-side incoming data
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        CommResponse ServerDecode(CommDataBase data);
    }
}
