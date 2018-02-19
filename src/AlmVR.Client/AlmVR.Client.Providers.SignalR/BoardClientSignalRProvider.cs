using AlmVR.Client.Core;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmVR.Client.Providers.SignalR
{
    public class BoardClientSignalRProvider : IBoardClient
    {
        public event EventHandler ThingHappenedToMe;

        private HubConnection connection;

        public Task ConnectAsync(string hostName, int port)
        {
            connection = new HubConnectionBuilder()
                .WithUrl($"http://{hostName}:{port}/board")
                .WithConsoleLogger()
                .Build();

            connection.On("DoThingToClients", () =>
            {
                Console.WriteLine("raising event");
                ThingHappenedToMe?.Invoke(this, new EventArgs());
            });

            return connection.StartAsync();
        }

        public async void Dispose()
        {
            await connection.DisposeAsync();
        }

        public Task<string> DoThingToServerAsync()
        {
            return connection.InvokeAsync<string>("DoThingToServer");
        }
    }
}
