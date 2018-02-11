using AlmVR.Server.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Server.Providers.Trello
{
    internal class TrelloBoardProvider : IBoardProvider
    {
        public string GetInfo()
        {
            return "From Trello";
        }
    }
}
