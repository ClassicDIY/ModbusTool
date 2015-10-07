
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
    /// Client-refined derivation of the data carrier
    /// </summary>
    public class ClientCommData
        : CommDataBase
    {
        public ClientCommData(IProtocol protocol)
            : base(protocol)
        {
        }



        /// <summary>
        /// Default timeout for the current query
        /// </summary>
        public int Timeout = 1000;  //ms



        /// <summary>
        /// Number of retries for re-sending the request
        /// </summary>
        public int Retries = 3;

    }
}
