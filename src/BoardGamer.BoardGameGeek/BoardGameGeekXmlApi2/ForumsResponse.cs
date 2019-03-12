using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ForumsResponse
    {
        public ForumsResponse(Forum forum)
        {
            this.Result = forum;
            this.Succeeded = true;
        }

        public Forum Result { get; }
        public bool Succeeded { get; }

        public class Forum
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public int NumThreads { get; set; }
            public int NumPosts { get; set; }
            public DateTimeOffset? LastPostDate { get; set; }
            public bool NoPosting { get; set; }
            public string TermsOfUse { get; set; }
            public List<Thread> Threads { get; set; }
        }

        public class Thread
        {
            public int Id { get; set; }
            public string Subject { get; set; }
            public string Author { get; set; }
            public int NumArticles { get; set; }
            public DateTimeOffset PostDate { get; set; }
            public DateTimeOffset LastPostDate { get; set; }
        }
    }
}