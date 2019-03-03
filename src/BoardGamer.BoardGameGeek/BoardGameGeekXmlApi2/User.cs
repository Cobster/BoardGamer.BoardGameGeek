using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarLink { get; set; }
        public string YearRegistered { get; set; }
        public string LastLogin { get; set; }
        public string StateOrProvince { get; set; }
        public string Country { get; set; }
        public string WebAddress { get; set; }
        public string XboxAccount { get; set; }
        public string WiiAccount { get; set; }
        public string PsnAccount { get; set; }
        public string BattleNetAccount { get; set; }
        public string SteamAccount { get; set; }
        public string TradeRating { get; set; }
        public string MarketRating { get; set; }
        public List<Buddy> Buddies { get; set; }
        public List<Guild> Guilds { get; set; }
        public List<Item> Top { get; set; }
        public List<Item> Hot { get; set; }
    }

    public class Buddy
    {
        public string Id { get; set; }

        public string Name { get; set; }
    }

    public class Guild
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Item
    {
        public string Rank { get; set; }
        public string Type { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
