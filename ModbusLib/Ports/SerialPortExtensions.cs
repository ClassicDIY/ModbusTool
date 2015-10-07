using System.IO.Ports;

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
    public static class SerialPortExtensions
    {
        /// <summary>
        /// Return a concrete implementation of a serial port client
        /// </summary>
        /// <param name="port"></param>
        /// <param name="setting"></param>
        /// <returns></returns>
        public static ICommClient GetClient(this SerialPort port)
        {
            return new SerialPortClient(port);
        }



        /// <summary>
        /// Return a concrete implementation of a serial port listener
        /// </summary>
        /// <param name="port"></param>
        /// <param name="protocol"></param>
        /// <returns></returns>
        public static ICommServer GetListener(
            this 
            SerialPort port,
            IProtocol protocol)
        {
            return new SerialPortServer(
                port,
                protocol);
        }

    }
}
