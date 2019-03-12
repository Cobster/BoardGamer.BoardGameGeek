using System;
using System.Collections.Generic;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ThreadsResponse
    {
        public ThreadsResponse(Thread thread)
        {
            Result = thread;
            Succeeded = true;
        }

        public Thread Result { get; }
        public bool Succeeded { get; }

        public class Thread
        {
            public int Id { get; set; }
            public int NumArticles { get; set; }
            public string Link { get; set; }
            public string TermsOfUse { get; set; }
            public string Subject { get; set; }
            public List<Article> Articles { get; set; }
        }

        public class Article
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Link { get; set; }
            public DateTimeOffset PostDate { get; set; }
            public DateTimeOffset EditDate { get; set; }
            public int NumEdits { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
        }
    }
}