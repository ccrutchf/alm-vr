using AlmVR.Client.Core;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmVR.Client.Providers.SignalR
{
    public abstract class ClientBase : IClient
    {
        protected HubConnection Connection { get; private set; }
        protected string Path { get; private set; }

        protected ClientBase(string path)
        {
            this.Path = path;
        }

        public Task ConnectAsync(string hostName, int port)
        {
            Connection = new HubConnectionBuilder()
                .WithUrl($"http://{hostName}:{port}/{Path}")
                .WithTransport(TransportType.LongPolling) // Hack because Unity does not like Websockets.
                .WithConsoleLogger()
                .Build();

            OnConnectionCreated();

            return Connection.StartAsync();
        }

        protected virtual void OnConnectionCreated() { }

        public virtual async void Dispose()
        {
            await Connection.DisposeAsync();
        }
    }
}
