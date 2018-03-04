using AlmVR.Client.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmVR.Client.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IP Address:");
            string ipaddress = Console.ReadLine();

            Console.WriteLine("Port:");
            string portString = Console.ReadLine();
            int port = int.Parse(portString);

            Console.WriteLine($"Connecting to http://{ipaddress}:{port}...");

            var boardClient = ClientFactory.GetInstance<IBoardClient>();
            boardClient.ConnectAsync(ipaddress, port).Wait();
            Console.WriteLine(boardClient.GetBoardAsync().Result.ID);

            boardClient.ThingHappenedToMe += BoardClient_ThingHappenedToMe;

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey(false);
        }

        private static void BoardClient_ThingHappenedToMe(object sender, EventArgs e)
        {
            Console.WriteLine("stop");
        }
    }
}
