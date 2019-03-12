using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class ForumListRequest
    {
        public ForumListRequest(int id, string type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.Id = id;
            this.Type = type;
            this.RelativeUrl = new UrlBuilder()
                .Path("forumlist")
                .AddQueryArgument("id", Id)
                .AddQueryArgument("type", Type)
                .ToUrl();
        }

        public int Id { get; }
        public string Type { get; }
        public Uri RelativeUrl { get; }
    }
}
