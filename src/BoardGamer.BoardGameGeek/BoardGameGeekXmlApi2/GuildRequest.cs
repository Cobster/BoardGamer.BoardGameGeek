using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class GuildRequest
    {
        public GuildRequest(
            int id, 
            bool? members = null, 
            string sort = null, 
            int? page = null)
        {
            Id = id;
            Members = members;
            Sort = sort;
            Page = page;
            RelativeUrl = this.BuildRelativeUrl();                   
        }

        public int Id { get; }
        public bool? Members { get; }
        public string Sort { get; }
        public int? Page { get; }
        public Uri RelativeUrl { get; }

        private Uri BuildRelativeUrl()
        {
            return new UrlBuilder()
                .Path("guilds")
                .AddQueryArgument("id", Id)
                .AddQueryArgument("members", Members)
                .AddQueryArgument("sort", Sort)
                .AddQueryArgument("page", Page)
                .ToUrl();
        }
    }
}