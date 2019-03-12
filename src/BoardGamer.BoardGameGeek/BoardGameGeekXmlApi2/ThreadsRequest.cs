using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ThreadsRequest
    {
        public ThreadsRequest(
            int id,
            int? minArticleId = null,
            DateTime? minArticleDate = null,
            int? count = null)
        {
            Id = id;
            MinArticleId = minArticleId;
            MinArticleDate = minArticleDate;
            Count = count;
            RelativeUrl = new UrlBuilder()
                .Path("thread")
                .AddQueryArgument("id", Id)
                .AddQueryArgument("minarticleid", MinArticleId)
                .AddQueryArgument("minarticledate", MinArticleDate, "yyyy-MM-dd HH:mm:ss")
                .AddQueryArgument("count", Count)
                .ToUrl();
        }

        public int Id { get; }
        public int? MinArticleId { get; }
        public DateTime? MinArticleDate { get; }
        public int? Count { get; }
        public Uri RelativeUrl { get; }
    }
}
