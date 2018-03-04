using AlmVR.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlmVR.Client.Core
{
    public interface IBoardClient : IDisposable
    {
        event EventHandler ThingHappenedToMe;

        Task ConnectAsync(string hostName, int port);

        Task<BoardModel> GetBoardAsync();
    }
}
