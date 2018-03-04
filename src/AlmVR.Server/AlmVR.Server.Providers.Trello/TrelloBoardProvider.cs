using AlmVR.Server.Core.Models;
using AlmVR.Server.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlmVR.Server.Providers.Trello
{
    internal class TrelloBoardProvider : IBoardProvider
    {
        private IConfigurationProvider configurationProvider;

        public TrelloBoardProvider(IConfigurationProvider configurationProvider)
        {
            this.configurationProvider = configurationProvider;
        }

        public async Task<BoardModel> GetBoardAsync()
        {
            var config = await configurationProvider.GetConfigurationAsync<TrelloConfiguration>();

            return new BoardModel
            {
                ID = config.ApiKey
            };
        }
    }
}
