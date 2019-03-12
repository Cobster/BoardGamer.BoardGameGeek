using System;
using System.Collections.Generic;
using System.Text;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class UserResponse
    {
        public UserResponse(User user)
        {
            this.Result = user;
            this.Succeeded = true;
        }

        public User Result { get; }
        public bool Succeeded { get; }

        public class User
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string TermsOfUse { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string AvatarLink { get; set; }
            public int YearRegistered { get; set; }
            public DateTime LastLogin { get; set; }
            public string StateOrProvince { get; set; }
            public string Country { get; set; }
            public string WebAddress { get; set; }
            public string XboxAccount { get; set; }
            public string WiiAccount { get; set; }
            public string PsnAccount { get; set; }
            public string BattleNetAccount { get; set; }
            public string SteamAccount { get; set; }
            public int TradeRating { get; set; }
            public int MarketRating { get; set; }
            public List<Buddy> Buddies { get; set; }
            public List<Guild> Guilds { get; set; }
            public List<Item> Top { get; set; }
            public List<Item> Hot { get; set; }
        }

        public class Buddy
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class Guild
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Item
        {
            public int Rank { get; set; }
            public string Type { get; set; }
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
