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
    public interface ICommServer
    {
        /// <summary>
        /// Start the server listener session
        /// </summary>
        void Start();

        /// <summary>
        /// Close/abort the opened listener session
        /// </summary>
        void Abort();

        /// <summary>
        /// Provide a callback to the host application
        /// for serving the received request
        /// </summary>
        event ServeCommandHandler ServeCommand;
    }



    /// <summary>
    /// Delegate for the "ServeCommand" event
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ServeCommandHandler(object sender, ServeCommandEventArgs e);



    /// <summary>
    /// Arguments used in the "ServeCommand" event
    /// </summary>
    public class ServeCommandEventArgs
        : EventArgs
    {
        public ServeCommandEventArgs(ServerCommData data)
        {
            Data = data;
        }



        public ServerCommData Data { get; private set; }
    }
}
