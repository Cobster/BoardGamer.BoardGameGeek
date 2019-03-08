using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class PlaysResponse
    {
        public PlaysResponse(PlaysCollection plays)
        {
            this.Result = plays;
            this.Succeeded = true;
        }

        public PlaysCollection Result { get; }
        public bool Succeeded { get; }


        public class PlaysCollection
        {
            public string Username { get; set; }
            public int UserId { get; set; }
            public int Total { get; set; }
            public int Page { get; set; }
            public string TermsOfUse { get; set; }
            public List<Play> Plays { get; set; }
        }

        public class Play
        {
            public int Id { get; set; }
            public DateTime Date { get; set; }
            public int Quantity { get; set; }
            public int Length { get; set; }
            public bool Incomplete { get; set; }
            public bool NowInStats { get; set; }
            public string Location { get; set; }
            public Item Item { get; set; }
            public string Comments { get; set; }
            public List<Player> Players { get; set; }
        }

        public class Item
        {
            public string Name { get; set; }
            public string ObjectType { get; set; }
            public int ObjectId { get; set; }
            public List<string> SubTypes { get; set; }
        }

        public class Player
        {
            public string Username { get; set; }
            public int? UserId { get; set; }
            public string Name { get; set; }
            public string StartPosition { get; set; }
            public string Color { get; set; }
            public string Score { get; set; }
            public bool New { get; set; }
            public int Rating { get; set; }
            public bool Win { get; set; }
        }
    }
}
