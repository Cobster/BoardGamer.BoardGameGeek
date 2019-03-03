using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class UserRequest
    {
        public UserRequest(
            string name, 
            bool buddies = false, 
            bool guilds = false,
            bool hot = false,
            bool top = false,
            string domain = null,
            int page = 1)
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

            Url = BuildRelativeUrl();
        }

        public string Name { get; }
        public bool Buddies { get; }
        public bool Guilds { get; }
        public bool Hot { get; }
        public bool Top { get; }
        public string Domain { get; }
        public int Page { get; }
        public Uri Url { get; }

        private Uri BuildRelativeUrl()
        {
            List<string> args = new List<string>();

            args.Add($"name={Name}");

            if (Buddies) args.Add("buddies=1");
            if (Guilds) args.Add("guilds=1");
            if (Hot) args.Add("hot=1");
            if (Top) args.Add("top=1");
            if (!String.IsNullOrWhiteSpace(Domain)) args.Add($"domain={Domain}");
            if (Page > 1) args.Add($"page={Page}");

            string queryArgs = String.Join("&", args);

            return new Uri($"user?{queryArgs}", UriKind.Relative);
        }
    }
}
