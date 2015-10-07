
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

/**
 * 09/Apr/2012: 
 *  Added an optional parameter to the constructor, so that the
 *  IncomingData reader can be initialized. This allows to
 *  create transport driver outside the ModbusLib
 *  assembly (the IncomingData property set is internal).
 *  Thanks to: http://www.codeplex.com/site/users/view/gralinpl
 **/
namespace ModbusLib
{
    /// <summary>
    /// Server-refined derivation of the data carrier
    /// </summary>
    public class ServerCommData
        : CommDataBase
    {
        public ServerCommData(
            IProtocol protocol, 
            ByteArrayReader incomingData = null)
            : base(protocol)
        {
            IncomingData = incomingData;
        }
    }
}
