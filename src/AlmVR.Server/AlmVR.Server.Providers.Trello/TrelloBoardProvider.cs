using AlmVR.Common.Models;
using AlmVR.Server.Core.Providers;
using AlmVR.Server.Providers.Trello.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            var apiKey = config.ApiKey;
            var boardID = config.BoardID;
            var oAuthToken = config.OAuthToken;

            var url = $"https://trello.com/1/boards/{boardID}/lists?cards=all&fields=name&key={apiKey}&token={oAuthToken}";

            var httpClient = new HttpClient();
            string json = null;
            using (var result = await httpClient.GetAsync(url))
            {
                result.EnsureSuccessStatusCode();

                json = await result.Content.ReadAsStringAsync();
            }

            var trelloSwimLanes = JsonConvert.DeserializeObject<IEnumerable<TrelloSwimLaneModel>>(json);

            return new BoardModel
            {
                ID = boardID,
                SwimLanes = (from t in trelloSwimLanes
                             select new BoardModel.SwimLaneModel
                             {
                                 ID = t.ID,
                                 Name = t.Name,
                                 Cards = (from c in t.Cards
                                          select new BoardModel.CardModel
                                          {
                                              ID = c.ID
                                          }).ToArray()
                             }).ToArray(),
            };
        }
    }
}
