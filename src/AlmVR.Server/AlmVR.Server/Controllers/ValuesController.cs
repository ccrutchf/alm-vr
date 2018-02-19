using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlmVR.Server.Core.Providers;
using AlmVR.Server.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AlmVR.Server.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private IBoardProvider boardProvider;
        private IHubContext<BoardHub> boardHubContext;

        public ValuesController(IBoardProvider boardProvider, IHubContext<BoardHub> boardHubContext)
        {
            this.boardProvider = boardProvider;
            this.boardHubContext = boardHubContext;
        }

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> GetAsync()
        {
            await boardHubContext.Clients.All.InvokeAsync("DoThingToClients");

            return new string[] { "value1", "value2", boardProvider.GetInfo() };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
