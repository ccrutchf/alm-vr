using System;
using System.Collections.Generic;
using System.Text;

namespace AlmVR.Server.Core.Models
{
    public class BoardModel
    {
        public class CardModel
        {
            public string ID { get; set; }
        }

        public string ID { get; set; }
        public IEnumerable<CardModel> Cards { get; set; }
    }
}
