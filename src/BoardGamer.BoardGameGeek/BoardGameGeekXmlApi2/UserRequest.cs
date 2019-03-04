using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class UserRequest
    {
        public UserRequest(
            string name, 
            bool? buddies = null, 
            bool? guilds = null,
            bool? hot = null,
            bool? top = null,
            string domain = null,
            int? page = null)
        {
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException(nameof(name));
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), page, "Must be greater than or equal to 1.");
            if (!String.IsNullOrWhiteSpace(domain))
            {
                string domainClean = domain.ToLower().Trim();

                if (!(domainClean == "boardgame" || domainClean == "rpg" || domainClean == "videogame"))
                {
                    throw new ArgumentOutOfRangeException(nameof(domain), domain, "Must be \"boardgame\", \"rpg\", or \"videogame\".");
                }

                this.Domain = domainClean;
            }

            Name = name;
            Buddies = buddies;
            Guilds = guilds;
            Hot = hot;
            Top = top;
            Page = page;

            RelativeUrl = BuildRelativeUrl();
        }

        public string Name { get; }
        public bool? Buddies { get; }
        public bool? Guilds { get; }
        public bool? Hot { get; }
        public bool? Top { get; }
        public string Domain { get; }
        public int? Page { get; }
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            UrlBuilder builder = new UrlBuilder()
                .Path("user")
                .AddQueryArgument("name", Name)
                .AddQueryArgument("buddies", Buddies)
                .AddQueryArgument("guilds", Guilds)
                .AddQueryArgument("hot", Hot)
                .AddQueryArgument("top", Top)
                .AddQueryArgument("domain", Domain)
                .AddQueryArgument("page", Page);

            return builder.ToUrl();
        }
    }
}
