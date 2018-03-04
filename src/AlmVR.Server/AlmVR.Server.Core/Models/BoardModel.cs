using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Server.Core.Models
{
    public class BoardModel
    {
        public class SwimLaneModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public IEnumerable<CardModel> Cards { get; set; }
        }

        public class CardModel
        {
            public string ID { get; set; }
        }

        public string ID { get; set; }
        public IEnumerable<SwimLaneModel> SwimLanes { get; set; }
    }
}
