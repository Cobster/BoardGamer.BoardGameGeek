using System;

namespace BoardGamer.BoardGameGeek.BoardGameGeekXmlApi2
{
    public class HotItemsRequest
    {
        public HotItemsRequest(string type = null)
        {
            Type = type;
            RelativeUrl = new UrlBuilder()
                .Path("hot")
                .AddQueryArgument("type", Type)
                .ToUrl();
        }

        public string Type { get; }
        public Uri RelativeUrl { get; }
    }
}