using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ForumListResponse
    {
        public ForumListResponse(Forums forums)
        {
            this.Result = forums;
            this.Succeeded = true;
        }

        public Forums Result { get;  }
        public bool Succeeded { get; }

        public class Forums : List<Forum>
        {
            public Forums(IEnumerable<Forum> collection)
                : base(collection)
            {}

            public string Type { get; set; }
            public int Id { get; set; }
            public string TermsOfUse { get; set; }
        }

        public class Forum
        {
            public int Id { get; set; }
            public int GroupId { get; set; }
            public string Title { get; set; }
            public bool NoPosting { get; set; }
            public string Description { get; set; }
            public int NumThreads { get; set; }
            public int NumPosts { get; set; }
            public DateTimeOffset? LastPostDate { get; set; }
        }
    }
}