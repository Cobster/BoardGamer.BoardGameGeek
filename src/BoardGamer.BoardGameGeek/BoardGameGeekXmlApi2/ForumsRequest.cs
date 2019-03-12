using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ForumsRequest
    {
        public ForumsRequest(int id, int? page = null)
        {
            this.Id = id;
            this.Page = page;
            this.RelativeUrl = new UrlBuilder()
                .Path("forum")
                .AddQueryArgument("id", Id)
                .AddQueryArgument("page", Page)
                .ToUrl();
        }

        public int Id { get; }
        public int? Page { get; }
        public Uri RelativeUrl { get; }
    }  
}