using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class GuildResponse
    {
        public GuildResponse(Guild guild)
        {
            this.Succeeded = true;
            this.Result = guild;
        }

        public Guild Result { get; }
        public bool Succeeded { get; }

        public class Guild
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public DateTimeOffset Created { get; set; }
            public string TermsOfUse { get; set; }
            public string Category { get; set; }
            public string Website { get; set; }
            public string Manager { get; set; }
            public string Description { get; set; }
            public Location Location { get; set; }
            public MemberCollection Members { get; set; }
        }

        public class Location
        {
            public string Addr1 { get; set; }
            public string Addr2 { get; set; }
            public string City { get; set; }
            public string StateOrProvince { get; set; }
            public string PostalCode { get; set; }
            public string Country { get; set; }
        }

        public class MemberCollection : List<Member>
        {
            public MemberCollection(IEnumerable<Member> members)
                : base(members)
            {}

            /// <summary>
            /// The total number of members in the guild.
            /// </summary>
            /// <remarks>
            /// This is mapped from the members element's count attribute.
            /// </remarks>
            public int Total { get; set; }

            /// <summary>
            /// The current page of guild members.
            /// </summary>
            /// <remarks>
            /// Each page contains 25 records.
            /// </remarks>
            public int Page { get; set; }
        }

        public class Member
        {
            public string Name { get; set; }
            public DateTimeOffset Date { get; set; }
        }
    }
}