/*

Copyright 2014 Daniel Kinsman.

This file is part of Stand Alone Map View.

Stand Alone Map View is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Stand Alone Map View is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Stand Alone Map View.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StandAloneMapView.client
{
    public class TcpWorker
    {
        protected IPEndPoint serverEndPoint;
        public TcpClient Client;
        protected bool runWorker = true;
        public string savePath;
        public ManualResetEvent SaveReceived;
        public ManualResetEvent AtLeastOneSaveReceived;

        protected object _saveFileLock = new object();
        public object SaveFileLock { get { return _saveFileLock; } }

        protected static volatile TcpWorker instance = null;
        public static TcpWorker Instance
        {
            get
            {
                if(instance == null)
                    instance = new TcpWorker(Startup.SavePath);

                return instance;
            }
        }

        protected TcpWorker(string savePath)
        {
            this.savePath = savePath;
            this.SaveReceived = new ManualResetEvent(false);
            this.AtLeastOneSaveReceived = new ManualResetEvent(false);
        }

        public void Start()
        {
            this.Stop();

            var settings = Settings.Load();
            IPAddress serverAddress = Dns.GetHostAddresses(settings.Server)[0];
            this.serverEndPoint = new IPEndPoint(serverAddress, settings.ServerPort);

            this.runWorker = true;
            new Thread(Worker).Start();
        }

        public void Stop()
        {
            this.runWorker = false;
            if(this.Client != null)
                this.Client.Close();

            this.AtLeastOneSaveReceived.Reset();
            this.SaveReceived.Reset();
        }

        public void Worker()
        {
            while(this.runWorker)
            {
                try
                {
                    using(this.Client = new TcpClient())
                    {
                        this.Client.Connect(this.serverEndPoint);
                    
                        while(this.runWorker && this.Client.Connected)
                        {
                            var stream = this.Client.GetStream();
                            var messageType = (comms.TcpMessage)stream.ReadByte();
                            if(messageType == comms.TcpMessage.ConnectionTest)
                                continue;

                            if(messageType != comms.TcpMessage.SaveUpdate)
                                throw new IOException("Unknown message type {0}", (byte)messageType);

                            comms.Save.ReadAndSave(stream, this.savePath, this.SaveFileLock);
                            this.AtLeastOneSaveReceived.Set();
                            this.SaveReceived.Set();
                        }

                        this.Client.Close();
                    }
                }
                catch(Exception e)
                {
                    // todo log
                    if(!(e is System.IO.IOException || e is SocketException))
                        throw;
                }

                Thread.Sleep(500);
            }
        }
    }
}